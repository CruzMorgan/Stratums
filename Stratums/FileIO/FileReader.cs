using Stratums.Singletons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.FileIO
{
    public class FileReader
    {
        public static void LoadTextureMetadata(string filePath)
        {
            var textureMetadata = TextureMetadata.GetInstance();
            try
            {
                using StreamReader reader = new StreamReader(filePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 3)
                    {
                        string name = parts[0];

                        if (int.TryParse(parts[1], out var frames) && int.TryParse(parts[2], out var fps))
                        {
                            textureMetadata.TryAdd
                            (
                                name, 
                                new Metadata
                                {
                                    Frames = frames,
                                    FPS = fps
                                }
                            );
                        }
                        else
                        {
                            Console.WriteLine("Error parsing values in line: " + line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid line format: " + line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
        }

        public static void SaveToCsv(string filePath ,IEnumerable<KeyValuePair<string, Metadata>> data)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(filePath);
                foreach (var (name, metadata) in data)
                {
                    writer.WriteLine($"{name},{metadata.Frames},{metadata.FPS}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file: " + ex.Message);
            }
        }
    }
}
