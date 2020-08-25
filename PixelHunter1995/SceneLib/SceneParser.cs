﻿using Microsoft.Xna.Framework;
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
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            List<Tileset> tilesets = new List<Tileset>();
            List<IDrawable> drawables = new List<IDrawable>();
            List<IUpdateable> updateables = new List<IUpdateable>();
            List<IDog> dogs = new List<IDog>();
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
                        IDog dog;
                        int x = (int)Math.Round(float.Parse(dogNode.Attributes["x"].Value));
                        int y = (int)Math.Round(float.Parse(dogNode.Attributes["y"].Value));
                        if (dogNode.Attributes["gid"] != null)
                        {
                            int width = (int)Math.Round(float.Parse(dogNode.Attributes["width"].Value));
                            int height = (int)Math.Round(float.Parse(dogNode.Attributes["height"].Value));
                            int gid = int.Parse(dogNode.Attributes["gid"].Value);
                            y = y - height; // Compensate for Tiled's coordinate system
                            Tileset tileset = GetTilesetFromTileGid(tilesets, gid);
                            dog = new Dog(x, y, width, height, gid, tileset);
                        }
                        else
                        {
                            dog = new PolygonDog(x, y, ParsePolygonXml(dogNode));
                        }
                        drawables.Add(dog);
                        dogs.Add(dog);
                    }
                }
                else if (node.Name == "objectgroup" && node.Attributes["name"]?.InnerText == "walking")
                {
                    WalkingArea walkingArea = new WalkingArea(ParsePolygonXml(node.ChildNodes[0]));
                    drawables.Add(walkingArea);

                    // TODO Make player its own objectgroup, or part of "portal", or something similar.
                    if (player == null)
                    {
                        player = new Player(50, 50);
                        drawables.Add(player);
                        updateables.Add(player);
                        loadables.Add(player);
                    }
                }
            }
            return new Scene(drawables, updateables, loadables, dogs);
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

        private static List<Coord> ParsePolygonXml(XmlNode node)
        {
            // TODO: We should probably allow multiple walking areas.

            float baseX = float.Parse(node.Attributes["x"]?.InnerText);
            float baseY = float.Parse(node.Attributes["y"]?.InnerText);

            Debug.Assert(node.ChildNodes.Count > 0);
            XmlNode polygonNode = null;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "polygon")
                {
                    polygonNode = childNode;
                    break;
                }
            }

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

            return points;
        }
    }
}
