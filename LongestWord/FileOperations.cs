using System;
using System.IO;

namespace LongestWord
{
    /// <summary>
    /// Helper class for file operations
    /// </summary>
    public static class FileOperations
    {
        /// <summary>
        /// Reads the text file
        /// </summary>
        /// <param name="filePath">Full physical path for text file</param>
        /// <returns>file contents</returns>
        public static string ReadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (new FileInfo(filePath).Length > (Constants.MaxFileSize))
                {
                    throw new ArgumentException("Max file size allowed is 5000KB.");
                }

                try
                {
                    var fileContent = File.ReadAllText(filePath);
                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        throw new ArgumentException("No content in selected text file.");
                    }

                    return fileContent;
                }
                catch (NotSupportedException)
                {
                    throw new NotSupportedException("Please provide only text file. Other formats are not supported yet.");
                }
            }
            else
            {
                throw new FileNotFoundException("File path is not correct.");
            }
        }
    }
}
