using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project2.Component.Guns;
using Project2.Component.WeaponHudElements;
using Project2.Entity;

namespace Project2.Component
{
    class WeaponController:Component
    {
        public const int MAX_MISSILES = 15;
        public const int MAX_BULLETS = 1000;
        Shootable machineGun;
        Shootable missiles;
        Retical retical;
        LockOn lockOn;
        LockOnBackGound lockOnBackground;
        public int bulletAmmo { get; set; }
        public int missileAmmo { get; set; }

        public WeaponController(Game game, Entity.Entity parent, Shootable machineGun, Shootable missiles)
            :base(game, parent)
        {
            retical = new Retical(game);
            lockOn = new LockOn(game);
            //lockOnBackground = new LockOnBackGound(game);

            this.machineGun = machineGun;
            this.missiles = missiles;
            resetAmmo();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState controllerState = GamePad.GetState(PlayerIndex.One);
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            lockOn.coolingDown = !missiles.checkCanFire();
            lockOn.Update(gameTime);

            if (bulletAmmo > 0 && (controllerState.Triggers.Right > 0 || mouse.LeftButton == ButtonState.Pressed))
            {
                if (machineGun.fire())
                {
                    bulletAmmo--;
                }
            }
            if (missileAmmo > 0 && (mouse.RightButton == ButtonState.Pressed || controllerState.Triggers.Left > 0))
            {

                if (lockOn.hasMissileLock)
                {
                    if (missiles.fire())
                    {
                        lockOn.missileFired();
                        missileAmmo--;
                    }
                }
                else
                {
                    if (missiles.fireUnguided())
                    {
                        lockOn.missileFired();
                        missileAmmo--;
                    }
                }
            }
            base.Update(gameTime);
        }        

        public override void cleanUp()
        {
            retical.dispose();
            lockOn.dispose();
            lockOnBackground.dispose();
            base.cleanUp();
        }

        public void resetAmmo()
        {
            bulletAmmo = MAX_BULLETS;
            missileAmmo = MAX_MISSILES;
        }
    }
}
