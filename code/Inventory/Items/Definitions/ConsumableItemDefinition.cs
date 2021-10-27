using Sandbox;
using System;

namespace JobSim.Inventory
{
	public class ConsumableItemDefinition : ItemDefinition
	{
		public Func<IItemContainer, IItem, int> ConsumeFunc { get; set; } = ( user, item ) => 1;

		public override bool IsUsable( IItemContainer container, IItem item ) => true;
		public override (bool, int) OnUse( IItemContainer container, IItem item ) => (true, ConsumeFunc.Invoke( container, item ));
	}
}
