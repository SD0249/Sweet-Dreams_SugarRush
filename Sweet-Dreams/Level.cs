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

                        // Save the corresponding row and column data
                        // from the text file
                        int x = int.Parse(splitData[1]);
                        int y = int.Parse(splitData[2]);
                        spriteWidth = int.Parse(splitData[3]);
                        spriteHeight = int.Parse(splitData[4]);

                        // Add the corresponding tile asset information to the dictionary
                        textureMap.Add(splitData[0],                        // KEY
                                       new Rectangle(x,                     // X position
                                                     y,                     // Y position
                                                     spriteWidth,           // Width
                                                     spriteHeight));        // Height
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

                // A local variable used to draw a tile asset
                // to the appropriate location  
                int rowCount = 0;

                while ((line = reader.ReadLine()!) != null)
                {
                    splitData = line.Split(',');

                    // The line with 2 data has information
                    // of the intended size of a tile
                    if(splitData.Length == 2)
                    {
                        intendedSize = int.Parse(splitData[1]);
                    }
                    // The line with 3 data has information
                    // of the rows & columns of the tile array
                    else if(splitData.Length == 3)
                    {
                        column = int.Parse(splitData[1]);
                        row = int.Parse(splitData[2]);

                        tileSet = new LevelTile[column, row];
                    }
                    // The rest of the lines includes information about
                    // each individual tile's location in relation to the tileSet array
                    else
                    {
                        // The column and row information for each tile
                        // to store them at the right tileSet location
                        for(int c = 0; c < splitData.Length; c++)
                        {
                            // Before populating the tileSet,
                            // calculate the x and y position on the window to place a tile
                            int xPosition = c * intendedSize;
                            int yPosition = rowCount;

                            // Store each level tile object to the tileSet field.
                            tileSet[c, rowCount] = new LevelTile(spriteSheet,
                                                                 textureMap[splitData[c]], 
                                                                 new Rectangle
                                                                 (xPosition, yPosition, intendedSize, intendedSize));
                        }

                        // Increment the RowCount variable for the next background line to use
                        rowCount++;
                    }
                }

                // Close the stream to ensure the level loading is complete
                reader.Close();
            }
            // Throw an exception if there is an error in the process
            catch(Exception error)
            {
                Debug.WriteLine("There was an error in Level Loading.");
                Debug.WriteLine("Check the following the details: " + error.Message);
            }
        }


        /// <summary>
        /// Draws all level tiles to the console window
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with.</param>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        public void DisplayTiles(SpriteBatch sb, Vector2 worldToScreen)
        {
            for(int r = 0; r < row; r++)
            {
                for(int c = 0; c < column; c++)
                {
                    // Draws all tiles that would be at all visible on the screen
                    if (tileSet[r, c].IsOnScreen(worldToScreen))
                    {
                        tileSet[r, c].Draw(sb);
                    }
                }
            }
        }
    }
}
