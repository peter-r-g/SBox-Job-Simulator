using JobSim.Inventory;
using Sandbox;

namespace JobSim.Entities
{
	partial class ItemEntity : ModelEntity, IUse
	{
		public NetworkedItemContainer ItemContainer { get; private set; }
		public IItem Item
		{
			get { return ItemContainer.Items[0]; }
			set
			{
				ItemContainer.Items[0] = value;
				UpdateEntity();
			}
		}

		private TimeSince timeSinceSpawn;

		public override void Spawn()
		{
			base.Spawn();
			ItemContainer = new( null, this, 1 );

			SetModel( "models/citizen_props/cardboardbox01.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			timeSinceSpawn = 0;
		}

		private void UpdateEntity()
		{
			if ( Item == null )
			{
				Delete();
				return;
			}

			ItemDefinition itemDef = Item.Definition;
			if ( string.IsNullOrEmpty( itemDef.DroppedModelPath ) )
				SetModel( "models/citizen_props/cardboardbox01.vmdl" );
			else
				SetModel( itemDef.DroppedModelPath );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		public bool IsUsable( Entity user ) => user is IItemContainer || user is JobSimPlayer;

		public bool OnUse( Entity user )
		{
			if ( !IsServer )
				return false;

			bool success = false;
			if ( user is IItemContainer container )
				(success, _) = container.TryGiveItem( Item );
			else if ( user is JobSimPlayer ply )
				(success, _) = ply.Inventory.TryGiveItem( Item );

			if ( success )
				Delete();

			return false;
		}

#pragma warning disable IDE0051 // Remove unused private members
		[Event.Tick.Server]
		private void TickServer()
		{
			if ( Item == null && timeSinceSpawn > 0.1f )
				Delete();
		}
#pragma warning restore IDE0051 // Remove unused private members
	}
}
