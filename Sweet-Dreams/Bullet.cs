using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
        private float rotation;
        private Vector2 direction;
        private Vector2 origin;
        private double reloadTimer;     // This should probably be in Player or Game1

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        public int Timer
        {
            // Only need a set to change the timer when a power-up is picked up
            set { reloadTimer = value; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Bullet(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
        :base(asset, position, screenWidth, screenHeight)
        {
            this.asset = asset;
            this.position = position;
            speed = 50;

            // The origin of the bullets will always be the starting position
            origin = new Vector2(position.X, position.Y);

            // Since bullets won't have animation, timer is used for reloading
            reloadTimer = 1;
        }


        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        /// <summary>
        /// Determines whether an object is at all visible on screen.
        /// </summary>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        /// <returns>True if any part of the object is on screen.</returns>
        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            // Returns false if any of the following out of bounds conditions are true
            return !(position.X + position.Width < worldToScreen.X      // Too far left
                || position.X > screenWidth + worldToScreen.X           // Too far right
                || position.Y + position.Height < worldToScreen.Y       // Too far up
                || position.Y > screenHeight + worldToScreen.Y);        // Too far down
        }

        /// <summary>
        /// Bullets won't have an animation for this project
        /// The only animation they would have is exploding (not doing that)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Put this stuff in Player or Game1 instead
            
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed &&
                reloadTimer <= 0)
            {
                // Add a new bullet to the public list of bullets

                // Resets the timer for reloading the gun 
                reloadTimer = 1;
            }
            // Has a 1 second timer between shooting a bullet
            reloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;

            // Finding the rotation of the bullet based off the mouse
            rotation = (float)Math.Atan2(mouse.Y, mouse.X);

            // Finding the direction the bullet need to go based of the rotation
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

            // Changing the bullets position
            position.X += (int)direction.X * speed;
            position.Y += (int)direction.Y * speed;

            // Use this youtube video to help with shooting bullets
            //https://www.youtube.com/watch?v=yESHtmwYgDY&t=513s
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                position,
                null,
                Color.White,
                rotation,
                origin,
                SpriteEffects.None,
                0);
        }
    }
}
