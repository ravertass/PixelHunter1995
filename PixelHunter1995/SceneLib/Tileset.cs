using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PixelHunter1995.SceneLib
{
    class Tileset
    {
        string imagePath;
        int imageWidth;
        int imageHeight;
        int firstGid;
        string name;
        int tileWidth;
        int tileHeight;
        int tileCount;
        int noOfColumns;

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

        public void DrawTile(SpriteBatch spriteBatch, Rectangle destinationRectangle, int gid)
        {
            Debug.Assert(gid >= firstGid && gid < firstGid + tileCount, "Gid outside of valid range.");
            int row = (gid - firstGid) / noOfColumns;
            int column = (gid - firstGid) % noOfColumns;
            Rectangle sourceRectangle = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
            spriteBatch.Draw(image, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
