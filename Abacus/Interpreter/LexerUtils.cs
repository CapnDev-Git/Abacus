using System.Collections.Generic;
using System.Linq;

namespace Abacus.Interpreter
{
    public partial class Lexer
    {
        /**
         * <summary> Updates all the variables so far with the given result of the current evaluated expression. </summary>
         * <param name="res"> The result of the current expression given by the AST </param>
         */
        public void UpdateRam(int res)
        {
            // Clear the RAM of duplicates
            RemoveDuplicates();
            
            // Update each variable's value from current call evaluation
            foreach (var variable in _ram.Where(variable => variable.Value == null))
            {
                variable.Value = res;
            }
        }
        
        /**
         * <summary> Splits the user input into multiple expression strings. </summary>
         * <param name="input"> The string input directly from Stdin input. </param>
         * <returns> List of tokens in order of appearance. </returns>
         */
        public List<string> SplitInput(string input)
        {
            var splitStrings = input.Split(';').ToList();
            splitStrings.RemoveAll(item => item == "");
            return splitStrings;
        }
        
        /**
         * <summary> Gets all the var names of the current RAM state of the Lexer. </summary>
         * <returns> List of strings of the names in order of appearance in the RAM. </returns>
         */
        private List<string> GetVarNames()
        {
            // Return the list of names
            return _ram.Select(variable => variable.Name).ToList();
        }
        
        /**
         * <summary> Finds all the indexes of the occurences of the given name </summary>
         * <param name="names"> The list of names to check from. </param>
         * <param name="name"> The name to check for. </param>
         * <returns> List of indexes of the occurences. </returns>
         */
        private static List<int> FindOccurences(IReadOnlyList<string> names, string name)
        {
            // Initialize the final list & counters
            var res = new List<int>();
            int i = 0, namesLen = names.Count;
            
            // Collect all the indexes
            while (i < namesLen)
            {
                if (names[i] == name) res.Add(i);
                i++;
            }
            
            // Return the list of indexes
            return res;
        }

        /**
         * <summary> Removes all the duplicates variables from the current RAM state of the Lexer </summary>
         */
        private void RemoveDuplicates()
        {
            // Initialize the final list
            var names = GetVarNames();
            
            // Loop through all the variables names of the RAM
            foreach (var nameOccs in names.Select(name => FindOccurences(names, name)))
            {
                for (var i = 0; i < nameOccs.Count-1; i++) _ram.RemoveAt(nameOccs[i]);
                
                // Update the names to not re-iterate over the old names list
                names = GetVarNames();
            }
        }
    }
}