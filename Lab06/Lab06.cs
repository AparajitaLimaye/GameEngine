using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;
using Lab02;

namespace Lab06
{
    public class Lab06 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BoxCollider boxCollider;
        SphereCollider sphere1, sphere2;

        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;

        Random random;
        //** from Lab4
        Model model;
        Camera camera;
        Transform cameraTransform;
        int numberCollisions = 0;

        public Lab06()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            random = new Random();
            transforms = new List<Transform>();
            colliders = new List<Collider>();
            rigidbodies = new List<Rigidbody>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10; //box size is 10

            for( int i = 0; i < 2; i++ ) //each sphere
            {
                Transform transform = new Transform();
                transform.LocalPosition += Vector3.Right * 3 * i; // does not overlap sphere
                Rigidbody rigidbody = new Rigidbody();
                rigidbody.Transform = transform;
                rigidbody.Mass = 1;

                Vector3 direction = new Vector3(
                    (float) random.NextDouble(), (float) random.NextDouble(),
                    (float) random.NextDouble());
                direction.Normalize();
                rigidbody.Velocity = direction * ((float) random.NextDouble() * 5 + 5);
                SphereCollider sphereCollider = new SphereCollider();
                sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
                sphereCollider.Transform = transform;

                transforms.Add(transform);
                colliders.Add(sphereCollider);
                rigidbodies.Add(rigidbody);
            }

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time.Update(gameTime);
            foreach (Rigidbody rigidbody in rigidbodies) rigidbody.Update();

            //*** Check the collision
            Vector3 normal;
            for(int i = 0; i < transforms.Count; i++)
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

            foreach (Transform transform in transforms)
                model.Draw(transform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}