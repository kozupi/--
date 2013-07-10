using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class FlameParticleSystem:ParticleSystem
    {
        public FlameParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = @"Textures/explosion";

            settings.MaxParticles = 500;

            settings.Duration = TimeSpan.FromSeconds(0.05f);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray * 0.7f;
            settings.MaxColor = Color.Gray * 0.7f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 5;
            settings.MaxStartSize = 7;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;
            settings.EmitterVelocitySensitivity = 0.2f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
