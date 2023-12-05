using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace FinalGame
{
    public class Player : GameObject
    {
        //Properties
        public TerrainRenderer Terrain { get; set; }
        Model model;
        GameObject gameObject = new GameObject();
        Camera Camera;

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera,
            GraphicsDevice graphicsDevice, Light light) : base()
        {
            model = Content.Load<Model>("Sphere");
            Terrain = terrain;
            Camera = camera;

            //Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            //Sphere Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1;
            collider.Transform = Transform;
            Add<Collider>(collider);

            //Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(model, Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            Add<Renderer>(renderer);
        }

        public override void Update()
        {
            /*Debug.WriteLine("Forward: " + ((this.Transform.LocalPosition + this.Transform.Forward).Y) +
                "Backward: " + (Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Backward * Time.ElapsedGameTime * 10f)));*/
            //Control the player
            if (InputManager.IsKeyDown(Keys.W) && !(Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Forward * Time.ElapsedGameTime * 10f) > 1))//move forward
            {
                this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * 10f;
                Camera.Transform.LocalPosition = this.Transform.Position;
            }
            if (InputManager.IsKeyDown(Keys.S) && !(Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Backward * Time.ElapsedGameTime * 10f) > 1)) //move backward
            { 
                this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * 10f;
                Camera.Transform.LocalPosition = this.Transform.Position;
            }
            if (InputManager.IsKeyDown(Keys.A) && !(Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Left * Time.ElapsedGameTime * 10f) > 1)) //move left
            {
                //this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * 10f;
                this.Transform.Rotate(new Vector3(0, 2, 0), Time.ElapsedGameTime);
                Camera.Transform.LocalPosition = this.Transform.Position;
                Camera.Transform.Rotate(new Vector3(0,2,0), Time.ElapsedGameTime);
            }
            if (InputManager.IsKeyDown(Keys.D) && !(Terrain.GetAltitude(this.Transform.LocalPosition + this.Transform.Right * Time.ElapsedGameTime * 10f) > 1)) //move right
            {
                //this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * 10f;
                this.Transform.Rotate(new Vector3(0, -2, 0), Time.ElapsedGameTime);
                Camera.Transform.LocalPosition = this.Transform.Position;
                Camera.Transform.Rotate(new Vector3(0, -2, 0), Time.ElapsedGameTime);
            }


            //make sure that the player is at the right altitude of the terrain
            this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                Terrain.GetAltitude(this.Transform.LocalPosition),
                this.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update();
        }

        public void Draw(Camera camera)
        {
            model.Draw(this.Transform.World, camera.View, camera.Projection);
            base.Draw();
        }
    }
}
