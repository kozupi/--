using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Component
{
    class PathingChase : Pathing
    {
        Entity.Entity chasing;
        float speed;
        const float rotationSpeed = 0.06f;
        bool alwaysChase;

        public PathingChase(Game game, Entity.Entity parent, Entity.Entity entityToChase, float speed,bool alwaysChase = false)
            : base(game, parent)
        {
            this.speed = speed;
            chasing = entityToChase;
            this.alwaysChase = alwaysChase;
        }

        public override void Update(GameTime gameTime)
        {
            if (chasing != null)
            {
                Spatial chase = chasing.getComponent<Spatial>();
                if (chase != null)
                {
                    Vector3 pos = getComponent<Spatial>().position;
                    if ((chase.position - pos) != Vector3.Zero)
                    {
                        Vector3 vectorBetweenPos = chase.position - pos;
                        Vector3 focus = Vector3.Normalize(chase.position - pos);
                        float dot = (float)Math.Acos(Vector3.Dot(focus, Vector3.Normalize(getComponent<Spatial>().focus - pos)));
                        if (dot < rotationSpeed || alwaysChase)
                        {
                            pos += Vector3.Normalize(vectorBetweenPos) * gameTime.ElapsedGameTime.Milliseconds * speed;
                        }
                        else
                        {
                            Vector3 foc = getComponent<Spatial>().focus - pos;
                            focus = Vector3.Transform(foc, Matrix.CreateFromAxisAngle(Vector3.Cross(foc, focus), rotationSpeed));
                            focus = Vector3.Normalize(focus);
                            pos += focus * gameTime.ElapsedGameTime.Milliseconds * speed;
                        }


                        getComponent<Spatial>().transform = Vector3.Backward.align(focus);
                        getComponent<Spatial>().focus = focus + pos;
                        getComponent<Spatial>().position = pos;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
