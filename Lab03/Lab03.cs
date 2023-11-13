using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
                                                                                                                                                                                                                      
using CPI311.GameEngine;
using Lab02;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Lab03
{
    public class Lab03 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // *** First step
        Model model; //For FBX 3D model
        Matrix world, view, projection;

        // *** Second Step
        Vector3 cameraPos = new Vector3(0, 0, 5);
        Vector3 modelPos = new Vector3(0, 0, 0);
        float modelScale = 1.0f;
        float yaw=0,pitch=0, roll=0;

        // *** Toggle
        bool worldBool = true; // world view
        bool projectionBool = true; // camera view

        // *** Camera Center and Size
        Vector2 cameraCenter = new Vector2(0, 0);
        Vector2 cameraSize = new Vector2(1, 1);

        //Draw String
        SpriteFont font;

        public Lab03()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
                            
        protected override void Initialize()
        {
            InputManager.Initialize();
            Time.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");

            model = Content.Load<Model>("Torus");
            //** Lighting Effect *************************
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            //**********************************************
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // ** CPI311 Manager Update
            InputManager.Update();
            Time.Update(gameTime);
            //** Camera *************************
            if(InputManager.IsKeyDown(Keys.S))
                cameraPos += Vector3.Up * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.W))
                cameraPos += Vector3.Down * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A))
                cameraPos += Vector3.Right * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.D))
                cameraPos += Vector3.Left * Time.ElapsedGameTime * 5;
            // ********* Camera Center ******************
            if (InputManager.IsKeyDown(Keys.S) && InputManager.IsKeyDown(Keys.LeftShift))
                cameraCenter.Y -= Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.W) && InputManager.IsKeyDown(Keys.LeftShift))
                cameraCenter.Y += Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.A) && InputManager.IsKeyDown(Keys.LeftShift))
                cameraCenter.X -= Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.D) && InputManager.IsKeyDown(Keys.LeftShift))
                cameraCenter.X += Time.ElapsedGameTime * 0.5f;
            // ********* Camera Dimension ******************
            if (InputManager.IsKeyDown(Keys.S) && InputManager.IsKeyDown(Keys.LeftControl))
                cameraSize.X -= Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.W) && InputManager.IsKeyDown(Keys.LeftControl))
                cameraSize.X += Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.A) && InputManager.IsKeyDown(Keys.LeftControl))
                cameraSize.Y -= Time.ElapsedGameTime * 0.5f;
            if (InputManager.IsKeyDown(Keys.D) && InputManager.IsKeyDown(Keys.LeftControl))
                cameraSize.Y += Time.ElapsedGameTime * 0.5f;

            //** Model *************************
            if (InputManager.IsKeyDown(Keys.Up))
                modelPos += Vector3.Forward * Time.ElapsedGameTime * 5;
                //yaw += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Down))
                modelPos += Vector3.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Right))
                modelPos += Vector3.Right * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Left))
                modelPos += Vector3.Left * Time.ElapsedGameTime * 5;

            //** yaw, pitch, roll, model Scale ***************
            if (InputManager.IsKeyDown(Keys.Insert))
                yaw += 0.5f;
            if (InputManager.IsKeyDown(Keys.Delete))
                yaw -= 0.5f;
            if (InputManager.IsKeyDown(Keys.Home))
                pitch += 0.5f;
            if (InputManager.IsKeyDown(Keys.End))
                pitch -= 0.5f;
            if (InputManager.IsKeyDown(Keys.PageUp))
                roll += 0.5f;
            if (InputManager.IsKeyDown(Keys.PageDown))
                roll -= 0.5f;
            if (InputManager.IsKeyDown(Keys.Up) && InputManager.IsKeyDown(Keys.LeftShift))
                modelScale = 1.0f;
            if (InputManager.IsKeyDown(Keys.Down) && InputManager.IsKeyDown(Keys.LeftShift))
                modelScale = -1.0f;

            if (InputManager.IsKeyPressed(Keys.Space))
                worldBool = !worldBool;
            if(worldBool)
            {
                world = Matrix.CreateScale(modelScale) *
                    Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                    Matrix.CreateTranslation(modelPos);
            }
            else
            {
                world = Matrix.CreateTranslation(modelPos) *
                        Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                        Matrix.CreateScale(modelScale);
            }

            view = Matrix.CreateLookAt(
                cameraPos, 
                cameraPos + Vector3.Forward,
                new Vector3(0,1,0));


            if (InputManager.IsKeyPressed(Keys.Tab))
                projectionBool = !projectionBool;
            if (projectionBool)
            {
                projection = Matrix.CreatePerspectiveOffCenter(cameraCenter.X - cameraSize.X, cameraCenter.X + cameraSize.X, cameraCenter.Y - cameraSize.Y, cameraCenter.Y + cameraSize.Y, 0.1f, 100f);
            }
            else
            {
                projection = Matrix.CreateOrthographicOffCenter(cameraCenter.X - cameraSize.X, cameraCenter.X + cameraSize.X, cameraCenter.Y - cameraSize.Y, cameraCenter.Y + cameraSize.Y, 0.1f, 100f);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            model.Draw(world, view, projection);

            _spriteBatch.Begin();
            //Camera Center
            _spriteBatch.DrawString(font, "Camera Center X: " + cameraCenter.X.ToString(), new Vector2(50, 20), Color.Black);
            _spriteBatch.DrawString(font, "Camera Center Y: " + cameraCenter.Y.ToString(), new Vector2(50, 35), Color.Black);
            //Camera Size
            _spriteBatch.DrawString(font, "Camera Size X: " + cameraSize.X.ToString(), new Vector2(50, 50), Color.Black);
            _spriteBatch.DrawString(font, "Camera Size Y: " + cameraSize.Y.ToString(), new Vector2(50, 65), Color.Black);
            //Camera Position
            _spriteBatch.DrawString(font, "Camera Pos X: " + cameraPos.X.ToString(), new Vector2(50, 80), Color.Black);
            _spriteBatch.DrawString(font, "Camera Pos Y: " + cameraPos.Y.ToString(), new Vector2(50, 95), Color.Black);
            //yaw, pitch, roll, model Scale
            _spriteBatch.DrawString(font, "Yaw: " + cameraPos.X.ToString(), new Vector2(50, 110), Color.Black);
            _spriteBatch.DrawString(font, "Pitch: " + cameraPos.Y.ToString(), new Vector2(50, 125), Color.Black);
            _spriteBatch.DrawString(font, "Roll: " + cameraPos.X.ToString(), new Vector2(50, 140), Color.Black);
            _spriteBatch.DrawString(font, "Model Scale: " + modelScale, new Vector2(50, 155), Color.Black);
            //world view
            if (worldBool)
                _spriteBatch.DrawString(font, "World View: Scale * Rotation * Pitch", new Vector2(50, 170), Color.Black);
            else
                _spriteBatch.DrawString(font, "World View: Pitch * Rotation * Scale", new Vector2(50, 170), Color.Black);
            //projection type
            if (projectionBool)
                _spriteBatch.DrawString(font, "Projection View: Perspective", new Vector2(50, 185), Color.Black);
            else
                _spriteBatch.DrawString(font, "Projection View: Orthographic", new Vector2(50, 185), Color.Black);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}