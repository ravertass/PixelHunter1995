using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PixelHunter1995
{
    class SceneHandler
    {
        List<Scene> scenes;
        Scene currentScene;

        public SceneHandler()
        {
            scenes = new List<Scene>();
        }
        public void AddScene(Scene scene)
        {
            scenes.Add(scene);
        }
        public List<string> Images()
        {
            List<string> content = new List<string>();
            foreach (Scene scene in scenes)
            {
                content.AddRange(scene.Images());
            }
            return content;
        }
    }
}
