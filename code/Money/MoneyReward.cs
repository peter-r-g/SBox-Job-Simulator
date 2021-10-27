using JobSim.Characters;
using JobSim.Job;
using Sandbox;

namespace JobSim.Money
{
	public class MoneyReward : IReward
	{
		public float RewardAmount { get; set; } = 0;

		public MoneyReward( float rewardAmount )
		{
			RewardAmount = rewardAmount;
		}

		public void GiveReward( IEmployableCharacter character )
		{
			if ( Realm.IsServer && character is IMoneyContainer container )
				container.GiveMoney( RewardAmount );
		}
	}
}
