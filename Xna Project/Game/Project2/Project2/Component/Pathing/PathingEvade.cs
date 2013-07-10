using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;
using Project2.Component;

namespace Project2.Component
{
    class PathingEvade:Pathing
    {
        float speed;
        const float rotationSpeed = 0.03f;
        Entity.Entity target;
        Vector3 heading;

        public PathingEvade(Game game, Entity.Entity parent, Entity.Entity entityToWanderAround, float speed)
            : base(game, parent)
        {
            Spatial sp = getComponent<Spatial>();
            heading = Vector3.Transform(sp.focus - sp.position,Matrix.CreateFromAxisAngle(new Vector3(rand(),rand(),rand()),rand(-10,10)/10.0f));
            heading = Vector3.Normalize(heading);
            this.speed = speed;
            target = entityToWanderAround;
        }

        private float rand(float min = 0, float max = Game1.worldRadius/2)
        {
            return ((Game1)Game).rnd.Next((int)min, (int)max);
        }

        public override void Update(GameTime gameTime)
        {
            Spatial sp = getComponent<Spatial>();
            Vector3 position = sp.position;
            Vector3 focus = sp.focus - sp.position;
            float dot = (float)Math.Acos(Vector3.Dot(heading, focus));
            if (dot > 0.9f)
            {
                focus = Vector3.Transform(focus, Matrix.CreateFromAxisAngle(Vector3.Cross(focus, heading), rotationSpeed));
            }
            focus = Vector3.Normalize(focus);
            position += focus * ((speed * gameTime.ElapsedGameTime.Milliseconds) + (target.getComponent<Controllable>().speed * speed * gameTime.ElapsedGameTime.Milliseconds / 16));
            if (position.Y < speed * gameTime.ElapsedGameTime.Milliseconds * 2)
            {
                focus.Y = 0.1f;
            }
            getComponent<Spatial>().transform = Vector3.Backward.align(focus);
            getComponent<Spatial>().position = position;
            getComponent<Spatial>().focus = focus + position;
            speed += speed / (float)gameTime.ElapsedGameTime.Milliseconds * 0.15f;
            base.Update(gameTime);
        }
    }
}
