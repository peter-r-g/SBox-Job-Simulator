using Sandbox;
using System.Collections.Generic;
using System.IO;

namespace JobSim.Inventory
{
	[Library( "persistent_data_item_container" )]
	public class ItemContainerPersistentData : PersistentData
	{
		public IItemContainer Container { get; private set; }

		public int Slots { get; private set; }
		public List<IItem> Items { get; private set; }

		public ItemContainerPersistentData() { }
		public ItemContainerPersistentData( IItemContainer container ) : base( container.UUID )
		{
			Container = container;
			Slots = container.Slots;
			Items = container.Items;
		}

		public override void ReadData( BinaryReader reader )
		{
			UUID = reader.ReadString();
			Slots = reader.ReadInt32();

			Items = new();
			int itemCount = reader.ReadInt32();
			for ( int i = 0; i < itemCount; i++ )
			{
				IItem item = Library.Create<IItem>( reader.ReadString() );
				item.ID = reader.ReadUInt32();
				item.Stack = reader.ReadInt32();
				item.Slot = reader.ReadInt32();

				while ( Items.Count < item.Slot )
					Items.Add( new Item( Items.Count ) );

				Items.Add( item );
			}

			for ( int i = Items.Count; i < Slots; i++ )
				Items.Add( new Item( i ) );
		}

		public override void WriteData( BinaryWriter writer )
		{
			writer.Write( "persistent_data_item_container" );
			writer.Write( UUID );
			writer.Write( Slots );

			List<int> nonNullItems = new();
			for ( int i = 0; i < Items.Count; i++ )
			{
				if ( Items[i].IsNull )
					continue;

				nonNullItems.Add( Items[i].Slot );
			}

			writer.Write( nonNullItems.Count );
			for ( int i = 0; i < nonNullItems.Count; i++ )
			{
				IItem item = Items[nonNullItems[i]];
				writer.Write( item.Definition.ItemClass );
				writer.Write( item.ID );
				writer.Write( item.Stack );
				writer.Write( item.Slot );
			}
		}
	}
}
