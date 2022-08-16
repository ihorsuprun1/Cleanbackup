using System;
using System.IO;
using System.Text.Json;

namespace CleanBackup
{
    class JsonService
    {
        private readonly string settingsPath = Environment.CurrentDirectory + @"\Settings.json";

        public JsonService()
        {
            //WriteJsonConf();
        }

        public Settings ReadJsonConf()
        {
            if (new FileInfo(settingsPath).Length == 0)
            {
                WriteJsonConf();
                // file is empty
            }
            
            string conf = File.ReadAllText(settingsPath);
            var settings = JsonSerializer.Deserialize<Settings>(conf);
            return settings;
          
        }
        private void WriteJsonConf()
        {
            
            Settings conf = new Settings();
            var json = JsonSerializer.Serialize<Settings>(conf);
            File.WriteAllText(settingsPath, json);
        }
    }
}
