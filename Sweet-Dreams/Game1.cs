using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        // Basic fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // State fields
        private GameState gameState;
        private PlayerState playerState;
        private KeyboardState currentKbState;
        private KeyboardState previousKbState;
        private MouseState mouse;

        // Interface Textures
        private Texture2D background;
        private Texture2D title;
        private Texture2D startButton;
        private Texture2D instructionButton;
        private Texture2D quitButton;
        private Texture2D credits;
        private Texture2D winningScreen;
        private Texture2D gameOverScreen;
        private Texture2D lifeHeart;

        // Game textures
        private Texture2D playerAnimation;
        private Texture2D purpleDungeon;
        private Texture2D candySprites;
        private Texture2D enemySprites;
        private Texture2D tempButton;

        // Object components
        private Random rng;
        private List<Candy> collectibles;
        private List<Bullet> bullets;
        private EnemyManager enemyManager;
        private Button start;
        private Button instruction;
        private Button quit;
        private Level level1;
        private OrthographicCamera camera;
        private Player player;

        // Additional fields used for the game
        private int screenWidth;
        private int screenHeight;
        private SpriteFont arial12;
        private double deathTimer;
        private bool credit;
        float rotation;
        float startingX;
        float startingY;

        /// <summary>
        /// Whether or not the game is currently in god/debug mode.
        /// </summary>
        public static bool GodMode { get; private set; }

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
            GodMode = false;

            // Initialize Camera
            camera = new OrthographicCamera(_graphics.GraphicsDevice.Viewport);

            deathTimer = 0.98;
            rotation = 0;
            startingX = 385;
            startingY = 213;
            mouse = Mouse.GetState();

            // Credit is not drawn from the start
            credit = false;

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
            enemySprites = Content.Load<Texture2D>("DemonSprites");

            background = Content.Load<Texture2D>("StartScreenbg");
            startButton = Content.Load<Texture2D>("StartButton");
            instructionButton = Content.Load<Texture2D>("InstructionsButton");
            quitButton = Content.Load<Texture2D>("QuitButton");
            title = Content.Load<Texture2D>("GameTitle");
            credits = Content.Load<Texture2D>("creditScene");
            winningScreen = Content.Load<Texture2D>("WinningScreen");
            gameOverScreen = Content.Load<Texture2D>("SweetDreamsGameOver");
            lifeHeart = Content.Load<Texture2D>("heart");

            // Initialize Buttons here after loading the assets
            start = new Button(startButton,
                               new Rectangle(205, 115, 300, 150),
                               new Rectangle(263, 179, 411, 210));

            instruction = new Button(instructionButton,
                                     new Rectangle(5, 5, 56, 50),
                                     new Rectangle(29, 10, 97, 91));

            quit = new Button(quitButton,
                              new Rectangle(screenWidth - 152, screenHeight - 120, 150, 120),
                              new Rectangle(881, 544, 194, 169));

            // Load the Level
            level1 = new Level(purpleDungeon, "../../../Content/purpleDungeonTextureMapping.txt", _spriteBatch);
            level1.LoadLevel("../../../Content/Level1.txt");

            // Creates the player at its starting position
            player = new Player(playerAnimation, 
                new Rectangle(screenWidth / 2 - 15, screenHeight / 2 - 27, 51, 54),
                screenWidth, 
                screenHeight);

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
                    // TO DO: Maybe since we have various buttons that does different stuff,
                    //        it might be useful to make a manager for the UIs.

                    // Updates the buttons:
                    // Checks hovering and clicking
                    start.Update(mouse);
                    if (start.buttonPressed(mouse) == true)
                    {
                        gameState = GameState.Game;
                    }

                    // Checks hovering
                    instruction.Update(mouse);

                    // Checks hovering and clicking
                    quit.Update(mouse);
                    if(quit.buttonPressed(mouse) == true)
                    {
                        Exit();
                    }

                    // The credits scene toggle on and off using the 'C' key.
                    if(SingleKeyPress(Keys.C))
                    {
                        credit = !credit;
                    }
                    break;

                case GameState.Game:

                    // Victory when all enemies are gone and the player is at the door
                    if (enemyManager.IsLevelCleared() && 
                        player.WorldPosition.Contains(new Point(470, 30)))
                    {
                        gameState = GameState.Win;
                    }

                    // when the player dies???
                    if (player.Health <= 0)
                    {
                        if (deathTimer <= 0)
                        {
                            gameState = GameState.Lose;
                            deathTimer = 0.98;
                        }
                        deathTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    // ADD WHEN GAME DOOR IS ADDED!!! if player reaches the door when enemy list isnt empty player dies :)

                    // Turns on or off god/debug mode if G is pressed
                    if (SingleKeyPress(Keys.G))
                    {
                        GodMode = !GodMode;
                    }

                    // Checks for player-candy collisions
                    for (int i = 0; i < collectibles.Count; i++)
                    {
                        if (player.CollidesWith(collectibles[i]))
                        {
                            // If there is a collision, that candy gets collected then deleted
                            player.CollectCandy(collectibles[i].CType);
                            collectibles.RemoveAt(i);
                            i--;
                        }
                    }

                    // Updates the player
                    player.Update(gameTime);

                    // Checks for a left click and bullet timer or god mode to shoot
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        player.ReloadTimer <= 0)
                    {
                        // If the player is left of the center...
                        if (player.WorldPosition.X < 385)
                        {
                            // Match the bullets startingX to the player's X
                            startingX = player.WorldPosition.X;
                        }
                        // If the player is right of the center...
                        else if (player.WorldPosition.X > level1.WorldWidth - 385)
                        {
                            // Match the bullets startingX to the center
                            // plus how far right of the center the player is 
                            startingX = Math.Abs(385 + (player.WorldPosition.X - level1.WorldWidth + 385));
                        }

                        // If the player is above the center...
                        else if (player.WorldPosition.Y < 213)
                        {
                            // Match the bullets startingY to the player's Y
                            startingY = player.WorldPosition.Y;
                        }
                        // If the player is below the center...
                        else if (player.WorldPosition.Y > level1.WorldWidth - 213)
                        {
                            // Match the bullets startingY to the center
                            // plus how far below the center the player is 
                            startingY = Math.Abs(213 + (player.WorldPosition.Y - level1.WorldWidth + 213));
                        }
                        else
                        {
                            startingX = 385;
                            startingY = 213;
                        }

                        // Finding the rotation of the bullet based off the mouse
                        rotation = (float)Math.Atan2(startingY - mouse.Y,
                                                     startingX - mouse.X);

                        // Makes a new bullet every time you shoot
                        bullets.Add(new Bullet(candySprites, 
                            new Rectangle(player.WorldPosition.X, player.WorldPosition.Y, 16, 16),
                            new Rectangle(0, 0, 16, 16),
                            player.Damage, screenWidth, screenHeight, level1.WorldWidth, level1.WorldHeight, rotation));

                        // Resets the timer for reloading the gun 
                        player.ReloadTimer = 1;
                    }

                    // Has a 1 second timer between shooting a bullet
                    player.ReloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // Updates all bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime);

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
                        enemyManager.UpdateAll(gameTime);
                    }

                    // If the player is dead the game state changes to lose
                    if (playerState == PlayerState.Dead)
                    {
                        gameState = GameState.Lose;
                    }

                    // Update ALL the camera related stuff
                    camera.Update(player.WorldPosition, 
                                  level1.WorldWidth, 
                                  level1.WorldHeight);

                    // Keep player in bounds
                    player.KeepPlayerInBounds(level1.WorldWidth, level1.WorldHeight);

                    break;

                case GameState.Win:

                    // draw win screen to console
                    // assuming were also going to need the button class?
                    // Draw win screen to console

                    if (SingleKeyPress(Keys.Enter))
                    {
                        NewGame();
                        gameState = GameState.Menu;
                    }

                    break;

                case GameState.Lose:

                    // Draw game over to console
                    // Press enter to continue back to home screen 

                    if (SingleKeyPress(Keys.Enter))
                    {
                        NewGame();
                        gameState = GameState.Menu;
                    }

                    break;
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draws everything whose position needs to be translated
            _spriteBatch.Begin(transformMatrix: camera.CameraMatrix);
            if (gameState == GameState.Game)
            {
                // Draws the level itself
                level1.DisplayTiles(_spriteBatch, screenWidth, screenHeight);

                // Draws all candy
                for (int i = 0; i < collectibles.Count; i++)
                {
                    if (collectibles[i].IsOnScreen(camera))
                    {
                    collectibles[i].Draw(_spriteBatch);
                    }
                }

                // Draws all bullets
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (bullets[i].IsOnScreen(camera))
                    {
                        bullets[i].Draw(_spriteBatch);
                    }
                }                

                // Draws all enemies that are on screen
                enemyManager.DrawAll(_spriteBatch, camera);

                // Draws the player
                player.Draw(_spriteBatch);            
            }
            _spriteBatch.End();

            // Draws everything that should be stationary on the screen
            _spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Menu:
                    _spriteBatch.Draw(background,
                                      new Rectangle(0, 0, screenWidth, screenHeight),
                                      Color.White);

                    _spriteBatch.Draw(title,
                                      new Rectangle(85, 20, screenWidth - 150, 90),
                                      new Rectangle(205, 63, 705, 92),
                                      Color.White);

                    // Draws the buttons
                    start.Draw(_spriteBatch);
                    instruction.Draw(_spriteBatch);
                    quit.Draw(_spriteBatch);

                    // Draws placeholder instructions
                    // : Maybe this should be toggled on and off by mouse hovering over.
                    // : Nice chance to play with events and delegates.
                    if (instruction.IsHovered == true)
                    {
                        DebugLib.DrawRectFill(
                            _spriteBatch,
                            new Rectangle(20, screenHeight - 85, 535, 80),
                            Color.Black);

                        _spriteBatch.DrawString(
                        arial12,
                        "Instructions: Shoot all enemies by clicking the mouse where you want to aim.\n" +
                        "Don't get hit by them! Pick up the candy that they drop to gain power-ups.\n" +
                        "Toggle 'god mode' (and debug information) with the G key.\n" +
                        "Press C Key for CREDITS.",
                        new Vector2(30, screenHeight - 80),
                        Color.White);

                    }

                    // Draws the credit scene if it is toggled on by the 'C' key
                    if(credit)
                    {
                        DebugLib.DrawRectFill(_spriteBatch, 
                                              new Rectangle(0, 0, screenWidth, screenHeight), 
                                              Color.CornflowerBlue);

                        _spriteBatch.Draw(credits,
                                          new Rectangle(30, 15, (int)(screenWidth * 0.9), (int)(screenHeight * 0.9)),
                                          Color.White);
                    }
                    break;

                case GameState.Game:

                    // Draws a heart for each of the player's remaining lives
                    Rectangle heartRect = new Rectangle(-10, -10, 80, 80);
                    for (int i = 0; i < player.Health; i++)
                    {
                        _spriteBatch.Draw(
                            lifeHeart,
                            heartRect,
                            Color.White);

                        heartRect.X += 50;
                    }

                    // Draws the Debug Information if debug mode is on
                    if (GodMode)
                    {
                        _spriteBatch.DrawString(
                            arial12,
                            "God mode enabled. Enemies will not damage you in this state.",
                            new Vector2(350, screenHeight - 24),
                            Color.White);

                        DrawDebugInfo(_spriteBatch);
                    }

                    break;

                case GameState.Win:

                    _spriteBatch.Draw(
                        winningScreen,
                        new Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);

                    _spriteBatch.DrawString(
                        arial12,
                        "Press ENTER to return to the menu screen.",
                        new Vector2(255, 20),
                        Color.White);

                    break;

                case GameState.Lose:

                    _spriteBatch.Draw(
                        gameOverScreen,
                        new Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);

                    _spriteBatch.DrawString(
                        arial12,
                        "Press ENTER to return to the menu screen.",
                        new Vector2(250, screenHeight - 50),
                        Color.LightGray);

                    break;
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

            //Draws one enemy's world position
            /* if (enemyManager.WorldPositions.Count > 0)
            {
                sb.DrawString(
                    arial12,
                    $"One enemy's position: ({enemyManager.WorldPositions[0].X}, " +
                    $"{enemyManager.WorldPositions[0].Y})",
                    new Vector2(460, screenHeight - 24),
                    Color.White);
            } */

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

            //Draws player's remaining health
            sb.DrawString(
                arial12,
                $"Remaining Health: {player.Health}",
                new Vector2(10,screenHeight - 124),
                Color.White);

            //Draws the player's world position
            sb.DrawString(
                arial12,
                $"Player World Position: {player.WorldPosition.X}, {player.WorldPosition.Y}",
                new Vector2(10, screenHeight - 150),
                Color.White);

            // Screen dimensions
            sb.DrawString(
                arial12,
                $"Screen Size: {screenWidth} x {screenHeight}",
                new Vector2(10, screenHeight - 176),
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

        /// <summary>
        /// Reinitializes game objects to restart the level.
        /// </summary>
        private void NewGame()
        {
            // Lists to hold all candies and bullets currently in the world
            bullets = new List<Bullet>();
            collectibles = new List<Candy>();

            // God mode starts turned off
            GodMode = false;

            // Creates the player at its starting world and screen positions
            player = new Player(playerAnimation,
                new Rectangle(screenWidth / 2 - 15, screenHeight / 2 - 27, 30, 51),
                screenWidth,
                screenHeight);

            // Loads in level 1 enemy data
            enemyManager = new EnemyManager(rng, "../../../Content/Enemy Data.txt", collectibles, bullets,
                player, enemySprites, candySprites, screenWidth, screenHeight,
                level1.WorldWidth, level1.WorldHeight);
        }
    }
}
