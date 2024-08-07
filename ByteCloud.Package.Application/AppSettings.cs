using ByteCloud.Package.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ByteCloud.Package.Application
{
    /// <summary>
    /// Provides access to application settings stored in a JSON configuration file.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets the loaded configuration root.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="jsonFile">The path to the JSON configuration file. Defaults to "appsettings.json".</param>
        public AppSettings(string jsonFile = "appsettings.json")
        {
            Log.Assert(File.Exists(jsonFile), $"App settings file {jsonFile} could not be found. Is it being copied to the output folder?");

            this.fileName = jsonFile;

            Configuration = new ConfigurationBuilder()
                .AddJsonFile(jsonFile)
                .Build();
        }

        /// <summary>
        /// Retrieves a configuration section and maps it to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to map the configuration section to.</typeparam>
        /// <param name="section">The name of the configuration section to retrieve.</param>
        /// <returns>An instance of type <typeparamref name="T"/> populated with data from the configuration section.</returns>
        public T Get<T>(string section)
            where T : class, new()
        {
            Log.Assert(!string.IsNullOrWhiteSpace(section), "Section was null or whitespace");

            var result = Configuration.GetSection("EnvironmentVariables").Get<T>();
            Log.Assert(result != null, $"Could not find section {section} in {fileName}");
            return result;
        }

        /// <summary>
        /// Retrieves a configuration section based on the name of type <typeparamref name="T"/> and maps it to an object of that type.
        /// </summary>
        /// <typeparam name="T">The type of object to map the configuration section to.</typeparam>
        /// <returns>An instance of type <typeparamref name="T"/> populated with data from the configuration section.</returns>
        public T Get<T>()
            where T : class, new()
        {
            var result = Configuration.GetSection(typeof(T).Name).Get<T>();
            Log.Assert(result != null, $"Could not find section {typeof(T).Name} in {fileName}");
            return result;
        }
    }
}