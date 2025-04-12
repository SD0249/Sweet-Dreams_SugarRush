using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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
        private bool hitEnemy;
        private int damage;

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
        /// Whether or not this bullet has hit an enemy.
        /// </summary>
        public bool HitEnemy
        {
            get { return hitEnemy; }
            set { hitEnemy = value; }
        }

        /// <summary>
        /// How much damage this bullet deals to an enemy.
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Bullet(Texture2D asset, Rectangle worldPosition,  Rectangle screenPosition, 
            int damage, int screenWidth, int screenHeight)
        :base(asset, worldPosition, screenPosition, screenWidth, screenHeight)
        {
            this.asset = asset;
            this.damage = damage;
            hitEnemy = false;
            speed = 3;
            mouse = Mouse.GetState();
            origin = new Vector2(0, 0);

            // Finding the rotation of the bullet based off the mouse
            rotation = (float)Math.Atan2(screenPosition.Y - mouse.Y, screenPosition.X - mouse.X);

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
        /// <param name="gameTime">Info from Monogame about the time state.</param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Updates the bullet's world & screen position
        /// </summary>
        /// <param name="gameTime">Info from Monogame about the time state.</param>
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
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

        /// <summary>
        /// Draws the bullet :]
        /// </summary>
        /// <param name="sb"> SpriteBatch to draw with </param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                new Rectangle(worldPosition.X + 15, worldPosition.Y + 10, 16, 16),
                new Rectangle(0, 0, 16, 16),
                Color.White,
                rotation - 0.78f,
                origin,
                SpriteEffects.None,
                1);

            //DebugLib.DrawRectOutline(sb, screenPosition, 3, Color.Black);
        }


        /// <summary>
        /// Instead of having this as a property, get information from the camera 
        /// to get the accurate bounds to determine whether the enemy is on screen.
        /// </summary>
        /// <param name="camera">The current camera created and used in Game1</param>
        /// <returns>Whether this enemy is on screen; if it is seen by the camera.</returns>
        public bool IsOnScreen(OrthographicCamera camera)
        {
            return camera.CameraBound.Contains(this.worldPosition);
        }

        /// <summary>
        /// Determines whether or not the bullet is past the world's bounds.
        /// </summary>
        /// <param name="worldWidth">Width of the world map.</param>
        /// <param name="worldHeight">Height of the world map.</param>
        /// <returns>Whether or not the bullet has traveled off the world map.</returns>
        public bool OutOfBounds(int worldWidth, int worldHeight)
        {
            return worldPosition.Y + worldPosition.Height < 0       // Too far up
                || worldPosition.Y > worldHeight                    // Too far down
                || worldPosition.X + worldPosition.Width < 0        // Too far left
                || worldPosition.X > worldWidth;                    // Too far right
        }
    }
}
