# WordListScanner
 It is is a simple program which parses a text file and finds following things in it.
 1. The longest word in the file that can be constructed by concatenating copies of shorter words also found in the file
 2. The second longest word found as well
 3. Total count of words in the list can be constructed of other words in the list

# Assumptions:
 1. Only text files are accepted
 2. Text file is supposed to contain one word per line
 3. The application can be optimized further for performance

# Approach
 - Parallel tasks are created to check if a word is constructed by concatenating copies of shorter words also found in the file
 - ConcurrentBag collects all combination words from parallel tasks
 - Finally, this ConcurrentBag is used to get the stats
 - Since the task can be long running, results are logged into text file

# Sample Results
Code include couple of word lists for testing. The result for test fiel LargeList.txt
 - Largest combination word : {largestCombination}, having length {largestCombination.Length}
 - Second Largest combination word : {secondLargestCombination}, having length {secondLargestCombination.Length}
 - Total '{combinationCount}' words in the list can be constructed of other words in the list
