using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!

namespace Sweet_Dreams
{
    // Ayvin Krug
    // Purpose: Used inside of Game1 to update and draw all enemies at once
    //          and to trigger new waves without needing any checks in Game1.

    /// <summary>
    /// Draws, updates, and creates waves of all enemies in a level.
    /// </summary>
    public class EnemyManager
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        private Queue<Enemy> allEnemies;
        private List<Enemy> currentEnemies;
        private List<Candy> collectibles;

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        /// <summary>
        /// Instantiates a manager for all enemies in a level.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        /// <param name="collectibles">Reference to the game's list of candy to draw.</param>
        public EnemyManager(string fileName, List<Candy> collectibles)
        {
            // Inits fields with empty data structures
            allEnemies = new Queue<Enemy>();
            currentEnemies = new List<Enemy>();

            // Fills the queue with enemy data from the file
            this.ReadEnemyData(fileName);

            // Gives a reference to the list of collectibles to be drawn
            this.collectibles = collectibles;
        }

        // --------------------------------------------------------------
        // Public methods
        // --------------------------------------------------------------
        /// <summary>
        /// Updates all enemy positions, removes them from the level and has them
        /// drop candies if they're dead, and dequeues more enemies into the level 
        /// if the current wave has finished.
        /// </summary>
        /// <param name="gameTime">Time information from MonoGame.</param>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        public void UpdateAll(GameTime gameTime, Vector2 worldToScreen)
        {
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                currentEnemies[i].Update(gameTime, worldToScreen);
            }

            // TODO: Add drop candy, remove from level, and next wave logic
        }

        /// <summary>
        /// Draws all enemies currently in the level to the screen (if they're in bounds).
        /// </summary>
        /// <param name="sb">The SpriteBatch object that does the drawing.</param>
        /// <param name="worldToScreen">Worldspace to screenspace offset vector.</param>
        public void DrawAll(SpriteBatch sb)
        {
            // Draws all enemies that will appear on the screen
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                if (currentEnemies[i].IsOnScreen)
                {
                    currentEnemies[i].Draw(sb);
                }
            }
        }

        // --------------------------------------------------------------
        // Private helper methods
        // --------------------------------------------------------------
        /// <summary>
        /// Fills the queue with all enemies that will spawn in the level,
        /// as determined by file data.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        private void ReadEnemyData(string fileName)
        {

        }

        /// <summary>
        /// Whether or not all enemies are gone.
        /// </summary>
        /// <returns>True if no more enemies exist or will be generated
        /// in the level, false otherwise.</returns>
        public bool IsLevelCleared()
        {
            return currentEnemies.Count == 0 && allEnemies.Count == 0;
        }
    }
}
