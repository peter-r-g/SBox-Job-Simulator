using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Money;
using Sandbox;

namespace JobSim
{
	public static class JobSimLocal
	{
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static IEmployableCharacter Character = (Local.Pawn as JobSimPlayer)?.Character;
		public static IItemContainer ItemContainer = (Local.Pawn as JobSimPlayer)?.Inventory;
		public static IMoneyContainer MoneyContainer = (Local.Pawn as JobSimPlayer)?.Money;
#pragma warning restore CA2211 // Non-constant fields should not be visible
	}
}
