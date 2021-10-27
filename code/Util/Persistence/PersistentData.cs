using System.IO;

namespace JobSim
{
	public abstract class PersistentData
	{
		public string UUID { get; protected set; }

		public PersistentData() { }
		public PersistentData( string uuid )
		{
			UUID = uuid;
		}

		public abstract void ReadData( BinaryReader reader );
		public abstract void WriteData( BinaryWriter writer );
	}
}
