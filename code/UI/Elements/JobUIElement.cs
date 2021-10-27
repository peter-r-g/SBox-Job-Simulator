using JobSim.Characters;
using JobSim.Job;
using Sandbox;
using Sandbox.UI;

namespace JobSim.UI
{
	[Library( "ui_job" )]
	public class JobUIElement : Panel
	{
		public BaseJob Job { get; set; }
		public IEmployableCharacter Character { get; set; }

		public virtual void Setup() { }
		public virtual void Cleanup() { }
	}
}
