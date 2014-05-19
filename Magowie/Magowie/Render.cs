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
        Model model;
        Room room;
        Matrix World;
        Cam camera = new Cam();
       
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
            
          

            room.Initialize();
            room.Position(300f, 50f);
            
            player.Initialize();
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;


           
            room.LoadContent(Content);

            player.LoadContent(Content);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            room.Update(gameTime);
            List<Monsters> stworzenia = room.GetStworzenia();
            bool kolizja = false;
            for (int i = 0; i < stworzenia.Count; i++)
            {
                if (player.collisionCheck(stworzenia[0].getBoundingSphere()) == true)
                {
                    kolizja = true;
                    break;
                }
                if (player.collisionCheck(stworzenia[1].getBoundingSphere()) == true)
                {
                    kolizja = true;
                    break;
                }
                if (player.collisionCheck(stworzenia[2].CalculateBoundingBox()) == true)
                {
                    kolizja = true;
                    break;
                }
            }

            if (kolizja == false)
            {
                player.Update(gameTime);
            }
            else
            {
                player.UpdateKolizja(gameTime);
            }
            
        
        }
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            ToggleFullScreen();
            GraphicsDevice device = graphics.GraphicsDevice;
            room.Draw(camera);
            
           
            player.Draw();

        }
        private void ToggleFullScreen()
        {
            int newWidth = 0;
            int newHeight = 0;

            newWidth = GraphicsDevice.DisplayMode.Width;
            newHeight = GraphicsDevice.DisplayMode.Height;

            graphics.PreferredBackBufferWidth = newWidth;
            graphics.PreferredBackBufferHeight = newHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
        }

    }
}
