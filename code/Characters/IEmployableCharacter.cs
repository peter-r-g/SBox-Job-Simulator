using JobSim.Job;
using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace JobSim.Characters
{
	/// <summary>
	/// Represents an employable character in the game world.
	/// </summary>
	public interface IEmployableCharacter : ICharacter
	{
		/// <summary>
		/// The unique ID that represents the job the character is currently doing.
		/// </summary>
		public int JobID { get; set; }
		/// <summary>
		/// The stage of the job the character is currently doing.
		/// </summary>
		public int JobStage { get; set; }
		/// <summary>
		/// The job definition of the job the character is currently doing.
		/// </summary>
		public BaseJob Job { get; }

		/// <summary>
		/// List of all IEmployableCharacter instances in the realm.
		/// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public new static List<IEmployableCharacter> All = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible

		/// <summary>
		/// Gets an <see cref="IEmployableCharacter"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="characterId">See <see cref="IUnique.UUID"/></param>
		/// <returns>See <see cref="IEmployableCharacter"/></returns>
		public static new IEmployableCharacter FromUUID( string characterId ) => FromUUID( characterId, Entity.All.OfType<IEmployableCharacter>() );
		/// <summary>
		/// Gets an <see cref="IEmployableCharacter"/> from the passed <see cref="IUnique.UUID"/>
		/// </summary>
		/// <param name="characterId">See <see cref="IUnique.UUID"/></param>
		/// <param name="characters">The list of <see cref="IEmployableCharacter"/> to search through. Useful if you plan on using this function many times at once.</param>
		/// <returns>See <see cref="IEmployableCharacter"/></returns>
		public static IEmployableCharacter FromUUID( string characterId, IEnumerable<IEmployableCharacter> characters )
		{
			foreach ( IEmployableCharacter character in characters )
			{
				if ( character.UUID == characterId )
					return character;
			}

			return null;
		}
	}
}
