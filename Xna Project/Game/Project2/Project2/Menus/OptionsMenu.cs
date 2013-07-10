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
    class OptionsMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont normalText;
        SpriteFont selectedText;
        SpriteFont titleText;
        int selectedTextCount;
        float inputDelay;
        const float delayBetweenSwitches = 0.25f;
        SelectText[] menuList;
        public bool ableToDraw { get; set; }
        Texture2D background;

        public OptionsMenu(Game game, String backgroundTexture) :base(game)
        {
            selectedTextCount = 0;
            ableToDraw = false;
            normalText = Game.Content.Load<SpriteFont>(@"Fonts/NormalText");
            selectedText = Game.Content.Load<SpriteFont>(@"Fonts/SelectedText");
            titleText = Game.Content.Load<SpriteFont>(@"Fonts/TitleText");
            background = Game.Content.Load<Texture2D>(backgroundTexture);
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            menuList = new SelectText[] {new SelectText("INVERSE CONTROLS: " + ((Game1)Game).invertControls, spriteBatch, new Vector2(((Game1)Game).Window.ClientBounds.Width/2, 100), normalText, selectedText),
                new SelectText("VOLUME: " + (int)(SoundManager.volume), spriteBatch, new Vector2(((Game1)Game).Window.ClientBounds.Width/2, 200), normalText, selectedText),
                new SelectText("BACK", spriteBatch, new Vector2(((Game1)Game).Window.ClientBounds.Width/2, 300), normalText, selectedText)};
        }

        public override void Update(GameTime gameTime)
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
                selectedTextCount = (selectedTextCount + menuList.Length - 1) % menuList.Length;
                inputDelay = 0.0f;
            }


            if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 0 && inputDelay > delayBetweenSwitches)
            {
                ((Game1)Game).invertControls = !((Game1)Game).invertControls;
                menuList[0].updateText("INVERSE CONTROLS: " + ((Game1)Game).invertControls);
            }

            if ((gamepadState.ThumbSticks.Left.X < -0.5 || keyboard.IsKeyDown(Keys.Left) || gamepadState.DPad.Left == ButtonState.Pressed) && selectedTextCount == 1 && inputDelay > delayBetweenSwitches)
            {
                if (SoundManager.volume >= 1)
                {
                    SoundManager.volume -= 1;
                    menuList[1].updateText("VOLUME: " + (int)(SoundManager.volume));
                    inputDelay = 0.0f;
                }                
            }

            if ((gamepadState.ThumbSticks.Left.X > 0.5 || keyboard.IsKeyDown(Keys.Right) || gamepadState.DPad.Right == ButtonState.Pressed) && selectedTextCount == 1 && inputDelay > delayBetweenSwitches)
            {
                if (SoundManager.volume <= 9)
                {
                    SoundManager.volume += 1;
                    menuList[1].updateText("VOLUME: " + (int)(SoundManager.volume));
                    inputDelay = 0.0f;
                }
            }

            if ((gamepadState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)) && selectedTextCount == 2 && inputDelay > delayBetweenSwitches)
            {
                ableToDraw = false;
                selectedTextCount = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
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
            spriteBatch.DrawString(titleText, "OPTIONS", new Vector2((Game.Window.ClientBounds.Width / 2) - (titleText.MeasureString("OPTIONS").Length() / 2), Game.Window.ClientBounds.Height - 100), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
