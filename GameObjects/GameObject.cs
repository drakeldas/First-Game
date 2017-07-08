using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.GameObjects
{
    public abstract class GameObject
    {
        public Vector2 Velocity { get; set; }
        public abstract Vector2 Center { get; }
        public virtual void Update(GameTime gameTime)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}