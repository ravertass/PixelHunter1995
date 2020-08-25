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
                string filename = Path.GetFileName(filepath);

                scenes.Add(filename, SceneParser.ParseSceneXml(filepath));
            };
        }

        public void SetCurrentSceneByName(string sceneName)
        {
            currentScene = scenes[sceneName];
        }
    }
}
