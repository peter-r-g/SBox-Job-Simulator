using System.IO;

namespace JobSim
{
	public interface IPersistent : IUnique
	{
		/// <summary>
		/// Whether or not this item should be saved across sessions.
		/// </summary>
		public bool IsPersistent { get; }

		/// <summary>
		/// Serializes all data that this persistent class has.
		/// </summary>
		public PersistentData SerializeData();
	}
}
