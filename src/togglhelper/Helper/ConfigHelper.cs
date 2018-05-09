using System.IO;
using Newtonsoft.Json;
using togglhelper.Models;

namespace togglhelper.Helper
{
    public static class ConfigHelper
    {
        public static Config GetConfig(string configFile)
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));
        }
    }
}
