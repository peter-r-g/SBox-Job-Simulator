using JobSim.Money;
using Sandbox;
using Sandbox.UI;
using System;

namespace JobSim.UI
{
	[UseTemplate]
	public class MoneyDisplay : Panel
	{
		public static MoneyDisplay Instance;

		public Label TotalMoney { get; set; }
		public Label PositiveMoney { get; set; }
		public Label NegativeMoney { get; set; }

		public Panel PositiveMoneyContainer { get; set; }
		public Panel NegativeMoneyContainer { get; set; }

		private IMoneyContainer _moneyContainer;
		public IMoneyContainer MoneyContainer
		{
			get { return _moneyContainer; }
			set
			{
				_moneyContainer = value;
				UpdateMoney( 0 );
			}
		}

		private float currentMoney;
		private float lastPositiveDif;
		private float lastNegativeDif;
		private TimeSince timeSincePositiveShown;
		private TimeSince timeSinceNegativeShown;

		public MoneyDisplay()
		{
			if ( Instance != null )
				throw new Exception( "An instance of MoneyDisplay already exists?" );

			Instance = this;
		}

		public override void Tick()
		{
			base.Tick();

			SetClass( "hidden", CharacterScreen.Instance.IsVisible );
			if ( !IsVisible )
				return;

			PositiveMoneyContainer.SetClass( "hidden", timeSincePositiveShown > 5 );
			NegativeMoneyContainer.SetClass( "hidden", timeSinceNegativeShown > 5 );
		}

		private void UpdateMoney( float delta )
		{
			float newMoney = _moneyContainer.Money;
			if ( delta > 0 )
			{
				if ( timeSincePositiveShown <= 5 )
					delta += lastPositiveDif;
				
				PositiveMoney.Text = $"+{delta}";
				timeSincePositiveShown = 0;
				lastPositiveDif = delta;
			}
			else if ( delta < 0 )
			{
				if ( timeSinceNegativeShown <= 5 )
					delta += lastNegativeDif;

				NegativeMoney.Text = delta.ToString();
				timeSinceNegativeShown = 0;
				lastNegativeDif = delta;
			}

			TotalMoney.Text = MoneyFormat.Format( newMoney );
			currentMoney = newMoney;
		}

		[MoneyEvent.MoneyChanged.Client]
		private void MoneyChanged( MoneyChangedEventArgs eventArgs )
		{
			if ( eventArgs.Container != _moneyContainer )
				return;

			UpdateMoney( eventArgs.Delta );
		}
	}
}
