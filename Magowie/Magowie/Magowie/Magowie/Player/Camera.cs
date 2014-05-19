using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Magowie.Objekty
{
    class Camera
    {
              private float yaw, pitch, roll;
private float speed;
private Matrix cameraRotation;
 
        private Vector3 position;
        private Vector3 target;
        public Matrix viewMatrix, projectionMatrix;
        public enum CameraMode
        {
            free = 0,
            chase = 1,
            orbit = 2
        }
        public CameraMode currentCameraMode = CameraMode.free;
        public Camera()
        {
            ResetCamera();
        }
    
        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }
        private Vector3 desiredPosition;
        private Vector3 desiredTarget;
        private Vector3 offsetDistance;

       
public void ResetCamera()
{
    desiredPosition = position;
    desiredTarget = target;

    offsetDistance = new Vector3(0, 0, 50);
    yaw = 0.0f;
    pitch = 0.0f;
    roll = 0.0f;
 
    speed = .3f;
 
    cameraRotation = Matrix.Identity;
}
public void Update()
{
    HandleInput();
}

private void HandleInput()
{
    KeyboardState keyboardState = Keyboard.GetState();

    if (keyboardState.IsKeyDown(Keys.J))
    {
        yaw += .02f;
    }
    if (keyboardState.IsKeyDown(Keys.L))
    {
        yaw += -.02f;
    }
    if (keyboardState.IsKeyDown(Keys.I))
    {
        pitch += -.02f;
    }
    if (keyboardState.IsKeyDown(Keys.K))
    {
        pitch += .02f;
    }
    if (keyboardState.IsKeyDown(Keys.U))
    {
        roll += -.02f;
    }
    if (keyboardState.IsKeyDown(Keys.O))
    {
        roll += .02f;
    }
    if (currentCameraMode == CameraMode.free)
    {
        if (keyboardState.IsKeyDown(Keys.W))
        {
            MoveCamera(cameraRotation.Forward);
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            MoveCamera(-cameraRotation.Forward);
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            MoveCamera(-cameraRotation.Right);
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            MoveCamera(cameraRotation.Right);
        }
        if (keyboardState.IsKeyDown(Keys.E))
        {
            MoveCamera(cameraRotation.Up);
        }
        if (keyboardState.IsKeyDown(Keys.Q))
        {
            MoveCamera(-cameraRotation.Up);
        }
    }
}
private void UpdateViewMatrix(Matrix chasedObjectsWorld)
        {
            switch (currentCameraMode)
            {
                case CameraMode.free:

                    //Free-camera code goes here.

                    break;

                case CameraMode.chase:

                    cameraRotation.Forward.Normalize();
                    chasedObjectsWorld.Right.Normalize();
                    chasedObjectsWorld.Up.Normalize();

                    cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

                    desiredTarget = chasedObjectsWorld.Translation;
                    target = desiredTarget;
                   target += chasedObjectsWorld.Right * yaw;
                   target += chasedObjectsWorld.Up * pitch;
                    desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);
                    position = Vector3.SmoothStep(position, desiredPosition, .15f);

                    yaw = MathHelper.SmoothStep(yaw, 0f, .1f);
                    pitch = MathHelper.SmoothStep(pitch, 0f, .1f);
                    roll = MathHelper.SmoothStep(roll, 0f, .2f);

                    break;

                case CameraMode.orbit:

                    break;
            }

            viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
            cameraRotation.Forward.Normalize();
            cameraRotation.Up.Normalize();
            cameraRotation.Right.Normalize();

            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, pitch);
            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, yaw);
            cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            target = position + cameraRotation.Forward;

            viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
        }
    }
}
