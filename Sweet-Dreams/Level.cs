using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /* Amy Lee
     * Purpose: A Level class that uses the LevelTile objects as 
     *          the basic building blocks of a level background. 
     *          Loads information from the file and constructs the level accordingly. */
    internal class Level
    {
        // ----------------------------------------------------------------
        // Fields
        // ----------------------------------------------------------------

        // A Texture2D asset that includes all the assets for the background
        private Texture2D spriteSheet;

        // A SpriteBatch field used to draw the level to the game window
        private SpriteBatch spriteBatch;

        // The intended Size of each level tile in pixels.
        private int intendedSize;

        // The 2D array of LevelTiles to store tile information
        // of the level read from the file
        private LevelTile[,] tileSet;

        // A string-Rectangle dictionary to store the asset associated
        // with a particular tile using the source rectangle
        private Dictionary<string, Rectangle> textureMap;


        // ----------------------------------------------------------------
        // Parameterized Constructor
        // ----------------------------------------------------------------
        /// <summary>
        /// Constructs a Level instance. 
        /// Sets the fields of the Level class for it to function 
        /// and performs texture mapping.
        /// </summary>
        /// <param name="spriteSheet">
        /// Texture2D asset that includes all the assets for the background</param>
        /// <param name="filePath">File Path to read the level information from.</param>
        /// <param name="sb">SpriteBatch objected needed to draw the textures.</param>
        public Level(Texture2D spriteSheet, string filePath, SpriteBatch sb)
        {
            // **********************************************************
            // Setting the fields of the Level class
            // **********************************************************
            this.spriteSheet = spriteSheet;
            this.spriteBatch = sb;
            this.textureMap = new Dictionary<string, Rectangle>();


            // **********************************************************
            // Texture Mapping -->
            // Stores source rectangle of each asset in the Dictionary Field
            // **********************************************************
            // Each sprite size in the original spritesheet for texture mapping
            // (Comes from the file)
            int spriteWidth = 0;
            int spriteHeight = 0;

            try
            {

            }
            catch(Exception error)
            {
                Debug.WriteLine("There was an error in texture mapping.");
                Debug.WriteLine("Check the following the details: " + error.Message);
            }

        }


        // ----------------------------------------------------------------
        // Class Methods
        // ----------------------------------------------------------------

        /// <summary>
        /// Reads information from the file and constructs the
        /// LevelTile 2D array with correct information
        /// </summary>
        /// <param name="filePath">File Path to read the level information from.</param>
        public void LoadLevel(string filePath)
        {

        }


        /// <summary>
        /// Draws all the level tile to the console window
        /// </summary>
        public void DisplayTiles()
        {
            for(int r = 0; r < tileSet.GetLength(0); r++)
            {
                for(int c = 0; c < tileSet.GetLength(1); c++)
                {
                    tileSet[r, c].Draw(spriteBatch);
                }
            }
        }
    }
}
