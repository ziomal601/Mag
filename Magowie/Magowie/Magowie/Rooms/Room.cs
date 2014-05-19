using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Magowie.Camera;
using Magowie.Creatures;
using Magowie.Objekty;
using Magowie.BoudnigDraw;
namespace Magowie.Rooms
{
    class Room
    {
        Model model;
        ContentManager content;
        protected Matrix World;
        KeyboardState keyBoardState;
        public BoundingBox completeCityBox { get; set; }
        Matrix viewMatrix;
        Vector3 modelPosition = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 10.0f, 20.0f);
        List<Monsters> stworzenia = new List<Monsters>();
        List<parts> parts = new List<parts>();
        BoundingSphere playersphere = new BoundingSphere();
        //Player player;
        public Room()
        {
            stworzenia.Add(new Imp());

            stworzenia.Add(new Manfish());
            //  stworzenia.Add(new Book());


            parts.Add(new fountain());
            parts.Add(new Sciany(1));
            parts.Add(new Sciany(2));
            parts.Add(new Sciany(3));
            parts.Add(new Sciany(4));
            parts.Add(new livefence1());
            parts.Add(new livefence1());
            parts.Add(new livefence1());
            parts.Add(new livefence1());

            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());

            parts.Add(new Doors());
            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new Sciany(5));

            parts.Add(new Doors());
            parts.Add(new Doors());
            parts.Add(new Doors());

            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());
            parts.Add(new livefence2());

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
            //create floor
            Vector3[] boundaryPoints = new Vector3[2];
            boundaryPoints[0] = new Vector3(-1203, -1450, -720);
            boundaryPoints[1] = new Vector3(1720, 1360, -721f);
            completeCityBox = BoundingBox.CreateFromPoints(boundaryPoints);
        }
        public void UnloadContent(ContentManager content)
        {
            content.Unload();
        }

        public void Update(GameTime gameTime, BoundingSphere sphere)
        {
            //for (int i = 0; i < stworzenia.Count; i++)
            this.playersphere = sphere;
            {
                stworzenia[1].Update(gameTime);
            }
            stworzenia[0].Update(gameTime, sphere);

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

        public void Initialize(GraphicsDevice graphicsDevice)
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

            parts[0].Position(200, 0);
            parts[5].Position(-1120, 1360);
            parts[6].Position(1720, -1450);
            parts[7].Position(1720, 1360);
            parts[8].Position(-1120, -1450);
            parts[9].Position(-650, 1360);
            parts[10].Position(1260, 1360);
            parts[11].Position(-650, -1450);
            parts[12].Position(1260, -1450);
            parts[13].Position(-1203, -50);
            parts[14].Position(0, -1450);
            parts[15].Position(550, -1450);
            parts[16].Position(-100, 1360);
            parts[17].Position(550, 1360);
            parts[21].Position(1800, -50);
            parts[21].Rotate(MathHelper.Pi);
            parts[19].Position(297, -1550);
            parts[19].Rotate(0.5f * MathHelper.Pi);
            parts[20].Position(297, 1450);
            parts[20].Rotate(1.5f * MathHelper.Pi);
            World = Matrix.Identity;
            parts[22].Position(1700, 920);
            parts[22].Rotate(0.5f * MathHelper.Pi);
            parts[23].Position(1700, 300);
            parts[23].Rotate(0.5f * MathHelper.Pi);
            parts[24].Position(1700, -420);
            parts[24].Rotate(0.5f * MathHelper.Pi);
            parts[25].Position(1700, -1020);
            parts[25].Rotate(0.5f * MathHelper.Pi);
            //1700 1430 (bez rogu, trzeba dodac troche do 1 zmiennej )
            //modelMin = new Vector3(-1471, -10, -1800);
            //modelMax = new Vector3(-1470, 10, 1200);
            //-1100 1430 (bez rogu, trzeba odjac troche od 1 zmiennej )


            DebugShapeRender.Initialize(graphicsDevice);

        }

        public void Draw(GameTime gameTime, Cam camera)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true; // Turn on the lighting subsystem. 
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.12f, 0.12f, 0.12f);
                    effect.DirectionalLight0.Direction = new Vector3(4, -5f, -4);
                    effect.DirectionalLight0.SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f);

                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * World;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
            DebugShapeRender.AddBoundingSphere(playersphere, Color.Blue);
            for (int i = 0; i < stworzenia.Count; i++)
            {
                stworzenia[i].Draw(camera);
                DebugShapeRender.AddBoundingSphere(stworzenia[i].getBoundingSphere(), Color.Red);
            }
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].Draw(gameTime, camera);
                DebugShapeRender.AddBoundingSphere(parts[i].getBoundingSphere(), Color.Green);
                DebugShapeRender.AddBoundingBox(parts[i].CalculateBoundingBox(), Color.Yellow);
            }

            DebugShapeRender.Draw(gameTime, camera.viewMatrix, camera.projectionMatrix);

            //BoundingSphere sphere = parts[0].getsphere;
            //DebugShapeRender.AddBoundingSphere(,Color.Red);
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
