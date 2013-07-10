using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class ShipExplosion : ParticleSystem
    {
        VertexBuffer particleVertexBuffer;

        Vector3 position;

        int lifeLeft;

        int numParticlesPerRound;
        static Random rnd = new Random();
        int roundTime;
        int timeSinceLastRound = 0;

        ParticleSettings particleSettings;

        int endOfLiveParticlesIndex = 0;
        int endOfDeadParticlesIndex = 0;
        
        public override bool isDead
        {
            get {return endOfDeadParticlesIndex == maxParticles; }
        }

        public ShipExplosion(GraphicsDevice graphicsDevice, Vector3 position, int lifeLeft,
            int roundTime, int numParticlesPerRound, int maxParticles, Texture2D particleColorsTexture,
            ParticleSettings particleSettings, Effect particleEffect) : base(graphicsDevice, particleEffect,
            particleColorsTexture, maxParticles)
        {
            this.position = position;
            this.lifeLeft = lifeLeft;
            this.numParticlesPerRound = numParticlesPerRound;
            this.roundTime = roundTime;
            this.particleSettings = particleSettings;

            InitializeParticleVertices();
        }

        public override void InitializeParticleVertices()
        {
            base.InitializeParticleVertices();

            vertexDirectionArray = new Vector3[maxParticles];

            for (int i = 0; i < maxParticles; ++i)
            {
                float size = (float)rnd.NextDouble() * particleSettings.maxSize;

                verts[i * 4] = new VertexPositionTexture(position, new Vector2(0, 0));
                verts[(i * 4) + 1] = new VertexPositionTexture(new Vector3(position.X, position.Y + size, position.Z), new Vector2(0, 1));
                verts[(i * 4) + 2] = new VertexPositionTexture(new Vector3(position.X + size, position.Y, position.Z), new Vector2(1, 0));
                verts[(i * 4) + 3] = new VertexPositionTexture(new Vector3(position.X+size, position.Y + size, position.Z), new Vector2(1, 1));

                Vector3 direction = new Vector3(
                    (float)rnd.NextDouble() * 2 - 1,
                    (float)rnd.NextDouble() * 2 - 1,
                    (float)rnd.NextDouble() * 2 - 1);
                direction.Normalize();

                direction *= (float)rnd.NextDouble();

                vertexDirectionArray[i] = direction;

                vertexColorArray[i] = colors[(rnd.Next(0, particleColorsTexture.Height) * particleColorsTexture.Width) +
                    rnd.Next(0, particleColorsTexture.Width)];
            }

            particleVertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
        }

        public override void Update(GameTime gameTime)
        {
            if (lifeLeft > 0)
                lifeLeft -= gameTime.ElapsedGameTime.Milliseconds;

            timeSinceLastRound += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastRound > roundTime)
            {
                timeSinceLastRound -= roundTime;

                if (endOfLiveParticlesIndex < maxParticles)
                {
                    endOfLiveParticlesIndex += numParticlesPerRound;
                    if (endOfLiveParticlesIndex > maxParticles)
                        endOfLiveParticlesIndex = maxParticles;
                }
                if (lifeLeft <= 0)
                {
                    if (endOfDeadParticlesIndex < maxParticles)
                    {
                        endOfDeadParticlesIndex += numParticlesPerRound;
                        if (endOfDeadParticlesIndex > maxParticles)
                            endOfDeadParticlesIndex = maxParticles;
                    }
                }
            }

            for (int i = endOfDeadParticlesIndex; i < endOfLiveParticlesIndex; ++i)
            {
                verts[i * 4].Position += vertexDirectionArray[i];
                verts[(i * 4) + 1].Position += vertexDirectionArray[i];
                verts[(i * 4) + 2].Position += vertexDirectionArray[i];
                verts[(i * 4) + 3].Position += vertexDirectionArray[i];
            }
        }

        public override void Draw(Camera camera)
        {
            graphicsDevice.SetVertexBuffer(particleVertexBuffer);
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rs;

            if (endOfLiveParticlesIndex - endOfDeadParticlesIndex > 0)
            {
                for (int i = endOfDeadParticlesIndex; i < endOfLiveParticlesIndex; ++i)
                {
                    particleEffect.Parameters["WorldViewProjection"].SetValue(
                        camera.view * camera.projection);
                    particleEffect.Parameters["particleColor"].SetValue(
                        vertexColorArray[i].ToVector4());

                    foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, verts, i * 4, 2);
                    }
                }
            }
        }

        
    }
}
