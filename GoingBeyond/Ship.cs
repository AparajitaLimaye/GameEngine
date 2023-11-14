﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using Lab02;

namespace GoingBeyond
{
    class Ship : GameObject
    {
        public Model model;
        public Matrix[] Transforms;

        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;

        //** direction the model is facing
        Vector3 direction = Vector3.Zero;

        //Velocity of the mode, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;
        //amplifies controller speed input
        private const float VelocityScale = 5.0f;
        public bool isActive = true;

        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }
                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix =
                    Matrix.CreateRotationX(MathHelper.PiOver2) 
                    * Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public void Update(GamePadState controllerState)
        {

            /*//Rotate the model using the left thumbstick, and scale it down.
            Rotation -= controllerState.ThumbSticks.Left.X * 0.10f;

            //Finally, add this vector to our velocity.
            Velocity += RotationMatrix.Forward * VelocityScale *
                controllerState.Triggers.Right;*/
            InputManager.Update();
            //Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.W))
            {
                Position += Vector3.Forward; // direction * 0.5f;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                Rigidbody.Transform.LocalPosition += Vector3.Backward;
            }
            /*if (InputManager.IsKeyDown(Keys.A))
            {
                Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                direction = new Vector3((float)Math.Cos(Time.ElapsedGameTime), (float)Math.Sin(Time.ElapsedGameTime), 0f);
                direction.Normalize();
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
                direction = new Vector3((float)Math.Cos(Time.ElapsedGameTime), (float)Math.Sin(Time.ElapsedGameTime), 0f);
                direction.Normalize();
            }*/
        }
    }
}
