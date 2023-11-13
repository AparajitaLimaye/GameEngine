using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine; //Go to Lab02 Dependencies -> Right click Add Reference -> Check GameEngine, Click OK

namespace Lab02
{
    public class Lab02 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpiralMover spiral;

        Sprite sprite;
        //KeyboardState prevState;

        public Lab02()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //prevState = Keyboard.GetState();

            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //sprite = new Sprite(Content.Load<Texture2D>("Square"));
            spiral = new SpiralMover(Content.Load<Texture2D>("Square"), new Vector2(300, 300));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Update();
            Time.Update(gameTime);
            spiral.Update();
            //KeyboardState currentState = Keyboard.GetState();
            /*if (InputManager.IsKeyPressed(Keys.Left))
                sprite.Position += Vector2.UnitX * -5;
            if (InputManager.IsKeyPressed(Keys.Right))
                sprite.Position += Vector2.UnitX * 5;
            if (InputManager.IsKeyPressed(Keys.Up))
                sprite.Position += Vector2.UnitY * -5;
            if (InputManager.IsKeyPressed(Keys.Down))
                sprite.Position += Vector2.UnitY * 5;
            if (InputManager.IsKeyDown(Keys.Space))
                sprite.Rotation += 0.05f;*/
            //prevState = currentState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            spiral.Draw(_spriteBatch); //Sprite class Method: Draw
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}