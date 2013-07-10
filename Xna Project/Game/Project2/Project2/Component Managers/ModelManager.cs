using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project2.Entity;

namespace Project2
{
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public delegate void draw(Camera camera);

        List<draw> models = new List<draw>();

        public ModelManager(Game game)
            : base(game)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Loop through and draw each model
            foreach (draw bm in models)
            {
                bm(((Game1)Game).camera);
            }

            base.Draw(gameTime);
        }

        public void addModelToDrawList(draw model)
        {
            models.Add(model);
        }
        //this may not work
        public void removeFromDrawList(draw model)
        {
            models.Remove(model);
        }
    }
}
