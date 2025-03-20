using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /* Amy Lee
     * Purpose: A LevelTile class to represent a single tile that 
     *          will be used to form a level from the level class. */
    internal class LevelTile
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------

        // Where in the sprite sheet to get the tile
        private Rectangle sourceRectangle;

        // Where and what size to draw the tile
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
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, drawnRectangle, sourceRectangle, Color.White);
        }
    }
}
