using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Component
{
    class PathingBspline:Pathing
    {
        float t;
        Vector3[] curvePath;
        Vector3 direction;
        int currentPath;
        float travelTime;
        const float MOVEMENT_SPEED = 3.0f;

        public PathingBspline(Game game, Entity.Entity parent)
            : base(game, parent)
        {
            //use this for squadron
            //r = new Random();
            curvePath = new Vector3[7];
            curvePath[0] = getComponent<Spatial>().position;
            curvePath[1] = new Vector3(rand(), randAboveZero(), rand());
            curvePath[2] = new Vector3(rand(), randAboveZero(), rand());
            curvePath[3] = new Vector3(rand(), randAboveZero(), rand());
            curvePath[4] = new Vector3(rand(), randAboveZero(), rand());
            curvePath[5] = new Vector3(rand(), randAboveZero(), rand());
            curvePath[6] = new Vector3(rand(), randAboveZero(), rand());
            currentPath = 0;

            t = float.MaxValue;
            travelTime = 2.0f;
        }

        private float rand()
        {
            return ((Game1)Game).rnd.Next(-1200, 1200);
        }

        private float randAboveZero()
        {
            return ((Game1)Game).rnd.Next(000, 1200);
        }

        public override void Update(GameTime gameTime)
        {
            if (t >= travelTime)
            {
                travelTime = (curvePath[(currentPath+1) % 7] - curvePath[currentPath % 7]).Length()/MOVEMENT_SPEED * 16;
                direction = Vector3.Normalize(curvePath[(currentPath +1) % 7] - curvePath[currentPath % 7]);
                getComponent<Spatial>().focus = direction + getComponent<Spatial>().position;
                getComponent<Spatial>().transform = Vector3.Backward.align(direction);
                getComponent<Spatial>().position = getComponent<Spatial>().focus - direction;
                direction = direction * MOVEMENT_SPEED;
                t = 0;
                ++currentPath;
            }
            else
            {
                t += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (gameTime.ElapsedGameTime.Milliseconds>0)
            {
                getComponent<Spatial>().position += direction * (16 / gameTime.ElapsedGameTime.Milliseconds);                
            }
            base.Update(gameTime);
        }
    }
}
