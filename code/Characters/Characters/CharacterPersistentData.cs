using Sandbox;
using System.IO;

namespace JobSim.Characters
{
	[Library( "persistent_data_character" )]
	public class CharacterPersistentData : PersistentData
	{
		public ICharacter Character { get; private set; }

		public string Name { get; private set; }
		public Clothing.Container Clothes { get; private set; }

		public CharacterPersistentData() { }
		public CharacterPersistentData( ICharacter character ) : base( character.UUID )
		{
			Character = character;
			Name = character.Name;
			Clothes = character.Clothes;
		}

		public override void ReadData( BinaryReader reader )
		{
			UUID = reader.ReadString();
			Name = reader.ReadString();
			Clothes = new();
			Clothes.Deserialize( reader.ReadString() );
		}

		public override void WriteData( BinaryWriter writer )
		{
			writer.Write( "persistent_data_character" );
			writer.Write( UUID );
			writer.Write( Name );
			writer.Write( Clothes.Serialize() );
		}
	}
}
