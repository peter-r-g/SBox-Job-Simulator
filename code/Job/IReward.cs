using JobSim.Characters;

namespace JobSim.Job
{
	/// <summary>
	/// Represents a reward to give upon job completion.
	/// </summary>
	public interface IReward
	{
		/// <summary>
		/// Gives the reward to the passed <see cref="IEmployableCharacter"/>.
		/// </summary>
		/// <param name="character">The <see cref="IEmployableCharacter"/> to give the reward to.</param>
		public void GiveReward( IEmployableCharacter character );
	}
}
