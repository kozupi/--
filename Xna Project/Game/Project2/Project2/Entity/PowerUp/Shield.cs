using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Events;

namespace Project2.Entity
{
    class Shield:Entity
    {
        Entity parent;
        public Shield(Game game, Entity parent)
            :base(game)
        {
            addComponent(new Spatial(game, this, new Vector3(0,800,0)));
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"Models\NewGameWorld")));
            getComponent<Drawable3D>().turnOnStealth(.3f);
            addComponent(new Collidable(game, this, CollisionType.powerUp, onHit,30,0));
            this.parent = parent;
            PowerUp.powerUpManager.addShield(this);
        }

        public void onHit(eCollision e)
        {
            if (getComponent<Collidable>().health <= 0)
            {
                dispose();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Spatial spatial = getComponent<Spatial>();
            if(spatial != null)
                spatial.transform = Matrix.CreateScale(15) * parent.getComponent<Spatial>().transform;
            base.Update(gameTime);
        }
    }
}
