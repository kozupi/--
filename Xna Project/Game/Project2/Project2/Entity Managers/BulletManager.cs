using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Entity;
using Microsoft.Xna.Framework;
using Project2.Component;
using Microsoft.Xna.Framework.Graphics;
using Project2.Particle_Systems;
namespace Project2.Entity_Managers
{
    class BulletManager: GameComponent
    {
        List<Bullet> bullets;
        public int bulletCount { get; protected set; }
        public int missileCount { get; protected set; }

        public BulletManager(Game game)
            : base(game)
        {
            bullets = new List<Bullet>();
            bulletCount = 0;
        }

        public void addBullet(Model model,Vector3 position, Vector3 target, CollisionType type,float speed = 5.0f)
        {
            bullets.Add(new Bullet(Game, model, type, position, target,speed));
            bulletCount++;
        }

        public void addMissile(Model model, Vector3 position, Entity.Entity target, CollisionType type, float speed = 5.0f)
        {
            bullets.Add(new Missile(Game, model, type, position, target, speed));
            missileCount++;
        }

        public void addMissile(Model model, Vector3 position, Vector3 target, CollisionType type, float speed = 5.0f)
        {
            bullets.Add(new Missile(Game, model, type, position, target, speed));
            missileCount++;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (bullets[i].GetType() == typeof(Missile))
                {
                    Spatial sp = bullets[i].getComponent<Spatial>();
                }

                if (bullets[i].time > Bullet.lifeTime)
                {
                    bullets[i].dispose();
                    bullets.RemoveAt(i--);
                }
            }
            base.Update(gameTime);
        }

        public bool isHitByBullet(Vector3 pos, float range = 10)
        {
            bool isHit = false;
            foreach(Bullet b in bullets)
            {
                if (b.hasHit(pos, range))
                {
                    isHit = true;
                    break;
                }
            }
            return isHit;
        }

        public void remove(Bullet b)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] == b)
                {
                    bullets.RemoveAt(i);
                    break;
                }
            }
        }

        public void removeAllBullets()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets.RemoveAt(i);
                i--;
            }
        }
    }
}
