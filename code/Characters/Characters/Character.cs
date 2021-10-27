using Sandbox;
using System.IO;

namespace JobSim.Characters
{
	public class Character : ICharacter
	{
		public string UUID { get; private set; }
		public string Name { get; private set; }
		public Entity Entity { get; private set; }
		public Client EntityOwner => Entity.Client;
		public bool IsClientOwned => Entity.Client != null;
		public Clothing.Container Clothes { get; private set; }
		public bool IsPersistent { get; set; } = false;

		~Character()
		{
			ICharacter.All.Remove( this );
		}

		public Character( string uuid, string name, Entity entity, Clothing.Container clothes )
		{
			UUID = uuid;
			Name = name;
			Entity = entity;
			Clothes = clothes;

			ICharacter.All.Add( this );
		}

		public Character( Entity entity, CharacterPersistentData cData ) : this( cData.UUID, cData.Name, entity, cData.Clothes )
		{
			IsPersistent = true;
		}

		public virtual PersistentData SerializeData() => new CharacterPersistentData( this );
	}
}
