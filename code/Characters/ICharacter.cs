using Sandbox;
using System.Collections.Generic;

namespace JobSim.Characters
{
	/// <summary>
	/// Represents a character in the game world.
	/// </summary>
	public interface ICharacter : IPersistent
	{
		/// <summary>
		/// The name of this character.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// The entity of this character.
		/// </summary>
		public Entity Entity { get; }
		/// <summary>
		/// The clothes this character wears.
		/// </summary>
		public Clothing.Container Clothes { get; }
		/// <summary>
		/// The client owner of this characters entity.
		/// </summary>
		public Client EntityOwner { get; }
		/// <summary>
		/// Whether or not this character is owned by a client.
		/// </summary>
		public bool IsClientOwned { get; }

		/// <summary>
		/// List of all ICharacter instances in the realm.
		/// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static List<ICharacter> All = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible

		/// <summary>
		/// Gets an <see cref="ICharacter"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="characterId">See <see cref="IUnique.UUID"/></param>
		/// <returns>See <see cref="ICharacter"/></returns>
		public static ICharacter FromUUID( string characterId ) => FromUUID( characterId, All );
		/// <summary>
		/// Gets an <see cref="ICharacter"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="characterId">See <see cref="IUnique.UUID"/></param>
		/// <param name="characters">The list of <see cref="ICharacter"/> to search through. Useful if you plan on using this function many times at once.</param>
		/// <returns>See <see cref="ICharacter"/></returns>
		public static ICharacter FromUUID( string characterId, IEnumerable<ICharacter> characters )
		{
			foreach ( ICharacter character in characters )
			{
				if ( character.UUID == characterId )
					return character;
			}

			return null;
		}
	}
}
