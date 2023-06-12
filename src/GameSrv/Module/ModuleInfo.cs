using System.Reflection;
using System.Text.Json.Serialization;

namespace GameSrv
{
    public class ModuleInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isBundledWithHost")]
        public bool IsBundledWithHost { get; set; }

        [JsonPropertyName("version")]
        public Version Version { get; set; }

        public Assembly Assembly { get; set; }
    }
}
