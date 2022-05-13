using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using GameArchitectureExample.Screens;
using GameArchitectureExample.StateManagement;

namespace FireworkClicker
{
    public class Game1 : Game, IParticleEmitter
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;

        MouseState _priorMouse;
        KeyboardState _priorKeyboard;

        Texture2D _mouseTexture;

        SoundEffect errorSound;

        SoundEffect fireworkExplosionSound;
        FireworkParticleSystem _fireworks;

        Song backgroundMusic;
        SpriteFont purposeFont;

        public Circles circle;

        public int score;
        public int highscore;
        bool shake = false;
        MainMenuScreen menuscrn = new MainMenuScreen();

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.IsFullScreen = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            
            //_screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(menuscrn, null);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            StarsParticleSystem stars = new StarsParticleSystem(this, new Rectangle(-10, -100, 800, 500));
            Components.Add(stars);

            _fireworks = new FireworkParticleSystem(this, 20);
            Components.Add(_fireworks);

            PixieParticleSystem pixie = new PixieParticleSystem(this, this);
            Components.Add(pixie);

            circle = new Circles(this, menuscrn);

            Components.Add(circle);

            score = 0;
            highscore = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            errorSound = this.Content.Load<SoundEffect>("error");
            fireworkExplosionSound = this.Content.Load<SoundEffect>("firework");
            backgroundMusic = this.Content.Load<Song>("renewal");
            //MediaPlayer.Play(backgroundMusic);
            purposeFont = Content.Load<SpriteFont>("purposeFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mouseTexture = this.Content.Load<Texture2D>("mouse");
            Mouse.SetCursor(MouseCursor.FromTexture2D(_mouseTexture, _mouseTexture.Width / 2, _mouseTexture.Height / 2));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState currentMouse = Mouse.GetState();
            KeyboardState currentKeyboard = Keyboard.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);
            var mouserect = new Rectangle(mousePosition.ToPoint(), mousePosition.ToPoint());

            circle.shrink(gameTime, score);

            if (circle._rectange.Contains(new Point(currentMouse.X, currentMouse.Y)))
            {
                circle.collision = true;
            }
            else
            {
                circle.collision = false;
                if (((currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released) || (currentKeyboard.IsKeyDown(Keys.Space) && _priorKeyboard.IsKeyUp(Keys.Space))))
                {
                    if (score > highscore)
                    {
                        highscore = score;
                    }
                    score = 0;
                    //add shake to score here
                    shake = true;
                    errorSound.Play(0.3f, 0, 0);
                    circle.updatePosition();
                }
                else
                {
                    shake = false;
                }
            }


            if (((currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released) || (currentKeyboard.IsKeyDown(Keys.Space) && _priorKeyboard.IsKeyUp(Keys.Space))) && mouserect.Intersects(circle._rectange))
            {

                for (int i = 0; i < 5; i++)
                {
                    _fireworks.PlaceFirework(mousePosition);
                }
                circle.updatePosition();
                fireworkExplosionSound.Play(0.3f, 0, 0);
                score++;
            }

            Velocity = mousePosition - Position;
            Position = mousePosition;

            _priorKeyboard = currentKeyboard;
            _priorMouse = currentMouse;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            Matrix tranformation = Matrix.CreateTranslation(RandomHelper.Next(-10,10), RandomHelper.Next(-10, 10), 0);

            GraphicsDevice.Clear(Color.Black);

            if (!menuscrn.IsActive)
            {
                if (shake)
                {
                    _spriteBatch.Begin(transformMatrix: tranformation);
                    _spriteBatch.DrawString(purposeFont, "Highcore: " + highscore.ToString() + "\nScore: " + score.ToString(), new Vector2(10, 10), Color.White);
                    _spriteBatch.End();
                }
                else
                {
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(purposeFont, "Highcore: " + highscore.ToString() + "\nScore: " + score.ToString(), new Vector2(10, 10), Color.White);
                    _spriteBatch.End();
                }
            }

            

            base.Draw(gameTime);
        }
    }
}
