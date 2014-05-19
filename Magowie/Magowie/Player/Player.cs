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
using Magowie.Animation;
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
        BoundingSphere sphere;

        AnimationPlayer animationPlayer;




        public Player(Cam camera)
        {
            this.camera = camera;
        }

        public void Initialize()
        {
            playerWorld = Matrix.Identity;
        }

        public virtual void LoadContent(ContentManager content)
        
        {
            this.content = content;
            model = content.Load<Model>(@"Models\ArsaMagiAlpha(Resampled)");

            SkinningData skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
        }

        public void Update(GameTime gameTime)
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

            if (keyBoardState.IsKeyDown(Keys.W))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Forward * 4);
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            if (keyBoardState.IsKeyDown(Keys.S))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Backward * 4);
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            if (keyBoardState.IsKeyDown(Keys.A))
            {
                playerWorld *= Matrix.CreateTranslation(-playerWorld.Right * 4);
            }
            if (keyBoardState.IsKeyDown(Keys.D))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Right * 4);
            }
    

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
            sphere.Center = position;
            sphere.Radius *= scale.X/2;
            //Console.WriteLine(scale.X);
            return sphere;
        }

        public bool collisionCheck(BoundingSphere sphere1)
        {
            sphere = getBoundingSphere();
            if (sphere1.Intersects(sphere))
            {
                Console.WriteLine("KOLIZJA");
                return true;
            }
            else
            {
                //Console.WriteLine("NIEMA KOLIZJI");
                return false;
            }
        }

        public bool collisionCheck(BoundingBox box)
        {
            sphere = getBoundingSphere();
            if (box.Intersects(sphere))
            {
                Console.WriteLine("KOLIZJA");
                return true;
            }
            else
            {
                //Console.WriteLine("NIEMA KOLIZJI");
                return false;
            }
        }


        public void UpdateKolizja(GameTime gameTime)
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

            playerWorld *= Matrix.CreateTranslation(playerWorld.Backward * 4);
            _prevkeyBoardState = keyBoardState;
            _prevMouseState = st;
        }




    }
}
