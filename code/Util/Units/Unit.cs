using System.Collections.Generic;

namespace JobSim.Units
{
	class Unit
	{
		public Name Name { get; }
		public double Ratio { get; }
		public UnitType Type { get; }
		public UnitType PreviousType { get; }
		public UnitType NextType { get; }
		public UnitSystem System { get; }

		public Unit( Name name, double ratio, UnitType type, UnitType previousType, UnitType nextType, UnitSystem system )
		{
			Name = name;
			Ratio = ratio;
			Type = type;
			PreviousType = previousType;
			NextType = nextType;
			System = system;
		}

		public double Convert( double value, Unit targetUnit ) => (value * Ratio) / targetUnit.Ratio;

		public static readonly Dictionary<UnitType, Unit> All = new()
		#region Unit Definitions
		{
			{
				UnitType.NanoMetre,
				new Unit( new Name()
				{
					Single = "nanometre",
					Plural = "nanometres",
					ShortForm = "nm"
				}, 1e-9, UnitType.NanoMetre, UnitType.NanoMetre, UnitType.MicroMetre, UnitSystem.Metric )
			},
			{
				UnitType.MicroMetre,
				new Unit( new Name()
				{
					Single = "micrometre",
					Plural = "micrometres",
					ShortForm = "μm"
				}, 1e-6, UnitType.MicroMetre, UnitType.NanoMetre, UnitType.Milimetre, UnitSystem.Metric )
			},
			{
				UnitType.Milimetre,
				new Unit( new Name()
				{
					Single = "milimetre",
					Plural = "milimetres",
					ShortForm = "mm"
				}, 1e-3, UnitType.Milimetre, UnitType.MicroMetre, UnitType.Centimeter, UnitSystem.Metric )
			},
			{
				UnitType.Centimeter,
				new Unit( new Name()
				{
					Single = "centimeter",
					Plural = "centimeters",
					ShortForm = "cm"
				}, 1e-2, UnitType.Centimeter, UnitType.Milimetre, UnitType.Meter, UnitSystem.Metric )
			},
			{
				UnitType.Meter,
				new Unit( new Name()
				{
					Single = "meter",
					Plural = "meters",
					ShortForm = "m"
				}, 1, UnitType.Meter, UnitType.Centimeter, UnitType.Kilometer, UnitSystem.Metric )
			},
			{
				UnitType.Kilometer,
				new Unit( new Name()
				{
					Single = "kilometer",
					Plural = "kilometers",
					ShortForm = "km"
				}, 1e+3, UnitType.Kilometer, UnitType.Meter, UnitType.Kilometer, UnitSystem.Metric )
			},

			{
				UnitType.Inch,
				new Unit( new Name()
				{
					Single = "inch",
					Plural = "inches",
					ShortForm = "in"
				}, 0.0254, UnitType.Inch, UnitType.Inch, UnitType.Foot, UnitSystem.Imperial )
			},
			{
				UnitType.Foot,
				new Unit( new Name()
				{
					Single = "foot",
					Plural = "feet",
					ShortForm = "ft"
				}, 0.3048, UnitType.Foot, UnitType.Inch, UnitType.Yard, UnitSystem.Imperial )
			},
			{
				UnitType.Yard,
				new Unit( new Name()
				{
					Single = "yard",
					Plural = "yards",
					ShortForm = "yd"
				}, 0.9144, UnitType.Yard, UnitType.Foot, UnitType.Mile, UnitSystem.Imperial )
			},
			{
				UnitType.Mile,
				new Unit( new Name()
				{
					Single = "mile",
					Plural = "miles",
					ShortForm = "mi"
				}, 1609.34, UnitType.Mile, UnitType.Yard, UnitType.Mile, UnitSystem.Imperial )
			},

			{
				UnitType.SourceUnit,
				new Unit( new Name()
				{
					Single = "source unit",
					Plural = "source units",
					ShortForm = "su"
				}, 0.0254, UnitType.SourceUnit, UnitType.SourceUnit, UnitType.SourceUnit, UnitSystem.Source )
			}
		};
		#endregion
	}

	struct Name
	{
		public string Single;
		public string Plural;
		public string ShortForm;
	}

	public enum UnitType
	{
		NanoMetre,
		MicroMetre,
		Milimetre,
		Centimeter,
		Meter,
		Kilometer,

		Inch,
		Foot,
		Yard,
		Mile,

		SourceUnit
	}

	public enum UnitSystem
	{
		Metric,
		Imperial,
		Source
	}
}
