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

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            while (elapsedTime >= spriteInfo.TimeToFrame)
            {
                currentFrame = (currentFrame + 1) % spriteInfo.FrameCount;
                elapsedTime -= spriteInfo.TimeToFrame;
            }

            var newPosition = Position + Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRect = new Rectangle((currentFrame % spriteInfo.FramesInRow) * spriteInfo.FrameWidth, (currentFrame / spriteInfo.FramesInRow) * spriteInfo.FrameHeight, spriteInfo.FrameWidth - 1, spriteInfo.FrameHeight - 1);
            spriteBatch.Draw(spriteInfo.Texture, Position, sourceRect, Color.White, 0f, origin, 2.5f, SpriteEffects.None, 0f);
        }
        public override Vector2 Center
        {
            get { return Position; }
        }
    }
}
