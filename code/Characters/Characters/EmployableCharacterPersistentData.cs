using JobSim.Job;
using Sandbox;
using System.IO;

namespace JobSim.Characters
{
	[Library( "persistent_data_employable_character" )]
	public class EmployableCharacterPersistentData : CharacterPersistentData
	{
		public int JobID { get; private set; }
		public int JobStage { get; private set; }

		public EmployableCharacterPersistentData() { }
		public EmployableCharacterPersistentData( IEmployableCharacter character ) : base( character )
		{
			JobID = character.JobID;
			JobStage = character.JobStage;
		}

		public override void ReadData( BinaryReader reader )
		{
			base.ReadData( reader );

			JobID = reader.ReadInt32();
			JobStage = reader.ReadInt32();
		}

		public override void WriteData( BinaryWriter writer )
		{
			writer.Write( "persistent_data_employable_character" );
			writer.Write( UUID );
			writer.Write( Name );
			writer.Write( Clothes.Serialize() );
			writer.Write( JobID );
			writer.Write( JobStage );
		}
	}
}
