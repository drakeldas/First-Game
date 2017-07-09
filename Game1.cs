using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Game1.GameObjects;

namespace Game1
{
   
    public class Game1 : Game
    {
        const float HITBOXSCALE = .5f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Tilemap;
        private HashSet<GameObject> gameObjects;
        int fraimWidth = 0;
        bool spaceDown;
        bool gameStarted;
        float gravitySpeed;
        float playerSpeedX;
        float playerJumpY;
        float ballSpeedX;
        float ballCast;
        Sprite Player;
        Sprite Player2;
        Sprite Ball;
        Sprite Fon;
        int screenWidth=1500;
        int screenHeight=900;
        Song intro;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;  
            graphics.PreferredBackBufferHeight = screenHeight;  
        }

       
        protected override void Initialize()
        {
           
            this.IsMouseVisible = true;
            spaceDown = false;
            gameStarted = false;
            playerSpeedX = 800;
            playerJumpY = 1200;
            gravitySpeed = 500;
            ballSpeedX = 1200;
            base.Initialize();
        }
       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Fon = new Sprite(GraphicsDevice, "Content/fon.jpg", 2);
            Tilemap = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/tilemap.png"));
            Player2 = new Sprite(GraphicsDevice, "Content/player.png", 1);
            Player = new Sprite(GraphicsDevice, "Content/player.png", 1);
            Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
            this.intro = Content.Load<Song>("intro");
            MediaPlayer.Play(intro);

            var sheepSprite = new SpriteInfo
			{
				Texture = Content.Load<Texture2D>("barash"),
				FrameCount = 96,
				TimeToFrame = TimeSpan.FromMilliseconds(150),
				FrameWidth = 48,
				FrameHeight = 48,
				FramesInRow = 12,
			};

            gameObjects = new HashSet<GameObject>();
            gameObjects.Add(new Sheep(sheepSprite)
			{
				Position = new Vector2(1400, 790),
				Velocity = new Vector2(0.05f, 0f),
			});

            Player2.x = 30;
            Player2.y = 690;
            Player.x = 30;
            Player.y = 690;
        }
        protected override void UnloadContent()
        {
            
        }
        
        public void StartGame()
        {
            Player2.x = 30;
            Player2.y = 690;
            
        }

        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    gameStarted = true;
                    spaceDown = true;

                }
                return;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Player2.x -= playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                ballCast = -1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Player2.x += playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                ballCast = 1;
            }

            
            if (keyboardState.IsKeyDown(Keys.F))
            {
                Ball.y = Player2.y;
                Ball.x += ballCast*ballSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }




            if (Math.Abs(Ball.x - Player2.x) > 400 || Ball.y!=Player2.y)
            {
                Ball.x = Player2.x;
                Ball.y = Player2.y;
            }
            if (keyboardState.IsKeyDown(Keys.Space) && Player2.y > 550) 
            {
                Player2.y -= playerJumpY * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
            if (Player2.y < 690)
            {
                Player2.y += gravitySpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }

            foreach (var gameObject in gameObjects)
			{
				gameObject.Update(gameTime);
			}
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler(); 
            Player2.Update(elapsedTime);

         //if (state.IsKeyDown(Keys.Left)) Player2.dX = -playerSpeedX;
                //else if (state.IsKeyDown(Keys.Right)) Player2.dX = playerSpeedX;
            if (keyboardState.IsKeyDown(Keys.Left)) Player2.dX = -playerSpeedX;
                else if (keyboardState.IsKeyDown(Keys.Right)) Player2.dX = playerSpeedX;
                else Player2.dX = 0;
            


            if (Player2.y > screenHeight )
            {
                Player2.dY = 0;
                Player2.y = 690;
            }

            
            if (Player2.x > screenWidth)
            {
                Player2.x = screenWidth-60;
                Player2.dX = 0;
            }
            if (Player2.x < 0 )
            {
                Player2.x = 60;
                Player2.dX = 0;
            }
            base.Update(gameTime);
        }

            

                

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var keyboardState = Keyboard.GetState();
            
            spriteBatch.Begin();
            Fon.Draw(spriteBatch);

            
            var Tilmap1 = new Rectangle(256, 0, 256, 256);
            for (int i = 0; i < 6; i++)
            {
                fraimWidth = i * 256;
                spriteBatch.Draw(Tilemap, new Vector2(fraimWidth, 660), Tilmap1, Color.White);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Player.x = Player2.x;
                Player.y = Player2.y;
                Player.Draw(spriteBatch);
                
            }
            else
            {
                Player2.Draw(spriteBatch);
            }



            if (keyboardState.IsKeyDown(Keys.F))
            {
               Ball.Draw(spriteBatch);
            }
            if(!keyboardState.IsKeyDown(Keys.F))
            {
                Ball.x = Player2.x;
                Ball.y = Player2.y;
            }
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        
        


    }
}
