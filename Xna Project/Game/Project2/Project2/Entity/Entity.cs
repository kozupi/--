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

namespace Project2.Entity
{
   
    public class Entity : Microsoft.Xna.Framework.GameComponent
    {
        protected List<Component.Component> components;
   
        public Entity(Game game)
            : base(game)
        {
            components = new List<Component.Component>();
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public T getComponent<T>()
        {
            for (int i = 0; i < components.Count(); i++)
            {
                if (components[i] is T)
                {
                    Type conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    T componentToReturn = (T)Convert.ChangeType(components[i], conversionType);
                    return componentToReturn;
                }
            }
            return default(T);
        }

        public void addComponent(Component.Component newComponent)
        {
            components.Add(newComponent);
        }

        public virtual void dispose()
        {
            for (int i = 0; i < components.Count(); i++)
            {
                components[i].cleanUp();
                removeComponent(components[i--]);
            }
        }

        public void removeComponent<T>()
        {
            for (int i = 0; i < components.Count(); i++)
            {
                if (components[i] is T)
                {
                    components[i].cleanUp();
                    components.RemoveAt(i);
                    i--;
                }
            }
        }

        public void removeComponent(Component.Component c)
        {
            for (int i = 0; i < components.Count(); i++)
            {
                if (components[i] == c)
                {
                    components[i].cleanUp();
                    components.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var comp in components)
            {
                comp.Update(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
