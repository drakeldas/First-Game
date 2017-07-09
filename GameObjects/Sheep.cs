using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1.GameObjects
{
    public class Sheep : GameObject
    {
        private int currentFrame;
        private TimeSpan elapsedTime;
        private SpriteInfo spriteInfo;
        private Vector2 origin;

        public Sheep(SpriteInfo spriteInfo)
        {
            this.spriteInfo = spriteInfo;
            origin = new Vector2(spriteInfo.FrameWidth / 2f, spriteInfo.FrameHeight );
        }

        public Vector2 Position { get; set; }

        public void Left()
        {
            if (currentFrame >= 12 && currentFrame < 14)
            {
                currentFrame++;
            }
            else currentFrame = 12;
        }

        public void Right()
        {
            if (currentFrame >= 24 && currentFrame < 26)
            {
                currentFrame++;
            }
            else currentFrame = 24;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            
            while (elapsedTime >= spriteInfo.TimeToFrame)
            {
                if (Position.X <= 0 || Position.X >= 1500)
                {
                   Velocity = -Velocity;
                }

                if (Velocity.X > 0)
                {
                    Right();
                }
                else Left();
                elapsedTime -= spriteInfo.TimeToFrame;
            }

            var newPosition = Position + Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position = newPosition;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRect = new Rectangle((currentFrame % spriteInfo.FramesInRow) * spriteInfo.FrameWidth, (currentFrame / spriteInfo.FramesInRow) * spriteInfo.FrameHeight, spriteInfo.FrameWidth, spriteInfo.FrameHeight);
            spriteBatch.Draw(spriteInfo.Texture, Position, sourceRect, Color.White, 0f, origin, 2.75f, SpriteEffects.None, 0f);
        }
        public override Vector2 Center
        {
            get { return Position; }
        }
    }
}
