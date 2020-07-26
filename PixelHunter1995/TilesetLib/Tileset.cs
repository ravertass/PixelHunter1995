using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PixelHunter1995.TilesetLib
{
    class Tileset
    {
        private readonly string imagePath;
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly int firstGid;
        private readonly string name;
        public readonly int tileWidth;
        private readonly int tileHeight;
        private readonly int tileCount;
        private readonly int noOfColumns;

        Texture2D image;

        public Tileset(string imagePath, int imageWidth, int imageHeight, int firstGid, string name, int tileWidth,
            int tileHeight, int tileCount, int noOfColumns)
        {
            this.imagePath = imagePath;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.firstGid = firstGid;
            this.name = name;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileCount = tileCount;
            this.noOfColumns = noOfColumns;
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>(imagePath);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 destination, int gid, double scaling, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(gid >= firstGid && gid < firstGid + tileCount, "Gid outside of valid range.");
            int row = (gid - firstGid) / noOfColumns;
            int column = (gid - firstGid) % noOfColumns;
            Rectangle sourceRectangle = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
            Rectangle destinationRectangle = new Rectangle((int)destination.X, (int)destination.Y, (int)(tileWidth * scaling), (int)(tileHeight * scaling));
            spriteBatch.Draw(image, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(), spriteEffects, 0);
        }
    }
}
