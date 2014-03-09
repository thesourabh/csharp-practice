using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XnaCards;

namespace ProgrammingAssignment6
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // keep track of game state and current winner
        static GameState gameState = GameState.Play;
        static Player currentWinner = Player.None;

        // hands and battle piles for the players
        WarHand playerOneWarHand, playerTwoWarHand;
        WarBattlePile playerOneBattlePile, playerTwoBattlePile;

        // winner messages for players
        WinnerMessage playerOneWinnerMessage, playerTwoWinnerMessage;

        // menu buttons
        MenuButton flipButton, collectWinningsButton, quitButton;

        // window height and width constants
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // make mouse visible and set resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // create the deck object and shuffle
            Deck deck = new Deck(Content, 1, 1);
            deck.Shuffle();

            // create the player hands and fully deal the deck into the hands
            playerOneWarHand = new WarHand(WINDOW_WIDTH / 2, 100);
            playerTwoWarHand = new WarHand(WINDOW_WIDTH / 2, 500);
            while (!deck.Empty)
            {
                playerOneWarHand.AddCard(deck.TakeTopCard());
                playerTwoWarHand.AddCard(deck.TakeTopCard());
            }

            // create the player battle piles
            playerOneBattlePile = new WarBattlePile(WINDOW_WIDTH / 2, 200);
            playerTwoBattlePile = new WarBattlePile(WINDOW_WIDTH / 2, 400);

            // create the player winner messages
            playerOneWinnerMessage = new WinnerMessage(Content, 600, 100);
            playerTwoWinnerMessage = new WinnerMessage(Content, 600, 500);

            // create the menu buttons
            flipButton = new MenuButton(Content, "flipbutton", 200, 150, GameState.Flip);
            quitButton = new MenuButton(Content, "quitbutton", 200, 450, GameState.Quit);
            collectWinningsButton = new MenuButton(Content, "collectwinningsbutton", 200, 300, GameState.CollectWinnings);
            collectWinningsButton.Visible = false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();

            // update the menu buttons
            flipButton.Update(mouse);
            collectWinningsButton.Update(mouse);
            quitButton.Update(mouse);


            // update based on game state
            if (gameState == GameState.Flip)
            {
                if (!playerOneWarHand.Empty)
                {
                    playerOneBattlePile.AddCard(playerOneWarHand.TakeTopCard());
                    playerOneBattlePile.GetTopCard().FlipOver();
                    playerTwoBattlePile.AddCard(playerTwoWarHand.TakeTopCard());
                    playerTwoBattlePile.GetTopCard().FlipOver();
                    if (playerOneBattlePile.GetTopCard().WarValue > playerTwoBattlePile.GetTopCard().WarValue)
                    {
                        currentWinner = Player.Player1;
                        playerOneWinnerMessage.Visible = true;
                    }
                    else if (playerOneBattlePile.GetTopCard().WarValue < playerTwoBattlePile.GetTopCard().WarValue)
                    {
                        currentWinner = Player.Player2;
                        playerTwoWinnerMessage.Visible = true;
                    }
                    else
                    {
                        currentWinner = Player.None;
                    }
                }
                flipButton.Visible = false;
                collectWinningsButton.Visible = true;
                gameState = GameState.Play;

            }
            else if (gameState == GameState.CollectWinnings)
            {

                // Three cases: Player 1 wins, Player 2 wins, or both are equal
                if (currentWinner == Player.Player1)
                {
                    playerOneWarHand.AddCards(playerOneBattlePile);
                    playerOneWarHand.AddCards(playerTwoBattlePile);
                }
                else if (currentWinner == Player.Player2)
                {
                    playerTwoWarHand.AddCards(playerOneBattlePile);
                    playerTwoWarHand.AddCards(playerTwoBattlePile);
                }
                else
                {
                    playerOneWarHand.AddCards(playerOneBattlePile);
                    playerTwoWarHand.AddCards(playerTwoBattlePile);
                }
                currentWinner = Player.None;
                playerOneWinnerMessage.Visible = false;
                playerTwoWinnerMessage.Visible = false;
                flipButton.Visible = true;
                collectWinningsButton.Visible = false;
                if (playerOneWarHand.Empty)
                {
                    gameState = GameState.GameOver;
                    currentWinner = Player.Player2;
                }
                else if (playerTwoWarHand.Empty)
                {
                    gameState = GameState.GameOver;
                    currentWinner = Player.Player1;
                }
            }
            else if (gameState == GameState.GameOver)
            {
                flipButton.Visible = false;
                collectWinningsButton.Visible = false;
                if (currentWinner == Player.Player1)
                    playerOneWinnerMessage.Visible = true;
                else if (currentWinner == Player.Player2)
                    playerTwoWinnerMessage.Visible = true;
                gameState = GameState.Play;
            }
            else if (gameState == GameState.Quit)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Goldenrod);

            spriteBatch.Begin();

            // draw the game objects
            playerOneWarHand.Draw(spriteBatch);
            playerTwoWarHand.Draw(spriteBatch);
            playerOneBattlePile.Draw(spriteBatch);
            playerTwoBattlePile.Draw(spriteBatch);

            // draw the winner messages
            playerOneWinnerMessage.Draw(spriteBatch);
            playerTwoWinnerMessage.Draw(spriteBatch);

            // draw the menu buttons
            flipButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            collectWinningsButton.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the state of the game
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            gameState = newState;
        }
    }
}
