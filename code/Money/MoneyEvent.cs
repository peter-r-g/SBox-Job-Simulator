using Sandbox;

namespace JobSim.Money
{
	/// <summary>
	/// Events pertaining to money.
	/// </summary>
	static class MoneyEvent
	{
		#region MoneyChanged
		public const string MoneyChangedEvent = "moneyChanged";

		/// <summary>
		/// Called when the money has been changed in an <see cref="IMoneyContainer"/>.
		/// <see cref="MoneyChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public static class MoneyChanged
		{
			public const string ServerEvent = MoneyChangedEvent + ".Server";
			public const string ClientEvent = MoneyChangedEvent + ".Client";

			/// <summary>
			/// Called when the money has been changed in an <see cref="IMoneyContainer"/> only on server.
			/// <see cref="MoneyChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ServerAttribute : EventAttribute
			{
				public ServerAttribute() : base( ServerEvent ) { }
			}

			/// <summary>
			/// Called when the money has been changed in an <see cref="IMoneyContainer"/> only on client.
			/// <see cref="MoneyChangedEventArgs"/> will always be passed to this event.
			/// </summary>
			public class ClientAttribute : EventAttribute
			{
				public ClientAttribute() : base( ClientEvent ) { }
			}
		}

		/// <summary>
		/// Called when the money has been changed in an <see cref="IMoneyContainer"/> on server and client.
		/// <see cref="MoneyChangedEventArgs"/> will always be passed to this event.
		/// </summary>
		public class MoneyChangedAttribute : EventAttribute
		{
			public MoneyChangedAttribute() : base( MoneyChangedEvent ) { }
		}
		#endregion
	}

	#region EventArgs
	class MoneyChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The container that had its money changed.
		/// </summary>
		public IMoneyContainer Container { get; }
		/// <summary>
		/// The amount of money that it was changed by.
		/// </summary>
		public float Delta { get; }

		public MoneyChangedEventArgs( IMoneyContainer container, float delta )
		{
			Container = container;
			Delta = delta;
		}
	}
	#endregion
}
