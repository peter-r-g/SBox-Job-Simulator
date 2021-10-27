using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Money;
using Sandbox;

namespace JobSim
{
    public partial class JobSimPlayer : Player
	{
		[Net, Local]
		public NetworkedEmployableCharacter Character { get; private set; }
		[Net, Local]
		public new NetworkedItemContainer Inventory { get; private set; }
		[Net, Local]
		public NetworkedMoneyContainer Money { get; private set; }

		public override void Respawn()
		{
			base.Respawn();

			SetModel( "models/citizen/citizen.vmdl" );

			Camera = new ThirdPersonCamera();
			Controller = new JobSimPlayerController();
			Animator = new StandardPlayerAnimator();

			Character.Clothes.DressEntity( this );

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( Camera is FirstPersonCamera )
					Camera = new ThirdPersonCamera();
				else
					Camera = new FirstPersonCamera();
			}

			if ( Input.Pressed( InputButton.Use ) )
				TickPlayerUse();

			if ( Input.Pressed( InputButton.Slot1 ) )
				Inventory.TryUseItem( 0 );
			else if ( Input.Pressed( InputButton.Slot2 ) )
				Inventory.TryUseItem( 1 );
			else if ( Input.Pressed( InputButton.Slot3 ) )
				Inventory.TryUseItem( 2 );
			else if ( Input.Pressed( InputButton.Slot4 ) )
				Inventory.TryUseItem( 3 );
			else if ( Input.Pressed( InputButton.Slot5 ) )
				Inventory.TryUseItem( 4 );
			else if ( Input.Pressed( InputButton.Slot6 ) )
				Inventory.TryUseItem( 5 );
			else if ( Input.Pressed( InputButton.Slot7 ) )
				Inventory.TryUseItem( 6 );
			else if ( Input.Pressed( InputButton.Slot8 ) )
				Inventory.TryUseItem( 7 );
			else if ( Input.Pressed( InputButton.Slot9 ) )
				Inventory.TryUseItem( 8 );
			else if ( Input.Pressed( InputButton.Slot0 ) )
				Inventory.TryUseItem( 9 );

			if ( Input.ActiveChild != null )
				ActiveChild = Input.ActiveChild;

			SimulateActiveChild( cl, ActiveChild );
		}

		public Entity FindUsableEntity()
		{
			return FindUsable();
		}
	}
}
