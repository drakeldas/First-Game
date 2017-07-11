﻿using Microsoft.Xna.Framework;
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
        Texture2D startGameSplash;
        Sprite Tilemap;
        int k;
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
        int MobSpeed;
        Sprite Player;
        Players Player2;
        Sprite Ball;
        Sprite Fon;
        Sprite Rip;
        Mobs Mob1;
        Mobs Mob2;
        SpriteFont scoreFont;
        SpriteFont stateFont;
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
            k = 0;
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
            MobSpeed = 300;
            base.Initialize();
        }
       
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Fon = new Sprite(GraphicsDevice, "Content/fon.jpg", 2);
            Tilemap = new Sprite(GraphicsDevice, "Content/tilemap.png", 1);
            Player2 = new Players(GraphicsDevice, "Content/player.png", 1);
            Player = new Sprite(GraphicsDevice, "Content/player2.png", 1);
            Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
            Mob1 = new Mobs(GraphicsDevice, "Content/mob.png", 0.4f);
            Mob2 = new Mobs(GraphicsDevice, "Content/mob2.png", 0.4f);
            Rip = new Sprite(GraphicsDevice, "Content/rip.png", 0.4f);

            startGameSplash = Texture2D.FromStream(GraphicsDevice, File.Open("Content/start-splash.png", FileMode.Open));
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");

            Player2.x = 30;
            Player2.y = 690;
            Player.x = 30;
            Player.y = 690;
            Mob1.firstx = 1500;
            Mob1.firsty = 605;
            Mob1.x = 1300;
            Mob1.y = 605;
            Mob1.dX = MobSpeed;
            Mob2.firstx = 0;
            Mob2.firsty = 605;
            Mob2.x = 0;
            Mob2.y = 605;
            Mob2.dX = MobSpeed;
            Player2.lvl = 1;
            Player2.health = 100 * Player2.lvl;
            Player2.exp = 0;
        }
        protected override void UnloadContent()
        {
            
        }
        
        protected override void Update(GameTime gameTime)
        {

            if (Player2.exp == 10*Player2.lvl)
            {
                Player2.exp = 0; Player2.lvl++; Mob2.dX += MobSpeed / 5; Mob1.dX += MobSpeed / 5;
            }
            if (Player2.exp > 10 * Player2.lvl) { Player2.exp = 0; }


            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();
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
            if (gameOver && keyboardState.IsKeyDown(Keys.Enter))
            {
                Mob2.x = 0;
                Mob1.x = 1500;
                StartGame();
                gameOver = false;
                
            }



            if (gameStarted) { 
            
                if (!gameOver)
                { 
                    Player2.Update(elapsedTime);
                    KeyboardHandler();
                }
               

            Ball.Update(elapsedTime);
                if (Lifmob)
                {
                    Mob1.Update(elapsedTime, Player2);
                }
                if(!Lifmob) Mob2.Update(elapsedTime, Player2);

            void KeyboardHandler()
                {



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
                            Ball.firstx = Player2.x;
                            Ball.y = Player2.y;
                            BallRun = true;
                            Ball.dX = ballCast * ballSpeedX;
                        }
                    }

                    if (Math.Abs(Ball.firstx - Ball.x) > 400 || !keyboardState.IsKeyDown(Keys.F))
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

                    else { spaceDown = false; if (Player2.y < 550) { Player2.dY = playerJumpY; } }


                    if (Player2.y != 690) { spaceDown = true; }
                    if (Player2.y == 691) { spaceDown = false; Player2.dY = 0; }









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
                }
        }
            if (Player2.RectangleCollision(Mob1)) { gameOver = true; }

            if (Player2.RectangleCollision(Mob1)) { gameOver = true; }
            if (Player2.health < 0) { gameOver = true; Player2.health = 100 * Player2.lvl; }
            base.Update(gameTime);
            
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var keyboardState = Keyboard.GetState();
            
            spriteBatch.Begin();
            Fon.Draw(spriteBatch);

            
            
            for (int i = 0; i < 8; i++)
            {
                Tilemap.framX = 256;
                Tilemap.framY = 0;
                Tilemap.framDx = 256;
                Tilemap.framDy = 256;
                Tilemap.x = i * 256;
                Tilemap.y = 920;
                Tilemap.Draw(spriteBatch);
            }
            if (!gameOver)
            {
                if (keyboardState.IsKeyDown(Keys.A) || ballCast == -1)
                {
                    Player.x = Player2.x;
                    Player.y = Player2.y;
                    Player.Draw(spriteBatch);
                }
                else
                {
                    Player2.Draw(spriteBatch);
                }
            }
            if (gameOver)
            {
                Rip.x = Player2.x;
                Rip.y = Player2.y;
                Rip.Draw(spriteBatch);
            }


            if (BallRun)
            {
               Ball.Draw(spriteBatch);
            }
            

            

            if (!Mob1.RectangleCollision(Ball) && Lifmob)
            {
                Mob1.Draw(spriteBatch); 
            }
            if(Mob1.RectangleCollision(Ball))
            {
                Lifmob = false;
                Mob1.x = Mob1.firstx;
                Player2.exp++;
                
            }
            if (!Mob2.RectangleCollision(Ball) && !Lifmob)
            {
                Mob2.Draw(spriteBatch);
            }
            if (Mob2.RectangleCollision(Ball))
            {
                Lifmob = true;
                Mob2.x = Mob2.firstx;
                Player2.exp++;
            }

            if (Player2.exp > 10 * Player2.lvl) { Player2.exp = 0; }

            if (!gameStarted)
            {

                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

                String title = "First-Game 2D";
                String pressSpace = "Press Space to start";


                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);


                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3),Color.Red);
                spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2,screenHeight / 2), Color.White);
            }
            if (gameStarted)
            {

                String title=Player2.lvl.ToString()+"lvl";
                String title2 = "press the button F to attack";
                spriteBatch.DrawString(stateFont, title, new Vector2(0,0), Color.Red);
                
                spriteBatch.DrawString(stateFont, title2, new Vector2(0, 100), Color.Red, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);
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
