using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Magowie.Camera;

namespace Magowie.Creatures
{
    class Monsters 
    {
        protected Matrix World;
        public  Monsters ()
        {
        }

        public virtual void LoadContent(ContentManager content1)
        {
        }


        public virtual void Update(GameTime gameTime)
        {
        
        }
        public virtual void Update(GameTime gameTime,BoundingSphere sphere)
        {

        }


        public virtual void Initialize()
        {
           
        }

        public virtual void Draw(Cam camera)
        {

        }


        public  void Position (float s1, float s2)
        {

            World *= Matrix.CreateTranslation(World.Forward * s1);
            World *= Matrix.CreateTranslation(-World.Right * s2);

        }

        virtual public BoundingSphere getBoundingSphere()
        {

            return new BoundingSphere();
        }
       
    }
}
