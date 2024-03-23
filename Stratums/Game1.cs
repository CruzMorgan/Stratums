using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Stratums.Entities;
using Microsoft.Xna.Framework.Content;
using System;
using Stratums.Singletons;
using System.Collections.Generic;
using Stratums.HelperMethods;
using Stratums.Entities.EntityPartitioning;

namespace Stratums
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityBatch _entityBatch;

        private Entity _player;

        private Vector2 _camera;

        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _camera = new Vector2(-300, -100);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityBatch = new EntityBatch(2000, 2000, 5, 5);

            TextureMetadata.GetInstance().Load();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Debugger.LoadContent(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player = new Entity(Content, _spriteBatch)
                .AddInertia()
                .AddAnimation("Magic_Dog_Run")
                .AddHitbox(Vector2.Zero, 100, 100)
                .AddFriction(0.9f)
                .OverridePosition(new Vector2(-200, 0))
                //.AddGravity()
                .AddAirResistance()
                .AddDebugVisuals()
                ;


            for (int x = 0; x < 50; x++)
            for (int y = 0; y < 50; y++)
            {
                _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                    .AddAnimation("test")
                    .AddHitbox(Vector2.Zero, 100, 100)
                    .OverridePosition(x * 99 - 50, y * 99)
                    //.AddGravity()
                    //.AddAirResistance()
                    .AddRandomMovement()
                    .AddFriction()
                    .AddDebugVisuals()
                    .AddInertia()
                    );

            }
/*            for (int x = 0; x < 50; x++)
            for (int y = 0; y < 50; y++)
            {
                _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                    .AddAnimation("100x100Ball")
                    .AddHitbox(Vector2.Zero, 50)
                    .OverridePosition(x * 99, y * 99)
                    //.AddGravity()
                    //.AddAirResistance()
                    .AddRandomMovement()
                    .AddFriction()
                    .AddDebugVisuals()
                    .AddInertia()
                    );

            }*/

            _entityBatch.AddEntity( _player );
            //_entityBatch.AddEntity(_object);



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var cameraSpeed = 10;
            var speed = 10;

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) { _camera += new Vector2(0, -cameraSpeed); }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) { _camera += new Vector2(cameraSpeed, 0); }
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) { _camera += new Vector2(-cameraSpeed, 0); }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) { _camera += new Vector2(0, cameraSpeed); }

            if (Keyboard.GetState().IsKeyDown(Keys.W)) { _player.InfluenceVelocity(0, speed); }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { _player.InfluenceVelocity(-speed, 0); }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { _player.InfluenceVelocity(0, -speed); }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { _player.InfluenceVelocity(speed, 0); }

            _entityBatch.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(new Color(_player.GetEntityData().Position.X / 30000 + 0.5f, ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) / 2 + 0.5f) / 5, _player.GetEntityData().Position.Y / 30000 + 0.5f));

            _spriteBatch.Begin();

            _entityBatch.Draw(_player.GetEntityData().Position.InvertY() + _camera);

            _spriteBatch.DrawString(Debugger.Font, $"FPS = {1f / (float)gameTime.ElapsedGameTime.TotalSeconds}\nPos = {_player.GetEntityData().Position}", Vector2.Zero, Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}