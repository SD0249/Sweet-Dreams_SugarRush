using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!

// -------------------------------
// NOTE: all the damage variables are at 1 as placeholder values
// -------------------------------
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

        // generates a number that determines the type of enemy
        Random rng;

        // CONSTRUCTORS
        public Enemy()
        {
            rng = new Random();

            isAlive = true;

            // this value will be determine the type of Enemy
            int rngNum = rng.Next(0, 4);
            switch (rngNum)
            {
                case 0:
                    eType = EnemyType.Imp;
                    damage = 1;
                    break;
                case 1:
                    eType = EnemyType.MouthDemon;
                    damage = 1;
                    break;
                case 2:
                    eType = EnemyType.HornDemon;
                    damage = 1;
                    break;
                case 3:
                    eType = EnemyType.Cloak;
                    damage =
                    break;
            }
        }
        public Enemy(EnemyType eType)
        {
            switch (eType)
            {
                case EnemyType.Imp:
                    damage =
                    break;
                case EnemyType.MouthDemon:
                    damage =
                    break;
                case EnemyType.HornDemon:
                    damage =
                    break;
                case EnemyType.Cloak:
                    damage =
                    break;
            }
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
