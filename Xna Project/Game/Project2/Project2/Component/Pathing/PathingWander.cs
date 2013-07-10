using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;
using Project2.Component;

namespace Project2.Component
{
    class PathingWander: Pathing
    {
        Entity.Entity target;
        float speed;
        const float rotationSpeed = 0.03f, evasiveRange = 4000;

        public PathingWander(Game game, Entity.Entity parent, Entity.Entity entityToWanderAround, float speed)
            : base(game, parent)
        {
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
            Vector3 wingOffset = Vector3.Cross(Vector3.Transform(Vector3.Up,sp.transform) - position, focus) * 50;
            Vector3 heading = getNewHeading();
            updateBuildingCollision(position, ref focus);
            updateBuildingCollision(position + wingOffset, ref focus);
            updateBuildingCollision(position - wingOffset, ref focus);
            float dot = (float)Math.Acos(Vector3.Dot(heading,focus));

            if (dot > 0.9f)
            {
                //rotate slowly to the new heading
                focus = Vector3.Transform(focus, Matrix.CreateFromAxisAngle(Vector3.Cross(focus, heading), rotationSpeed));
            }

            focus = Vector3.Normalize(focus);
            position += focus * ((speed * gameTime.ElapsedGameTime.Milliseconds) + (target.getComponent<Controllable>().speed * speed * gameTime.ElapsedGameTime.Milliseconds/16));
            // avoid ground            
            if (position.Y < speed * gameTime.ElapsedGameTime.Milliseconds * 2)
            {
                focus.Y = 0.1f;
            }

            getComponent<Spatial>().transform = Vector3.Backward.align(focus);
            getComponent<Spatial>().position = position;
            getComponent<Spatial>().focus = focus + position;

            base.Update(gameTime);
        }

        private Vector3 getNewHeading()
        {
            //calculate the heading we will need to get a random distance in front of the target
            Spatial opponentsSpatial = target.getComponent<Spatial>();
            Vector3 opponentFocus = Vector3.Normalize(target.getComponent<Controllable>().focus - new Vector3(0, 0.5f, 0));
            Vector3 heading = (opponentFocus * ((Game1)Game).rnd.Next(550, 950)) + opponentsSpatial.position;
            Vector3 position = getComponent<Spatial>().position;
            heading = Vector3.Normalize(heading - position);
            return heading;
        }

        private void updateBuildingCollision(Vector3 position,ref Vector3 focus)
        {
            Ray headingRay = new Ray(position, focus);
            float? collisionCheck = Collidable.collidableManager.checkCollisionAgainstEnvironment(headingRay);
            if (collisionCheck != null)
            {
                dodgeBuilding(collisionCheck,ref focus, position);
            }
        }

        private void dodgeBuilding(float? collisionCheck,ref Vector3 focus, Vector3 position)
        {
            Vector3 newFocusRight = Vector3.Transform(focus, Matrix.CreateFromAxisAngle(Vector3.Up, rotationSpeed * 10));
            Ray headingRayRight = new Ray(position, newFocusRight);
            Vector3 newFocusLeft = Vector3.Transform(focus, Matrix.CreateFromAxisAngle(Vector3.Up, -rotationSpeed * 10));
            Ray headingRayLeft = new Ray(position, newFocusLeft);
            float? nextCollisionCheckRight = Collidable.collidableManager.checkCollisionAgainstEnvironment(headingRayRight);
            float? nextCollisionCheckLeft = Collidable.collidableManager.checkCollisionAgainstEnvironment(headingRayLeft);
            if (nextCollisionCheckRight == null || (nextCollisionCheckRight > collisionCheck && nextCollisionCheckRight > nextCollisionCheckLeft))
            {
                focus = newFocusRight;
            }
            else if (nextCollisionCheckLeft == null || (nextCollisionCheckLeft > collisionCheck && nextCollisionCheckLeft > nextCollisionCheckRight))
            {
                focus = newFocusLeft;
            }
            else
            {
                focus.Y = 1.0f;
            }
        }
    }
}
