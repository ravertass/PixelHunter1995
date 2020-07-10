using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelHunter1995.Components;
using System;

namespace PixelHunter1995
{
    /// <summary>
    /// This is slated to be removed, only temporarily has it as I am playing around with different implementations
    /// </summary>
    interface IPlayer : IUpdateable, IDrawable
    {

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        void LoadContent(ContentManager content);
    }
}
