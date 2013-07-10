using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Component_Managers
{

    class SpriteManager:DrawableGameComponent
    {
        public delegate void draw(SpriteBatch spriteBatch);
        SpriteBatch spriteBatch;
        public SpriteFont font;
        List<draw> drawCalls;

        public SpriteManager(Game game, SpriteFont font)
            : base(game)
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            drawCalls = new List<draw>();
            this.font = font;
        }

        public void addSprite(draw drawSprite)
        {
            drawCalls.Add(drawSprite);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            foreach (var dc in drawCalls)
            {
                dc(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void removeSprite(draw drawSprite)
        {
            drawCalls.Remove(drawSprite);
        }
    }
}
