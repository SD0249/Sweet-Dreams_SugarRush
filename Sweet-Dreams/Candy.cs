using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        GreenCandy
    }
    public class Candy : GameObject
    {
        // FIELDS
        // the type of candy that will appear on screen
        CandyType cType;

        // generates a number that determines the type of candy dropped
        Random rng;


        // CONSTRUCTORS
        public Candy(Texture2D asset, Rectangle position, int screenWidth, int screenHeight)
            : base(asset, position, screenWidth, screenHeight)
        {
            rng = new Random();

            // this value will be determine the type of Candy
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
                    cType = CandyType.PinkCandy;
                    break;
                case 3:
                    cType = CandyType.GreenCandy;
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

        }
    }
}
