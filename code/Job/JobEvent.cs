using JobSim.Characters;
using Sandbox;

namespace JobSim.Job
{
	/// <summary>
	/// Events pertaining to jobs.
	/// </summary>
	static class JobEvent
	{
		#region JobStarted
		public const string JobStartedEvent = "jobStarted";

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has started a <see cref="Job"/>.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class JobStarted
		{
			public const string ServerEvent = JobStartedEvent + ".Server";
			public const string ClientEvent = JobStartedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has started a <see cref="Job"/> only on server.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has started a <see cref="Job"/> only on client.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has started a <see cref="Job"/> on server and client.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public class JobStartedAttribute : EventAttribute
		{
			public JobStartedAttribute() : base( JobStartedEvent ) { }
		}
		#endregion

		#region JobCompleted
		public const string JobCompletedEvent = "jobCompleted";

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/>.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class JobCompleted
		{
			public const string ServerEvent = JobCompletedEvent + ".Server";
			public const string ClientEvent = JobCompletedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> only on server.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> only on client.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> on server and client.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public class JobCompletedAttribute : EventAttribute
		{
			public JobCompletedAttribute() : base( JobCompletedEvent ) { }
		}
		#endregion

		#region JobFailed
		public const string JobFailedEvent = "jobFailed";

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has failed a <see cref="Job"/>.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class JobFailed
		{
			public const string ServerEvent = JobFailedEvent + ".Server";
			public const string ClientEvent = JobFailedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> only on server.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> only on client.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has completed a <see cref="Job"/> on server and client.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public class JobFailedAttribute : EventAttribute
		{
			public JobFailedAttribute() : base( JobFailedEvent ) { }
		}
		#endregion

		#region JobStageCompleted
		public const string JobStageCompletedEvent = "jobStageCompleted";

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has completed a stage of a <see cref="Job"/>.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class JobStageCompleted
		{
			public const string ServerEvent = JobStageCompletedEvent + ".Server";
			public const string ClientEvent = JobStageCompletedEvent + ".Client";

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a stage of a <see cref="Job"/> only on server.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when a <see cref="IEmployableCharacter"/> has completed a stage of a <see cref="Job"/> only on client.
			/// <see cref="JobEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when a <see cref="IEmployableCharacter"/> has completed a stage of a <see cref="Job"/> on server and client.
		/// <see cref="JobEventArgs"/> will always be passed to this event.
		/// </summary>
		public class JobStageCompletedAttribute : EventAttribute
		{
			public JobStageCompletedAttribute() : base( JobStageCompletedEvent ) { }
		}
		#endregion
	}

	#region EventArgs
	class JobEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="IEmployableCharacter"/> pertaining to the event called.
		/// </summary>
		public IEmployableCharacter Character { get; }

		public JobEventArgs( IEmployableCharacter character )
		{
			Character = character;
		}
	}
	#endregion
}
