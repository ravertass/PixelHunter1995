﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.SceneLib;
using PixelHunter1995.TilesetLib;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.InventoryLib
{
    class InventoryItem : IDog
    {
        public string Name { get; set; }
        readonly Tileset Tileset;
        readonly int TilesetGid;
        private int X;
        private int Y;
        private readonly int Width;
        private readonly int Height;

        public Vector2 Position {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = (int) value.X;
                Y = (int) value.Y;
            }
        }

        public InventoryItem(string name, Tileset tileset, int tilesetGid, int x, int y, int width, int height)
        {
            Name = name;
            Tileset = tileset;
            TilesetGid = tilesetGid;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scalingMin)
        {
            Tileset.Draw(spriteBatch, Position, TilesetGid, 1.0);
        }

        public bool Contains(Vector2 point)
        {
            return new Rectangle(X, Y, Width, Height).Contains(new Vector2(point.X, point.Y));
        }

        public int ZIndex()
        {
            return 10;
        }
    }
}
