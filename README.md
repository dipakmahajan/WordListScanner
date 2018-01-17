# WordListScanner
 It is is a simple program which parses a text file and finds following things in it.
 1. The longest word in the file that can be constructed by concatenating copies of shorter words also found in the file
 2. The second longest word found as well
 3. Total count of words in the list can be constructed of other words in the list

## Assumptions:
 1. Only text files are accepted
 2. Text file is supposed to contain one word per line

## Approach
 - Parallel tasks are created to check if a word is constructed by concatenating copies of shorter words also found in the file
 - ConcurrentBag collects all combination words from parallel tasks
 - Finally, this ConcurrentBag is used to get the stats
 - Since the task can be long running, results are logged into text file
 - The application is created in limited time, can be optimized further for performance
 - Current scanner uses .Contains(). Will be splitting word list into 26 arrays, each array containing words starting from separate letter and uses .StartsWith() function instead of .Contains() to improve performance
 - Logging is not configurable yet. Making default log level to error would increase some performance

## Sample Results
Code include couple of word lists for testing. The result for test file named LargeList.txt is
 - Largest combination word : 'electroencephalographically', having length 27
 - Second Largest combination word : 'ethylenediaminetetraacetate', having length 27
 - Total '50325' words in the list can be constructed of other words in the list
 - Time it took to calculate result is '36' minutes
