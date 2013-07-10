using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Particle_Systems;
namespace Project2
{
    class ParticleStarSheet
    {
        VertexPositionTexture[] verts;
        Color[] vertexColorArray;
        VertexBuffer particleVertexBuffer;

        Vector3 maxPosition;
        int maxParticles;
        static Random rand = new Random();

        GraphicsDevice graphicDevice;

        ParticleSettings particleSettings;

        Effect particleEffect;

        Texture2D particleColorsTexture;

        public ParticleStarSheet(GraphicsDevice graphicsDevice, Vector3 maxPosition, int maxParticles, Texture2D particleColorsTexture, ParticleSettings particlesSettings, Effect particleEffect)
        {
            this.maxParticles = maxParticles;
            this.graphicDevice = graphicsDevice;
            this.particleSettings = particlesSettings;
            this.particleEffect = particleEffect;
            this.particleColorsTexture = particleColorsTexture;
            this.maxPosition = maxPosition;

            InitializeParticleVertices();
        }

        private void InitializeParticleVertices()
        {
            verts = new VertexPositionTexture[maxParticles * 4];
            vertexColorArray = new Color[maxParticles];

            Color[] colors = new Color[particleColorsTexture.Width * particleColorsTexture.Height];
            particleColorsTexture.GetData(colors);

            for (int i = 0; i < maxParticles; ++i)
            {
                float size = (float)rand.NextDouble() * particleSettings.maxSize;

                Vector3 position = new Vector3(rand.Next(-(int)maxPosition.X,(int)maxPosition.X),rand.Next(-(int)maxPosition.Y,(int)maxPosition.Y),maxPosition.Z);

                verts[i * 4] = new VertexPositionTexture(position,new Vector2(0,0));

                verts[(i * 4) + 1] = new VertexPositionTexture(new Vector3(position.X, position.Y + size, position.Z), new Vector2(0,1));

                verts[(i * 4) + 2] = new VertexPositionTexture(new Vector3(position.X + size, position.Y, position.Z), new Vector2(1, 0));

                verts[(i * 4) + 3] = new VertexPositionTexture(new Vector3(position.X + size, position.Y + size, position.Z), new Vector2(1, 1));

                vertexColorArray[i] = colors[(rand.Next(0,particleColorsTexture.Height) *  particleColorsTexture.Width) + rand.Next(0,particleColorsTexture.Width)];
            }

            particleVertexBuffer = new VertexBuffer(graphicDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
        }

        public void Draw(Camera camera)
        {
            graphicDevice.SetVertexBuffer(particleVertexBuffer);

            for (int i = 0; i < maxParticles; ++i)
            {
                particleEffect.Parameters["WorldViewProjection"].SetValue(camera.view * camera.projection);

                particleEffect.Parameters["particleColor"].SetValue(vertexColorArray[i].ToVector4());

                foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip,verts,i*4,2);
                }
            }
        }
    }
}
