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
    public class GUIElement
    {
        //Properties
        public delegate void EventHandler(GUIElement sender);
        public event EventHandler Action;
        protected void OnAction()
        {
            if (Action != null) Action(this);// Any method is not specified yet
        }
        public Rectangle Bounds { get; set; }
        public Texture2D Texture { get; set; }
        public String Text { get; set; }
        public bool Selected { get; set; }

        //Update
        public virtual void Update()
        {
            //keepin it blank
        }

        //Draw
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if(Texture != null)
            {
                spriteBatch.Draw(Texture, Bounds, Selected ? Color.Yellow : Color.White);
            }
        }
    }
}
