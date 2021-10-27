using JobSim.Money;
using Sandbox;

namespace JobSim.Entities
{
	partial class MoneyEntity : ModelEntity, IUse
	{
		[Net]
		public float Money { get; set; }

		private TimeSince timeSinceSpawn;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/citizen_props/cardboardbox01.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			timeSinceSpawn = 0;
		}

		public bool IsUsable( Entity user ) => user is IMoneyContainer;

		public bool OnUse( Entity user )
		{
			if ( !IsServer )
				return false;

			(user as IMoneyContainer).GiveMoney( Money );
			Delete();

			return false;
		}

#pragma warning disable IDE0051 // Remove unused private members
		[Event.Tick.Server]
		private void ValidCheck()
		{
			if ( Money <= 0 && timeSinceSpawn > 0.1f )
				Delete();
		}
#pragma warning restore IDE0051 // Remove unused private members
	}
}
