using System;
using System.Xml;

namespace PixelHunter1995
{
    class SceneParser
    {
        public static void ParseSceneXml(String sceneXmlPath)
        {
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
                    // TODO: Load walking area
                    Console.WriteLine("walking " + node.Name);
                }
            }
        }
    }
}
