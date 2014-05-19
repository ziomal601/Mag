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
    class Gargoyle : Monsters
    {
        Model model;
        ContentManager content;
        Matrix mebleWorld;
        BoundingSphere sphere;

        public Gargoyle()
            : base()
        {
        }

        public override void LoadContent(ContentManager content1)
        {
            this.content = content1;
            model = content.Load<Model>(@"Models\ArsaMagiWalkInit");

        }
        public override void Initialize()
        {
            mebleWorld = Matrix.Identity;
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

        public override void Update(GameTime gameTime)
        {

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
            sphere.Radius *= 0.80f;
            //Console.WriteLine("zwracam sfere ryboczleka");
            return sphere;
        }


   
    }
}
