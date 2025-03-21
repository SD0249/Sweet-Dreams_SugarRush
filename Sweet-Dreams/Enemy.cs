using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!

// -------------------------------
// NOTE: all the damage and candyNum variables are at 1 as placeholder values
// -------------------------------
// -------------------------------
// ANOTHER NOTE: A List<Candy> field should be added, and candyNum should be replaced 
//               with the count. Also change switch(EnemyType) to be a helper method.
// -------------------------------
namespace Sweet_Dreams
{
    public enum EnemyType
    {
        Imp,
        MouthDemon,
        HornDemon,
        Cloak
    }
    public class Enemy : GameObject
    {
        // FIELDS

        // the type of Enemy
        EnemyType eType;

        // the status of the Enemy
        bool isAlive;

        // the number of candies the Enemy will drop
        int candyNum;

        // the amount of damage that the Enemy can deal to the player
        int damage;

        // reference to the game's random generator, used to determine the type of enemy
        Random rng;

        // CONSTRUCTORS
        /// <summary>
        /// Randomly generates an Enemy
        /// </summary>
        public Enemy(Random rng, Texture2D asset, Rectangle position)
            :base(asset, position)
        {
            isAlive = true;
            this.rng = rng;


            // This value will determine the type of Enemy
            int randomNum = rng.Next(0, 4);
            switch (randomNum)
            {
                case 0:
                    eType = EnemyType.Imp;
                    damage = 1;
                    candyNum = 1;
                    break;
                case 1:
                    eType = EnemyType.MouthDemon;
                    damage = 1;
                    candyNum = 1;
                    break;
                case 2:
                    eType = EnemyType.HornDemon;
                    damage = 1;
                    candyNum = 1;
                    break;
                case 3:
                    eType = EnemyType.Cloak;
                    damage = 1;
                    candyNum = 1;
                    break;
            }
        }
        /// <summary>
        /// Generates chosen enemies
        /// </summary>
        /// <param name="eType">the type of enemy chosen</param>
        public Enemy(Random rng, EnemyType eType, Texture2D asset, Rectangle position)
            : base(asset, position)
        {
            isAlive = true;
            this.rng = rng;

            switch (eType)
            {
                case EnemyType.Imp:
                    damage = 1;
                    candyNum = 1;
                    break;
                case EnemyType.MouthDemon:
                    damage = 1;
                    candyNum = 1;
                    break;
                case EnemyType.HornDemon:
                    damage = 1;
                    candyNum = 1;
                    break;
                case EnemyType.Cloak:
                    damage = 1;
                    candyNum = 1;
                    break;
            }
        }

        //METHODS

        public override void UpdateAnimation(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {

        }

        public bool CollidesWith()
        {
            return true;
        }

        /// <summary>
        /// When the enemy dies, a Candy will be drawn in at the enemy position
        /// </summary>
        public void DropCandy()
        {
            if (!isAlive)
            {

            }
        }
    }
}
