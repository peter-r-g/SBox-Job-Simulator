using JobSim.Characters;

namespace JobSim.Job
{
	public class NullJob : BaseJob
	{
		public NullJob() : base() { }

		public override void Tick( IEmployableCharacter character ) { }
	}
}
