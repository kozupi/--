using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
namespace Project2.Entity
{
    class Shrapnel : Entity
    {
        public static List<Shrapnel> shrapnels;
        float scale;
        float rotation;
        float lifeTime;
        float time;

        public Shrapnel(Game game, Vector3 pos, float lifetime = 1000)
            : base(game)
        {
            //pos = new Vector3(0, 2500, 0);
            addComponent(new Spatial(game, this, pos));
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"Models/Shrapnel_Zero")));
            scale = 0.0f;
            getComponent<Spatial>().transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(getComponent<Spatial>().position);// *rotate;
            rotation = 0;
            time = 0;
            lifeTime = lifetime;
            shrapnels.Add(this);
        }

        public static void init(Game1 game1)
        {
            shrapnels = new List<Shrapnel>();
        }

        public override void Update(GameTime gameTime)
        {
            if (time > lifeTime)
            {
                dispose();
            }
            else
            {
                Matrix rotate = Matrix.CreateRotationX(rotation / 2) * Matrix.CreateRotationY(rotation / 0.4f) * Matrix.CreateRotationZ(rotation);
                getComponent<Spatial>().transform = rotate * Matrix.CreateScale(scale) * Matrix.CreateTranslation(getComponent<Spatial>().position);

                if(scale < 3)
                    scale += 0.1f;
                rotation = (rotation + ((float)Math.PI / 500.0f)) % ((float)Math.PI * 2);
                time += gameTime.ElapsedGameTime.Milliseconds;

                base.Update(gameTime);
            }
        }
    }
}
