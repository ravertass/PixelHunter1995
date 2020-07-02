using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace PixelHunter1995
{
    using Dog = System.ValueTuple<float, float, float, float>;
    class SceneParser
    {
        public static Scene ParseSceneXml(String sceneXmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            Background background = null;
            WalkingArea walkingArea = null;
            List<Dog> dogs = null;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == "imagelayer" && node.Attributes["name"]?.InnerText == "background")
                {
                    Debug.Assert(node.ChildNodes.Count == 1);
                    XmlNode imageNode = node.ChildNodes[0];
                    string image = imageNode.Attributes["source"].Value;
                    int width = int.Parse(imageNode.Attributes["width"].Value);
                    int height = int.Parse(imageNode.Attributes["height"].Value);
                    background = new Background(image, width, height);
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "dogs")
                {
                    dogs = new List<Dog>();
                    foreach (XmlNode dogNode in node.ChildNodes)
                    {
                        float x = float.Parse(dogNode.Attributes["x"].Value);
                        float y = float.Parse(dogNode.Attributes["y"].Value);
                        float width = float.Parse(dogNode.Attributes["width"].Value);
                        float height = float.Parse(dogNode.Attributes["height"].Value);
                        dogs.Add((x, y, width, height));
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    walkingArea = ParseWalkingXml(node);
                }
            }
            return new Scene(background, dogs, walkingArea);
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
