using CPI311.GameEngine;
using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms.Design;

namespace Assignment2
{
    public class Assignment2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model sun;
        Transform sunTransform;
        Model mercury;
        Transform mercuryTransform;
        Model earth;
        Transform earthTransform;
        Model moon;
        Transform moonTransform;
        Camera camera;
        Transform cameraTransform;
        Camera TPcamera;
        Transform TPcameraTransform;
        Camera FPcamera;
        Transform FPcameraTransform;
        Model background;
        Transform backgroundTransform;
        SpriteFont font;

        float speed = 1f;

        public Assignment2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            sun = Content.Load<Model>("Sun");
            sunTransform = new Transform();

            mercury = Content.Load<Model>("mercury");
            mercuryTransform = new Transform();
            mercuryTransform.Parent = sunTransform;
            mercuryTransform.LocalPosition = mercuryTransform.LocalPosition + Vector3.Up*2;

            earth = Content.Load<Model>("earth");
            earthTransform = new Transform();
            earthTransform.Parent = sunTransform;
            earthTransform.LocalPosition = sunTransform.LocalPosition + Vector3.Up * 8;

            moon = Content.Load<Model>("moon");
            moonTransform = new Transform();
            moonTransform.Parent = earthTransform;
            moonTransform.LocalPosition = moonTransform.LocalPosition + Vector3.Up * 2;

            TPcameraTransform = new Transform();
            TPcamera = new Camera();
            TPcameraTransform.Position = new Vector3(0, 0, 0);
            TPcameraTransform.LocalPosition = (Vector3.Right * 50);
            TPcameraTransform.LocalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 300f);
            TPcameraTransform.Rotate(Vector3.Right, -1.55f);
            TPcamera.Transform = TPcameraTransform;

            FPcamera = new Camera();
            FPcameraTransform = new Transform();
            FPcameraTransform.Position = new Vector3(0,0,0);
            FPcameraTransform.LocalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 300f);
            FPcamera.Transform = FPcameraTransform;

            camera = new Camera();
            cameraTransform = new Transform();
            camera = TPcamera;
            cameraTransform = TPcameraTransform;

            background = Content.Load<Model>("Plane");
            backgroundTransform = new Transform();
            backgroundTransform.LocalPosition = Vector3.Left * 10;
            backgroundTransform.LocalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 300f);

            font = Content.Load<SpriteFont>("Font");

            //*** Lighting Effect ********************************
            foreach (ModelMesh mesh in sun.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in mercury.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in earth.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in moon.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            //Rotation and Radius
            sunTransform.Rotate(Vector3.Left, Time.ElapsedGameTime * speed);
            sunTransform.Scale = new Vector3(5,5,5);
            mercuryTransform.Scale = new Vector3(2,2,2);
            earthTransform.Rotate(Vector3.Left, 0.01f);
            earthTransform.Scale = new Vector3(3, 3, 3);
            moonTransform.Rotate(Vector3.Left, 0.05f);
            moonTransform.Scale = new Vector3(1, 1, 1);

            //Toggle of Camera
            if (InputManager.IsKeyPressed(Keys.Tab))
            {
                if(camera != TPcamera)
                {
                    camera = TPcamera; 
                    cameraTransform = TPcameraTransform;
                }
                else
                {
                    camera = FPcamera;
                    cameraTransform = FPcameraTransform;
                }
            }

            //Camera Movement
            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.LocalPosition += cameraTransform.Left * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.LocalPosition += cameraTransform.Right * Time.ElapsedGameTime * 5;

            //Speed of animation
            if (InputManager.IsKeyDown(Keys.Right))
                speed += 0.1f;
            if (InputManager.IsKeyDown(Keys.Left))
                speed -= 0.1f;

            //Zoom of camera
            if (InputManager.IsKeyDown(Keys.Up)) //Zoom in
            {
                if(camera.FieldOfView > 0.1f)
                    camera.FieldOfView -= 0.1f;
            }
            if (InputManager.IsKeyDown(Keys.Down)) //Zoom out
            {
                if (camera.FieldOfView < 2)
                    camera.FieldOfView += 0.1f;
            }

            //Testing Rotation *************************
            /*if (InputManager.IsKeyDown(Keys.Left))
                cameraTransform.Rotate(new Vector3(1, 0, 0), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Right))
                cameraTransform.Rotate(new Vector3(0, 1, 0), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Up))
                cameraTransform.Rotate(new Vector3(0, 0, 1), Time.ElapsedGameTime);*/

            /*if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.LocalPosition += cameraTransform.Left * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.LocalPosition += cameraTransform.Right * Time.ElapsedGameTime * 5;

            if (InputManager.IsKeyDown(Keys.Left))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Right))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(-1, 0, 0), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Up))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Down))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.M))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.N))
                cameraTransform.LocalRotation += Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), Time.ElapsedGameTime);*/
            //*********************************************

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = new DepthStencilState();

            sun.Draw(sunTransform.World, camera.View, camera.Projection);
            mercury.Draw(mercuryTransform.World, camera.View, camera.Projection);
            earth.Draw(earthTransform.World, camera.View, camera.Projection);
            moon.Draw(moonTransform.World, camera.View, camera.Projection);
            background.Draw(backgroundTransform.World, camera.View, camera.Projection);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Tab: TP vs FP", new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "WASD: Camera movement", new Vector2(0, 15), Color.Black);
            _spriteBatch.DrawString(font, "Right/Left Key: Animation Speed", new Vector2(0, 30), Color.Black);
            _spriteBatch.DrawString(font, "Up/Down Key: Camera Zoom", new Vector2(0, 45), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}