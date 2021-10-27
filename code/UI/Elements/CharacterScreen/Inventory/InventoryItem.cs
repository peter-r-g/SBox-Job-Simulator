using JobSim.Inventory;
using Sandbox.UI;

namespace JobSim.UI
{
	[UseTemplate]
	public class InventoryItem : Button
	{
		public static InventoryItem SelectedItem;
		public static bool IsSelectedBeingDragged = false;

		public IItemContainer Container { get; set; }

		private IItem _item;
		public IItem Item
		{
			get { return _item; }
			set
			{
				_item = value;
				Refresh();
			}
		}

		public Label StackLabel { get; set; }

		public override void Tick()
		{
			base.Tick();

			if ( SelectedItem == this && (Item.IsNull || Item.Stack <= 0) )
				SelectedItem = null;

			SetClass( "selected", SelectedItem == this );
		}

		protected override void OnDragSelect( SelectionEvent e )
		{
			base.OnDragSelect( e );

			e.Target = this;
			// TODO: Implement dragging items to switch positions.
		}

		protected override void OnClick( MousePanelEvent e )
		{
			if ( Item.IsNull )
				return;

			base.OnClick( e );

			if ( SelectedItem == this )
				SelectedItem = null;
			else
				SelectedItem = this;
		}

		private void Refresh()
		{
			if ( Item.IsNull )
				return;

			Style.SetBackgroundImageAsync( ItemManager.Instance.GetDefinitionByID( _item.ID ).IconPath );

			StackLabel.SetClass( "hidden", Item.Stack <= 1 );
			StackLabel.Text = _item.Stack.ToString();
		}
	}
}
