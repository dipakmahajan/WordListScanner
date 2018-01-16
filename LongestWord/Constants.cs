using System;
using System.Configuration;

namespace LongestWord
{
    /// <summary>
    /// Constants to be used in the applications
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Initializes static members
        /// </summary>
        static Constants()
        {
            // Max file size config value is in KB. Converted to bytes by multiplying 1024
            MaxFileSize = 1024 * GetConfigValue("MaxFileSize", 5000);
            MaxParallelTasks = GetConfigValue("MaxParallelTasks", 25);
        }

        /// <summary>
        /// Gets Max file size
        /// </summary>
        public static int MaxFileSize { get; private set; }

        /// <summary>
        /// Max Parallel tasks
        /// </summary>
        public static int MaxParallelTasks { get; private set; }

        private static int GetConfigValue(string configName, int defaultValue)
        {
            int configValue = defaultValue;
            var configEntry = ConfigurationManager.AppSettings[configName];
            if (!string.IsNullOrWhiteSpace(configEntry))
            {
                try
                {
                    configValue = Convert.ToInt32(configEntry);
                }
                catch (FormatException)
                {
                    // Suppress Exception
                    Logger.LogInfoAsync($"Configuration do not have value for {configName}. Using default value.").Wait();
                }
            }

            return configValue;
        }
    }
}
