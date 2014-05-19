using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Magowie;

namespace Magowie.Camera
{
    class Cam
    {
        private Vector3 position;
        private Vector3 target;
        public Matrix viewMatrix, projectionMatrix;
        private float yaw, pitch, roll;
        private float speed;
        private Matrix cameraRotation;
        private Matrix _prevcameraRotation;
        private Vector3 desiredPosition;
        private Vector3 desiredTarget;
        private Vector3 offsetDistance;
        private MouseState _prevMouseState;
        private int newWidth, newHeight;
        //private int centerX = newWidth/2;
       // private int centerY = newHeight/2;

        public enum CameraMode
        {
            free = 0,
            chase = 1
        }
        public CameraMode currentCameraMode = CameraMode.chase;

        public Cam()
        {
          
            ResetCamera();
        }

        //public void LoadContent(int width, int height)
        //{
        //    this.width = width;
        //    this.height = height;
        //}
        public void setCam(int newWidth1, int newHeight1)
        {
            this.newWidth = newWidth1;
            this.newHeight = newHeight1;
         
        }

        public void ResetCamera()
        {
            position = new Vector3(0, 0, 100);
            target = new Vector3();

            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f), 16 / 9, .5f,5000f);
            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;
            cameraRotation = Matrix.Identity;
            speed = .3f;
            _prevcameraRotation = cameraRotation;
            
            desiredPosition = position;
            desiredTarget = target;

            offsetDistance = new Vector3(0, 200, 400);
        }

        public void Update(Matrix chasedObjectsWorld)
        {
            HandleInput();
            UpdateViewMatrix(chasedObjectsWorld);
           
        }

        private void HandleInput()
        {
            

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState st = Mouse.GetState();
            
            if (st != _prevMouseState)
            {
                if (currentCameraMode == CameraMode.chase)
                {
                    if (st.X < _prevMouseState.X)
                    {
                        yaw += .1f;
                    }
                    if (st.X > _prevMouseState.X)
                    {
                        yaw += -.1f;
                    }
                    if (keyboardState.IsKeyDown(Keys.V))
                    {
                        roll += -.02f;
                    }
                    if (keyboardState.IsKeyDown(Keys.B))
                    {
                        roll += .02f;
                    }
                }
                if (currentCameraMode == CameraMode.free)
                {
                    if (st.X < _prevMouseState.X)
                    {
                        yaw += .05f;
                    }
                    if (st.X > _prevMouseState.X)
                    {
                        yaw += -.05f;
                    }

                    if (keyboardState.IsKeyDown(Keys.I))
                    {
                        MoveCamera(_prevcameraRotation.Forward * 10);
                    }
                    if (keyboardState.IsKeyDown(Keys.K))
                    {
                        MoveCamera(-_prevcameraRotation.Forward * 10);
                    }
                    if (keyboardState.IsKeyDown(Keys.J))
                    {
                        MoveCamera(-_prevcameraRotation.Right * 10);
                    }
                    if (keyboardState.IsKeyDown(Keys.L))
                    {
                        MoveCamera(_prevcameraRotation.Right * 10);
                    }
                    if (keyboardState.IsKeyDown(Keys.U))
                    {
                        MoveCamera(_prevcameraRotation.Up * 4);
                    }
                    if (keyboardState.IsKeyDown(Keys.O))
                    {
                        MoveCamera(-_prevcameraRotation.Up * 4);
                    }
                }
                //Mouse.SetPosition(width, height);
            }
           // Mouse.SetPosition(centerX, centerY);

            if (st.X > newWidth-5 || st.X < 5)
                Mouse.SetPosition(newWidth / 2, newHeight / 2);
            _prevMouseState = st;
        }

        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }

        public void SwitchCameraMode()
        {
            ResetCamera();

            currentCameraMode++;

            if ((int)currentCameraMode > 1)
            {
                currentCameraMode = 0;
            }
        }

        private void UpdateViewMatrix(Matrix chasedObjectsWorld)
        {
           

            switch (currentCameraMode)
            {
                case CameraMode.free:
                    
                    _prevcameraRotation.Forward.Normalize();
                    _prevcameraRotation.Up.Normalize();
                    _prevcameraRotation.Right.Normalize();

                    _prevcameraRotation *= Matrix.CreateFromAxisAngle(_prevcameraRotation.Right, pitch);
                    _prevcameraRotation *= Matrix.CreateFromAxisAngle(_prevcameraRotation.Up, yaw);
                    _prevcameraRotation *= Matrix.CreateFromAxisAngle(_prevcameraRotation.Forward, roll);

                    yaw = 0.0f;
                    pitch = 0.0f;
                    roll = 0.0f;

                    target = position + _prevcameraRotation.Forward;


                    break;

                case CameraMode.chase:
                    cameraRotation.Forward.Normalize();
                    chasedObjectsWorld.Right.Normalize();
                    chasedObjectsWorld.Up.Normalize();

                    cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

                    desiredTarget = chasedObjectsWorld.Translation;
                    target = desiredTarget;
                    target += chasedObjectsWorld.Right * yaw;
                    target += chasedObjectsWorld.Up * pitch;

                    desiredPosition = Vector3.Transform(offsetDistance, cameraRotation);
                    desiredPosition += chasedObjectsWorld.Translation;
                    position = desiredPosition;

                    target = chasedObjectsWorld.Translation;

                    roll = MathHelper.SmoothStep(roll, 0f, .15f);

                    break;
            }
          viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
                

        }

    }
}
