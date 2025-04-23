using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Sweet_Dreams
{
	internal class EnemyBullet : GameObject
	{
		// --------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------
		private Player player;
		private int speed;
		private float rotation;
		private Vector2 direction;
		private Vector2 origin;
		private bool hitPlayer;
		private int damage;

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
		/// Whether or not this bullet has hit an enemy.
		/// </summary>
		public bool HitPlayer
		{
			get { return hitPlayer; }
			set { hitPlayer = value; }
		}

		/// <summary>
		/// How much damage this bullet deals to an enemy.
		/// </summary>
		public int Damage
		{
			get { return damage; }
		}

		// --------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------
		public EnemyBullet(Texture2D asset, Rectangle worldPosition,
			int damage, int screenWidth, int screenHeight, int worldWidth, int worldHeight)
		: base(asset, worldPosition, screenWidth, screenHeight)
		{
			this.asset = asset;
			this.damage = damage;
			hitPlayer = false;
			speed = 3;
			//mouse = Mouse.GetState(); (playerPosition)
			origin = new Vector2(0, 0);
			float startingX = 385;
			float startingY = 213;

			// If the enemy is left of the center...
			if (worldPosition.X < 385)
			{
				// Match the bullets startingX to the enemy's X
				startingX = worldPosition.X;
			}
			// If the enemy is right of the center...
			if (worldPosition.X > worldWidth - 385)
			{
				// Match the bullets startingX to the center
				// plus how far right of the center the enemy is 
				startingX = Math.Abs(385 + (worldPosition.X - worldWidth + 385));
			}

			// If the enemy is above the center...
			if (worldPosition.Y < 213)
			{
				// Match the bullets startingY to the enemy's Y
				startingY = worldPosition.Y;
			}
			// If the enemy is below the center...
			if (worldPosition.Y > worldHeight - 213)
			{
				// Match the bullets startingY to the center
				// plus how far below the center the enemy is 
				startingY = Math.Abs(213 + (worldPosition.Y - worldHeight + 213));
			}

			// Finding the rotation of the bullet based off the player
			rotation = (float)Math.Atan2(startingY - player.WorldPosition.Y,
										 startingX - player.WorldPosition.X);

			// Finding the direction the bullet need to go based of the rotation
			direction = new Vector2((float)Math.Cos(rotation + 3.14), (float)Math.Sin(rotation + 3.14));
		}

		// --------------------------------------------------------------
		// Methods
		// --------------------------------------------------------------

		/// <summary>
		/// Bullets won't have an animation for this project
		/// The only animation they would have is exploding (not doing that)
		/// </summary>
		/// <param name="gameTime">Info from Monogame about the time state.</param>
		public override void UpdateAnimation(GameTime gameTime)
		{

		}

		/// <summary>
		/// Updates the bullet's world & screen position
		/// </summary>
		/// <param name="gameTime">Info from Monogame about the time state.</param>
		public override void Update(GameTime gameTime)
		{
			// Changing the bullets position
			worldPosition.X += (int)Math.Round(direction.X * speed);
			worldPosition.Y += (int)Math.Round(direction.Y * speed);
		}

		/// <summary>
		/// Draws the bullet :]
		/// </summary>
		/// <param name="sb"> SpriteBatch to draw with </param>
		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(
				asset,
				new Rectangle(worldPosition.X + 15, worldPosition.Y + 10, 16, 16),
				new Rectangle(0, 0, 16, 16),
				Color.White,
				rotation - 0.78f,
				origin,
				SpriteEffects.None,
				1);

			//if (Game1.GodMode)
			//{
			//    DebugLib.DrawRectOutline(sb, screenPosition, 3, Color.Black);
			//}
		}


		/// <summary>
		/// Instead of having this as a property, get information from the camera 
		/// to get the accurate bounds to determine whether the bullet is on screen.
		/// </summary>
		/// <param name="camera">The current camera created and used in Game1</param>
		/// <returns>Whether this bullet is on screen; if it is seen by the camera.</returns>
		public bool IsOnScreen(OrthographicCamera camera)
		{
			return camera.CameraBound.Intersects(this.worldPosition);
		}

		/// <summary>
		/// Determines whether or not the bullet is past the world's bounds.
		/// </summary>
		/// <param name="worldWidth">Width of the world map.</param>
		/// <param name="worldHeight">Height of the world map.</param>
		/// <returns>Whether or not the bullet has traveled off the world map.</returns>
		public bool OutOfBounds(int worldWidth, int worldHeight)
		{
			return worldPosition.Y + worldPosition.Height < 0       // Too far up
				|| worldPosition.Y > worldHeight                    // Too far down
				|| worldPosition.X + worldPosition.Width < 0        // Too far left
				|| worldPosition.X > worldWidth;                    // Too far right
		}
	}
}
