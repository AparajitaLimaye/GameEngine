using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lab02;

namespace CPI311.GameEngine
{
    public class SpiralMover
    {
        public Sprite sprite { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Phase { get; set; }
        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float Speed { get; set; }

        public SpiralMover(Texture2D texture, Vector2 position, float radius = 150, float amplitude = 10, float frequency = 20, float speed = 1)
        {
            sprite = new Sprite(texture);
            Position = position;
            Radius = radius;
            Amplitude = amplitude;
            Frequency = frequency;
            Speed = speed;
            sprite.Position = Position + new Vector2(Radius, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public void Update()
        {
            Position = new Vector2(300, 300);

            if (InputManager.IsKeyDown(Keys.Right)) Radius += 1;
            if (InputManager.IsKeyDown(Keys.Left)) Radius -= 1;
            if (InputManager.IsKeyDown(Keys.Up)) Speed += 0.05f;
            if (InputManager.IsKeyDown(Keys.Down)) Speed -= 0.05f;

            if (InputManager.IsKeyDown(Keys.W)) Amplitude += 2;
            if (InputManager.IsKeyDown(Keys.S)) Amplitude -= 2;
            if (InputManager.IsKeyDown(Keys.A)) Frequency += 2;
            if (InputManager.IsKeyDown(Keys.D)) Frequency -= 2;

            Phase += Time.ElapsedGameTime * Speed;

            sprite.Position = Position + new Vector2(
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Cos(Phase)),
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Sin(Phase))
                );
            /*InputManager.Update();
            if (InputManager.IsKeyPressed(Keys.Left))
                sprite.Position += Vector2.UnitX * -5;
            if (InputManager.IsKeyPressed(Keys.Right))
                sprite.Position += Vector2.UnitX * 5;
            if (InputManager.IsKeyPressed(Keys.Up))
                sprite.Position += Vector2.UnitY * -5;
            if (InputManager.IsKeyPressed(Keys.Down))
                sprite.Position += Vector2.UnitY * 5;*/
        }
    }
}
