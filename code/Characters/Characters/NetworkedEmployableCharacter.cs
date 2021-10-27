using JobSim.Job;
using Sandbox;

namespace JobSim.Characters
{
	public partial class NetworkedEmployableCharacter : NetworkedCharacter, IEmployableCharacter
	{
		[Net]
		public int JobID { get; set; }
		[Net]
		public int JobStage { get; set; }
		public BaseJob Job => BaseJob.All[JobID];

		~NetworkedEmployableCharacter()
		{
			IEmployableCharacter.All.Remove( this );
		}

		public NetworkedEmployableCharacter()
		{
			ICharacter.All.Remove( this );
			IEmployableCharacter.All.Add( this );
		}

		public NetworkedEmployableCharacter( string uuid, string name, Entity entity, Clothing.Container clothes, int jobId, int jobStage )
			: base( uuid, name, entity, clothes )
		{
			JobID = jobId;
			JobStage = jobStage;

			ICharacter.All.Remove( this );
			IEmployableCharacter.All.Add( this );
		}

		public NetworkedEmployableCharacter( Entity entity, EmployableCharacterPersistentData cData )
			: this( cData.UUID, cData.Name, entity, cData.Clothes, cData.JobID, cData.JobStage )
		{
			IsPersistent = true;
		}

		public override PersistentData SerializeData() => new EmployableCharacterPersistentData( this );
	}
}
