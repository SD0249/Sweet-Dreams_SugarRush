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
        private double stunTimer;
        private int playerHealth;
        private bool isAlive;
        private Vector2 velocity;
        private int damage;

        // Animation fields
        private double timer;
        private double fps;
        private double spf;
        private double reloadTimer;

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
        /// Whether or not the object is visible. Always true for the player.
        /// </summary>
        public bool IsOnScreen
        {
            get { return true; }
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
            stunTimer = 1;
            velocity = new Vector2(0, 0);       // Placeholder velocity
            timer = 0.0;                        // Change these values
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
                if (currentFrame >= 3)
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
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            // Updates world position based on keyboard input
            KeyboardState kbState = Keyboard.GetState();

            // TODO: Remove this test movement and use velocity inside of FSM instead
            if (kbState.IsKeyDown(Keys.Right))
            {
                worldPosition.X += 5;
            }
            if (kbState.IsKeyDown(Keys.Left))
            {
                worldPosition.X -= 5;
            }
            if (kbState.IsKeyDown(Keys.Up))
            {
                worldPosition.Y -= 5;
            }
            if (kbState.IsKeyDown(Keys.Down))
            {
                worldPosition.Y += 5;
            }

            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X + (int)worldToScreen.X,
                worldPosition.Y + (int)worldToScreen.Y,
                worldPosition.Width,
                worldPosition.Height);

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
            sb.Draw(asset,
                worldPosition,
                new Rectangle(7, 7, 10, 18),
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
                    break;

                case CandyType.Peppermint:
                    // Increase bullet velocity
                    break;

                case CandyType.CandyCorn:
                    // Do something
                    break;

                case CandyType.GreenCandy:
                    // Do something
                    break;

                case CandyType.YellowCandy:
                    // Do something
                    break;
            }
        }
    }
}
