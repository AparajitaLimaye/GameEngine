﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;
using Lab02;

namespace Lab04
{
    public class Lab04 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;
        Transform modelTransform;
        Transform cameraTransform;
        Camera camera;

        //*** Lab4-C *************
        Model parent;
        Transform parentTransform;
        //************************

        public Lab04()
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
            model = Content.Load<Model>("Torus");
            modelTransform = new Transform();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5 + Vector3.Up;
            camera = new Camera();
            camera.Transform = cameraTransform; // update camera Transform

            //*** LAB4_C *************************
            parent = Content.Load<Model>("Sphere");
            parentTransform = new Transform();
            parentTransform.LocalPosition = Vector3.Right * 5;
            //*** Question how to make the Torus as a Child of Sphere
            //Parenting (model and parent) here!
            modelTransform.Parent = parentTransform;
            modelTransform.LocalPosition = parentTransform.LocalPosition + Vector3.One;
            //*************************************

            //** Lighting Effect *************************
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in parent.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            //**********************************************

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);

            //*** LAB4-C 
            if (InputManager.IsKeyDown(Keys.Up))
                parentTransform.LocalPosition += parentTransform.Up * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Down))
                parentTransform.LocalPosition += parentTransform.Down * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Right))
                parentTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Left))
                parentTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            //******************************************************************************

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            model.Draw(modelTransform.World, camera.View, camera.Projection);
            parent.Draw(parentTransform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}