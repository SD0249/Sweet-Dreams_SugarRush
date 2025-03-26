using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!

// -------------------------------
// NOTE: all the damage and candyNum variables are at 1 as placeholder values
// -------------------------------
// -------------------------------
// ANOTHER NOTE: candyNum should be replaced with candyDrops.Count
//               Also maybe change switch(EnemyType) to be a helper method.
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
        private EnemyType eType;

        // the status of the Enemy
        private bool isAlive;

        // how many candies the enemy will drop when it dies
        private int candyNum;

        // the amount of damage that the Enemy can deal to the player
        private int damage;

        // CONSTRUCTORS
        /// <summary>
        /// Randomly generates an Enemy
        /// </summary>
        public Enemy(Random rng, Texture2D asset, Rectangle position, int screenWidth, 
            int screenHeight)
            :base(asset, position, screenWidth, screenHeight)
        {
            isAlive = true;

        }
        /// <summary>
        /// Generates chosen enemies
        /// </summary>
        /// <param name="eType">the type of enemy chosen</param>
        public Enemy(EnemyType eType, Texture2D asset, Rectangle position, int screenWidth, 
            int screenHeight) : base(asset, position, screenWidth, screenHeight)
        {
            isAlive = true;

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

        public bool CollidesWith()
        {
            return true;
        }

        /// <summary>
        /// When the enemy dies, Candy will be drawn near the enemy position
        /// </summary>
        public void DropCandy(List<Candy> collectibles)
        {
            if (!isAlive)
            {
                for (int i = 0; i < candyNum; i++)
                {
                    //// TODO: determine params for Candy(), remove isAlive check
                    //collectibles.Add(new Candy());
                }
            }
        }

        // HELPER METHOD
        private void CreateEnemy(int number)
        {
            // This value will determine the type of Enemy
            Random rng = new Random();
            eType = (EnemyType)rng.Next(0, 4);
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
    }
}
