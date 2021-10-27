using JobSim.Entities;
using Sandbox;

namespace JobSim.Inventory
{
	public class ItemDefinition
	{
		public virtual string ItemClass { get; } = "item";
		/// <summary>
		/// The unique ID that represents this definition.
		/// </summary>
		public uint ItemID { get; set; }
		/// <summary>
		/// The name of the item.
		/// </summary>
		public string ItemName { get; set; }
		/// <summary>
		/// The max amount of this item that can be stacked in a single slot.
		/// </summary>
		public int MaxStack { get; set; }
		/// <summary>
		/// The path to the UI icon.
		/// </summary>
		public string IconPath { get; set; }
		/// <summary>
		/// The path to the world model of the item when it's dropped.
		/// </summary>
		public string DroppedModelPath { get; set; }

		public virtual string GetUseText( IItemContainer container, IItem item ) => "Use";
		public virtual bool IsUsable( IItemContainer container, IItem item ) => false;
		public virtual (bool, int) OnUse( IItemContainer container, IItem item ) => (false, 0);

		public virtual string GetDropText( IItemContainer container, IItem item ) => "Drop";
		public virtual bool IsDroppable( IItemContainer container, IItem item ) => true;
		public virtual void OnDrop( IItemContainer container, IItem item )
		{
			if ( !Realm.IsServer )
				return;

			ItemEntity entity = Entity.Create<ItemEntity>();
			entity.Item = item.Clone();
			entity.Position = container.Entity.Position + (Vector3.Up * 10);
		}
	}
}
