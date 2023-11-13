using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Sprite
    {
        //Properties
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Source { get; set; }
        public Color Color { get; set; }
        public Single Rotation { get; set; }
        public Vector2 origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects Effect { get; set; }
        public Single Layer { get; set; }


        //constructor
        public Sprite(Texture2D texture) 
        {
            Texture = texture;
            Position = new Vector2(0, 0);
            Source = new Rectangle(0,0,texture.Width, texture.Height);
            Color = Color.White;
            Rotation = 0;
            origin = new Vector2(20, 10);
            Scale = new Vector2(1, 1);
            Effect = SpriteEffects.None;
            Layer = 1;
        }

        //Methods
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color, Rotation, origin, Scale, Effect, Layer);
        }
    }
}
