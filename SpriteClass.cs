using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarWizard2D
{
    public class Sprite
    {
        const float HITBOXSCALE = .5f;

        public Sprite(GraphicsDevice graphicsDevice, string textureName, float scale)
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

        public void Update (float elapsedTime)
        {
            this.X += this.Dx * elapsedTime;
            this.Y += this.Dy * elapsedTime;
            this.Angle += this.Da * elapsedTime;
        }

        public void Draw (SpriteBatch spriteBatch)
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

        public bool RectangleCollision (Sprite otherSprite)
        {
            if (this.X + this.Texture.Width * this.Scale * HITBOXSCALE / 2 < otherSprite.X - otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y + this.Texture.Height * this.Scale * HITBOXSCALE / 2 < otherSprite.Y - otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            if (this.X - this.Texture.Width * this.Scale * HITBOXSCALE / 2 > otherSprite.X + otherSprite.Texture.Width * otherSprite.Scale / 2) return false;
            if (this.Y - this.Texture.Height * this.Scale * HITBOXSCALE / 2 > otherSprite.Y + otherSprite.Texture.Height * otherSprite.Scale / 2) return false;
            return true;
        }
        
        public Texture2D Texture { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float FirstX { get; set; }

        public float FirstY { get; set; }

        public float Angle { get; set; }

        public float Dx { get; set; }

        public float Dy { get; set; }

        public float Da { get; set; }

        public float Scale { get; set; }

        public int FramX { get; set; }

        public int FramY { get; set; }

        public int FramDx { get; set; }

        public int FramDy { get; set; }

    }
}