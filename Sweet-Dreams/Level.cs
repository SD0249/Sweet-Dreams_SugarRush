using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public class Level
    {
        // ----------------------------------------------------------------
        // Fields
        // ----------------------------------------------------------------

        // A Texture2D asset that includes all the assets for the background
        private Texture2D spriteSheet;

        // A SpriteBatch field used to draw the level to the game window
        // private SpriteBatch spriteBatch;

        // The intended Size of each level tile in pixels drawn on the game window.
        private int intendedSize;

        // The 2D array of LevelTiles to store tile information
        // of the level read from the file
        private LevelTile[,] tileSet;

        // The column dimension of the tile set. 
        private int column;

        // The row dimension of the tile set.
        private int row;

        // A string-Rectangle dictionary to store the asset associated
        // with a particular tile using the source rectangle
        private Dictionary<string, Rectangle> textureMap;


        // ----------------------------------------------------------------
        // Properties
        // ----------------------------------------------------------------

        // World Width & Height returned for future clamping
        public int WorldWidth
        {
            get { return column * intendedSize; }
        }

        public int WorldHeight
        {
            get { return row * intendedSize; }
        }

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
                StreamReader reader = new StreamReader(filePath);

                // Variables to temporarily store the information
                // read from the file
                string line = "";
                string[] splitData = null;

                while((line = reader.ReadLine()!) != null)
                {
                    // The line that starts with either
                    // of these characters should be ignored.
                    if(!line.StartsWith('/') && !line.StartsWith('-'))
                    {
                        splitData = line.Split(',');

                        // We have the size of one tile from the file
                        if(splitData.Length == 2)
                        {
                            spriteWidth = int.Parse(splitData[0]);
                            spriteHeight = int.Parse(splitData[1]);
                        }

                        // We have a tile to populate the dictionary
                        if(splitData.Length == 3)
                        {
                            // Save the corresponding row and column data
                            // from the text file
                            int column = int.Parse(splitData[1]);
                            int row = int.Parse(splitData[2]);

                            textureMap.Add(splitData[0],                        // KEY
                                           new Rectangle(column * spriteWidth,  // X position
                                                         row * spriteHeight,    // Y position
                                                         spriteWidth,           // Width
                                                         spriteHeight));        // Height
                        }
                    }
                }

                // Close the stream to ensure the population of Dictionary is saved
                reader.Close();
            }
            // Throw an exception if there is an error in the process
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
            // Read the level data from a text file and
            // give information for the tileset field.
            try
            {
                // Open the reader
                StreamReader reader = new StreamReader(filePath);

                // Variables to temporarily store information read in from the file
                string line = "";
                string[] splitData = null;



            }
            // Throw an exception if there is an error in the process
            catch(Exception error)
            {

            }
        }


        /// <summary>
        /// Draws all the level tile to the console window
        /// </summary>
        public void DisplayTiles(SpriteBatch sb)
        {
            for(int r = 0; r < tileSet.GetLength(0); r++)
            {
                for(int c = 0; c < tileSet.GetLength(1); c++)
                {
                    tileSet[r, c].Draw(sb);
                }
            }
        }
    }
}
