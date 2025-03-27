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
    enum PlayerState
    {
        WalkRight,
        WalkLeft,
        FaceRight,
        FaceLeft,
        Dead,
        Hit
    }

    internal class Player : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        private PlayerState playerState;
        private KeyboardState kbState;
        private int health;
        private double stunTimer;
        private int playerHealth;
        private bool isAlive;
        
        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        public PlayerState PlayerState
        {
            get { return playerState; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Player(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
        : base(asset, position, screenWidth, screenHeight)
        {
            this.asset = asset;
            this.position = position;
            health = 6;
            stunTimer = 1;
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

        public override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            //Player FSM (incomplete)
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

        public override void Draw(SpriteBatch sb)
        {
            //Draws the player with no movement
            sb.Draw(asset,
                position,
                new Rectangle(7, 7, 10, 18),
                Color.White);


        }

        /// <summary>
        /// Player is always on screen, this method is mostly for Bullet & Enemy
        /// </summary>
        /// <param name="worldToScreen">Worldspace to screenspace offset vector.</param>
        /// <returns></returns>
        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            return true;
        }

    }
}
