using JobSim.Inventory;
using Sandbox;

namespace JobSim
{
	public partial class JobSimPlayerInventory : NetworkedItemContainer
	{
		public JobSimPlayerInventory() : this( "unknown", null, 0 ) { }
		public JobSimPlayerInventory( string uuid, Entity entity, int slots ) : base( uuid, entity, slots ) { }
		public JobSimPlayerInventory( Entity entity, ItemContainerPersistentData iData ) : base( entity, iData ) { }

		public override bool IsEquippable( IEntityItem item ) => item.Entity != null;
		public override bool IsEquipped( IEntityItem item ) => Entity.ActiveChild == item.Entity;

		public override bool TryEquipItem( IEntityItem item )
		{
			if ( item.Entity == null || Entity.ActiveChild == item.Entity )
				return false;

			Entity.ActiveChild = item.Entity;

			return true;
		}

		public override bool TryUnequipItem( IEntityItem item )
		{
			if ( item.Entity == null || Entity.ActiveChild != item.Entity )
				return false;

			Entity.ActiveChild = null;

			return true;
		}
	}
}
