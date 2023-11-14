﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment4
{
     public class Bullet : GameObject
    {

        //constructor
        public Bullet(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            //Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            //Add Renderer
            Texture2D texture = content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(content.Load<Model>("bullet"), 
                Transform, camera, content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            //Add Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            collider.Transform = Transform;
            Add<Collider>(collider);
        }
    }
}
