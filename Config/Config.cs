namespace Prometheum.Config {
    public class ConfigFile {
        public string Token { get; set; }
        public string[] Prefixes { get; set; }
        public string TesterToken { get; set; }
        public string MongoDatabaseName { get; set; }
        public string MongoConnectionURL { get; set; } 
    }
}