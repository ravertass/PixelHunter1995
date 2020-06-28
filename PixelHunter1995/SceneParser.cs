using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace PixelHunter1995
{
    class SceneParser
    {
        public static void ParseSceneXml(String sceneXmlPath)
        {
            // TODO: Make this function return a Scene object.

            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.Name == "imagelayer" && node.Attributes["name"]?.InnerText == "background")
                {
                    // TODO: Load background
                    Console.WriteLine("background " + node.Name);
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "dogs")
                {
                    // TODO: Load dogs
                    Console.WriteLine("dogs " + node.Name);
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    // TODO: Add to scene
                    Console.WriteLine(ParseWalkingXml(node));
                }
            }
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
