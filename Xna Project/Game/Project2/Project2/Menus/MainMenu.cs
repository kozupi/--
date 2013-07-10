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
    class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont normalText;
        SpriteFont selectedText;
        SpriteFont titleText;
        SpriteFont nameText;
        int selectedTextCount;
        bool moveSelection;
        SelectText[] menuList;
        OptionsMenu optionsMenu;
        bool buttonPressed;
        Texture2D background;
        String gameName;
        String names;

        public MainMenu(Game game) 
            : base(game)
        {
            selectedTextCount = 0;
            moveSelection = true;
            buttonPressed = false;
            normalText = Game.Content.Load<SpriteFont>(@"Fonts/NormalText");
            selectedText = Game.Content.Load<SpriteFont>(@"Fonts/SelectedText");
            titleText = Game.Content.Load<SpriteFont>(@"Fonts/TitleText");
            nameText = Game.Content.Load<SpriteFont>(@"Fonts/NameText");
            background = Game.Content.Load<Texture2D>(@"Textures/MainMenuBackground");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            optionsMenu = new OptionsMenu(game, @"Textures/MainMenuBackground");
            gameName = "CITY DEFENSE";
            names = "Ryan Adams\nBryce Ellsworth\nDoug Fresh\nJessie Morales";

            menuList = new SelectText[] {new SelectText("PLAY", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("PLAY").Length()/2), 100), normalText, selectedText),
                new SelectText("OPTIONS", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("OPTIONS").Length()/2 - titleText.MeasureString("OPTIONS").Length()/9), 200), normalText, selectedText),
                new SelectText("EXIT", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("EXIT").Length()/2), 300), normalText, selectedText)};
        }

        public override void Update(GameTime gameTime)
        {
            if (optionsMenu.ableToDraw)
            {
                optionsMenu.Update(gameTime);
            }
            else
            {
                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState keyboard = Keyboard.GetState();

                if ((gamepadState.ThumbSticks.Left.Y < -0.5 || keyboard.IsKeyDown(Keys.Down) || gamepadState.DPad.Down == ButtonState.Pressed) && moveSelection)
                {
                    if (selectedTextCount >= 2)
                        selectedTextCount = 2;
                    else
                        selectedTextCount++;

                    moveSelection = false;
                }
                if ((gamepadState.ThumbSticks.Left.Y > 0.5 || keyboard.IsKeyDown(Keys.Up) || gamepadState.DPad.Up == ButtonState.Pressed) && moveSelection)
                {
                    if (selectedTextCount <= 0)
                        selectedTextCount = 0;
                    else
                        selectedTextCount--;

                    moveSelection = false;
                }
                if (gamepadState.ThumbSticks.Left.Y > -0.5 && gamepadState.ThumbSticks.Left.Y < 0.5 && keyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyUp(Keys.Up)
                    && gamepadState.DPad.Down == ButtonState.Released && gamepadState.DPad.Up == ButtonState.Released)
                {
                    moveSelection = true;
                }

                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 0 && !buttonPressed)
                {
                    ((Game1)Game).startGame();
                    buttonPressed = true;
                }
                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 1 && !buttonPressed)
                {
                    optionsMenu.ableToDraw = true;
                    buttonPressed = true;
                }
                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 2 && !buttonPressed)
                {
                    ((Game1)Game).Exit();
                }
                if (gamepadState.Buttons.A == ButtonState.Released && keyboard.IsKeyUp(Keys.Enter))
                {
                    buttonPressed = false;
                }
            }
            base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (optionsMenu.ableToDraw)
            {
                optionsMenu.Draw(gameTime);
            }
            else
            {
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                    Game.Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero,
                    SpriteEffects.None, 0);

                for (int i = 0; i < menuList.Length; i++)
                {
                    if (i == selectedTextCount)
                        menuList[i].drawSelectedText();
                    else
                        menuList[i].drawNormalText();
                }

                spriteBatch.DrawString(titleText, gameName, new Vector2((Game.Window.ClientBounds.Width / 2) - (titleText.MeasureString("CITY DEFENSE").Length()/2), Game.Window.ClientBounds.Height - 100), Color.Yellow);
                spriteBatch.DrawString(nameText, names, new Vector2(Game.Window.ClientBounds.Width - 150, Game.Window.ClientBounds.Height - 120), Color.Yellow);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            
        }
    }
}
