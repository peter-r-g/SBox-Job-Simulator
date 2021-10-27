using Sandbox;
using System.IO;

namespace JobSim.Money
{
	public partial class NetworkedMoneyContainer : BaseNetworkable, IMoneyContainer
	{
		[Net]
		public string UUID { get; private set; }
		[Net]
		public Entity Entity { get; private set; }
		public Client EntityOwner => Entity.Client;
		public bool IsClientOwned => Entity.Client != null;
		[Net]
		public bool IsPersistent { get; set; } = false;
		[Net, Change]
		public float Money { get; private set; } = 0;

		~NetworkedMoneyContainer()
		{
			IMoneyContainer.All.Add( this );
		}

		public NetworkedMoneyContainer()
		{
			IMoneyContainer.All.Add( this );
		}

		public NetworkedMoneyContainer( string uuid, Entity entity ) : this()
		{
			UUID = uuid;
			Entity = entity;
		}

		public NetworkedMoneyContainer( Entity entity, MoneyContainerPersistentData mData ) : this( mData.UUID, entity )
		{
			Money = mData.Money;
			IsPersistent = true;
		}

		public void GiveMoney( float money )
		{
			if ( money == 0 )
				return;

			float oldMoney = Money;
			Money += money;
			OnMoneyGiven( oldMoney, Money );
		}

		public void TakeMoney( float money )
		{
			if ( money == 0 )
				return;

			float oldMoney = Money;
			Money -= money;
			OnMoneyTaken( oldMoney, Money );
		}

		public bool TryTakeMoney( float money )
		{
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

		private void OnMoneyChanged( float oldMoney, float newMoney )
		{
			if ( newMoney - oldMoney > 0 )
				OnMoneyGiven( oldMoney, newMoney );
			else
				OnMoneyTaken( oldMoney, newMoney );
		}

		public PersistentData SerializeData() => new MoneyContainerPersistentData( this );
	}
}
