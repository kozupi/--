using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project2.Component;
using Project2.Component.Guns;
using Project2.Events;
using Project2.Particle_Systems;


namespace Project2.Entity
{
    class Player: Entity
    {        
        public const int MAX_HEALTH = 1000;
        //const int maxNumClouds = 125;
        //int cloudsMade = 0;
        public int score { get; set; }
        float worldRadius;
        private float waitTimer;
        public Shootable Gun;
        public Shootable Missile;
        public Shootable Flare;
        //List<StationaryParticleEmitter> clouds;
        public float angleOfWings = 0.0f;
        private bool openingWings = false,closingWings = false;
        private float resetTimer = 0.0f;

        private const float speedOfWingAnimation = 0.4f;
        private const float timeToReset = 3.0f;

        public Player(Game game, Vector3 position, float worldDiameter)
            : base(game)
        {
            addComponent(new Drawable3D(game, this, Game.Content.Load<Model>(@"models\fighterPlane1")));

            addComponent(new Spatial(game, this, position));
            getComponent<Spatial>().focus = position + Vector3.Forward;
            addComponent(new Controllable(game, this));
            addComponent(new Collidable(game, this, CollisionType.player, onHit, MAX_HEALTH, 100000));

            Missile = new Missiles(game, this);
            addComponent(Missile);
            Gun = new MachineGun(game, this);
            addComponent(Gun);
            Flare = new Flare(game,this);
            addComponent(Flare);
            addComponent(new Powerable(game, this));
            addComponent(new WeaponController(game, this, Gun, Missile));

            addParticleEffects(game);

            this.worldRadius = worldDiameter;
        }

        public void addParticleEffects(Game game)
        {
            var exhaust = new ShipExhaustParticleSystem(game, ((Game1)Game).Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, exhaust, 400, float.MaxValue, getExhaustPosition));
            var flames = new FlameParticleSystem(game, Game.Content);
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, flames, 250, float.MaxValue, getFlamePositionRight));
            ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, flames, 250, float.MaxValue, getFlamePositionLeft));
        }

        public Vector3 getExhaustPosition()
        {
            return (getComponent<Spatial>().position - (getComponent<Controllable>().focus * 13));
        }

        public Vector3 getFlamePositionRight()
        {
            Controllable cont = getComponent<Controllable>();
            return (getComponent<Spatial>().position - (cont.focus * 12) + (Vector3.Cross(cont.focus, cont.up) * 1.6f));
        }

        public Vector3 getFlamePositionLeft()
        {
            Controllable cont = getComponent<Controllable>();
            return (getComponent<Spatial>().position - (cont.focus * 12) - (Vector3.Cross(cont.focus, cont.up) * 1.6f));
        }

        //void disperseClouds()
        //{
           
        //    for (int i = 0; i <= worldRadius * 2; i += ((int)(worldRadius * 2) / 5))
        //    {
        //        for (int j = 0; j <= worldRadius; j += ((int)(worldRadius) / 5))
        //        {
        //            for (int k = 0; k <= worldRadius * 2; k += ((int)(worldRadius * 2) / 5))
        //            {
        //                Vector3 initPos = new Vector3(i - worldRadius, j, k - worldRadius);
        //                makeCloud(initPos);
        //            }
        //        }
        //    }
        //    emitClouds();
        //}

        //void makeCloud(Vector3 initPos)
        //{
        //    if (initPos.Length() < worldRadius)
        //    {
        //        clouds.Add(new StationaryParticleEmitter(ref cloudParticles, initPos, 600, 400, 400, 300));
        //    }
        //}

        //void emitClouds()
        //{
        //    foreach (StationaryParticleEmitter spe in clouds)
        //    {
        //        spe.Emit();
        //    }
        //}

        public void onHit(eCollision e)
        {
            if (e.mainObject == getComponent<Collidable>())
            {
                if (e.hitObject.damage > 50)
                {
                    SoundManager.playSound("Alarm");
                    ((Game1)Game).currentVibration += 1.0f;
                }
                else if(e.hitObject.damage > 0)
                {
                    SoundManager.playSound("BulletImpact");
                    ((Game1)Game).currentVibration += 0.1f;

                }
            }
            else
            {
                if (e.mainObject.damage > 50)
                {
                    SoundManager.playSound("Alarm");
                    ((Game1)Game).currentVibration += 1.0f;
                }
                else if (e.mainObject.damage > 0)
                {
                    SoundManager.playSound("BulletImpact");
                    ((Game1)Game).currentVibration += 0.1f;
                }
            }
            Collidable c = getComponent<Collidable>();
            if ( c != null && c.health <= 0)
            {
                SoundManager.playSound("Explosion");
                removeComponent<Drawable3D>();
                Spatial sp = getComponent<Spatial>();
                var explosion = new ExplosionParticleSystem(Game, Game.Content);
                ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, explosion, 40, 2.0f,sp.position));
                var smoke = new ExplosionSmokeParticleSystem(Game, Game.Content);
                ParticleEmitterManager.addParticleEmitter(new ParticleEmitter(Game, smoke, 50, 3.0f,sp.position));                
                
                getComponent<Spatial>().position = new Vector3(1000000, 1000000, 1000000);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (getComponent<Collidable>().health > 0)
            {
                if (getComponent<Spatial>().position.LengthSquared() > (worldRadius * worldRadius) && waitTimer > .5f)
                {
                    turnPlayerAround();
                    waitTimer = 0;
                }
                else
                {
                    waitTimer += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                }

                if (getComponent<Spatial>().position.Y <= 0)
                {
                    getComponent<Collidable>().health = 0;
                    onHit(new eCollision(getComponent<Collidable>(), getComponent<Collidable>()));
                }
                updateWings();
                base.Update(gameTime);
            }
            else
            {
                resetPlayer(gameTime.ElapsedGameTime.Milliseconds/1000.0f);
            }
        }

        private void resetPlayer(float dt)
        {
            if (resetTimer < timeToReset)
            {
                resetTimer += dt;
            }
            else
            {
                addComponent(new Drawable3D(Game, this, Game.Content.Load<Model>(@"models\fighterPlane1")));
                getComponent<Spatial>().position = new Vector3(0, 1400, 0);
                getComponent<Spatial>().focus = new Vector3(0, 1400, 0) + Vector3.Forward;
                removeComponent<Controllable>();
                addComponent(new Controllable(Game, this));
                getComponent<Collidable>().health = MAX_HEALTH;
                getComponent<WeaponController>().resetAmmo();
                resetTimer = 0.0f;

                SoundManager.playSound("DoomSpawn");
            }
        }

        public void resetPlayerOnGameRestart()
        {
            addComponent(new Drawable3D(Game, this, Game.Content.Load<Model>(@"models\FighterPlane1")));
            getComponent<Spatial>().position = new Vector3(0, 1400, 0);
            getComponent<Spatial>().focus = new Vector3(0, 1400, 0) + Vector3.Forward;
            removeComponent<Controllable>();
            addComponent(new Controllable(Game, this));
            getComponent<Collidable>().health = 1000;
            getComponent<WeaponController>().resetAmmo();
            resetTimer = 0.0f;
        }

        private void updateWings()
        {
            if (openingWings)
            {
                angleOfWings -= speedOfWingAnimation;
                openingWings = angleOfWings > 0.0f;
            }
            if (closingWings)
            {
                angleOfWings += speedOfWingAnimation;
                closingWings = angleOfWings < 50.0f;
            }
        }

        public void closeWings()
        {
            if (!closingWings && angleOfWings < 49.8f)
            {
                closingWings = true;
                openingWings = false;
            }
        }

        public void openWings()
        {
            if (!openingWings && angleOfWings > 0.2f)
            {
                openingWings = true;
                closingWings = false;
            }
        }

        private void turnPlayerAround()
        {
            getComponent<Controllable>().pitch *= -1;
            getComponent<Controllable>().yaw += (float)Math.PI;

            getComponent<Drawable3D>().TurnOnBlinking(1);
        }
    }
}