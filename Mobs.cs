using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarWizard2D
{
    public class Mobs : SpriteAbstract
    {
        
        const float HITBOXSCALE = .5f;
        
        public Mobs(GraphicsDevice graphicsDevice, string textureName, float scale)
        {
            this.Scale = scale;
            if (Texture == null)
            {
                using (var stream = TitleContainer.OpenStream(textureName))
                {
                    this.Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
        }
        public void Update(float elapsedTime, Players player)
        {
            if (this.X < -50) { this.X = -50; }
            if (this.X > 1550) { this.X = 1550; }
            if ((player.X - this.X - this.Texture.Width * this.Scale * HITBOXSCALE / 2) < 0) { this.X -= this.dX * elapsedTime; }
            if ((player.X - this.X + this.Texture.Width * this.Scale * HITBOXSCALE / 2) > 0) { this.X += this.dX * elapsedTime; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spritePosition = new Vector2(this.X, this.Y);

            if (this.FramDx == 0 || this.FramDy == 0)
            {
                spriteBatch.Draw(Texture, spritePosition, null, Color.White, this.Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), new Vector2(Scale, Scale), SpriteEffects.None, 0f);
            }
            else
            {
                var FramMob = new Rectangle(this.FramX, this.FramY, this.FramDx, this.FramDy);
                spriteBatch.Draw(Texture, spritePosition, FramMob, Color.White, this.Angle, new Vector2(Texture.Width / 2, Texture.Height / 2), new Vector2(Scale, Scale), SpriteEffects.None, 0f);
            }
        }
        public override bool RectangleCollision(SpriteAbstract otherSprite)
        {
            if (this.X + this.Texture.Width * this.Scale * HITBOXSCALE / 2 < otherSprite.X - otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y + this.Texture.Height * this.Scale * HITBOXSCALE / 2 < otherSprite.Y - otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            if (this.X - this.Texture.Width * this.Scale * HITBOXSCALE / 2 > otherSprite.X + otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y - this.Texture.Height * this.Scale * HITBOXSCALE / 2 > otherSprite.Y + otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            return true;
        }
        

        public override Texture2D Texture { get;set;}
        

    }
}
   