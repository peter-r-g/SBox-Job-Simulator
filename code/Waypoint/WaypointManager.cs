using Sandbox;
using System;
using System.Collections.Generic;

namespace JobSim.Waypoints
{
	static class WaypointManager
	{
		public static List<Waypoint> All = new();

		private static readonly Dictionary<WaypointType, Dictionary<WaypointColor, Texture>> WaypointTextures = new();

		public static void LoadTextures()
		{
			Realm.Assert( RealmType.Client );

			foreach ( KeyValuePair<WaypointType, Dictionary<WaypointColor, Texture>> textureDict in WaypointTextures )
				textureDict.Value.Clear();
			WaypointTextures.Clear();

			foreach ( WaypointType type in Enum.GetValues<WaypointType>() )
			{
				WaypointTextures.Add( type, new() );
				string typeName = Enum.GetName( type ).ToLower();

				foreach ( WaypointColor color in Enum.GetValues<WaypointColor>() )
				{
					string colorName = Enum.GetName( color ).ToLower();
					WaypointTextures[type].Add( color, Texture.Load( $"/textures/ui/waypoint/{typeName}/{colorName}.png" ) );
				}
			}
		}

		public static void Add( Waypoint waypoint )
		{
			All.Add( waypoint );
			EventRunner.Run( WaypointEvent.WaypointCreatedEvent, new WaypointEventArgs( waypoint ) );
		}

		public static void Remove( Waypoint waypoint )
		{
			All.Remove( waypoint );
			EventRunner.Run( WaypointEvent.WaypointDeletedEvent, new WaypointEventArgs( waypoint ) );
		}

		public static void ClearWaypoints()
		{
			while ( All.Count > 0 )
				All[0].Delete();
		}

		public static float DistanceTo( Waypoint waypoint ) => Realm.Assert( RealmType.Client, waypoint.Position.Distance( Local.Pawn.Position ) );
		public static float DistanceTo( Waypoint waypoint, Entity target ) => Realm.Assert( RealmType.Client, waypoint.Position.Distance( target.Position ) );
		public static float DistanceTo( Waypoint waypoint, Vector3 position ) => Realm.Assert( RealmType.Client, waypoint.Position.Distance( position ) );
		public static Texture GetWaypointTexture( WaypointType type, WaypointColor color ) => Realm.Assert( RealmType.Client, WaypointTextures[type][color] );
	}

	public enum WaypointType
	{
		Hazard,
		House,
		Position
	}

	public enum WaypointColor
	{
		Blue,
		Green,
		Red,
		Yellow,
		White
	}
}
