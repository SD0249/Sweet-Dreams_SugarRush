using System;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Sweet Dreams - Sugar Rush
// A shooter game. Kill all the enemies to survive and collect candies!
namespace Sweet_Dreams
{
    /// <summary>
    /// Possible states of the game
    /// </summary>
    public enum GameState
    {
        Menu,
        Game,
        Win,
        Lose
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Additional fields used for the game
        private Random rng;
        private List<Candy> collectibles;
        private List<Bullet> bullets;
        private EnemyManager enemyManager;
        private GameState gameState;
        private MouseState mouse;
        private Player player;
        private int screenWidth;
        private int screenHeight;
        private int worldWidth;
        private int worldHeight;
        private Vector2 worldToScreen;
        private Texture2D playerAnimation;
        private Texture2D purpleDungeon;
        private Texture2D candySprites;
        private SpriteFont arial12;
        private PlayerState currentPlayerState;
        private bool doorIsReached;
        private Level level1;

        // Whether or not the game is currently in debug mode
        public static bool debugMode;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;
            bullets = new List<Bullet>();
            debugMode = true;

            // WorldToScreen initialized for testing
            worldToScreen = new Vector2(0, 0);

            mouse = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerAnimation = Content.Load<Texture2D>("PlayerAnimation");
            purpleDungeon = Content.Load<Texture2D>("Full");
            arial12 = Content.Load<SpriteFont>("arial12");
            candySprites = Content.Load<Texture2D>("acursedpixel_16x16_candyicons");

            // Load the Level
            level1 = new Level(purpleDungeon, "../../../Content/purpleDungeonTextureMapping.txt", _spriteBatch);
            level1.LoadLevel("../../../Content/Level1.txt");

            player = new Player(playerAnimation, 
                new Rectangle(screenWidth/2 - 15, screenHeight/2 - 27, 38, 52), 
                screenHeight, 
                screenHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouse = Mouse.GetState();

            switch (gameState)
            {
                case GameState.Menu:

                    // Draw menu to console

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Game;
                    }

                    // If button is pressed?
                    // Need button class

                    break;

                case GameState.Game:

                    // Draw game to console

                    // TODO: Initialize fields before uncommenting
                    /*
                    // Victory when all enemies are gone and the player is touching the door
                    // if (enemyManager.IsLevelCleared() && doorIsReached)
                    {
                        gameState = GameState.Win;
                    }
                    */

                    // Checks for a left click and bullet timer to shoot
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        player.ReloadTimer <= 0)
                    {
                        // Makes a new bullet every time you shoot
                        bullets.Add(new Bullet(candySprites, player.Position, screenWidth, screenHeight));

                        // Resets the timer for reloading the gun 
                        player.ReloadTimer = 1;
                    }
                    // Has a 1 second timer between shooting a bullet
                    player.ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // Updates all bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime);
                    }
                    // Update Methods for the player
                    player.Update(gameTime);
                    player.UpdateAnimation(gameTime);

                    // If the player is dead the game state changes to lose
                    if (currentPlayerState == PlayerState.Dead)
                    {
                        gameState = GameState.Lose;
                    }

                    break;

                case GameState.Win:

                    // Draw win screen to console

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }

                    break;

                case GameState.Lose:

                    // Draw game over to console
                    // Press enter to continue back to home screen 

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }

                    break;
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // If in Game mode, the following is drawn translated with respect to
            // the player's world position
            if (gameState == GameState.Game)
            {
            // Draws everything whose position needs to be translated
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateTranslation(player.Position.X, player.Position.Y, 0));

                // Draws the level itself
                // level1.DisplayTiles(_spriteBatch);

                // TODO: Uncomment the following once fields are initialized
                /*
                // Draws all candies that are on screen
                for (int i = 0; i < collectibles.Count; i++)
                {
                    if (collectibles[i].IsOnScreen(worldToScreen))
                    {
                        collectibles[i].Draw(_spriteBatch);
                    }
                }

                // Draws all enemies that are on screen
                enemyManager.DrawAll(_spriteBatch, worldToScreen);
                */

                // Draws all bullets that are on screen
                             
                

                _spriteBatch.End();
            }
            
            // Draws everything that should be stationary on the screen
            _spriteBatch.Begin();
            
            switch (gameState)
            {
                case GameState.Menu:

                    GraphicsDevice.Clear(Color.Black);

                    _spriteBatch.DrawString(
                        arial12,
                        "         Sweet Dreams\n Press enter to start game",
                        new Vector2(300, 200),
                        Color.White);

                    break;

                case GameState.Game:

                    GraphicsDevice.Clear(Color.Black);

                    level1.DisplayTiles(_spriteBatch, worldToScreen, screenWidth, screenHeight);

                    //Draws the player
                    player.Draw(_spriteBatch);

                    for (int i = 0; i < bullets.Count; i++)
                    {
                        if (bullets[i].IsOnScreen(worldToScreen))
                        {
                            bullets[i].Draw(_spriteBatch);
                        }
                    }

                    break;

                case GameState.Win:

                    GraphicsDevice.Clear(Color.Honeydew);

                    _spriteBatch.DrawString(
                        arial12,
                        "#YouWon",
                        new Vector2(300, 200),
                        Color.Black);

                    break;

                case GameState.Lose:

                    GraphicsDevice.Clear(Color.Black);

                    _spriteBatch.DrawString(
                        arial12,
                        "#YouGetNoSweetDreams",
                        new Vector2(300, 200),
                        Color.White);

                    break;
            }

            //Draws the Debug Information if debug mode is on
            if (debugMode)
            {
                DrawDebugInfo(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the needing Debuging info to the game screen
        /// </summary>
        /// <param name="sb">The sprite batch needed to draw</param>
        private void DrawDebugInfo(SpriteBatch sb)
        {
            //Draws the Mouses's X and Y position
            sb.DrawString(
                arial12,
                $"Mouse X: {mouse.X}, Mouse Y:{mouse.Y}",
                new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 24),
                Color.Black);

            //Draws the current state of the game
            sb.DrawString(
                arial12,
                $"Game's State: {gameState}",
                new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 48),
                Color.Black);

            //Draws the current state of the player
            sb.DrawString(
                arial12,
                $"Player's State: {player.PlayerState}",
                new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 72),
                Color.Black);

            //Draws the current number of bullets
            sb.DrawString(
                arial12,
                $"Bullet Count: {bullets.Count}",
                new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 98),
                Color.Black);
        }
    }
}
