using System;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prometheum.Config {
    class ConfigManager {
        public Config Config { get; private set; }
        public String FilePath { get; private set; }
        public ConfigManager(String @Path) {
            this.FilePath = Path;

            if (File.Exists(Path)) {
                Config = ParseConfig();
            } else {
                CreateConfig();
            }
        }
        private Config ParseConfig() {
            string jsonString = File.ReadAllText(FilePath);
            Console.WriteLine("Parsed Config at {0}", FilePath);
            return JsonSerializer.Deserialize<Config>(jsonString);
        }

        private void CreateConfig() {

            Config defaultConfig = new Config {
                Token = "INSERT_DISCORD_TOKEN"
            };


            // using FileStream createStream = File.Create("./Config.json");
            string serializedJson = JsonSerializer.Serialize<Config>(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, serializedJson);
            Console.WriteLine("[First Startup] Created Default Config.json file.  You can enter the config values now.");
        } 
    }
}