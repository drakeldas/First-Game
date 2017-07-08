using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Game1
{
   
    public class Game1 : Game
    {
        const float HITBOXSCALE = .5f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D Tilemap;
        
        int fraimWidth = 0;
        bool spaceDown;
        bool gameStarted;
        bool gameOver;
        bool BallRun;
        bool Lifmob;
        float gravitySpeed;
        float playerSpeedX;
        float playerJumpY;
        float ballSpeedX;
        float ballCast;
        Sprite Player;
        Sprite Player2;
        Sprite Ball;
        Sprite Fon;
        Sprite Rip;
        Mobs Mob1;
        Mobs Mob2;
        int screenWidth=1500;
        int screenHeight=900;
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
            gravitySpeed = 0.05f;
            ballSpeedX = 800;
            gameOver = false;
            BallRun = false;
            Lifmob = true;
            base.Initialize();
        }
       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Fon = new Sprite(GraphicsDevice, "Content/fon.jpg", 2);
            Tilemap = Texture2D.FromStream(GraphicsDevice, File.Open("Content/tilemap.png", FileMode.Open));
            Player2 = new Sprite(GraphicsDevice, "Content/player.png", 1);
            Player = new Sprite(GraphicsDevice, "Content/player2.png", 1);
            Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
            Mob1 = new Mobs(GraphicsDevice, "Content/mob.png", 0.4f);
            Mob2 = new Mobs(GraphicsDevice, "Content/mob2.png", 0.4f);
            Rip = new Sprite(GraphicsDevice, "Content/rip.png", 0.4f);
            Player2.x = 30;
            Player2.y = 690;
            Player.x = 30;
            Player.y = 690;
            Mob1.firstx = 1200;
            Mob1.firsty = 605;
            Mob1.x = 1300;
            Mob1.y = 605;
            Mob1.dX = 200;
        }
        protected override void UnloadContent()
        {
            
        }
        
        protected override void Update(GameTime gameTime)
        {


            

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler();
            Player2.Update(elapsedTime);
            Ball.Update(elapsedTime);
            Mob1.Update(elapsedTime);

            void KeyboardHandler()
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
                        if (!BallRun)
                        {   
                            Ball.y = Player2.y;
                            BallRun = true;
                            Ball.dX = ballCast * ballSpeedX;
                        }
                    }

                    if (Math.Abs(Ball.x - Player2.x) > 400)
                    {
                        BallRun = false;
                        Ball.x = Player2.x;
                        Ball.y = 1000;
                    }
                    if (!keyboardState.IsKeyDown(Keys.F))
                    {
                    BallRun = false;
                    Ball.x = Player2.x;
                    Ball.y = 1000;
                    }

                if (Player2.y < 690)
                    {
                        Player2.dY += gravitySpeed * playerJumpY;
                    }
                if (keyboardState.IsKeyDown(Keys.Space))
                {

                    if (!spaceDown && Player2.y >= screenHeight * HITBOXSCALE - 1) { spaceDown = true; Player2.dY = -playerJumpY; }

                    
                }

                else { spaceDown = false;  if (Player2.y < 550 ) { Player2.dY = playerJumpY; } }


                if (Player2.y != 690) { spaceDown = true; }
                if (Player2.y == 691) { spaceDown = false; Player2.dY = 0; }



                    if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }

                    if (!gameStarted)
                    {
                        if (keyboardState.IsKeyDown(Keys.Space))
                        {
                            StartGame();
                            gameStarted = true;
                            spaceDown = true;
                        }
                        return;
                    }
                





            if (keyboardState.IsKeyDown(Keys.Left)) Player2.dX = -playerSpeedX;
                    else if (keyboardState.IsKeyDown(Keys.Right)) Player2.dX = playerSpeedX;
                    else Player2.dX = 0;

                    if (Player2.y > 690)
                    {
                        Player2.dY = 0;
                        Player2.y = 690;
                    }

                    if (Player2.x > screenWidth)
                    {
                        Player2.x = screenWidth - 60;
                        Player2.dX = 0;
                    }
                    if (Player2.x < 0)
                    {
                        Player2.x = 60;
                        Player2.dX = 0;
                    }
                if (gameOver && keyboardState.IsKeyDown(Keys.Enter))
                { 
                    StartGame();
                    gameOver = false;
                }
        }
            if (Mob1.RectangleCollision(Player2)) gameOver = true;
            if (Mob1.RectangleCollision(Player)) gameOver = true;
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

            if (keyboardState.IsKeyDown(Keys.A) || ballCast==-1)
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
            

            Mob1.framX = 0;
            Mob1.framY = 0;
            Mob1.framDx = 894;
            Mob1.framDy = 894;

            if (!Mob1.RectangleCollision(Ball) && Lifmob)
            {
                Mob1.Draw(spriteBatch); Lifmob = true;
            }
            if(Mob1.RectangleCollision(Ball))
            {
                Lifmob = false;
                Rip.x = Mob1.x;
                Rip.y = Mob1.y+90;
                Rip.Draw(spriteBatch);
            }
            if(!Lifmob)
            {
                Rip.x = Rip.x;
                Rip.y = Rip.y;
                Rip.Draw(spriteBatch);
            }



            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public void StartGame()
        {
            Player2.x = 30;
            Player2.y = 690;
        }
        

    }
}
