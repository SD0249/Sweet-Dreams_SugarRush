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
        Imp,
        MouthDemon,
        HornDemon,
        Cloak
    }
    internal class Enemy
    {
        // FIELDS
        EnemyType eType;
        bool isAlive;
        int points;
        int damage;

        // CONSTRUCTORS
        public Enemy(int points, int damage)
        {
            isAlive = true;

            // setting by random generation or by file?
            this.points = points;
            this.damage = damage;
        }

        //METHODS
        bool CollidesWith()
        {
            return true;
        }

        /// <summary>
        /// When the enemy dies, a Candy will be drawn in at the enemy position
        /// </summary>
        void DropCandy()
        {
            if (!isAlive)
            {

            }
        }
    }
}
