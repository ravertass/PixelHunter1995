using PixelHunter1995.SceneLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

namespace PixelHunter1995
{
    class SceneParser
    {
        public static Scene ParseSceneXml(string sceneXmlPath, Game game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            Tileset tileset = null; // TODO: Possible to have many tilesets per scene?
            List<IDrawable> drawables = new List<IDrawable>();
            List<IUpdateable> updateables = new List<IUpdateable>();
            List<ILoadContent> loadables = new List<ILoadContent>();
            Player player = null;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == "imagelayer" && node.Attributes["name"]?.InnerText == "background")
                {
                    Debug.Assert(node.ChildNodes.Count == 1);
                    XmlNode imageNode = node.ChildNodes[0];
                    string imagePathRelative = imageNode.Attributes["source"].Value;
                    string imagePath = Path.Combine(Path.GetFileNameWithoutExtension(Path.GetDirectoryName(sceneXmlPath)),
                                                    Path.GetDirectoryName(imagePathRelative),
                                                    Path.GetFileNameWithoutExtension(imagePathRelative));
                    int width = int.Parse(imageNode.Attributes["width"].Value);
                    int height = int.Parse(imageNode.Attributes["height"].Value);
                    Background background = new Background(imagePath, width, height);
                    // TODO Preferably, all these things (background, player, etc.) should add themselves to the lists.
                    // TODO Maybe as part of constructor?
                    drawables.Add(background);
                    loadables.Add(background);
                }
                else if (node.Name == "tileset")
                {
                    string tilesetXmlPathRelative = node.Attributes["source"].Value;
                    string tilesetXmlPath = Path.Combine(Path.GetDirectoryName(sceneXmlPath),
                                                         Path.GetDirectoryName(tilesetXmlPathRelative),
                                                         Path.GetFileName(tilesetXmlPathRelative));
                    int tilesetFirstGid = int.Parse(node.Attributes["firstgid"].Value);
                    tileset = ParseTilesetXml(tilesetXmlPath, tilesetFirstGid);
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "dogs")
                {
                    foreach (XmlNode dogNode in node.ChildNodes)
                    {
                        int x = (int)Math.Round(float.Parse(dogNode.Attributes["x"].Value));
                        int y = (int)Math.Round(float.Parse(dogNode.Attributes["y"].Value));
                        int width = (int)Math.Round(float.Parse(dogNode.Attributes["width"].Value));
                        int height = (int)Math.Round(float.Parse(dogNode.Attributes["height"].Value));
                        int gid = int.Parse(dogNode.Attributes["gid"].Value);
                        y = y - height; // Compensate for Tiled's coordinate system
                        Dog dog = new Dog(x, y, width, height, gid);
                        drawables.Add(dog);
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    WalkingArea walkingArea = ParseWalkingXml(node);
                    drawables.Add(walkingArea);

                    // TODO Make player its own objectgroup, or part of "portal", or something similar.
                    if (player == null)
                    {
                        player = new Player(game, 50, 50);
                        drawables.Add(player);
                        updateables.Add(player);
                        loadables.Add(player);
                    }
                }
            }
            return new Scene(drawables, updateables, loadables, tileset);
        }

        private static Tileset ParseTilesetXml(string tilesetXmlPath, int firstGid)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(tilesetXmlPath);

            string name = doc.DocumentElement.Attributes["name"].Value;
            int tileWidth = int.Parse(doc.DocumentElement.Attributes["tilewidth"].Value);
            int tileHeight = int.Parse(doc.DocumentElement.Attributes["tileheight"].Value);
            int tileCount = int.Parse(doc.DocumentElement.Attributes["tilecount"].Value);
            int noOfColumns = int.Parse(doc.DocumentElement.Attributes["columns"].Value);
            string imagePath = "";
            int imageWidth = 0;
            int imageHeight = 0;

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Name == "image")
                {
                    string imagePathRelative = node.Attributes["source"].Value;
                    imagePath = Path.Combine(Path.GetFileNameWithoutExtension(Path.GetDirectoryName(tilesetXmlPath)),
                                             Path.GetDirectoryName(imagePathRelative),
                                             Path.GetFileNameWithoutExtension(imagePathRelative));
                    imageWidth = int.Parse(node.Attributes["width"].Value);
                    imageHeight = int.Parse(node.Attributes["height"].Value);
                }
            }

            return new Tileset(imagePath, imageWidth, imageHeight, firstGid, name, tileWidth,
                tileHeight, tileCount, noOfColumns);
        }

        private static WalkingArea ParseWalkingXml(XmlNode node)
        {
            // TODO: We should probably allow multiple walking areas.

            Debug.Assert(node.ChildNodes.Count > 0);
            XmlNode walkingNode = node.ChildNodes[0];

            float baseX = float.Parse(walkingNode.Attributes["x"]?.InnerText);
            float baseY = float.Parse(walkingNode.Attributes["y"]?.InnerText);

            Debug.Assert(walkingNode.ChildNodes.Count > 0);
            XmlNode polygonNode = walkingNode.ChildNodes[0];

            String polygonPointsString = polygonNode.Attributes["points"]?.InnerText;
            List<String> splitPolygonPointsString = polygonPointsString.Split(' ').ToList();

            List<Coord> points = new List<Coord>();
            foreach (String singlePointString in splitPolygonPointsString)
            {
                List<String> splitSinglePointString = singlePointString.Split(',').ToList();
                Debug.Assert(splitSinglePointString.Count == 2);
                float x = float.Parse(splitSinglePointString[0]);
                float y = float.Parse(splitSinglePointString[1]);
                Coord point = new Coord(baseX + x, baseY + y);
                points.Add(point);
            }

            return new WalkingArea(points);
        }
    }
}
