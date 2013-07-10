#region File Description
//===================================================================
// DefaultTexturedQuadParticleSystemTemplate.cs
// 
// This file provides the template for creating a new Textued Quad Particle
// System that inherits from the Default Textured Quad Particle System.
//
// The spots that should be modified are marked with TODO statements.
//
// Copyright Daniel Schroeder 2008
//===================================================================
#endregion

#region Using Statements
using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Project2.Particle_Systems
{
#if (WINDOWS)
    [Serializable]
#endif
    class PlayerExhaust : DefaultSprite3DBillboardParticleSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerExhaust(Game game) : base(game) { }

        private Color[] smokeColors = { Color.WhiteSmoke, Color.LightGray, Color.DarkGray, Color.Gray, Color.Black};
        public int currentColor = 0;

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000, "Textures/Smoke", cSpriteBatch);
            LoadSmokeEvents();
            Emitter.EmitParticlesAutomatically = true;
            Emitter.ParticlesPerSecond = 25;
            Name = "PlayerExhaust";
        }

        public void LoadSmokeEvents()
        {
            ParticleInitializationFunction = InitializeParticleSmokeTrail;

            ParticleEvents.RemoveAllEvents();
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration, 500);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndSlowFadeOut, 100);
        }

        public void InitializeParticleSmokeTrail(DefaultSprite3DBillboardParticle cParticle)
        {
            cParticle.Lifetime = RandomNumber.Between(1.0f, 7.0f);

            cParticle.Position = Emitter.PositionData.Position;
            cParticle.Size = 5;
            cParticle.Color = smokeColors[currentColor];
            cParticle.Rotation = RandomNumber.Between(0, MathHelper.TwoPi);

            cParticle.Velocity = new Vector3(0, 0, 0);
            cParticle.Acceleration = Vector3.Zero;
            cParticle.RotationalVelocity = RandomNumber.Between(-MathHelper.Pi, MathHelper.Pi);

            cParticle.StartSize = cParticle.Size;
        }

        public void ChangeColor()
        {
            if (++currentColor >= smokeColors.Length)
            {
                currentColor = 0;
            }
        }
    }
}
