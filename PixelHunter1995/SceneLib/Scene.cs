using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.SceneLib;
using PixelHunter1995.Utilities;
using PixelHunter1995.WalkingAreaLib;
using System.Collections.Generic;

namespace PixelHunter1995
{
    class Scene : ILoadContent
    {
        private List<IDrawable> Drawables;
        private List<IUpdateable> Updateables;
        private List<ILoadContent> Loadables;
        private IDictionary<string, Portal> Portals;
        public List<IDog> Dogs;
        public Player Player;
        public WalkingArea WalkingArea;
        private readonly float CharacterScalingMin;

        public int Width { get; private set; }

        public Scene(List<IDrawable> drawables,
                     List<IUpdateable> updateables,
                     List<ILoadContent> loadables,
                     List<IDog> dogs,
                     IDictionary<string, Portal> portals,
                     Player player,
                     WalkingArea walkingArea,
                     int width,
                     float characterScalingMin)
        {
            Player = player;
            Drawables = drawables;
            Drawables.Add(player);
            Updateables = updateables;
            Loadables = loadables;
            Loadables.Add(player);
            Dogs = dogs;
            Portals = portals;
            Player = player;
            Width = width;
            WalkingArea = walkingArea;
            CharacterScalingMin = characterScalingMin;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            // We sort on Z and draw lowest first.
            Drawables.Sort((a, b) => a.ZIndex().CompareTo(b.ZIndex()));
            foreach (IDrawable drawable in this.Drawables)
            {
                drawable.Draw(graphics, spriteBatch, CharacterScalingMin);
            }
        }

        public void Update(GameTime gameTime, InputManager input, bool playerControllable)
        {
            Player.Update(gameTime, input, playerControllable);
            foreach (IUpdateable updateable in this.Updateables)
            {
                updateable.Update(gameTime, input);
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (ILoadContent loadable in this.Loadables)
            {
                loadable.LoadContent(content);
            }
        }

        public bool GetDogAtCursor(InputManager input, out IDog dogAtCursor)
        {
            dogAtCursor = null;

            Vector2 mousePos = new Vector2(input.MouseSceneX, input.MouseSceneY);
            // We sort on Z index, to check the top dog first. Note that this is reversed from
            // when we draw them, since in that case we want to draw the thing on top last.
            Dogs.Sort((a, b) => b.ZIndex().CompareTo(a.ZIndex()));
            foreach (IDog dog in Dogs)
            {
                if (dog.Contains(mousePos))
                {
                    dogAtCursor = dog;
                    return true;
                }
            }

            return false;
        }

        public bool GetPortalAtCursor(InputManager input, out Portal portalAtCursor)
        {
            portalAtCursor = null;

            Vector2 mousePos = new Vector2(input.MouseSceneX, input.MouseSceneY);
            foreach (Portal portal in Portals.Values)
            {
                if (portal.Contains(mousePos))
                {
                    portalAtCursor = portal;
                    return true;
                }
            }

            return false;
        }

        public Portal GetPortalByName(string name)
        {
            return Portals[name];
        }
    }
}
