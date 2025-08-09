using System.Text;
using System.Text.RegularExpressions;

using static ElementalWords.PreLoaded;

namespace ElementalWords
{
    public class ElementalWords
    {
        private static readonly List<string> keyList = [.. ELEMENTS.Keys];
        static List<string> possibilities = new();

        private const int maxPossibleCharacterLimit = 3;

        /// <summary>
        /// Entry point to the work
        /// </summary>
        /// <param name="word"></param>
        /// <returns>The final reuslt</returns>
        public static string[][] ElementalForms(string word)
        {
            List<string[]> results = CreateStringArray(word);
            return [.. results];
        }

        /// <summary>
        /// Start the work
        /// </summary>
        /// <param name="word"></param>
        /// <returns>The completed result set</returns>
        private static List<string[]> CreateStringArray(string word)
        {
            if((possibilities = [.. ElementsThatCouldCreateTheWord(word)]).Count == 0)
            {
                //if it is impossible to make the word because there are no matches
                return [];
            }

            List<string[]> finalResult = FindAllWordsThatCanBeMade(word);

            return finalResult;
        }

        /// <summary>
        /// Create a list of all the variations that can make the word
        /// </summary>
        /// <param name="word"></param>
        /// <returns>
        ///     A formatted list of the elements that can make the word, in order
        /// </returns>
        static List<string[]> FindAllWordsThatCanBeMade(string word)
        {
            string? str;
            StringBuilder lookingFor = new(maxPossibleCharacterLimit);

            // To prevent resizing too much set capacity
            // Almost arbitrary value but done so the capacity should roughly reflect the possibilities available, with some room at the top
            List<string> potentialResults = new(possibilities.Count);
            List<string> impossibleToFinish = new(possibilities.Count);

            // loop through each letter of the word, looking for matches up to the limit of the max character length
            for (int startIndex = 0; startIndex < word.Length; startIndex++)
            {
                for (int i = 1; i <= maxPossibleCharacterLimit; i++)
                {
                    // if we will be taken outside the bounds of the word
                    // in a 5 letter word, there is no point sarting on letter 4 and looking for 2 characters
                    if (startIndex + i > word.Length)
                    {
                        continue;
                    }

                    lookingFor.Append(word.AsSpan(startIndex, i));

                    if ((str = possibilities.FirstOrDefault(x => x.Equals(lookingFor.ToString(), StringComparison.OrdinalIgnoreCase))) is not null)
                    {
                        List<string> resultsThatNeedTheLookingForString = potentialResults.FindAll((x => x.Equals(word.Substring(0, startIndex), StringComparison.CurrentCultureIgnoreCase)));
                        if(resultsThatNeedTheLookingForString.Count > 0)
                        {
                            foreach(string s in resultsThatNeedTheLookingForString)
                            {
                                potentialResults[potentialResults.IndexOf(s)] = s + str;

                                // if there may yet be a result that could match, but might be longer we should re-add the string.
                                // Eg for 'snack', we could use the S finding SN, but also have SNa to find.
                                // We need to keep the 'S' around as we haven't hit the character limit yet.
                                if (i < maxPossibleCharacterLimit)
                                {
                                    potentialResults.Add(s);
                                }
                            }
                        }
                        else if(startIndex == 0)
                        {
                            // We've just started, so add the string
                            potentialResults.Add(str);
                        }
                    }
                    lookingFor.Clear();
                }

                // Some words will be impossible to finish now,
                // We may have found the first letter only and now be looking for the third. These need removing.
                // I'd rather not add them :/
                if ((impossibleToFinish = potentialResults.Where(x => word.Length - x.Length >= word.Length - startIndex).ToList()).Count > 0)
                {
                    impossibleToFinish.ForEach(x => potentialResults.Remove(x));
                }
            }

            List<string[]> formattedResults = FormatListItems([.. potentialResults]);
            potentialResults.Clear();
            impossibleToFinish.Clear();

            return formattedResults;
        }

        /// <summary>
        /// The list only contains the word as Elemental symbols,
        /// this takes those and converts them to the format required
        /// </summary>
        /// <param name="ListOfWords"></param>
        /// <returns>
        /// List of a List of strings, formatted correctly
        /// </returns>
        private static List<string[]> FormatListItems(List<string> ListOfWords)
        {
            List<string[]> results = new();
            string regexToSplitResultsByCapitalLetter = @"(?<!^)(?=[A-Z])";

            ListOfWords.ForEach(x =>
            {
                string[] elements = Regex.Split(x, regexToSplitResultsByCapitalLetter);

                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i] = $"{ELEMENTS[elements[i]]} ({elements[i]})";
                }

                results.Add(elements);
            });

            return results;
        }

        /// <summary>
        /// Get all symbols that could make the word
        /// </summary>
        /// <param name="word"></param>
        /// <returns>List of elements that might create the word</returns>
        private static List<string> ElementsThatCouldCreateTheWord(string word)
        {
            List<string> ListOfChemicalSymbolsThatExistInTheWord = [..keyList.Where(
                x => word.Contains(x!, StringComparison.OrdinalIgnoreCase))];

            return ListOfChemicalSymbolsThatExistInTheWord;
        }
    }
}
