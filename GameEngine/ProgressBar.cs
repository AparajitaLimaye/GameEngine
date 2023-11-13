using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class ProgressBar : Sprite
    {
        public Color FillColor { get; set;}
        public float Value { get; set;}
        public float Speed { get; set;}
        public Texture2D Texture { get; set;}

        public ProgressBar(Texture2D texture) : base(texture)
        {
            Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)Value, 32),
                FillColor, Rotation, origin, Scale, Effect, Layer);
        }
    }
}
