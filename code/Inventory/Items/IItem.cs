using Sandbox;

namespace JobSim.Inventory
{
	/// <summary>
	/// Represents an item in the game world.
	/// </summary>
	public interface IItem
	{
		/// <summary>
		/// The unique number that represents the item this is.
		/// </summary>
		public uint ID { get; set; }
		/// <summary>
		/// The amount of the same item is in this.
		/// </summary>
		public int Stack { get; set; }
		/// <summary>
		/// The slot of the container this item is in.
		/// </summary>
		public int Slot { get; set; }
		/// <summary>
		/// The definition of this item.
		/// </summary>
		public ItemDefinition Definition { get; }

		/// <summary>
		/// Whether or not this is a null item.
		/// </summary>
		public bool IsNull => ItemManager.IsNull( ID );

		/// <summary>
		/// Clones this item into a new instance.
		/// </summary>
		/// <returns>The new instance of <see cref="IItem"/></returns>
		public IItem Clone();

		/// <summary>
		/// Reads all data the item needs from the network.
		/// </summary>
		/// <param name="reader">The reader instance to read from.</param>
		public void NetRead( ref NetRead reader );
		/// <summary>
		/// Writes all data the item needs to send over the network.
		/// </summary>
		/// <param name="writer">The writer that's sending data over the network.</param>
		public void NetWrite( NetWrite writer );
	}
}
