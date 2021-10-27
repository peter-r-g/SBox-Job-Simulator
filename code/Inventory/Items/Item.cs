using Sandbox;

namespace JobSim.Inventory
{
	[Library( "item" )]
	public class Item : IItem
	{
		public uint ID { get; set;  } = 0;
		public int Stack { get; set; } = 0;
		public int Slot { get; set; } = -1;

		public ItemDefinition Definition => ItemManager.Instance.GetDefinitionByID( ID );

		public Item()
		{
			ID = ItemManager.NullItemID;
		}

		public Item( int slot ) : this()
		{
			Slot = slot;
		}

		public Item( IItem item )
		{
			ID = item.ID;
			Stack = item.Stack;
			Slot = item.Slot;
		}

		public virtual IItem Clone() => new Item( this );

		public virtual void NetRead( ref NetRead reader )
		{
			ID = reader.Read<uint>();
			Stack = reader.Read<int>();
			Slot = reader.Read<int>();
		}

		public virtual void NetWrite( NetWrite writer )
		{
			writer.Write( Definition.ItemClass );
			writer.Write( ID );
			writer.Write( Stack );
			writer.Write( Slot );
		}

		public static Item FromDefinition( ItemDefinition itemDefinition ) => FromDefinition( itemDefinition, 1 );
		public static Item FromDefinition( ItemDefinition itemDefinition, int amount, int slot = -1 )
		{
			Item item = new( slot );
			item.ID = itemDefinition.ItemID;
			item.Stack = amount;

			return item;
		}

		public static Item FromName( string itemName ) => FromName( itemName, 1 );
		public static Item FromName( string itemName, int amount, int slot = -1 )
		{
			Item item = new( slot );
			item.ID = ItemManager.Instance.GetDefinition( itemName ).ItemID;
			item.Stack = amount;

			return item;
		}
	}
}
