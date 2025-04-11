using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!

// -------------------------------
// NOTE: all the damage and candyNum variables are at 1 as placeholder values
// -------------------------------
namespace Sweet_Dreams
{
    public enum EnemyType
    {
        Imp,
        MouthDemon,
        HornDemon,
        Cloak
    }

    public class Enemy : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        // the type of Enemy
        private EnemyType eType;

        // the hit points of the Enemy
        private int health;

        // how many candies the enemy will drop when it dies
        private int candyNum;

        // the amount of damage that the Enemy can deal to the player
        private int damage;

        // the enemy's speed and direction
        private Vector2 velocity;

        // Reference to the game's randomizer
        private Random rng;

        // Bounds of the world map
        private int worldWidth;
        private int worldHeight;

        //Source Rectangle for animations
        Rectangle sourceRect;

        // values needed for the enemy's animation
        private double timer;
        private double fps;
        private double spf;

        // Values needed for enemy movement
        private float rotation;
        private Vector2 direction;
        private Vector2 playerLocation;
        private int speed;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        /// <summary>
        /// Whether or not any part of the object is visible on the screen.
        /// </summary>
        public bool IsOnScreen
        {
            get
            {
                return !(screenPosition.X + screenPosition.Width < 0
                    || screenPosition.X > screenWidth
                    || screenPosition.Y + screenPosition.Height < 0
                    || screenPosition.Y > screenHeight);
            }
        }

        /// <summary>
        /// This object's position in the world.
        /// </summary>
        public override Rectangle WorldPosition
        {
            get { return worldPosition; }
        }

        /// <summary>
        /// Whether or not this enemy has any health remaining.
        /// </summary>
        public bool IsAlive
        {
            get { return health > 0; }
        }

        // --------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------
        /// <summary>
        /// Generates a randomly positioned enemy of a given type.
        /// </summary>
        /// <param name="eType">The type of enemy.</param>
        /// <param name="rng">Reference to the game's randomizer.</param>
        /// <param name="asset">Spritesheet of enemies.</param>
        /// <param name="anyRect">Any rect can be passed in here. 
        /// Position will be randomized in the constructor.</param>
        /// <param name="screenWidth">Screen's width.</param>
        /// <param name="screenHeight">Screen's height.</param>
        public Enemy(EnemyType eType, Random rng, Texture2D asset,
            int screenWidth, int screenHeight, int worldWidth, int worldHeight)
            :base(asset, new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1), 
                 screenWidth, screenHeight)
        {
            this.rng = rng;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;

            // Determines type-specific field values for this enemy
            this.eType = eType;
            CreateEnemy();

            // Randomizes enemy's position to somewhere on the border
            int edge = rng.Next(4);
            switch (edge)
            {
                // Left edge, random height
                case 0:
                    worldPosition.X = -worldPosition.Width;
                    worldPosition.Y = rng.Next(-worldPosition.Height, worldHeight);
                    break;

                // Right edge, random height
                case 1:
                    worldPosition.X = worldWidth;
                    worldPosition.Y = rng.Next(-worldPosition.Height, worldHeight);
                    break;

                // Top edge, random X value
                case 2:
                    worldPosition.X = rng.Next(-worldPosition.Width, worldWidth);
                    worldPosition.Y = -worldPosition.Height;
                    break;

                // Bottom edge, random X value
                case 3:
                    worldPosition.X = rng.Next(-worldPosition.Width, worldWidth);
                    worldPosition.Y = worldHeight;
                    break;
            }
            
            // Gives screen position a default value until it is updated in Update()
            screenPosition = worldPosition;

            // TODO: Change these values
            health = 0;
            speed = 5;
            timer = 0.0;
            spf = 0.0;
            fps = 0.0;

        }

        public Enemy(EnemyType eType, Texture2D asset,
           int screenWidth, int screenHeight, int worldWidth, int worldHeight)
           : base(asset, new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1),
                screenWidth, screenHeight)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;

            // Determines type-specific field values for this enemy
            this.eType = eType;
            CreateEnemy();

            // Randomizes enemy's position to somewhere on the border
            worldPosition.X = 500;
            worldPosition.Y = 500;

            // Gives screen position a default value until it is updated in Update()
            screenPosition = worldPosition;

            // TODO: Change these values
            health = 5;
            speed = 5;
            timer = 0.0;
            spf = 0.0;
            fps = 0.0;

        }

        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------
        /// <summary>
        /// 
        // THIS IS TEMPORARY RIGHT NOW MAKE SURE TO CHANGE IF NEEDED
        ///
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            // ElapsedGameTime is the duration of the last GAME frame
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time passed to flip to the next frame?
            if (timer >= spf)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                currentFrame++;
                if (currentFrame >= 3)
                {
                    currentFrame = 0;
                }

                // Reset the time counter, keeping remaining elapsed time
                timer -= spf;
            }
        }

        /// <summary>
        /// Updates the enemies position to always 
        /// move towards the player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            playerLocation = new Vector2(385 + worldToScreen.X, 213 + worldToScreen.Y);
            
            // Updates world position by moving toward the player
            rotation = (float)Math.Atan2(screenPosition.Y - playerLocation.Y, screenPosition.X - playerLocation.X);
            direction = new Vector2((float)Math.Cos(rotation + 3.14), (float)Math.Sin(rotation + 3.14));

            worldPosition.X += (int)Math.Round(direction.X * speed);
            worldPosition.Y += (int)Math.Round(direction.Y * speed);

            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X + (int)worldToScreen.X,
                worldPosition.Y + (int)worldToScreen.Y,
                worldPosition.Width, 
                worldPosition.Height);
        }

        /// <summary>
        /// Draws the enemy based on its enemy type
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            // Draws the enemy
            sb.Draw(
                asset,
                worldPosition,
                sourceRect,
                Color.White);
        }

        /// <summary>
        /// If this object is colliding with another given object.
        /// </summary>
        /// <param name="gameObject">The object to check collisions with.</param>
        /// <returns>Whether or not the objects' position rectangles intersect.</returns>
        public bool CollidesWith(GameObject gameObject)
        {
            return worldPosition.Intersects(gameObject.WorldPosition);
        }

        /// <summary>
        /// When the enemy dies, Candy will be drawn near the enemy position
        /// </summary>
        /// <param name="collectibles"> List of dropped candies </param>
        public void DropCandy(List<Candy> collectibles, Texture2D candyAsset)
        {
            // Add all the dropped candies to the collectibles list
            for (int i = 0; i < candyNum; i++)
            {
                // Position is randomized close to the enemy
                collectibles.Add(new Candy(
                    candyAsset, 
                    new Rectangle(
                        worldPosition.X + rng.Next(-32, 41),
                        worldPosition.Y + rng.Next(-32, 41),
                        16,
                        16),
                    screenWidth, 
                    screenHeight));
            }
        }

        /// <summary>
        /// A helper method used when loading the enemies in order to
        /// set their damage, source rectangle, and how many candies they drop.
        /// </summary>
        private void CreateEnemy()
        {
            //Initializes the Enemy fields based on the Enemy type
            switch (eType)
            {
                case EnemyType.Imp:
                    health = 1;
                    damage = 1;
                    candyNum = 1;
                    sourceRect = new Rectangle(5, 5, 9, 13);

                    break;
                case EnemyType.MouthDemon:
                    health = 1;
                    damage = 1;
                    candyNum = 3;
                    sourceRect = new Rectangle(5, 51, 20, 35);

                    break;
                case EnemyType.HornDemon:
                    health = 1;
                    damage = 1;
                    candyNum = 2;
                    sourceRect = new Rectangle(4, 28, 11, 23);

                    break;
                case EnemyType.Cloak:
                    health = 1;
                    damage = 1;
                    candyNum = 2;
                    sourceRect = new Rectangle(2, 13, 12, 15);

                    break;
            }

            // Changes the enemy worldPosition based on the enemies sourceRect
            worldPosition.Width = sourceRect.Width;
            worldPosition.Height = sourceRect.Height;
        }
    }
}
