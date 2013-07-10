using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project2
{
    class AutomatedSprite: Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, Vector2 centerPoint)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, centerPoint)
        { }

        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed,Vector2 centerPoint, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed,centerPoint)
        { }

        public override Vector2 direction
        {
            get { return speed; }
        }
        public override void reflect(Vector2 normal)
        {
            speed = Vector2.Reflect(speed, normal);
        }

        public override Vector2 position
        {
            get { return pos; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            pos += direction;
            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
            {
                speed = Vector2.Reflect(speed,new Vector2(1,0));
            }
            if (position.Y < 0)
            {
                speed = Vector2.Reflect(speed, new Vector2(0, 1));
            }
            if (position.X > clientBounds.Width - frameSize.X)
            {
                speed = Vector2.Reflect(speed, new Vector2(-1, 0));
            }
            if (position.Y > clientBounds.Height - frameSize.Y)
            {
                speed = Vector2.Reflect(speed, new Vector2(0, -1));
            }
            base.Update(gameTime, clientBounds);
        }
    }
}
