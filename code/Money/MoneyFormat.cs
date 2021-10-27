using System;

namespace JobSim.Money
{
	/// <summary>
	/// Utility class for formatting money amounts.
	/// </summary>
	static class MoneyFormat
	{
		/// <summary>
		/// The currency to use.
		/// </summary>
		public const string CURRENCY = "S&Buck";

		/// <summary>
		/// Helper function to nicely format a money amount.
		/// </summary>
		/// <param name="money">The money to format.</param>
		/// <returns>The formatted money.</returns>
		public static string Format( float money )
		{
			string currency = CURRENCY;
			if ( Math.Abs( money ) != 1 )
				currency += "s";

			return $"{money} {currency}";
		}
	}
}
