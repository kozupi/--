using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project2
{
    abstract class Sprite
    {
        protected Texture2D textureImage;
        protected Point frameSize;
        protected Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;
        protected Vector2 speed;
        protected Vector2 pos;
        protected Vector2 centerPoint = Vector2.Zero;
        protected float theta = 0f;

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Vector2 centerPoint)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed,centerPoint, defaultMillisecondsPerFrame) { }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,Vector2 centerPoint,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.pos = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.centerPoint = centerPoint;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public abstract Vector2 direction
        {
            get;
        }
        public abstract Vector2 position
        {
            get;
        }

        public abstract void reflect(Vector2 normal);

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)pos.X + collisionOffset,
                (int)pos.Y + collisionOffset,
                frameSize.X - (collisionOffset * 2),
                frameSize.Y - (collisionOffset * 2));
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X, frameSize.Y),
            Color.White, theta, centerPoint,
            1f, SpriteEffects.None, 0);
        }
    }
}
