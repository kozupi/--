using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Project2.Component
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

    class Controllable : Component
    {
        public float speed = 0.5f;
        public Vector3 up { get; set; }
        private const float rotationSpeed = 0.039f, rollDegeradation = 0.055f, minSpeed = 2.0f, maxSpeed = 33.0f, naturalSpeed = 6.5f, acceleration = 0.03f, startVibration = 20.0f;
        public float roll, pitch, yaw;
        public Vector3 focus, position;
        public bool inverted;
        private Vector3 trueFocus;

        public Controllable(Game game, Entity.Entity parent)
            : base(game, parent)
        {
            up = Vector3.Up;
            roll = 0;
            pitch = 0;
            yaw = 0;
            speed = 0.5f;
            inverted = false;
            trueFocus = Vector3.Normalize(new Vector3(0.0f, -3.1f, -8.0f));
            position = getComponent<Spatial>().position;            
        }        

        public void Yaw(Directions dir)
        {
            Matrix rotation = Matrix.Identity;
            switch (dir)
            {
                case Directions.RIGHT:
                    yaw -= rotationSpeed + speed/4000;
                    break;
                case Directions.LEFT:
                    yaw += rotationSpeed + speed/4000;
                    break;
            }
        }

        private void yawWithRoll(float rotateSpeed = rotationSpeed)
        {
            yaw += rotateSpeed * roll * (speed/maxSpeed) * 4;
        }

        public void Pitch(Directions dir, float rotateSpeed = rotationSpeed)
        {
            switch (dir)
            {
                case Directions.UP:
                    //if (pitch < Math.PI/2+0.5f)
                    {
                        pitch += rotateSpeed;
                    }
                    break;
                case Directions.DOWN:
                    //if (pitch > -Math.PI/2-0.5f)
                    {
                        pitch -= rotateSpeed;
                    }
                    break;
            }
        }

        public void move(Directions dir, float rotateSpeed = rotationSpeed)
        {
            switch (dir)
            {
                case Directions.RIGHT:
                    yawWithRoll(rotateSpeed);
                    if (roll > -.8f)
                        roll -= rollDegeradation * 2;
                    else
                        roll -= rollDegeradation;
                    break;
                case Directions.LEFT:
                    yawWithRoll(rotateSpeed);
                    if (roll < .8f)
                        roll += rollDegeradation * 2;
                    else
                        roll += rollDegeradation;
                    break;
                case Directions.FORWARD:
                    if (speed <= naturalSpeed-acceleration)
                    {
                        speed += acceleration;                        
                    }
                    else
                    {
                        speed += acceleration + (((maxSpeed - speed) / maxSpeed) * (float)Math.Pow(acceleration * 100, 4) / 100);
                    }
                    break;
                case Directions.BACKWARD:
                    if (speed <= naturalSpeed - acceleration)
                    {
                        speed -= acceleration;
                    }
                    else
                    {
                        speed -= acceleration + (((maxSpeed - speed) / maxSpeed) * (float)Math.Pow(acceleration * 100, 4) / 100);
                    }                       
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            checkInput();
            controlFlight();
            focus = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
            Vector3 movement = Vector3.Normalize(focus) * speed;
            up = Vector3.Transform(Vector3.Up, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
            getComponent<Spatial>().transform = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            getComponent<Spatial>().position = position + movement;
            getComponent<Spatial>().focus = Vector3.Transform(trueFocus, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0)) + getComponent<Spatial>().position;
            position += movement;
        }

        private void controlFlight()
        {
            if (roll > rollDegeradation)
            {
                roll -= rollDegeradation;
            }
            else if (roll < -rollDegeradation)
            {
                roll += rollDegeradation;
            }

            if (speed > naturalSpeed)
            {                
                speed -= (acceleration + (((maxSpeed - speed) / maxSpeed) * (float)Math.Pow(acceleration * 100, 4) / 100)) * 0.6f;
                if (speed < naturalSpeed)
                {
                    SoundManager.playSound("disengage_engines");
                    speed = naturalSpeed;
                }
            }
            else
            {
                SoundManager.resumeSound("engine_1");
                SoundManager.pauseSound("afterburner_1");
            }
            
            if (speed < minSpeed)
            {
                
                speed = minSpeed;
            }
            else if (speed > maxSpeed)
            {                
                speed = maxSpeed;
            }

            roll = roll % (float)(2 *Math.PI);
            pitch = pitch % (float)(2 * Math.PI);
        }

        private void checkInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
                Pitch(Directions.UP);
            if (state.IsKeyDown(Keys.Down))
                Pitch(Directions.DOWN);
            if (state.IsKeyDown(Keys.A))
                move(Directions.LEFT);
            if (state.IsKeyDown(Keys.D))
                move(Directions.RIGHT);
            if (state.IsKeyDown(Keys.W))
                move(Directions.FORWARD);
            if (state.IsKeyDown(Keys.S))
                move(Directions.BACKWARD);
            if (state.IsKeyDown(Keys.E))
                Yaw(Directions.RIGHT);
            if (state.IsKeyDown(Keys.Q))
                Yaw(Directions.LEFT);

            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if (gamepadState.ThumbSticks.Left.Y > 0)
            {
                if (((Game1)Game).invertControls)
                    Pitch(Directions.DOWN, Math.Abs(gamepadState.ThumbSticks.Left.Y) * rotationSpeed);
                else
                    Pitch(Directions.UP, Math.Abs(gamepadState.ThumbSticks.Left.Y) * rotationSpeed);
            }
            if (gamepadState.ThumbSticks.Left.Y < 0)
            {
                if (((Game1)Game).invertControls)
                    Pitch(Directions.UP, Math.Abs(gamepadState.ThumbSticks.Left.Y) * rotationSpeed);
                else
                    Pitch(Directions.DOWN, Math.Abs(gamepadState.ThumbSticks.Left.Y) * rotationSpeed);
            }
            if (gamepadState.ThumbSticks.Left.X > 0)
            {
                if (pitch < (float)Math.PI / 2.0f && pitch > -(float)Math.PI / 2.0f)
                    move(Directions.RIGHT, Math.Abs(gamepadState.ThumbSticks.Left.X) * rotationSpeed);
                else
                    move(Directions.LEFT, Math.Abs(gamepadState.ThumbSticks.Left.X) * rotationSpeed);
            }
            if (gamepadState.ThumbSticks.Left.X < 0)
            {
                if (pitch < (float)Math.PI/2.0f && pitch > -(float)Math.PI / 2.0f)
                    move(Directions.LEFT, Math.Abs(gamepadState.ThumbSticks.Left.X) * rotationSpeed);
                else
                    move(Directions.RIGHT, Math.Abs(gamepadState.ThumbSticks.Left.X) * rotationSpeed);
            }

            if (gamepadState.ThumbSticks.Right.Y > 0)
            {
                if (((Game1)Game).invertControls)
                    Pitch(Directions.DOWN, Math.Abs(gamepadState.ThumbSticks.Right.Y) * rotationSpeed / 4);
                else
                    Pitch(Directions.UP, Math.Abs(gamepadState.ThumbSticks.Right.Y) * rotationSpeed / 4);
            }
            if (gamepadState.ThumbSticks.Right.Y < 0)
            {
                if (((Game1)Game).invertControls)
                    Pitch(Directions.UP, Math.Abs(gamepadState.ThumbSticks.Right.Y) * rotationSpeed / 4);
                else
                    Pitch(Directions.DOWN, Math.Abs(gamepadState.ThumbSticks.Right.Y) * rotationSpeed / 4);
            }
            if (gamepadState.ThumbSticks.Right.X > 0)
            {
                if (pitch < (float)Math.PI / 2.0f && pitch > -(float)Math.PI / 2.0f)
                    move(Directions.RIGHT, Math.Abs(gamepadState.ThumbSticks.Right.X) * rotationSpeed / 4);
                else
                    move(Directions.LEFT, Math.Abs(gamepadState.ThumbSticks.Right.X) * rotationSpeed / 4);
            }
            if (gamepadState.ThumbSticks.Right.X < 0)
            {
                if (pitch < (float)Math.PI / 2.0f && pitch > -(float)Math.PI / 2.0f)
                    move(Directions.LEFT, Math.Abs(gamepadState.ThumbSticks.Right.X) * rotationSpeed / 4);
                else
                    move(Directions.RIGHT, Math.Abs(gamepadState.ThumbSticks.Right.X) * rotationSpeed / 4);
            }

            if (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                move(Directions.BACKWARD);
            }
            if (gamepadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                move(Directions.FORWARD);
            }

            if (speed > startVibration)
            {
                SoundManager.resumeSound("afterburner_1");
                SoundManager.pauseSound("engine_1");
                ((Game1)Game).shakeScreen(.016f);
                ((Game1)Game).currentVibration += (1.0f - ((Game1)Game).currentVibration) * (speed - startVibration) / (maxSpeed - startVibration);
            }

            if ((speed - naturalSpeed)/(maxSpeed - naturalSpeed) > 0.1f)
            {
                ((Entity.Player)parent).closeWings();
            }
            else
            {
                ((Entity.Player)parent).openWings();
            }
        }
    }
}
