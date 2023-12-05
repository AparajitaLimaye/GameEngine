using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace FinalGame
{
    public class Bullet : GameObject
    {
        //Properties
        public bool isActive;
        Player player;

        //constructor
        public Bullet(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light, Player player) : base()
        {
            isActive = true;
            //Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            rigidbody.Velocity = Transform.Forward * 3f;
            Add<Rigidbody>(rigidbody);

            //Add Renderer
            Texture2D texture = content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(content.Load<Model>("Sphere"),
                Transform, camera, content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            //Add Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            collider.Transform = Transform;
            Add<Collider>(collider);

            this.Transform.Scale = Vector3.One * 0.5f;

            this.player = player;
            //Debug.WriteLine("In Here");
        }

        public override void Update()
        {

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                Debug.WriteLine("In Here");
                isActive = true;
                Transform.Position = player.Transform.Position;
                Transform.LocalRotation = player.Transform.LocalRotation;
                Rigidbody.Velocity = Transform.Forward * 10f;
                //Debug.WriteLine("In Here");
            }
            

            base.Update();
        }

        public override void Draw()
        {
            if (!isActive)
                return;

            base.Draw();

        }
    }
}
