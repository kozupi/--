using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Component.Guns
{
    class Flare:Shootable
    {
        public Flare(Game game, Entity.Entity parent, float fireRate = 015f)
            : base(game, parent, fireRate)
        {

        }

        public override bool fire()
        {
            bool canFire = timeSinceLastShot > fireRate;
            if (canFire)
            {
                timeSinceLastShot = 0;
                Spatial sp = getComponent<Spatial>();
                if (sp.focus != null)
                {
                    Bullet.bulletManager.addBullet(Game.Content.Load<Model>(@"Models\Bullet"), sp.position, sp.focus, getComponent<Collidable>().type);
                }
            }
            return canFire;
        }
    }
}
