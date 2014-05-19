using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Magowie.Camera;
namespace Magowie.Creatures
{
    class Imp : Monsters
    {
        Model model;
        ContentManager content;
        BoundingSphere sphere;
        private int ruch;
        private int temp;
        public Imp()
        {
        }

        public override void LoadContent(ContentManager content1)
        {
            this.content = content1;
            model = content.Load<Model>(@"Models\corv");
            World *= Matrix.CreateTranslation(World.Up * 15);

        }
        public override void Initialize()
        {
            World = Matrix.Identity;
            temp = 0;
            ruch = 1;
        }


        public override void Draw(Cam camera)
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

        }

        public override void Update(GameTime gameTime, BoundingSphere sphere)
        {
            BoundingSphere sferaPotwora = getBoundingSphere();
            Matrix macierzKolizji = World;
            bool czyKolizja = false;

            if (temp == 450)
            {
                temp = 0;
                ruch = -ruch;
            }
            else
            {

                if (ruch == 1)
                {
                    macierzKolizji *= Matrix.CreateTranslation(World.Forward * 1);
                    Vector3 position;
                    Vector3 scale;
                    Quaternion rotation;
                    macierzKolizji.Decompose(out scale, out rotation, out position);
                    sferaPotwora.Center = position;
                    if (collisionCheck(sferaPotwora, sphere))
                    {
                        czyKolizja = true;
                    }
                    if (!czyKolizja)
                    {
                        World *= Matrix.CreateTranslation(World.Forward * 1);
                        temp++;
                    }
                }
                else
                {

                    macierzKolizji *= Matrix.CreateTranslation(World.Backward * 1);
                    Vector3 position;
                    Vector3 scale;
                    Quaternion rotation;
                    macierzKolizji.Decompose(out scale, out rotation, out position);
                    sferaPotwora.Center = position;
                    if (collisionCheck(sferaPotwora, sphere))
                    {
                        czyKolizja = true;
                    }
                    if (!czyKolizja)
                    {
                        World *= Matrix.CreateTranslation(World.Backward * 1);
                        temp++;
                    }

                }
            }

        }

        override public BoundingSphere getBoundingSphere()
        {
            sphere = new BoundingSphere();

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }
            Vector3 position;
            Vector3 scale;
            Quaternion rotation;
            World.Decompose(out scale, out rotation, out position);
            sphere.Center = position;
            position.Y -= 15;
            position.X -= 15;
            sphere.Radius *= scale.X / 5;
            //Console.WriteLine("zwracam sfere ryboczleka");
            return sphere;
        }
        //public void UnloadContent(ContentManager content)
        //{
        //    content.Unload();
        //}


        public bool collisionCheck(BoundingSphere sphere1, BoundingSphere sphere2)
        {
            if (sphere1.Intersects(sphere2))
            {

                return true;
            }
            else
            {

                return false;
            }
        }

    }
}
