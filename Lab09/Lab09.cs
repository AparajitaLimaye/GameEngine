using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using CPI311.GameEngine;
//using System.Windows.Forms;
//using System.Drawing;
using System;
using Lab02;

namespace Lab09
{
    public class Lab09 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model cube;
        Model sphere;
        AStarSearch search;
        List<Vector3> path;

        Camera camera;
        Transform cameraTransform;

        Random random = new Random();
        private int size = 10;

        public Lab09()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            InputManager.Initialize();
            Time.Initialize();
            ScreenManager.Initialize(_graphics);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = (Vector3.Up * 10) + (Vector3.Right * 5);
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            camera.Position = new Vector2(0.5f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            search = new AStarSearch(size, size); // size of grid
            foreach (AStarNode node in search.Nodes)
                if (random.NextDouble() < 0.2)
                    search.Nodes[random.Next(size), random.Next(size)].Passable = false;
            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[size - 1, size - 1];
            search.End.Passable = true;

            search.Search(); // A search is made here.
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

            cube = Content.Load<Model>("cube");
            sphere = Content.Load<Model>("Sphere");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            Time.Update(gameTime);

            if(InputManager.IsKeyPressed(Keys.Space))
            {
                int colsS = new Random().Next(size);
                int rowsS = new Random().Next(size);
                int colsE = new Random().Next(size);
                int rowsE = new Random().Next(size);
                search.Start = search.Nodes[colsS, rowsS]; // assign a random start node (passable)
                search.End = search.Nodes[colsE, rowsE]; // assign a random end node (passable)
                search.Search();
                path.Clear();
                AStarNode current = search.End;
                while (current != null)
                {
                    path.Insert(0, current.Position);
                    current = current.Parent;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (AStarNode node in search.Nodes)
                if (!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) *
                    Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);
            foreach (Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) * 
                    Matrix.CreateTranslation(position), camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}