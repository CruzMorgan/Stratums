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

namespace Stratums
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityBatch _entityBatch;

        private Entity _dog;

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
            _entityBatch = new EntityBatch();

            TextureMetadata.GetInstance().Load();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Debugger.LoadContent(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _dog = new Entity(Content, _spriteBatch)
                .AddInertia()
                .AddAnimation("Magic_Dog_Run")
                .AddHitbox(100, 100, true)
                .AddCollider()
                ;

            int count = 1000;
            for (int x = 1; x <= (int)Math.Sqrt(count); x++)
            for (int y = 1; y <= (int)Math.Sqrt(count); y++)
            {
                _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                    .AddAnimation("WhitePixel")
                    .AddHitbox(10,10,true)
                    .OverridePosition(x*50, y*50)
                    );
            }

            _entityBatch.AddEntity( _dog );

            _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                .AddAnimation("test")
                .AddInertia()
                .OverridePosition(-300, -50)
                .AddHitbox(100, 100, true)
                .AddCollider()
                );

            _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                .OverridePosition(200, 40)
                .AddHitbox(new List<Vector2>() { new Vector2 (30, 40), new Vector2 (-56, 105), new Vector2(-103, -10), new Vector2(73, -150)}, true)
                .AddCollider()
                );

            _entityBatch.AddEntity(new Entity(Content, _spriteBatch)
                .OverridePosition(10, 400)
                .AddHitbox(300, 500, true)
                .AddCollider()
                );


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

            if (Keyboard.GetState().IsKeyDown(Keys.W)) { _dog.OverrideVelocity(0, speed); }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { _dog.OverrideVelocity(-speed, 0); }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { _dog.OverrideVelocity(0, -speed); }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { _dog.OverrideVelocity(speed, 0); }

            _entityBatch.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _entityBatch.Draw(_camera);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}