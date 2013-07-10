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
    class PauseMenu: Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont normalText;
        SpriteFont selectedText;
        SpriteFont titleText;
        int selectedTextCount;
        float inputDelay;
        const float delayBetweenSwitches = 0.25f;
        SelectText[] menuList;
        OptionsMenu optionsMenu;
        bool buttonPressed;
        bool startButtonPressed;
        Texture2D backDrop;

        public PauseMenu(Game game) 
            : base(game)
        {
            selectedTextCount = 0;
            buttonPressed = false;
            startButtonPressed = true;
            normalText = Game.Content.Load<SpriteFont>(@"Fonts/NormalText");
            selectedText = Game.Content.Load<SpriteFont>(@"Fonts/SelectedText");
            titleText = Game.Content.Load<SpriteFont>(@"Fonts/TitleText");
            backDrop = Game.Content.Load<Texture2D>(@"Textures/BackDrop");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            optionsMenu = new OptionsMenu(game, @"Textures/BackDrop");

            menuList = new SelectText[] {new SelectText("RESUME", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("RESUME").Length()/2), 100), normalText, selectedText),
                new SelectText("OPTIONS", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("OPTIONS").Length()/2), 200), normalText, selectedText),
                new SelectText("MAIN MENU", spriteBatch, new Vector2((Game.Window.ClientBounds.Width/2) - (titleText.MeasureString("MAIN MENU").Length()/2), 300), normalText, selectedText)};
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
                inputDelay += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

                if ((gamepadState.ThumbSticks.Left.Y < -0.5 || keyboard.IsKeyDown(Keys.Down) || gamepadState.DPad.Down == ButtonState.Pressed) && inputDelay > delayBetweenSwitches)
                {
                    selectedTextCount = ++selectedTextCount % menuList.Length;
                    inputDelay = 0.0f;
                }
                else if ((gamepadState.ThumbSticks.Left.Y > 0.5 || keyboard.IsKeyDown(Keys.Up) || gamepadState.DPad.Up == ButtonState.Pressed) && inputDelay > delayBetweenSwitches)
                {
                    selectedTextCount = (selectedTextCount + menuList.Length-1) % menuList.Length;
                    inputDelay = 0.0f;
                }

                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 0 && !buttonPressed)
                {
                    ((Game1)Game).paused = false;
                    buttonPressed = true;
                    startButtonPressed = true;
                    selectedTextCount = 0;
                }
                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 1 && !buttonPressed)
                {
                    optionsMenu.ableToDraw = true;
                    buttonPressed = true;
                }
                if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 2 && !buttonPressed)
                {
                    ((Game1)Game).playing = false;
                    selectedTextCount = 0;
                    ((Game1)Game).paused = false;
                }
                /*if ((gamepadState.Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape)) && !startButtonPressed)
                {
                    ((Game1)Game).paused = false;
                    startButtonPressed = true;
                }*/

                if (gamepadState.Buttons.A == ButtonState.Released && keyboard.IsKeyUp(Keys.Enter))
                {
                    buttonPressed = false;
                }

                if (gamepadState.Buttons.Start == ButtonState.Released && keyboard.IsKeyUp(Keys.Escape))
                {
                    startButtonPressed = false;
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

                spriteBatch.Draw(backDrop, new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                    Game.Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero,
                    SpriteEffects.None, 0);

                for (int i = 0; i < menuList.Length; i++)
                {
                    if (i == selectedTextCount)
                        menuList[i].drawSelectedText();
                    else
                        menuList[i].drawNormalText();
                }
                spriteBatch.DrawString(titleText, "PAUSED", new Vector2((Game.Window.ClientBounds.Width / 2) - (titleText.MeasureString("PAUSED").Length() / 2), Game.Window.ClientBounds.Height - 100), Color.Yellow);
                spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}
