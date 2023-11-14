using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;

namespace GoingBeyond
{
    public class GoingBeyond : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Camera/View information
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        Matrix projectionMatrix;
        Matrix viewMatrix;
        //Audio Components
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        SoundEffect soundExplosion2;
        SoundEffect soundExplosion3;
        SoundEffect soundWeaponsFire;
        //Visual components
        Ship ship = new Ship();
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Model bulletModel;
        Matrix[] bulletTransforms;
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];
        Texture2D stars;
        SpriteFont spritefont;
        int score;

        Random random = new Random();

        public GoingBeyond()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        private void ResetAsteroids()
        {
            float xStart = random.Next();
            float yStart = random.Next();
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i].position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].direction.X = -(float)Math.Sin(angle);
                asteroidList[i].direction.Y = (float)Math.Cos(angle);
                asteroidList[i].speed = GameConstants.AsteroidMinSpeed +
                    (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
                asteroidList[i].isActive = true;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before
        /// starting to run. This is where it can query for any required
        /// services and load any non‐graphic related content.
        /// Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                GraphicsDevice.DisplayMode.AspectRatio,
                GameConstants.CameraHeight - 1000.0f,
                GameConstants.CameraHeight + 1000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

            ResetAsteroids();

            base.Initialize();
        }

        //Lighting Effect for the model
        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ship.model = Content.Load<Model>("p1_wedge");
            asteroidModel = Content.Load<Model>("asteroid4");
            asteroidTransforms = SetupEffectDefaults(asteroidModel);
            bulletModel = Content.Load<Model>("bullet");
            bulletTransforms = SetupEffectDefaults(bulletModel);
            stars = Content.Load<Texture2D>("B1_stars");
            ship.Transforms = SetupEffectDefaults(ship.model);
            spritefont = Content.Load<SpriteFont>("spriteFont");

            soundEngine = Content.Load<SoundEffect>("engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundHyperspaceActivation =
                Content.Load<SoundEffect>("hyperspace_activate");

            soundExplosion2 =
                Content.Load<SoundEffect>("explosion2");
            soundExplosion3 =
                Content.Load<SoundEffect>("explosion3");
            soundWeaponsFire =
                Content.Load<SoundEffect>("tx0_fire1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            //Get some input.
            UpdateInput(gameTime);

            //Add velocity to the current position
            ship.Position += ship.Velocity;

            //Bleed off velocity over time.
            ship.Velocity *= 0.95f;

            //asteroids in motion
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    bulletList[i].Update(timeDelta);
                }
            }

            //Sound
            //ship‐asteroid collision check
            if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                    ship.Position, ship.model.Meshes[0].BoundingSphere.Radius *
                    GameConstants.ShipBoundingSphereScale);
                for (int i = 0; i < asteroidList.Length; i++)
                {
                    if (asteroidList[i].isActive)
                    {
                        BoundingSphere b = new BoundingSphere(asteroidList[i].position,
                            asteroidModel.Meshes[0].BoundingSphere.Radius *
                            GameConstants.AsteroidBoundingSphereScale);
                        if (b.Intersects(shipSphere))
                        {
                            //blow up ship
                            soundExplosion3.Play();
                            ship.isActive = false;
                            asteroidList[i].isActive = false;
                            break; //exit the loop
                        }
                    }
                }
            }

            //bullet‐asteroid collision check
            for (int i = 0; i < asteroidList.Length; i++)
            {
                if (asteroidList[i].isActive)
                {
                    BoundingSphere asteroidSphere =
                    new BoundingSphere(asteroidList[i].position,
                    asteroidModel.Meshes[0].BoundingSphere.Radius *
                    GameConstants.AsteroidBoundingSphereScale);
                    for (int j = 0; j < bulletList.Length; j++)
                    {
                        if (bulletList[j].isActive)
                        {
                            BoundingSphere bulletSphere = new BoundingSphere(
                            bulletList[j].position,
                            bulletModel.Meshes[0].BoundingSphere.Radius);
                            if (asteroidSphere.Intersects(bulletSphere))
                            {
                                soundExplosion2.Play();
                                asteroidList[i].isActive = false;
                                bulletList[j].isActive = false;
                                break; //no need to check other bullets
                            }
                        }
                    }
                }
            }

            ship.Update(gameTime);

            base.Update(gameTime);
        }

        protected void UpdateInput(GameTime gameTime)
        {
            InputManager.Update();
            Time.Update(gameTime);

            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                ship.Update(gameTime);
                //Play engine sound only when the engine is on.
                if (InputManager.IsKeyPressed(Keys.W))
                {
                    if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else
                        soundEngineInstance.Resume();
                }
                /*else if (currentState.Triggers.Right == 0)
                {
                    if (soundEngineInstance.State == SoundState.Playing)
                        soundEngineInstance.Pause();
                }*/
                // In case you get lost, press A to warp back to the center.
                if (InputManager.IsKeyPressed(Keys.LeftShift))
                {
                    ship.Position = Vector3.Zero;
                    ship.Velocity = Vector3.Zero;
                    ship.Rotation = 0.0f;
                    soundHyperspaceActivation.Play();
                }
                //are we shooting?
                if (ship.isActive && InputManager.IsKeyPressed(Keys.Space))
                {
                    //add another bullet. Find an inactive bullet slot and use it
                    //if all bullets slots are used, ignore the user input
                    for (int i = 0; i < GameConstants.NumBullets; i++)
                    {
                        if (!bulletList[i].isActive)
                        {
                            bulletList[i].direction = ship.RotationMatrix.Forward;
                            bulletList[i].speed = GameConstants.BulletSpeedAdjustment;
                            bulletList[i].position = ship.Position + (200 * bulletList[i].direction);
                            bulletList[i].isActive = true;
                            soundWeaponsFire.Play();
                            score -= GameConstants.ShotPenalty;
                            break; //exit the loop
                        }
                    }
                }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
            _spriteBatch.DrawString(spritefont, "Score: " + score, new Vector2(100, 50), Color.LimeGreen);
            _spriteBatch.End();

            if (ship.isActive)
            {
                Matrix shipTransformMatrix = ship.RotationMatrix
                    * Matrix.CreateTranslation(ship.Position);
                DrawModel(ship.model, shipTransformMatrix, ship.Transforms);
            }

            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (asteroidList[i].isActive)
                {
                    Matrix asteroidTransform =
                        Matrix.CreateTranslation(asteroidList[i].position);
                    DrawModel(asteroidModel, asteroidTransform, asteroidTransforms);
                }
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    Matrix bulletTransform =
                    Matrix.CreateTranslation(bulletList[i].position);
                    DrawModel(bulletModel, bulletTransform, bulletTransforms);
                }
            }

            base.Draw(gameTime);
        }

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index]
                        *modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
    }
}