using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;
using PixelHunter1995.InventoryLib;
using PixelHunter1995.Utilities;

namespace PixelHunter1995.DialogLib
{
    class DialogChoicePrompt : IUpdateable
    {
        public static SpriteFont Font = FontManager.Instance.getFontByName("FreePixel");
        private static readonly int PROMPT_X_POS = 4;
        public static readonly int PROMPT_Y_POS = 160 + PROMPT_X_POS; // Use same margin on all sides
        private static readonly int WIDTH = GlobalSettings.WINDOW_WIDTH - (2 * PROMPT_X_POS);
        private static readonly int TEXT_HEIGHT = (int)Font.MeasureString("o").Y - 2;
        private static readonly int LINES = 5;

        public bool Active;
        private List<DialogChoice> Choices = new List<DialogChoice>();
        private int ScrollIndex = 0;

        public DialogChoicePrompt(List<string> choiceStrings)
        {
            int index = 0;
            foreach (string choiceString in choiceStrings)
            {
                Choices.Add(new DialogChoice(choiceString, index));
                index++;
            }
            Active = true;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if (!Active)
            {
                return;
            }
            foreach (DialogChoice choice in Choices)
            {
                choice.Draw(spriteBatch, ScrollIndex);
            }
            if (Choices.Count > LINES)
            {
                // TODO Draw scroll sidebar
            }
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (!Active)
            {
                return;
            }
            // Listen for scroll
            if (input.ScrollWheelDelta != 0)
            {
                ScrollIndex = ScrollIndex - input.ScrollWheelTicksDelta;  // Scrolling down gives a negative value
                ScrollIndex = Math.Min(ScrollIndex, Choices.Count - LINES);
                ScrollIndex = Math.Max(ScrollIndex, 0);
            }
            Vector2 mousePos = new Vector2(input.MouseX, input.MouseY);
            foreach (DialogChoice choice in Choices)
            {
                choice.Highlighted = choice.GetRect(ScrollIndex).Contains(mousePos);
                if (choice.Highlighted && input.Input.GetKeyState(MouseKeys.LeftButton).IsEdgeDown) // Left click
                {
                    Active = false;
                    return; // TODO figure out something smarter to do when someone clicks...
                }
            }
        }

        public int ZIndex()
        {
            return 0;
        }

        private class DialogChoice
        {
            private readonly string ChoiceString;
            private readonly int Index;
            private readonly Color DefaultFontColor = Color.Purple; // TODO we have this both here and in Player. Could be centralized.
            private readonly Color HighlightFontColor = Color.MediumPurple;
            public bool Highlighted = false;

            public DialogChoice(string choiceString, int index)
            {
                ChoiceString = choiceString;
                Index = index;
            }

            public Rectangle GetRect(int scrollIndex)
            {
                Vector2 pos = GetPos(scrollIndex);
                return new Rectangle((int)pos.X, (int)pos.Y, WIDTH, TEXT_HEIGHT);
            }

            private Vector2 GetPos(int scrollIndex)
            {
                return new Vector2(PROMPT_X_POS, PROMPT_Y_POS + (Index - scrollIndex) * TEXT_HEIGHT);
            }

            public void Draw(SpriteBatch spriteBatch, int scrollIndex)
            {
                if ((Index - scrollIndex) < 0 || (Index - scrollIndex) > LINES)
                {
                    return;
                }
                spriteBatch.DrawString(Font, ChoiceString, GetPos(scrollIndex),
                                       Highlighted ? HighlightFontColor : DefaultFontColor);
            }
        }
    }
}
