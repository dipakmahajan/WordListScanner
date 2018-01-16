using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestWord
{
    /// <summary>
    /// Class to scan word list
    /// </summary>
    public class WordListScanner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordListScanner" /> class.
        /// </summary>
        /// <param name="wordList">Word list, one word per line</param>
        public WordListScanner(string wordList)
        {
            var words = wordList.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (!words.Any())
            {
                throw new ArgumentException("Word List does not contain any words.");
            }

            this.Words = words.OrderByDescending(w => w.Length).ToList().ConvertAll(w => w.ToLower());
        }

        /// <summary>
        /// Gets the list of words (in lower case) in the provided word list in descending order by length
        /// </summary>
        public List<string> Words { get; private set; }

        /// <summary>
        /// Prints combination word stats
        /// </summary>
        public void GetCombinationWordStats(StringBuilder results)
        {
            var combinations = this.GetCombinations();
            if (combinations.Any())
            {
                var sortedLargestWords = combinations.OrderByDescending(w => w.Length);
                var combinationCount = combinations.Count();
                var largestCombination = sortedLargestWords.First();
                results.AppendLine($"Largest combination word : {largestCombination}, having length {largestCombination.Length}.");
                if (combinationCount >= 2)
                {
                    var secondLargestCombination = sortedLargestWords.ElementAt(1);
                    results.AppendLine($"Second Largest combination word : {secondLargestCombination}, having length {secondLargestCombination.Length}.");
                }

                results.AppendLine($"Total '{combinationCount}' words in the list can be constructed of other words in the list.");
            }
            else
            {
                results.AppendLine("No words found in the list which are contactenated.");
            }

        }

        /// <summary>
        /// Gets list of Combination words
        /// </summary>
        /// <returns>List of Combination words</returns>
        private ConcurrentBag<string> GetCombinations()
        {
            var combinations = new ConcurrentBag<string>();

            var taskList = new List<Task>();
            var chunks = this.Words.Chunk(Constants.MaxParallelTasks);
            var chunkCount = chunks.Count();
            for (int i = 0; i < chunkCount; i++)
            {
                Parallel.ForEach(chunks.ElementAt(i),
                    new ParallelOptions { MaxDegreeOfParallelism = Constants.MaxParallelTasks },
                    (w, loopState, returnVal) =>
                    {
                        if (IsCombinationWord(w))
                        {
                            combinations.Add(w);
                        }
                    });

                if (i % 10 == 0)
                {
                    taskList.Add(Logger.LogInfoAsync($"Scanned {i + 1} chunks of word list out of {chunkCount}"));
                }

                if (taskList.Count >= Constants.MaxParallelTasks)
                {
                    Task.WaitAll(taskList.ToArray());
                }
            }

            if (taskList.Any())
            {
                Task.WaitAll(taskList.ToArray());
            }

            return combinations;
        }

        private bool IsCombinationWord(string word)
        {
            var localCopy = String.Copy(word);

            var matchedParts = this.Words.Where(w => word.Contains(w) && !word.Equals(w)).OrderByDescending(w => w.Length);
            if (!matchedParts.Any())
            {
                return false;
            }

            foreach (var matchedPart in matchedParts)
            {
                localCopy = localCopy.Replace(matchedPart, string.Empty);
                if (localCopy.Length == 0)
                {
                    break;
                }
            }

            if (localCopy.Length == 0)
            {
                return true;
            }

            return false;
        }
    }
}
