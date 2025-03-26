using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    public class Bullet : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        private MouseState mouse;
        private int speed;
        private double rotation;
        private Vector2 direction;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------


        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Bullet(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
        :base(asset, position, screenWidth, screenHeight)
        {
            this.asset = asset;
            this.position = position;
            speed = 50;
        }


        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            return false;
        }

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
            mouse = Mouse.GetState();

            rotation = Math.Atan2(mouse.Y, mouse.X);
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

            position.X += (int)direction.X * speed;
            position.Y += (int)direction.Y * speed;
            // Use this youtube video to help with shooting bullets
            //https://www.youtube.com/watch?v=yESHtmwYgDY&t=513s
        }

        public override void Draw(SpriteBatch sb)
        {
            
        }
    }
}
