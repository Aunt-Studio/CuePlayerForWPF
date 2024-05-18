using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HashGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: HashGenerator <AssetsFolderPath> <OutputFilePath>");
                return;
            }

            string assetsFolderPath = args[0];
            string outputFilePath = args[1];

            if (!Directory.Exists(assetsFolderPath))
            {
                Console.WriteLine("The specified assets folder path does not exist.");
                return;
            }

            Dictionary<string, string> fileHashes = new Dictionary<string, string>();

            foreach (var filePath in Directory.GetFiles(assetsFolderPath, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = GetRelativePath(assetsFolderPath, filePath);
                string fileHash = ComputeHash(filePath);
                fileHashes[relativePath] = fileHash;
            }

            string json = JsonConvert.SerializeObject(fileHashes, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(outputFilePath, json);

            Console.WriteLine($"Hashes have been successfully saved to {outputFilePath}");
        }

        private static string ComputeHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
