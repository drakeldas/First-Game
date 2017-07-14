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

namespace Game1
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
            score = 0;
            highscore = 0;
            damage = 30;
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

            
            Mob1.firstx = screenWidth+600; ;
            Mob1.firsty = 605;
            Mob1.x = 1300;
            Mob1.y = 605;
            Mob1.dX = MobSpeed;
            Mob2.firstx = -600;
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
            //смена спрайтов с уровнем
            if (Player2.lvl >= 2) { Ball.texture = Fire; }
            if (Player2.lvl >= 3) { Player2.texture = Player_2; Player.texture = Player_1; Player.scale = 0.2f; Player2.scale = 0.2f; }
            //условие набора уровня
            if (Player2.exp == 10 * Player2.lvl)
            {
                Player2.exp = 0; Player2.lvl++; lvlup.Play(); Mob2.dX += MobSpeed / 5; Mob1.dX += MobSpeed / 5; damage += Player2.lvl * 5;
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
                Lifmob = true;
                Mob2.x = 0;
                Mob1.x = screenWidth; ;
                Mob1.dX = MobSpeed;
                Mob2.dX = MobSpeed;
                Player2 = new Players(GraphicsDevice, "Content/player.png", 1);
                Player = new Sprite(GraphicsDevice, "Content/player2.png", 1);
                Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
                Player2.exp = 0;
                Player2.lvl = 1;
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
            if (Player2.health < 0) { gameOver = true; Player2.health = 100 * Player2.lvl; }
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
                Tilemap.framX = 256;
                Tilemap.framY = 0;
                Tilemap.framDx = 256;
                Tilemap.framDy = 256;
                Tilemap.x = i * 256;
                Tilemap.y = 920;
                Tilemap.Draw(spriteBatch);
            }
            //отрисовка персонажа
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
                String titleGameOver = "GAME OVER";
                String titleAgain = "Press Enter to start again";
                String titleExit = "Press Esc to exit";
                BallRun = false;
                Ball.dX = 0;
                Ball.y = 1000;
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
                Mob2.x = Mob2.firstx;
                Mob1.Draw(spriteBatch); 
            }
            if(Mob1.RectangleCollision(Ball))
            {
                BallRun = false;
                Lifmob = false;
                shot.Play();
                Player2.exp++;
                score++;
                if (score > highscore)
                { highscore = score; }

            }
            if (!Lifmob)
            {
                Mob1.x = Mob1.firstx;
                Mob2.Draw(spriteBatch);
            }
            if (Mob2.RectangleCollision(Ball))
            {
                BallRun = false;
                Lifmob = true;
                shot.Play();
                Player2.exp++;
                score++;
                if (score > highscore)
                { highscore = score; }
            }


            //начальная заставка
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

                String titleLevel= "Level "+Player2.lvl.ToString();
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
            Player2.x = 30;
            Player2.y = 690;
        }

        void KeyboardHandler(Players Player2, GameTime gameTime) //кнопки и некторые условия
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
                    Ball.firstx = Player2.x;
                    Ball.y = Player2.y;
                    BallRun = true;
                    Ball.dX = ballCast * ballSpeedX;
                }
            }

            if (Math.Abs(Ball.firstx - Ball.x) > 400 || !BallRun)
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
}