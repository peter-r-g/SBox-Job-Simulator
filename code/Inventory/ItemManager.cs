using Sandbox;
using System;
using System.Collections.Generic;

namespace JobSim.Inventory
{
	public class ItemManager
	{
		public static ItemManager Instance;
		public static ItemDefinition NullItem = null;
		public const uint NullItemID = 0;

		private uint nextId = 0;
		private readonly Dictionary<string, ItemDefinition> itemDefinitions = new();
		private readonly Dictionary<uint, ItemDefinition> itemDefinitionsByID = new();

		public ItemManager()
		{
			if ( Instance != null )
				throw new Exception( "An instance of ItemManager already exists?" );

			Instance = this;

			ItemDefinition nullItem = new();
			nullItem.ItemName = "Null";
			Register( nullItem );
		}

		public void Register( ItemDefinition item )
		{
			string name = item.ItemName.ToLower();
			if ( itemDefinitions.ContainsKey( name ) )
			{
				Realm.Log.Error( $"An item with the name \"{item.ItemName}\" already exists" );
				return;
			}
			else if ( name == "null" )
				NullItem = item;

			item.ItemID = nextId;
			itemDefinitions.Add( name, item );
			itemDefinitionsByID.Add( item.ItemID, item );
			nextId++;
		}

		public ItemDefinition GetDefinition( string itemName ) => itemDefinitions[itemName.ToLower()];
		public ItemDefinition GetDefinitionByID( uint id ) => itemDefinitionsByID[id];

		public static void RequestUseItem( int slot )
		{
			Realm.Assert( RealmType.Client );

			ServerRequestUseItem( slot );
			(Local.Pawn as JobSimPlayer).Inventory.TryUseItem( slot );
		}

		public static void RequestDropItem( int slot )
		{
			Realm.Assert( RealmType.Client );

			ServerRequestDropItem( slot );
			(Local.Pawn as JobSimPlayer).Inventory.TryDropItem( slot );
		}

		public static bool IsNull( IItem item ) => IsNull( item.ID );
		public static bool IsNull( uint id ) => id == NullItemID;

		[ServerCmd]
		public static void ServerRequestUseItem( int slot )
		{
			Realm.Assert( RealmType.Server );
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Inventory.TryUseItem( slot );
		}

		[ServerCmd]
		public static void ServerRequestDropItem( int slot )
		{
			Realm.Assert( RealmType.Server );
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Inventory.TryDropItem( slot );
		}
	}
}
