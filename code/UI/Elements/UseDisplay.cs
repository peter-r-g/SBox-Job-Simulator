using Sandbox;
using Sandbox.UI;
using System;

namespace JobSim.UI
{
	[UseTemplate]
	public class UseDisplay : Panel
	{
		const float DISPLAY_SIZE = 100;
		const float HALF_DISPLAY_SIZE = DISPLAY_SIZE / 2;
		const float FONT_SIZE = 32;

		public Label Text { get; set; }

		private ModelEntity lastModelEntity;

		public override void Tick()
		{
			base.Tick();

			Entity usableEntity = (Local.Pawn as JobSimPlayer).FindUsableEntity();
			SetClass( "hidden", usableEntity == null || CharacterScreen.Instance.IsVisible );

			if ( usableEntity == null || !usableEntity.IsValid )
			{
				if ( lastModelEntity != null )
				{
					if ( lastModelEntity.IsValid )
					{
						lastModelEntity.GlowState = GlowStates.GlowStateOff;
						lastModelEntity.GlowColor = Color.Transparent;
					}
					
					lastModelEntity = null;
				}

				return;
			}
			
			if ( usableEntity is ModelEntity model )
			{
				if ( lastModelEntity != null && model != lastModelEntity && lastModelEntity.IsValid )
				{
					lastModelEntity.GlowState = GlowStates.GlowStateOff;
					lastModelEntity.GlowColor = Color.Transparent;
				}

				lastModelEntity = model;
				model.GlowState = GlowStates.GlowStateOn;
				model.GlowColor = Color.Green;
			}

			Vector2 screenPos = usableEntity.Position.ToScreen();
			screenPos.x = Math.Max( 0, Math.Min( 1, screenPos.x ) );
			screenPos.y = Math.Max( 0, Math.Min( 1, screenPos.y ) );

			PanelTransform transform = new();
			transform.AddTranslateX( Math.Clamp( screenPos.x * Screen.Width - HALF_DISPLAY_SIZE, 0, Screen.Width - (DISPLAY_SIZE) ) );
			transform.AddTranslateY( Math.Clamp( screenPos.y * Screen.Height - HALF_DISPLAY_SIZE, 0, Screen.Height - (DISPLAY_SIZE) - FONT_SIZE ) );
			Style.Transform = transform;
		}
	}
}
