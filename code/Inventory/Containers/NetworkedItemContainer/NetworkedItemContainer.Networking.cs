using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace JobSim.Inventory
{
    public partial class NetworkedItemContainer
	{
		private readonly List<(EventType, uint, int)> changes = new();

		private byte sv_messageIndex = 0;
		private byte cl_lastMessageIndex = 0;

		public void Read( NetRead reader )
		{
			if ( !ReadHead( ref reader ) )
				return;

			Items.Clear();

			List<(EventType, uint, int)> changes = ReadChanges( ref reader );
			ReadInfo( ref reader );
			ReadItems( ref reader );

			for ( int i = 0; i < changes.Count; i++ )
			{
				string eventName = "";
				ContainerChangedEventArgs eventArgs = new(
					this,
					ItemManager.Instance.GetDefinitionByID( changes[i].Item2 ),
					changes[i].Item3
				);

				switch ( changes[i].Item1 )
				{
					case EventType.Added:
						eventName = InventoryEvent.ItemsAddedEvent;
						break;
					case EventType.Removed:
						eventName = InventoryEvent.ItemsRemovedEvent;
						break;
					default:
						Realm.Log.Error( $"Got unknown event type {changes[i].Item1}" );
						break;
				}
				
				EventRunner.Run( eventName, eventArgs );
			}
		}

		private bool ReadHead( ref NetRead reader )
		{
			byte newMessageIndex = reader.Read<byte>();
			if ( newMessageIndex == cl_lastMessageIndex )
				return false;

			cl_lastMessageIndex = newMessageIndex;
			return true;
		}

		private List<(EventType, uint, int)> ReadChanges( ref NetRead reader )
		{
			List<(EventType, uint, int)> changeList = new();
			int changeCount = reader.Read<int>();
			for ( int i = 0; i < changeCount; i++ )
				changeList.Add( ((EventType)reader.Read<byte>(), reader.Read<uint>(), reader.Read<int>()) );

			return changeList;
		}

		private void ReadInfo( ref NetRead reader )
		{
			UUID = reader.ReadString();
			int networkIdent = reader.Read<int>();
			Entity = Entity.All.First( ( entity ) => entity.NetworkIdent == networkIdent );
			Slots = reader.Read<int>();
		}

		private void ReadItems( ref NetRead reader )
		{
			int itemCount = reader.Read<int>();
			for ( int i = 0; i < itemCount; i++ )
			{
				IItem item = Library.Create<IItem>( reader.ReadString() );
				item.NetRead( ref reader );

				while ( Items.Count < item.Slot )
					Items.Add( new Item( Items.Count ) );

				Items.Add( item );
			}

			for ( int i = Items.Count; i < Slots; i++ )
				Items.Add( new Item( i ) );
		}

		public void Write( NetWrite writer )
		{
			sv_messageIndex++;
			writer.Write( sv_messageIndex );

			writer.Write( changes.Count );
			for ( int i = 0; i < changes.Count; i++  )
			{
				writer.Write( changes[i].Item1 );
				writer.Write( changes[i].Item2 );
				writer.Write( changes[i].Item3 );
			}

			writer.Write( UUID );
			if ( Entity != null )
				writer.Write( Entity.NetworkIdent );
			else
				writer.Write( -1 );
			writer.Write( Slots );

			int numNonNullItems = 0;
			List<int> nonNullItems = new();
			for ( int i = 0; i < Items.Count; i++ )
			{
				if ( Items[i].IsNull )
					continue;

				numNonNullItems++;
				nonNullItems.Add( i );
			}

			writer.Write( numNonNullItems );
			for ( int i = 0; i < nonNullItems.Count; i++ )
				Items[nonNullItems[i]].NetWrite( writer );

			changes.Clear();
		}
	}
}
