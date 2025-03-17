using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
