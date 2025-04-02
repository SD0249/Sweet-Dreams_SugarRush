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
        private Vector2 worldToScreen;
        private Texture2D playerAnimation;
        private Texture2D purpleDungeon;
        private Texture2D candySprites;
        private SpriteFont arial12;
        private PlayerState currentPlayerState;
        private KeyboardState currentKbState;
        private KeyboardState previousKbState;
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
            // Screen dimensions
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;

            // Lists to hold all candies and bullets currently in the world
            bullets = new List<Bullet>();
            collectibles = new List<Candy>();

            // Debug mode is on for testing
            debugMode = true;

            mouse = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads assets
            playerAnimation = Content.Load<Texture2D>("PlayerAnimation");
            purpleDungeon = Content.Load<Texture2D>("Full");
            arial12 = Content.Load<SpriteFont>("arial12");
            candySprites = Content.Load<Texture2D>("acursedpixel_16x16_candyicons");

            // Load the Level
            level1 = new Level(purpleDungeon, "../../../Content/purpleDungeonTextureMapping.txt", _spriteBatch);
            level1.LoadLevel("../../../Content/Level1.txt");

            // Creates the player at its starting world and screen positions
            player = new Player(playerAnimation, 
                new Rectangle(screenWidth / 2 - 15, screenHeight / 2 - 27, 30, 54),
                new Rectangle(screenWidth / 2 - 15, screenHeight / 2 - 27, 30, 54),
                screenWidth, 
                screenHeight);

            // Determines world to screen offset vector
            worldToScreen = new Vector2(player.ScreenPosition.X - player.WorldPosition.X,
                      player.ScreenPosition.Y - player.WorldPosition.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouse = Mouse.GetState();

            // Updates current and previous keyboard states
            previousKbState = currentKbState;
            currentKbState = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Menu:

                    // Draw menu to console

                    if (currentKbState.IsKeyDown(Keys.Enter))
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

                    // Updates the player
                    player.Update(gameTime, worldToScreen);
                    //player.UpdateAnimation(gameTime);

                    // Updates world to screen offset vector
                    worldToScreen = new Vector2(player.ScreenPosition.X - player.WorldPosition.X,
                      player.ScreenPosition.Y - player.WorldPosition.Y);

                    // Checks for a left click and bullet timer to shoot
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        player.ReloadTimer <= 0)
                    {
                        // Makes a new bullet every time you shoot
                        bullets.Add(new Bullet(candySprites, player.WorldPosition, player.ScreenPosition, screenWidth, screenHeight));

                        // Resets the timer for reloading the gun 
                        player.ReloadTimer = 1;
                    }
                    // Has a 1 second timer between shooting a bullet
                    player.ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // Updates all bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime, worldToScreen);
                    }
                    
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Porobably get rid of this
            /*
            // If in Game mode, the following is drawn translated with respect to
            // the player's world position
            if (gameState == GameState.Game)
            {
            // Draws everything whose position needs to be translated
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateTranslation(-player.Position.X, -player.Position.Y, 0));

            _spriteBatch.End()
            }
            */
            
            // Draws everything that should be stationary on the screen
            _spriteBatch.Begin();
            
            switch (gameState)
            {
                case GameState.Menu:

                    _spriteBatch.DrawString(
                        arial12,
                        "         Sweet Dreams\n Press enter to start game",
                        new Vector2(300, 200),
                        Color.White);

                    break;

                case GameState.Game:

                    // Draws the level itself
                    level1.DisplayTiles(_spriteBatch, worldToScreen, screenWidth, screenHeight);

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

                    // Draws all bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        if (bullets[i].IsOnScreen)
                        {
                            bullets[i].Draw(_spriteBatch);
                        }
                    }

                    // Draws the player
                    player.Draw(_spriteBatch);
                    
                    DebugLib.DrawRectOutline(_spriteBatch,
                        player.WorldPosition,
                        2,
                        Color.Black);

                    break;

                case GameState.Win:

                    // GraphicsDevice.Clear(Color.Honeydew);

                    _spriteBatch.DrawString(
                        arial12,
                        "#YouWon",
                        new Vector2(300, 200),
                        Color.White);

                    break;

                case GameState.Lose:

                    _spriteBatch.DrawString(
                        arial12,
                        "#YouGetNoSweetDreams",
                        new Vector2(300, 200),
                        Color.White);

                    break;
            }

            // Draws the Debug Information if debug mode is on
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
                new Vector2(10, screenHeight - 24),
                Color.White);

            //Draws the current state of the game
            sb.DrawString(
                arial12,
                $"Game's State: {gameState}",
                new Vector2(10, screenHeight - 48),
                Color.White);

            //Draws the current state of the player
            sb.DrawString(
                arial12,
                $"Player's State: {player.PlayerState}",
                new Vector2(10, screenHeight - 72),
                Color.White);

            //Draws the current number of bullets
            sb.DrawString(
                    arial12,
                    $"Bullet Count: {bullets.Count}",
                    new Vector2(10, screenHeight - 98),
                    Color.White);

            //Draws the player's screen position
            sb.DrawString(
                arial12,
                $"Player Screen Position: {player.ScreenPosition.X}, {player.ScreenPosition.Y}",
                new Vector2(10,screenHeight - 124),
                Color.White);

            //Draws the player's world position
            sb.DrawString(
                arial12,
                $"Player World Position: {player.WorldPosition.X}, {player.WorldPosition.Y}",
                new Vector2(10, screenHeight - 150),
                Color.White);

            //Draws worldToScreen vector components
            sb.DrawString(
                arial12,
                $"World-to-Screen Offset: {worldToScreen.X}, {worldToScreen.Y}",
                new Vector2(10, screenHeight - 176),
                Color.White);
        }
    }
}
