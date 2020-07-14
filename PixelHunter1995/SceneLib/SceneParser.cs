using PixelHunter1995.SceneLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace PixelHunter1995
{
    class SceneParser
    {
        public static Scene ParseSceneXml(string sceneXmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            Background background = null;
            Tileset tileset = null; // TODO: Possible to have many tilesets per scene?
            WalkingArea walkingArea = null;
            List<Dog> dogs = null;

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
                    background = new Background(imagePath, width, height);
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
                    dogs = new List<Dog>();
                    foreach (XmlNode dogNode in node.ChildNodes)
                    {
                        int x = (int)Math.Round(float.Parse(dogNode.Attributes["x"].Value));
                        int y = (int)Math.Round(float.Parse(dogNode.Attributes["y"].Value));
                        int width = (int)Math.Round(float.Parse(dogNode.Attributes["width"].Value));
                        int height = (int)Math.Round(float.Parse(dogNode.Attributes["height"].Value));
                        int gid = int.Parse(dogNode.Attributes["gid"].Value);
                        y = y - height; // Compensate for Tiled's coordinate system
                        dogs.Add(new Dog(x, y, width, height, gid));
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    walkingArea = ParseWalkingXml(node);
                }
            }
            return new Scene(background, dogs, walkingArea, tileset);
        }

        private static Tileset ParseTilesetXml(string tilesetXmlPath, int firstGid)
        {
            string imagePath = "";
            int imageWidth = 0;
            int imageHeight = 0;
            string name = "";
            int tileWidth = 0;
            int tileHeight = 0;
            int tileCount = 0;
            int noOfColumns = 0;

            XmlDocument doc = new XmlDocument();
            doc.Load(tilesetXmlPath);

            foreach (XmlAttribute attribute in doc.DocumentElement.Attributes)
            {
                if (attribute.Name == "name")
                {
                    name = attribute.Value;
                }
                else if (attribute.Name == "tilewidth")
                {
                    tileWidth = int.Parse(attribute.Value);
                }
                else if (attribute.Name == "tileheight")
                {
                    tileHeight = int.Parse(attribute.Value);
                }
                else if (attribute.Name == "tilecount")
                {
                    tileCount = int.Parse(attribute.Value);
                }
                else if (attribute.Name == "columns")
                {
                    noOfColumns = int.Parse(attribute.Value);
                }
            }

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
