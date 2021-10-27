using JobSim.Characters;
using Sandbox;
using System;
using System.Linq;

namespace JobSim.Job
{
	public class JobManager
	{
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static JobManager Instance;
		public static BaseJob NullJob;
#pragma warning restore CA2211 // Non-constant fields should not be visible
		public static int NullJobID => NullJob.ID;

		public JobManager()
		{
			if ( Instance != null )
				throw new Exception( "An instance of JobManager already exists?" );

			Instance = this;
			NullJob = new NullJob();
		}

		public static BaseJob GetJob( string jobName )
		{
			BaseJob job = null;
			for ( int i = 0; i < BaseJob.All.Count; i++ )
			{
				if ( BaseJob.All[i].Name != jobName )
					continue;

				job = BaseJob.All[i];
				break;
			}

			return job;
		}

		public static int GetStage( IEmployableCharacter character ) => character.JobStage;

		public static void GiveJob( IEmployableCharacter character, string jobName )
		{
			Realm.Assert( RealmType.Server );

			BaseJob job = GetJob( jobName );
			if ( job == null )
			{
				Realm.Log.Error( $"No job with the name {jobName} exists?" );
				return;
			}

			if ( character.Job != NullJob )
				character.Job.FailStage( character );

			job.Start( character );
		}

#pragma warning disable IDE0051 // Remove unused private members
		[Event.Tick]
		private static void Tick()
		{
			foreach ( IEmployableCharacter character in IEmployableCharacter.All )
				character.Job.Tick( character );
		}
#pragma warning restore IDE0051 // Remove unused private members

		public static void RequestJob( string job )
		{
			Realm.Assert( RealmType.Client );

			ServerRequestJob( job );
		}

		[ServerCmd]
		public static void ServerRequestJob( string job )
		{
			Realm.Assert( RealmType.Server );
			if ( ConsoleSystem.Caller == null )
				return;

			GiveJob( (ConsoleSystem.Caller.Pawn as JobSimPlayer).Character, job );
		}

#pragma warning disable IDE0051 // Remove unused private members
		[Event( "net_" + JobEvent.JobStartedEvent )]
		private static void JobStartedClient( EventArgs _ )
		{
			JobSimLocal.Character.Job.OnStarted( JobSimLocal.Character );
		}
		[Event( "net_" + JobEvent.JobCompletedEvent )]
		private static void JobCompletedClient( EventArgs _ )
		{
			JobSimLocal.Character.Job.OnCompleted( JobSimLocal.Character );
		}
		[Event( "net_" + JobEvent.JobFailedEvent )]
		private static void JobFailedClient( EventArgs _ )
		{
			JobSimLocal.Character.Job.OnFailed( JobSimLocal.Character );
		}
		[Event( "net_" + JobEvent.JobStageCompletedEvent )]
		private static void JobStageCompletedClient( EventArgs _ )
		{
			JobSimLocal.Character.Job.OnStageCompleted( JobSimLocal.Character );
		}
#pragma warning restore IDE0051 // Remove unused private members
	}
}
