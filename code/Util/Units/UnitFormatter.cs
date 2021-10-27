using Sandbox;
using System;

namespace JobSim.Units
{
	static class UnitFormatter
	{
		[ClientVar( "unit_system", Help = "The unit system you'd prefer to see. Options are: metric, imperial, and source", Saved = true )]
		public static string PreferredUnitSystem { get; set; } = DefaultUnitSystem;
		public const string DefaultUnitSystem = "source";

		public static string Format( double value, UnitFormatterOptions options = null )
		{
			if ( options == null )
				options = GetDefaultOptions();

			if ( options.OutputType != null )
				return FormatString( UnitConverter.Convert( value, options.InputType, options.OutputType.Value ), options.OutputType.Value, options.AppendShortForm, options.Decimals );
			
			UnitSystem inputSystem = Unit.All[options.InputType].System;
			UnitSystem outputSystem;
			if ( options.OutputSystem == null )
				outputSystem = inputSystem;
			else
				outputSystem = options.OutputSystem.Value;

			switch ( outputSystem )
			{
				case UnitSystem.Imperial:
				case UnitSystem.Metric:
					Tuple<UnitType, double> values = ConvertValue( value, options.InputType, outputSystem );
					return FormatString( values.Item2, values.Item1, options.AppendShortForm, options.Decimals );
				case UnitSystem.Source:
					if ( options.InputType == UnitType.SourceUnit )
						return FormatString( value, UnitType.SourceUnit, options.AppendShortForm, options.Decimals );

					return FormatString( UnitConverter.Convert( value, options.InputType, UnitType.SourceUnit ), UnitType.SourceUnit, options.AppendShortForm, options.Decimals );
				default:
					throw new Exception( "This should be unreachable" );
			}
		}
		public static string Format( float value, UnitFormatterOptions options = null ) => Format( (double)value, options );

		private static string FormatString( double value, UnitType unitType, bool appendShortForm, int decimals )
		{
			double formattedValue = Math.Round( value, decimals );
			return $"{formattedValue}{(appendShortForm ? Unit.All[unitType].Name.ShortForm : "")}";
		}

		private static Tuple<UnitType, double> ConvertValue( double value, UnitType inputType, UnitSystem outputSystem )
		{
			UnitType outputType = outputSystem switch
			{
				UnitSystem.Imperial => UnitType.Inch,
				UnitSystem.Metric => UnitType.NanoMetre,
				_ => throw new Exception( "This should be unreachable" ),
			};

			double currentValue = UnitConverter.Convert( value, inputType, outputType );
			while ( currentValue > 1 )
			{
				UnitType nextUnitType = Unit.All[outputType].NextType;
				if ( outputType == nextUnitType )
					break;

				double nextCurrentValue = UnitConverter.Convert( currentValue, outputType, nextUnitType );
				if ( nextCurrentValue < 1 )
					break;

				outputType = nextUnitType;
				currentValue = nextCurrentValue;
			}

			return new Tuple<UnitType, double>( outputType, currentValue );
		}

		public static UnitFormatterOptions GetDefaultOptions()
		{
			return new UnitFormatterOptions()
			{
				InputType = UnitType.SourceUnit,
				OutputType = null,
				OutputSystem = GetPreferredUnitSystem(),
				Decimals = 0,
				AppendShortForm = true
			};
		}

		public static UnitSystem GetPreferredUnitSystem()
		{
			if ( PreferredUnitSystem != "metric" && PreferredUnitSystem != "imperial" && PreferredUnitSystem != "source" )
			{
				Realm.Log.Warning( $"Got invalid unit system from client: {PreferredUnitSystem}, resetting to default ({DefaultUnitSystem})" );
				PreferredUnitSystem = DefaultUnitSystem;
			}

			return Enum.Parse<UnitSystem>( PreferredUnitSystem.ToTitleCase() );
		}
	}

	public class UnitFormatterOptions
	{
		public UnitType InputType { set;  get; }
		public UnitType? OutputType { get; set; }
		public UnitSystem? OutputSystem { set; get; }
		public int Decimals { set; get; }
		public bool AppendShortForm { get; set; }
	}
}
