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
        private Texture2D darkDungeon;
        private SpriteFont arial12;
        private PlayerState currentPlayerState;
        private List<Enemy> currentEnemyList =  new List<Enemy>();


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

            mouse = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerAnimation = Content.Load<Texture2D>("PlayerAnimation");
            purpleDungeon = Content.Load<Texture2D>("Full");
            darkDungeon = Content.Load<Texture2D>("mainlevbuild");
            arial12 = Content.Load<SpriteFont>("arial12");

            player = new Player(playerAnimation, 
                new Rectangle(screenWidth/2 - 15, screenHeight/2 - 27, 30, 54), 
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

                    // draw menu to console

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Game;
                    }

                    // if button is pressed?
                    // need button class

                    break;

                case GameState.Game:

                    // draw game to console

                    // #ToBeDetermened
                    // READ THIS!!!!
                    // ememy manager is currentluy NULL!!! this code will not run untill the file
                    // for enemy manager is made and added to the game!!!
                    /*if (enemyManager.CheckEnemys(currentEnemyList) == false && door is reached)
                    {
                        gameState = GameState.Win;
                    }*/

                    // if the player is dead the game state changes to lose
                    if (currentPlayerState == PlayerState.Dead)
                    {
                        gameState = GameState.Lose;
                    }

                    break;

                case GameState.Win:

                    // draw win screen to console

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }

                    break;

                case GameState.Lose:

                    // draw game over to console
                    // press enter to continue back to home screen 

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }

                    break;
            }
            
            //Update Methods for the player
            player.Update(gameTime);
            player.UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
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

                    GraphicsDevice.Clear(Color.Honeydew);

                    //Draws the player
                    player.Draw(_spriteBatch);

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

            //Draws the Debug Information
            DebugInfo(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the needing Debuging info to the game screen
        /// </summary>
        /// <param name="sb">The sprite batch needed to draw</param>
        private void DebugInfo(SpriteBatch sb)
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
                $"Game's State: {player.PlayerState}",
                new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 74),
                Color.Black);
        }
    }
}
