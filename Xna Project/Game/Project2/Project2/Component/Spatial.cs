using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project2.Component
{
    public class Spatial:Component
    {
        public Matrix transform { get; set; }
        public Vector3 focus { get; set; }

        public Spatial(Game game, Entity.Entity parent, Vector3 position)
            : base(game, parent)
        {
            transform = Matrix.CreateTranslation(position);
        }

        public Vector3 position
        {
            get { return transform.Translation; }
            set { transform *= Matrix.CreateTranslation(value - position); }
        }

        public override void Update(GameTime gameTime)
        {
            if (focus != null)
            {
                
            }
            base.Update(gameTime);
        }
    }
}
