
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.InventoryLib
{
    class InventoryItem
    {
        readonly string Name;
        readonly Tileset Tileset;
        readonly int TilesetGid;

        public InventoryItem(string name, Tileset tileset, int tilesetGid)
        {
            Name = name;
            Tileset = tileset;
            TilesetGid = tilesetGid;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling, Vector2 target)
        {
            Tileset.Draw(spriteBatch, target, TilesetGid, scaling);
        }
    }
}
