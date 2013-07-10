using System;
using System.Collections.Generic;
using System.Linq;
using Indiefreaks.AOP.Profiler;
using Indiefreaks.Xna.Profiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Project2.Component;
using Project2.Entity;
using Project2.Entity_Managers;
using Project2.Menus;
using Project2.Particle_Systems;


namespace Project2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int maxNumClouds = 100;

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera { get; protected set; }
        SpriteManager spriteManager;
        SpriteFont targetingFont;
        Player player;
        Ring spawnRing;
        public const float worldRadius = 12000;
        public Random rnd = new Random();
        bool showDebug;
        public float currentVibration = 0.0f;
        int SpeedToShake = 0;
        const int SpeedToShakeMax = 4;
        float lengthOfScreenShake = 0.0f;
        float elapsedScreenShake;
        public bool playing, invertControls, paused;
        bool firstTime;

        MainMenu mainMenu;
        PauseMenu pauseMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1600;
            Content.RootDirectory = "Content";
            var profilerGameComponent = new ProfilerGameComponent(this, @"Fonts\gameFont");
            showDebug = false;
            Components.Add(profilerGameComponent);
        }

        private void loadCanyon()
        {
            //new BasicModel(this, Content.Load<Model>(@"Models\Canyon1"), Matrix.CreateScale(100));
            //new BasicModel(this, Content.Load<Model>(@"Models\Canyon2"), Matrix.CreateScale(100));
            //new BasicModel(this, Content.Load<Model>(@"Models\Canyon3"), Matrix.CreateScale(100));
            //new BasicModel(this, Content.Load<Model>(@"Models\Canyon4"), Matrix.CreateScale(100));
            //new BasicModel(this, Content.Load<Model>(@"Models\Canyon5"), Matrix.CreateScale(100));
        }

        protected override void Initialize()
        {
            invertControls = false;
            firstTime = true;

            initializeSound();
            initializeMainMenu();
            ParticleEmitterManager.init(this);
            
            elapsedScreenShake = 0;
            playing = false;
            paused = false;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            base.Initialize();
        }

        public void startGame()
        {
            playing = true;

            if (firstTime)
            {
                initializeComponents();
                initializeEntities();
                initializeModels();
                SoundManager.start();
                addComponents();
                AddSomePowerUps();
                firstTime = false;
                loadCanyon();
                //RollingText rt = new RollingText(this, "I Sure Hope This Works", new Vector2(500));
            }
            else
            {
                resetGame();
            }
        }

        private void resetGame()
        {
            player.resetPlayerOnGameRestart();
            Entity.Enemy.enemyManager.removeAllEnemies();
            Entity.Bullet.bulletManager.removeAllBullets();
        }

        private void initializeMainMenu()
        {
            mainMenu = new MainMenu(this);
        }

        private void initializeSound()
        {
            SoundManager.init(this, playerPosition,playerForward, playerUp);
            Components.Add(SoundManager.instance);
        }

        public Vector3 playerPosition()
        {
            return player.getComponent<Spatial>().position;
        }

        public Vector3 playerUp()
        {
            return player.getComponent<Spatial>().focus - player.getComponent<Spatial>().position;
        }

        public Vector3 playerForward()
        {
            return player.getComponent<Controllable>().up;
        }

        private void initializeComponents()
        {
            camera = new Camera(this, new Vector3(50, 0, 0), new Vector3(0.001f, 0.0f, -1.0f), 1.0f, worldRadius * 2, Vector3.Up);
            Component.Drawable3D.init(this);
            Component.Drawable2D.init(this,targetingFont);
            Component.DrawableString.init(Drawable2D.spriteManager);
            Component.Collidable.init(this);
            spriteManager = new SpriteManager(this);
            pauseMenu = new PauseMenu(this);
        }

        private void addComponents()
        {
            Components.Add(camera);
            Components.Add(Component.Drawable3D.modelManager);
            Components.Add(Component.Drawable2D.spriteManager);
            Components.Add(Component.Collidable.collidableManager);
            Components.Add(spriteManager);
            Components.Add(ParticleEmitterManager.particleEmitterManager);
            Components.Add(Bullet.bulletManager);
            Components.Add(Enemy.enemyManager);
            Components.Add(PowerUp.powerUpManager);
            Components.Add(RollingText.rollingTextManager);
        }

        private void initializeEntities()
        {
            Entity.Enemy.init(this);
            Entity.Bullet.init(this);
            Entity.PowerUp.init(this);
            Entity.Building.init(this);
            Entity.Shrapnel.init(this);
            Entity.RollingText.init(this);
        }

        private void initializeModels()
        {
            new BasicModel(this, Content.Load<Model>(@"Models\NewGameWorld"), Matrix.CreateScale(worldRadius));
            new BasicModel(this, Content.Load<Model>(@"Models\Buildings\Floor"), Matrix.CreateScale(new Vector3(worldRadius / 5, 1.0f, worldRadius / 5)));
            player = new Player(this, new Vector3(0, 1600, 500), worldRadius);
            spawnRing = new Ring(this, new Vector3(0, 2000, 0));
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            targetingFont = Content.Load<SpriteFont>(@"Fonts\gameFont");
            

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GamePadState controllerState = GamePad.GetState(PlayerIndex.One);
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            if (!playing)
                mainMenu.Update(gameTime);
            if (playing && paused)
                pauseMenu.Update(gameTime);

            if (playing && !paused)
            {
                player.Update(gameTime);
                updateCamera();
                ScreenShake(gameTime);

                if (controllerState.Buttons.Start == ButtonState.Pressed)
                {
                    paused = !paused;
                }
                if (keyboard.IsKeyDown(Keys.Escape))
                {
                    paused = !paused;
                }                

                if (keyboard.IsKeyDown(Keys.H))
                    showDebug = !showDebug;
            }

            ProfilingManager.Run = showDebug;
            setVibration();
            if (playing && !paused)
                base.Update(gameTime);
        }

        private void setVibration()
        {
            if (currentVibration > 0.0f)
            {
                currentVibration -= 0.1f;
            }
            else if (currentVibration < 0.0f)
            {
                currentVibration = 0.0f;
            }
            GamePad.SetVibration(PlayerIndex.One, currentVibration, currentVibration);

        }

        private void updateCamera()
        {
            Vector3 position = player.getComponent<Controllable>().position;
            Vector3 focus = player.getComponent<Controllable>().focus;
            Vector3 up = player.getComponent<Controllable>().up;
            camera.update(position - (focus * 60.0f - (up * 45)), up, position + focus * 18);
        }
       
        public void increasePlayerScore()
        {
            player.score++;
        }
        public Spatial getPlayerSpatial()
        {
            return player.getComponent<Spatial>();
        }
        public int getPlayerScore()
        {
            return player.score;
        }       
        public float getPlayerSpeed()
        {
            return player.getComponent<Controllable>().speed * 50;
        }
        public float getPlayerHealth()
        {
            return (float)player.getComponent<Collidable>().health/Player.MAX_HEALTH;
        }
        public int getBulletCount()
        {
            return player.getComponent<WeaponController>().bulletAmmo;
        }
        public int getMissileCount()
        {
            return player.getComponent<WeaponController>().missileAmmo;
        }
        public float getPlayerAltitude()
        {
            return player.getComponent<Spatial>().position.Y / 2;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);
            if (!playing)
                mainMenu.Draw(gameTime);
            if (playing)
                base.Draw(gameTime);
            if (playing && paused)
                pauseMenu.Draw(gameTime);

            /**
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.view);
            effect.Parameters["Projection"].SetValue(camera.projection);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, aWorld.getComponent<Drawable3D>().modelExtractor.vpcVerts, 0, 2);
            }

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), aWorld.getComponent<Drawable3D>().modelExtractor.vpcVerts.Length, BufferUsage.None);
            vertexBuffer.SetData(aWorld.getComponent<Drawable3D>().modelExtractor.vpcVerts);
            **/
            
        }

        public void addAnEnemy()
        {
            //new Enemy(this, new Vector3(0 + rnd.Next(-(int)worldRadius / 4, (int)worldRadius / 4), 1050, 0 + rnd.Next(-(int)worldRadius / 4, (int)worldRadius / 4)), player);
            new Enemy(this, spawnRing.getComponent<Spatial>().position, player);
        }


        public void AddSomePowerUps()
        {
            new PowerUp(this, new Vector3(-400, 900, 0), powerType.ammo);
            new PowerUp(this, new Vector3(1600, 1000, 1600), powerType.ammo);
            new PowerUp(this, new Vector3(200, 700, 500), powerType.ammo);
            new PowerUp(this, new Vector3(1250, 1050, 0), powerType.ammo);
            new PowerUp(this, new Vector3(0, 2000, 0), powerType.ammo);
            //new Shrapnel(this, Vector3.Zero);
        }        

        private void ShakeScreen(int speedToShake)
        {
            Vector3 cross = Vector3.Normalize(Vector3.Cross(camera.focus - camera.position, camera.up + new Vector3((float)rnd.Next(4) / 10.0f))) / (SpeedToShakeMax / 2.0f);
            camera.focus += cross * speedToShake;
            camera.update(camera.position, camera.up, camera.focus);
        }

        public void shakeScreen(float time)
        {
            lengthOfScreenShake += time;
        }

        public void ScreenShake(GameTime time)
        {
            if (lengthOfScreenShake > elapsedScreenShake)
            {
                SpeedToShake++;
                SpeedToShake = SpeedToShake % SpeedToShakeMax;
                ShakeScreen(SpeedToShake - SpeedToShakeMax / 2);
                elapsedScreenShake += (float)time.ElapsedGameTime.Milliseconds / 1000.0f;
            }
            else
            {
                lengthOfScreenShake = 0;
                elapsedScreenShake = 0;
            }
        }
    }


    public static class Extensions
    {
        public static Matrix align(this Vector3 original, Vector3 newOrientation)
        {
            original.Y = 0;
            original = Vector3.Normalize(original);
            float yaw = (float)Math.Acos(Vector3.Dot(original, Vector3.Normalize(new Vector3(newOrientation.X, 0, newOrientation.Z))));
            float pitch = 0.0f;
            //check if yaw is NaN
            if (yaw * 0 != 0.0f)
            {
                yaw = 0;
                pitch = (float)Math.Acos(Vector3.Dot(original, Vector3.Normalize(newOrientation)));
            }
            else if (newOrientation.X < 0)
            {
                yaw = -yaw;
                Vector3 pitchVector = Vector3.Transform(original, Matrix.CreateFromAxisAngle(Vector3.Up, yaw));
                pitch = (float)Math.Acos(Vector3.Dot(pitchVector, Vector3.Normalize(newOrientation)));
            }
            else
            {
                Vector3 pitchVector = Vector3.Transform(original, Matrix.CreateFromAxisAngle(Vector3.Up, yaw));
                pitch = (float)Math.Acos(Vector3.Dot(pitchVector, Vector3.Normalize(newOrientation)));
            }
            //check if pitch is NaN
            if (pitch * 0 != 0.0f)
            {
                pitch = 0.0f;
            }
            else if (newOrientation.Y > 0)
            {
                pitch = -pitch;
            }

            return Matrix.CreateFromYawPitchRoll(yaw, pitch, 0);
        }

        public static void clamp(this float original, float min, float max)
        {
            if (original > max)
            {
                original = max;
            }
            else if (original < min)
            {
                original = min;
            }
        }

    }
}


