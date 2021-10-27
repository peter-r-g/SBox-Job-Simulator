using Sandbox;
using System.Linq;

namespace JobSim.Inventory
{
	[Library( "item_entity" )]
	public class EntityItem : Item, IEntityItem
	{
		public Entity Entity { get; set; } = null;

		public EntityItem() : base () { }
		public EntityItem( int slot ) : base( slot ) { }
		public EntityItem( IItem item ) : base(item)
		{
			if ( item is IEntityItem entityItem )
				Entity = entityItem.Entity;
		}

		public override IItem Clone() => new EntityItem( this );

		public override void NetRead( ref NetRead reader )
		{
			base.NetRead( ref reader );

			int entityIdent = reader.Read<int>();
			Entity = Entity.All.FirstOrDefault( ( entity ) => entity.NetworkIdent == entityIdent );
		}

		public override void NetWrite( NetWrite writer )
		{
			base.NetWrite( writer );

			if ( Entity != null && Entity.IsValid )
				writer.Write( Entity.NetworkIdent );
			else
				writer.Write( -1 );
		}
	}
}
