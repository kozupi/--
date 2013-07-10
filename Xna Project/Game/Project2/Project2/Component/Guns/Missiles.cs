using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Component.Guns
{
    class Missiles:Shootable
    {
        public Missiles(Game game, Entity.Entity parent, float fireRate = 2.5f)
            : base(game, parent, fireRate)
        {
        }        

        public override bool fire()
        {
            bool canFire = checkCanFire();
            if (canFire)
            {
                timeSinceLastShot = 0;
                Spatial sp = getComponent<Spatial>();
                if (sp.focus != null)
                {
                    if (parent.GetType() == typeof(Player))
                    {
                        SoundManager.createCue("fireMissile");
                    }
                    float speed = 0;
                    if(parent.getComponent<Controllable>() != null)
                        speed = parent.getComponent<Controllable>().speed;
                    if(parent.GetType() == typeof(Player))
                    {
                        Enemy e = Enemy.enemyManager.closestEnemy;
                        if (e != null)
                        {
                            Bullet.bulletManager.addMissile(Game.Content.Load<Model>(@"Models\FighterMissle"), sp.position,e, getComponent<Collidable>().type, 0.1f + speed * 0.5f);
                        }
                        else
                        {
                            Bullet.bulletManager.addMissile(Game.Content.Load<Model>(@"Models\FighterMissle"), sp.position, sp.focus, getComponent<Collidable>().type, 1.0f + speed);
                        }
                    }
                    else
                        Bullet.bulletManager.addMissile(Game.Content.Load<Model>(@"Models\FighterMissle"),sp.position, sp.focus, getComponent<Collidable>().type,2.0f + speed);
                }
            }

            return canFire;
        }

        public override bool fireUnguided()
        {
            bool canFire = checkCanFire();
            if (canFire)
            {
                timeSinceLastShot = 0;
                Spatial sp = getComponent<Spatial>();
                if (sp.focus != null)
                {
                    if (parent.GetType() == typeof(Player))
                    {
                        SoundManager.createCue("fireMissile");
                    }
                    float speed = 0;
                    if (parent.getComponent<Controllable>() != null)
                        speed = parent.getComponent<Controllable>().speed;
                    
                    Bullet.bulletManager.addMissile(Game.Content.Load<Model>(@"Models\FighterMissle"), sp.position, sp.focus, getComponent<Collidable>().type, 1.0f + speed);
                }
            }
            return canFire;
        }
    }
}
