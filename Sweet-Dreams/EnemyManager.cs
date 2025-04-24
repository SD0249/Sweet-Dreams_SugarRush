using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        private List<Bullet> playerBullets;
        private List<Bullet> enemyBullets;
        private Texture2D candyAsset;
        private Player player;
        private float rotation;
        private int screenWidth;
        private int screenHeight;
        private int worldWidth;
        private int worldHeight;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        /// <summary>
        /// List of enemies' world positons as (X,Y) vectors. Used for testing.
        /// </summary>
        public List<Vector2> WorldPositions
        {
            get
            {
                List<Vector2> positions = new List<Vector2>();
                foreach (Enemy singleEnemy in currentEnemies)
                {
                    positions.Add(new Vector2(singleEnemy.WorldPosition.X, 
                        singleEnemy.WorldPosition.Y));
                }
                return positions;
            }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        /// <summary>
        /// Instantiates a manager for all enemies in a level.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        /// <param name="collectibles">Reference to the game's list of candy to draw.</param>
        public EnemyManager(Random rng, string fileName, List<Candy> collectibles, 
            List<Bullet> playerBullets, Player player, Texture2D enemyAsset, Texture2D candyAsset, 
            int screenWidth, int screenHeight, int worldWidth, int worldHeight)
        {
            // Initializes fields with empty data structures
            allEnemies = new Queue<Enemy>();
            currentEnemies = new List<Enemy>();
            enemyBullets = new List<Bullet>();

            // Saves screen/world's width & height
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;

            // Fills the queue with enemy data from the file
            this.ReadEnemyData(rng, fileName, enemyAsset, player, screenWidth, 
                screenHeight, worldWidth, worldHeight);

            // Gains a reference to the list of candy to be drawn plus their spritesheet
            this.collectibles = collectibles;
            this.candyAsset = candyAsset;

            // Gains a reference to the list of objects that can interact with enemies
            this.playerBullets = playerBullets;

            // Gains a reference to a player
            this.player = player;

            // Creates the first wave of enemies
            NewWave(10);
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
        public void UpdateAll(GameTime gameTime, Level level1, OrthographicCamera camera)
        {
            // Updates all enemy positions and states
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                currentEnemies[i].Update(gameTime);
            }

            // Checks for player-enemy collisions unless god mode is on
            if (!Game1.GodMode)
            {
                for (int i = 0; i < currentEnemies.Count; i++)
                {
                    if (player.CollidesWith(currentEnemies[i]))
                    {
                        // Player takes damage
                        player.Health -= currentEnemies[i].Damage;
                        player.Hurt = true;

                        // Teleports the enemy back to the walls
                        currentEnemies[i].GoToWorldEdge();
                    }
                }
            }

            // Removes bullets and damages enemies if they collide
            for (int enemyIndex = 0; enemyIndex < currentEnemies.Count; enemyIndex++)
            {
                for (int bulletIndex = 0; bulletIndex < playerBullets.Count; bulletIndex++)
                {
                    if (currentEnemies[enemyIndex].CollidesWith(playerBullets[bulletIndex]))
                    {
                        currentEnemies[enemyIndex].Health -= playerBullets[bulletIndex].Damage;
                        playerBullets[bulletIndex].HitEnemy = true;
                    }
                }
            }

            // Any bullets that hit enemies get removed from the list
            for (int i = 0; i < playerBullets.Count; i++)
            {
                if (playerBullets[i].HitEnemy)
                {
                    playerBullets.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < enemyBullets.Count; i++)
            {
                enemyBullets[i].Update(gameTime);

                if (enemyBullets[i].WorldPosition.Intersects(player.WorldPosition) &&
                    !Game1.GodMode)
                {
                    player.Hurt = true;
                    enemyBullets.RemoveAt(i);
                    i--;
                }

                // Removes the bullet from the list if it is out of bounds
                if (i >= 0 &&
                    enemyBullets[i].OutOfBounds(level1.WorldWidth, level1.WorldHeight))
                {
                    enemyBullets.RemoveAt(i);
                    i--;
                }
            }
            

            // If any enemy is dead, it drops candy then gets removed from the list
            // Also, if any of the enemies should add a bullet, do that
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                currentEnemies[i].Attack(camera);

                if (currentEnemies[i].AddBullet)
                {
                    // Finding the rotation of the bullet based off the mouse
                    rotation = (float)Math.Atan2(currentEnemies[i].WorldPosition.Y - player.WorldPosition.Y,
                                                 currentEnemies[i].WorldPosition.X - player.WorldPosition.X);

                    // Makes a new bullet
                    enemyBullets.Add(new Bullet(candyAsset, 
                                                new Rectangle(currentEnemies[i].WorldPosition.X,
                                                              currentEnemies[i].WorldPosition.Y,
                                                              16,
                                                              16),
                                                new Rectangle(64, 96, 16, 16),
                                                1, 
                                                screenWidth, 
                                                screenHeight, 
                                                5,
                                                worldWidth, 
                                                worldHeight, 
                                                rotation));

                    currentEnemies[i].AddBullet = false;
                }

                if (!currentEnemies[i].IsAlive)
                {
                    currentEnemies[i].DropCandy(collectibles, candyAsset);
                    currentEnemies.RemoveAt(i);
                    i--;
                }

            }

            // A new wave of ten enemies forms once all current enemies are gone
            if (currentEnemies.Count == 0)
            {
                NewWave(10);
            }
        }

        /// <summary>
        /// Draws all enemies and enemy bullets to the screen (unless they're out of bounds).
        /// </summary>
        /// <param name="sb">The SpriteBatch object that does the drawing.</param>
        public void DrawAll(SpriteBatch sb, OrthographicCamera camera)
        {
            // Draws all enemies that will appear on the screen
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                if (currentEnemies[i].IsOnScreen(camera))
                {
                    currentEnemies[i].Draw(sb);
                }
            }

            // Draws all enemy bullets that will appear on the screen
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                if (enemyBullets[i].IsOnScreen(camera))
                {
                    enemyBullets[i].Draw(sb);
                }
            }
        }

        /// <summary>
        /// Whether or not all enemies are gone.
        /// </summary>
        /// <returns>True if no more enemies exist or will be generated
        /// in the level, false otherwise.</returns>
        public bool IsLevelCleared()
        {
            if (currentEnemies.Count == 0 && allEnemies.Count == 0)
            {
                return true;
            }
            if (player.Health <=0 && currentEnemies.Count !=0)
            {
                currentEnemies.Clear();
                return false;
            }
            if (player.Health == 5)
            {
                // ReadEnemyData();
                return false;
            }

            return false;
        }

        // --------------------------------------------------------------
        // Private helper methods
        // --------------------------------------------------------------
        /// <summary>
        /// Fills the queue with all enemies that will spawn in the level,
        /// as determined by file data.
        /// </summary>
        /// <param name="fileName">File containing all enemy data for the level.</param>
        private void ReadEnemyData(Random rng, string fileName, Texture2D asset, Player player,
            int screenWidth, int screenHeight, int worldWidth, int worldHeight)
        {
            // File reading/writing:
            StreamReader reader = null;
            string lineFromFile = "";
            EnemyType enemyType = EnemyType.Imp;

            try
            {
                //Creating a new StreamReader object
                reader = new StreamReader(fileName);

                //Read the data from the file
                while ((lineFromFile = reader.ReadLine()) != null)
                {
                    // Skips any lines that start with a doulbe slash
                    while(lineFromFile.Split(' ')[0] == "//")
                    {
                        lineFromFile = reader.ReadLine();
                    }

                    // Sets the enemy type based off the line from the file
                    if (lineFromFile == "Imp")
                    {
                        enemyType = EnemyType.Imp;
                    }
                    else if (lineFromFile == "Cloak")
                    {
                        enemyType = EnemyType.Cloak;
                    }
                    else if (lineFromFile == "MouthDemon")
                    {
                        enemyType = EnemyType.MouthDemon;
                    }
                    else if (lineFromFile == "HornDemon")
                    {
                        enemyType = EnemyType.HornDemon;
                    }

                    // Queues a new enemy
                    allEnemies.Enqueue(new Enemy(enemyType,
                                                 rng,
                                                 asset,
                                                 player,
                                                 screenWidth,
                                                 screenHeight,
                                                 worldWidth,
                                                 worldHeight));
                }
            }
            catch (Exception e)
            {
                //Catch the potential exception and print error message
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                //Close the reader
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Spawns a new wave of enemies into the level.
        /// </summary>
        /// <param name="amount">The number of enemies in the wave.</param>
        private void NewWave(int amount)
        {
            // As long as there are enemies left to be spawned in the level, they
            // are moved to the new wave until the given amount have been added
            while (amount > 0 && allEnemies.Count > 0)
            {
                // Moves the next enemy for the level into the list of current enemies
                currentEnemies.Add(allEnemies.Dequeue());

                // Decrements the number of enemies that still need to be spawned
                amount--;
            }
        }
    }
}
