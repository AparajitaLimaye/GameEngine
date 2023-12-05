using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using Lab02;
using Color = Microsoft.Xna.Framework.Color;
using System.Collections.Generic;
using System;

namespace FinalGame
{
    public class FinalGame : Game
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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Camera camera;
        Light light;
        SpriteFont font;

        //Developing Maze Terrain : Lab 10
        TerrainRenderer terrain;
        Effect effect;

        //Player and Agents stuff
        Player player;
        List<Agent> agents;
        List<Bomb> assignments;
        Bomb bomb;
        Bullet bullet;

        //game
        int professorCatch = 0;
        int assignmentsKilled = 0;

        //*** LAB 11 **********************
        //variables Section D
        Dictionary<String, Scene> scenes;
        Scene currentScene;

        //Button exitButton;
        Texture2D texture;
        public Color background = Color.White;

        //Secion E
        List<GUIElement> guiElements;
        //*******************************

        public FinalGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            //graphicsDevice = new GraphicsDevice(_graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);
            scenes = new Dictionary<string, Scene>();
            guiElements = new List<GUIElement>();

            //**** Camera
            /*camera = new Camera();
            camera.Transform = new Transform();
            //Top Down View
            camera.Transform.LocalPosition = Vector3.Backward * 15 + Vector3.Right * 3 + Vector3.Up * 55;
            camera.Transform.Rotate(Vector3.Left, 20.3f);
            */
            //First Person View
            //camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right*3 + Vector3.Up * 5;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("font");

            //*** LAB 11 SCENE CHANGE ***************
            //section D
            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            scenes.Add("EndCredits", new Scene(EndCreditsUpdate, EndCreditsDraw));
            currentScene = scenes["Menu"];


            /* exitButton = new Button();
             exitButton.Texture = texture;
             exitButton.Text = "Exit";
             exitButton.Bounds = new Rectangle(50, 50, 300, 20);
             exitButton.Action += ExitGame;*/

            Button box = new Button();
            box.Texture = texture;
            //box.Texture
            box.Text = "                                                                       PLAY";
            box.Bounds = new Rectangle(100, 200, 600, 50);
            box.Action += PlayScene;

            Button fullButton = new Button();
            fullButton.Texture = texture;
            fullButton.Text = "                                                            Full Screen Mode";
            fullButton.Bounds = new Rectangle(100, 300, 600, 50);
            fullButton.Action += FullScreen;

            Button endCredits = new Button();
            endCredits.Texture = texture;
            endCredits.Text = "                                                                   Credits";
            endCredits.Bounds = new Rectangle(100, 400, 600, 50);
            endCredits.Action += EndCreditScene;

            //section E
            //guiElements.Add(exitButton);
            guiElements.Add(box);
            guiElements.Add(fullButton);
            guiElements.Add(endCredits);
            //***************************************

            //*** Terrain Maze
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("mazeH2"),
                Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f)); //all 0.2
            effect.Parameters["Shininess"].SetValue(20f);

            //**** Camera
            camera = new Camera();
            camera.Transform = new Transform();
            //First Person View
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right*3 + Vector3.Up * 4;
            //Top Down
            /*camera.Transform.LocalPosition = Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);*/

            //* light
            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            //** Player
            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            agents = new List<Agent>();
            assignments = new List<Bomb>();
            //player.Transform.LocalPosition = new Vector3(3, 10, 15);
            for (int i = 0; i < 3; i++)
            {
                Agent agent = new Agent(terrain, Content, camera, GraphicsDevice, light);
                agents.Add(agent);
            }
            //*** Bomb
            for (int i = 0; i < 3; i++)
            {
                Bomb bomb = new Bomb(terrain, Content, camera, GraphicsDevice, light, player);
                assignments.Add(bomb);
            }
            //** Bullet
            bullet = new Bullet(Content, camera, GraphicsDevice, light, player);
        }
        void ExitGame(GUIElement element)
        {
            background = (background == Color.White ? Color.Blue : Color.White);
        }

        //Section E
        void PlayScene(GUIElement element)
        {
            currentScene = scenes["Play"];
        }
        void FullScreen(GUIElement element)
        {
            ScreenManager.Setup(1920, 1080);
            //ScreenManager.IsFullScreen = !ScreenManager.IsFullScreen;  //kinda risky
            background = (background == Color.White ? Color.Blue : Color.White);
        }
        //private methods
        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
            background = Color.Blue;
        }
        void MainMenuDraw()
        {
            _spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(_spriteBatch, font);
            _spriteBatch.DrawString(font, "I'M STRESSED", new Vector2(350, 150), Color.Red);
            _spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            player.Draw();
            bullet.Draw();

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            //Terrain stuff
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                for (int i = 0; i < 3; i++)
                {
                    assignments[i].Draw();
                }
                for (int i = 0; i < 3; i++)
                {
                    agents[i].Draw();
                }
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back",
                Vector2.Zero, Color.White);
            _spriteBatch.End();
        }
        void EndCreditScene(GUIElement element)
        {
            currentScene = scenes["EndCredits"];
        }
        void EndCreditsUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
            background = Color.Blue;
        }
        void EndCreditsDraw()
        {
            _spriteBatch.Begin();
           /* foreach (GUIElement element in guiElements)
                element.Draw(_spriteBatch, font);*/
            _spriteBatch.DrawString(font, "Created by ME! Aparajita Limaye", new Vector2(300, 150), Color.Red);
            _spriteBatch.DrawString(font, "Press Esc to go back to the Menu", new Vector2(300, 200), Color.Red);
            _spriteBatch.End();
        }

        protected void restart()
        {
            player.Transform.LocalPosition = new Vector3(3, terrain.GetAltitude(player.Transform.LocalPosition), 10);
            for (int i = 0; i < 3; i++)
            {
                assignments[i].RandomPathFinding();
            }
            for (int i = 0; i < 3; i++)
            {
                agents[i].RandomPathFinding();
            }
            professorCatch = 0;
            player.Transform.LocalPosition = new Vector3(
                player.Transform.LocalPosition.X,
                terrain.GetAltitude(player.Transform.LocalPosition),
                player.Transform.LocalPosition.Z) + Vector3.Up;
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            Time.Update(gameTime);
            InputManager.Update();
            currentScene.Update();
            player.Update();
            bullet.Update();
            for (int i = 0; i < 3; i++)
            {
                assignments[i].Update();
            }

            for (int i = 0; i < 3; i++)
            {
                agents[i].Update();
            }

            if (InputManager.IsKeyDown(Keys.Up)) camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Down)) camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);
            //Console.WriteLine("Player position: " + player.Transform.LocalPosition);

            //Catching Alien
            for (int j = 0; j < 3; j++)
            {
                if (player.Collider.Collides(agents[j].Collider, out Vector3 normal))
                {
                    agents[j].RandomPathFinding();
                    professorCatch++;
                }
            }
            //Assignments catching you
            for (int j = 0; j < 3; j++)
            {
                if (player.Collider.Collides(assignments[j].Collider, out Vector3 normal1))
                {
                    restart();
                }
            }
            //Bullet kills a bomb
            for (int j = 0; j < 3; j++)
            {
                if (bullet.Collider.Collides(assignments[j].Collider, out Vector3 normal2))
                {
                    assignments[j].RandomPathFinding();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(background);

            currentScene.Draw();

            /*effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            //Terrain stuff
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                player.Draw();
                for (int i = 0; i < 3; i++)
                {
                    agents[i].Draw();
                }
            }*/

            /*foreach(AStarNode node in search.Nodes)
                if(!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) *
                        Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);
            foreach(Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) *
                    Matrix.CreateTranslation(position), camera.View, camera.Projection);*/

            _spriteBatch.Begin();
            /*_spriteBatch.DrawString(font, "Player position: " + player.Transform.LocalPosition, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Terrain position: " + terrain.Transform.LocalPosition, new Vector2(0, 15), Color.Black);
            _spriteBatch.DrawString(font, "Camera position: " + camera.Transform.LocalPosition, new Vector2(0, 30), Color.Black);
            _spriteBatch.DrawString(font, "Agents caught: " + agentCaught, new Vector2(0, 45), Color.Black);
            _spriteBatch.DrawString(font, "Time Spent: " + Time.TotalGameTime, new Vector2(0, 60), Color.Black);*/
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}