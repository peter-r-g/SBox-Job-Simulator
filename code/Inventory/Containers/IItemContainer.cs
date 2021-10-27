using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace JobSim.Inventory
{
	/// <summary>
	/// Represents a container of items in the game world.
	/// </summary>
	public interface IItemContainer : IPersistent
	{
		/// <summary>
		/// The amount of slots this container has.
		/// </summary>
		public int Slots { get; }
		/// <summary>
		/// The entity of this container.
		/// </summary>
		public Entity Entity { get; }
		/// <summary>
		/// The client owner of this containers entity.
		/// </summary>
		public Client EntityOwner { get; }
		/// <summary>
		/// Whether or not this container is owned by a client.
		/// </summary>
		public bool IsClientOwned { get; }
		/// <summary>
		/// The list of items the container has.
		/// </summary>
		public List<IItem> Items { get; }

		/// <summary>
		/// Returns whether or not the container has the item.
		/// </summary>
		/// <param name="itemName">The name of the item to look for.</param>
		/// <returns>The first value will be true if the item exists. The second value will be the total amount of said items exist in the container.</returns>
		public (bool, int) HasItem( string itemName );
		/// <summary>
		/// Returns whether or not the container has the item.
		/// </summary>
		/// <param name="itemName">The name of the item to look for.</param>
		/// <param name="num">The number of items to look for.</param>
		/// <returns>The first value will be true if the amount of items exist. The second value will be the total amount of said items exist in the container.</returns>
		public (bool, int) HasItem( string itemName, int num );
		/// <summary>
		/// Returns whether or not the container has the specified type of item.
		/// </summary>
		/// <typeparam name="T">The type of item definition to look for.</typeparam>
		/// <returns>The first value will be true if the amount of items exist. The second value will be the total amount of said items of the provided type exist in the container.</returns>
		public (bool, int) HasItemType<T>() where T : ItemDefinition;
		/// <summary>
		/// Returns whether or not the container has the amount of the specified type of item.
		/// </summary>
		/// <typeparam name="T">The type of item definition to look for.</typeparam>
		/// <param name="num">The number of the item type to look for.</param>
		/// <returns>The first value will be true if the amount of items exist. The second value will be the total amount of said items of the provided type exist in the container.</returns>
		public (bool, int) HasItemType<T>( int num ) where T : ItemDefinition;
		/// <summary>
		/// Attempts to give a single item to the container.
		/// </summary>
		/// <param name="itemName">The name of the item to give.</param>
		/// <returns>The first value will be true if giving the item succeeded, false otherwise. The second value will be the number of items that were left over after filling all available slots.</returns>
		public (bool, int) TryGiveItem( string itemName );
		/// <summary>
		/// Attempts to give the specified number of items to the container.
		/// </summary>
		/// <param name="itemName">The name of the item to give.</param>
		/// <param name="num">The number of the specified item to give.</param>
		/// <returns>The first value will be true if giving the item succeeded, false otherwise. The second value will be the number of items that were left over after filling all available slots.</returns>
		public (bool, int) TryGiveItem( string itemName, int num );
		/// <summary>
		/// Attempts to give the whole item with its stack to the container.
		/// </summary>
		/// <param name="item">The item to give.</param>
		/// <returns>The first value will be true if giving the item succeeded, false otherwise. The second value will be the number of items that were left over after filling all available slots.</returns>
		public (bool, int) TryGiveItem( IItem item );
		/// <summary>
		/// Attempts to take a single item from the container.
		/// </summary>
		/// <param name="itemName">The name of the item to take.</param>
		/// <returns>The first value will be true if taking the item succeeded, false otherwise. The second value will be the number of items that were left over after taking all available slots.</returns>
		public (bool, int) TryTakeItem( string itemName );
		/// <summary>
		/// Attempts to take the specified number of items from the container.
		/// </summary>
		/// <param name="itemName">The name of the item to take.</param>
		/// <param name="num">The number of the specified item to take.</param>
		/// <returns>The first value will be true if taking the item succeeded, false otherwise. The second value will be the number of items that were left over after taking all available slots.</returns>
		public (bool, int) TryTakeItem( string itemName, int num );
		/// <summary>
		/// Attempts to take the whole item with its stack from the container.
		/// </summary>
		/// <param name="item">The item to take.</param>
		/// <returns>The first value will be true if taking the item succeeded, false otherwise. The second value will be the number of items that were left over after taking all available slots.</returns>
		public (bool, int) TryTakeItem( IItem item );
		/// <summary>
		/// Attempts to create an entity for the provided entity.
		/// </summary>
		/// <param name="item">The item to create the entity for.</param>
		/// <returns>Whether or not the entity was created for the item.</returns>
		public bool TryCreateItemEntity( IItem item );
		/// <summary>
		/// Attempts to drop the item in the provided slot.
		/// </summary>
		/// <param name="slot">The index of the item to drop.</param>
		/// <returns>Whether or not the item was dropped.</returns>
		public bool TryDropItem( int slot );
		/// <summary>
		/// Attempts to drop the provided item.
		/// </summary>
		/// <param name="item">The item to drop.</param>
		/// <returns>Whether or not the item was dropped.</returns>
		public bool TryDropItem( IItem item );
		/// <summary>
		/// Attempts to use the item in the provided slot.
		/// </summary>
		/// <param name="slot">The index of the item to use.</param>
		/// <returns>Whether or not the item was used.</returns>
		public bool TryUseItem( int slot );
		/// <summary>
		/// Attempts to use the provided item.
		/// </summary>
		/// <param name="item">The item to use.</param>
		/// <returns>Whether or not the item was used.</returns>
		public bool TryUseItem( IItem item );
		public bool IsEquippable( int slot );
		public bool IsEquippable( IEntityItem item );
		public bool IsEquipped( int slot );
		public bool IsEquipped( IEntityItem item );
		public bool TryEquipItem( int slot );
		public bool TryEquipItem( IEntityItem item );
		public bool TryUnequipItem( int slot );
		public bool TryUnequipItem( IEntityItem item );

		/// <summary>
		/// List of all IItemContainer instances in the realm.
		/// </summary>
		public static List<IItemContainer> All = new();

		/// <summary>
		/// Gets an <see cref="IItemContainer"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="containerId">See <see cref="IUnique.UUID"/></param>
		/// <returns>See <see cref="IItemContainer"/></returns>
		public static IItemContainer FromUUID( string containerId ) => FromUUID( containerId, Entity.All.OfType<IItemContainer>() );
		/// <summary>
		/// Gets an <see cref="IItemContainer"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="containerId">See <see cref="IUnique.UUID"/></param>
		/// /// <param name="containers">The list of <see cref="IItemContainer"/> to search through. Useful if you plan on using this function many times at once.</param>
		/// <returns>See <see cref="IItemContainer"/></returns>
		public static IItemContainer FromUUID( string containerId, IEnumerable<IItemContainer> containers )
		{
			foreach ( IItemContainer container in containers )
			{
				if ( container.UUID == containerId )
					return container;
			}

			return null;
		}
	}
}
