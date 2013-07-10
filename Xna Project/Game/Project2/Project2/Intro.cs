using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project2.Entity;
using Project2.Component;

namespace Project2
{
    class Intro:GameComponent
    {
        RollingText text;
        RollingText pressButton;
        int stage = 0;

        public Intro(Game game, Player player, Camera camera)
            :base(game)
        {
            player.removeComponent<Controllable>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void moveCamera()
        {
            GamePadState controllerState = GamePad.GetState(PlayerIndex.One);

        }
    }
}
