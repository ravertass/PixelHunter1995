using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelHunter1995.Inputs;

namespace PixelHunter1995.Components
{
    internal class Voice : IUpdateable
    {
        private string CurrentLine;
        private TimeSpan LineEndTime;
        public bool Speaking { get; private set; }
        private readonly Queue<string> UpcomingLines;
        private static readonly int TALKING_SPEED = 100;  // Number of milliseconds per character in line
        private static readonly int CHARS_PER_LINE = 40;

        public Voice()
        {
            UpcomingLines = new Queue<string>();
        }

        public void Say(string speech)
        {
            UpcomingLines.Enqueue(speech);
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            bool hasSpoken = LineEndTime != null;
            if (hasSpoken)
            {
                bool finishedCurrentLine = LineEndTime < gameTime.TotalGameTime;
                if (!finishedCurrentLine)
                {
                    return;  // Not ready to say new line
                }
            }
            bool hasNewLine = UpcomingLines.Count > 0;
            if (hasNewLine)
            {
                Speaking = true;
                NextLine(gameTime);
            }
            else
            {
                Speaking = false;
            }
        }

        private void NextLine(GameTime gameTime)
        {
            CurrentLine = UpcomingLines.Dequeue();
            var speechDuration = new TimeSpan(0, 0, 0, 0, TALKING_SPEED * CurrentLine.Length);
            LineEndTime = gameTime.TotalGameTime + speechDuration;
        }

        public void Draw(SpriteBatch spriteBatch, string fontName, Color fontColor, Vector2 charCenterPos)
        {
            if (!Speaking)
            {
                return;
            }
            // We need to cut the current speech line into... lines
            int index = 1;
            var lines = SplitWordChunks(CurrentLine, CHARS_PER_LINE);
            lines = lines.Reverse();
            foreach (string line in lines)
            {
                SpriteFont font = FontManager.Instance.getFontByName(fontName);
                int deltaX = -(int)font.MeasureString(line).X / 2;
                int deltaY = (-(int)font.MeasureString(line).Y * index) - 5;
                // Draw black around the letters to see them better
                spriteBatch.DrawString(font, line, charCenterPos + new Vector2(deltaX + 1, deltaY + 1), Color.Black);
                spriteBatch.DrawString(font, line, charCenterPos + new Vector2(deltaX - 1, deltaY - 1), Color.Black);
                spriteBatch.DrawString(font, line, charCenterPos + new Vector2(deltaX, deltaY), fontColor);
                index++;
            }
        }

        private static IEnumerable<string> SplitWordChunks(string speech, int chunkSize)
        {
            var words = speech.Split(' ');
            List<string> chunkWords = new List<string>();
            foreach (string word in words)
            {
                var candidate = string.Join(" ", chunkWords);
                if (candidate.Length + word.Length > chunkSize)  // No more word can fit
                {
                    yield return candidate;
                    chunkWords = new List<string>();
                }
                chunkWords.Add(word);
            }
            yield return string.Join(" ", chunkWords);
        }
    }
}
