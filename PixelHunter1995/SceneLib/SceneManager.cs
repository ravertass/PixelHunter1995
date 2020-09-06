using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace PixelHunter1995
{
    class SceneManager
    {
        public Dictionary<string, Scene> scenes;
        public Scene currentScene;

        public SceneManager()
        {
        }

        public void Initialize(string scenesPath)
        {
            scenes = new Dictionary<string, Scene>();
            foreach (string filepath in Directory.GetFiles(scenesPath))
            {
                string fileName = Path.GetFileName(filepath);
                string suffix = ".tmx";
                string sceneName = fileName.Substring(0, fileName.Length - suffix.Length);

                scenes.Add(sceneName, SceneParser.ParseSceneXml(filepath));
            }
        }

        public void SetCurrentSceneByName(string sceneName)
        {
            if (!scenes.ContainsKey(sceneName))
            {
                throw new InvalidDataException(string.Format("Could not find scene with scene name {0}", sceneName));
            }
            currentScene = scenes[sceneName];
        }
    }
}
