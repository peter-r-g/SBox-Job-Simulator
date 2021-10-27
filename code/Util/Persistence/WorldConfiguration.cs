using System.IO;

namespace JobSim
{
	public class WorldConfiguration
	{
		public virtual void ReadData( BinaryReader reader ) { }
		public virtual void WriteData( BinaryWriter writer ) { }
	}
}
