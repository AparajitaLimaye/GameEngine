using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5
{
    public class Player : GameObject
    {
        //Properties
        public TerrainRenderer Terrain { get; set; }
        Model model;
        GameObject gameObject = new GameObject();

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera,
            GraphicsDevice graphicsDevice, Light light) : base()
        {
            model = Content.Load<Model>("Torus");
            Terrain = terrain;

            //Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            this.Add<Rigidbody>(rigidbody);
        }

        public override void Update()
        {
            InputManager.Update();

            //Control the player
            if (InputManager.IsKeyDown(Keys.W)) //move forward
                this.Transform.LocalPosition += Vector3.Forward;
            if (InputManager.IsKeyDown(Keys.S)) //move backward
                this.Transform.LocalPosition += Vector3.Backward;
            if (InputManager.IsKeyDown(Keys.A)) //move left
                this.Transform.LocalPosition += Vector3.Left;
            if (InputManager.IsKeyDown(Keys.D)) //move right
                this.Transform.LocalPosition += Vector3.Right;


            //make sure that the player is at the right altitude of the terrain
            /*this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                10,
                this.Transform.LocalPosition.Z);*/

            base.Update();
        }

        public void Draw(Camera camera)
        {
            model.Draw(this.Transform.World, camera.View, camera.Projection);
            base.Draw();
        }
    }
}
