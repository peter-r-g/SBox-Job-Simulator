using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Money;
using Sandbox;
using Sandbox.UI;
using System;

namespace JobSim.UI
{
	[UseTemplate]
	public class CharacterScreen : Panel
	{
		public static CharacterScreen Instance;

		private ICharacter _character;
		public ICharacter Character
		{
			get { return _character; }
			set
			{
				_character = value;
				CharacterInfo.Character = _character;
			}
		}

		private IItemContainer _itemContainer;
		public IItemContainer ItemContainer
		{
			get { return _itemContainer; }
			set
			{
				_itemContainer = value;
				InventoryDisplay.Container = _itemContainer;
			}
		}

		private IMoneyContainer _moneyContainer;
		public IMoneyContainer MoneyContainer
		{
			get { return _moneyContainer; }
			set
			{
				_moneyContainer = value;
				CharacterInfo.MoneyContainer = _moneyContainer;
			}
		}

		public InventoryDisplay InventoryDisplay { get; set; }
		public CharacterInfo CharacterInfo { get; set; }

		public CharacterScreen()
		{
			if ( Instance != null )
				throw new Exception( "An instance of CharacterScreen already exists?" );

			Instance = this;
		}

		public override void Tick()
		{
			SetClass( "open", Input.Down( InputButton.Score ) );

			if ( !IsVisible )
				return;

			base.Tick();
		}
	}
}
