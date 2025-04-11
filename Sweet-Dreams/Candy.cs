using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
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
        CandyCorn,
        GreenCandy,
        YellowCandy,
        Chocolate
    }

    public class Candy : GameObject
    {
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        // The type of candy that will appear on screen
        CandyType cType;

        // Generates a number that determines the type of candy dropped
        Random rng;

        // Source Rectangle
        Rectangle sourceRect;

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
                return !(screenPosition.X + screenPosition.Width < 0
                    || screenPosition.X > screenWidth
                    || screenPosition.Y + screenPosition.Height < 0
                    || screenPosition.Y > screenHeight);
            }
        }

        /// <summary>
        /// This object's position in the world.
        /// </summary>
        public override Rectangle WorldPosition
        {
            get { return worldPosition; }
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
            int rngNum = rng.Next(0, 6);
            switch (rngNum)
            {
                case 0:
                    cType = CandyType.SkullCandy;
                    sourceRect = new Rectangle(65, 97, 16, 16);

					break;
                case 1:
                    cType = CandyType.Peppermint;
                    sourceRect = new Rectangle(0, 16, 16, 16);

					break;
                case 2:
                    cType = CandyType.CandyCorn;
                    sourceRect = new Rectangle(0, 0, 16, 16);

					break;
                case 3:
                    cType = CandyType.GreenCandy;
                    sourceRect = new Rectangle(17, 33, 16, 16);

					break;
                case 4:
                    cType = CandyType.YellowCandy;
                    sourceRect = new Rectangle(33, 81, 16, 16);

					break;
                case 5:
                    cType = CandyType.Chocolate;
                    sourceRect = new Rectangle(0, 129, 16, 16);

					break;
            }
        }

        // --------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------
        public override void UpdateAnimation(GameTime gameTime)
        {
           
        }

        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            // Updates world position by moving toward the player

            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X + (int)worldToScreen.X,
                worldPosition.Y + (int)worldToScreen.Y,
                worldPosition.Width,
                worldPosition.Height);
        }

        /// <summary>
        /// Draws the candy based on its candy type
        /// </summary>
        /// <param name="sb"> SpriteBatch to draw with </param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                asset,
                worldPosition,
                sourceRect,
                Color.White);
        }
    }
}
