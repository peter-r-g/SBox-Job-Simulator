using JobSim.Units;
using JobSim.Waypoints;
using Sandbox;
using Sandbox.UI;
using System;

namespace JobSim.UI
{
	[UseTemplate]
    public class WaypointDisplay : Panel
    {
		const float DISPLAY_SIZE = 64;
		const float MINIMUM_SCALE = 0.6f;
		const float UPDATE_THRESHOLD = 1;

		public Image MarkerImage { get; set; }
		public Label DistanceLabel { get; set; }

		private Waypoint _waypoint;
		public Waypoint Waypoint
		{
			get { return _waypoint; }
			set
			{
				_waypoint = value;
				UpdateWaypoint();
			}
		}

		private float lastDistance = 0;
		private int fontSize = 0;

		public void UpdateWaypoint()
		{
			MarkerImage.Style.SetBackgroundImage( WaypointManager.GetWaypointTexture( _waypoint.Type, _waypoint.Color ) );

			if ( _waypoint.TextOptions != null )
			{
				DistanceLabel.Style.FontColor = _waypoint.TextOptions.Color;

				DistanceLabel.Style.FontFamily = _waypoint.TextOptions.FontFamily;
				DistanceLabel.Style.FontWeight = _waypoint.TextOptions.FontWeight;
				DistanceLabel.Style.FontSize = _waypoint.TextOptions.FontSize;
				DistanceLabel.Style.Height = _waypoint.TextOptions.FontSize;
				fontSize = _waypoint.TextOptions.FontSize;
			}
		}

		public override void Tick()
		{
			base.Tick();

			if ( _waypoint == null )
			{
				Delete( true );
				return;
			}

			SetClass( "hidden", _waypoint == null || CharacterScreen.Instance.IsVisible );

			DistanceLabel.SetClass( "hidden", _waypoint.TextOptions == null );

			Vector2 screenPos = _waypoint.Position.ToScreen();
			screenPos.x = Math.Max( 0, Math.Min( 1, screenPos.x ) );
			screenPos.y = Math.Max( 0, Math.Min( 1, screenPos.y ) );

			float distance = WaypointManager.DistanceTo( _waypoint );
			float scalePercent = ( 1 - MINIMUM_SCALE ) * ( distance / 4000 );
			float scale = Math.Clamp( MINIMUM_SCALE + scalePercent, MINIMUM_SCALE, 1);
			float halfSize = ( DISPLAY_SIZE * scale ) / 2;

			if ( _waypoint.TextOptions != null && Math.Abs( distance - lastDistance ) > UPDATE_THRESHOLD )
			{
				lastDistance = distance;
				DistanceLabel.Text = UnitFormatter.Format( distance, UnitFormatter.GetDefaultOptions() );
			}

			PanelTransform transform = new();
			transform.AddScale( scale );
			transform.AddTranslateX( Math.Clamp( screenPos.x * Screen.Width - halfSize, 0, Screen.Width - ( DISPLAY_SIZE * scale ) ) );
			transform.AddTranslateY( Math.Clamp( screenPos.y * Screen.Height - halfSize, 0, Screen.Height - ( DISPLAY_SIZE * scale ) - ( fontSize * scale ) ) );
			Style.Transform = transform;
		}
	}
}
