using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using Lab02;
using System;
using SharpDX.WIC;
using System.IO;
using System.Collections.Generic;

namespace Assignment5
{
    public class Assignment5 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Camera camera;
        Random random = new Random();
        const int Size = 20;

        //Developing Maze Terrain : Lab 10
        TerrainRenderer terrain;
        Effect effect;

        //Create grid and search algorithm: Lab 9
        AStarSearch search;
        List<Vector3> path;
        Model cube;
        Model sphere;

        public Assignment5()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            camera = new Camera();
            camera.Transform = new Transform();
            //Top Down View
            camera.Transform.LocalPosition = Vector3.Backward * 15 + Vector3.Right * 3 + Vector3.Up * 55;
            camera.Transform.Rotate(Vector3.Left, 20.3f);

            //First Person View
            //camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right*3 + Vector3.Up * 5;

            //*** Grid
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
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //*** Terrain Maze
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("mazeH"),
                Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1,5,1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
            effect.Parameters["Shininess"].SetValue(20f);

            cube = Content.Load<Model>("cube");
            sphere = Content.Load<Model>("Sphere");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(camera.Transform.Position + Vector3.Up * 10);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
            }

            foreach(AStarNode node in search.Nodes)
                if(!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) *
                        Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);
            foreach(Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) *
                    Matrix.CreateTranslation(position), camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}