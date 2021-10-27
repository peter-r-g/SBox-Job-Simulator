using Sandbox;

namespace JobSim.Inventory
{
	/// <summary>
	/// Events pertaining to inventories and items.
	/// </summary>
	static class InventoryEvent
	{
		#region ItemsAdded
		public const string ItemsAddedEvent = "itemsAdded";

		/// <summary>
		/// Called when items have been added to an <see cref="IItemContainer"/>.
		/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class ItemsAdded
		{
			public const string ServerEvent = ItemsAddedEvent + ".Server";
			public const string ClientEvent = ItemsAddedEvent + ".Client";

			/// <summary>
			/// Called when items have been added to an <see cref="IItemContainer"/> only on server.
			/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when items have been added to an <see cref="IItemContainer"/> only on client.
			/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when items have been added to an <see cref="IItemContainer"/> on both server and client.
		/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public class ItemsAddedAttribute : EventAttribute
		{
			public ItemsAddedAttribute() : base( ItemsAddedEvent ) { }
		}
		#endregion

		#region ItemsRemoved
		public const string ItemsRemovedEvent = "itemsRemoved";

		/// <summary>
		/// Called when items have been removed from an <see cref="IItemContainer"/>.
		/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class ItemsRemoved
		{
			public const string ServerEvent = ItemsRemovedEvent + ".Server";
			public const string ClientEvent = ItemsRemovedEvent + ".Client";

			/// <summary>
			/// Called when items have been added to an <see cref="IItemContainer"/> only on server.
			/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when items have been added to an <see cref="IItemContainer"/> only on client.
			/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when items have been removed from an <see cref="IItemContainer"/> on both server and client.
		/// <see cref="ContainerChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public class ItemsRemovedAttribute : EventAttribute
		{
			public ItemsRemovedAttribute() : base( ItemsRemovedEvent ) { }
		}
		#endregion

		#region ItemDropped
		public const string ItemDroppedEvent = "itemDropped";

		/// <summary>
		/// Called when an item has been dropped from an <see cref="IItemContainer"/>.
		/// <see cref="ItemDroppedEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class ItemDropped
		{
			public const string ServerEvent = ItemDroppedEvent + ".Server";
			public const string ClientEvent = ItemDroppedEvent + ".Client";

			/// <summary>
			/// Called when an item has been dropped from an <see cref="IItemContainer"/> only on server.
			/// <see cref="ItemDroppedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when an item has been dropped from an <see cref="IItemContainer"/> only on client.
			/// <see cref="ItemDroppedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when an item has been dropped from an <see cref="IItemContainer"/> on both server and client.
		/// <see cref="ItemDroppedEventArgs"/> will always be passed to this event.
		/// </summary>
		public class ItemDroppedAttribute : EventAttribute
		{
			public ItemDroppedAttribute() : base( ItemDroppedEvent ) { }
		}
		#endregion

		#region ItemUsed
		public const string ItemUsedEvent = "itemUsed";

		/// <summary>
		/// Called when an item has been used from an <see cref="IItemContainer"/>.
		/// <see cref="ItemUsedEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class ItemUsed
		{
			public const string ServerEvent = ItemUsedEvent + ".Server";
			public const string ClientEvent = ItemUsedEvent + ".Client";

			/// <summary>
			/// Called when an item has been used from an <see cref="IItemContainer"/> only on server.
			/// <see cref="ItemUsedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when an item has been used from an <see cref="IItemContainer"/> only on client.
			/// <see cref="ItemUsedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when an item has been used from an <see cref="IItemContainer"/> on both server and client.
		/// <see cref="ItemUsedEventArgs"/> will always be passed to this event.
		/// </summary>
		public class ItemUsedAttribute : EventAttribute
		{
			public ItemUsedAttribute() : base( ItemUsedEvent ) { }
		}
		#endregion
	}

	#region EventArgs
	class ContainerChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The container that was changed. This will always be null on client.
		/// </summary>
		public IItemContainer Container { get; }
		/// <summary>
		/// The item definition of the item that was changed within the container.
		/// </summary>
		public ItemDefinition ItemDefinition { get; }
		/// <summary>
		/// The amount of the item that was added/removed from the container.
		/// </summary>
		public int Amount { get; }

		public ContainerChangedEventArgs( ItemDefinition itemDefinition, int amount )
		{
			ItemDefinition = itemDefinition;
			Amount = amount;
		}

		public ContainerChangedEventArgs( IItemContainer container, ItemDefinition itemDefinition, int amount ) : this( itemDefinition, amount )
		{
			Container = container;
		}
	}

	class ItemDroppedEventArgs : EventArgs
	{
		/// <summary>
		/// The container that dropped the item.
		/// </summary>
		public IItemContainer Container { get; }
		/// <summary>
		/// The item before it was dropped.
		/// </summary>
		public IItem Item { get; }
		/// <summary>
		/// The item after it was dropped.
		/// </summary>
		public IItem DroppedItem { get; }

		public ItemDroppedEventArgs( IItemContainer container, IItem item, IItem droppedItem )
		{
			Container = container;
			Item = item;
			DroppedItem = droppedItem;
		}
	}

	class ItemUsedEventArgs : EventArgs
	{
		/// <summary>
		/// The container that used the item.
		/// </summary>
		public IItemContainer Container { get; }
		/// <summary>
		/// The item before it was used.
		/// </summary>
		public IItem Item { get; }
		/// <summary>
		/// The item after it was used.
		/// </summary>
		public IItem UsedItem { get; }

		public ItemUsedEventArgs( IItemContainer container, IItem item, IItem usedItem )
		{
			Container = container;
			Item = item;
			UsedItem = usedItem;
		}
	}
	#endregion
}
