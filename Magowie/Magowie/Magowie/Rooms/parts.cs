using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Magowie.Camera;
namespace Magowie.Rooms
{
    class parts
    {
       protected Model model;
       protected ContentManager content;
       protected Matrix World;
      
        public parts()
        {
           
        }

        public virtual void LoadContent(ContentManager content1)
        {
        }


        public virtual void Update(GameTime gameTime)
        {
        
        }
        public virtual void Initialize()
        {
            World = Matrix.Identity;
        }


        public virtual void Draw(GameTime gameTime, Cam camera)
        {

        }

        public void Rotate(float f)
        {
            World = Matrix.CreateFromAxisAngle(Vector3.Up, f) * World;
        }

        public void Position(float s1, float s2)
        {

            World *= Matrix.CreateTranslation(World.Forward * s1);
            World *= Matrix.CreateTranslation(-World.Right * s2);

        }

        virtual public BoundingBox CalculateBoundingBox()
        {
            return new BoundingBox();
        }
        virtual public BoundingSphere getBoundingSphere()
        {

            return new BoundingSphere();
        }

    }
}


