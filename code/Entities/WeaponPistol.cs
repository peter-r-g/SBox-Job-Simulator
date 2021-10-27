using Sandbox;

namespace JobSim.Entities
{
	[Library( "weapon_pistol", Title = "Pistol" )]
	public partial class WeaponPistol : BaseWeapon
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		}

		public override void CreateViewModel()
		{
			if ( string.IsNullOrEmpty( ViewModelPath ) )
				return;

			ViewModelEntity = new ViewModel
			{
				Position = Position,
				Owner = Owner,
				EnableViewmodelRendering = true
			};
			ViewModelEntity.SetModel( ViewModelPath );
		}

		public override void AttackPrimary()
		{
			base.AttackPrimary();

			PlaySound( "rust_pistol.shoot" );
			ShootBullet( 0.2f, 1.5f, 9.0f, 3.0f );
		}

		/// <summary>
		/// Shoot a single bullet
		/// </summary>
		public virtual void ShootBullet( float spread, float force, float damage, float bulletSize, int bulletCount = 1 )
		{
			//
			// Seed rand using the tick, so bullet cones match on client and server
			//
			Rand.SetSeed( Time.Tick );

			for ( int i = 0; i < bulletCount; i++ )
			{
				Vector3 forward = Owner.EyeRot.Forward;
				forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
				forward = forward.Normal;

				//
				// ShootBullet is coded in a way where we can have bullets pass through shit
				// or bounce off shit, in which case it'll return multiple results
				//
				foreach ( TraceResult tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * 5000, bulletSize ) )
				{
					tr.Surface.DoBulletImpact( tr );

					if ( !IsServer ) continue;
					if ( !tr.Entity.IsValid() ) continue;

					DamageInfo damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100 * force, damage )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}
		}

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

			if ( IsLocalPawn )
				_ = new Sandbox.ScreenShake.Perlin();

			ViewModelEntity?.SetAnimBool( "fire", true );
			CrosshairPanel?.CreateEvent( "fire" );
		}
	}
}
