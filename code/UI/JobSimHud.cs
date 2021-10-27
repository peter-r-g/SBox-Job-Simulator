using Sandbox;
using System;

namespace JobSim.UI
{
    public class JobSimHud : HudEntity<JobSimRootPanel>
	{
		public static JobSimHud Instance;

		public JobSimHud()
		{
			if ( Instance != null )
				throw new Exception( "An instance of JobSimHud already exists?" );

			Instance = this;
		}
    }
}
