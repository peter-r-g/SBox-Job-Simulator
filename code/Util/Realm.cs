using System;

namespace Sandbox
{
	/// <summary>
	/// Utility class for working with realms.
	/// </summary>
	static class Realm
	{
		/// <summary>
		/// Whether we're in the server realm or not.
		/// </summary>
		public static bool IsServer => Host.IsServer;
		/// <summary>
		/// Whether we're in the client realm or not.
		/// </summary>
		public static bool IsClient => Host.IsClient;
		/// <summary>
		/// Whether we're in the menu realm or not.
		/// </summary>
		public static bool IsMenu => Host.IsMenu;
		/// <summary>
		/// Whether we're in either menu or client realms.
		/// </summary>
		public static bool IsMenuOrClient => Host.IsMenuOrClient;

		/// <summary>
		/// Asserts that the executing code is happening in the passed <see cref="RealmType"/>.
		/// </summary>
		/// <param name="realm">The desired realm to be in.</param>
		public static void Assert( RealmType realm )
		{
			RealmType currentRealm = Get();
			if ( realm != currentRealm )
				throw new RealmException( realm, currentRealm );
		}

		/// <summary>
		/// Asserts that the executing code is happening in the passed <see cref="RealmType"/>
		/// </summary>
		/// <typeparam name="T">The type of value to return back to the code.</typeparam>
		/// <param name="realm">The desired realm to be in.</param>
		/// <param name="returnValue">The value to return back to the executing code.</param>
		/// <returns>See <paramref name="returnValue"/></returns>
		public static T Assert<T>( RealmType realm, T returnValue )
		{
			Assert( realm );
			return returnValue;
		}

		/// <summary>
		/// Gets the realm code is currently executing in.
		/// </summary>
		/// <returns>The realm the code is executing in.</returns>
		public static RealmType Get()
		{
			if ( IsServer )
				return RealmType.Server;
			else if ( IsClient )
				return RealmType.Client;
			else if ( IsMenu )
				return RealmType.Menu;

			return RealmType.Unknown;
		}

		/// <summary>
		/// Wrapper class for <see cref="Sandbox.Log"/> to add support for logging what realm logs are coming from.
		/// </summary>
		public static class Log
		{
			/// <summary>
			/// See <see cref="Sandbox.Log.Error(Exception, string)"/>
			/// </summary>
			public static void Error( Exception e, params object[] inputs ) => Sandbox.Log.Error( e, GetText( inputs ) );
			/// <summary>
			/// See <see cref="Sandbox.Log.Error(Exception)"/>
			/// </summary>
			public static void Error( Exception e ) => Sandbox.Log.Error( e );
			/// <summary>
			/// See <see cref="Sandbox.Log.Error(string)"/>
			/// </summary>
			public static void Error( params object[] inputs ) => Sandbox.Log.Error( GetText( inputs ) );
			/// <summary>
			/// See <see cref="Sandbox.Log.Info(string)"/>
			/// </summary>
			public static void Info( params object[] inputs ) => Sandbox.Log.Info( GetText( inputs ) );
			/// <summary>
			/// See <see cref="Sandbox.Log.Trace(string)"/>
			/// </summary>
			public static void Trace( params object[] inputs ) => Sandbox.Log.Trace( GetText( inputs ) );
			/// <summary>
			/// See <see cref="Sandbox.Log.Warning(string)"/>
			/// </summary>
			public static void Warning( params object[] inputs ) => Sandbox.Log.Warning( GetText( inputs ) );
			/// <summary>
			/// See <see cref="Sandbox.Log.Warning(Exception, string)"/>
			/// </summary>
			public static void Warning( Exception e, params object[] inputs ) => Sandbox.Log.Warning( e, GetText( inputs ) );

			private static string GetText( params object[] inputs )
			{
				string output = "";

				for ( int i = 0; i < inputs.Length; i++ )
				{
					string inputStr;
					if ( inputs[i] == null )
						inputStr = "null";
					else
						inputStr = inputs[i].ToString();

					output += $"{inputStr}\t";
				}

				return $"{Get()}: {output.TrimEnd( '\t' )}";
			}
		}
	}

	/// <summary>
	/// The list of all realms that code can be executed in.
	/// </summary>
	public enum RealmType
	{
		Server,
		Client,
		Menu,
		Unknown
	}

	/// <summary>
	/// The exception that is thrown when <see cref="Realm.Assert{T}(RealmType, T)"/> fails.
	/// </summary>
	public class RealmException : Exception
	{
		public RealmException( RealmType expected, RealmType current ) : base( $"Expected {expected}, got {current}" ) { }
	}
}
