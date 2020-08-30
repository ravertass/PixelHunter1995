using Microsoft.Xna.Framework;
using PixelHunter1995.Utilities;

namespace PixelHunter1995
{
    class Camera
    {
        private static readonly double CAMERA_SPEED = 1.5;
        private static readonly int START_MOVING_OFFSET = 100;
        private static readonly int STOP_MOVING_OFFSET = 2;

        private bool moving;
        public double X { get; private set; }

        public Camera()
        {
            moving = false;
            X = 0;
        }

        public Matrix GetTransformation()
        {
            // NOTE: We only support moving the camera in the X dimension as of now.
            return Matrix.CreateTranslation(new Vector3((int)-X, 0, 0));
        }

        public void Update(Vector2 playerPosition, int currentSceneWidth)
        {
            int playerXOnScreen = (int)(playerPosition.X - X);
            int xOffsetFromMid = playerXOnScreen - (GlobalSettings.WINDOW_WIDTH / 2);

            // Start moving if player has strayed too far from center.
            if ((xOffsetFromMid > START_MOVING_OFFSET) || (xOffsetFromMid < -START_MOVING_OFFSET))
            {
                moving = true;
            }

            if (moving)
            {
                // Move until player is (kind of) centered.
                if (xOffsetFromMid > STOP_MOVING_OFFSET)
                {
                    X += CAMERA_SPEED;
                }
                else if (xOffsetFromMid < -STOP_MOVING_OFFSET)
                {
                    X -= CAMERA_SPEED;
                }
                else
                {
                    moving = false;
                }
            }

            ClampWithinScreen(currentSceneWidth);
        }

        private void ClampWithinScreen(int currentSceneWidth)
        {
            if (X < 0)
            {
                X = 0;
            }

            if ((X + GlobalSettings.WINDOW_WIDTH) > currentSceneWidth)
            {
                X = currentSceneWidth - GlobalSettings.WINDOW_WIDTH;
            }
        }
    }
}
