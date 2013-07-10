using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Events;
using Project2.Entity_Managers;

namespace Project2.Entity
{
    class Bullet :Entity
    {
        Vector3 target;
        public float time { get; set; }
        public static BulletManager bulletManager;
        public float speed { get; protected set; }
        public const float lifeTime = 3.0f;

        public Bullet(Game game, Model m,CollisionType type, Vector3 start, Vector3 target, float speed, int damage = 20)
            : base(game)
        {
            addComponent(new Spatial(game, this, start));
            getComponent<Spatial>().position = start;
            getComponent<Spatial>().transform *= Matrix.CreateScale(0.1f);
            addComponent(new Drawable3D(game, this, m));
            addComponent(new Collidable(game, this, type, onHit,1,damage));
            Vector3 focus = target - start;
            Matrix mat = Vector3.Backward.align(focus);
            getComponent<Spatial>().transform = mat * Matrix.CreateScale(3.0f) * Matrix.CreateTranslation(start);
            getComponent<Spatial>().focus = focus + start;
            this.speed = speed;

            this.target = Vector3.Normalize(target - start) * this.speed;
            time = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 movement = (target * (float)gameTime.ElapsedGameTime.Milliseconds);
            getComponent<Spatial>().position += movement;
            getComponent<Spatial>().focus += movement;
            time += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
        }

        public bool hasHit(Vector3 pos, float range = 10)
        {
            bool collision = false;
            float dist = Vector3.Dot(getComponent<Spatial>().position, pos);
            if (dist <= range)
                collision = true;
            return collision;
        }

        public static void init(Game game)
        {
            bulletManager = new BulletManager(game);
        }

        public void onHit(eCollision e)
        {
            Collidable c = getComponent<Collidable>();
            if (c != null && c.health <= 0)
            {                
                bulletManager.remove(this);
                dispose();
            }
        }
    }
}
