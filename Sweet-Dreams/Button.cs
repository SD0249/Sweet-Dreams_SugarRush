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
        // --------------------------------------------------------------
        // Felids :)
        // --------------------------------------------------------------
        private Rectangle Bounds;
        private Texture2D Texture;
        private MouseState mouse;

        // These two can totally be changed depending on the real
        // Button we use and the vibe of the game :)
        private Color NormalColor = Color.White;
        private Color HoveredOverColor = Color.Gray;

        private Color currentButtonColor;

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Button(Texture2D texture, Rectangle Bounds)
        {
            this.Texture = texture;
            this.Bounds = Bounds;

            // Sets the normal, not hovering over the button with the mouse,
            // color to the default C:
            currentButtonColor = NormalColor;
        }

        // --------------------------------------------------------------
        // Methods :P
        // --------------------------------------------------------------
        /// <summary>
        /// The update method for any button, used to 
        /// check if a button is being hovered overed
        /// </summary>
        /// <param name="mouse"> The mouse that would hover over the button </param>
        /// <returns> If the button is being hovered over </returns>
        public void Update(MouseState mouse)
        {
            Point mousePoint = new Point(mouse.X, mouse.Y);
            bool hoveringOverButton = Bounds.Contains(mousePoint);

            if (hoveringOverButton)
            {
                currentButtonColor = HoveredOverColor;
            }
            else
            {
                currentButtonColor = NormalColor;
            }

            // Shouldn't this only need to check if the button is being
            // hovered over? Also you call the same method in Game1
            // regardless of what Update returns...
            /*
            if (hoveringOverButton)
            {
                return true;
            }

            return false; */
        }

        /// <summary>
        /// Checks if the button is being 
        /// hovered over and clicked
        /// </summary>
        /// <param name="mouse"> The mouse that would hover over the button </param>
        /// <returns> If the button has been pressed </returns>
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

        /// <summary>
        /// Draws the button :|
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Bounds, 
                currentButtonColor);
        }

    }
}
