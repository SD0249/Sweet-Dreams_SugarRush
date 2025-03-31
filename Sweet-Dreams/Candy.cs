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
        // FIELDS
        // the type of candy that will appear on screen
        CandyType cType;

        // generates a number that determines the type of candy dropped
        Random rng;

        // source Rectangle
        Rectangle sourceRectangle;

        // CONSTRUCTORS
        public Candy(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
            : base(asset, position, screenWidth, screenHeight)
        {
            
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

        // METHODS
        /// <summary>
        /// Determines whether an object is at all visible on screen.
        /// </summary>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        /// <returns>True if any part of the object is on screen.</returns>
        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            // Returns false if any of the following out of bounds conditions are true
            return !(position.X + position.Width < worldToScreen.X      // Too far left
                || position.X > screenWidth - worldToScreen.X           // Too far right
                || position.Y + position.Height < worldToScreen.Y       // Too far up
                || position.Y > screenHeight - worldToScreen.Y);        // Too far down
        }
        public override void UpdateAnimation(GameTime gameTime)
        {
           
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch sb)
        {

        }
    }
}
