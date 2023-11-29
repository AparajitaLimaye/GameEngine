using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using Lab02;
using Color = Microsoft.Xna.Framework.Color;

namespace Assignment5
{
    public class Assignment5 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Camera camera;
        Light light;
        SpriteFont font;

        //Developing Maze Terrain : Lab 10
        TerrainRenderer terrain;
        Effect effect;

        //Player stuff
        Player player;
        Agent agent;

        public Assignment5()
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

            //**** Camera
            /*camera = new Camera();
            camera.Transform = new Transform();
            //Top Down View
            camera.Transform.LocalPosition = Vector3.Backward * 15 + Vector3.Right * 3 + Vector3.Up * 55;
            camera.Transform.Rotate(Vector3.Left, 20.3f);
            */
            //First Person View
            //camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right*3 + Vector3.Up * 5;

           /* //*** Grid
            search = new AStarSearch(Size, Size); //size of grid
            foreach (AStarNode node in search.Nodes)
                if (random.NextDouble() < 0.2)
                    search.Nodes[random.Next(Size), random.Next(Size)].Passable = false;
            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[Size - 1, Size - 1];
            search.End.Passable = true;

            search.Search(); // a search is made here
            path = new List<Vector3>();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }*/

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");

            //*** Terrain Maze
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("mazeH2"),
                Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1,5,1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f)); //all 0.2
            effect.Parameters["Shininess"].SetValue(20f);

            //**** Camera
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2-0.2f);

            //* light
            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            //** Player
            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            //player.Transform.LocalPosition = new Vector3(3, 10, 15);
            agent = new Agent(terrain, Content, camera, GraphicsDevice, light);

        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            Time.Update(gameTime);
            InputManager.Update();
            player.Update();
            agent.Update();

            if (InputManager.IsKeyDown(Keys.Up)) camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Down)) camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);
            //Console.WriteLine("Player position: " + player.Transform.LocalPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
            
            //Terrain stuff
            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                player.Draw();
                agent.Draw();
            }

            /*foreach(AStarNode node in search.Nodes)
                if(!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) *
                        Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);
            foreach(Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) *
                    Matrix.CreateTranslation(position), camera.View, camera.Projection);*/

            /*//Player stuff
            player.Draw();*/

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Player position: " + player.Transform.LocalPosition, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Terrain position: " + terrain.Transform.LocalPosition, new Vector2(0, 15), Color.Black);
            _spriteBatch.DrawString(font, "Camera position: " + camera.Transform.LocalPosition, new Vector2(0, 30), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}