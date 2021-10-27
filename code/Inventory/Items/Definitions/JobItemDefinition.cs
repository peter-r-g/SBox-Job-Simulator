namespace JobSim.Inventory
{
	public class JobItemDefinition : ItemDefinition
	{
		public override bool IsUsable( IItemContainer container, IItem item ) => false;
		public override bool IsDroppable( IItemContainer container, IItem item ) => false;
	}
}
