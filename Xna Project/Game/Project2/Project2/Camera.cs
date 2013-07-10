using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Project2
{
    public enum Directions
    {
        RIGHT = 0,
        LEFT = 1,
        UP = 2,
        DOWN = 3,
        FORWARD = 4,
        BACKWARD = 5
    }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        public Vector3 focus { get; set; }
        public Vector3 position { get; set; }
        public Vector3 up;
        public float near, far;
        private Matrix rotation= Matrix.Identity;
        private float currentYaw = 0, currentPitch = 0, currentRoll = 0;
        private const float rotationSpeed = 0.015f;
        private const float movementSpeed = 0.51f;

        public Camera(Game game, Vector3 position, Vector3 focus, float near, float far, Vector3 up)
            : base(game)
        {
            this.position = position;
            this.focus = focus;
            this.up = up;
            this.near = near;
            this.far = far;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height, near, far);
        }

        public Camera(Game game, Vector3 position, Vector3 focus, float near, float far)
            : base(game)
        {
            this.position = position;
            this.focus = focus;
            this.up = new Vector3(0, 1, 0);
            this.near = near;
            this.far = far;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height, near, far);
        }

        private void updateProjection()
        {
            view = Matrix.CreateLookAt(position, focus, up);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            updateProjection();

            base.Initialize();
        }

        public void update(Vector3 position, Vector3 up, Vector3 focus)
        {
            // TODO: Add your update code here
            this.position = position;
            this.up = up;
            this.focus = focus;
            updateProjection();
        }

        public Vector3 rayCast(Vector2 mousePosition)
        {
            Vector3 v;
            v.X = (((2.0f * mousePosition.X) / ((Game1)Game).graphics.PreferredBackBufferWidth) - 1) / projection.M11;
            v.Y = (((2.0f * mousePosition.Y) / ((Game1)Game).graphics.PreferredBackBufferHeight) - 1) / projection.M22;
            v.Z = 1.0f;

            Matrix m = Matrix.Invert(view);
            Vector3 rayDir, rayOrigin;
            rayDir.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
            rayDir.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
            rayDir.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;
            rayOrigin.X = m.M41;
            rayOrigin.Y = m.M42;
            rayOrigin.Z = m.M43;

            return rayDir;
        }

        public void roll(Directions dir)
        {
            switch (dir)
            {
                case Directions.RIGHT:
                    rotation = Matrix.CreateFromAxisAngle(focus-position, rotationSpeed *.1f);
                    break;
                case Directions.LEFT:
                    rotation = Matrix.CreateFromAxisAngle(focus-position, -rotationSpeed *.1f);
                    break;
            }
            up = Vector3.Transform(up, rotation);
            updateProjection();
        }

        public void yaw(Directions dir)
        {
            switch (dir)
            {
                case Directions.RIGHT:
                    rotation = Matrix.CreateFromAxisAngle(up, -rotationSpeed);
                    break;
                case Directions.LEFT:
                    rotation = Matrix.CreateFromAxisAngle(up, rotationSpeed);
                    break;
            }
            focus = Vector3.Transform(focus-position, rotation) + position;
            updateProjection();
        }

        public void pitch(Directions dir)
        {
            switch (dir)
            {
                case Directions.UP:
                    //currentPitch -= rotationSpeed * .1f;
                    rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Normalize(up),focus-position), -rotationSpeed * 0.01f);
                    break;
                case Directions.DOWN:
                    //currentPitch += rotationSpeed * .1f;
                    rotation = Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Normalize(up), focus-position), rotationSpeed * 0.01f);
                    break;
            }
            up = Vector3.Normalize(Vector3.Transform(up, rotation));
            focus = Vector3.Transform(focus-position,rotation) + position;
            updateProjection();
        }

        public void resetCamera()
        {
            currentPitch = 0;
            currentRoll = 0;
            currentYaw = 0;
            view = Matrix.Identity;
            position = new Vector3(40, 0, 50);
            focus = new Vector3(0.000001f,0,0);
            up = Vector3.Up;
            updateProjection();
        }

        public void move(Directions dir)
        {
            Vector3 translation = new Vector3();
            switch (dir)
            {
                case Directions.RIGHT:
                    translation = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(up), Vector3.Normalize(focus-position))) * -movementSpeed;
                    break;
                case Directions.LEFT:
                    translation = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(up), Vector3.Normalize(focus - position))) * movementSpeed;
                    break;
                case Directions.UP:
                    translation = Vector3.Normalize(Vector3.Transform(up, Matrix.CreateFromYawPitchRoll(currentYaw, currentPitch, currentRoll))) * movementSpeed;
                    break;
                case Directions.DOWN: 
                    translation = Vector3.Normalize(Vector3.Transform(up, Matrix.CreateFromYawPitchRoll(currentYaw, currentPitch, currentRoll))) * -movementSpeed;
                    break;
                case Directions.FORWARD:
                    translation = Vector3.Normalize(focus-position) * movementSpeed;
                    break;
                case Directions.BACKWARD:
                    translation = Vector3.Normalize(focus-position) * -movementSpeed;
                    break;
            }
            focus += translation;
            position += translation;
            updateProjection();
        }
    }
}