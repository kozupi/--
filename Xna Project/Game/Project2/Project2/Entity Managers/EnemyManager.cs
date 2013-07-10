using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Particle_Systems;


namespace Project2.Entity_Managers
{
    class EnemyManager : GameComponent
    {
        const int maxEnemies = 6;
        const int timeBetweenSpawns = 5000;
        int passedSpawnTime;
        public List<Enemy> enemies;
        const int numExplosionParticles = 30;
        const int numExplosionSmokeParticles = 50;
        const int numShrapnel = 50;
        public Enemy closestEnemy { get; protected set; }
        public float targetingDistance = float.MaxValue;

        public EnemyManager(Game game)
            : base(game)
        {
            enemies = new List<Enemy>();
            //shrapnel.DrawOrder = 300;
            passedSpawnTime = 0;
            //((Game1)Game).particleManager.addParticleSystem(shrapnel);

        }

        public void add(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void removeEnemy(Enemy enemy)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == enemy)
                {
                    enemies.Remove(enemy);
                    break;
                }
            }
        }

        public void removeAllEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (enemies.Count() < maxEnemies)
            {
                spawnEnemies(gameTime);
            }
            foreach (var e in enemies)
            {
                e.Update(gameTime);
            }
            getClosestEnemy();
            base.Update(gameTime);
        }

        private void spawnEnemies(GameTime gameTime)
        {
            passedSpawnTime += gameTime.ElapsedGameTime.Milliseconds;

            if (passedSpawnTime >= timeBetweenSpawns)
            {
                passedSpawnTime = 0;
                ((Game1)Game).addAnEnemy();
            }

        }

        public void getClosestEnemy()
        {
            targetingDistance = float.MaxValue;
            float lastEnemy = float.MaxValue;
            Enemy enemy = null;
            foreach (Enemy m in enemies)
            {
                Vector3 mPos = m.getComponent<Spatial>().position;
                Vector3 pPos = ((Game1)Game).getPlayerSpatial().position;
                Vector3 pFoc = Vector3.Normalize(((Game1)Game).getPlayerSpatial().focus - pPos);
                float checkDistance = (mPos - (pPos + (Vector3.Dot(mPos - pPos, pFoc) * pFoc))).Length();
                float ship = checkDistance;
                if (ship < lastEnemy && Vector3.Dot(Vector3.Normalize(mPos - pPos), pFoc) >= 0)
                {
                    targetingDistance = ship;
                    lastEnemy = ship;
                    enemy = m;
                }
            }
            closestEnemy = enemy;
        }        
    }
}
