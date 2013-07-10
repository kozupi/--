using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Component
{
    class Shootable:Component
    {
        protected float fireRate { get; set; }
        protected float timeSinceLastShot=0;
        public Shootable(Game game, Entity.Entity parent, float fireRate = 01.5f)
            : base(game, parent)
        {
            this.fireRate = fireRate;
        }

        public virtual bool fire() { return false; }
        public virtual bool fireUnguided() { return false; }
        public override void Update(GameTime gameTime)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;            
            base.Update(gameTime);
        }

        public bool checkCanFire()
        {
            return timeSinceLastShot > fireRate;
        }
    }
}
