using System;
using System.Text;
using System.Threading.Tasks;

namespace LongestWord
{
    /// <summary>
    /// Program class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point for the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            TaskRunner(args).Wait();
        }

        private static async Task TaskRunner(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentNullException("FilePath argument is missing");
                }

                var filePath = args[0];
                var results = new StringBuilder();
                results.AppendLine($"Scanned file '{filePath}'.");

                // Read text file to get contents
                var fileContents = FileOperations.ReadTextFile(filePath);

                // Instantiate scanner to parse list
                var wordListScanner = new WordListScanner(fileContents);

                // Call method to get largest word stats
                wordListScanner.GetCombinationWordStats(results);

                await Logger.LogResultAsync(results.ToString());
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync($"Something went wrong. Details - {ex.Message}");
            }

            await Logger.LogInfoAsync("Application is shutting down.");
        }
    }
}
