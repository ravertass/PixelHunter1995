
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.InventoryLib
{
    class Inventory
    {
        private static readonly int X_POS = 120;
        private static readonly int Y_POS = 160;
        private static readonly int X_PADDING = 5;
        private static readonly int Y_PADDING = 5;
        private static readonly int X_ITEM_SIZE = 32;
        private static readonly int Y_ITEM_SIZE = 32;
        private static readonly int COLUMNS = 6;
        private static readonly int ROWS = 2;

        private List<InventoryItem> ItemList = new List<InventoryItem>();
        private Texture2D InventoryBackground;
        private Tileset InventoryTileset;

        public void Initialize(string tilesetDirPath)
        {
            int tilesetFirstGid = 1; // default value
            string tilesetXmlPath = Path.Combine(tilesetDirPath, "dogs.tsx");
            InventoryTileset = TilesetParser.ParseTilesetXml(tilesetXmlPath, tilesetFirstGid);
            System.Console.WriteLine(InventoryTileset.name);
        }

        public void LoadContent(ContentManager content)
        {
            InventoryBackground = content.Load<Texture2D>("Images/inventory");
            InventoryTileset.LoadContent(content);
        }

        private Vector2 GetItemTilePosition(int tileNumber)
        {
            Debug.Assert(tileNumber >= 0 && tileNumber < COLUMNS * ROWS, "tileNumber outside of valid range.");
            int row = tileNumber / COLUMNS;
            int column = tileNumber % COLUMNS;
            int x_pos = X_POS + X_PADDING + column * (X_ITEM_SIZE + X_PADDING);
            int y_pos = Y_POS + Y_PADDING + row * (Y_ITEM_SIZE + Y_PADDING);
            return new Vector2(x_pos, y_pos);
        }
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            // TODO: Some way to display more than COLUMNS * ROWS items

            // Temporary code give the player some stuff if empty inventory
            if (ItemList.Count == 0)
            {
                System.Console.WriteLine("You have nothing in your inventory, here take some stuff!");
                Add("banana", "dogs");
                Add("apple", "dogs");
                Add("broccoli", "dogs");
                System.Console.WriteLine(this);
            }
            spriteBatch.Draw(InventoryBackground, new Vector2(X_POS, Y_POS), Color.White);
            for (int i = 0; i < ItemList.Count; i++)
            {
                System.Console.WriteLine("Draw " + ItemList[i] + " at " + GetItemTilePosition(i));
                ItemList[i].Draw(graphics, spriteBatch, GetItemTilePosition(i));
            }
        }

        private int GetGidFromName(string itemName, string tilesetName)
        {
            Debug.Assert(tilesetName.ToLower().Equals(InventoryTileset.name.ToLower()),
                "Tileset " + tilesetName + " is not loaded for inventory use.");
            // TODO: Maybe not have this kind of table in source code?
            //       Would have been good if it was included in tsx file and we could parse it...
            switch (itemName.ToLower())
            {
                case "banana":
                    return 1;
                case "apple":
                    return 2;
                case "broccoli":
                    return 3;
                default:
                    Debug.Fail(itemName.ToLower() + " do not exist in tileset " + tilesetName + ".");
                    return 0;
            }

        }

        public void Add(string itemName, string tilesetName)
        {
            ItemList.Add(new InventoryItem(itemName, InventoryTileset, GetGidFromName(itemName, tilesetName)));
        }

        public void Remove(string itemName)
        {
            // TODO
            Debug.Fail("Trying to remove " + itemName.ToLower() + " but function is not implemented :(");
        }

        public override string ToString()
        {
            if (ItemList.Count == 0)
            {
                return "InventoryList is empty!";
            }
            string res = "InventoryList contains:\n";
            for (int i = 0; i < ItemList.Count; i++)
            {
                res += i + ":\t" + ItemList[i] + "\n";
            }
            return res;
        }

    }
}
