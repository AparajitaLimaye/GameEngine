using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Lab11
{
    public class Lab11 : Game
    {
        //Inner class
        public class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        //variables Section D
        Dictionary<String, Scene> scenes;
        Scene currentScene;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Button exitButton;
        Texture2D texture;
        SpriteFont font;
        public Color background = Color.White;

        //Secion E
        List<GUIElement> guiElements;

        public Lab11()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);
            scenes = new Dictionary<string, Scene>();
            guiElements = new List<GUIElement>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("Font");

            //section D
            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];

            CheckBox box = new CheckBox();
            box.Box = texture;
            box.Text = "Switch Scene";
            box.Bounds = new Rectangle(50, 50, 300, 50);
            box.Action += SwitchScene;

            Button fullButton = new Button();
            fullButton.Texture = texture;
            fullButton.Text = "Full Screen Mode";
            fullButton.Bounds = new Rectangle(50, 200, 300, 10);
            fullButton.Action += FullScreen;

            //section E
            //guiElements.Add(exitButton);
            guiElements.Add(box);
            guiElements.Add(fullButton);

        }

        void ExitGame(GUIElement element)
        {
            background = (background == Color.White ? Color.Blue : Color.White);
        }

        //Section E
        void SwitchScene(GUIElement element)
        {
            currentScene = scenes["Play"];
        }
        void FullScreen(GUIElement element)
        {
            ScreenManager.Setup(1920, 1080);
            background = (background == Color.White ? Color.Blue : Color.White);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            currentScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(background);

            //_spriteBatch.Begin();
            currentScene.Draw();
            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        //private methods
        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }
        void MainMenuDraw()
        {
            _spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(_spriteBatch, font);
            _spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back",
                Vector2.Zero, Color.Black);
            _spriteBatch.End();
        }
    }
}