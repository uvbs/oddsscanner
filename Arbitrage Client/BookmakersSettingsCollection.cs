using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using BetsLibrary;
using System.IO;

namespace Arbitrage_Client
{
    public static class BookmakersSettingsCollection
    {
        private const string filePath = "bookmakers.set";
        private static Dictionary<Bookmaker, BookmakerSettings> settingsDict = new Dictionary<Bookmaker, BookmakerSettings>();
        
        public static BookmakerSettings Get(Bookmaker bookmaker)
        {
            if (!settingsDict.TryGetValue(bookmaker, out BookmakerSettings settings))
            {
                settings = new BookmakerSettings();
                settingsDict.Add(bookmaker, settings);
            }
            return settings;
        }

        static BookmakersSettingsCollection()
        {
            if (!File.Exists(filePath)) return;

            JsonSerializer serializer = new JsonSerializer();
            
            using (StreamReader sw = new StreamReader(filePath))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                settingsDict = serializer.Deserialize<Dictionary<Bookmaker, BookmakerSettings>>(reader);
            }


        }

        public static void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, settingsDict);
            }
        }
        
    }
}
