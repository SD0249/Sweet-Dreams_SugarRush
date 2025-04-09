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
        /// <summary>
        /// Whether or not any part of the object is visible on the screen.
        /// </summary>
        public bool IsOnScreen
        {
            get
            {
                return !(screenPosition.X + screenPosition.Width < 0
                    || screenPosition.X > screenWidth
                    || screenPosition.Y + screenPosition.Height < 0
                    || screenPosition.Y > screenHeight);
            }
        }

        // --------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------
        /// <summary>
        /// Randomly generates an Enemy
        /// </summary>
        public Enemy(Random rng, Texture2D asset, Rectangle worldPosition,
            int screenWidth, int screenHeight)
            :base(asset, worldPosition, worldPosition, screenWidth, screenHeight)
        {
            isAlive = true;
            screenPosition = worldPosition;

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
        public Enemy(EnemyType eType, Texture2D asset, Rectangle worldPosition, 
            int screenWidth, int screenHeight) 
            : base(asset, worldPosition, worldPosition, screenWidth, screenHeight)
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
        /// 
        // THIS IS TEMPORARY RIGHT NOW MAKE SURE TO CHANGE IF NEEDED
        ///
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateAnimation(GameTime gameTime)
        {
            // ElapsedGameTime is the duration of the last GAME frame
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time passed to flip to the next frame?
            if (timer >= spf)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                currentFrame++;
                if (currentFrame >= 3)
                {
                    currentFrame = 0;
                }

                // Reset the time counter, keeping remaining elapsed time
                timer -= spf;
            }
        }

        /// <summary>
        /// Updates the enemies position to always 
        /// move towards the player
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="worldToScreen"> World to screen offset vector </param>
        public override void Update(GameTime gameTime, Vector2 worldToScreen)
        {
            // Updates world position by moving toward the player
            
            // Updates screen position
            screenPosition = new Rectangle(
                worldPosition.X - (int)worldToScreen.X,
                worldPosition.Y - (int)worldToScreen.Y, 
                worldPosition.Width, 
                worldPosition.Height);
        }

        /// <summary>
        /// Draws the enemy based on it's enemy type
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            //Draws an Enemy at a given position
            // CLOAK
            if (eType == EnemyType.Cloak)
            {
                sb.Draw(
                asset,
                worldPosition,
                new Rectangle(2, 7, 12, 15), // x, y, width, height
                Color.White);
            }

            //IMP
            if (eType == EnemyType.Imp)
            {
                sb.Draw(
                asset,
                worldPosition,
                new Rectangle(5, 5, 9, 13),
                Color.White);
            }
             

            //MOUTH DEMON
            if (eType == EnemyType.MouthDemon)
            {
                sb.Draw(
                asset,
                worldPosition,
                new Rectangle(5, 7, 20, 35),
                Color.White);
            }

            // HORN DEMON
            if (eType == EnemyType.HornDemon)
            {
                sb.Draw(
                asset,
                worldPosition,
                new Rectangle(4, 6, 11, 23),
                Color.White);
            }
        }

        /// <summary>
        /// Checks if the enemy collied with the player
        /// </summary>
        /// <returns> If they are colliding </returns>
        public bool CollidesWith()
        {
            //if (worldPosition.Intersects())
            return false;
        }

        /// <summary>
        /// When the enemy dies, Candy will be drawn near the enemy position
        /// </summary>
        /// <param name="collectibles"> List of dropped candies </param>
        public void DropCandy(List<Candy> collectibles)
        {
            // If the enemy is dead
            if (!isAlive)
            {
                // Add all the dropped candies to the collectibles list
                for (int i = 0; i < candyNum; i++)
                {
                    // Should be drawn at the enemy's death position
                    collectibles.Add(new Candy(asset, worldPosition, screenWidth, screenHeight));
                }
            }
        }

        /// <summary>
        /// A helper method used when loading the enemies
        /// in order to set their damage and how many candies they drop
        /// </summary>
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
