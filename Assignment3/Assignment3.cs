using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Threading;  //For Multi Threading
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
//using SharpDX.Direct3D9;

namespace Assignment3
{
    public class Assignment3 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BoxCollider boxCollider;
        Light light;
        Transform lightTransform;
        GameObject GameObject;
        List<GameObject> gameObjects;
        Texture2D texture;

        Effect effect;
        bool isColorsShown = false;
        String diagnostics = "";
        bool isDialogueShown = false;
        bool isTextureShown = false;
        double frameRate = 0;

        Random random;
        //** from Lab4
        Model model;
        Camera camera;
        Transform cameraTransform;
        int numberCollisions = 0;

        //** Lab 07 *****************
        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;
        SpriteFont font;
        //***************************

        public Assignment3()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //*********
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            //** Lab 07 *********************************************
            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            //*******************************************************

            random = new Random();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10; //box size is 10


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = Content.Load<Effect>("SimpleShading");
            texture = Content.Load<Texture2D>("Square");

            model = Content.Load<Model>("Sphere");
            
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

            //** Lighting Effect *************************
            /*foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }*/
            //**********************************************

            font = Content.Load<SpriteFont>("Font");

            light = new Light();
            lightTransform = new Transform();
            lightTransform.LocalPosition = Vector3.Backward * 10 + Vector3.Right * 5;
            light.Transform = lightTransform;

            boxCollider = new BoxCollider();
            boxCollider.Size = 10; //box size is 10
            gameObjects = new List<GameObject>();
            AddGameObject();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time.Update(gameTime);
            InputManager.Update();

            frameRate = (1 / gameTime.ElapsedGameTime.TotalSeconds);

            if (InputManager.IsKeyPressed(Keys.Up)) AddGameObject();
            if (InputManager.IsKeyPressed(Keys.Down)) gameObjects.Remove(gameObjects[0]);
            if (InputManager.IsKeyPressed(Keys.LeftShift))
            {
                isDialogueShown = !isDialogueShown;
            }
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                Time.Speed = Time.Speed * 2;
            }
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                Time.Speed = Time.Speed/2;
            }
            if (InputManager.IsKeyPressed(Keys.Space))
            {
                isColorsShown = !isColorsShown;
            }
            if (InputManager.IsKeyPressed(Keys.LeftAlt))
            {
                isTextureShown = !isTextureShown;

            }
            if (InputManager.IsKeyPressed(Keys.T))
            {
                haveThreadRunning = !haveThreadRunning;

            }
            foreach (GameObject G in gameObjects) G.Update();

            //*** Check the collision
            Vector3 normal;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (boxCollider.Collides(gameObjects[i].Get<Collider>(), out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, gameObjects[i].Get<Rigidbody>().Velocity) < 0)
                        gameObjects[i].Rigidbody.Impulse += Vector3.Dot(normal, gameObjects[i].Get<Rigidbody>().Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    if (gameObjects[i].Get<Collider>().Collides(gameObjects[j].Get<Collider>(), out normal)) numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                    gameObjects[i].Get<Rigidbody>().Velocity - gameObjects[j].Get<Rigidbody>().Velocity) * -2
                    * normal * gameObjects[i].Get<Rigidbody>().Mass * gameObjects[j].Get<Rigidbody>().Mass;

                    gameObjects[i].Rigidbody.Impulse += velocityNormal / 2;
                    gameObjects[j].Rigidbody.Impulse += -velocityNormal / 2;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (!isTextureShown)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    float speed = gameObjects[i].Get<Rigidbody>().Velocity.Length();
                    float speedValue = MathHelper.Clamp(speed / 50f, 0, 1);
                    if (isColorsShown)
                    {
                        (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                            new Vector3(speedValue, speedValue, 1);
                    }
                    model.Draw(gameObjects[i].Transform.World, camera.View, camera.Projection);
                }
            }
            else
            {
                for (int i = 0; i < gameObjects.Count; i++) gameObjects[i].Draw();
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Press Shift to see Diagnostics: ", new Vector2(0, 200), Color.Black);
            if (isDialogueShown)
            {
                _spriteBatch.DrawString(font, "Number Of Collisions: " + numberCollisions, new Vector2(0, 40), Color.Black);
                _spriteBatch.DrawString(font, "Last Second Collisions: " + lastSecondCollisions, new Vector2(0, 20), Color.Black);
                _spriteBatch.DrawString(font, "Number of Spheres: " + gameObjects.Count, Vector2.Zero, Color.Black);
                _spriteBatch.DrawString(font, "FPS: " + frameRate, new Vector2(0, 60), Color.Black);
                _spriteBatch.DrawString(font, "Press T to toggle between Threads: " + haveThreadRunning, new Vector2(0, 80), Color.Black);
                _spriteBatch.DrawString(font, "All other keys on the instructions are the same ", new Vector2(0, 220), Color.Black);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //*** Lab 07 ******************
        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void AddGameObject()
        {
            //Intialize Game Object -- Assignment 3
            GameObject = new GameObject();
            //Step 1
            GameObject.Transform.LocalPosition += Vector3.Right * 3 * random.Next(1, 5); // does not overlap sphere
            //Step 2 Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            //rigidbody.Transform = transform;
            rigidbody.Mass = random.Next(1,5);
            Vector3 direction = new Vector3(
                (float)random.NextDouble(), (float)random.NextDouble(),
                (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);
            //Step 3 Collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * GameObject.Transform.LocalScale.Y;
            sphereCollider.Transform = GameObject.Transform;

            //Step 4 Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");

            Renderer renderer = new Renderer(model, GameObject.Transform, camera, Content, GraphicsDevice, light,
                1, "SimpleShading", 20f, texture);

            GameObject.Add<Renderer>(renderer);
            GameObject.Add<Collider>(sphereCollider);
            GameObject.Add<Rigidbody>(rigidbody);
            
            gameObjects.Add(GameObject);
        }
    }
}