using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
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

        // Source Rectangle for animations
        Rectangle sourceRect;

        // values needed for the enemy's animation
        private double timer;
        private double fps;
        private double spf;

        // Unit direction vector
        private Vector2 direction;

        // Speed scalar 
        private int speed;

        // Reference to the player
        Player player;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        /// <summary>
        /// Whether or not any part of the object is visible on the screen.
        /// </summary>
        /* public bool IsOnScreen
        {
            get
            {
                return !(screenPosition.X + screenPosition.Width < 0
                    || screenPosition.X > screenWidth
                    || screenPosition.Y + screenPosition.Height < 0
                    || screenPosition.Y > screenHeight);
            }
        } */

        /// <summary>
        /// This object's position in the world.
        /// </summary>
        public override Rectangle WorldPosition
        {
            get { return worldPosition; }
        }

        /// <summary>
        /// The amount of damage this enemy does to the player.
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }

        /// <summary>
        /// Whether or not this enemy has any health remaining.
        /// </summary>
        public bool IsAlive
        {
            get { return health > 0; }
        }

        /// <summary>
        /// How much health the player has left.
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }

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
        /// Position will be randomized in the constructor.</param>
        /// <param name="screenWidth">Screen's width.</param>
        /// <param name="screenHeight">Screen's height.</param>
        public Enemy(EnemyType eType, Random rng, Texture2D asset, Player player,
            int screenWidth, int screenHeight, int worldWidth, int worldHeight)
            :base(asset, new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1), 
                 screenWidth, screenHeight)
        {
            this.rng = rng;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.player = player;

            // Determines type-specific field values for this enemy
            this.eType = eType;
            CreateEnemy();

            // Positions the enemy on the world's border
            GoToWorldEdge();
            
            // Gives screen position a default value until it is updated in Update()
            screenPosition = worldPosition;

            // TODO: Change these values
            timer = 0.0;
            spf = 0.0;
            fps = 0.0;
        }

        /*public Enemy(EnemyType eType, Texture2D asset,
            int screenWidth, int screenHeight, int worldWidth, int worldHeight)
            :base(asset, new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1),
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
        
        } */

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
        /// <param name="gameTime">Info from MonoGame about the time state.</param>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            // Updates world position by moving toward the player

            // rotation = (float)Math.Atan2(worldPosition.Y - player.WorldPosition.Y,
            //     worldPosition.X - player.WorldPosition.X);
            // direction = new Vector2((float)Math.Cos(rotation + 3.14), (float)Math.Sin(rotation + 3.14));

            // Creates a direction vector pointing toward the player
            direction = new Vector2(player.WorldPosition.X - worldPosition.X,
                player.WorldPosition.Y - worldPosition.Y);

            // As long as the direction is not the zero vector, it is normalized so 
            // the enemy will have the same displacement every movement frame
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            // Updates world position by moving toward the player
            worldPosition.X += (int)(direction.X * speed);
            worldPosition.Y += (int)(direction.Y * speed);

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
                        24,
                        24),
                    screenWidth, 
                    screenHeight));
            }
        }

        /// <summary>
        /// Instead of having this as a property, get information from the camera 
        /// to get the accurate bounds to determine whether the enemy is on screen.
        /// </summary>
        /// <param name="camera">The current camera created and used in Game1</param>
        /// <returns>Whether this enemy is on screen; if it is seen by the camera.</returns>
        public bool IsOnScreen(OrthographicCamera camera)
        {
            //return camera.CameraBound.Contains(this.worldPosition);

            return camera.CameraBound.Intersects(this.worldPosition);
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
                    speed = 4;
                    sourceRect = new Rectangle(4, 4, 10, 12);

                    break;
                case EnemyType.MouthDemon:
                    health = 1;
                    damage = 1;
                    candyNum = 3;
                    speed = 2;
                    sourceRect = new Rectangle(5, 107, 23, 30);

                    break;
                case EnemyType.HornDemon:
                    health = 1;
                    damage = 1;
                    candyNum = 2;
                    speed = 3;
                    sourceRect = new Rectangle(3, 60, 11, 18);

                    break;
                case EnemyType.Cloak:
                    health = 1;
                    damage = 1;
                    speed = 3;
                    candyNum = 2;
                    sourceRect = new Rectangle(1, 40, 13, 15);

                    break;
            }

            // Changes the enemy worldPosition based on the enemy's sourceRect
            worldPosition = new Rectangle(0, 0, sourceRect.Width * 4, sourceRect.Height * 4);
        }

        /// <summary>
        /// Randomizes the enemy's position to somewhere on the border
        /// </summary>
        public void GoToWorldEdge()
        {
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
        }
    }
}
