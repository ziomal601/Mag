using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Magowie.Camera;
using Magowie.Creatures;
using Magowie.Objekty;
namespace Magowie.Rooms
{
    class Room

    {
        Model model;
        ContentManager content;
        protected Matrix World;
        KeyboardState keyBoardState;
        Matrix viewMatrix;
        Vector3 modelPosition = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 10.0f, 20.0f);
        List<Monsters> stworzenia = new List<Monsters>();
        //Player player;
        public Room()
        {
            stworzenia.Add(new Manfish());
            stworzenia.Add(new Imp());
            stworzenia.Add(new Book());
        }

        public void LoadContent(ContentManager content1)
        {
            viewMatrix = Matrix.Identity;
            this.content = content1;
            model = content.Load<Model>(@"Models\Room-Fountain-raw-v2");
            
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].LoadContent(content1);
            }
            //player.LoadContent(content1);
        }
        public void UnloadContent(ContentManager content)
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].Update(gameTime);
            }
            //bool kolizja = false;
            //for (int i = 0; i < stworzenia.Count; i++)
            //{
            //    if (player.collisionCheck(stworzenia[i].getBoundingSphere()) == true)
            //    {
            //        kolizja = true;
            //        break;
            //    }
            //}

            //if (kolizja == false)
            //{
            //    player.Update(gameTime);
            //}
            //else
            //{
            //    player.UpdateKolizja(gameTime);
            //}
            
        }

        public void Initialize()
        {
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].Initialize();
            }
            //player.Initialize();
            stworzenia[0].Position(-15f, -550f);
            stworzenia[1].Position(750f, -100f);
           stworzenia[2].Position(-15f, 550f);
            World = Matrix.Identity;
            
           
        }

        public void Draw(Cam camera)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * World;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].Draw(camera);
            }
            //player.Draw();
        }

        public void Position(float s1, float s2)
        {

            World *= Matrix.CreateTranslation(World.Forward * s1);
            World *= Matrix.CreateTranslation(World.Right * s2);

        }
        public List<Monsters> GetStworzenia()
        {

            return stworzenia;
        }


    }
}
