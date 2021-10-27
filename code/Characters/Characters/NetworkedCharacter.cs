using Sandbox;
using System.IO;

namespace JobSim.Characters
{
	public partial class NetworkedCharacter : BaseNetworkable, ICharacter
	{
		[Net]
		public string UUID { get; private set; }
		[Net]
		public string Name { get; private set; }
		[Net]
		public Entity Entity { get; private set; }
		public Client EntityOwner => Entity.Client;
		public bool IsClientOwned => Entity.Client != null;
		[Net]
		public Clothing.Container Clothes { get; private set; }
		[Net]
		public bool IsPersistent { get; set; } = false;

		~NetworkedCharacter()
		{
			ICharacter.All.Remove( this );
		}

		public NetworkedCharacter()
		{
			ICharacter.All.Add( this );
		}

		public NetworkedCharacter( string uuid, string name, Entity entity, Clothing.Container clothes ) : this()
		{
			UUID = uuid;
			Name = name;
			Entity = entity;
			Clothes = clothes;
		}

		public NetworkedCharacter( Entity entity, CharacterPersistentData cData ) : this( cData.UUID, cData.Name, entity, cData.Clothes )
		{
			IsPersistent = true;
		}

		public virtual PersistentData SerializeData() => new CharacterPersistentData( this );
	}
}
