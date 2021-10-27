using JobSim.Inventory;
using Sandbox.UI;
using Sandbox.UI.Tests;
using System.Collections.Generic;

namespace JobSim.UI
{
	public class InventoryGrid : VirtualScrollPanel<InventoryItem>
	{
		private IItemContainer _container;
		public IItemContainer Container
		{
			get { return _container; }
			set
			{
				_container = value;
				RefreshItems();
			}
		}

		public Vector2 ItemSize
		{
			get { return Layout.ItemSize; }
			set { Layout.ItemSize = value; }
		}

		public int Columns
		{
			get { return Layout.Columns; }
			set { Layout.Columns = value; }
		}

		public override void OnCellCreated( int i, Panel cell )
		{
			base.OnCellCreated( i, cell );

			InventoryItem itemPanel = cell.GetChild( 0 ) as InventoryItem;
			itemPanel.Container = Container;
			itemPanel.Item = Container.Items[i];
		}

#pragma warning disable IDE0051 // Remove unused private members
		[InventoryEvent.ItemsAdded.Client]
		private void ItemsAdded( ContainerChangedEventArgs _ ) => RefreshItems();
		[InventoryEvent.ItemsRemoved.Client]
		private void ItemsRemoved( ContainerChangedEventArgs _ ) => RefreshItems();
#pragma warning restore IDE0051 // Remove unused private members

		private void RefreshItems()
		{
			Clear();

			for ( int i = 0; i < Container.Items.Count; i++ )
				AddItem( null );
		}
	}
}
