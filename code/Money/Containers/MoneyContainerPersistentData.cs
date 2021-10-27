using Sandbox;
using System.IO;

namespace JobSim.Money
{
	[Library( "persistent_data_money_container" )]
	public class MoneyContainerPersistentData : PersistentData
	{
		public IMoneyContainer Container { get; private set; }

		public float Money { get; private set; }

		public MoneyContainerPersistentData() { }
		public MoneyContainerPersistentData( IMoneyContainer container ) : base( container.UUID )
		{
			Container = container;
			Money = container.Money;
		}

		public override void ReadData( BinaryReader reader )
		{
			UUID = reader.ReadString();
			Money = reader.ReadSingle();
		}

		public override void WriteData( BinaryWriter writer )
		{
			writer.Write( "persistent_data_money_container" );
			writer.Write( UUID );
			writer.Write( Money );
		}
	}
}
