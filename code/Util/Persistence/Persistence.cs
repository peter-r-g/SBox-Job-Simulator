using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Money;
using Sandbox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JobSim
{
	public enum DataType
	{
		EmployableCharacter,
		Character,
		ItemContainer,
		MoneyContainer
	}

	public static class Persistence
	{
		public const long CurrentFileVersion = 0;
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static long LoadedFileVersion = CurrentFileVersion;
#pragma warning restore CA2211 // Non-constant fields should not be visible

		[ServerVar( "jobsim_save_name", Saved = true )]
		public static string FileName { get; set; } = "save";
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static WorldConfiguration WorldData;
#pragma warning restore CA2211 // Non-constant fields should not be visible

		public static IReadOnlyDictionary<DataType, Dictionary<string, PersistentData>> GameData => gameData;
		private static readonly Dictionary<DataType, Dictionary<string, PersistentData>> gameData = new();
		private static readonly Dictionary<DataType, IReadOnlyList<IPersistent>> dataPoints = new()
		{
			{ DataType.Character, ICharacter.All },
			{ DataType.EmployableCharacter, IEmployableCharacter.All },
			{ DataType.ItemContainer, IItemContainer.All },
			{ DataType.MoneyContainer, IMoneyContainer.All }
		};

		public static void Init()
		{
			gameData[DataType.Character] = new();
			gameData[DataType.EmployableCharacter] = new();
			gameData[DataType.ItemContainer] = new();
			gameData[DataType.MoneyContainer] = new();

			LoadFileVersion();
			LoadWorld();
		}

		public static PersistentData GetData( DataType type, string uuid )
		{
			Realm.Assert( RealmType.Server );

			if ( GameData[type].ContainsKey( uuid ) )
				return GameData[type][uuid];

			return null;
		}

		private static void LoadFileVersion()
		{
			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				return;

			using BinaryReader reader = new( FileSystem.Data.OpenRead( $"{FileName}/version", FileMode.Open ) );
			LoadedFileVersion = reader.ReadInt64();
		}

		private static void LoadWorld()
		{
			WorldData = new();
			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				return;

			using BinaryReader reader = new( FileSystem.Data.OpenRead( $"{FileName}/world", FileMode.Open ) );
			WorldData.ReadData( reader );
		}

		public static void Load( string uuid )
		{
			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				return;

			foreach ( DataType type in (DataType[])Enum.GetValues( typeof( DataType ) ) )
			{
				foreach ( string file in FileSystem.Data.FindFile( $"{FileName}/{type}", uuid ) )
				{
					using BinaryReader reader = new( FileSystem.Data.OpenRead( $"{FileName}/{type}/{file}" ) );
					PersistentData data = Library.Create<PersistentData>( reader.ReadString() );
					data.ReadData( reader );
					gameData[type].Add( data.UUID, data );
				}
			}
		}

		private static void SaveVersion()
		{
			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				FileSystem.Data.CreateDirectory( FileName );

			using BinaryWriter writer = new( FileSystem.Data.OpenWrite( $"{FileName}/version", FileMode.Create ) );
			writer.Write( CurrentFileVersion );
		}

		private static void SaveWorld()
		{
			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				FileSystem.Data.CreateDirectory( FileName );

			using BinaryWriter writer = new( FileSystem.Data.OpenWrite( $"{FileName}/world", FileMode.Create ) );
			WorldData.WriteData( writer );
		}

		public static void Save( string uuid )
		{
			foreach ( DataType type in (DataType[])Enum.GetValues( typeof( DataType ) ) )
			{
				if ( !FileSystem.Data.DirectoryExists( $"{FileName}/{type}" ) )
					FileSystem.Data.CreateDirectory( $"{FileName}/{type}" );

				if ( dataPoints[type].FirstOrDefault( ( item ) => item.UUID == uuid ) is IPersistent item )
				{
					using BinaryWriter writer = new( FileSystem.Data.OpenWrite( $"{FileName}/{type}/{uuid}" ) );
					item.SerializeData().WriteData( writer );
				}
			}
		}

		public static void SaveAndClear( string uuid )
		{
			Save( uuid );
			foreach ( DataType type in (DataType[])Enum.GetValues( typeof( DataType ) ) )
				gameData[type].Remove( uuid );
		}

		public static void SaveAll()
		{
			Realm.Assert( RealmType.Server );

			if ( !FileSystem.Data.DirectoryExists( FileName ) )
				FileSystem.Data.CreateDirectory( FileName );

			SaveVersion();
			SaveWorld();

			foreach ( DataType type in (DataType[])Enum.GetValues( typeof( DataType ) ) )
			{
				if ( !FileSystem.Data.DirectoryExists( $"{FileName}/{type}" ) )
					FileSystem.Data.CreateDirectory( $"{FileName}/{type}" );

				foreach ( IPersistent dataPoint in dataPoints[type] )
				{
					using BinaryWriter writer = new( FileSystem.Data.OpenWrite( $"{FileName}/{type}/{dataPoint.UUID}" ) );
					dataPoint.SerializeData().WriteData( writer );
				}
			}
		}
	}
}
