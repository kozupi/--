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


namespace Project2.Component
{
    public class Component : Microsoft.Xna.Framework.GameComponent
    {
        public Entity.Entity parent {get; protected set;}

        public Component(Game game, Entity.Entity entityParent)
            : base(game)
        {
            parent = entityParent;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public T getComponent<T>()
        {
            return parent.getComponent<T>();
        }

        public void removeComponent(Component c)
        {
            parent.removeComponent(c);
        }

        public void removeComponent<T>()
        {
            parent.removeComponent<T>();
        }

        public void disposeOfParent()
        {
            parent.dispose();
        }

        public virtual void cleanUp() 
        {
 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
