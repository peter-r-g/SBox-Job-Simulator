using JobSim.Characters;
using JobSim.Job;
using JobSim.Money;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace JobSim.UI
{
	[UseTemplate]
	public class CharacterInfo : Panel
	{
		private ICharacter _character;
		public ICharacter Character
		{
			get { return _character; }
			set
			{
				_character = value;
				RefreshAvatar();
			}
		}

		public IMoneyContainer MoneyContainer { get; set; }

		public ScenePanel AvatarScene { get; set; }
		public Label CharacterName { get; set; }
		public Label CharacterJob { get; set; }
		public Label CharacterMoney { get; set; }
		public Label CharacterHealth { get; set; }

		private SceneWorld AvatarWorld;
		private AnimSceneObject CitizenModel;
		private readonly List<AnimSceneObject> clothingObjects = new();
		private SpotLight LightWarm;

		private Vector3 lookPos;
		private Vector3 headPos;
		private Vector3 aimPos;

		public override void Tick()
		{
			base.Tick();

			CharacterName.Text = Character.Name;

			CharacterJob.SetClass( "disabled", Character is not IEmployableCharacter );
			CharacterMoney.SetClass( "disabled", MoneyContainer == null );
			CharacterHealth.SetClass( "disabled", Character.Entity == null );

			if ( Character is IEmployableCharacter employableCharacter && employableCharacter.Job != JobManager.NullJob )
				CharacterJob.Text = employableCharacter.Job.Name;
			else
				CharacterJob.Text = "None";

			if ( MoneyContainer != null )
				CharacterMoney.Text = MoneyFormat.Format( MoneyContainer.Money );

			if ( Character.Entity is Entity entity )
				CharacterHealth.Text = HealthFormat.Format( entity.Health );

			TickAvatar();
		}

		private void RefreshAvatar()
		{
			if ( AvatarWorld != null )
				return;

			clothingObjects.Clear();
			AvatarWorld = new SceneWorld();

			using ( SceneWorld.SetCurrent( AvatarWorld ) )
			{
				var model = Model.Load( "models/citizen/citizen.vmdl" );
				CitizenModel = new AnimSceneObject( model, Transform.Zero );

				DressModel();

				LightWarm = new SpotLight( Vector3.Up * 100.0f + Vector3.Forward * 100.0f + Vector3.Right * -200, new Color( 1.0f, 0.95f, 0.8f ) * 60.0f );
				LightWarm.Rotation = Rotation.LookAt( -LightWarm.Position );
				LightWarm.SpotCone = new SpotLightCone { Inner = 90, Outer = 90 };

				Angles angles = new( 25, 180, 0 );
				Vector3 pos = Vector3.Up * 56 + angles.Direction * -100;

				AvatarScene.World = AvatarWorld;
				AvatarScene.CameraPosition = pos;
				AvatarScene.CameraRotation = Rotation.From( angles );
				AvatarScene.FieldOfView = 20;
				AvatarScene.AmbientColor = Color.Gray * 0.2f;
			}
		}

		private void TickAvatar()
		{
			if ( CitizenModel == null )
				return;

			// Get mouse position
			var mousePosition = Mouse.Position;

			// subtract what we think is about the player's eye position
			mousePosition.x -= AvatarScene.Box.Rect.width * 0.5f;
			mousePosition.y -= AvatarScene.Box.Rect.height * 0.25f;
			mousePosition /= AvatarScene.ScaleToScreen;

			// convert it to an imaginary world position
			float citizenPosition = 1300;
			var worldpos = new Vector3( 200, mousePosition.x - citizenPosition, -mousePosition.y );

			// convert that to local space for the model
			lookPos = CitizenModel.Transform.PointToLocal( worldpos );
			headPos = Vector3.Lerp( headPos, CitizenModel.Transform.PointToLocal( worldpos ), Time.Delta * 20.0f );
			aimPos = Vector3.Lerp( aimPos, CitizenModel.Transform.PointToLocal( worldpos ), Time.Delta * 5.0f );

			CitizenModel.SetAnimBool( "b_grounded", true );
			CitizenModel.SetAnimVector( "aim_eyes", lookPos );
			CitizenModel.SetAnimVector( "aim_head", headPos );
			CitizenModel.SetAnimVector( "aim_body", aimPos );
			CitizenModel.SetAnimFloat( "aim_body_weight", 1.0f );

			CitizenModel.Update( RealTime.Delta );

			Angles angles = new( 15, 180, 0 );
			Vector3 pos = Vector3.Up * 56 + angles.Direction * -80;

			AvatarScene.CameraPosition = pos;
			AvatarScene.CameraRotation = Rotation.From( angles );

			foreach ( var clothingObject in clothingObjects )
				clothingObject.Update( RealTime.Delta );
		}

		private void DressModel()
		{
			Clothing.Container Container = new();
			Client cl = Character.EntityOwner;
			if ( cl == Local.Client )
				Container.Deserialize( ConsoleSystem.GetValue( "avatar" ) );
			else
				Container.LoadFromClient( cl );

			using ( SceneWorld.SetCurrent( AvatarWorld ) )
			{
				CitizenModel.SetMaterialGroup( "Skin01" );

				foreach ( AnimSceneObject model in clothingObjects )
					model?.Delete();
				clothingObjects.Clear();

				foreach ( Clothing clothingItem in Container.Clothing )
				{
					if ( clothingItem.Model == "models/citizen/citizen.vmdl" )
					{
						CitizenModel.SetMaterialGroup( clothingItem.MaterialGroup );
						continue;
					}

					Model model = Model.Load( clothingItem.Model );
					AnimSceneObject anim = new( model, CitizenModel.Transform );

					if ( !string.IsNullOrEmpty( clothingItem.MaterialGroup ) )
						anim.SetMaterialGroup( clothingItem.MaterialGroup );

					CitizenModel.AddChild( "clothing", anim );
					clothingObjects.Add( anim );

					anim.Update( 1.0f );
				}

				foreach ( (string name, int value) in Container.GetBodyGroups() )
					CitizenModel.SetBodyGroup( name, value );
			}
		}
	}
}
