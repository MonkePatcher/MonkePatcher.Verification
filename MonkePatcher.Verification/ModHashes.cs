using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkePatcher.Verification
{
    public class ModHashes
    {
        private static readonly string url = "https://raw.githubusercontent.com/MonkePatcher/resources/main/mods.json";

        private ModHashes() {}

        /// <summary>
        /// Gets all the verified and hashed mods from the MonkePatcher resources repository
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ModHashes>? GetHashes()
        {
            using HttpClient client = new(); // cba to close manually
            var response = client.GetAsync(url).Result;
            string json = response.Content.ReadAsStringAsync().Result;

            return JsonSerializer.Deserialize<Dictionary<string, ModHashes>>(json);
        }

        /// <summary>
        /// The hash of the actual SO mod
        /// </summary>
        [JsonPropertyName("soHash")]
        public string? SOHash { get; set; }

        /// <summary>
        /// The hash of the entrie QMOD
        /// </summary>
        [JsonPropertyName("qmodHash")]
        public string? QModHash { get; set; }
    }
}