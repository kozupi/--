using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Events;

namespace Project2.Component_Managers
{
    class CollidableManager : GameComponent
    {
        List<Collidable> movingCollidables;
        List<Collidable> stationaryCollidables;

        public CollidableManager(Game game)
            : base(game)
        {
            movingCollidables = new List<Collidable>();
            stationaryCollidables = new List<Collidable>();
        }

        public void addCollidable(Collidable c)
        {
            if (c.type == CollisionType.environment || c.type == CollisionType.powerUp)
            {
                stationaryCollidables.Add(c);
            }
            else
            {
                movingCollidables.Add(c);
            }
        }

        public void removeCollidable(Collidable c)
        {
            if (c.type == CollisionType.environment || c.type == CollisionType.powerUp)
            {
                stationaryCollidables.Remove(c);
            }
            else
            {
                movingCollidables.Remove(c);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < movingCollidables.Count; i++)
            {
                var c1 = movingCollidables[i];
                Spatial s1 = c1.getComponent<Spatial>();
                for (int j = 0; j < movingCollidables.Count; j++)
                {
                    var c2 = movingCollidables[j];
                    if (c1.type != c2.type && c1.parent.GetType() != c2.parent.GetType())
                    {
                        Spatial s2 = c2.getComponent<Spatial>();
                        checkCollision(c1, s1, c2, s2);
                    }
                }
                for (int j = 0; j < stationaryCollidables.Count; j++)
                {
                    var c2 = stationaryCollidables[j];
                    if (c1.type != c2.type && c1.parent.GetType() != c2.parent.GetType())
                    {
                        Spatial s2 = c2.getComponent<Spatial>();
                        checkCollision(c1, s1, c2, s2);
                    }
                }
            }
            base.Update(gameTime);
        }

        private void checkCollision(Collidable c1, Spatial sp1, Collidable c2, Spatial sp2)
        {
            float distanceBetween = (sp1.position - sp2.position).LengthSquared();
            if (distanceBetween < c2.rangeSquared)
            {
                Drawable3D dw1 = c1.getComponent<Drawable3D>();
                Drawable3D dw2 = c2.getComponent<Drawable3D>();                
                if (dw1 != null && dw2 != null)
                {
                    foreach (BoundingBox bb1 in dw1.boundingBoxes)
                    {
                        Vector3[] points1 = new Vector3[2];
                        Vector3[] points2 = new Vector3[2];
                        BoundingBox collsionBoundingBox2 = new BoundingBox();
                        points1[0] = Vector3.Transform(bb1.Min, sp1.transform);
                        points1[1] = Vector3.Transform(bb1.Max, sp1.transform);
                        BoundingBox collsionBoundingBox1 = BoundingBox.CreateFromPoints(points1);
                        foreach (BoundingBox bb2 in dw2.boundingBoxes)
                        {
                            points2[0] = Vector3.Transform(bb2.Min, sp2.transform);
                            points2[1] = Vector3.Transform(bb2.Max, sp2.transform);
                            collsionBoundingBox2 = BoundingBox.CreateFromPoints(points2);
                            if (collsionBoundingBox1.Intersects(collsionBoundingBox2))
                            {
                                onCollision(c1, sp1, c2);
                            }
                        }
                    }
                }
                else
                {
                    onCollision(c1, sp1, c2);
                }
            }
        }

        public float? checkCollisionAgainstEnvironment(Ray ray)
        {
            float? collided = null;
            foreach (Collidable c in stationaryCollidables)
            {
                if (c.type == CollisionType.environment && collided == null)
                {
                    if ((ray.Position - c.getComponent<Spatial>().position).LengthSquared() < c.rangeSquared)
                    {
                        if (c.children.Count > 0)
                        {
                            collided = checkAgainstChildren(c, ray);
                        }
                        else
                        {
                            collided = checkAgainstMesh(c.getComponent<Drawable3D>(), ray);
                        } 
                    }                    
                }
            }

            return collided;
        }

        private float? checkAgainstChildren(Collidable c, Ray ray)
        {
            float? collided = null;

            for (int i = 0; i < c.children.Count && collided == null; i++)
            {
                Drawable3D dr = c.children[i].getComponent<Drawable3D>();
                if (dr != null)
                {
                    collided = checkAgainstMesh(dr, ray);                    
                }
                else
                {
                    collided = checkAgainstChildren(c.children[i], ray);
                }
            }

            return collided;
        }

        private float? checkAgainstMesh(Drawable3D dw, Ray ray)
        {
            float? collided = null;
            Spatial sp = dw.getComponent<Spatial>();
            Vector3[] points = new Vector3[2];
            for (int i = 0; i < dw.boundingBoxes.Count && collided == null; i++)
            {
                points[0] = Vector3.Transform(dw.boundingBoxes[i].Min, sp.transform);
                points[1] = Vector3.Transform(dw.boundingBoxes[i].Max, sp.transform);
                collided = BoundingBox.CreateFromPoints(points).Intersects(ray);
            }
            return collided;
        }

        private void onCollision(Collidable c1, Spatial sp1, Collidable c2)
        {
            c1.health -= c2.damage;
            c2.health -= c1.damage;
            c1.onCollision(new eCollision(c1, c2));
            c2.onCollision(new eCollision(c1, c2));

            for (int i = 0; i < c2.children.Count; i++)
            {
                checkCollision(c1, sp1, c2.children[i], c2.children[i].getComponent<Spatial>());
            }
        }
    }
}
