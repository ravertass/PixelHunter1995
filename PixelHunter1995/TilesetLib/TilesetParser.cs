using System.IO;
using System.Xml;

namespace PixelHunter1995.TilesetLib
{
    class TilesetParser
    {
        public static Tileset ParseTilesetXml(string tilesetXmlPath, int firstGid)
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
    }
}
