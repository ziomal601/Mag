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
        List<parts> parts = new List<parts>();
        //Player player;
        public Room()
        {
            stworzenia.Add(new Imp());
            
            
            stworzenia.Add(new Book());
            //stworzenia.Add(new Manfish());

            parts.Add(new fountain());
            parts.Add(new Sciany(1));
            parts.Add(new Sciany(2));
            parts.Add(new Sciany(3));
            parts.Add(new Sciany(4));
            //parts.Add(new livefence1());
            //parts.Add(new livefence1());
            //parts.Add(new livefence1());
            //parts.Add(new livefence1());

            //parts.Add(new livefence2());
            //parts.Add(new livefence2());
            //parts.Add(new livefence2());
            //parts.Add(new livefence2());
        }

        public void LoadContent(ContentManager content1)
        {
            viewMatrix = Matrix.Identity;
            this.content = content1;
            model = content.Load<Model>(@"Models\Room");
            
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].LoadContent(content1);
            }
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].LoadContent(content1);
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
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].Initialize();
            }
            //player.Initialize();
            stworzenia[0].Position(-15f, -550f);
            stworzenia[1].Position(-800f, 530f);
            //stworzenia[2].Position(-15f, 550f);
          
            parts[0].Position(800, 800);
            //parts[1].Position(-250, 650);
            //parts[2].Position(-900, 250);
           
            World = Matrix.Identity;
            //1700 1430 (bez rogu, trzeba dodac troche do 1 zmiennej )
            //modelMin = new Vector3(-1471, -10, -1800);
            //modelMax = new Vector3(-1470, 10, 1200);
            //-1100 1430 (bez rogu, trzeba odjac troche od 1 zmiennej )
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
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].Draw(camera);
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
        public List<parts> GetParts()
        {

            return parts;
        }


    }
}
