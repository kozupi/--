using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Component;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;
using Project2.Particle_Systems;

namespace Project2.Entity
{
    class Missile :Bullet
    {
        public Missile(Game game, Model m, CollisionType type, Vector3 start, Entity target, float speed)
            : base(game,m,type,start,Vector3.Zero,speed,50)
        {
            var exhaust = new MissileExhaustParticleSystem(game, ((Game1)Game).Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, exhaust, 1000, lifeTime, getExhaustPosition));          
            addComponent(new PathingChase(game, this, target, speed, true));
        }

        public Vector3 getExhaustPosition()
        {
            Spatial sp = getComponent<Spatial>();
            if (sp != null)
            {
                return (sp.position - ((sp.focus - sp.position) * 13));
            }
            else
            {
                return new Vector3(100000.0f, 1000000.0f, 10000000.0f);
            }
        }

        public Missile(Game game, Model m, CollisionType type, Vector3 start, Vector3 target, float speed)
            : base(game,m,type,start,target,speed,50)
        {
            var exhaust = new MissileExhaustParticleSystem(game, ((Game1)Game).Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, exhaust, 30, float.MaxValue, getExhaustPosition));
        }

        public override void Update(GameTime gameTime)
        {
            if (getComponent<PathingChase>() != null)
            {
                base.time += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                getComponent<PathingChase>().Update(gameTime);
                Vector3 pos = getComponent<Spatial>().position;
                getComponent<Spatial>().transform *= Matrix.CreateScale(3.0f);
                getComponent<Spatial>().position = pos;
            }
            else
            {
                base.Update(gameTime);
            }            
        }

        public override void dispose()
        {
            Vector3 position = getComponent<Spatial>().position;
            var explosion = new ExplosionParticleSystem(Game, Game.Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, explosion, 40, 2.0f, position));
            var smoke = new ExplosionSmokeParticleSystem(Game, Game.Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, smoke, 50, 3.0f, position));
            SoundManager.stopCue("fireMissile");
            SoundManager.playSound("Explosion", position);
            base.dispose();
        }
    }
}
