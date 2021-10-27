using Sandbox;

namespace JobSim.Inventory
{
	public class EntityItemDefinition : ItemDefinition
	{
		public override string ItemClass => "item_entity";
		/// <summary>
		/// The class of the entity to create.
		/// </summary>
		public string EntityClass { get; set; }

		public EntityItemDefinition() { MaxStack = 1; }

		public override string GetUseText( IItemContainer container, IItem item ) => container.IsEquipped( item.Slot ) ? "Unequip" : "Equip";
		public override bool IsUsable( IItemContainer container, IItem item ) => container.IsEquippable( item as IEntityItem );
		public override (bool, int) OnUse( IItemContainer container, IItem item )
		{
			bool success;
			if ( container.IsEquipped( item as IEntityItem ) )
				success = container.TryUnequipItem( item as IEntityItem );
			else
				success = container.TryEquipItem( item as IEntityItem );

			return (success, 0);
		}
	}
}
