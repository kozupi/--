using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Entity;

namespace Project2.Entity_Managers
{
    class RollingTextManager:GameComponent
    {
        List<RollingText> rollingText;

        public RollingTextManager(Game game)
            : base(game)
        {
            rollingText = new List<RollingText>();
        }

        public void addRollingText(RollingText rt)
        {
            rollingText.Add(rt);
        }

        public void removeRollingText(RollingText rt)
        {
            rollingText.Remove(rt);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var rt in rollingText)
            {
                rt.Update(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
