using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

 // Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /* Brooke Maciejewski, Ayvin Krug
     * Purpose: A Level class that uses the LevelTile objects as 
     *          the basic building blocks of a level background. 
     *          Loads information from the file and constructs the level accordingly. */
    public enum CandyType
    {
        SkullCandy,
        Peppermint,
        PinkCandy,
        GreenCandy,
        YellowCandy
    }
    public class Candy : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        // the type of candy that will appear on screen
        CandyType cType;

        // generates a number that determines the type of candy dropped
        Random rng;

        // source Rectangle
        Rectangle sourceRectangle;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        /// <summary>
        /// Whether or not any part of the object is visible on the screen.
        /// </summary>
        public bool IsOnScreen
        {
            get
            {
                return screenPosition.X + screenPosition.Width < 0
                    || screenPosition.X > screenWidth
                    || screenPosition.Y + screenPosition.Height < 0
                    || screenPosition.Y > screenHeight;
            }
        }

        // --------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------
        public Candy(Texture2D asset, Rectangle worldPosition, int screenWidth, int screenHeight)
            : base(asset, worldPosition, worldPosition, screenWidth, screenHeight)
        {
            // Only used here in the constructor
            // TODO: Maybe use a reference one generator instead of a new one for each object?
            rng = new Random();

            // this value will be determine the type of Candy
            // SOURCE RECTANGLE
            int rngNum = rng.Next(5);
            switch (rngNum)
            {
                case 0:
                    cType = CandyType.SkullCandy;
                    break;
                case 1:
                    cType = CandyType.Peppermint;
                    break;
                case 2:
                    cType = CandyType.PinkCandy;
                    break;
                case 3:
                    cType = CandyType.GreenCandy;
                    break;
                case 4:
                    cType = CandyType.YellowCandy;
                    break;
            }
        }

        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------

        public override void UpdateAnimation(GameTime gameTime)
        {
           
        }
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            // Updates world position by moving toward the player

            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X - (int)worldToScreen.X,
                worldPosition.Y - (int)worldToScreen.Y,
                worldPosition.Width,
                worldPosition.Height);
        }
        public override void Draw(SpriteBatch sb)
        {

        }
    }
}
