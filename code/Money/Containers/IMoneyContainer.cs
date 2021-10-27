using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace JobSim.Money
{
	/// <summary>
	/// Represents a container of money in the game world.
	/// </summary>
	public interface IMoneyContainer : IPersistent
	{
		/// <summary>
		/// The entity of this character.
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
		/// The amount of money stored in this container.
		/// </summary>
		public float Money { get; }

		/// <summary>
		/// Gives the specified amount of money to this container.
		/// </summary>
		/// <param name="money">The amount of money to add.</param>
		public void GiveMoney( float money );
		/// <summary>
		/// Takes the specified amount of money from this container.
		/// </summary>
		/// <param name="money">The amount of money to take.</param>
		public void TakeMoney( float money );
		/// <summary>
		/// Attempts to take the specified amount of money from this container.
		/// </summary>
		/// <param name="money">The amount of money to take.</param>
		/// <returns>Whether or not the amount of money was taken.</returns>
		public bool TryTakeMoney( float money );

		/// <summary>
		/// List of all IMoneyContainer instances in the realm.
		/// </summary>
		public static List<IMoneyContainer> All = new();

		/// <summary>
		/// Gets an <see cref="IMoneyContainer"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="containerId">See <see cref="IUnique.UUID"/></param>
		/// <returns>See <see cref="IMoneyContainer"/></returns>
		public static IMoneyContainer FromUUID( string containerId ) => FromUUID( containerId, Entity.All.OfType<IMoneyContainer>() );
		/// <summary>
		/// Gets an <see cref="IMoneyContainer"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="containerId">See <see cref="IUnique.UUID"/></param>
		/// /// <param name="containers">The list of <see cref="IMoneyContainer"/> to search through. Useful if you plan on using this function many times at once.</param>
		/// <returns>See <see cref="IMoneyContainer"/></returns>
		public static IMoneyContainer FromUUID( string containerId, IEnumerable<IMoneyContainer> containers )
		{
			foreach ( IMoneyContainer container in containers )
			{
				if ( container.UUID == containerId )
					return container;
			}

			return null;
		}
	}
}
