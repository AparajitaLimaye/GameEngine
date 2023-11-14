using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment4
{
    public class Ship : GameObject
    {
        //*** Properties
        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;
        public GameObject gameObject = new GameObject();
        Model model;
        Vector3 direction;

        //constructor
        public Ship(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            InputManager.Initialize();
            Time.Initialize();

            direction = new Vector3();

            //Create Model
            model = content.Load<Model>("p1_wedge");

            //Game Object stuff
            gameObject.Transform.LocalPosition = Position;

            //Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = gameObject.Transform;
            rigidbody.Mass = 1;
            gameObject.Add<Rigidbody>(rigidbody);

            //Add Renderer
            Texture2D texture = content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(content.Load<Model>("p1_wedge"),
                 gameObject.Transform, camera, content, graphicsDevice, light, 1, null, 20f, texture);
            gameObject.Add<Renderer>(renderer);

            //Add Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * gameObject.Transform.LocalScale.Y; ;
            collider.Transform = gameObject.Transform;
            gameObject.Add<Collider>(collider);
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.W))
            {
                gameObject.Rigidbody.Transform.LocalPosition += direction * 0.5f;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                gameObject.Rigidbody.Transform.LocalPosition += Vector3.Backward;
            }
            if (InputManager.IsKeyDown(Keys.A))
            {
                gameObject.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                direction = new Vector3((float)Math.Cos(Time.ElapsedGameTime),(float)Math.Sin(Time.ElapsedGameTime), 0f);
                direction.Normalize();
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                gameObject.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
                direction = new Vector3((float)Math.Cos(Time.ElapsedGameTime), (float)Math.Sin(Time.ElapsedGameTime), 0f);
                direction.Normalize();
            }
        }

        public void Draw(Camera camera)
        {
            model.Draw(gameObject.Transform.World, camera.View, camera.Projection);
            base.Draw();
        }
    }
}
