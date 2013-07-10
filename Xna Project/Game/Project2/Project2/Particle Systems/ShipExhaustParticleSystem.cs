using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class ShipExhaustParticleSystem : ParticleSystem
    {
        public ShipExhaustParticleSystem(Game game, ContentManager content)
            : base(game, content) { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = @"Textures/smoke";

            settings.MaxParticles = 5000;

            settings.Duration = TimeSpan.FromSeconds(.5f);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.EndVelocity = 1f;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.LightGray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 4;
            settings.MaxStartSize = 6;

            settings.MinEndSize = 9;
            settings.MaxEndSize = 20;
            settings.EmitterVelocitySensitivity = 0.3f;

            settings.BlendState = BlendState.AlphaBlend;
        }
    }
}
