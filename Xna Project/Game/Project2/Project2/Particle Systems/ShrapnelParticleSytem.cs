using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project2.Particle_Systems
{
    class ShrapnelParticleSytem: ParticleSystem
    {
        public ShrapnelParticleSytem(Game game, ContentManager content)
            : base(game, content) { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = @"Textures/Shrapnel";

            settings.MaxParticles = 100;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 50;

            settings.MinVerticalVelocity = -15;
            settings.MaxVerticalVelocity = 20;

            settings.Gravity = new Vector3(0, -20, 0);

            settings.EndVelocity = 0;

            settings.MinColor = Color.LightGray;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 7;

            settings.MinEndSize = 10;
            settings.MaxEndSize = 40;
        }
    }
}
