using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
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

	/// <summary>
	/// Game object that chases and damages the player.
	/// </summary>
	public class Enemy : GameObject
	{
		// --------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------
		// the type of Enemy
		private EnemyType eType;

		// the hit points of the Enemy
		private int health;

		// how many candies the enemy will drop when it dies
		private int candyNum;

		// the amount of damage that the Enemy can deal to the player
		private int damage;

		// the radius of the MouthDemon's attack
		private Rectangle attackRadius;

		// o.o
		private bool addBullet;

		// the attack and reloadTimer for the Cloak
		private double reloadTimer;

		// Reference to the game's randomizer
		private Random rng;

		// Bounds of the world map
		private int worldWidth;
		private int worldHeight;

		// values needed for the enemy's animation
		private double timer;
		private double shockwaveTimer;
		private double spf;
		private List<Rectangle> enemyAnim;
		private List<Rectangle> shockwaveAnim;
		private Rectangle startingWP;
		private Rectangle shockwaveWP;
        private Rectangle animationWP;

		// Unit direction vector
		private Vector2 direction;

		// Speed scalar 
		private int speed;

		// Reference to the player
		Player player;
		int prevPHealth;

        // fields for damage indication
        private double tintTimer;
		Color tint;

		// --------------------------------------------------------------
		// Properties
		// --------------------------------------------------------------

		/// <summary>
		/// This object's position in the world.
		/// </summary>
		public override Rectangle WorldPosition
		{
			get { return worldPosition; }
		}

		public bool AddBullet
		{
			get { return addBullet; }
			set { addBullet = value; }
		}

		/// <summary>
		/// The amount of damage this enemy does to the player.
		/// </summary>
		public int Damage
		{
			get { return damage; }
		}

		/// <summary>
		/// The time between the player's shots
		/// </summary>
		public double ReloadTimer
		{
			get { return reloadTimer; }
			set { reloadTimer = value; }
		}

		/// <summary>
		/// Whether or not this enemy has any health remaining.
		/// </summary>
		public bool IsAlive
		{
			get { return health > 0; }
		}

		/// <summary>
		/// How much health the player has left.
		/// </summary>
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

        // --------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------
        /// <summary>
        /// Generates a randomly positioned enemy of a given type.
        /// </summary>
        /// <param name="eType">The type of enemy.</param>
        /// <param name="rng">Reference to the game's randomizer.</param>
        /// <param name="asset">Spritesheet of enemies.</param>
        /// Position will be randomized in the constructor.</param>
        /// <param name="screenWidth">Screen's width.</param>
        /// <param name="screenHeight">Screen's height.</param>
        public Enemy(EnemyType eType, Random rng, Texture2D asset, Player player,
			int screenWidth, int screenHeight, int worldWidth, int worldHeight)
			: base(asset, new Rectangle(0, 0, 1, 1), screenWidth, screenHeight)
		{
			this.rng = rng;
			this.worldWidth = worldWidth;
			this.worldHeight = worldHeight;
			this.player = player;
			prevPHealth = 100;
			reloadTimer = 1;

			// Determines type-specific field values for this enemy
			this.eType = eType;
			CreateEnemy(asset);

			// Positions the enemy on the world's border
			GoToWorldEdge();

			// Default animation values
			timer = 0.0;
			shockwaveTimer = 0;
			spf = 0.2;
			tintTimer = 0.0;
			tint = Color.White;
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
				if (currentFrame >= 4)
				{
					currentFrame = 0;
				}

				// Reset the time counter, keeping remaining elapsed time
				timer -= spf;
			}

			// Updates the enemy's rectangles so they move properly
			animationWP.X = worldPosition.X;
			animationWP.Y = worldPosition.Y;
			animationWP.Width = enemyAnim[currentFrame].Width * 3;
			animationWP.Height = enemyAnim[currentFrame].Height * 3;

			if (eType == EnemyType.Imp)
			{
				if (currentFrame == 2)
				{
					animationWP.Y = worldPosition.Y - 2;
				}
			}
			if (shockwaveTimer > 1.3)
			{
				/* if (currentFrame == 0)
				{
					shockwaveWP.X = startingWP.X - 10;
                    shockwaveWP.Y = startingWP.Y + 55;
                    shockwaveWP.Width = shockwaveAnim[0].Width * 2;
                    shockwaveWP.Height = shockwaveAnim[0].Height * 2;
                } */
                if (currentFrame == 1)
                {
                    shockwaveWP.X = startingWP.X;
					shockwaveWP.Y = startingWP.Y + 45;
                    shockwaveWP.Width = shockwaveAnim[1].Width;
                    shockwaveWP.Height = shockwaveAnim[1].Height;
                }
                if (currentFrame == 2)
                {
                    shockwaveWP.X = startingWP.X - 60;
                    shockwaveWP.Y = startingWP.Y - 15;
                    shockwaveWP.Width = shockwaveAnim[2].Width * 3;
                    shockwaveWP.Height = shockwaveAnim[2].Height * 3;
                }
                if (currentFrame == 3)
                {
                    shockwaveWP.X = startingWP.X - 154;
                    shockwaveWP.Y = startingWP.Y - 109;
                    shockwaveWP.Width = shockwaveAnim[3].Width * 5;
                    shockwaveWP.Height = shockwaveAnim[3].Height * 5;
                }
				if (shockwaveTimer < 1.3)
				{
					shockwaveWP.Width = 2;
					shockwaveWP.Height = 2;
				}
            }
            if (eType == EnemyType.Cloak)
            {
                if (currentFrame == 0)
                {
                    animationWP.Y = worldPosition.Y + 3;
                }
                if (currentFrame == 2)
                {
                    animationWP.Y = worldPosition.Y - 3;
                }
            }
        }

		/// <summary>
		/// Updates the enemies position to always 
		/// move towards the player
		/// </summary>
		/// <param name="gameTime">Info from MonoGame about the time state.</param>
		public override void Update(GameTime gameTime)
		{
			// Creates a direction vector pointing toward the player
			direction = new Vector2(player.WorldPosition.X - worldPosition.X,
									player.WorldPosition.Y - worldPosition.Y);

			// As long as the direction is not the zero vector, it is normalized so 
			// the enemy will have the same displacement every movement frame
			if (direction != Vector2.Zero)
			{
				direction.Normalize();
			}

			// Updates world position by moving toward the player
			worldPosition.X += (int)Math.Round(direction.X * speed);
			worldPosition.Y += (int)Math.Round(direction.Y * speed);
			attackRadius.X = worldPosition.X - 130;
			attackRadius.Y = worldPosition.Y - 130;
			attackRadius.Width = 375;
			attackRadius.Height = 375;

            // Updates the enemy's animation
            reloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;
			shockwaveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            UpdateAnimation(gameTime);

			//Updates the enemy's hurt status
			if (tintTimer > 0)
			{
				tintTimer -= gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (tintTimer < 0)
			{
				tintTimer = 0;
				tint = Color.White;
			}
		}

		/// <summary>
		/// Draws the enemy based on its enemy type
		/// </summary>
		/// <param name="sb"></param>
		public override void Draw(SpriteBatch sb)
		{
			// Draws the shockwave
            if (shockwaveTimer > 1.3 && eType == EnemyType.MouthDemon && currentFrame > 0)
            {
                sb.Draw(asset,
                        shockwaveWP,
                        shockwaveAnim[currentFrame],
                        Color.White);
            }

            // Draws the enemy
            if (eType == EnemyType.Imp || eType == EnemyType.Cloak)
			{
				if (direction.X > 0)
				{
					sb.Draw(asset,
							animationWP,
							enemyAnim[currentFrame],
							tint);
				}
				else
				{
					sb.Draw(asset,
							animationWP,
							enemyAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
				}
			}
			else
			{
				if (direction.X > 0)
				{
					sb.Draw(asset,
							worldPosition,
							enemyAnim[currentFrame],
							tint);
				}
				else
				{
					sb.Draw(asset,
							worldPosition,
							enemyAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
				}
			}
			
		}

		/// <summary>
		/// If this object is colliding with another given object.
		/// </summary>
		/// <param name="gameObject">The object to check collisions with.</param>
		/// <returns>Whether or not the objects' position rectangles intersect.</returns>
		public bool CollidesWith(GameObject gameObject)
		{
			return worldPosition.Intersects(gameObject.WorldPosition);
		}

		/// <summary>
		/// When the enemy dies, Candy will be drawn near the enemy position
		/// </summary>
		/// <param name="collectibles"> List of dropped candies </param>
		public void DropCandy(List<Candy> collectibles, Texture2D candyAsset)
		{
			// Add all the dropped candies to the collectibles list
			for (int i = 0; i < candyNum; i++)
			{
				// Position is randomized close to the enemy
				collectibles.Add(new Candy(
					candyAsset,
					new Rectangle(
						worldPosition.X + rng.Next(-32, 41),
						worldPosition.Y + rng.Next(-32, 41),
						24,
						24),
					screenWidth,
					screenHeight));
			}
		}

		/// <summary>
		/// Instead of having this as a property, get information from the camera 
		/// to get the accurate bounds to determine whether the enemy is on screen.
		/// </summary>
		/// <param name="camera">The current camera created and used in Game1</param>
		/// <returns>Whether this enemy is on screen; if it is seen by the camera.</returns>
		public bool IsOnScreen(OrthographicCamera camera)
		{
			//return camera.CameraBound.Contains(this.worldPosition);

			return camera.CameraBound.Intersects(this.worldPosition);
		}

		/// <summary>
		/// A helper method used when loading the enemies in order to
		/// set their damage, source rectangle, and how many candies they drop.
		/// </summary>
		private void CreateEnemy(Texture2D asset)
		{
			//Initializes the Enemy fields based on the Enemy type
			switch (eType)
			{
				case EnemyType.Imp:
					health = 1;
					damage = 1;
					candyNum = 1;
					speed = 4;
					animationWP = new Rectangle(0, 0, 30, 36);
					enemyAnim = new List<Rectangle>(4);
					enemyAnim.Add(new Rectangle(4, 20, 10, 12));
					enemyAnim.Add(new Rectangle(20, 20, 10, 12));
					enemyAnim.Add(new Rectangle(36, 19, 10, 12));
                    enemyAnim.Add(new Rectangle(20, 20, 10, 12));
                    break;
				case EnemyType.MouthDemon:
					health = 5;
					damage = 2;
					candyNum = 3;
					speed = 1;
                    attackRadius = new Rectangle(worldPosition.X - 150, worldPosition.Y - 150, 300, 300);
                    enemyAnim = new List<Rectangle>(4);
					enemyAnim.Add(new Rectangle(5, 81, 22, 30));
					enemyAnim.Add(new Rectangle(5, 81, 22, 30));
					enemyAnim.Add(new Rectangle(36, 80, 23, 31));
					enemyAnim.Add(new Rectangle(36, 80, 23, 31));
                    shockwaveAnim = new List<Rectangle>(4);
                    shockwaveAnim.Add(new Rectangle(70, 18, 45, 45));
                    shockwaveAnim.Add(new Rectangle(123, 1, 95, 95));
                    shockwaveAnim.Add(new Rectangle(137, 114, 71, 71));
                    shockwaveAnim.Add(new Rectangle(19, 117, 80, 80));
                    break;
				case EnemyType.HornDemon:
					health = 2;
					damage = 1;
					candyNum = 2;
					speed = 2;
					enemyAnim = new List<Rectangle>(4);
					enemyAnim.Add(new Rectangle(3, 60, 11, 18));
					enemyAnim.Add(new Rectangle(3, 60, 11, 18));
					enemyAnim.Add(new Rectangle(35, 63, 11, 15));
					enemyAnim.Add(new Rectangle(35, 63, 11, 15));
					break;
				case EnemyType.Cloak:
					health = 3;
					damage = 1;
					speed = 2;
					candyNum = 2;
                    animationWP = new Rectangle(0, 0, 30, 36);
                    enemyAnim = new List<Rectangle>(4);
					enemyAnim.Add(new Rectangle(1, 40, 13, 15));
					enemyAnim.Add(new Rectangle(18, 39, 14, 16));
                    enemyAnim.Add(new Rectangle(37, 38, 12, 17));
                    enemyAnim.Add(new Rectangle(18, 39, 14, 16));
                    break;
			}

			// Changes the enemy worldPosition based on the enemy's sourceRect
			worldPosition = new Rectangle(0, 0, enemyAnim[0].Width * 3, enemyAnim[0].Height * 3);
		}

		/// <summary>
		/// Randomizes the enemy's position to somewhere on the border
		/// </summary>
		public void GoToWorldEdge()
		{
			// Randomizes enemy's position to somewhere on the border
			int edge = rng.Next(4);
			switch (edge)
			{
				// Left edge, random height
				case 0:
					worldPosition.X = -worldPosition.Width;
					worldPosition.Y = rng.Next(-worldPosition.Height, worldHeight);
					break;

				// Right edge, random height
				case 1:
					worldPosition.X = worldWidth;
					worldPosition.Y = rng.Next(-worldPosition.Height, worldHeight);
					break;

				// Top edge, random X value
				case 2:
					worldPosition.X = rng.Next(-worldPosition.Width, worldWidth);
					worldPosition.Y = -worldPosition.Height;
					break;

				// Bottom edge, random X value
				case 3:
					worldPosition.X = rng.Next(-worldPosition.Width, worldWidth);
					worldPosition.Y = worldHeight;
					break;
			}
		}

		public void Attack(OrthographicCamera camera)
		{
			if (eType == EnemyType.Cloak && IsOnScreen(camera))
			{
				// Makes the actual bullet
				if (reloadTimer <= 0)
				{
					reloadTimer = 2;
					addBullet = true;
				}
			}

			if (eType == EnemyType.MouthDemon)
			{
				// Check the enemy's attack rectangle
				if (attackRadius.Contains(player.WorldPosition) &&
					shockwaveTimer <= 0) 
				{
                    currentFrame = 0;
					startingWP = new Rectangle(worldPosition.X - 20,
											   worldPosition.Y,
											   worldPosition.Width,
											   worldPosition.Height);
					shockwaveTimer = 2;
				}
				// Saves the previous player health so the shockwave
				// doesnt one shot the player
				if (prevPHealth == 100)
				{
                    prevPHealth = player.Health;
                }
                // Checks if shockwave hits the player
                if (shockwaveWP.Intersects(player.WorldPosition) &&
                        !Game1.GodMode)
                {
                    player.Hurt = true;
					player.Health = prevPHealth - 2;
                }
                if (player.Hurt == false)
                {
					prevPHealth = 100;
                }
            }
		}

		/// <summary>
		/// Changes the color of the Enemy (called in the Manager) 
		/// when they collide with the Bullets
		/// </summary>
		public void Hurt()
		{
			tintTimer = 0.5;
			tint = Color.Red;
		}
	}
}
