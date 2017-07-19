using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace WarWizard2D
{
    public abstract class SpriteAbstract
    {
        
        abstract public void Draw(SpriteBatch spriteBatch);
        abstract public Texture2D Texture { get; set; }
        abstract public bool RectangleCollision(SpriteAbstract otherSprite);
         public float X { get; set; }
         public float Y { get; set; }
         public float Firstx { get; set; }
         public float Firsty { get; set; }
         public float Angle { get; set; }
         public float dX { get; set; }
         public float dY { get; set; }
         public float dA { get; set; }
         public float Scale { get; set; }
         public int FramX { get; set; }
         public int FramY { get; set; }
         public int FramDx { get; set; }
         public int FramDy { get; set; }
    }
}
