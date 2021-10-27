using JobSim.Characters;
using JobSim.Job;
using JobSim.UI;
using Sandbox;
using System.Collections.Generic;

namespace JobSim.Job
{
	public abstract class BaseJob
	{
		public static IReadOnlyList<BaseJob> All => _all;
		private static readonly List<BaseJob> _all = new();

		public virtual string Name => "Job";
		public virtual string Description => "None provided.";
		public virtual int NumStages => 1;
		public virtual IReward Reward => null;
		public virtual string UIElementClass => "ui_job";

		public int ID { get; set; }
		public JobUIElement UIElement { get; set; } = null;

		public BaseJob()
		{
			ID = _all.Count;

			if ( !_all.Contains( this ) )
				_all.Add( this );
		}

		public virtual void Setup( IEmployableCharacter character )
		{
			character.JobID = ID;
			character.JobStage = 0;
		}

		protected virtual void Cleanup( IEmployableCharacter character )
		{
			character.JobID = JobManager.NullJobID;
			character.JobStage = 0;
		}

		protected static int GetStage( IEmployableCharacter character ) => JobManager.GetStage( character );

		public void Start( IEmployableCharacter character )
		{
			Setup( character );
			OnStarted( character );
		}

		public void CompleteStage( IEmployableCharacter character )
		{
			OnStageCompleted( character );

			int newStage = character.JobStage + 1;
			if ( newStage >= NumStages )
			{
				Reward.GiveReward( character );
				OnCompleted( character );
				Cleanup( character );
				return;
			}

			character.JobStage = newStage;
		}

		public void FailStage( IEmployableCharacter character )
		{
			OnFailed( character );
			Cleanup( character );
		}

		public virtual void OnStarted( IEmployableCharacter character )
		{
			if ( Realm.IsServer && character.IsClientOwned )
				NetEvent.Instance.SendEvent( To.Single( character.Entity ), JobEvent.JobStartedEvent, false );

			EventRunner.Run( JobEvent.JobStartedEvent, new JobEventArgs( character ) );
		}
		public virtual void OnCompleted( IEmployableCharacter character )
		{
			if ( Realm.IsServer && character.IsClientOwned )
				NetEvent.Instance.SendEvent( To.Single( character.Entity ), JobEvent.JobCompletedEvent, false );

			EventRunner.Run( JobEvent.JobCompletedEvent, new JobEventArgs( character ) );
		}
		public virtual void OnFailed( IEmployableCharacter character )
		{
			if ( Realm.IsServer && character.IsClientOwned )
				NetEvent.Instance.SendEvent( To.Single( character.Entity ), JobEvent.JobFailedEvent, false );

			EventRunner.Run( JobEvent.JobFailedEvent, new JobEventArgs( character ) );
		}
		public virtual void OnStageCompleted( IEmployableCharacter character )
		{
			if ( Realm.IsServer && character.IsClientOwned )
				NetEvent.Instance.SendEvent( To.Single( character.Entity ), JobEvent.JobStageCompletedEvent, false );

			EventRunner.Run( JobEvent.JobStageCompletedEvent, new JobEventArgs( character ) );
		}

		public abstract void Tick( IEmployableCharacter character );
	}

	public enum JobEventType : byte
	{
		Started,
		StageCompleted,
		Completed,
		Failed
	}
}

namespace JobSim
{
	public partial class NetEvent
	{
		public void SendEvent( string eventName, bool _ ) => SendEvent( To.Everyone, eventName, _ );
		public void SendEvent( To to, string eventName, bool _ )
		{
			Realm.Assert( RealmType.Server );
			ReceiveEventRpc( to, eventName, _ );
		}

		[ClientRpc]
		private void ReceiveEventRpc( string eventName, bool _ )
		{
			EventRunner.Run( $"net_{eventName}", new JobEventArgs( (Local.Pawn as JobSimPlayer).Character ) );
		}
	}
}
