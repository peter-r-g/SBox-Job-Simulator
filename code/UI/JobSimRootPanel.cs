using JobSim.Job;
using JobSim.Waypoints;
using Sandbox;
using Sandbox.UI;

namespace JobSim.UI
{
	public class JobSimRootPanel : RootPanel
	{
		private JobUIElement currentJobElement = null;
		private MoneyDisplay localMoneyDisplay = null;
		private CharacterScreen characterScreen = null;
		private UseDisplay useDisplay = null;

		public override void Tick()
		{
			if ( Local.Pawn is JobSimPlayer ply )
			{
				if ( localMoneyDisplay == null )
				{
					localMoneyDisplay = AddChild<MoneyDisplay>();
					localMoneyDisplay.MoneyContainer = ply.Money;
				}

				if ( characterScreen == null )
				{
					characterScreen = AddChild<CharacterScreen>();
					characterScreen.Character = ply.Character;
					characterScreen.ItemContainer = ply.Inventory;
					characterScreen.MoneyContainer = ply.Money;
				}

				if ( useDisplay == null )
					useDisplay = AddChild<UseDisplay>();
			}

			base.Tick();
		}

#pragma warning disable IDE0051 // Remove unused private members
		[JobEvent.JobStarted.Client]
		private void JobStarted( JobEventArgs eventArgs )
		{
			currentJobElement = Library.Create<JobUIElement>( eventArgs.Character.Job.UIElementClass );
			currentJobElement.Character = eventArgs.Character;
			currentJobElement.Job = eventArgs.Character.Job;
			eventArgs.Character.Job.UIElement = currentJobElement;

			currentJobElement.Setup();
			AddChild( currentJobElement );
		}

		[JobEvent.JobCompleted.Client]
		[JobEvent.JobFailed.Client]
		private void JobFinished( JobEventArgs _ )
		{
			currentJobElement?.Cleanup();
			currentJobElement?.Delete( true );
		}

		[WaypointEvent.WaypointCreated.Client]
		private void WaypointCreated( WaypointEventArgs eventArgs )
		{
			WaypointDisplay display = AddChild<WaypointDisplay>( "hidden" );
			display.Waypoint = eventArgs.Waypoint;
			eventArgs.Waypoint.Display = display;
		}

		[WaypointEvent.WaypointDeleted.Client]
		private static void WaypointDeleted( WaypointEventArgs eventArgs )
		{
			eventArgs.Waypoint.Display?.Delete( true );
		}
#pragma warning restore IDE0051 // Remove unused private members
	}
}
