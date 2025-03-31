using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
            mouse = Mouse.GetState();


            // The origin of the bullets will always be the starting position
            origin = new Vector2(position.X, position.Y);

            

        }


        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            return true;
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
            
            // Finding the rotation of the bullet based off the mouse
            rotation = (float)Math.Atan2(mouse.Y, mouse.X);

            // Finding the direction the bullet need to go based of the rotation
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));


            // Changing the bullets position
            position.X += (int)direction.X * speed;
            position.Y += (int)direction.Y * speed;

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                new Rectangle(position.X, position.Y, 64, 64),
                new Rectangle(0, 0, 16, 16),
                Color.White,
                rotation,
                origin,
                SpriteEffects.None,
                0);

        }
    }
}
