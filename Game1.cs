using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace WarWizard2D
{
   
    public class Game1 : Game
    {
        const float HITBOXSCALE = .5f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D startGameSplash;
        Sprite Tilemap;
        bool spaceDown; //прыжок
        bool gameStarted; // начало игры
        bool gameOver; // конец игры
        bool BallRun; //шар
        bool Lifmob; // жизнь моба
        double damage;
        Song intro;
        SoundEffect retry;
        SoundEffect lvlup;
        SoundEffect shot;
        int score;
        int highscore;

        float gravitySpeed; //гравитация
        float playerSpeedX; // скорость игрока
        float playerJumpY;// скорость прыжка
        float ballSpeedX;// скорость шара
        float ballCast;// поворот шара
        int MobSpeed;// скорость мобов
        Sprite Player;
        Players Player2;
        Sprite Ball;
        Sprite Fon;
        Sprite Rip;
        Mobs Mob1;
        Mobs Mob2;
        Texture2D Fire;
        Texture2D Player_1;
        Texture2D Player_2;
        SpriteFont scoreFont;
        SpriteFont stateFont;
        int screenWidth = 1500;
        int screenHeight = 900;
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
            this.spaceDown = false;
            this.gameStarted = false;
            this.playerSpeedX = 800;
            this.playerJumpY = 1200;
            this.gravitySpeed = 0.05f;
            this.ballSpeedX = 800;
            this.gameOver = false;
            this.BallRun = false;
            this.Lifmob = true;
            this.MobSpeed = 300;
            this.score = 0;
            this.highscore = 0;
            this.damage = 30;
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
            this.intro = Content.Load<Song>("intro");
            this.retry = Content.Load<SoundEffect>("retry");
            this.lvlup = Content.Load<SoundEffect>("lvlup");
            this.shot = Content.Load<SoundEffect>("shot");
            Fire = Content.Load<Texture2D>("fireball");
            Player_1 = Content.Load<Texture2D>("player_1");
            Player_2 = Content.Load<Texture2D>("player_2");
            startGameSplash = Texture2D.FromStream(GraphicsDevice, File.Open("Content/start-splash.png", FileMode.Open));
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            MediaPlayer.Play(intro);

            
            Mob1.FirstX = screenWidth+600; ;
            Mob1.FirstY = 605;
            Mob1.X = 1300;
            Mob1.Y = 605;
            Mob1.Dx = MobSpeed;
            Mob2.FirstX = -600;
            Mob2.FirstY = 605;
            Mob2.X = 0;
            Mob2.Y = 605;
            Mob2.Dx = MobSpeed;
            Player2.Lvl = 1;
            Player2.Health = 100 * Player2.Lvl;
            Player2.Exp = 0;
        }
        protected override void UnloadContent()
        {
            
        }
        
        protected override void Update(GameTime gameTime)
        {
            //смена спрайтов с уровнем
            if (Player2.Lvl >= 2) { Ball.Texture = Fire; }
            if (Player2.Lvl >= 3) { Player2.Texture = Player_2; Player.Texture = Player_1; Player.Scale = 0.2f; Player2.Scale = 0.2f; }
            //условие набора уровня
            if (Player2.Exp == 10 * Player2.Lvl)
            {
                Player2.Exp = 0; Player2.Lvl++; lvlup.Play(); Mob2.Dx += MobSpeed / 5; Mob1.Dx += MobSpeed / 5; damage += Player2.Lvl * 5;
            }
            if (Player2.Exp > 10 * Player2.Lvl) { Player2.Exp = 0; }


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
                Lifmob = true;
                Mob2.X = 0;
                Mob1.X = screenWidth; ;
                Mob1.Dx = MobSpeed;
                Mob2.Dx = MobSpeed;
                Player2 = new Players(GraphicsDevice, "Content/player.png", 1);
                Player = new Sprite(GraphicsDevice, "Content/player2.png", 1);
                Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
                Player2.Exp = 0;
                Player2.Lvl = 1;
                score = 0;
                retry.Play();
                StartGame();
                gameOver = false;
                
            }
            //обновление координат спрайтов
            if (gameStarted)
            {

                if (!gameOver)
                { 
                    Player2.Update(elapsedTime);
                    KeyboardHandler(Player2, gameTime);
                }
               

            Ball.Update(elapsedTime);
                if (Lifmob)
                {
                    Mob1.Update(elapsedTime, Player2);
                }
                if(!Lifmob) Mob2.Update(elapsedTime, Player2);

                
        }
            if (Player2.RectangleCollision(Mob1) && Lifmob) { gameOver = true; }
            if (Player2.RectangleCollision(Mob2) && !Lifmob) { gameOver = true; }
            if (Player2.Health < 0) { gameOver = true; Player2.Health = 100 * Player2.Lvl; }
            base.Update(gameTime);
            
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var keyboardState = Keyboard.GetState();
            
            spriteBatch.Begin();
            Fon.Draw(spriteBatch);

            //отрисовка нижней платформы
            for (int i = 0; i < 8; i++)
            {
                Tilemap.FramX = 256;
                Tilemap.FramY = 0;
                Tilemap.FramDx = 256;
                Tilemap.FramDy = 256;
                Tilemap.X = i * 256;
                Tilemap.Y = 920;
                Tilemap.Draw(spriteBatch);
            }
            //отрисовка персонажа
            if (!gameOver)
            {
                if (keyboardState.IsKeyDown(Keys.A) || ballCast == -1)
                {
                    Player.X = Player2.X;
                    Player.Y = Player2.Y;
                    Player.Draw(spriteBatch);
                }
                else
                {
                    Player2.Draw(spriteBatch);
                }
            }
            if (gameOver)
            {
                Rip.X = Player2.X;
                Rip.Y = Player2.Y;
                Rip.Draw(spriteBatch);
                String titleGameOver = "GAME OVER";
                String titleAgain = "Press Enter to start again";
                String titleExit = "Press Esc to exit";
                BallRun = false;
                Ball.Dx = 0;
                Ball.Y = 1000;
                spriteBatch.DrawString(stateFont, titleGameOver, new Vector2(500, 275), Color.Red, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleAgain, new Vector2(575, 390), Color.Red, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleExit, new Vector2(635, 425), Color.Red, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 1f);
            }

            //отрисовка спрайта шара
            if (BallRun)
            {
               Ball.Draw(spriteBatch);
            }

            //отрисовка мобов
            if (Lifmob)
            {
                Mob2.X = Mob2.FirstX;
                Mob1.Draw(spriteBatch); 
            }
            if(Mob1.RectangleCollision(Ball))
            {
                BallRun = false;
                Lifmob = false;
                shot.Play();
                Player2.Exp++;
                score++;
                if (score > highscore)
                { highscore = score; }

            }
            if (!Lifmob)
            {
                Mob1.X = Mob1.FirstX;
                Mob2.Draw(spriteBatch);
            }
            if (Mob2.RectangleCollision(Ball))
            {
                BallRun = false;
                Lifmob = true;
                shot.Play();
                Player2.Exp++;
                score++;
                if (score > highscore)
                { highscore = score; }
            }


            //начальная заставка
            if (!gameStarted)
            {

                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

                String title = "War Wizard 2D";
                String pressSpace = "Press Space to start";


                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);


                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3),Color.Red);
                spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2,screenHeight / 2), Color.White);
            }
            if (gameStarted)
            {

                String titleLevel= "Level "+Player2.Lvl.ToString();
                String titlePlay = "Press the button F to attack";
                String titleScore = string.Format("Score: {0:D3}", score);
                String titleHighS = string.Format("HighScore: {0:D3}", highscore);
                
                spriteBatch.DrawString(stateFont, titleLevel, new Vector2(10,0), Color.Red);
                spriteBatch.DrawString(stateFont, titlePlay, new Vector2(10, 100), Color.Red, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleScore, new Vector2(1125, 0), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleHighS, new Vector2(1125, 50), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public void StartGame()
        {
            Player2.X = 30;
            Player2.Y = 690;
        }

        void KeyboardHandler(Players Player2, GameTime gameTime) //кнопки и некторые условия
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Player2.X -= playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                ballCast = -1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Player2.X += playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                ballCast = 1;
            }

            if (keyboardState.IsKeyDown(Keys.F))
            {
                if (!BallRun)
                {
                    Ball.FirstX = Player2.X;
                    Ball.Y = Player2.Y;
                    BallRun = true;
                    Ball.Dx = ballCast * ballSpeedX;
                }
            }

            if (Math.Abs(Ball.FirstX - Ball.X) > 400 || !BallRun)
            {
                BallRun = false;
                Ball.X = Player2.X;
                Ball.Y = 1000;
            }


            if (Player2.Y < 690)
            {
                Player2.Dy += gravitySpeed * playerJumpY;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {

                if (!spaceDown && Player2.Y >= screenHeight * HITBOXSCALE - 1) { spaceDown = true; Player2.Dy = -playerJumpY; }


            }

            else { spaceDown = false; if (Player2.Y < 550) { Player2.Dy = playerJumpY; } }


            if (Player2.Y != 690) { spaceDown = true; }
            if (Player2.Y == 691) { spaceDown = false; Player2.Dy = 0; }


            if (keyboardState.IsKeyDown(Keys.Left)) Player2.Dx = -playerSpeedX;
            else if (keyboardState.IsKeyDown(Keys.Right)) Player2.Dx = playerSpeedX;
            else Player2.Dx = 0;

            if (Player2.Y > 690)
            {
                Player2.Dy = 0;
                Player2.Y = 690;
            }

            if (Player2.X > screenWidth)
            {
                Player2.X = screenWidth - 60;
                Player2.Dx = 0;
            }
            if (Player2.X < 0)
            {
                Player2.X = 60;
                Player2.Dx = 0;
            }
        }
    }
}