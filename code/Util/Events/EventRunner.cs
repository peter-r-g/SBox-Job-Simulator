using Sandbox;

namespace JobSim
{
	static class EventRunner
	{
		public static EventArgs Run( string eventName ) => Run( eventName, new EventArgs() );
		public static T Run<T>( string eventName, T eventArgs ) where T : EventArgs
		{
			Event.Run( eventName, eventArgs );
			Event.Run( $"{eventName}.{Realm.Get()}", eventArgs );
			return eventArgs;
		}
	}
}
