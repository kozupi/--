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
using Project2.Component;

namespace Project2
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        List<Sprite> spriteList = new List<Sprite>();
        int screenWidth;
        int screenHeight;
        Color retical;
        SpriteFont scoreDisplay;

        Texture2D radar;
        Texture2D enemyRadarIcon;
        Texture2D bulletIcon;
        Texture2D missileIcon;
        Texture2D sight;
        Texture2D missileLock;
        Texture2D MaxHealth;
        Texture2D healthBar;
        Texture2D healthHud;

        
        public SpriteManager(Game game)
            : base(game)
        {
            screenWidth = game.Window.ClientBounds.Width;
            screenHeight = game.Window.ClientBounds.Height;
            retical = Color.White;
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            var r = new Random();
            sight =Game.Content.Load<Texture2D>(@"Textures/Retical_02");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            radar = Game.Content.Load<Texture2D>(@"Textures/Radar");
            enemyRadarIcon = Game.Content.Load<Texture2D>(@"Textures/Enemy");
            scoreDisplay = Game.Content.Load<SpriteFont>(@"Fonts/gameFont");
            bulletIcon = Game.Content.Load<Texture2D>(@"Textures/BulletIcon");
            missileIcon = Game.Content.Load<Texture2D>(@"Textures/MissileIcon");
            missileLock = Game.Content.Load<Texture2D>(@"Textures/MissileLock");
            MaxHealth = Game.Content.Load<Texture2D>(@"Textures/HealthStatusMax");
            healthBar = Game.Content.Load<Texture2D>(@"Textures/healthBar");
            healthHud = Game.Content.Load<Texture2D>(@"Textures/HealthHud");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update all sprites
            foreach (var s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            // Draw the player
            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            drawRadar();

            Spatial player = ((Game1)Game).getPlayerSpatial();

            string scoreText = "Planes Shot Down: " + ((Game1)Game).getPlayerScore();
            string bulletCount = "GUN: " + ((Game1)Game).getBulletCount();       
            string missileCount = "MSSl: " + ((Game1)Game).getMissileCount();
            string altitude = "ALT: " + ((Game1)Game).getPlayerAltitude();
            string Speed = "SPEED: " + Math.Ceiling(((Game1)Game).getPlayerSpeed());
            //string playerPos = "X: " + player.position.X + ", Y: " + player.position.Y + ", Z: " + player.position.Z;
            string enemyCount = "Enemy Count: " + Entity.Enemy.enemyManager.enemies.Count();

            spriteBatch.DrawString(scoreDisplay, altitude, new Vector2(1300, screenHeight/2), Color.Lime);
            spriteBatch.DrawString(scoreDisplay, Speed, new Vector2(200, screenHeight / 2), Color.Lime);
            //spriteBatch.DrawString(scoreDisplay, playerPos, new Vector2(0 + 160, screenHeight - 60), Color.White);
            //spriteBatch.Draw(missileLock, new Vector2(screenWidth / 2, screenHeight / 3 + 40), null, retical, 0.0f, new Vector2(screenWidth / 2, screenHeight / 3 + 40), WeaponController.missileLockScale, SpriteEffects.None, 0);
            spriteBatch.Draw(sight, new Rectangle(0, 60, sight.Width, sight.Height), retical);

            if (((Game1)Game).getBulletCount() > 0)
            {
                spriteBatch.Draw(bulletIcon, new Rectangle(((Game1)Game).Window.ClientBounds.Width - 310, screenHeight - 150, bulletIcon.Width, bulletIcon.Height), Color.Lime);
                spriteBatch.DrawString(scoreDisplay, bulletCount, new Vector2(1400, (screenHeight / 2) + 350), Color.Lime);
            }
            if (((Game1)Game).getMissileCount() > 0)
            {
                spriteBatch.Draw(missileIcon, new Rectangle(((Game1)Game).Window.ClientBounds.Width - 310, screenHeight - 60, bulletIcon.Width, bulletIcon.Height), Color.Lime);
                spriteBatch.DrawString(scoreDisplay, missileCount, new Vector2(1400, (screenHeight / 2) + 450), Color.Lime);
            }

            if (((Game1)Game).getBulletCount() <= 0)
            {
                spriteBatch.Draw(bulletIcon, new Rectangle(((Game1)Game).Window.ClientBounds.Width - 310, screenHeight - 150, bulletIcon.Width, bulletIcon.Height), Color.Red);
                spriteBatch.DrawString(scoreDisplay, bulletCount, new Vector2(1400, (screenHeight / 2) + 350), Color.Red);
            }
            if (((Game1)Game).getMissileCount() <= 0)
            {
                spriteBatch.Draw(missileIcon, new Rectangle(((Game1)Game).Window.ClientBounds.Width - 310, screenHeight - 60, bulletIcon.Width, bulletIcon.Height), Color.Red);
                spriteBatch.DrawString(scoreDisplay, missileCount, new Vector2(1400, (screenHeight / 2) + 450), Color.Red);
            }
            drawHealthBar();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawHealthBar()
        {
            spriteBatch.Draw(healthHud, new Vector2(-120, -40), healthHud.Bounds, Color.LightGray, 0.0f, Vector2.Zero, new Vector2(.75f,0.25f), SpriteEffects.None, 0.0f);
            float playerHealthPercentage = ((Game1)Game).getPlayerHealth();
            spriteBatch.Draw(healthBar,new Vector2(86,21.5f), healthBar.Bounds, Color.LightGray, 0.0f, Vector2.Zero, new Vector2((530 * playerHealthPercentage) / healthBar.Width, 0.168f), SpriteEffects.None, 1.0f);
        }

        private void drawRadar()
        {
            spriteBatch.Draw(radar, new Rectangle(0, Game.Window.ClientBounds.Height - radar.Height, radar.Width, radar.Height),null, Color.Lime,0.0f,Vector2.Zero,SpriteEffects.None,0.0f);

            Spatial playerSpatial = ((Game1)Game).getPlayerSpatial();
            Vector3 vec = (playerSpatial.focus - playerSpatial.position);
            vec.Y = 0.0f;
            vec = Vector3.Normalize(vec);
            float playerRotation = (float)Math.Acos(Vector3.Dot(vec, Vector3.Forward));
            if (vec.X < 0)
            {
                playerRotation = -playerRotation;
            }

            foreach (Entity.Enemy m in Entity.Enemy.enemyManager.enemies)
            {
                Spatial enemySpatial = m.getComponent<Spatial>();
                Vector3 position = enemySpatial.position;
                Vector3 centerPosition = playerSpatial.position;
                Vector3 enemyVector = position - centerPosition;
                enemyVector = Vector3.Transform(enemyVector, Matrix.CreateFromAxisAngle(Vector3.Up,playerRotation));
                Vector2 radarSpace = new Vector2(enemyVector.X, enemyVector.Z) * 0.05f;
                if (radarSpace.LengthSquared() < 8500.0f)
                {
                    spriteBatch.Draw(enemyRadarIcon, new Rectangle((int)radarSpace.X + radar.Width/2, (int)(Game.Window.ClientBounds.Height - radar.Height/2 + radarSpace.Y), enemyRadarIcon.Width, enemyRadarIcon.Height),null, Color.Red,0.0f,Vector2.Zero,SpriteEffects.None,1.0f);
                }
            }
        }        
    }
}