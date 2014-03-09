using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength, newSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        Texture2D texture, blinkingTexture, currentTexture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;
        Vector2 tileCenter;

        // blinking support
        const int TOTAL_BLINK_MILLISECONDS = 4000;
        int elapsedBlinkMilliseconds = 0;
        const int FRAME_BLINK_MILLISECONDS = 1000;
        int elapsedFrameMilliseconds = 0;

        // shrinking support
        const int TOTAL_SHRINK_MILLISECONDS = 2000;
        int elapsedShrinkMilliseconds = 0;

        // whether or not tile is visible, blinking, shrinking
        bool visible = true;
        bool isBlinking = false;
        bool isShrinking = false;

        // button status
        bool clickStarted = false;
        bool buttonReleased = true;

        // store sound bank
        SoundBank sBank;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        /// <param name="soundBank">the sound bank for playing cues</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber, SoundBank soundBank)
        {
            // set original side length field
            this.originalSideLength = sideLength;
            tileCenter = center;

            // set sound bank field
            sBank = soundBank;


            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number);
            drawRectangle = new Rectangle((int)center.X - sideLength / 2,
                 (int)center.Y - sideLength / 2, sideLength, sideLength);

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {

            // Check if tile is shrinking
            if (isBlinking)
            {
                elapsedBlinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedBlinkMilliseconds > TOTAL_BLINK_MILLISECONDS)
                    return true;
                else if (elapsedFrameMilliseconds > FRAME_BLINK_MILLISECONDS)
                {
                    if (sourceRectangle.X == 0)
                        sourceRectangle.X = currentTexture.Width / 2;
                    else
                        sourceRectangle.X = 0;
                    elapsedFrameMilliseconds = 0;
                }
            }
            else if (isShrinking)
            {

                // calculate new draw rectangle based on time elapsed since beginning of shrinking
                // else make tile invisible
                elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                newSideLength = (originalSideLength * (TOTAL_SHRINK_MILLISECONDS - elapsedShrinkMilliseconds)) / TOTAL_SHRINK_MILLISECONDS;
                if (newSideLength > 0)
                {
                    drawRectangle.Width = newSideLength;
                    drawRectangle.Height = newSideLength;
                    drawRectangle.X = (int)tileCenter.X - newSideLength / 2;
                    drawRectangle.Y = (int)tileCenter.Y - newSideLength / 2;
                }
                else
                    visible = false;
            }
            else
            {
                if (drawRectangle.Contains(mouse.X, mouse.Y))
                {
                    sourceRectangle.X = texture.Width / 2;
                    // check for click started on button
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        buttonReleased)
                    {
                        clickStarted = true;
                        buttonReleased = false;
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        buttonReleased = true;

                        // if click finished on button, change tile state
                        if (clickStarted)
                        {
                            if (isCorrectNumber)
                            {
                                isBlinking = true;
                                sBank.PlayCue("correctGuess");
                                currentTexture = blinkingTexture;
                                sourceRectangle.X = 0;
                            }
                            else
                            {
                                isShrinking = true;
                                sBank.PlayCue("incorrectGuess");
                            }
                        }
                    }
                }
                else
                {
                    sourceRectangle.X = 0;

                    clickStarted = false;
                    buttonReleased = false;
                }
            }

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the tile
            if (visible)
                spriteBatch.Draw(currentTexture, drawRectangle, sourceRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // load content for the tile and set source rectangle
            texture = contentManager.Load<Texture2D>(numberString);
            blinkingTexture = contentManager.Load<Texture2D>("blinking" + numberString);
            currentTexture = texture;
            sourceRectangle = new Rectangle(0, 0, texture.Width / 2, texture.Height);

        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}
