using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

namespace PixelHunter1995
{
    using Dog = System.ValueTuple<float, float, float, float>;
    class SceneParser
    {
        public static Scene ParseSceneXml(string sceneXmlPath, Game game)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(sceneXmlPath);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            List<Dog> dogs = null;
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
                    background = new Background(imagePath, width, height);
                    // TODO Preferably, all these things (background, player, etc.) should add themselves to the lists.
                    // TODO Maybe as part of constructor?
                    drawables.Add(background);
                    loadables.Add(background);
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
                        Dog dog = (x, y, width, height);
                        dogs.Add(dog);
                        // TODO Dogs are supposed to be drawable. But currently a valuetuple
                        //drawables.Add(dog);
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    walkingArea = ParseWalkingXml(node);
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
