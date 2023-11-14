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

        //constructor
        public Ship(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            InputManager.Initialize();
            Time.Initialize();

            //Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            //Add Renderer
            Texture2D texture = content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(content.Load<Model>("p1_wedge"),
                Transform, camera, content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            //Add Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            collider.Transform = Transform;
            Add<Collider>(collider);
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update();
            Time.Update(gameTime);

            if(InputManager.IsKeyDown(Keys.W))
            {
                Transform.LocalPosition += Vector3.Up;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                Transform.LocalPosition += Vector3.Down;
            }
            if (InputManager.IsKeyDown(Keys.A))
            {
                Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            }
        }
    }
}
