using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class SpriteInfo
    {
        public int FrameCount { get; set; }

        public TimeSpan TimeToFrame { get; set; }

        public Texture2D Texture { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int FramesInRow { get; set; }
    }
}