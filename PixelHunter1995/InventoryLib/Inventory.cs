
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.TilesetLib;

namespace PixelHunter1995.InventoryLib
{
    class Inventory: ILoadContent, IDrawable, IUpdateable
    {
        private static readonly int X_POS = 120;
        private static readonly int Y_POS = 175;
        private static readonly int X_PADDING = 2;
        private static readonly int Y_PADDING = 3;
        public static readonly int ITEM_WIDTH = 40;
        public static readonly int ITEM_HEIGHT = 30;
        private static readonly int COLUMNS = 6;
        private static readonly int ROWS = 2;
        private static readonly string TILESET_DIR = "Content\\Tileset";

        public List<InventoryItem> Items = new List<InventoryItem>();
        private Tileset InventoryTileset;

        public Inventory()
        {
            int tilesetFirstGid = 1; // default value
            string tilesetFilename = "dogs.tsx";
            string tilesetXmlPath = Path.Combine(TILESET_DIR, tilesetFilename); // TODO: support several tilesets
            InventoryTileset = TilesetParser.ParseTilesetXml(tilesetXmlPath, tilesetFirstGid);
        }

        public void LoadContent(ContentManager content)
        {
            InventoryTileset.LoadContent(content);
        }

        private Vector2 GetItemTilePosition(int tileNumber)
        {
            Debug.Assert(tileNumber >= 0 && tileNumber < COLUMNS * ROWS, "tileNumber outside of valid range.");
            int row = tileNumber / COLUMNS;
            int column = tileNumber % COLUMNS;
            int x_pos = X_POS + X_PADDING + column * (ITEM_WIDTH + X_PADDING);
            int y_pos = Y_POS + Y_PADDING + row * (ITEM_HEIGHT + Y_PADDING);
            return new Vector2(x_pos, y_pos);
        }
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, double scaling)
        {
            // TODO: Some way to display more than COLUMNS * ROWS items

            // Temporary code give the player some stuff if empty inventory
            if (Items.Count == 0)
            {
                System.Console.WriteLine("You have nothing in your inventory, here take some stuff!");
                Add("banana", "dogs");
                Add("apple", "dogs");
                Add("broccoli", "dogs");
                Add("broccoli", "dogs");
                Add("broccoli", "dogs");
                Add("broccoli", "dogs");
                Add("broccoli", "dogs");
                Add("broccoli", "dogs");
                System.Console.WriteLine(this);
            }
            DrawBackground(graphics, spriteBatch);
            foreach (InventoryItem item in Items)
            {
                item.Draw(graphics, spriteBatch, scaling);
            }
        }

        private void DrawBackground(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            // Draw background boxes for each visible item slot
            Texture2D rect = new Texture2D(graphics.GraphicsDevice, ITEM_WIDTH, ITEM_HEIGHT);
            Color[] data = new Color[ITEM_WIDTH * ITEM_HEIGHT];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Gray;
            rect.SetData(data);
            for (int row = 0; row < ROWS; row++)
            {
                for (int column = 0; column < COLUMNS; column++)
                {
                    Vector2 coor = new Vector2(X_POS + column * (ITEM_WIDTH + X_PADDING),
                                               Y_POS + row * (ITEM_HEIGHT + Y_PADDING));
                    spriteBatch.Draw(rect, coor, Color.White);
                }
            }
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Position = GetItemTilePosition(i);
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
                    Debug.Fail(itemName.ToLower() + " does not exist in tileset " + tilesetName + ".");
                    return 0;
            }

        }

        public void Add(string itemName, string tilesetName)
        {
            Vector2 itemPos = GetItemTilePosition(Items.Count);
            Items.Add(new InventoryItem(itemName, InventoryTileset, GetGidFromName(itemName, tilesetName),
                                        (int) itemPos.X, (int) itemPos.Y,
                                        ITEM_WIDTH, ITEM_HEIGHT));
        }

        public void Remove(string itemName)
        {
            // TODO
            Debug.Fail("Trying to remove " + itemName.ToLower() + " but function is not implemented :(");
        }

        public override string ToString()
        {
            if (Items.Count == 0)
            {
                return "InventoryList is empty!";
            }
            string res = "InventoryList contains:\n";
            for (int i = 0; i < Items.Count; i++)
            {
                res += i + ":\t" + Items[i] + "\n";
            }
            return res;
        }

        public int ZIndex()
        {
            return 0; // Not drawn by scene, value here does not matter
        }
    }
}
