using JobSim.Job;
using Sandbox;
using System.IO;

namespace JobSim.Characters
{
	public class EmployableCharacter : Character, IEmployableCharacter
	{
		public int JobID { get; set; }
		public int JobStage { get; set; }
		public BaseJob Job => BaseJob.All[JobID];

		~EmployableCharacter()
		{
			IEmployableCharacter.All.Remove( this );
		}

		public EmployableCharacter( string uuid, string name, Entity entity, Clothing.Container clothes, int jobId, int jobStage )
			: base( uuid, name, entity, clothes )
		{
			JobID = jobId;
			JobStage = jobStage;

			ICharacter.All.Remove( this );
			IEmployableCharacter.All.Add( this );
		}

		public EmployableCharacter( Entity entity, EmployableCharacterPersistentData cData )
			: this( cData.UUID, cData.Name, entity, cData.Clothes, cData.JobID, cData.JobStage )
		{
			IsPersistent = true;
		}

		public override PersistentData SerializeData() => new EmployableCharacterPersistentData( this );
	}
}
