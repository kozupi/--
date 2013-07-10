using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Project2
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class PathControlledSprite : Sprite
    {
       private CurvePath curve;
       public PathControlledSprite(CurvePath curvePath, Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Vector2 centerPoint)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, centerPoint)
        {
            curve = curvePath;
       }

       public PathControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,Vector2 centerPoint,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed,centerPoint, millisecondsPerFrame)
        {}

        public void setCurvePath(CurvePath path)
        {
            curve = path;
        }

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
            pos = curve.eval((float)gameTime.TotalGameTime.TotalMilliseconds * 0.002f);
            Vector2 deriv = curve.derivativeNZ((float)gameTime.TotalGameTime.TotalMilliseconds * 0.002f);
            theta = (float)Math.Atan2(deriv.Y, deriv.X);
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects effect = new SpriteEffects();
            Vector2 deriv = curve.derivativeNZ((float)gameTime.TotalGameTime.TotalMilliseconds * 0.002f);
            if (deriv.X < 0)
            {
                effect = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(textureImage,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X, frameSize.Y),
            Color.White, theta, centerPoint,
            1f, effect, 0);
        }
    }
}
