using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Component
{
    class Powerable: Component
    {
        float stealthed = 0;
        public float shielded = 0;
        public Powerable(Game game, Entity.Entity parent)
            :base(game, parent)
        {

        }

        public void stealth()
        {
            getComponent<Drawable3D>().turnOnStealth();
            stealthed = 10.0f;
        }

        public void shield()
        {
            new Shield(Game, parent);
        }

        public void ammo()
        {
            getComponent<WeaponController>().bulletAmmo += WeaponController.MAX_BULLETS;
            getComponent<WeaponController>().missileAmmo += WeaponController.MAX_MISSILES;
        }

        public void ring()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (stealthed > 0)
            {
                stealthed -= gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (stealthed <= 0)
                {
                    getComponent<Drawable3D>().turnOffStealth();
                }
            }
            base.Update(gameTime);
        }
    }
}
