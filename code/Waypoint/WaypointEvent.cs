using Sandbox;

namespace JobSim.Waypoints
{
	/// <summary>
	/// Events pertaining to waypoints.
	/// </summary>
	static class WaypointEvent
	{
		#region WaypointCreated
		public const string WaypointCreatedEvent = "waypointCreated";

		/// <summary>
		/// Called when a <see cref="Waypoint"/> has been created.
		/// <see cref="WaypointEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class WaypointCreated
		{
			public const string ClientEvent = WaypointCreatedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="Waypoint"/> has been created only on client.
			/// <see cref="WaypointEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="Waypoint"/> has been created only on client.
		/// <see cref="WaypointEventArgs"/> will always be passed to this event.
		/// </summary>
		public class WaypointCreatedAttribute : EventAttribute
		{
			public WaypointCreatedAttribute() : base( WaypointCreatedEvent ) { }
		}
		#endregion

		#region WaypointDeleted
		public const string WaypointDeletedEvent = "waypointDeleted";

		/// <summary>
		/// Called when a <see cref="Waypoint"/> has been deleted.
		/// <see cref="WaypointEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class WaypointDeleted
		{
			public const string ClientEvent = WaypointDeletedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="Waypoint"/> has been deleted only on client.
			/// <see cref="WaypointEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="Waypoint"/> has been deleted only on client.
		/// <see cref="WaypointEventArgs"/> will always be passed to this event.
		/// </summary>
		public class WaypointDeletedAttribute : EventAttribute
		{
			public WaypointDeletedAttribute() : base( WaypointDeletedEvent ) { }
		}
		#endregion
	}

	#region EventArgs
	class WaypointEventArgs : EventArgs
	{
		/// <summary>
		/// The waypoint that was created/deleted.
		/// </summary>
		public Waypoint Waypoint { get; }

		public WaypointEventArgs( Waypoint waypoint )
		{
			Waypoint = waypoint;
		}
	}
	#endregion
}
