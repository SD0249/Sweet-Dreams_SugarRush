using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /* Amy Lee, Ayvin Krug
     * Purpose: A LevelTile class to represent a single tile that 
     *          will be used to form a level from the level class. */
    public class LevelTile
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------

        // Where in the sprite sheet to get the tile
        private Rectangle sourceRectangle;

        // Where and what size to draw the tile in the world
        private Rectangle drawnRectangle;

        // The original sprite sheet to get the tile asset
        private Texture2D spriteSheet;


        // --------------------------------------------------------------
        // Parameterized Constructor
        // --------------------------------------------------------------
        /// <summary>
        /// Setting the necessary fields for a 
        /// single tile instance to function.
        /// </summary>
        /// <param name="spriteSheet">The original sprite sheet to get the tile asset</param>
        /// <param name="sourceRectangle">Where in the sprite sheet to get the tile</param>
        /// <param name="drawnRectangle"> Where and what size to draw the tile</param>
        public LevelTile
            (Texture2D spriteSheet, Rectangle sourceRectangle, Rectangle drawnRectangle)
        {
            this.spriteSheet = spriteSheet;
            this.sourceRectangle = sourceRectangle;
            this.drawnRectangle = drawnRectangle;
        }

        // --------------------------------------------------------------
        // Class Methods
        // --------------------------------------------------------------
        /// <summary>
        /// Draws a single Level tile object on the game window
        /// </summary>
        /// <param name="sb">SpriteBatch object used to 
        /// draw the object on the game window</param>
        public void Draw(SpriteBatch sb, Vector2 worldToScreen)
        {
            sb.Draw(spriteSheet, 
                new Rectangle(drawnRectangle.X + (int)worldToScreen.X,
                    drawnRectangle.Y + (int)worldToScreen.Y,
                    drawnRectangle.Width,
                    drawnRectangle.Height),
                sourceRectangle, Color.White);
        }

        /// <summary>
        /// TODO: Get rid of this; maybe add IsOnScreen property instead
        /// </summary>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        /// <returns>Whether or not the tile is at all visible on the screen.</returns>
        public bool IsOnScreen(Vector2 worldToScreen, int screenWidth, int screenHeight)
        {
            // Returns false if any of the following out of bounds conditions are true
            return !(drawnRectangle.X + drawnRectangle.Width < worldToScreen.X      // Too far left
                || drawnRectangle.X > screenWidth - worldToScreen.X                 // Too far right
                || drawnRectangle.Y + drawnRectangle.Height < worldToScreen.Y       // Too far up
                || drawnRectangle.Y > screenHeight - worldToScreen.Y);              // Too far down
        }
    }
}
