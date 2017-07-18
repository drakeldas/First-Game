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

    public class WarWizard2D : Game
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
        public WarWizard2D()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = this.screenWidth;
            graphics.PreferredBackBufferHeight = this.screenHeight;
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


            Mob1.Firstx = this.screenWidth + 600; ;
            Mob1.Firsty = 605;
            Mob1.X = 1300;
            Mob1.Y = 605;
            Mob1.dX = this.MobSpeed;
            Mob2.Firstx = -600;
            Mob2.Firsty = 605;
            Mob2.X = 0;
            Mob2.Y = 605;
            Mob2.dX = this.MobSpeed;
            Player2.Lvl = 1;
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
                Player2.Exp = 0; Player2.Lvl++; lvlup.Play(); Mob2.dX += this.MobSpeed / 5; Mob1.dX += this.MobSpeed / 5; this.damage += Player2.Lvl * 5;
            }
            if (Player2.Exp > 10 * Player2.Lvl) { Player2.Exp = 0; }


            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) { Exit(); }

            if (!gameStarted)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    this.gameStarted = true;
                    this.spaceDown = true;
                }
            }
            if (gameOver && keyboardState.IsKeyDown(Keys.Enter))
            {
                this.Lifmob = true;
                Mob2.X = 0;
                Mob1.X = screenWidth; ;
                Mob1.dX = this.MobSpeed;
                Mob2.dX = this.MobSpeed;
                Player2 = new Players(GraphicsDevice, "Content/player.png", 1);
                Player = new Sprite(GraphicsDevice, "Content/player2.png", 1);
                Ball = new Sprite(GraphicsDevice, "Content/redball.png", (float)0.2);
                Player2.Exp = 0;
                Player2.Lvl = 1;
                this.score = 0;
                retry.Play();
                StartGame();
                this.gameOver = false;

            }
            //обновление координат спрайтов
            if (this.gameStarted)
            {
                if (!this.gameOver)
                {
                    Player2.Update(elapsedTime);
                    KeyboardHandler(Player2, gameTime);
                }
                Ball.Update(elapsedTime);
                if (this.Lifmob)
                {
                    Mob1.Update(elapsedTime, Player2);
                }
                if (!this.Lifmob) Mob2.Update(elapsedTime, Player2);
            }
            if (Player2.RectangleCollision(Mob1) && this.Lifmob) { this.gameOver = true; }
            if (Player2.RectangleCollision(Mob2) && !this.Lifmob) { this.gameOver = true; }

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
            if (!this.gameOver)
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
            if (this.gameOver)
            {
                Rip.X = Player2.X;
                Rip.Y = Player2.Y;
                Rip.Draw(spriteBatch);
                String titleGameOver = "GAME OVER";
                String titleAgain = "Press Enter to start again";
                String titleExit = "Press Esc to exit";
                BallRun = false;
                Ball.dX = 0;
                Ball.Y = 1000;
                spriteBatch.DrawString(stateFont, titleGameOver, new Vector2(500, 275), Color.Red, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleAgain, new Vector2(575, 390), Color.Red, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(stateFont, titleExit, new Vector2(635, 425), Color.Red, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 1f);
            }

            //отрисовка спрайта шара
            if (this.BallRun)
            {
                Ball.Draw(spriteBatch);
            }

            //отрисовка мобов
            if (this.Lifmob)
            {
                Mob2.X = Mob2.Firstx;
                Mob1.Draw(spriteBatch);
            }
            if (Mob1.RectangleCollision(Ball))
            {
                this.BallRun = false;
                this.Lifmob = false;
                shot.Play();
                Player2.Exp++;
                this.score++;
                if (this.score > this.highscore)
                { this.highscore = this.score; }

            }
            if (!this.Lifmob)
            {
                Mob1.X = Mob1.Firstx;
                Mob2.Draw(spriteBatch);
            }
            if (Mob2.RectangleCollision(Ball))
            {
                this.BallRun = false;
                this.Lifmob = true;
                shot.Play();
                Player2.Exp++;
                this.score++;
                if (this.score > this.highscore)
                { this.highscore = this.score; }
            }


            //начальная заставка
            if (!this.gameStarted)
            {
                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
                String title = "First-Game 2D";
                String pressSpace = "Press Space to start";
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);
                spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.Red);
                spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White);
            }
            if (this.gameStarted)
            {

                String titleLevel = "Level " + Player2.Lvl.ToString();
                String titlePlay = "Press the button F to attack";
                String titleScore = string.Format("Score: {0:D3}", this.score);
                String titleHighS = string.Format("HighScore: {0:D3}", this.highscore);

                spriteBatch.DrawString(stateFont, titleLevel, new Vector2(10, 0), Color.Red);
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
                Player2.X -= this.playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                this.ballCast = -1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Player2.X += this.playerSpeedX * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                this.ballCast = 1;
            }
            if (keyboardState.IsKeyDown(Keys.F))
            {
                if (!this.BallRun)
                {
                    Ball.Firstx = Player2.X;
                    Ball.Y = Player2.Y;
                    this.BallRun = true;
                    Ball.dX = this.ballCast * this.ballSpeedX;
                }
            }
            if (Math.Abs(Ball.Firstx - Ball.X) > 400 || !this.BallRun)
            {
                this.BallRun = false;
                Ball.X = Player2.X;
                Ball.Y = 1000;
            }
            if (Player2.Y < 690)
            {
                Player2.dY += this.gravitySpeed * this.playerJumpY;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (!this.spaceDown && Player2.Y >= this.screenHeight * HITBOXSCALE - 1) { this.spaceDown = true; Player2.dY = -this.playerJumpY; }
            }
            else { this.spaceDown = false; if (Player2.Y < 550) { Player2.dY = playerJumpY; } }
            if (Player2.Y != 690) { this.spaceDown = true; }
            if (Player2.Y == 691) { this.spaceDown = false; Player2.dY = 0; }
            if (keyboardState.IsKeyDown(Keys.Left)) Player2.dX = -this.playerSpeedX;
            else if (keyboardState.IsKeyDown(Keys.Right)) Player2.dX = this.playerSpeedX;
            else Player2.dX = 0;
            if (Player2.Y > 690)
            {
                Player2.dY = 0;
                Player2.Y = 690;
            }
            if (Player2.X > this.screenWidth)
            {
                Player2.X = this.screenWidth - 60;
                Player2.dX = 0;
            }
            if (Player2.X < 0)
            {
                Player2.X = 60;
                Player2.dX = 0;
            }
        }
    }
}