using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Particle_Systems;
using Project2.Component;
using Project2.Entity;
using Microsoft.Xna.Framework;

namespace Project2
{
    class ParticleEmitterManager: DrawableGameComponent
    {
        public static ParticleEmitterManager particleEmitterManager { get; protected set; }
        List<ParticleEmitter> particleManagers;

        private ParticleEmitterManager(Game game)
            :base(game)
        {
            particleManagers = new List<ParticleEmitter>();
        }

        public static void addParticleEmitter(Game game,ParticleSystem particleSystem,
                               float particlesPerSecond,float lifeTime ,Func<Vector3> getLocation)
        {
            particleEmitterManager.particleManagers.Add(new ParticleEmitter(game, particleSystem, particlesPerSecond, lifeTime, getLocation));
        }

        public static void addParticleEmitter(ParticleEmitter particleEmitter)
        {
            particleEmitterManager.particleManagers.Add(particleEmitter);
        }

        public static void init(Game game)
        {
            particleEmitterManager = new ParticleEmitterManager(game);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < particleEmitterManager.particleManagers.Count; i++)
            {
                if (!particleEmitterManager.particleManagers[i].Update(gameTime))
                {
                    particleEmitterManager.particleManagers.RemoveAt(i);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var emitter in particleEmitterManager.particleManagers)
            {
                emitter.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
