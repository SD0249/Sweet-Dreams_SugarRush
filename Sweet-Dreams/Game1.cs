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
        private Texture2D enemySprites;
        private SpriteFont arial12;
        private PlayerState currentPlayerState;
        private Button myButton;
        private Texture2D tempButton;
        private KeyboardState currentKbState;
        private KeyboardState previousKbState;
        private bool doorIsReached;
        private Level level1;
        private OrthographicCamera camera;

        // Whether or not the game is currently in god/debug mode
        public static bool godMode;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Random
            rng = new Random();
            
            // Screen dimensions
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;

            // Lists to hold all candies and bullets currently in the world
            bullets = new List<Bullet>();
            collectibles = new List<Candy>();

            // Debug mode is on for testing
            godMode = false;

            // Initialize Camera
            camera = new OrthographicCamera(_graphics.GraphicsDevice.Viewport);

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
            tempButton = Content.Load<Texture2D>("ShitButton");

            myButton = new Button(tempButton,
                new Rectangle(340, 250, 100, 50));

            candySprites = Content.Load<Texture2D>("acursedpixel_16x16_candyicons");
            enemySprites = Content.Load<Texture2D>("DemonSprites");

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

            // Loads in level 1 enemy data
            enemyManager = new EnemyManager(rng, "../../../Content/Enemy Data.txt", collectibles, bullets, 
                player, enemySprites, candySprites, screenWidth, screenHeight, 
                level1.WorldWidth, level1.WorldHeight);
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

                    // Updates the play button
                    myButton.Update(mouse);
                    if (myButton.buttonPressed(mouse) == true)
                    {
                        gameState = GameState.Game;
                    }

                    // Makes sure that the player doesn't shoot
                    // when the play button is pressed
                    player.ReloadTimer = 0.3;

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

                    // ADD WHEN GAME DOOR IS ADDED!!! if player reaches the door when enemy list isnt empty player dies :)

                    // Turns on or off god/debug mode if G is pressed
                    if (SingleKeyPress(Keys.G))
                    {
                        godMode = !godMode;
                    }

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
                        bullets.Add(new Bullet(candySprites, player.WorldPosition, player.ScreenPosition, 
                            player.Damage, screenWidth, screenHeight));

                        // Resets the timer for reloading the gun 
                        player.ReloadTimer = 1;
                    }
                    // Has a 1 second timer between shooting a bullet
                    player.ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // Updates all bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime, worldToScreen);

                        // Removes the bullet from the list if it is out of bounds
                        if (bullets[i].OutOfBounds(level1.WorldWidth, level1.WorldHeight))
                        {
                            bullets.RemoveAt(i);
                            i--;
                        }
                    }

                    // Updates all enemies unless the level has been cleared
                    if (!enemyManager.IsLevelCleared())
                    {
                        enemyManager.UpdateAll(gameTime, worldToScreen);
                    }
                    
                    // If the player is dead the game state changes to lose
                    if (currentPlayerState == PlayerState.Dead)
                    {
                        gameState = GameState.Lose;
                    }

                    // Update ALL the camera related stuff
                    camera.Update(worldToScreen, 
                                  player.WorldPosition, 
                                  level1.WorldWidth, 
                                  level1.WorldHeight);

                    // Keep player in bounds
                    player.KeepPlayerInBounds(level1.WorldWidth, level1.WorldHeight);

                    break;

                case GameState.Win:

                    // draw win screen to console
                    // assuming were also going to need the button class?
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
            
            // If in Game mode, the following is drawn translated with respect to
            // the player's world position
            if (gameState == GameState.Game)
            {
            // Draws everything whose position needs to be translated
            _spriteBatch.Begin(transformMatrix: camera.CameraMatrix);

                // Draws the level itself
                level1.DisplayTiles(_spriteBatch, worldToScreen, screenWidth, screenHeight);

                // Draws all candy
                for (int i = 0; i < collectibles.Count; i++)
                {
                    // TODO: Uncomment IsOnScreen once it works
                    //if (collectibles[i].IsOnScreen)
                    //{
                    collectibles[i].Draw(_spriteBatch);
                    //}
                }

                // Draws all bullets
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (bullets[i].IsOnScreen)
                    {
                        bullets[i].Draw(_spriteBatch);
                    }
                }                

                // Draws all enemies that are on screen
                enemyManager.DrawAll(_spriteBatch, camera);

                // Draws the player
                player.Draw(_spriteBatch);

            
            _spriteBatch.End();
            }
            
            // Draws everything that should be stationary on the screen
            _spriteBatch.Begin();
            
            switch (gameState)
            {
                case GameState.Menu:

                    GraphicsDevice.Clear(Color.Black);

                    // Draws the play button
                    myButton.Draw(_spriteBatch);

                    /*
                    if (myButton.Update(mouse) == true)
                    {
                        myButton.Draw(_spriteBatch);
                    }
                    else
                    {
                        myButton.Draw(_spriteBatch);
                    } */

                    _spriteBatch.DrawString(
                        arial12,
                        "         Sweet Dreams\n Press enter to start game",
                        new Vector2(300, 200),
                        Color.White);

                    // Draws placeholder instructions
                    _spriteBatch.DrawString(
                        arial12,
                        "Instructions: Shoot all enemies by clicking the mouse where you want to aim.\n" +
                        "Don't get hit by them! Pick up the candy that they drop to gain power-ups.\n" +
                        "Toggle 'god mode' (and debug information) with the G key.",
                        new Vector2(30, screenHeight - 80),
                        Color.White);

                    break;

                case GameState.Game:

                    // Draws the level itself
                    // level1.DisplayTiles(_spriteBatch, worldToScreen, screenWidth, screenHeight);

                    // TODO: Uncomment the following once fields are initialized
                    /*
                    // Draws all candies that are on screen
                    for (int i = 0; i < collectibles.Count; i++)
                    {
                        if (collectibles[i].IsOnScreen)
                        {
                            collectibles[i].Draw(_spriteBatch);
                        }
                    }*/

                    
                    

                    // Draws all bullets
                    /* for (int i = 0; i < bullets.Count; i++)
                    {
                        if (bullets[i].IsOnScreen)
                        {
                            bullets[i].Draw(_spriteBatch);
                        }
                    } */

                    // Draws the player
                    // player.Draw(_spriteBatch);
                    
                    /* DebugLib.DrawRectOutline(_spriteBatch,
                        player.WorldPosition,
                        2,
                        Color.Black); */

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
            if (godMode)
            {
                DrawDebugInfo(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws debug info to the game screen.
        /// </summary>
        /// <param name="sb">The sprite batch needed to draw.</param>
        private void DrawDebugInfo(SpriteBatch sb)
        {
            ////Draws the Mouses's X and Y position
            //sb.DrawString(
            //    arial12,
            //    $"Mouse X: {mouse.X}, Mouse Y:{mouse.Y}",
            //    new Vector2(10, screenHeight - 24),
            //    Color.White);

            //Draws the number of enemies currently in the world
            sb.DrawString(
                arial12,
                "Enemies in the World: " + enemyManager.WorldPositions.Count,
                new Vector2(10, screenHeight - 24),
                Color.White);

            ////Draws one enemy's world position
            //if (enemyManager.WorldPositions.Count > 0)
            //{
            //    sb.DrawString(
            //        arial12,
            //        $"One enemy's position: ({enemyManager.WorldPositions[0].X}, " +
            //        $"{enemyManager.WorldPositions[0].Y})",
            //        new Vector2(460, screenHeight - 24),
            //        Color.White);
            //}

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

            //Draws player's remaining health
            sb.DrawString(
                arial12,
                $"Remaining Health: {player.Health}",
                new Vector2(10, screenHeight - 202),
                Color.White);
        }

        /// <summary>
        /// Detect a single key press.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether or not the key was pressed just now.</returns>
        private bool SingleKeyPress(Keys key)
        {
            return currentKbState.IsKeyDown(key) && previousKbState.IsKeyUp(key);
        }
    }
}
