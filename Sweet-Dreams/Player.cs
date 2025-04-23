using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Net.Http;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
	public enum PlayerState
	{
		WalkRight,
		WalkLeft,
		FaceRight,
		FaceLeft,
		Dead,
		Hit
	}

	public class Player : GameObject
	{
		// --------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------
		private PlayerState playerState;
		private PlayerState prevPS;
		private bool hurt;
		private int health;
		private double reloadTimer;
		private double stunTimer;
		private int playerHealth;
		private bool isAlive;
		private int damage;
		private int points;
		private int speed;
		private Vector2 direction;

		// Animation fields
		private List<Rectangle> idleAnim;
		private List<Rectangle> walkingAnim;
		private List<Rectangle> damageAnim;
		private List<Rectangle> deathAnim;
		private int currentAnim;
		private int prevAnim;
		private Rectangle animationWP;
		private double timer;
		private double spf;
		private Color tint;
		private double effectTimer;

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

		/// <summary>
		/// The current animation state of the player.
		/// </summary>
		public PlayerState PlayerState
		{
			get { return playerState; }
		}

		/// <summary>
		/// Keeps track of whether the player has been hurt
		/// (so there doesn't have to be a 'set' for PlayerState)
		/// </summary>
		public bool Hurt
		{
			set { hurt = value; }
		}

		/// <summary>
		/// The player's remaining health.
		/// </summary>
		public int Health
		{
			get { return health; }
			set { health = value; }
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
		/// How much damage the player's bullets deal.
		/// </summary>
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		/// <summary>
		/// The color the player should be tinted.
		/// </summary>
		public Color Tint
		{
			get { return tint; }
		}

		/// <summary>
		/// The number of points the player has earned.
		/// </summary>
		public int Points
		{
			get { return points; }
			set { points = value; }
		}

		// --------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------
		public Player(Texture2D asset, Rectangle worldPosition,
			int screenWidth, int screenHeight)
			: base(asset, worldPosition, screenWidth, screenHeight)
		{
			health = 6;
			damage = 1;
			speed = 2;
			points = 0;
			tint = Color.White;
			stunTimer = 0.9;
			reloadTimer = 1;
			direction = Vector2.Zero;
			timer = 0.0;
			spf = 0.3;
			effectTimer = 1.0;

			// Making animation lists
			idleAnim = new List<Rectangle>(4);
			idleAnim.Add(new Rectangle(4, 6, 17, 18));
			idleAnim.Add(new Rectangle(4, 6, 17, 18));
			idleAnim.Add(new Rectangle(28, 7, 17, 17));
			idleAnim.Add(new Rectangle(28, 7, 17, 17));

			walkingAnim = new List<Rectangle>(4);
			walkingAnim.Add(new Rectangle(4, 31, 17, 17));
			walkingAnim.Add(new Rectangle(28, 30, 17, 18));
			walkingAnim.Add(new Rectangle(52, 31, 17, 17));
			walkingAnim.Add(new Rectangle(76, 30, 17, 18));

			damageAnim = new List<Rectangle>(4);
			damageAnim.Add(new Rectangle(52, 6, 17, 18));
			damageAnim.Add(new Rectangle(52, 6, 17, 18));
			damageAnim.Add(new Rectangle(77, 6, 17, 18));
			damageAnim.Add(new Rectangle(77, 6, 17, 18));

			deathAnim = new List<Rectangle>(4);
			deathAnim.Add(new Rectangle(3, 54, 17, 18));
			deathAnim.Add(new Rectangle(28, 55, 17, 17));
			deathAnim.Add(new Rectangle(54, 59, 16, 13));
			deathAnim.Add(new Rectangle(79, 64, 15, 8));

			animationWP = new Rectangle(worldPosition.X,
										worldPosition.Y + (currentFrame + 1) % 2,
										walkingAnim[currentFrame].Width * 3 - 4,
										walkingAnim[currentFrame].Height * 3);
			currentAnim = 0;
			prevAnim = currentAnim;
		}

		// --------------------------------------------------------------
		// Methods
		// --------------------------------------------------------------
		/// <summary>
		/// Updates the 'current frame' for the player's animation
		/// </summary>
		/// <param name="gameTime">Info from Monogame about the time state.</param>
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

			if (currentAnim == 4)
			{
				if (currentFrame == 1)
				{
					if (prevPS == PlayerState.FaceRight ||
						prevPS == PlayerState.WalkRight)
					{
						animationWP.X = worldPosition.X + 3;
					}
					else
					{
						animationWP.X = worldPosition.X - 3;
					}

					animationWP.Y = worldPosition.Y + 2;
					animationWP.Height = 48;
				}
				if (currentFrame == 2)
				{
					if (prevPS == PlayerState.FaceRight ||
						prevPS == PlayerState.WalkRight)
					{
						animationWP.X = worldPosition.X + 3;
					}
					else
					{
						animationWP.X = worldPosition.X - 12;
					}

					animationWP.Y = worldPosition.Y + 10;
					animationWP.Width = 48;
					animationWP.Height = 36;
				}
				if (currentFrame == 3)
				{
					if (prevPS == PlayerState.FaceRight ||
						prevPS == PlayerState.WalkRight)
					{
						animationWP.X = worldPosition.X + 5;
					}
					else
					{
						animationWP.X = worldPosition.X - 15;
					}

					animationWP.Y = worldPosition.Y + 18;
					animationWP.Width = 45;
					animationWP.Height = 32;
				}
			}
		}

		/// <summary>
		/// Updates the players position based on what keys are pressed
		/// </summary>
		/// <param name="gameTime">Info from Monogame about the time state.</param>
		public override void Update(GameTime gameTime)
		{
			UpdateAnimation(gameTime);
			prevAnim = currentAnim;

			// If the player is hurt
			if (hurt)
			{
				hurt = false;
				// Save the previous player state
				if (playerState != PlayerState.Hit)
				{
					prevPS = playerState;
				}

				// Then change the player state to hit
				playerState = new PlayerState();
				playerState = PlayerState.Hit;
			}

			// If the player dies, they're dead ;)
			if (health <= 0)
			{
				playerState = PlayerState.Dead;
			}

			// Moves the player based on keyboard input
			MoveOnKeyPress();

			// Player FSM
			switch (playerState)
			{
				case PlayerState.WalkLeft:
					// Color Update
					if (tint != Color.White)
					{
						// Ensure the effect from the candy is consistent in walking/idle states.
						if (effectTimer > 0)
						{
							effectTimer -= gameTime.ElapsedGameTime.TotalSeconds;
						}
						// Change back after this amount of time.
						else
						{
							tint = Color.White;
							effectTimer = 1.0;
						}
					}

					// Updates the walking world position (so the player's head bobs)
					animationWP.X = worldPosition.X;
					animationWP.Y = worldPosition.Y + 2 * ((currentFrame + 1) % 2);
					currentAnim = 1;

					// Updates player state
					if (direction == Vector2.Zero)
					{
						playerState = PlayerState.FaceLeft;
					}
					if (direction.X > 0)
					{
						playerState = PlayerState.WalkRight;
					}
					if (health <= 0)
					{
						playerState = PlayerState.FaceLeft;
					}

					break;

				case PlayerState.WalkRight:
					// Color Update
					if (tint != Color.White)
					{
						// Ensure the effect from the candy is consistent in walking/idle states.
						if (effectTimer > 0)
						{
							effectTimer -= gameTime.ElapsedGameTime.TotalSeconds;
						}
						// Change back after this amount of time.
						else
						{
							tint = Color.White;
							effectTimer = 1.0;
						}
					}

					// Updates the walking world position
					animationWP.X = worldPosition.X;
					animationWP.Y = worldPosition.Y + 2 * ((currentFrame + 1) % 2);
					currentAnim = 2;

					// Updates player state
					if (direction == Vector2.Zero)
					{
						playerState = PlayerState.FaceRight;
					}
					if (direction.X < 0)
					{
						playerState = PlayerState.WalkLeft;
					}
					if (health <= 0)
					{
						playerState = PlayerState.Dead;
					}
					break;

				case PlayerState.FaceLeft:
				case PlayerState.FaceRight:
					// Color Update
					if (tint != Color.White)
					{
						// Ensure the effect from the candy is consistent in walking/idle states.
						if (effectTimer > 0)
						{
							effectTimer -= gameTime.ElapsedGameTime.TotalSeconds;
						}
						// Change back after this amount of time.
						else
						{
							tint = Color.White;
							effectTimer = 1.0;
						}
					}

					// Updates the animation world position
					animationWP.X = worldPosition.X;
					if (currentFrame == 0 || currentFrame == 1)
					{
						animationWP.Y = worldPosition.Y;
					}
					else
					{
						animationWP.Y = worldPosition.Y + 2;
					}
					if (playerState == PlayerState.FaceRight)
					{
						currentAnim = 0;
					}
					else
					{
						currentAnim = 5;
					}

					// Updates player state
					if (direction.X < 0 || (direction.Y != 0 &&
						playerState == PlayerState.FaceLeft))
					{
						playerState = PlayerState.WalkLeft;
					}
					if (direction.X > 0 || (direction.Y != 0 &&
						playerState == PlayerState.FaceRight))
					{
						playerState = PlayerState.WalkRight;
					}
					break;

				case PlayerState.Hit:
					// Update the color tint
					tint = Color.Red;

					// Updates the walking world position
					animationWP.X = worldPosition.X;
					animationWP.Y = worldPosition.Y + 2 * ((currentFrame + 1) % 2);
					currentAnim = 3;

					if (stunTimer <= 0)
					{
						// Reset the player state as well as the player tint
						playerState = prevPS;
						prevPS = new PlayerState();
						stunTimer = 0.8;
						tint = Color.White;
					}
					stunTimer -= gameTime.ElapsedGameTime.TotalSeconds;
					break;

				case PlayerState.Dead:
					tint = Color.White;
					currentAnim = 4;
					speed = 0;
					break;
			}

			if (currentAnim != prevAnim)
			{
				currentFrame = 0;
			}
		}

		/// <summary>
		/// Draws the player to the screen
		/// </summary>
		/// <param name="sb"> SpriteBatch to draw with </param>
		public override void Draw(SpriteBatch sb)
		{
			// Player FSM
			switch (playerState)
			{
				case PlayerState.WalkLeft:


					sb.Draw(asset,
							animationWP,
							walkingAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
					break;

				case PlayerState.WalkRight:
					sb.Draw(asset,
							animationWP,
							walkingAnim[currentFrame],
							tint);
					break;

				case PlayerState.FaceLeft:
					sb.Draw(asset,
							animationWP,
							idleAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
					break;

				case PlayerState.FaceRight:
					sb.Draw(asset,
						   animationWP,
						   idleAnim[currentFrame],
						   tint);
					break;

				case PlayerState.Hit:
					// Checks if the player's anim needs to be flipped
					if (prevPS == PlayerState.FaceRight ||
						prevPS == PlayerState.WalkRight)
					{
						sb.Draw(asset,
						   animationWP,
						   damageAnim[currentFrame],
						   tint);
					}
					else
					{
						sb.Draw(asset,
							animationWP,
							damageAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
					}
					break;

				// Change Game1 to wait 1 second before displaying the game over screen
				case PlayerState.Dead:
					// Checks if the player's anim needs to be flipped
					if (prevPS == PlayerState.FaceRight ||
						prevPS == PlayerState.WalkRight)
					{
						sb.Draw(asset,
						   animationWP,
						   deathAnim[currentFrame],
						   tint);
					}
					else
					{
						sb.Draw(asset,
							animationWP,
							deathAnim[currentFrame],
							tint,
							0,
							new Vector2(0, 0),
							SpriteEffects.FlipHorizontally,
							0);
					}
					break;
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
		/// In relation to the world width and height, 
		/// this method ensures that the player doesn't move out the bounds of the drawn map.
		/// </summary>
		/// <param name="worldWidth">World Width; Num of columns * Size of a tile</param>
		/// <param name="worldHeight">World Height; Num of rows * Size of a tile</param>
		public void KeepPlayerInBounds(int worldWidth, int worldHeight)
		{
			// If the player is too FAR LEFT
			if (worldPosition.X < 0)
			{
				worldPosition.X = 0;
			}

			// If the player is too FAR RIGHT
			if (worldPosition.X + worldPosition.Width > worldWidth)
			{
				worldPosition.X = worldWidth - worldPosition.Width;
			}

			// If the player is too FAR UP
			if (worldPosition.Y < 0)
			{
				worldPosition.Y = 0;
			}

			// If the player is too FAR DOWN
			if (worldPosition.Y + worldPosition.Height > worldHeight)
			{
				worldPosition.Y = worldHeight - worldPosition.Height;
			}
		}

		/// <summary>
		/// Gives a buff or debuff to the player based on the type of candy it is collecting.
		/// </summary>
		/// <param name="candyType">Type of candy that is being collected.</param>
		public void CollectCandy(CandyType candyType)
		{
			switch (candyType)
			{
				case CandyType.SkullCandy:
					// Deal damage to the player
					if (!Game1.GodMode)
					{
						health--;
						tint = Color.Red;
					}
					break;

				case CandyType.Peppermint:
<<<<<<< HEAD
					// Increase bullet reload time to half
					reloadTimer *= 0.5;
					tint = Color.Blue;
=======
					// Increase bullet velocity
					tint = Color.Cyan;
>>>>>>> 989ed48e7c81af22d045e00a4eb7a4dac5a832ca
					break;

				case CandyType.CandyCorn:
					// TEST
					// bullet pickups



					break;

				case CandyType.GreenCandy:
					// TO DO: Maybe add sound effect for these guys too?
					// Add 5 points to player points
					points += 5;
					tint = Color.Yellow;
					break;

				case CandyType.YellowCandy:
					// Add 10 points to player points
					points += 10;
					tint = Color.Yellow;
					break;
				case CandyType.Chocolate:
					// Heal player health
					if (health < 6)
					{
						health++;
					}
					break;
			}
			/*
			 * BUFFS AND DEBUFFS
			 * 
			 * BUFFS:
			 * More damage
			 * reload timer
			 * 
			 * DEBUFFS
			 * slower speed
			 * 
			 */
		}

		/// <summary>
		/// Moves the player based on which arrows are pressed.
		/// </summary>
		private void MoveOnKeyPress()
		{
			// Gets the current keyboard state
			KeyboardState kbState = Keyboard.GetState();

			// Zeros the direction vector
			direction = Vector2.Zero;

			// Changes direction based on arrow and WASD key presses
			if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
			{
				direction.X -= 1;
			}
			if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
			{
				direction.X += 1;
			}
			if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.W))
			{
				direction.Y -= 1;
			}
			if (kbState.IsKeyDown(Keys.Down) || kbState.IsKeyDown(Keys.S))
			{
				direction.Y += 1;
			}

			// Normalizes the direction vector
			if (direction != Vector2.Zero)
			{
				direction.Normalize();
			}

			// Speed scalar used for movement calculation is faster if in god mode
			int speedScalar = speed;
			if (Game1.GodMode)
			{
				speedScalar = 4;
			}

			// Moves the player by scaling its direction vector by its speed stat
			worldPosition = new Rectangle(
				worldPosition.X + (int)Math.Round(direction.X * speedScalar),
				worldPosition.Y + (int)Math.Round(direction.Y * speedScalar),
				worldPosition.Width,
				worldPosition.Height);
		}
	}
}
