using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project2.Component;
using Project2.Entity_Managers;

namespace Project2.Entity
{
    class RollingText:Entity
    {
        String currentString = "";
        String finalString;
        float lifeTime = 0.0f, secondsPerLetter;
        public static RollingTextManager rollingTextManager;

        public RollingText(Game game, String finalString, Vector2 position, float secondsPerLetter = 0.15f,float scale = 1.0f)
            : base(game)
        {
            addComponent(new Spatial2D(game, this, position, new Vector2(scale)));
            addComponent(new DrawableString(game, this, currentString, Color.Lime));
            this.finalString = finalString;
            this.secondsPerLetter = secondsPerLetter;
            rollingTextManager.addRollingText(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (currentString.Length < finalString.Length)
	        {
                lifeTime += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                int numberOfLetters = (int)(lifeTime / secondsPerLetter);
                if (numberOfLetters > finalString.Length)
                {
                    numberOfLetters = finalString.Length;
                }
                currentString = finalString.Substring(0, numberOfLetters);
	        }
            getComponent<DrawableString>().text = currentString;
            base.Update(gameTime);		             
        }

        public static void init(Game game)
        {
            rollingTextManager = new RollingTextManager(game);
        }

        public override void dispose()
        {
            rollingTextManager.removeRollingText(this);
            base.dispose();
        }
    }
}
