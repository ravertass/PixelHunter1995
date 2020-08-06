using Microsoft.Xna.Framework;
using PixelHunter1995.SceneLib;
using PixelHunter1995.TilesetLib;
using PixelHunter1995.Utilities;
using PixelHunter1995.WalkingAreaLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System;

namespace PixelHunter1995
{
    class SceneParser
    {
        public static Scene ParseSceneXml(string sceneXmlPath, Game game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            List<Tileset> tilesets = new List<Tileset>();
            List<IDrawable> drawables = new List<IDrawable>();
            List<IUpdateable> updateables = new List<IUpdateable>();
            List<ILoadContent> loadables = new List<ILoadContent>();
            Player player = null;

            // Get tileset first to be used when loading dogs
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "tileset")
                {
                    string tilesetXmlPathRelative = node.Attributes["source"].Value;
                    string tilesetXmlPath = Path.Combine(Path.GetDirectoryName(sceneXmlPath),
                                                         Path.GetDirectoryName(tilesetXmlPathRelative),
                                                         Path.GetFileName(tilesetXmlPathRelative));
                    int tilesetFirstGid = int.Parse(node.Attributes["firstgid"].Value);
                    Tileset tileset = TilesetParser.ParseTilesetXml(tilesetXmlPath, tilesetFirstGid);
                    tilesets.Add(tileset);
                    loadables.Add(tileset);
                }
            }

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
                        Tileset tileset = GetTilesetFromGid(tilesets, gid);
                        Dog dog = new Dog(x, y, width, height, gid, tileset);
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
            return new Scene(drawables, updateables, loadables);
        }

        private static Tileset GetTilesetFromGid(List<Tileset> tilesets, int gid)
        {
            // We sort Tilesets on first gid with largest first.
            tilesets.Sort((a, b) => b.firstGid.CompareTo(a.firstGid));
            foreach (var tileset in tilesets)
            {
                if (gid < tileset.firstGid)
                {
                    continue;
                }
                else
                {
                    return tileset;
                }
            }
            throw new ArgumentException("Can't find tileset for gid " + gid + ".");
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
