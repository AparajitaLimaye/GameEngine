using Lab02;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class AnimatedSprite : Sprite
    {
        //Properties
        public Texture2D Texture;
        public int Frames { get; set; } //number of frames
        public float Frame { get; set; } //current frame
        public float Speed { get; set; }
        int i = 0;

        /*int startFrame;
        int currFrame;
        int numFrames;
        float frameTime;
        float animFPS = 24.0f;*/

        //Constructor
        public AnimatedSprite(Texture2D texture, int frames = 8) : base(texture)
        {
            Frames = frames;
            Frame = 0;
            Speed = 2f;
            Texture = texture;
        }

        //Methods
        public override void Update()
        {
            //animation continues through time elapsed
            Frame += (Speed * Time.ElapsedGameTime);
            Debug.WriteLine("Frame: " + Frame);
            i = (int)Math.Floor(Frame) % Frames;
            Debug.WriteLine("i: " + i);
            Source = new Rectangle(i*32,Source.Y,32, 32);
        }
    }
}
