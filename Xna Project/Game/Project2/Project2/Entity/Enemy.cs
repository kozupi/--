using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Events;
using Project2.Entity_Managers;
using Project2.Component.Guns;
using Project2.Particle_Systems;

namespace Project2.Entity
{
    class Enemy : Entity
    {
        delegate void AILogic(GameTime gameTime);

        public static EnemyManager enemyManager;
        Entity targetToChase;
        Shootable weapon;
        Pathing path;
        AILogic logic;
        float recentlyHit = 0.0f;
        float lifeTime = 0.0f;
        float speed = 0.49f;
        const float chaseSpeedAdjustment = 0.33f;

        public Enemy(Game game, Vector3 position, Entity entityToChase)
            : base(game)
        {
            addComponent(new Spatial(game, this, position));
            getComponent<Spatial>().focus = Vector3.Forward + position;
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"models\Plane3")));
            addComponent(new Collidable(game, this, CollisionType.enemy, onHit, 50, 250,250));
            addComponent(new Spatial2D(game, this, Vector2.Zero, new Vector2(1.0f, 1.0f)));
            addComponent(new Drawable2D(game, this, Game.Content.Load<Texture2D>(@"Textures\TrackingTriangle"),Color.Lime));
            weapon = new Missiles(game, this);
            path = new PathingWander(Game, this, entityToChase, speed);
            addComponent(weapon);
            logic = joust;

            int type = ((Game1)Game).rnd.Next(0, 2);
            //int type = 0;
            switch (type)
            {
                case 0:
                    logic = joust;
                    break;
                case 1:
                    logic = stayInFrontAndEvade;
                    break;
                case 2:
                    logic = wanderInFront;
                    break;
                case 3:
                    logic = kamiKaze;
                    break;
                case 4:
                    logic = chaseDown;
                    break;
                default:
                    break;
            }
            addComponent(path);
            targetToChase = entityToChase;
            enemyManager.add(this);
        }

        public static void init(Game game)
        {
            enemyManager = new EnemyManager(game);
        }

        public void onHit(eCollision e)
        {
            if (e.mainObject == getComponent<Collidable>())
            {
                if (e.hitObject.damage > 0)
                {
                    recentlyHit += .75f;                    
                }
            }
            else
            {
                if (e.mainObject.damage > 0)
                {
                    recentlyHit += .75f;                    
                }
            }

            Collidable c = getComponent<Collidable>();
            if (c != null && c.health <= 0)
            {
                Vector3 position = getComponent<Spatial>().position;
                new PowerUp(Game, position, powerType.ring);
                var explosion = new ExplosionParticleSystem(Game, Game.Content);
                ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, explosion, 40, 2.0f, position));
                var smoke = new ExplosionSmokeParticleSystem(Game, Game.Content);
                ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, smoke, 50, 3.0f, position));
                dispose();
                SoundManager.playSound("Explosion", position);
                ((Game1)Game).addAnEnemy();
            }

        }
        

        private Vector3 getScreenPosition()
        {
            Camera cam = ((Game1)Game).camera;
            Vector3 screenPos = Vector3.Transform(getComponent<Spatial>().position, cam.view);
            screenPos = Vector3.Transform(screenPos, cam.projection);
            screenPos.X = ((Game1)Game).graphics.PreferredBackBufferWidth / 2.0f * (1 + screenPos.X / screenPos.Z);
            screenPos.Y = ((Game1)Game).graphics.PreferredBackBufferHeight / 2.0f * (1 - screenPos.Y / screenPos.Z);
            screenPos.Z = (cam.far + cam.near) / (cam.far - cam.near) + (1 / screenPos.Z) * ((-2 * cam.far * cam.near) / (cam.far - cam.near));
            if (screenPos.Z > 1)
            {
                screenPos.Z = 1;
            }
            return screenPos;
        }

        public override void Update(GameTime gameTime)
        {
            if (targetToChase != null)
            {
                logic(gameTime);
                switchWeapons();
            }
            lifeTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (lifeTime > 30.0f)
            {
                path = null;
                logic = joust;
                lifeTime = float.MinValue;
            }
            Vector3 screenPos = getScreenPosition();
            getComponent<Spatial2D>().position = new Vector2(screenPos.X, screenPos.Y);
            float zscale = 1.0f - (float)Math.Pow(screenPos.Z, 300);
            getComponent<Spatial2D>().scale = new Vector2(zscale);

            base.Update(gameTime);
        }

        private void switchWeapons()
        {
            float distance = (targetToChase.getComponent<Spatial>().position - getComponent<Spatial>().position).LengthSquared();
            if (distance < 400000)
            {
                if (distance < 160000 && weapon.GetType() != typeof(MachineGun))
                {
                    removeComponent(weapon);
                    weapon = new MachineGun(Game, this, 0.25f);
                    addComponent(weapon);
                }
                else if (distance > 160000 && weapon.GetType() != typeof(Missiles))
                {
                    removeComponent(weapon);
                    weapon = new Missiles(Game, this);
                    addComponent(weapon);
                }
            }
        }

        public override void dispose()
        {
            enemyManager.removeEnemy(this);
            base.dispose();
        }

        private void wanderInFront(GameTime gameTime)
        {
            if (path == null)
            {
                removeComponent<Pathing>();
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            if (path.GetType() != typeof(PathingWander))
            {
                removeComponent(path);
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            Spatial sp = getComponent<Spatial>();
            if (Vector3.Dot(sp.focus - sp.position, Vector3.Normalize(targetToChase.getComponent<Spatial>().position - sp.position))> 0.9f)
            {
                weapon.fire();
            }
        }

        private void joust(GameTime gameTime)
        {
            if (path == null)
            {
                removeComponent<Pathing>();
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            Spatial sp = getComponent<Spatial>();
            Spatial enemySp = targetToChase.getComponent<Spatial>();
            bool isWandering = path.GetType() == typeof(PathingWander);
            Vector3 vectorBetween = Vector3.Normalize(enemySp.position - sp.position);
            float dot = Vector3.Dot(enemySp.focus - enemySp.position,-vectorBetween);
            if (isWandering)
            {
                    if (dot > 0.93f)
                    {
                        removeComponent(path);
                        path = new PathingChase(Game, this, targetToChase, speed * chaseSpeedAdjustment);
                        addComponent(path);
                    }        
            }
            else if (!isWandering && dot < 0.0f)
            {
                removeComponent(path);
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            if (Vector3.Dot(sp.focus - sp.position, vectorBetween) > 0.9f)
            {
                weapon.fire();                
            }            
        }

        private void kamiKaze(GameTime gameTime)
        {
            if (path == null)
            {
                removeComponent<Pathing>();
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            if (path.GetType() != typeof(PathingChase))
            {
                removeComponent<Pathing>();
                path = new PathingChase(Game, this, targetToChase, speed * chaseSpeedAdjustment);
                addComponent(path);
            }
        }

        private void chaseDown(GameTime gameTime)
        {
            Spatial sp = getComponent<Spatial>();
            Vector3 vectorBetween = Vector3.Normalize(targetToChase.getComponent<Spatial>().position - sp.position);
            if (path == null)
            {
                removeComponent<Pathing>();
                path = new PathingChase(Game, this, targetToChase, speed * chaseSpeedAdjustment);
                addComponent(path);
            }

            if (Vector3.Dot(sp.focus - sp.position, vectorBetween) > 0.9f && ((Game1)Game).rnd.NextDouble() > 0.99)
            {
                weapon.fire();
            }
        }

        private void stayInFrontAndEvade(GameTime gameTime)
        {
            if (path == null)
            {
                removeComponent<Pathing>();
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
            if (recentlyHit > 0.0f)
            {
                if (path.GetType() != typeof(PathingEvade))
                {
                    removeComponent(path);
                    path = new PathingEvade(Game, this, targetToChase, speed);
                    addComponent(path);
                }
                recentlyHit -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            }
            else if (path.GetType() != typeof(PathingWander))
            {
                removeComponent(path);
                path = new PathingWander(Game, this, targetToChase, speed);
                addComponent(path);
            }
        }
        
    }
}
