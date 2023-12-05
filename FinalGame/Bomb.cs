using CPI311.GameEngine;
using FinalGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace FinalGame
{
    public class Bomb : GameObject
    {
        public AStarSearch search;
        public List<Vector3> path;

        private float speed = 5f; //moving speed
        private int gridSize = 20; //grid size
        private TerrainRenderer Terrain;
        Model model;
        Player player;
        public bool isActive;

        public Bomb(TerrainRenderer terrain, ContentManager Content, Camera camera,
            GraphicsDevice graphicsDevice, Light light, Player playr) : base()
        {
            isActive = true;
            model = Content.Load<Model>("Sphere");
            Terrain = terrain;
            path = null;
            player = playr;

            //Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            //Sphere Collider
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1;
            collider.Transform = Transform;
            Add<Collider>(collider);

            //Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(model, Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            Add<Renderer>(renderer);

            //*** Make a path
            search = new AStarSearch(gridSize, gridSize); //A start Search has the same grid size
            float gridW = Terrain.size.X / gridSize;
            float gridH = Terrain.size.Y / gridSize;

            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 pos = new Vector3(
                        gridW * i + gridW / 2 - Terrain.size.X / 2, //x position
                        0,
                        gridH * j + gridH / 2 - Terrain.size.Y / 2); //z position
                    if (Terrain.GetAltitude(pos) > 1.0)
                    {
                        search.Nodes[j, i].Passable = false;
                        Debug.WriteLine("return: " + Terrain.GetAltitude(pos));
                    }
                }
            /*for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    Debug.WriteLine("Is it false? search.Nodes[" + j + ", " + i + " = " + search.Nodes[j, i].Passable);*/

        }

        public override void Update()
        {
            if (path != null && path.Count > 0)
            {
                // Move to the destination along the path
                Vector3 currP = Transform.Position;
                Vector3 destP = GetGridPosition(path[0]);
                currP.Y = 0;
                destP.Y = 0;

                Vector3 direction = Vector3.Distance(currP, destP) == 0 ?
                    Vector3.Zero :
                    Vector3.Normalize(destP - currP);
                this.Rigidbody.Velocity = new Vector3(direction.X, 0, direction.Z) * speed;

                if (Vector3.Distance(currP, destP) < 1f) // if it reaches to a point, go to the next in path
                {
                    path.RemoveAt(0);
                    if (path.Count == 0) // if it reached to the goal
                    {
                        //path = null;
                        RandomPathFinding();
                        return;
                    }
                }
            }
            else
            {
                // Search again to make a new path
                RandomPathFinding();
                this.Transform.LocalPosition = GetGridPosition(path[0]); //move to start point
            }
            this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                Terrain.GetAltitude(this.Transform.LocalPosition),
                this.Transform.LocalPosition.Z) + Vector3.Up;
            Transform.Update();
            base.Update();
        }

        public Vector3 GetGridPosition(Vector3 gridPos)
        {
            float gridW = Terrain.size.X / search.Cols;
            float gridH = Terrain.size.Y / search.Rows;
            return new Vector3(
                gridW * gridPos.X + gridW / 2 - Terrain.size.X / 2,
                1,
                gridH * gridPos.Z + gridH / 2 - Terrain.size.Y / 2);
        }
        public void RandomPathFinding()
        {
            isActive = true;
            Random random = new Random();
            if (path == null)
            {
                while (!(search.Start = search.Nodes[random.Next(search.Rows), random.Next(search.Cols)]).Passable) ;
            }
            else
            {
                search.Start = search.End;
            }


            int xPlayer = (int)((player.Transform.LocalPosition.X + 45) / 5) % search.Rows + 1;
            int zPlayer = (int)((player.Transform.LocalPosition.Z + 45) / 5) % search.Cols + 1;
            search.End = search.Nodes[xPlayer, zPlayer]; //player position
            //Debug.WriteLine("curr player:" + search.End.Position);
            search.Search();
            path = new List<Vector3>();
            AStarNode current = search.End;
            var count = 0;
            while (current != null)
            {
                count++;
                path.Insert(0, current.Position);
                current = current.Parent;
            }

            //move from t
            Transform.LocalPosition = GetGridPosition(path[0]);
        }
    }
}
