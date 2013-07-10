using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;

namespace Project2.Component.WeaponHudElements
{
    class LockOn:Entity.Entity
    {
        public bool hasMissileLock { get; set; }
        float lockHeldTime = 0;
        Vector2 defualtPosition;
        Vector2 randomVector;
        int flipColor = 0;
        public bool coolingDown = false;
        const float MISSILE_LOCK_TIME = 1.25f, MISSILE_LOCK_DSITANCE = 200.0f, FASTER_LOCK_ON_DISTANCE = 25.0f;

        public LockOn(Game game)
            : base(game)
        {
            defualtPosition = new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 3 + 28);
            addComponent(new Spatial2D(game, this,defualtPosition , new Vector2(0.75f)));
            addComponent(new Drawable2D(game, this, Game.Content.Load<Texture2D>(@"Textures/LockOnCursor"), Color.Red));
        }

        private void createNewRandomVector()
        {
            Spatial2D sp = getComponent<Spatial2D>();
            Drawable2D dr = getComponent<Drawable2D>();
            randomVector = new Vector2(rnd((int)sp.position.X - dr.image.Width / 2, (int)sp.position.X + dr.image.Width / 2), rnd((int)sp.position.Y - dr.image.Height / 2, (int)sp.position.Y + dr.image.Height / 2));
        }

        private int rnd(int min, int max)
        {
            return ((Game1)Game).rnd.Next(min, max);
        }

        public override void Update(GameTime gameTime)
        {
            computeTargetDistance(gameTime);
            base.Update(gameTime);
        }

        public float computeTargetDistance(GameTime gameTime)
        {
            float distance = Enemy.enemyManager.targetingDistance;
            if (coolingDown)
            {
                //change color while cooling down
                resetPosition();
                getComponent<Drawable2D>().color = Color.White;
                if (flipColor > 5)
                {
                    getComponent<Drawable2D>().color = Color.Red;
                }
                flipColor = ++flipColor % 10;
                hasMissileLock = false;
            }
            else if (distance < MISSILE_LOCK_DSITANCE)
            {
                lockHeldTime += gameTime.ElapsedGameTime.Milliseconds * .001f;
                if (distance < FASTER_LOCK_ON_DISTANCE)
                {
                    lockHeldTime += gameTime.ElapsedGameTime.Milliseconds * .001f;
                }
                if (lockHeldTime > MISSILE_LOCK_TIME)
                {
                    hasMissileLock = true;
                }
                followEnemy();
            }
            else
            {
                hasMissileLock = false;
                lockHeldTime = 0;
                resetPosition();
            }
            return distance;
        }

        public void missileFired()
        {
            hasMissileLock = false;
            lockHeldTime = 0;
        }

        private void followEnemy()
        {
            getComponent<Drawable2D>().color = Color.Lime;
            getComponent<Drawable2D>().alpha = 255;
            float delta = lockHeldTime / MISSILE_LOCK_TIME;
            if (delta > 1)
            {
                delta = 1;
                if (flipColor > 5)
                {
                    getComponent<Drawable2D>().color = Color.Red;
                }
                flipColor = ++flipColor%10;
            }
            float offsetDelta;
            if (lockHeldTime >= MISSILE_LOCK_TIME / 2)
            {
                offsetDelta = 1 - ((delta * 2)-1);
            }
            else
            {
                offsetDelta = delta * 2;
            }
            Vector2 newPosition = Vector2.Lerp(getComponent<Spatial2D>().position,Enemy.enemyManager.closestEnemy.getComponent<Spatial2D>().position,delta);
            newPosition += (randomVector - defualtPosition) * offsetDelta;
            getComponent<Spatial2D>().position = newPosition;
        }

        private void resetPosition()
        {
            getComponent<Drawable2D>().color = Color.Black;
            getComponent<Drawable2D>().alpha = 0;
            getComponent<Spatial2D>().position = defualtPosition;
            createNewRandomVector();
        }
    }
}
