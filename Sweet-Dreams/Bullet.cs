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
        public Rectangle WorldPosition
        {
            get { return WorldPosition; }
            set { WorldPosition = value; }
        }

        /// <summary>
        /// Whether or not any part of the object is visible on the screen.
        /// </summary>
        public bool IsOnScreen
        {
            get
            {
                return !(screenPosition.X - screenPosition.Width/2 > screenPosition.X
                    || screenPosition.X < screenPosition.Height
                    || screenPosition.Y - screenPosition.Height/2 > screenPosition.Y
                    || screenPosition.Y < screenPosition.Height);
            }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Bullet(Texture2D asset, Rectangle worldPosition,  Rectangle screenPosition, int screenWidth, int screenHeight)
        :base(asset, worldPosition, screenPosition, screenWidth, screenHeight)
        {
            this.asset = asset;
            speed = 10;
            mouse = Mouse.GetState();
            origin = new Vector2(0, 0);

            // Finding the rotation of the bullet based off the mouse
            rotation = (float)Math.Atan2(worldPosition.Y - mouse.Y, worldPosition.X - mouse.X);

            // Finding the direction the bullet need to go based of the rotation
            direction = new Vector2((float)Math.Cos(rotation + 3.14), (float)Math.Sin(rotation + 3.14));
        }


        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        /// <summary>
        /// Bullets won't have an animation for this project
        /// The only animation they would have is exploding (not doing that)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            mouse = Mouse.GetState();

            // Changing the bullets position
            worldPosition.X += (int)Math.Round(direction.X * speed);
            worldPosition.Y += (int)Math.Round(direction.Y * speed);

            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X + (int)worldToScreen.X,
                worldPosition.Y + (int)worldToScreen.Y,
                worldPosition.Width,
                worldPosition.Height);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                new Rectangle(screenPosition.X + 15, screenPosition.Y + 10, 16, 16),
                new Rectangle(0, 0, 16, 16),
                Color.White,
                rotation - 0.78f,
                origin,
                SpriteEffects.None,
                1);

            DebugLib.DrawRectOutline(sb, screenPosition, 3, Color.Black);
        }
    }
}
