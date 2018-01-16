using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LongestWord
{
    /// <summary>
    /// Logging Helper
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Intitializes logger
        /// </summary>
        static Logger()
        {
            if(!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }
        }

        public static async Task LogInfoAsync(string logMessage)
        {
            using (StreamWriter w = File.AppendText("logs/info.log"))
            {
                await LogAsync(logMessage, w);
            }
        }


        public static async Task LogErrorAsync(string errorMessage)
        {
            using (StreamWriter w = File.AppendText("logs/error.log"))
            {
                await LogAsync(errorMessage, w);
            }
        }

        public static Task LogResultAsync(string message)
        {
            return Task.Run(async () =>
            {
                using (StreamWriter w = File.AppendText("Result.txt"))
                {
                    var taskList = new List<Task>
                    {
                        w.WriteLineAsync($"{Environment.NewLine}Results: "),
                        w.WriteLineAsync(message),
                        w.WriteLineAsync("-------------------------------")
                    };

                    await Task.WhenAll(taskList);
                }
            });
        }

        private static Task LogAsync(string message, TextWriter w)
        {
            return Task.Run(async () =>
            {
                var taskList = new List<Task>
                {
                    w.WriteLineAsync($"{Environment.NewLine}Log Entry : "),
                    w.WriteLineAsync($"Timestamp: {DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()}"),
                    w.WriteLineAsync($"Message : {message}"),
                    w.WriteLineAsync("-------------------------------")
                };

                await Task.WhenAll(taskList);
            });
        }
    }
}
