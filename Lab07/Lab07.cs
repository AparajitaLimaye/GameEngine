using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Threading;  //For Multi Threading
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab07
{
    public class Lab07 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BoxCollider boxCollider;
        SphereCollider sphere1, sphere2;

        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;
        List<Renderer> renderers;

        Random random;
        //** from Lab4
        Model model;
        Camera camera;
        Transform cameraTransform;
        int numberCollisions = 0;

        //** Lab 07 *****************
        int numberCollision;
        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;
        SpriteFont font;
        //***************************

        public Lab07()
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

            //** Lab 07 *********************************************
            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            //*******************************************************

            random = new Random();
            transforms = new List<Transform>();
            colliders = new List<Collider>();
            rigidbodies = new List<Rigidbody>();
            renderers = new List<Renderer>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10; //box size is 10

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Sphere");
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

            font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyPressed(Keys.Space)) AddSphere();

            foreach (Rigidbody rigidbody in rigidbodies) rigidbody.Update();

            //*** Check the collision
            Vector3 normal;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse += Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal)) numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                    rigidbodies[i].Velocity - rigidbodies[j].Velocity) * -2
                    * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;

                    rigidbodies[i].Impulse += velocityNormal / 2;
                    rigidbodies[j].Impulse += -velocityNormal / 2;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //foreach (Transform transform in transforms)
            //model.Draw(transform.World, camera.View, camera.Projection);

            for (int i = 0; i < renderers.Count; i++) renderers[i].Draw();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Collision: " + lastSecondCollisions, Vector2.Zero, Color.Black);
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

        private void AddSphere()
        {
            //Step 1
            Transform transform = new Transform();
            transform.LocalPosition += Vector3.Right * 3 * random.Next(1, 5); // does not overlap sphere
            //Step 2 Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
            rigidbody.Mass = 1;
            Vector3 direction = new Vector3(
                (float)random.NextDouble(), (float)random.NextDouble(),
                (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);
            //Step 3 Collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            //Step 4 Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(model, transform, camera, Content, GraphicsDevice, 
                1, "SimpleShading", 20f, texture);

            renderers.Add(renderer);
            transforms.Add(transform);
            colliders.Add(sphereCollider);
            rigidbodies.Add(rigidbody);
        }
    }
}