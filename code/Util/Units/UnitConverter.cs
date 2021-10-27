using System.Collections.Generic;

namespace JobSim.Units
{
	static class UnitConverter
	{
		public static double Convert( double value, UnitType fromUnit, UnitType toUnit ) => Unit.All[fromUnit].Convert( value, Unit.All[toUnit] );
		public static double Convert( double value, string fromUnitName, string toUnitName )
		{
			Unit fromUnit = null;
			Unit toUnit = null;

			foreach ( KeyValuePair<UnitType, Unit> pair  in Unit.All )
			{
				Name unitName = pair.Value.Name;

				if ( unitName.Single == fromUnitName || unitName.Plural == fromUnitName || unitName.ShortForm == fromUnitName )
					fromUnit = pair.Value;
				else if ( unitName.Single == toUnitName || unitName.Plural == toUnitName || unitName.ShortForm == toUnitName )
					toUnit = pair.Value;

				if ( fromUnit != null && toUnit != null )
					break;
			}

			return fromUnit.Convert( value, toUnit );
		}
		public static float Convert( float value, UnitType fromUnit, UnitType toUnit ) => (float)Convert( (double)value, fromUnit, toUnit );
		public static float Convert( float value, string fromUnit, string toUnit ) => (float)Convert( (double)value, fromUnit, toUnit );
	}
}
