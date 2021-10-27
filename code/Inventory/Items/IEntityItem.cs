using Sandbox;

namespace JobSim.Inventory
{
	public interface IEntityItem : IItem
	{
		/// <summary>
		/// The entity that is made from this item.
		/// </summary>
		public Entity Entity { get; set; }
	}
}
