using System;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prometheum.Config {
    class ConfigManager {
        public ConfigFile Config { get; private set; }
        public String FilePath { get; private set; }
        public ConfigManager(String @Path) {
            this.FilePath = Path;

            if (File.Exists(Path)) {
                Config = ParseConfig();
            } else {
                CreateConfig();
            }
        }
        private ConfigFile ParseConfig() {
            string jsonString = File.ReadAllText(FilePath);
            Console.WriteLine("Parsed Config at {0}", FilePath);
            return JsonSerializer.Deserialize<ConfigFile>(jsonString);
        }

        private void CreateConfig() {

            ConfigFile defaultConfig = new ConfigFile {
                Token = "INSERT_DISCORD_TOKEN",
                Prefixes = new string[] {"Insert", "Prefixes", "Here"}
            };


            // using FileStream createStream = File.Create("./Config.json");
            string serializedJson = JsonSerializer.Serialize<ConfigFile>(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, serializedJson);
            Console.WriteLine("[First Startup] Generated default config at {0}.  Change the values and run the app for changes to take effect.", FilePath);
        } 
    }
}