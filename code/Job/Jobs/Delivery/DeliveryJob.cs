using JobSim.Characters;
using JobSim.Money;
using Sandbox;

namespace JobSim.Job
{
	public partial class DeliveryJob : BaseJob
	{
		public override string Name { get; }
		public override string Description => "Simple as can be, deliver packages from point A to B";
		public override int NumStages => DeliveryPositions.Length;
		public override IReward Reward { get; }
		public override string UIElementClass => "ui_deliveryjob";
		public Vector3[] DeliveryPositions { get; }
		public float WithinThreshold { get; }

		public DeliveryJob( float withinThreshold, params Vector3[] endPositions )
		{
			WithinThreshold = withinThreshold;
			DeliveryPositions = endPositions;
			Reward = new MoneyReward( 100 * NumStages );
			Name = endPositions.Length > 1 ? "Deliver some packages" : "Deliver a package";
		}

		public override void OnStarted( IEmployableCharacter character )
		{
			base.OnStarted( character );

			if ( Realm.IsServer )
				(character.Entity as JobSimPlayer).Inventory.TryGiveItem( "Package", NumStages );
		}

		public override void OnStageCompleted( IEmployableCharacter character )
		{
			base.OnStageCompleted( character );

			if ( Realm.IsServer )
				(character.Entity as JobSimPlayer).Inventory.TryTakeItem( "Package", 1 );
		}

		public override void OnFailed( IEmployableCharacter character )
		{
			base.OnFailed( character );

			if ( Realm.IsServer )
				(character.Entity as JobSimPlayer).Inventory.TryTakeItem( "Package", NumStages - GetStage( character ) );
		}

		public override void Tick( IEmployableCharacter character )
		{
			if ( character.Entity.Position.Distance( DeliveryPositions[GetStage( character )] ) < WithinThreshold )
				CompleteStage( character );
		}
	}
}
