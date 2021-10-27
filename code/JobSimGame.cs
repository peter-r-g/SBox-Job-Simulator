using JobSim.Characters;
using JobSim.Inventory;
using JobSim.Job;
using JobSim.Money;
using JobSim.UI;
using JobSim.Waypoints;
using Sandbox;
using System;

namespace JobSim
{
	public partial class JobSimGame : Game
	{
		public static JobSimGame Instance => Current as JobSimGame;

		[Net]
		public JobSimHud JobSimHud { get; private set; } = null;
		[Net]
		public NetEvent NetEvent { get; private set; } = null;

		public JobManager JobManager { get; private set; } = null;
		public ItemManager ItemManager { get; private set; } = null;

		public JobSimGame()
		{
			if ( IsServer )
			{
				JobSimHud = new();
				NetEvent = new();
				Persistence.Init();
			}
			else
				WaypointManager.LoadTextures();

			JobManager = new();
			_ = new DeliveryJob( 100, new Vector3( 0, 0, 0 ), new Vector3( -1506, 1064, 0 ) );

			ItemManager = new();

			EntityItemDefinition pistol = new();
			pistol.ItemName = "Pistol";
			pistol.IconPath = "/textures/ui/items/pistol.png";
			pistol.DroppedModelPath = "models/citizen_props/cardboardbox01.vmdl";
			pistol.EntityClass = "weapon_pistol";
			ItemManager.Register( pistol );

			ItemDefinition brick = new();
			brick.ItemName = "Brick";
			brick.MaxStack = 64;
			brick.IconPath = "/textures/ui/items/brick.png";
			brick.DroppedModelPath = "models/citizen_props/cardboardbox01.vmdl";
			ItemManager.Register( brick );

			ConsumableItemDefinition pizza = new();
			pizza.ItemName = "Pizza";
			pizza.MaxStack = 10;
			pizza.IconPath = "/textures/ui/items/pizza.png";
			pizza.DroppedModelPath = "models/citizen_props/cardboardbox01.vmdl";
			pizza.ConsumeFunc = ( user, item ) =>
			{
				user.Entity.Health += 10;
				return 1;
			};
			ItemManager.Register( pizza );

			ConsumableItemDefinition badPizza = new();
			badPizza.ItemName = "Bad Pizza";
			badPizza.MaxStack = 10;
			badPizza.IconPath = "/textures/ui/items/bad_pizza.png";
			badPizza.DroppedModelPath = "models/citizen_props/cardboardbox01.vmdl";
			badPizza.ConsumeFunc = ( user, item ) =>
			{
				user.Entity.TakeDamage( DamageInfo.Generic( 10 ) );
				return 1;
			};
			ItemManager.Register( badPizza );

			JobItemDefinition package = new();
			package.ItemName = "Package";
			package.MaxStack = 9999;
			package.IconPath = "/textures/ui/items/package.png";
			package.DroppedModelPath = "models/citizen_props/carboardbox01.vmdl";
			ItemManager.Register( package );
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			Persistence.Load( cl.SteamId.ToString() );
			EmployableCharacterPersistentData cData = (EmployableCharacterPersistentData)Persistence.GetData( DataType.EmployableCharacter, cl.SteamId.ToString() );
			ItemContainerPersistentData iData = (ItemContainerPersistentData)Persistence.GetData( DataType.ItemContainer, cl.SteamId.ToString() );
			MoneyContainerPersistentData mData = (MoneyContainerPersistentData)Persistence.GetData( DataType.MoneyContainer, cl.SteamId.ToString() );

			JobSimPlayer ply = new();
			if ( cData != null && iData != null && mData != null )
				ply.Load( cData, iData, mData );
			else
				ply.Load( cl );

			cl.Pawn = ply;
			ply.Respawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );

			Persistence.SaveAndClear( cl.SteamId.ToString() );
		}

		public override void Shutdown()
		{
			base.Shutdown();

			if ( Realm.IsServer )
				Persistence.SaveAll();
		}

		[ServerCmd("get_pos")]
		public static void GetPawnPosition()
		{
			if ( ConsoleSystem.Caller == null )
				return;

			Realm.Log.Info( ConsoleSystem.Caller.Pawn.Position );
		}
		
		[ServerCmd( "give_job" )]
		public static void GiveJob( string job )
		{
			if ( ConsoleSystem.Caller == null )
				return;

			JobManager.GiveJob( (ConsoleSystem.Caller.Pawn as JobSimPlayer).Character, job );
		}

		[ServerCmd( "add_money" )]
		public static void AddMoneyCommand( float money )
		{
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Money.GiveMoney( money );
		}

		[ServerCmd( "take_money" )]
		public static void TakeMoneyCommand( float money )
		{
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Money.TakeMoney( money );
		}

		[ServerCmd( "give_item" )]
		public static void GiveItemCommand( string itemName, int amount )
		{
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Inventory.TryGiveItem( itemName, amount );
		}

		[ServerCmd( "take_item" )]
		public static void TakeItemCommand( string itemName, int amount )
		{
			if ( ConsoleSystem.Caller == null )
				return;

			(ConsoleSystem.Caller.Pawn as JobSimPlayer).Inventory.TryTakeItem( itemName, amount );
		}

#pragma warning disable IDE0051 // Remove unused private members
		[InventoryEvent.ItemsAdded]
		private static void ItemsAdded( ContainerChangedEventArgs eventArgs )
		{
			if ( Realm.IsServer )
				Realm.Log.Info( $"{eventArgs.Container.UUID}s items were changed: {eventArgs.Amount}x {eventArgs.ItemDefinition.ItemName} have been added" );
			else
				Realm.Log.Info( $"Local characters items were changed: {eventArgs.Amount}x {eventArgs.ItemDefinition.ItemName} have been added" );
		}

		[InventoryEvent.ItemsRemoved]
		private static void ItemsRemoved( ContainerChangedEventArgs eventArgs )
		{
			if ( Realm.IsServer )
				Realm.Log.Info( $"{eventArgs.Container.UUID}s items were changed: {eventArgs.Amount}x {eventArgs.ItemDefinition.ItemName} have been removed" );
			else
				Realm.Log.Info( $"Local characters items were changed: {eventArgs.Amount}x {eventArgs.ItemDefinition.ItemName} have been removed" );
		}

		[InventoryEvent.ItemDropped]
		private static void ItemDropped( ItemDroppedEventArgs eventArgs )
		{
			Realm.Log.Info( $"{eventArgs.Container.UUID} dropped {eventArgs.Item.Stack - eventArgs.DroppedItem.Stack}x {eventArgs.Item.Definition.ItemName}" );
		}

		[InventoryEvent.ItemUsed]
		private static void ItemUsed( ItemUsedEventArgs eventArgs )
		{
			int amountUsed = eventArgs.Item.Stack - eventArgs.UsedItem.Stack;
			if ( amountUsed == 0 )
				Realm.Log.Info( $"{eventArgs.Container.UUID} used a {eventArgs.Item.Definition.ItemName}" );
			else
				Realm.Log.Info( $"{eventArgs.Container.UUID} used {amountUsed}x {eventArgs.Item.Definition.ItemName}" );
		}

		[JobEvent.JobStarted]
		private static void JobStarted( JobEventArgs eventArgs )
		{
			Realm.Log.Info( $"{eventArgs.Character.Name} has started \"{eventArgs.Character.Job.Name}\"" );
		}

		[JobEvent.JobStageCompleted]
		private static void JobStageCompleted( JobEventArgs eventArgs )
		{
			int completedStage = eventArgs.Character.JobStage;
			Realm.Log.Info( $"{eventArgs.Character.Name} has completed stage {completedStage + 1} of \"{eventArgs.Character.Job.Name}\"" );
		}

		[JobEvent.JobCompleted]
		private static void JobCompleted( JobEventArgs eventArgs )
		{
			Realm.Log.Info( $"{eventArgs.Character.Name} has completed \"{eventArgs.Character.Job.Name}\"" );
		}

		[JobEvent.JobFailed]
		private static void JobFailed( JobEventArgs eventArgs )
		{
			int failedStage = eventArgs.Character.JobStage;
			Realm.Log.Info( $"{eventArgs.Character.Name} has failed \"{eventArgs.Character.Job.Name}\" at stage {failedStage + 1}" );
		}

		[MoneyEvent.MoneyChanged]
		private static void MoneyChanged( MoneyChangedEventArgs eventArgs )
		{
			if ( Realm.IsServer )
				Realm.Log.Info( $"{eventArgs.Container.UUID}s money was changed: {(eventArgs.Delta > 0 ? "Gained" : "Lost")} {MoneyFormat.Format( Math.Abs( eventArgs.Delta ) )}" );
			else
				Realm.Log.Info( $"Local characters money was changed: {(eventArgs.Delta > 0 ? "Gained" : "Lost")} {MoneyFormat.Format( Math.Abs( eventArgs.Delta ) )}" );
		}

		[WaypointEvent.WaypointCreated]
		private static void WaypointCreated( WaypointEventArgs _ ) => Realm.Log.Info( $"A waypoint has been created" );
		[WaypointEvent.WaypointDeleted]
		private static void WaypointDeleted( WaypointEventArgs _ ) => Realm.Log.Info( $"A waypoint has been deleted" );
#pragma warning restore IDE0051 // Remove unused private members
	}
}
