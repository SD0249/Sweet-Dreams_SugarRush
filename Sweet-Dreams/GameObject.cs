using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies
namespace Sweet_Dreams
{
    /* Nickolas Sailer
     * Purpose: A GameObject class will be the parent class for Bullet,
     * Player, and Enemies classes, since they all have similar fields and methods. */
    public abstract class GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        protected Texture2D asset;
        protected Rectangle worldPosition;
        protected Rectangle screenPosition;
        protected int screenWidth;
        protected int screenHeight;
        protected int currentFrame;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public GameObject(Texture2D asset, Rectangle worldPosition,
            Rectangle screenPosition, int screenWidth, int screenHeight)
        {
            this.asset = asset;
            this.worldPosition = worldPosition;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.screenPosition = screenPosition;
        }

        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------
        
        /// <summary>
        /// Requires child classes to update their animation
        /// </summary>
        /// <param name="gameTime"> Info about time from MonoGame </param>
        public abstract void UpdateAnimation(GameTime gameTime);

        /// <summary>
        /// Requires child classes to have their own update method
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame.</param>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        public abstract void Update(GameTime gameTime, Vector2 worldToScreen);

        /// <summary>
        /// Requires child classes to have their own draw method
        /// </summary>
        /// <param name="sb"> The sprite batch needed to draw </param>
        public abstract void Draw(SpriteBatch sb);

        
    }
}
