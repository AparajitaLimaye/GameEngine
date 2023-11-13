using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;
using System;
using Lab02;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Diagnostics;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ProgressBar = CPI311.GameEngine.ProgressBar;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using SharpDX.Direct3D9;
using Sprite = CPI311.GameEngine.Sprite;

namespace Assignment1
{
    public class Assignment1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont font;

        AnimatedSprite animatedSprite;
        Vector2 direction;
        int currentDirection = 0; //What direction its at

        Sprite timeBomb;
        ProgressBar distanceBar;
        ProgressBar timeRemainingBar;
        ProgressBar distanceBarBack;
        ProgressBar timeRemainingBarBack;
        Random random = new Random();

        public Assignment1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            direction = new Vector2(0, -1);

            timeBomb = new Sprite(Content.Load<Texture2D>("timeBomb")); //the square in the far left corner is this one
            timeBomb.Position = new Vector2(100, 300);

            animatedSprite = new AnimatedSprite(Content.Load<Texture2D>("explorer"));
            animatedSprite.Position = new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2);
            animatedSprite.Source = new Rectangle(0,0,32,32);

            timeRemainingBarBack = new ProgressBar(Content.Load<Texture2D>("timeRemainingBarBack"));
            timeRemainingBarBack.Position = new Vector2(200, 10);
            timeRemainingBarBack.Scale = new Vector2(3, 1);
            timeRemainingBar = new ProgressBar(Content.Load<Texture2D>("timeRemainingBar"));
            timeRemainingBar.Position = new Vector2(200, 10);
            timeRemainingBar.FillColor = Color.Red;
            timeRemainingBar.Value = 32;
            timeRemainingBar.Scale = new Vector2(3, 1);

            distanceBarBack = new ProgressBar(Content.Load<Texture2D>("distanceBarBack"));
            distanceBarBack.Position = new Vector2(450, 10);
            distanceBarBack.Scale = new Vector2(3, 1);
            distanceBar = new ProgressBar(Content.Load<Texture2D>("distanceBar"));
            distanceBar.Position = new Vector2(450, 10);
            distanceBar.FillColor = Color.Green;
            distanceBar.Value = 0;
            distanceBar.Scale = new Vector2(3, 1);
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Update();
            Time.Update(gameTime);
            animatedSprite.Update();

            //animatedSprite.Update();
            timeRemainingBar.Value -= Time.ElapsedGameTime;

            //explorer movement
            if (InputManager.IsKeyDown(Keys.Up))
            {
                animatedSprite.Position += direction * 0.5f;
                distanceBar.Value += 0.01f;
            }
            //Left Direction
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                if(currentDirection == 0) //if the character is facing North
                {
                    direction = new Vector2(-2, 0);
                    currentDirection = 1; //now its West
                    animatedSprite.Source = new Rectangle(0,64,32,32); //animation facing
                }
                else if (currentDirection == 1) //if the character is facing West
                {
                    direction = new Vector2(0, 2);
                    currentDirection = 2; //now its South
                    animatedSprite.Source = new Rectangle(0, 32, 32, 32);
                }
                else if (currentDirection == 2) //if the character is facing South
                {
                    direction = new Vector2(2, 0);
                    currentDirection = 3; //now its East
                    animatedSprite.Source = new Rectangle(0, 96, 32, 32);
                }
                else //if the character is facing East
                {
                    direction = new Vector2(0, -2); //now its North
                    currentDirection = 0;
                    animatedSprite.Source = new Rectangle(0, 0, 32, 32);
                }
            }
            //Right Direction
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                if (currentDirection == 0) //if the character is facing North
                {
                    direction = new Vector2(2, 0);
                    currentDirection = 3; //now its East
                    animatedSprite.Source = new Rectangle(0, 96, 32, 32);
                }
                else if (currentDirection == 1) //if the character is facing West
                {
                    direction = new Vector2(0, -2);
                    currentDirection = 0; //now its North
                    animatedSprite.Source = new Rectangle(0, 0, 32, 32);
                }
                else if (currentDirection == 2) //if the character is facing South
                {
                    direction = new Vector2(-2, 0);
                    currentDirection = 1; //now its West
                    animatedSprite.Source = new Rectangle(0, 64, 32, 32);
                }
                else //if the character is facing East
                {
                    direction = new Vector2(0, 2); //now its South
                    currentDirection = 2;
                    animatedSprite.Source = new Rectangle(0, 32, 32, 32);
                }
            }

            if ((animatedSprite.Position - timeBomb.Position).Length() < 30)
            {
                if(timeRemainingBar.Value + 5 < 32)
                    timeRemainingBar.Value += 5;
                else
                    timeRemainingBar.Value = 32;
                timeBomb.Position = new Vector2(random.Next(10, GraphicsDevice.Viewport.Width -10), random.Next(70, GraphicsDevice.Viewport.Height - 20));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            animatedSprite.Draw(_spriteBatch);
            //timeBomb.Draw(_spriteBatch);
            //Time Remaining
            _spriteBatch.DrawString(font, "Time Remaining: ", new Vector2(5, 10), Color.Black);
            _spriteBatch.DrawString(font, "Time Remaining: " + timeRemainingBar.Value, new Vector2(5, 50), Color.Black);
            timeRemainingBarBack.Draw(_spriteBatch);
            timeRemainingBar.Draw(_spriteBatch);
            //Distance Walking
            _spriteBatch.DrawString(font, "Distance Walked: ", new Vector2(250, 10), Color.Black);
            distanceBarBack.Draw(_spriteBatch);
            distanceBar.Draw(_spriteBatch);
            //Time Bomb
            timeBomb.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}