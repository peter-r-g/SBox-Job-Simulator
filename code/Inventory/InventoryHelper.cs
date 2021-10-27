using Sandbox;
using System;
using System.Collections.Generic;

namespace JobSim.Inventory
{
	/// <summary>
	/// Utility class for working with <see cref="IItemContainer"/>.
	/// </summary>
	static class InventoryHelper
	{
		/// <summary>
		/// Returns whether or not the container has the item.
		/// </summary>
		/// <remarks>This will not edit the <paramref name="container"/> or <paramref name="item"/>.</remarks>
		/// <param name="container">The <see cref="IItemContainer"/> to look through.</param>
		/// <param name="item">The <see cref="IItem"/> to look for.</param>
		/// <returns>The first value will be true if the amount of items exist. The second value will be the total amount of said items exist in the container.</returns>
		public static (bool, int) HasItem( IItemContainer container, IItem item )
		{
			int numItems = 0;
			List<IItem> items = container.Items;
			for ( int i = 0; i < container.Slots; i++ )
			{
				if ( items[i].ID == item.ID )
					numItems += items[i].Stack;
			}

			return (numItems >= item.Stack, numItems);
		}

		/// <summary>
		/// Returns whether or not the container has the specified type of item.
		/// </summary>
		/// <remarks>This will not edit the <paramref name="container"/>.</remarks>
		/// <typeparam name="T">The type of item definition to look for.</typeparam>
		/// <param name="container">The <see cref="IItemContainer"/> to look through.</param>
		/// <param name="num">The number of the item type to look for.</param>
		/// <returns>The first value will be true if the amount of items exist. The second value will be the total amount of said items of the provided type exist in the container.</returns>
		public static (bool, int) HasItemType<T>( IItemContainer container, int num ) where T : ItemDefinition
		{
			int numItems = 0;
			List<IItem> items = container.Items;
			for ( int i = 0; i < container.Slots; i++ )
			{
				if ( items[i] is T )
					numItems += items[i].Stack;
			}

			return (numItems >= num, numItems);
		}

		/// <summary>
		/// Returns whether or not the container can be given the item specified.
		/// </summary>
		/// <remarks>This will not edit the <paramref name="container"/> or <paramref name="itemToGive"/>.</remarks>
		/// <param name="container">The <see cref="IItemContainer"/> to look through.</param>
		/// <param name="itemToGive">The <see cref="IItem"/> to give.</param>
		/// <returns>Whether or not the item can be given to this container.</returns>
		public static bool CanGiveItem( IItemContainer container, IItem itemToGive )
		{
			int stackLeft = itemToGive.Stack;
			int maxStack = itemToGive.Definition.MaxStack;

			for ( int i = 0; i < container.Slots; i++ )
			{
				IItem item = container.Items[i];
				if ( item.IsNull )
					stackLeft -= maxStack;
				else if ( item.ID == itemToGive.ID && item.Stack < maxStack )
					stackLeft -= maxStack - item.Stack;
				else
					continue;

				if ( stackLeft <= 0 )
					return true;
			}

			return false;
		}

		/// <summary>
		/// Attempts to give the specified number of items to the container.
		/// </summary>
		/// <remarks>This will edit the <paramref name="container"/> but not the <paramref name="itemToGive"/>.</remarks>
		/// <param name="container">The <see cref="IItemContainer"/> to give to.</param>
		/// <param name="itemToGive">The <see cref="IItem"/> to give.</param>
		/// <returns>The first value will be true if giving the item succeeded, false otherwise. The second value will be the number of items that were left over after filling all available slots.</returns>
		public static (bool, int) TryGiveItem( IItemContainer container, IItem itemToGive )
		{
			List<IItem> items = container.Items;
			List<int> stackableItems = new();
			int stackLeft = itemToGive.Stack;
			int maxStack = itemToGive.Definition.MaxStack;

			for ( int i = 0; i < container.Slots; i++ )
			{
				IItem item = items[i];
				if ( item.IsNull )
					stackLeft -= maxStack;
				else if ( item.ID == itemToGive.ID && item.Stack < maxStack )
					stackLeft -= maxStack - item.Stack;
				else
					continue;

				stackableItems.Add( i );
				if ( stackLeft <= 0 )
				{
					int stackToAdd = itemToGive.Stack;

					for ( int i2 = 0; i2 < stackableItems.Count; i2++ )
					{
						IItem itemToAddTo = items[stackableItems[i2]];
						int addedToItem = Math.Min( maxStack  - itemToAddTo.Stack, stackToAdd );

						itemToAddTo.ID = itemToGive.ID;
						itemToAddTo.Stack += addedToItem;
						stackToAdd -= addedToItem;

						if ( itemToAddTo.Definition is EntityItemDefinition )
						{
							itemToAddTo = new EntityItem( itemToAddTo );
							TryCreateEntity( container, itemToAddTo );
							items[stackableItems[i2]] = itemToAddTo;
						}

						if ( stackToAdd <= 0 )
							break;
					}

					break;
				}
			}

			return (stackLeft <= 0, stackLeft);
		}

		/// <summary>
		/// Returns whether or not the container can remove the item specified.
		/// </summary>
		/// <remarks>This will not edit the <paramref name="container"/> or <paramref name="itemToTake"/>.</remarks>
		/// <param name="container">The <see cref="IItemContainer"/> to look through.</param>
		/// <param name="itemToTake">The <see cref="IItem"/> to take.</param>
		/// <returns>Whether or not the item can be taken from this container.</returns>
		public static bool CanTakeItem( IItemContainer container, IItem itemToTake )
		{
			int stackLeft = itemToTake.Stack;

			for ( int i = 0; i < container.Slots; i++ )
			{
				IItem item = container.Items[i];
				if ( item.ID == itemToTake.ID )
					stackLeft -= item.Stack;
				else
					continue;

				if ( stackLeft <= 0 )
					return true;
			}

			return false;
		}

		/// <summary>
		/// Attempts to take the specified number of items from the container.
		/// </summary>
		/// <remarks>This will edit the <paramref name="container"/> but not the <paramref name="itemToTake"/>.</remarks>
		/// <param name="container">The <see cref="IItemContainer"/> to take from.</param>
		/// <param name="itemToTake">The <see cref="IItem"/> to take.</param>
		/// <returns>The first value will be true if taking the item succeeded, false otherwise. The second value will be the number of items that were left over after taking all available slots.</returns>
		public static (bool, int) TryTakeItem( IItemContainer container, IItem itemToTake )
		{
			List<IItem> items = container.Items;
			List<int> removableItems = new();
			int stackLeft = itemToTake.Stack;

			for ( int i = 0; i < container.Slots; i++ )
			{
				IItem item = items[i];
				if ( item.ID == itemToTake.ID )
					stackLeft -= item.Stack;
				else
					continue;

				removableItems.Add( i );
				if ( stackLeft <= 0 )
				{
					int stackToRemove = itemToTake.Stack;

					for ( int i2 = 0; i2 < removableItems.Count; i2++ )
					{
						IItem itemToRemoveFrom = items[removableItems[i2]];
						int removedFromItem = Math.Min( stackToRemove, itemToRemoveFrom.Stack );

						itemToRemoveFrom.Stack -= removedFromItem;
						if ( itemToRemoveFrom.Stack <= 0 )
						{
							if ( itemToRemoveFrom.Definition is IEntityItem itemEntity )
							{
								itemEntity.Entity?.Delete();
								itemToRemoveFrom = new Item( itemToRemoveFrom );
								items[removableItems[i2]] = itemToRemoveFrom;
							}

							itemToRemoveFrom.ID = ItemManager.NullItemID;
						}

						stackToRemove -= removedFromItem;
					}

					break;
				}
			}

			return (stackLeft <= 0, stackLeft);
		}

		/// <summary>
		/// Attempts to create an entity for the provided item.
		/// </summary>
		/// <param name="container">The <see cref="IItemContainer"/> that contains the item.</param>
		/// <param name="item">The item to create the entity for.</param>
		/// <returns>Whether or not the entity was created for the item.</returns>
		public static bool TryCreateEntity( IItemContainer container, IItem item )
		{
			if ( item is not IEntityItem entityItem )
				return false;

			if ( entityItem.Entity != null && entityItem.Entity.IsValid )
				entityItem.Entity?.Delete();

			entityItem.Entity = Entity.Create( (item.Definition as EntityItemDefinition).EntityClass );
			entityItem.Entity.OnCarryStart( container.Entity );

			return true;
		}

		/// <summary>
		/// Attempts to drop the provided item.
		/// </summary>
		/// <remarks>This will edit the <paramref name="item"/>.</remarks>
		/// <param name="container">The container that is causing the drop to happen.</param>
		/// <param name="item">The item that is going to be dropped.</param>
		/// <returns>Whether or not dropping the item succeeded.</returns>
		public static bool TryDropItem( IItemContainer container, IItem item )
		{
			if ( !item.Definition.IsDroppable( container, item ) )
				return false;

			item.Definition.OnDrop( container, item );
			if ( Realm.IsServer && item is IEntityItem entityItem && entityItem.Entity != null && entityItem.Entity.IsValid )
				entityItem.Entity.OnCarryDrop( container.Entity );

			item.ID = ItemManager.NullItemID;
			item.Stack = 0;

			return true;
		}

		/// <summary>
		/// Attempts to use the provided item.
		/// </summary>
		/// <remarks>This will edit the <paramref name="item"/>.</remarks>
		/// <param name="container">The container that is causing the use to happen.</param>
		/// <param name="item">The item that is going to be used.</param>
		/// <returns>Whether or not using the item succeeded.</returns>
		public static (bool, bool, int) TryUseItem( IItemContainer container, IItem item )
		{
			if ( !item.Definition.IsUsable( container, item ) )
				return (false, false, 0);

			(bool shouldConsume, int consumeAmount) = item.Definition.OnUse( container, item );
			if ( shouldConsume )
			{
				item.Stack -= consumeAmount;
				if ( item.Stack <= 0 )
					item.ID = ItemManager.NullItemID;
			}

			return (true, shouldConsume, consumeAmount);
		}
	}
}
