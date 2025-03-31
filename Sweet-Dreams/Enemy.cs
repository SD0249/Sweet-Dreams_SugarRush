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
        // --------------------------------------------------------------
        // Fields
        // --------------------------------------------------------------
        // the type of Enemy
        private EnemyType eType;

        // the status of the Enemy
        private bool isAlive;

        // how many candies the enemy will drop when it dies
        private int candyNum;

        // the amount of damage that the Enemy can deal to the player
        private int damage;

        // the enemy's speed and direction
        private Vector2 velocity;

        // values needed for the enemy's animation
        private double timer;
        private double fps;
        private double spf;

        // --------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------
        

        // --------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------
        /// <summary>
        /// Randomly generates an Enemy
        /// </summary>
        public Enemy(Random rng, Texture2D asset, Rectangle position, int screenWidth, 
            int screenHeight)
            :base(asset, position, screenWidth, screenHeight)
        {
            isAlive = true;

            eType = (EnemyType)rng.Next(0, 4);
            CreateEnemy();

            // TODO: Change these values
            timer = 0.0;
            spf = 0.0;
            fps = 0.0;

        }
        /// <summary>
        /// Generates chosen enemies
        /// </summary>
        /// <param name="eType">the type of enemy chosen</param>
        public Enemy(EnemyType eType, Texture2D asset, Rectangle position, int screenWidth, 
            int screenHeight) : base(asset, position, screenWidth, screenHeight)
        {
            isAlive = true;

            this.eType = eType;
            CreateEnemy();

            // TODO: Change these values
            timer = 0.0;
            spf = 0.0;
            fps = 0.0;
        }

        // --------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------
        /// <summary>
        /// Determines whether an object is at all visible on screen.
        /// </summary>
        /// <param name="worldToScreen">World to screen offset vector.</param>
        /// <returns>True if any part of the object is on screen.</returns>
        public override bool IsOnScreen(Vector2 worldToScreen)
        {
            // Returns false if any of the following out of bounds conditions are true
            return !(position.X + position.Width < worldToScreen.X      // Too far left
                || position.X > screenWidth + worldToScreen.X           // Too far right
                || position.Y + position.Height < worldToScreen.Y       // Too far up
                || position.Y > screenHeight + worldToScreen.Y);        // Too far down
        }

        public override void UpdateAnimation(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= spf)
            {
                currentFrame++;
                if (currentFrame >= 3)
                {
                    currentFrame = 0;
                }

                timer -= spf;
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            //Draws an Enemy at a given position
            // CLOAK
            sb.Draw(
                asset,
                position,
                new Rectangle(2, 7, 12, 15), // x, y, width, height
                Color.White);

            /* IMP
             * sb.Draw(
             * asset,
             * positon
             * new Rectangle(5, 5, 9, 13),
             * Color.White)
             */

            /* MOUTH DEMON
             * sb.Draw(
             * asset,
             * positon
             * new Rectangle(5, 7, 20, 35),
             * Color.White)
             */

            /* HORN DEMON
             * sb.Draw(
             * asset,
             * positon
             * new Rectangle(4, 6, 11, 23),
             * Color.White)
             */
        }

        public bool CollidesWith()
        {
            // if the Enemy position intersects with the player position
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
        private void CreateEnemy()
        {
            switch (eType)
            {
                case EnemyType.Imp:
                    damage = 1;
                    candyNum = 1;
                    break;
                case EnemyType.MouthDemon:
                    damage = 1;
                    candyNum = 3;
                    break;
                case EnemyType.HornDemon:
                    damage = 1;
                    candyNum = 2;
                    break;
                case EnemyType.Cloak:
                    damage = 1;
                    candyNum = 2;
                    break;
            }
        }
    }
}
