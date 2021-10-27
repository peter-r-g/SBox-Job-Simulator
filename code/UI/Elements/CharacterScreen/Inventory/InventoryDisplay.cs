using JobSim.Inventory;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;

namespace JobSim.UI
{
	[UseTemplate]
	public class InventoryDisplay : Panel
	{
		private IItemContainer _container;
		public IItemContainer Container
		{
			get { return _container; }
			set
			{
				_container = value;
				InventoryGrid.Container = _container;
			}
		}

		public ItemDisplay ItemDisplay { get; set; }
		public InventoryGrid InventoryGrid { get; set; }

		protected override void PostTemplateApplied()
		{
			base.PostTemplateApplied();

			InventoryGrid.ItemSize = new Vector2( 64, 64 );
			InventoryGrid.Columns = 10;
		}
	}
}
