using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Project2.Menus
{
    class SelectText
    {
        string displayText;
        SpriteBatch spriteBatch;
        Vector2 position;
        SpriteFont normalText;
        SpriteFont selectedText;

        public SelectText(String displayText, SpriteBatch sb, Vector2 pos, SpriteFont nt, SpriteFont st)
        {
            this.displayText = displayText;
            spriteBatch = sb;
            position = pos;
            normalText = nt;
            selectedText = st;
        }

        public void drawNormalText()
        {
            spriteBatch.DrawString(normalText, displayText, position, Color.PaleGreen);
        }

        public void drawSelectedText()
        {
            spriteBatch.DrawString(selectedText, displayText, position, Color.Yellow);
        }

        public void updateText(string updatedText)
        {
            displayText = updatedText;
        }
    }
}
