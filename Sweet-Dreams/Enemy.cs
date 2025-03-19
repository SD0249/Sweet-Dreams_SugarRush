using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    enum EnemyType
    {
        
    }
    internal class Enemy
    {
        // FIELDS
        EnemyType eType;
        bool isAlive;
        int points;
        int damage;

        // CONSTRUCTORS
        public Enemy()
        {
            
        }

        //METHODS
        bool CollidesWith()
        {
            return true;
        }
    }
}
