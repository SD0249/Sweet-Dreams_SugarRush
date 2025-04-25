using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /// <summary>
    /// A hoverable and clickable button.
    /// </summary>
    public class Button
    {
        // --------------------------------------------------------------
        // Felids :)
        // --------------------------------------------------------------
        private Rectangle bounds;
        private Rectangle sourceRec;
        private Texture2D texture;
        private bool isHovered;

        // These two can totally be changed depending on the real
        // Button we use and the vibe of the game :)
        private Color NormalColor = Color.White;
        private Color HoveredOverColor = Color.Gray;

        private Color currentButtonColor;

        // --------------------------------------------------------------
        // Property
        // --------------------------------------------------------------
        public bool IsHovered
        {
            get { return isHovered; }
        }


        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Button(Texture2D texture, Rectangle bounds, Rectangle sourceRec)
        {
            this.texture = texture;
            this.bounds = bounds;
            this.sourceRec = sourceRec;

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

            if (bounds.Contains(mousePoint))
            {
                currentButtonColor = HoveredOverColor;
                isHovered = true;
            }
            else
            {
                currentButtonColor = NormalColor;
                isHovered = false;
            }
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

            if (bounds.Contains(mousePoint) && mouse.LeftButton == ButtonState.Pressed)
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
                texture,
                bounds, 
                sourceRec,
                currentButtonColor);
        }
    }
}
