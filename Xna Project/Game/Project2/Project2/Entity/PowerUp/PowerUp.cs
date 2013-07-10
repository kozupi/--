using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Events;
using Project2.Entity_Managers;

namespace Project2.Entity
{
    enum powerType
    {
        stealth = 0,
        shield = 1,
        ammo = 2,
        ring
    }
    class PowerUp:Entity
    {
        float currentRotation = 0.0f;
        powerType type;
        public static PowerUpManager powerUpManager;
        public PowerUp(Game game, Vector3 position, powerType type)
            : base(game)
        {
            addComponent(new Spatial(game, this, position));
            getComponent<Spatial>().transform = Matrix.CreateScale(20.0f) * Matrix.CreateTranslation(position);
            if (type == powerType.ammo)
            {
                addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"models\Ammo Refill")));
                getComponent<Spatial>().transform = Matrix.CreateScale(20.0f) * Matrix.CreateRotationZ((float)Math.PI / 2) * Matrix.CreateTranslation(position);
            }
            else if (type == powerType.ring)
            {
                getComponent<Spatial>().transform = Matrix.CreateScale(25.0f) * Matrix.CreateRotationX((float)Math.PI/2) * Matrix.CreateTranslation(position);
                addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"Models\Ring2")));
            }
            else
            {
                addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"models\NewGameWorld")));
            }
            addComponent(new Collidable(game, this, CollisionType.powerUp, onHit, 1, 0));
            this.type = type;
            powerUpManager.addPowerUp(this);
        }

        public override void Update(GameTime gameTime)
        {
            currentRotation += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            Vector3 position = getComponent<Spatial>().position;
            getComponent<Spatial>().transform = Matrix.CreateScale(25.0f) * Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateRotationY(currentRotation) * Matrix.CreateTranslation(position);
            base.Update(gameTime);
        }

        public static void init(Game game)
        {
            powerUpManager = new PowerUpManager(game);
        }

        public void onHit(eCollision e)
        {
            Collidable otherObject;
            if (e.hitObject == getComponent<Collidable>())
            {
                otherObject = e.mainObject;
            }
            else
            {
                otherObject = e.hitObject;
            }

            if (otherObject.getComponent<Powerable>() != null)
            {
                switch (type)
                {
                    case powerType.stealth:
                        otherObject.getComponent<Powerable>().stealth();
                        break;
                    case powerType.shield:
                        otherObject.getComponent<Powerable>().shield();
                        break;
                    case powerType.ammo:
                        otherObject.getComponent<Powerable>().ammo();
                        break;
                    case powerType.ring:
                        otherObject.getComponent<Powerable>().ring();
                        break;
                    default:
                        break;
                }
                powerUpManager.removePowerUp(this);
                dispose();
            }
        }
    }
}
