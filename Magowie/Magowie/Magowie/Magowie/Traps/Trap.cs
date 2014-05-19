using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Magowie.Traps
{
    class Trap
    {
        protected float x;
        protected float y;
        protected float z;
        protected float aspectRatio;
        Model model;

        public Trap(float x1, float y1, float z1)
        {
            this.x = x1;
            this.y = y1;
            this.z = z1;
        }

        public virtual void LoadContent(SpriteBatch spritebatch, Model model, float aspectRatio)
        {
            this.model = model;
            this.aspectRatio = aspectRatio;
        }


        public virtual void Update(GameTime gameTime)
        {
        
        }

        public virtual void Draw(GameTime gameTime)
        {

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateTranslation(x, y, z);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(20.0f), aspectRatio,
                    1.0f, 50.0f);
                }

                mesh.Draw();
            }


        }
    }
}
