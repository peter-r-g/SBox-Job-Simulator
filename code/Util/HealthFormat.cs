namespace JobSim
{
	static class HealthFormat
	{
		public static string Format( float health )
		{
			if ( health <= 0 )
				return "Dead";
			else if ( health <= 20 )
				return "Very Unhealthy";
			else if ( health <= 50 )
				return "Normal";
			else if ( health <= 80 )
				return "Healthy";
			else
				return "Very Healthy";
		}
	}
}
