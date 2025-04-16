using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Net.Http;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    public enum PlayerState
    {
        WalkRight,
        WalkLeft,
        FaceRight,
        FaceLeft,
        Dead,
        Hit
    }

    public class Player : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        private PlayerState playerState;
        private int health;
        private double reloadTimer;
        private double stunTimer;
        private int playerHealth;
        private bool isAlive;
        private Vector2 velocity;
        private int damage;
        private int points;
        private int speed;

        // Animation fields
        private List<Rectangle> idleAnim;
        private List<Rectangle> walkingAnim;
        private List<Rectangle> damageAnim;
        private List<Rectangle> deathAnim;
        private Rectangle walkingWP;
        private double timer;
        private double fps;
        private double spf;
        private Color tint;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        /// <summary>
        /// This object's position in the world.
        /// </summary>
        public override Rectangle WorldPosition
        {
            get { return worldPosition; }
        }

        /// <summary>
        /// The player's position on the screen.
        /// </summary>
        public Rectangle ScreenPosition
        {
            get { return screenPosition; }
        }
        
        /// <summary>
        /// The current animation state of the player.
        /// </summary>
        public PlayerState PlayerState
        {
            get { return playerState; }
        }
        
        /// <summary>
        /// The player's remaining health.
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public double ReloadTimer
        {
            get { return reloadTimer; }
            set { reloadTimer = value; }
        }

        /// <summary>
        /// How much damage the player's bullets deal.
        /// </summary>
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        /// <summary>
        /// The color the player should be tinted
        /// </summary>
        public Color Tint
        {
            get { return tint; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Player(Texture2D asset, Rectangle worldPosition, Rectangle screenPosition,
            int screenWidth, int screenHeight)
            : base(asset, worldPosition, screenPosition, screenWidth, screenHeight)
        {
            health = 6;
            damage = 1;
            speed = 3;
            points = 0;
            tint = Color.White;
            stunTimer = 1;
            reloadTimer = 1;
            velocity = new Vector2(0, 0);       // Placeholder velocity
            timer = 0.0;                        // Change these values
            spf = 0.2;
            fps = 5.0;
            walkingWP = worldPosition;

            // Making animation lists
            idleAnim = new List<Rectangle>(2);
            idleAnim.Add(new Rectangle(7, 7, 10, 17));
            idleAnim.Add(new Rectangle(7, 7, 10, 17));
            idleAnim.Add(new Rectangle(31, 8, 10, 16));
            idleAnim.Add(new Rectangle(31, 8, 10, 16));

            walkingAnim = new List<Rectangle>(4);
            walkingAnim.Add(new Rectangle(7, 32, 10, 16));
            walkingAnim.Add(new Rectangle(31, 31, 10, 17));
            walkingAnim.Add(new Rectangle(55, 32, 10, 16));
            walkingAnim.Add(new Rectangle(79, 31, 10, 17));

            damageAnim = new List<Rectangle>(2);
            damageAnim.Add(new Rectangle(150, 7, 11, 17));
            damageAnim.Add(new Rectangle(150, 7, 11, 17));
            damageAnim.Add(new Rectangle(175, 7, 10, 17));
            damageAnim.Add(new Rectangle(175, 7, 10, 17));

            deathAnim = new List<Rectangle>(4);
            deathAnim.Add(new Rectangle(103, 103, 10, 17));
            deathAnim.Add(new Rectangle(128, 104, 10, 16));
            deathAnim.Add(new Rectangle(152, 108, 13, 12));
            deathAnim.Add(new Rectangle(176, 112, 15, 8));
        }

        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------
        /// <summary>
        /// 
        // THIS IS TEMPORARY RIGHT NOW MAKE SURE TO CHANGE IF NEEDED
        ///
        /// </summary>
        /// <param name="gameTime">Info from Monogame about the time state.</param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            // ElapsedGameTime is the duration of the last GAME frame
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time passed to flip to the next frame?
            if (timer >= spf)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                currentFrame++;
                if (currentFrame >= 4)
                {
                    currentFrame = 0;
                }

                // Reset the time counter, keeping remaining elapsed time
                timer -= spf;
            }
        }

        /// <summary>
        /// Updates the players position based on what keys are pressed
        /// </summary>
        /// <param name="gameTime">Info from Monogame about the time state.</param>
        public override void Update(GameTime gameTime)
        {
            // Updates world position based on keyboard input
            KeyboardState kbState = Keyboard.GetState();

            // TODO: Remove this test movement and use velocity inside of FSM instead
            if (kbState.IsKeyDown(Keys.Right))
            {
                worldPosition.X += speed;
            }
            if (kbState.IsKeyDown(Keys.Left))
            {
                worldPosition.X -= speed;
            }
            if (kbState.IsKeyDown(Keys.Up))
            {
                worldPosition.Y -= speed;
            }
            if (kbState.IsKeyDown(Keys.Down))
            {
                worldPosition.Y += speed;
            }

            // Updates screen position
            screenPosition = worldPosition;

            // Updates the walking world position (so the player's head bobs)
            walkingWP = new Rectangle(worldPosition.X,
                                  worldPosition.Y + (currentFrame + 1) % 2,
                                  worldPosition.Width,
                                  walkingAnim[currentFrame].Height * 3);

            // Player FSM (incomplete)
            switch (playerState)
            {

                case PlayerState.WalkLeft:
                    if (kbState.IsKeyUp(Keys.Left))
                    {
                        playerState = PlayerState.FaceLeft;
                    }
                    if (kbState.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;

                    }
                    if (health <= 0)
                    {
                        playerState = PlayerState.Dead;
                    }
                    break;

                case PlayerState.WalkRight:
                    if (kbState.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    if (kbState.IsKeyUp(Keys.Right))
                    {
                        playerState = PlayerState.FaceRight;
                    }
                    if (health <= 0)
                    {
                        playerState = PlayerState.Dead;
                    }
                    break;

                case PlayerState.FaceLeft:
                    if (kbState.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    if (kbState.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    if (health <= 0)
                    {
                        playerState = PlayerState.Dead;
                    }
                    break;

                case PlayerState.FaceRight:
                    if (kbState.IsKeyDown(Keys.Left))
                    {
                        playerState = PlayerState.WalkLeft;
                    }
                    if (kbState.IsKeyDown(Keys.Right))
                    {
                        playerState = PlayerState.WalkRight;
                    }
                    if (health <= 0)
                    {
                        playerState = PlayerState.Dead;
                    }
                    break;

                case PlayerState.Hit:
                    if (health <= 0)
                    {
                        playerState = PlayerState.Dead;
                    }
                    break;

                case PlayerState.Dead:
                    break;
            }
        }

        /// <summary>
        /// Draws the player to the screen
        /// </summary>
        /// <param name="sb"> SpriteBatch to draw with </param>
        public override void Draw(SpriteBatch sb)
        {
            // Draws the player
            /* sb.Draw(asset,
                worldPosition,
                new Rectangle(7, 7, 10, 18),
                tint); */

            // Player FSM (incomplete)
            switch (playerState)
            {

                case PlayerState.WalkLeft:
                    sb.Draw(asset,
                            walkingWP,
                            walkingAnim[currentFrame],
                            tint,
                            0,
                            new Vector2(0,0),
                            SpriteEffects.FlipHorizontally,
                            0);
                    break;

                case PlayerState.WalkRight:
                    sb.Draw(asset,
                            walkingWP,
                            walkingAnim[currentFrame],
                            tint);
                    break;

                case PlayerState.FaceLeft:
                    sb.Draw(asset,
                            worldPosition, // WP for idle is not correct
                            idleAnim[currentFrame],
                            tint,
                            0,
                            new Vector2(0, 0),
                            SpriteEffects.FlipHorizontally,
                            0);
                    break;

                case PlayerState.FaceRight:
                    sb.Draw(asset,
                           worldPosition,
                           idleAnim[currentFrame],
                           tint);
                    break;

                case PlayerState.Hit:
                    
                    break;

                // Change Game1 to wait 1 second before displaying the game over screen
                case PlayerState.Dead:
                    break;
            }
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
        /// In relation to the world width and height, 
        /// this method ensures that the player doesn't move out the bounds of the drawn map.
        /// </summary>
        /// <param name="worldWidth">World Width; Num of columns * Size of a tile</param>
        /// <param name="worldHeight">World Height; Num of rows * Size of a tile</param>
        public void KeepPlayerInBounds(int worldWidth, int worldHeight)
        {
            // If the player is too FAR LEFT
            if (worldPosition.X < 0)
            {
                worldPosition.X = 0;
            }

            // If the player is too FAR RIGHT
            if (worldPosition.X + worldPosition.Width > worldWidth)
            {
                worldPosition.X = worldWidth - worldPosition.Width;
            }

            // If the player is too FAR UP
            if (worldPosition.Y < 0)
            {
                worldPosition.Y = 0;
            }

            // If the player is too FAR DOWN
            if (worldPosition.Y + worldPosition.Height > worldHeight)
            {
                worldPosition.Y = worldHeight - worldPosition.Height;
            }
        }

        /// <summary>
        /// Gives a buff or debuff to the player based on the type of candy it is collecting.
        /// </summary>
        /// <param name="candyType">Type of candy that is being collected.</param>
        public void CollectCandy(CandyType candyType)
        {
            switch (candyType)
            {
                case CandyType.SkullCandy:
                    // Deal damage to the player
                    playerHealth--;
                    break;

                case CandyType.Peppermint:
                    // Increase bullet velocity
                    break;

                case CandyType.CandyCorn:
                    // bullet pickups
                    
                    break;

                case CandyType.GreenCandy:
                    // Add 5 points to player points
                    points += 5;
                    break;

                case CandyType.YellowCandy:
                    // Add 10 points to player points
                    points += 10;
                    break;
                case CandyType.Chocolate:
                    // Heal player health
                    playerHealth++;
                    break;
            }
            /*
             * BUFFS AND DEBUFFS
             * 
             * BUFFS:
             * More damage
             * reload timer
             * 
             * DEBUFFS
             * slower speed
             * 
             */
        }
    }
}
