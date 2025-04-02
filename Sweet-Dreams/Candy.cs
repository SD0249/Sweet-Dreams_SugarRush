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
        CandyCorn,
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
            int rngNum = rng.Next(0, 4);
            switch (rngNum)
            {
                case 0:
                    cType = CandyType.SkullCandy;
                    break;
                case 1:
                    cType = CandyType.Peppermint;
                    break;
                case 2:
                    cType = CandyType.CandyCorn;
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
        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            return false;
        }
        public override void UpdateAnimation(GameTime gameTime)
        {
           
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch sb)
        {
            if (cType == CandyType.Peppermint)
            {
                sb.Draw(
                asset,
                position,
                new Rectangle(0, 16, 16, 16),
                Color.White);
            }
            if (cType == CandyType.CandyCorn)
            {
                sb.Draw(
                asset,
                position,
                new Rectangle(0, 0, 16, 16),
                Color.White);
            }
            if (cType == CandyType.GreenCandy)
            {
                sb.Draw(
                asset,
                position,
                new Rectangle(17, 33, 16, 16),
                Color.White);
            }
            if (cType == CandyType.YellowCandy)
            {
                sb.Draw(
                asset,
                position,
                new Rectangle(33, 81, 16, 16),
                Color.White);
            }
            if (cType == CandyType.SkullCandy)
            {
                sb.Draw(
                asset,
                position,
                new Rectangle(65, 97, 16, 16),
                Color.White);
            }
        }
    }
}
