﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

namespace CPI311.GameEngine
{
    public class Material
    {
        public Effect effect;
        public Texture2D Texture;
        public int Passes { get { return effect.CurrentTechnique.Passes.Count; } }
        public int CurrentTechnique { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Specular {  get; set; }
        public float Shininess { get; set; }
        public Matrix World { get; set; }
        public Camera Camera { get; set; }

        public Material(Matrix world, Camera camera, ContentManager content, string filename, int currentTechnique, float shininess, Texture2D texture)
        {
            effect = content.Load<Effect>(filename);
            World = world;
            Camera = camera;
            CurrentTechnique = currentTechnique;
            Shininess = shininess;
            Texture = texture;
            Diffuse = Color.Gray.ToVector3();
            Ambient = Color.Gray.ToVector3();
            Specular = Color.Gray.ToVector3();
        }
        public virtual void Apply(int currentPass)
        {
            effect.CurrentTechnique = effect.Techniques[CurrentTechnique];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(Camera.View);
            effect.Parameters["Projection"].SetValue(Camera.Projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
            Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(Camera.Transform.Position);
            effect.Parameters["Shininess"].SetValue(Shininess);
            effect.Parameters["AmbientColor"].SetValue(Ambient);
            effect.Parameters["DiffuseColor"].SetValue(Diffuse);
            effect.Parameters["SpecularColor"].SetValue(Specular);
            effect.Parameters["DiffuseTexture"].SetValue(Texture);
            effect.CurrentTechnique.Passes[currentPass].Apply();
        }
    }
}
