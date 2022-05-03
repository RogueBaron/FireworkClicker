using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FireworkClicker
{
    public class Circles: DrawableGameComponent
    {

        Game _game;

        public Boolean collision = false;

        public Vector2 _position;

        //this will be a circle eventually
        public Rectangle _rectange;

        protected static ContentManager contentManager;

        SpriteBatch spriteBatch;
        Texture2D whiteRectangle;

        int size = 100;

        public Circles(Game game) : base(game)
        {
            _game = game;
            //_position = position;
            _rectange = new Rectangle(RandomHelper.Next(40,540), RandomHelper.Next(40,380), size, size);

        }

        public void updatePosition()
        {
            _rectange = new Rectangle(RandomHelper.Next(40, 540), RandomHelper.Next(40, 380), size, size);
        }

        public void shrink(GameTime gameTime, int score)
        {
            

            if (_rectange.Width > 15)
            {
                if (gameTime.TotalGameTime.Milliseconds % 10 == 0)
                {
                    _rectange = new Rectangle(_rectange.X + 1, _rectange.Y + 1, _rectange.Width - 1, _rectange.Height - 1);
                }
            }
            else
            {
                this.updatePosition();
            }
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (contentManager == null) contentManager = new ContentManager(Game.Services, "Content");
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            whiteRectangle = contentManager.Load<Texture2D>("target");//new Texture2D(GraphicsDevice, 1, 1);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            whiteRectangle.Dispose();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            spriteBatch.Begin();

            // Option One (if you have integer size and coordinates)
            if (collision)
            {
                spriteBatch.Draw(whiteRectangle, _rectange, Color.LightGreen);
            }
            else
            {
                spriteBatch.Draw(whiteRectangle, _rectange, Color.Red);
            }

            spriteBatch.End();
        }
    }
}
