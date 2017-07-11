using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Players
    {
        const float HITBOXSCALE = .5f;
        bool pass = false;
        public Players(GraphicsDevice graphicsDevice, string textureName, float scale)
        {
            this.scale = scale;
            if (texture == null)
            {
                using (var stream = TitleContainer.OpenStream(textureName))
                {
                    texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
        }
        public void Update(float elapsedTime)
        {
            this.x += this.dX * elapsedTime;
            this.y += this.dY * elapsedTime;
            this.angle += this.dA * elapsedTime;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spritePosition = new Vector2(this.x, this.y);

            if (this.framDx == 0 || this.framDy == 0)
            {
                spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
            }
            else
            {
                var FramMob = new Rectangle(this.framX, this.framY, this.framDx, this.framDy);
                spriteBatch.Draw(texture, spritePosition, FramMob, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
            }
        }
        public bool RectangleCollision(Mobs otherSprite)
        {
            if (this.x  < otherSprite.x - otherSprite.texture.Width * otherSprite.scale / 3) return false;
            if (this.y + this.texture.Height * this.scale * HITBOXSCALE / 2 < otherSprite.y - otherSprite.texture.Height * otherSprite.scale / 2) return false;
            if (this.x  > otherSprite.x + otherSprite.texture.Width * otherSprite.scale / 3) return false;
            if (this.y - this.texture.Height * this.scale * HITBOXSCALE / 2 > otherSprite.y + otherSprite.texture.Height * otherSprite.scale / 2) return false;
            return true;
        }
        public Texture2D texture
        {
            get;
        }
        public int lvl
        {
            get;
            set;
        }
        public int exp
        {
            get;
            set;
        }
        public int health
        {
            get;
            set;
        }

        public float x
        {
            get;
            set;
        }

        public float y
        {
            get;
            set;
        }
        public float firstx
        {
            get;
            set;
        }

        public float firsty
        {
            get;
            set;
        }

        public float angle
        {
            get;
            set;
        }

        public float dX
        {
            get;
            set;
        }

        public float dY
        {
            get;
            set;
        }

        public float dA
        {
            get;
            set;
        }

        public float scale
        {
            get;
            set;
        }

        public int framX
        {
            get;
            set;
        }

        public int framY
        {
            get;
            set;
        }
        public int framDx
        {
            get;
            set;
        }

        public int framDy
        {
            get;
            set;
        }

    }
}
