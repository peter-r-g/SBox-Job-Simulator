using JobSim.Job;
using JobSim.Waypoints;
using Sandbox;

namespace JobSim.UI.Jobs
{
	[Library( "ui_deliveryjob" )]
	public class DeliveryJobUIElement : JobUIElement
	{
		private DeliveryJob DeliveryJob => Job as DeliveryJob;
		private Waypoint deliveryWaypoint;

		public override void Setup()
		{
			base.Setup();

			deliveryWaypoint = new Waypoint()
				.WithDescription( "Destination" )
				.WithType( WaypointType.Position )
				.WithColor( WaypointColor.Yellow )
				.WithTextOptions( Color.White, "Roboto" );
		}

		public override void Cleanup()
		{
			base.Cleanup();

			deliveryWaypoint.Delete();
		}

		public override void Tick()
		{
			base.Tick();

			deliveryWaypoint.Position = DeliveryJob.DeliveryPositions[JobManager.GetStage( Character )];
		}
	}
}
