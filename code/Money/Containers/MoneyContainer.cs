using Sandbox;
using System.IO;

namespace JobSim.Money
{
	public class MoneyContainer : IMoneyContainer
	{
		public string UUID { get; private set; }
		public Entity Entity { get; private set; }
		public Client EntityOwner => Entity.Client;
		public bool IsClientOwned => Entity.Client != null;
		public bool IsPersistent { get; set; } = false;

		public float Money { get; private set; } = 0;

		~MoneyContainer()
		{
			IMoneyContainer.All.Remove( this );
		}

		public MoneyContainer( string uuid, Entity entity )
		{
			UUID = uuid;
			Entity = entity;

			IMoneyContainer.All.Add( this );
		}

		public MoneyContainer( Entity entity, MoneyContainerPersistentData mData ) : this( mData.UUID, entity )
		{
			Money = mData.Money;
			IsPersistent = true;
		}

		public void GiveMoney( float money )
		{
			Realm.Assert( RealmType.Server );
			if ( money == 0 )
				return;

			float oldMoney = Money;
			Money += money;
			OnMoneyGiven( oldMoney, Money );
		}

		public void TakeMoney( float money )
		{
			Realm.Assert( RealmType.Server );
			if ( money == 0 )
				return;

			float oldMoney = Money;
			Money -= money;
			OnMoneyTaken( oldMoney, Money );
		}

		public bool TryTakeMoney( float money )
		{
			Realm.Assert( RealmType.Server );
			if ( Money < money )
				return false;
			else if ( money == 0 )
				return true;

			float oldMoney = Money;
			Money -= money;
			OnMoneyTaken( oldMoney, Money );

			return true;
		}

		protected virtual void OnMoneyGiven( float oldMoney, float newMoney )
			=> EventRunner.Run( MoneyEvent.MoneyChangedEvent, new MoneyChangedEventArgs( this, newMoney - oldMoney ) );

		protected virtual void OnMoneyTaken( float oldMoney, float newMoney )
			=> EventRunner.Run( MoneyEvent.MoneyChangedEvent, new MoneyChangedEventArgs( this, newMoney - oldMoney ) );

		public PersistentData SerializeData() => new MoneyContainerPersistentData( this );
	}
}
