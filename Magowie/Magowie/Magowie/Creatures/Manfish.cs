using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Magowie.Camera;
using Magowie.BoudnigDraw;
using SkinnedModelPipeline;
namespace Magowie.Creatures
{
    class Manfish : Monsters
    {
        Model model;

        ContentManager content;
        BoundingSphere sphere;
        AnimationPlayer animationPlayer;
        AnimationPlayer.AnimationClip currentclip;
        AnimationPlayer.AnimationClip Idle;
        AnimationPlayer.AnimationClip Walk;
        AnimationPlayer.AnimationClip AttcInit;
        public Manfish()
        {
        }

        public override void LoadContent(ContentManager content1)
        {
            this.content = content1;
            model = content.Load<Model>(@"Models\DasRyba1");
            AnimationPlayer.SkinningData skinningData = model.Tag as AnimationPlayer.SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            animationPlayer = new AnimationPlayer(skinningData);

            Walk = skinningData.AnimationClips["Walk"];
            Idle = skinningData.AnimationClips["Idle"];
            AttcInit = skinningData.AnimationClips["FishDies"];
            currentclip = Walk;
            animationPlayer.StartClip(currentclip);
        }

        public override void Initialize()
        {
            World = Matrix.Identity;
            World *= Matrix.CreateTranslation(World.Up * 50f);
        }

        public override void Draw(Cam camera)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
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
            BoundingSphere sferaPotwora = getBoundingSphere();
            Matrix macierzKolizji = World;
            bool czyKolizja = false;
            macierzKolizji *= Matrix.CreateTranslation(World.Forward * 0.1f);
            Vector3 position;
            Vector3 scale;
            Quaternion rotation;

            macierzKolizji.Decompose(out scale, out rotation, out position);
            position.Y += 155;
            sferaPotwora.Center = position;
            if (collisionCheck(sferaPotwora, sphere))
            {

                czyKolizja = true;
            }
            if (!czyKolizja)
            {
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                // World *= Matrix.CreateTranslation(World.Backward * 0.1f);
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
            position.Y += 55;
            position.Y -= 15;
            position.X -= 15;
            sphere.Center = position;
            sphere.Radius *= scale.X / 2;
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
