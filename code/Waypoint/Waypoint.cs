using JobSim.UI;
using Sandbox;

namespace JobSim.Waypoints
{
	public class Waypoint
	{
		public WaypointDisplay Display { get; set; } = null;

		public Vector3 Position { get; set; } = Vector3.Zero;
		public string Description { get; set; } = "None provided";
		public WaypointType Type { get; set; } = WaypointType.Position;
		public WaypointColor Color { get; set; } = WaypointColor.White;
		public WaypointTextOptions TextOptions { get; set; } = null;

		public Waypoint()
		{
			Realm.Assert( RealmType.Client );

			WaypointManager.Add( this );
		}

        public Waypoint WithPosition( Vector3 pos )
		{
			Position = pos;
			return this;
		}

		public Waypoint WithDescription( string description )
		{
			Description = description;
			return this;
		}

		public Waypoint WithType( WaypointType waypointType )
		{
			Type = waypointType;
			return this;
		}

		public Waypoint WithColor( WaypointColor waypointColor )
		{
			Color = waypointColor;
			return this;
		}

		public Waypoint WithTextOptions( WaypointTextOptions textOptions )
		{
			TextOptions = textOptions;
			return this;
		}

		public Waypoint WithTextOptions( Color color, string fontFamily = "Arial", int fontSize = 20, int fontWeight = 700 )
		{
			TextOptions = new()
			{
				Color = color,
				FontFamily = fontFamily,
				FontSize = fontSize,
				FontWeight = fontWeight
			};
			return this;
		}

		public void Delete()
		{
			WaypointManager.Remove(this);
		}
	}

	public class WaypointTextOptions
	{
		public Color Color { get; set; } = Color.White;
		public string FontFamily { get; set; } = "Arial";
		public int FontSize { get; set; } = 20;
		public int FontWeight { get; set; } = 700;
	}
}
