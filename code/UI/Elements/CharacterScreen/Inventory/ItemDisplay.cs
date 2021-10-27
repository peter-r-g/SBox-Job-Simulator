using JobSim.Inventory;
using Sandbox;
using Sandbox.UI;

namespace JobSim.UI
{
	[UseTemplate]
	public class ItemDisplay : Panel
	{
		public Panel Icon { get; set; }
		public Label Description { get; set; }
		public Button UseButton { get; set; }
		public Button DropButton { get; set; }

		private InventoryItem currentItem;

		public override void Tick()
		{
			base.Tick();

			SetClass( "hidden", InventoryItem.SelectedItem == null );
			if ( currentItem != InventoryItem.SelectedItem )
			{
				RefreshDisplay();
				currentItem = InventoryItem.SelectedItem;
			}

			if ( currentItem != null )
			{
				IItem item = InventoryItem.SelectedItem.Item;
				if ( item.IsNull )
				{
					InventoryItem.SelectedItem = null;
					return;
				}

				if ( item.Stack > 1 )
					Description.Text = $"{item.Stack}x {item.Definition.ItemName}";
				else
					Description.Text = item.Definition.ItemName;

				UseButton.Text = item.Definition.GetUseText( InventoryItem.SelectedItem.Container, item );
				DropButton.Text = item.Definition.GetDropText( InventoryItem.SelectedItem.Container, item );
			}
		}

		private void RefreshDisplay()
		{
			if ( InventoryItem.SelectedItem == null )
				return;

			IItem item = InventoryItem.SelectedItem.Item;
			ItemDefinition itemDef = item.Definition;
			Icon.Style.SetBackgroundImageAsync( itemDef.IconPath );

			IItem clonedItem = item.Clone();
			UseButton.SetClass( "disabled", itemDef.IsUsable( (Local.Pawn as JobSimPlayer).Inventory, clonedItem ) );
			UseButton.SetClass( "disabled", itemDef.IsDroppable( (Local.Pawn as JobSimPlayer).Inventory, clonedItem ) );
		}

		public void UseItem() => ItemManager.RequestUseItem( InventoryItem.SelectedItem.Item.Slot );
		public void DropItem()
		{
			ItemManager.RequestDropItem( InventoryItem.SelectedItem.Item.Slot );
			InventoryItem.SelectedItem = null;
		}
	}
}
