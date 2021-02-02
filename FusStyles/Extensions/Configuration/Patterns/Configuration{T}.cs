using Ws.Extensions.Patterns;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using Serilog;

namespace Ws.Extensions.Configuration.Patterns
{
    public class Configuration<T> : Wrapper<T>
    {
        private static readonly ILogger _logger = Log.ForContext<Configuration<T>>();

        public Configuration(IEnumerable<KeyValuePair<string, bool>> jsonFiles)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            foreach (var file in jsonFiles)
                builder.AddJsonFile(file.Key, file.Value);

            var config = builder.Build();

            var settings = config.Get<T>();

            if (settings != null)
                Value = settings;

            //if (Value == null)
            //{
            //    try
            //    {
            //        Value = Activator.CreateInstance<T>();
            //    }
            //    catch
            //    { /* ignore exception */ }
            //}
        }

        public Configuration(string jsonFile)
        {
            T settings = default(T);

            try
            {
                settings = JsonConvert.DeserializeObject<T>(File.ReadAllText(jsonFile), new StringEnumConverter());
                Value = settings;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to deserialize from {jsonFile}", jsonFile);
            }
        }
    }
}
