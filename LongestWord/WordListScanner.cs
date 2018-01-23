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

            this.words = words.OrderByDescending(w => w.Length).ToList().ConvertAll(w => w.ToLower());
            this.PopulateDictionary();
        }

        /// <summary>
        /// The list of words (in lower case) in the provided word list in descending order by length
        /// </summary>
        private List<string> words;

        /// <summary>
        /// Dictionary of words with key as alphabet starting all words in it
        /// list of words is in descending order by length
        /// </summary>
        private Dictionary<char, IEnumerable<string>> dictionary = new Dictionary<char, IEnumerable<string>>();

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
            var chunks = this.words.Chunk(Constants.MaxParallelTasks);
            var chunkCount = chunks.Count();
            for (int i = 0; i < chunkCount; i++)
            {
                Parallel.ForEach(chunks.ElementAt(i),
                    new ParallelOptions { MaxDegreeOfParallelism = Constants.MaxParallelTasks },
                    (w, loopState, returnVal) =>
                    {
                        if (IsCombinationWord(w, String.Copy(w)))
                        {
                            combinations.Add(w);
                        }
                    });

                if (i % 100 == 0)
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

        private bool IsCombinationWord(string originalWord, string word)
        {
            if(word.Length == 0)
            {
                return false;
            }

            var matchedPart = this.dictionary[word[0]].FirstOrDefault(w => word.StartsWith(w) && !w.Equals(originalWord, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrWhiteSpace(matchedPart))
            {
                return false;
            }

            word = word.Replace(matchedPart, string.Empty);

            if (word.Length == 0)
            {
                return true;
            }

            return IsCombinationWord(originalWord, word);
        }

        private void PopulateDictionary()
        {
            var alphabets = "abcdefghijklmnopqrstuvwxyz";
            foreach (char alphabet in alphabets)
            {
                this.dictionary.Add(alphabet, this.words.Where(w => w.StartsWith(alphabet.ToString())).OrderByDescending(w => w.Length));
            }
        }
    }
}
