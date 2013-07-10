using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Component.Guns
{
    class MachineGun:Shootable
    {
        public MachineGun(Game game, Entity.Entity parent, float fireRate = 0.05f)
            : base(game, parent,fireRate)
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
                    if (parent.GetType() == typeof(Player))
                    {
                        SoundManager.playSound("MachineGun");
                        
                    } 
                    float speed = 0;
                    if (getComponent<Controllable>() != null)
                    {
                        speed = parent.getComponent<Controllable>().speed;
                    }

                    Random ran = ((Game1)Game).rnd;
                    Vector3 spread = new Vector3(ran.Next(-15,15) * 0.3f, ran.Next(-15, 15) * 0.3f, ran.Next(-15, 15) * 0.2f);
                    Bullet.bulletManager.addBullet(Game.Content.Load<Model>(@"Models\Bullet"), sp.position + spread, sp.focus + spread, getComponent<Collidable>().type, 1 + speed);
                }
            }

            return canFire;
        }
    }
}
