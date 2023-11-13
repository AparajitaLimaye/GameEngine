using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public static class Time
    {
        public static float ElapsedGameTime { get; private set; }
        public static TimeSpan TotalGameTime { get; private set; }
        public static float Speed { get; set; }
        public static void Initialize()
        {
            ElapsedGameTime = 0;
            TotalGameTime = new TimeSpan(0);
            Speed = 1;
        }

        public static void Update(GameTime gameTime)
        {
            ElapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
            TotalGameTime = gameTime.TotalGameTime;
        }
    }
}
