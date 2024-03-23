using Microsoft.Xna.Framework.Graphics;
using Stratums.FileIO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Singletons
{ 
    public struct Metadata
    {
        public int Frames { get; init; }
        public int FPS { get; init; }
    }

    public class TextureMetadata
    {
        private static TextureMetadata _instance;

        private const string FilePath = "C:/Users/cruph/Documents/Repositories/MonoCode/Stratums/Stratums/SaveData/texture-metadata.csv";

        private readonly ConcurrentDictionary<string, Metadata> _textureMetadata;

        public Metadata this[string textureName] => _textureMetadata[textureName];

        private TextureMetadata() 
        {
            _textureMetadata = new ConcurrentDictionary<string, Metadata>();

        }

        public static TextureMetadata GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TextureMetadata();
            }

            return _instance;
        }

        public bool TryAdd(string textureName, Metadata metadata)
        {
            return _textureMetadata.TryAdd(textureName, metadata);
        }

        public void Save()
        {
            FileReader.SaveToCsv(FilePath, _textureMetadata);
        }

        public void Load()
        {
            FileReader.LoadTextureMetadata(FilePath);
        }
    }
}
