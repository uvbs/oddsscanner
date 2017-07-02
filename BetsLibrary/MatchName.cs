using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public class MatchName
    {
        public string FirstTeam { get; private set; }
        public string SecondTeam { get; private set; }

        private string[] FirstTeamNormalized;
        private string[] SecondTeamNormalized;

        private List<string> FirstTeamLastWords = new List<string>();
        private List<string> SecondTeamLastWords = new List<string>();

        public MatchName(string FirstTeam, string SecondTeam)
        {
            this.FirstTeam = FirstTeam;
            this.SecondTeam = SecondTeam;
            this.FirstTeamNormalized = NormalizeName(FirstTeam);
            this.SecondTeamNormalized = NormalizeName(SecondTeam);
            if (FirstTeamNormalized.Length > 1) FirstTeamLastWords = GetWordVariants(FirstTeamNormalized[FirstTeamNormalized.Length - 1]);
            if (SecondTeamNormalized.Length > 1) SecondTeamLastWords = GetWordVariants(SecondTeamNormalized[SecondTeamNormalized.Length - 1]);
        }
        
        private string[] NormalizeName(string name)
        {
            string[] symbolsToRemove = { ".", ",", "-", "(", ")" };

            foreach (var symbol in symbolsToRemove)
                name = name.Replace(symbol, string.Empty);

            name = name.ToLower();
            name = name.Replace("fk ", string.Empty);
            name = name.Replace("fc ", string.Empty);

            return name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static List<string> GetWordVariants(string word)
        {
            var result = AddSymbols(word[0].ToString(), 1);
            result.Add(word[0].ToString());
            return result;

            List<string> AddSymbols(string start, int index)
            {
                List<string> res = new List<string>();
                if (index == word.Length) return res;
                string wordWithSymbol = start + word[index];
                res.Add(wordWithSymbol);
                res.AddRange(AddSymbols(start, index + 1));
                res.AddRange(AddSymbols(wordWithSymbol, index + 1));

                return res;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", FirstTeam, SecondTeam);
        }


        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is MatchName bet)
            {
                return Equals(bet);
            }
            else return false;
        }

        public bool Equals(MatchName name)
        {


            return EqualsTeamName(FirstTeamNormalized, FirstTeamLastWords, name.FirstTeamNormalized, name.FirstTeamLastWords) ||
                EqualsTeamName(SecondTeamNormalized, SecondTeamLastWords, name.SecondTeamNormalized, name.SecondTeamLastWords);
        }

        private bool EqualsTeamName(string[] firstTeamName, List<string> firstTeamLastWords, string[] secondTeamName, List<string> secondTeamLastWords)
        {
            if (firstTeamName.Length != secondTeamName.Length) return false;

            if (firstTeamName.Length > 1)
            {

                for (int i = 0; i < firstTeamName.Length - 1; i++)
                    if (firstTeamName.Length != secondTeamName.Length) return false;


                foreach (var word in firstTeamLastWords)
                    if (secondTeamLastWords.Contains(word)) return true;

                return false;
            }

            return firstTeamName[0] == secondTeamName[0];
            
        }
    }
}
