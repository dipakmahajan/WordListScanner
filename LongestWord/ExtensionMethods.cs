using System;
using System.Collections.Generic;

namespace LongestWord
{
    /// <summary>
    /// Class to hold extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Chunks the list into lists of given size
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="list">list instance</param>
        /// <param name="chunkSize">chunk size</param>
        /// <returns>chunks</returns>
        public static IEnumerable<List<T>> Chunk<T>(this List<T> list, int chunkSize = 30)
        {
            for (int i = 0; i < list.Count; i += chunkSize)
            {
                yield return list.GetRange(i, Math.Min(chunkSize, list.Count - i));
            }
        }
    }
}
