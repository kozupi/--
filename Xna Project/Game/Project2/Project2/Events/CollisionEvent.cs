using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Component;

namespace Project2.Events
{
    class eCollision
    {
        public Collidable mainObject {get; protected set;}
        public Collidable hitObject {get; protected set;}
        public eCollision(Collidable collidable1, Collidable collidable2)
        {
            mainObject = collidable1;
            hitObject = collidable2;
        }
    }

    delegate void CollisionEvent(eCollision e);
}
