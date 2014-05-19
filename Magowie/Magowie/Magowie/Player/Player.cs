using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Magowie.Objekty;
using Magowie.Camera;
using SkinnedModelPipeline;


namespace Magowie.Objekty
{
    class Player
    {
        Model model;
        Matrix playerWorld;
        private MouseState _prevMouseState;
        KeyboardState keyBoardState;
        KeyboardState _prevkeyBoardState;
        ContentManager content;
        Cam camera;
        public BoundingSphere sphere;

        AnimationPlayer animationPlayer;
        AnimationPlayer.AnimationClip currentclip;
        AnimationPlayer.AnimationClip Cast;
        AnimationPlayer.AnimationClip WalkFront;
        AnimationPlayer.AnimationClip Idle;
        AnimationPlayer.AnimationClip WalkBack;
        public Player(Cam camera)
        {
            this.camera = camera;
        }

        public void Initialize()
        {
            playerWorld = Matrix.Identity;
            playerWorld *= Matrix.CreateTranslation(playerWorld.Forward * -725);
            playerWorld *= Matrix.CreateTranslation(playerWorld.Up * 65);
        }

        public virtual void LoadContent(ContentManager content)
        {
            this.content = content;
            model = content.Load<Model>(@"Models\Export006");

            AnimationPlayer.SkinningData skinningData = model.Tag as AnimationPlayer.SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            animationPlayer = new AnimationPlayer(skinningData);

            Cast = skinningData.AnimationClips["Cast"];
            WalkFront = skinningData.AnimationClips["WalkFront"];
            WalkBack = skinningData.AnimationClips["WalkBack"];
            Idle = skinningData.AnimationClips["Idle"];
            currentclip = Cast;
            animationPlayer.StartClip(currentclip);
        }

        public void Update(GameTime gameTime, List<BoundingSphere> sfery, List<BoundingBox> boxy, BoundingBox completeCityBox)
        {
            keyBoardState = Keyboard.GetState();
            MouseState st = Mouse.GetState();
            camera.Update(playerWorld);



            if (keyBoardState.IsKeyDown(Keys.Space) && _prevkeyBoardState.IsKeyUp(Keys.Space))
            {
                camera.SwitchCameraMode();
            }
            if (camera.currentCameraMode == Cam.CameraMode.chase)
            {
                if (st.X < _prevMouseState.X)
                {
                    playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, .1f) * playerWorld;
                }
                if (st.X > _prevMouseState.X)
                {
                    playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, -.1f) * playerWorld;
                }

            }

            if (keyBoardState.IsKeyDown(Keys.X))
            {
                playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, .02f) * playerWorld;
            }
            if (keyBoardState.IsKeyDown(Keys.Z))
            {
                playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, -.02f) * playerWorld;
            }
            //////////////////////////////////////

            BoundingSphere sferaGracza = getBoundingSphere();
            //Console.WriteLine(sferaGracza.Center);
            Matrix macierzKolizji = playerWorld;
            bool czyKolizja = false;





            //if coitains floor
            if (completeCityBox.Contains(sferaGracza) != ContainmentType.Contains)
            {
                Vector3 pos = Matrix.Invert(playerWorld).Translation;
                Console.WriteLine("Gracz");
                Console.WriteLine(pos);
                Console.WriteLine("sfera");
                Console.WriteLine(sferaGracza.Center);
            }
            else
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Up * -2);
            }

            if (keyBoardState.IsKeyDown(Keys.W))
            {
                if (currentclip != WalkFront)
                {
                    currentclip = WalkFront;
                    animationPlayer.StartClip(currentclip);         //animacja
                }
                macierzKolizji *= Matrix.CreateTranslation(playerWorld.Forward * 4);
                Vector3 position;
                Vector3 scale;
                Quaternion rotation;
                macierzKolizji.Decompose(out scale, out rotation, out position);

                sferaGracza.Center = position;
                for (int i = 0; i < sfery.Count; i++)
                {
                    if (collisionCheck(sferaGracza, sfery[i]))
                    {
                        czyKolizja = true;
                    }
                }
                for (int i = 0; i < boxy.Count; i++)
                {
                    if (collisionCheck(sferaGracza, boxy[i]))                                  //kolizja
                    {
                        czyKolizja = true;
                    }
                }
                if (!czyKolizja)
                {
                    playerWorld *= Matrix.CreateTranslation(playerWorld.Forward * 4);
                    animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
            }
            if (keyBoardState.IsKeyDown(Keys.S))
            {
                if (currentclip != WalkBack)
                {
                    currentclip = WalkBack;
                    animationPlayer.StartClip(currentclip);
                }

                macierzKolizji *= Matrix.CreateTranslation(playerWorld.Backward * 4);
                Vector3 position;
                Vector3 scale;
                Quaternion rotation;
                macierzKolizji.Decompose(out scale, out rotation, out position);
                sferaGracza.Center = position;
                for (int i = 0; i < sfery.Count; i++)
                {
                    if (collisionCheck(sferaGracza, sfery[i]))
                    {
                        czyKolizja = true;
                    }
                }
                for (int i = 0; i < boxy.Count; i++)
                {
                    if (collisionCheck(sferaGracza, boxy[i]))
                    {
                        czyKolizja = true;
                    }
                }
                if (!czyKolizja)
                {
                    playerWorld *= Matrix.CreateTranslation(playerWorld.Backward * 4);
                    animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                }



            }
            if (keyBoardState.IsKeyDown(Keys.A))
            {
                if (currentclip != WalkFront)
                {
                    currentclip = WalkFront;
                    animationPlayer.StartClip(currentclip);         //animacja
                }
                macierzKolizji *= Matrix.CreateTranslation(-playerWorld.Right * 4);
                Vector3 position;
                Vector3 scale;
                Quaternion rotation;
                macierzKolizji.Decompose(out scale, out rotation, out position);
                sferaGracza.Center = position;
                for (int i = 0; i < sfery.Count; i++)
                {
                    if (collisionCheck(sferaGracza, sfery[i]))
                    {
                        czyKolizja = true;
                    }
                }
                for (int i = 0; i < boxy.Count; i++)
                {
                    if (collisionCheck(sferaGracza, boxy[i]))
                    {
                        czyKolizja = true;
                    }
                }
                if (!czyKolizja)
                {
                    playerWorld *= Matrix.CreateTranslation(-playerWorld.Right * 4);
                    animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                }


            }
            if (keyBoardState.IsKeyDown(Keys.M))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Up * 4);
            }
            if (keyBoardState.IsKeyDown(Keys.E))
            {
                if (currentclip != Cast)
                {
                    currentclip = Cast;
                    animationPlayer.StartClip(currentclip);         //animacja
                }
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            if (keyBoardState.IsKeyDown(Keys.D))
            {
                if (currentclip != WalkFront)
                {
                    currentclip = WalkFront;
                    animationPlayer.StartClip(currentclip);         //animacja
                }
                macierzKolizji *= Matrix.CreateTranslation(playerWorld.Right * 4);
                Vector3 position;
                Vector3 scale;
                Quaternion rotation;
                macierzKolizji.Decompose(out scale, out rotation, out position);
                sferaGracza.Center = position;
                for (int i = 0; i < sfery.Count; i++)
                {
                    if (collisionCheck(sferaGracza, sfery[i]))
                    {
                        czyKolizja = true;
                    }
                }
                for (int i = 0; i < boxy.Count; i++)
                {
                    if (collisionCheck(sferaGracza, boxy[i]))
                    {
                        czyKolizja = true;
                    }
                }
                if (!czyKolizja)
                {
                    playerWorld *= Matrix.CreateTranslation(playerWorld.Right * 4);
                    animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                }

            }
            if (keyBoardState.GetPressedKeys().Length == 0)
            {
                if (currentclip != Idle)
                {
                    currentclip = Idle;
                    animationPlayer.StartClip(currentclip);         //animacja
                    animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

                }

            }
            ////////////////////////////////
            _prevkeyBoardState = keyBoardState;
            _prevMouseState = st;
        }



        public void Draw()
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

                    effect.World = modelTransforms[mesh.ParentBone.Index] * playerWorld;

                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
                mesh.Draw();
            }
        }
        private bool KeyJustPressed(Keys key)
        {
            return keyBoardState.IsKeyDown(key) && _prevkeyBoardState.IsKeyUp(key);
        }

        public BoundingSphere getBoundingSphere()
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

            playerWorld.Decompose(out scale, out rotation, out position);
            position.Y -= 15;
            position.X -= 15;
            sphere.Center = position;
            sphere.Radius *= scale.X / 2;


            return sphere;
        }

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

        public bool collisionCheck(BoundingSphere sphere1, BoundingBox box)
        {
            if (box.Intersects(sphere1))
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
