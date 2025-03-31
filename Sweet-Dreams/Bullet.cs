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
    /* Nick Sailor, Ayvin Krug
     * Purpose: */
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

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Bullet(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
        :base(asset, position, screenWidth, screenHeight)
        {
            this.asset = asset;
            this.position = position;
            speed = 10;
            mouse = Mouse.GetState();
            origin = new Vector2(0, 0);

            // Finding the rotation of the bullet based off the mouse
            rotation = (float)Math.Atan2(position.Y - mouse.Y, position.X - mouse.X);

            // Finding the direction the bullet need to go based of the rotation
            direction = new Vector2((float)Math.Cos(rotation + 3.14), (float)Math.Sin(rotation + 3.14));
        }


        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        /// <summary>
        /// Determines whether an object is at all visible on screen.
        /// </summary>
        /// <param name="worldToScreen"> World to screen offset vector. </param>
        /// <returns> True if any part of the object is on screen. </returns>
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
            mouse = Mouse.GetState();



            // Changing the bullets position
            position.X += (int)Math.Round(direction.X * speed);
            position.Y += (int)Math.Round(direction.Y * speed);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                new Rectangle(position.X + 15, position.Y + 10, 16, 16),
                new Rectangle(0, 0, 16, 16),
                Color.White,
                (rotation - 0.78f),
                origin,
                SpriteEffects.None,
                1);
        }
    }
}
