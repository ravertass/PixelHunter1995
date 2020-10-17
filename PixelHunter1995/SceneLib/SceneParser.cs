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
        public static Scene ParseSceneXml(string sceneXmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNode baseNode = doc.DocumentElement;
            XmlNodeList nodes = baseNode.ChildNodes;
            int sceneWidth = int.Parse(baseNode.Attributes["width"].Value);
            List<Tileset> tilesets = new List<Tileset>();
            List<IDrawable> drawables = new List<IDrawable>();
            List<IUpdateable> updateables = new List<IUpdateable>();
            List<IDog> dogs = new List<IDog>();
            List<ILoadContent> loadables = new List<ILoadContent>();
            IDictionary<string, Portal> portals = new Dictionary<string, Portal>();
            WalkingArea walkingArea = null;
            Player player = null;
            float characterScalingMin = 1.0F;

            XmlNode propertiesNode = GetChildNode(baseNode, "properties");
            string songName = GetPropertyValue(propertiesNode, "song");

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
                if (node.Name == "imagelayer")
                {
                    if (node.Attributes["name"]?.InnerText == "background")
                    {
                        Debug.Assert(node.ChildNodes.Count == 1, "More than one background layer in scene");
                        ImageLayer background = ParseImageNode(node.ChildNodes[0], sceneXmlPath, 0);
                        drawables.Add(background);
                        loadables.Add(background);
                    }
                    else if (node.Attributes["name"]?.InnerText == "foreground")
                    {
                        Debug.Assert(node.ChildNodes.Count == 1, "More than one foreground layer in scene");
                        ImageLayer foreground = ParseImageNode(node.ChildNodes[0], sceneXmlPath, 1000);
                        drawables.Add(foreground);
                        loadables.Add(foreground);
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "dogs")
                {
                    foreach (XmlNode dogNode in node.ChildNodes)
                    {
                        IDog dog = ParseDogNode(dogNode, tilesets, sceneWidth);
                        drawables.Add(dog);
                        dogs.Add(dog);
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "player")
                {
                    Debug.Assert(node.ChildNodes.Count == 1);
                    XmlNode playerNode = node.ChildNodes[0];
                    int x = (int)Math.Round(float.Parse(playerNode.Attributes["x"].Value));
                    int y = (int)Math.Round(float.Parse(playerNode.Attributes["y"].Value));
                    y -= (int)Math.Round(float.Parse(playerNode.Attributes["height"].Value)); // Compensate for Tiled's coordinate system
                    string name = playerNode.Attributes["name"]?.InnerText;
                    player = new Player(x, y, name ?? "Felixia");
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    walkingArea = new WalkingArea(ParsePolygonXml(node.ChildNodes[0]), sceneWidth);
                    drawables.Add(walkingArea);
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "portals")
                {
                    foreach (XmlNode portalNode in node.ChildNodes)
                    {
                        Portal portal = ParsePortalNode(portalNode, sceneWidth);
                        portals[portal.Name] = portal;
                        drawables.Add(portal);
                    }
                }
                else if (node.Name == "properties")
                {
                    characterScalingMin = float.Parse(GetPropertyValueOrDefault(node, "scaling_min", "1.0"));
                }
                if (player == null)
                {
                    player = new Player("Felixia");
                }
            }
            return new Scene(drawables,
                             updateables,
                             loadables,
                             dogs,
                             portals,
                             player,
                             walkingArea,
                             sceneWidth,
                             characterScalingMin,
                             songName);
        }

        private static IDog ParseDogNode(XmlNode dogNode, List<Tileset> tilesets, int sceneWidth)
        {
            IDog dog;
            int x = (int)Math.Round(float.Parse(dogNode.Attributes["x"].Value));
            int y = (int)Math.Round(float.Parse(dogNode.Attributes["y"].Value));
            string name = dogNode.Attributes["name"]?.InnerText;
            if (dogNode.Attributes["gid"] != null)
            {
                int width = (int)Math.Round(float.Parse(dogNode.Attributes["width"].Value));
                int height = (int)Math.Round(float.Parse(dogNode.Attributes["height"].Value));
                int gid = int.Parse(dogNode.Attributes["gid"].Value);
                y -= height; // Compensate for Tiled's coordinate system
                Tileset tileset = GetTilesetFromTileGid(tilesets, gid);
                dog = new Dog(x, y, width, height, gid, tileset, name ?? $"Dog {gid}");
            }
            else
            {
                dog = new PolygonDog(x, y, ParsePolygonXml(dogNode), sceneWidth, name ?? "PolygonDog");
            }
            return dog;
        }

        private static Portal ParsePortalNode(XmlNode portalNode, int sceneWidth)
        {
            int x = (int)Math.Round(float.Parse(portalNode.Attributes["x"].Value));
            int y = (int)Math.Round(float.Parse(portalNode.Attributes["y"].Value));

            XmlNode propertiesNode = GetChildNode(portalNode, "properties");
            string name = GetPropertyValue(propertiesNode, "name");
            string destinationScene = GetPropertyValue(propertiesNode, "destination");
            string destinationPortal = GetPropertyValue(propertiesNode, "destination_portal");

            return new Portal(x, y, ParsePolygonXml(portalNode), sceneWidth, name, destinationScene, destinationPortal);
        }

        private static String GetPropertyValue(XmlNode propertiesNode, String propertyName)
        {
            foreach (XmlNode propertyNode in propertiesNode.ChildNodes)
            {
                String name = propertyNode.Attributes["name"].Value;
                String value = propertyNode.Attributes["value"].Value;

                if (name.Equals(propertyName))
                {
                    return value;
                }
            }

            throw new InvalidOperationException(String.Format("No property with name {0} found", propertyName));
        }

        private static String GetPropertyValueOrDefault(XmlNode propertiesNode, String propertyName, String defaultValue)
        {
            try
            {
                return GetPropertyValue(propertiesNode, propertyName);
            }
            catch (InvalidOperationException)
            {
                return defaultValue;
            }
        }

        private static XmlNode GetChildNode(XmlNode node, String nodeName)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == nodeName)
                {
                    return childNode;
                }
            }

            throw new InvalidOperationException(String.Format("No node with name {0} found", nodeName));
        }

        private static Tileset GetTilesetFromTileGid(List<Tileset> tilesets, int tileGid)
        {
            // We sort Tilesets on first gid with largest first.
            tilesets.Sort((a, b) => b.firstGid.CompareTo(a.firstGid));
            foreach (var tileset in tilesets)
            {
                if (tileGid < tileset.firstGid)
                {
                    continue;
                }
                else
                {
                    return tileset;
                }
            }
            throw new ArgumentException("Can't find tileset for tile gid " + tileGid + ".");
        }

        private static ImageLayer ParseImageNode(XmlNode imageNode, string sceneXmlPath, int z)
        {
            string imagePathRelative = imageNode.Attributes["source"].Value;
            string imagePath = Path.Combine(Path.GetFileNameWithoutExtension(Path.GetDirectoryName(sceneXmlPath)),
                                            Path.GetDirectoryName(imagePathRelative),
                                            Path.GetFileNameWithoutExtension(imagePathRelative));
            int width = int.Parse(imageNode.Attributes["width"].Value);
            int height = int.Parse(imageNode.Attributes["height"].Value);
            return new ImageLayer(imagePath, width, height, z);
        }

        private static List<Vector2> ParsePolygonXml(XmlNode node)
        {
            float baseX = float.Parse(node.Attributes["x"]?.InnerText);
            float baseY = float.Parse(node.Attributes["y"]?.InnerText);

            Debug.Assert(node.ChildNodes.Count > 0);
            XmlNode polygonNode = GetChildNode(node, "polygon");

            String polygonPointsString = polygonNode.Attributes["points"]?.InnerText;
            List<String> splitPolygonPointsString = polygonPointsString.Split(' ').ToList();

            List<Vector2> points = new List<Vector2>();
            foreach (String singlePointString in splitPolygonPointsString)
            {
                List<String> splitSinglePointString = singlePointString.Split(',').ToList();
                Debug.Assert(splitSinglePointString.Count == 2);
                float x = float.Parse(splitSinglePointString[0]);
                float y = float.Parse(splitSinglePointString[1]);
                Vector2 point = new Vector2(baseX + x, baseY + y);
                points.Add(point);
            }

            return points;
        }
    }
}
