using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 // Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    enum CandyType
    {
        SkullCandy,
    }
    internal class Candy
    {
        // FIELDS
        // the type of candy that will appear on screen
        CandyType cType;

        // this will correlate with the position of the enemy
        Rectangle position;

        // generates a number that determines the type of candy dropped
        Random rng;


        // CONSTRUCTORS
        public Candy(Rectangle position)
        {
            rng = new Random();

            // this value will be determined by the Enemy position
            this.position = position;

            // this value will be determined randomly
            int rngNum = rng.Next(0, 5);
        }
    }
}
