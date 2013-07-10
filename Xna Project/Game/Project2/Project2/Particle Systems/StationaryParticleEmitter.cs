using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class StationaryParticleEmitter
    {
        ParticleSystem particleSystem;
        Vector3 position;
        int xRadius, yRadius, zRadius, numParticlesToEmit;
        Random rnd = new Random();

        public StationaryParticleEmitter(ref ParticleSystem system, Vector3 initialPos,
            int xRadius, int yRadius, int zRadius, int numParticlesToEmit)
        {
            particleSystem = system;
            position = initialPos;
            this.xRadius = xRadius;
            this.yRadius = yRadius;
            this.zRadius = zRadius;
            this.numParticlesToEmit = numParticlesToEmit;
        }

        public void Emit()
        {
            for (int i = 0; i < numParticlesToEmit; i++)
            {
                float s = (float)(rnd.NextDouble() * (2 * Math.PI));
                float t = (float)(rnd.NextDouble() * (2 * Math.PI));
                float xAdd = (float)(rnd.Next(xRadius*2) * Math.Cos(s) * Math.Sin(t));
                float yAdd = (float)(rnd.Next(yRadius*2) * Math.Sin(s) * Math.Sin(t));
                float zAdd = (float)(rnd.Next(zRadius*2) * Math.Cos(t));
                Vector3 particlePos = new Vector3(position.X + xAdd, position.Y + yAdd, position.Z + zAdd);
                particleSystem.AddParticle(particlePos, Vector3.Zero);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            particleSystem.SetCamera(view, projection);
            particleSystem.Draw(gameTime);
        }


    }
}
