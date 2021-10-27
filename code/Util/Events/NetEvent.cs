using Sandbox;
using System;

namespace JobSim
{
	public partial class NetEvent : Entity
	{
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static NetEvent Instance;
#pragma warning restore CA2211 // Non-constant fields should not be visible

		public NetEvent()
		{
			if ( Instance != null )
				throw new Exception( "An instance of NetEvent already exists?" );

			Instance = this;
		}

		public override void Spawn()
		{
			base.Spawn();

			Transmit = TransmitType.Always;
		}

		public void SendEvent( string eventName ) => SendEvent( To.Everyone, eventName );
		public void SendEvent( To to, string eventName )
		{
			Realm.Assert( RealmType.Server );
			ReceiveEventRpc( to, eventName );
		}

#pragma warning disable CA1822 // Mark members as static
		[ClientRpc]
		private void ReceiveEventRpc( string eventName )
		{
			Realm.Assert( RealmType.Client );
			EventRunner.Run( $"net_{eventName}" );
		}
#pragma warning restore CA1822 // Mark members as static
	}
}
