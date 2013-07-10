using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Entity;
using Microsoft.Xna.Framework;
using Project2.Events;
using Project2.Component_Managers;

namespace Project2.Component
{
    enum CollisionType
    {
        player = 1,
        enemy = 2,
        powerUp = 3,
        environment = 4
    }
    class Collidable: Component
    {
        public CollisionEvent onCollision { get; set;}
        public CollisionType type { get; set; }
        public int health { get; set; }
        public int damage { get; set; }
        public float rangeSquared;
        public BoundingBox boundingBox;
        public static CollidableManager collidableManager {get; set;}
        public List<Collidable> children;

        public Collidable(Game game, Entity.Entity parent, CollisionType type, CollisionEvent e, int health, int damage, float range = 50.0f)
            : base(game, parent)
        {
            this.onCollision = e;
            this.type = type;
            this.health = health;
            this.damage = damage;
            collidableManager.addCollidable(this);
            children = new List<Collidable>();
            rangeSquared = range * range;
        }

        public Collidable(Game game, Entity.Entity parent, CollisionType type, CollisionEvent e, int health, int damage,Collidable parentCollidable, float range = 50.0f)
            : base(game, parent)
        {
            this.onCollision = e;
            this.type = type;
            this.health = health;
            this.damage = damage;
            children = new List<Collidable>();
            parentCollidable.addChild(this);
            rangeSquared = range * range;
        }


        public Collidable(Game game, Entity.Entity parent, CollisionType type, CollisionEvent e, int health, int damage, BoundingBox boundingBox)
            : base(game, parent)
        {
            this.onCollision = e;
            this.type = type;
            this.health = health;
            this.damage = damage;
            collidableManager.addCollidable(this);
            children = new List<Collidable>();
            this.boundingBox = boundingBox;
        }

        public Collidable(Game game, Entity.Entity parent, CollisionType type, CollisionEvent e, int health, int damage, Collidable parentCollidable, BoundingBox boundingBox)
            : base(game, parent)
        {
            this.onCollision = e;
            this.type = type;
            this.health = health;
            this.damage = damage;
            children = new List<Collidable>();
            parentCollidable.addChild(this);
            this.boundingBox = boundingBox;
        }

        public void addChild(Collidable child)
        {
            children.Add(child);
        }

        public static void init(Game game)
        {
            collidableManager = new CollidableManager(game);
        }

        public override void cleanUp()
        {
            collidableManager.removeCollidable(this);
            base.cleanUp();
        }
    }
}
