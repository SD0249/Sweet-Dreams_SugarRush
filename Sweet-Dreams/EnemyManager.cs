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
    /// <summary>
    /// Draws and updates all enemies in a level.
    /// </summary>
    public class EnemyManager
    {
        // Fields
        private Queue<Enemy> allEnemies;
        private List<Enemy> currentEnemies;

        // Constructor
        /// <summary>
        /// Instantiates a manager for all enemies in a level.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        public EnemyManager(string fileName)
        {
            // Inits fields with empty data structures
            allEnemies = new Queue<Enemy>();
            currentEnemies = new List<Enemy>();

            // Fills the queue with enemy data from the file
            this.ReadEnemyData(fileName);
        }

        // Public methods
        /// <summary>
        /// Updates all enemy positions, removes them from the level if they're dead,
        /// and also has them drop candy if they're dead.
        /// </summary>
        public void UpdateAll()
        {
            // TODO: Should the camera system translation matrix be a parameter?
        }

        // TODO: Add the rest of the method stubs

        // Private helper methods
        /// <summary>
        /// Fills the queue with all enemies that will spawn in the level,
        /// as determined by file data.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        private void ReadEnemyData(string fileName)
        {

        }
    }
}
