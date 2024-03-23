using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.Rendering;
using Stratums.Entities;
using Stratums.Entities.EntityPartitioning;

namespace Stratums.Properties
{
    public class Animation : Property
    {
        private ContentManager _contentManager;

        private Texture2D _texture;
        private string _textureName;
        private int _currentFrame;
        private float _elapsedTime;


        private Metadata _textureMetadata;

        private Color _color;

        public Animation(ContentManager contentManager, string textureName)
        {
            _contentManager = contentManager;
            _textureName = textureName;

            _texture = _contentManager.Load<Texture2D>(_textureName);

            _currentFrame = 0;

            _textureMetadata = TextureMetadata.GetInstance()[_textureName];
        }

        public override IEnumerable<RenderData> GetRenderData()
        {
            var frameWidth = _texture.Width / _textureMetadata.Frames;
            var x = frameWidth * (_currentFrame % _textureMetadata.Frames);
            var y = 0;

            var renderData = new RenderData()
            {
                SourceRectangle = new Rectangle(x, y, frameWidth, _texture.Height),

                DestinationRectangle = new Rectangle(0, 0, frameWidth, _texture.Height),

                Texture = _texture,

                Origin = new Vector2(frameWidth / 2, _texture.Height / 2),

                Color = _color

            };

            return new[] { renderData };
        }

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            _color = entityData.Color;

            _elapsedTime += (float)deltaTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime >= 1f / _textureMetadata.FPS)
            {
                _elapsedTime = 0;
                _currentFrame++;
            }
        }
    }
}
