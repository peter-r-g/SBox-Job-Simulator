using Sandbox;
using System.Collections.Generic;

namespace JobSim.Inventory
{
	public class ItemContainer : IItemContainer
	{
		public string UUID { get; private set; }
		public Entity Entity { get; private set; }
		public Client EntityOwner => Entity.Client;
		public bool IsClientOwned => Entity.Client != null;
		public int Slots { get; private set; }
		public bool IsPersistent { get; set; } = false;

		public List<IItem> Items { get; private set; } = new();

		~ItemContainer()
		{
			IItemContainer.All.Remove( this );
		}

		public ItemContainer( string uuid, Entity entity, int slots )
		{
			UUID = uuid;
			Entity = entity;
			Slots = slots;

			IItemContainer.All.Add( this );
		}

		public ItemContainer( Entity entity, ItemContainerPersistentData iData ) : this( iData.UUID, entity, iData.Slots )
		{
			Items = iData.Items;
			IsPersistent = true;

			for ( int i = 0; i < Items.Count; i++ )
			{
				if ( Items[i] is EntityItem )
					TryCreateItemEntity( Items[i] );
			}
		}

		public (bool, int) HasItem( string itemName ) => HasItem( itemName, 1 );
		public (bool, int) HasItem( string itemName, int num ) => InventoryHelper.HasItem( this, Item.FromName( itemName, num ) );

		public (bool, int) HasItemType<T>() where T : ItemDefinition => HasItemType<T>( 1 );
		public (bool, int) HasItemType<T>( int num ) where T : ItemDefinition => InventoryHelper.HasItemType<T>( this, num );

		public (bool, int) TryGiveItem( string itemName ) => TryGiveItem( itemName, 1 );
		public (bool, int) TryGiveItem( string itemName, int num ) => TryGiveItem( Item.FromName( itemName, num ) );
		public (bool, int) TryGiveItem( IItem item )
		{
			(bool success, int leftover) = InventoryHelper.TryGiveItem( this, item );
			if ( success )
			{
				OnItemsAdded( item.Definition.ItemID, item.Stack );
				EventRunner.Run( InventoryEvent.ItemsAddedEvent, new ContainerChangedEventArgs( this, item.Definition, item.Stack ) );
			}

			return (success, leftover);
		}

		public (bool, int) TryTakeItem( string itemName ) => TryTakeItem( itemName, 1 );
		public (bool, int) TryTakeItem( string itemName, int num ) => TryTakeItem( Item.FromName( itemName, num ) );
		public (bool, int) TryTakeItem( IItem item )
		{
			(bool success, int leftover) = InventoryHelper.TryTakeItem( this, item );
			if ( success )
			{
				OnItemsRemoved( item.Definition.ItemID, item.Stack );
				EventRunner.Run( InventoryEvent.ItemsRemovedEvent, new ContainerChangedEventArgs( this, item.Definition, item.Stack ) );
			}

			return (success, leftover);
		}

		public virtual bool TryCreateItemEntity( IItem item ) => Realm.Assert( RealmType.Server, InventoryHelper.TryCreateEntity( this, item ) );

		public bool TryDropItem( int slot ) => TryDropItem( Items[slot] );
		public virtual bool TryDropItem( IItem item )
		{
			IItem clonedItem = item.Clone();
			bool success = InventoryHelper.TryDropItem( this, item );

			if ( success )
			{
				if ( Realm.IsServer )
				{
					OnItemsRemoved( clonedItem.ID, clonedItem.Stack );
					EventRunner.Run( InventoryEvent.ItemsRemovedEvent, new ContainerChangedEventArgs( this, clonedItem.Definition, clonedItem.Stack ) );
				}

				EventRunner.Run( InventoryEvent.ItemDroppedEvent, new ItemDroppedEventArgs( this, clonedItem, item ) );
			}

			return success;
		}

		public bool TryUseItem( int slot ) => TryUseItem( Items[slot] );
		public virtual bool TryUseItem( IItem item )
		{
			IItem clonedItem = item.Clone();
			(bool success, bool shouldConsume, int consumeAmount) = InventoryHelper.TryUseItem( this, item );

			if ( success )
			{
				if ( Realm.IsServer && shouldConsume && consumeAmount > 0 )
				{
					OnItemsRemoved( clonedItem.ID, consumeAmount );
					EventRunner.Run( InventoryEvent.ItemsRemovedEvent, new ContainerChangedEventArgs( this, clonedItem.Definition, consumeAmount ) );
				}

				EventRunner.Run( InventoryEvent.ItemUsedEvent, new ItemUsedEventArgs( this, clonedItem, item ) );
			}

			return success;
		}

		public PersistentData SerializeData() => new ItemContainerPersistentData( this );

		protected virtual void OnItemsAdded( uint id, int amountGained ) { }
		protected virtual void OnItemsRemoved( uint id, int amountRemoved ) { }

		public bool IsEquippable( int slot ) => IsEquippable( Items[slot] as IEntityItem );
		public virtual bool IsEquippable( IEntityItem item ) => false;
		public bool IsEquipped( int slot ) => IsEquipped( Items[slot] as IEntityItem );
		public virtual bool IsEquipped( IEntityItem item ) => false;
		public bool TryEquipItem( int slot ) => TryEquipItem( Items[slot] as IEntityItem );
		public virtual bool TryEquipItem( IEntityItem item ) => false;
		public bool TryUnequipItem( int slot ) => TryUnequipItem( Items[slot] as IEntityItem );
		public virtual bool TryUnequipItem( IEntityItem item ) => false;
	}
}
