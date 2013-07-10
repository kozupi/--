using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class CloudParticleSystem : ParticleSystem
    {
        public CloudParticleSystem(Game game, ContentManager content)
            : base(game, content) { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = @"Textures/smoke";

            settings.MaxParticles = 27500;

            settings.Duration = TimeSpan.FromSeconds(9999);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.EndVelocity = 0;

            settings.MinColor = Color.LightGray *0.2f;
            settings.MaxColor = Color.LightGray *0.2f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 250;
            settings.MaxStartSize = 250;

            settings.MinEndSize = 250;
            settings.MaxEndSize = 250;

            settings.BlendState = BlendState.AlphaBlend;
        }
    }
}
