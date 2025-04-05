using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    internal class Button
    {
        // felids :)

        private Rectangle Bounds;
        private Texture2D Texture;
        private MouseState mouse;

        // these two can totally be changed depending on the real
        // button we use and the vibe of the game :)
        private Color NormalColor = Color.White;
        private Color HoveredOverColor = Color.Gray;

        private Color currentButtonColor;

        // constructor

        public Button(Texture2D texture, Rectangle Bounds)
        {
            this.Texture = texture;
            this.Bounds = Bounds;

            // sets the normal, not hovering over the button with the mouse,
            // color to the default C:
            currentButtonColor = NormalColor;
        }

        // methods :P

        public bool Update(MouseState mouse)
        {
            Point mousePoint = new Point(mouse.X, mouse.Y);
            bool hoveringOverButton = Bounds.Contains(mousePoint);

            if (hoveringOverButton == true)
            {
                currentButtonColor = HoveredOverColor;
            }
            else
            {
                currentButtonColor = NormalColor;
            }

            if (hoveringOverButton == true)
            {
                return true;
            }

            return false;
        }

        public bool buttonPressed(MouseState mouse)
        {
            Point mousePoint = new Point(mouse.X, mouse.Y);
            bool hoveringOverButton = Bounds.Contains(mousePoint);

            if (hoveringOverButton && mouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Bounds, 
                currentButtonColor);
        }

    }
}
