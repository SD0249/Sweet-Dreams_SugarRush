using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sweet_Dreams
{
    abstract class GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        protected Texture2D asset;
        protected Rectangle position;


        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        public Rectangle Position
        {
            get { return position; }
        }


        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public GameObject(Texture2D asset, Rectangle positon)
        {
            this.asset = asset;
            this.position = positon;
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
