using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Magowie.Objekty;
using Magowie.Creatures;
using Magowie.Rooms;
using Magowie.Camera;
namespace Magowie
{
    public class Render : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float aspectRatio;
        Player player;
        Room room;
        Matrix World;
        private int newWidth = 0;
        private int newHeight = 0;
        Cam camera = new Cam();
        //private int width, height;
        //GraphicsDevice device;
     
        KeyboardState keyState;
       
        public Render()
        {
            
            room = new Room();
         
            player = new Player(camera);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

      
        protected override void Initialize()
        {
          
            World = Matrix.Identity;

            room.Initialize(GraphicsDevice);
            room.Position(300f, 50f);
            
            player.Initialize();
            base.Initialize();
            this.IsMouseVisible = false;
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            //device = GraphicsDevice;
            //width = device.Viewport.Width / 2;
            //height = device.Viewport.Height / 2;
            //camera.LoadContent(width, height);

            room.LoadContent(Content);

            player.LoadContent(Content);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
           
            room.Update(gameTime, player.getBoundingSphere());
            List<Monsters> stworzenia = room.GetStworzenia();
            List<parts> parts = room.GetParts();
            List<BoundingSphere> sfery = new List<BoundingSphere>();
            List<BoundingBox> boxy = new List<BoundingBox>();
            for (int i = 0; i < stworzenia.Count; i++)
            {
                //if (player.collisionCheck(stworzenia[0].getBoundingSphere()) == true)
                //{
                //    kolizja = true;
                //    break;
                //}
                //if (player.collisionCheck(stworzenia[1].getBoundingSphere()) == true)
                //{
                //    kolizja = true;
                //    break;
                //}
                //if (player.collisionCheck(stworzenia[2].CalculateBoundingBox()) == true)
                //{
                //    kolizja = true;
                //    break;
                //}
                sfery.Add(stworzenia[i].getBoundingSphere());
            }
            sfery.Add(parts[0].getBoundingSphere());

            boxy.Add(parts[1].CalculateBoundingBox());
            boxy.Add(parts[2].CalculateBoundingBox());
            boxy.Add(parts[3].CalculateBoundingBox());
            boxy.Add(parts[4].CalculateBoundingBox());

                player.Update(gameTime, sfery,boxy,room.completeCityBox);
            
            
        
        }
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            ToggleFullScreen();
            GraphicsDevice device = graphics.GraphicsDevice;
            room.Draw(gameTime, camera);
            
           
            player.Draw();

        }
        private void ToggleFullScreen()
        {
            

            newWidth = GraphicsDevice.DisplayMode.Width;
            newHeight = GraphicsDevice.DisplayMode.Height;

            graphics.PreferredBackBufferWidth = newWidth;
            graphics.PreferredBackBufferHeight = newHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            camera.setCam(newWidth, newHeight);
        }
     

    }
}
