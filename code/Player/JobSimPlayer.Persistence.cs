using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Money;
using Sandbox;

namespace JobSim
{
	public partial class JobSimPlayer
	{
		public virtual void Load( Client cl )
		{
			Realm.Assert( RealmType.Server );

			Clothing.Container clothes = new();
			clothes.LoadFromClient( cl );
			Character = new NetworkedEmployableCharacter( cl.SteamId.ToString(), cl.Name, this, clothes, 0, 0 )
			{
				IsPersistent = true
			};
			Inventory = new JobSimPlayerInventory( cl.SteamId.ToString(), this, 60 )
			{
				IsPersistent = true
			};
			Money = new NetworkedMoneyContainer( cl.SteamId.ToString(), this )
			{
				IsPersistent = true
			};

			Inventory.TryGiveItem( "Pistol", 1 );
			Inventory.TryGiveItem( "Pizza", 10 );
			Inventory.TryGiveItem( "Bad Pizza", 10 );
			Inventory.TryGiveItem( "Brick", 64 );
		}

		public virtual void Load( EmployableCharacterPersistentData cData, ItemContainerPersistentData iData, MoneyContainerPersistentData mData )
		{
			Realm.Assert( RealmType.Server );

			Character = new( this, cData );
			Inventory = new JobSimPlayerInventory( this, iData );
			Money = new( this, mData );
		}
	}
}
