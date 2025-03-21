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
        protected Rectangle position;
        protected bool onScreen;


        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }
        public bool OnScreen
        {
            get { return onScreen; }
            set { onScreen = value; }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public GameObject(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.position = position;

            // onScreen defaults to true, but will be accurately set in Update()
            onScreen = true; 
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
        /// <param name="gameTime"> Info about time from MonoGame </param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Requires child classes to have their own draw method
        /// </summary>
        /// <param name="sb"> The sprite batch needed to draw </param>
        public abstract void Draw(SpriteBatch sb);
    }
}
