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

        private List<string> FirstTeamNormalized;
        private List<string> SecondTeamNormalized;

        public MatchName(string FirstTeam, string SecondTeam)
        {
            this.FirstTeam = FirstTeam;
            this.SecondTeam = SecondTeam;
            this.FirstTeamNormalized = NormalizeName(FirstTeam);
            this.SecondTeamNormalized = NormalizeName(SecondTeam);
        }
        
        private List<string> NormalizeName(string name)
        {
            name = name.ToLower();
            name = name.Replace("fk ", string.Empty);
            name = name.Replace("fc ", string.Empty);

            List<string> result = new List<string>();
            result.Add(name);

            string[] symbolsToRemove = { ".", ",", "-", "(", ")", "'", "/" };

            foreach (var symbol in symbolsToRemove)
                result = RemoveSymbol(result, symbol);

            return result;
        }

        private bool IsSameTeamName(List<string> teamAList, List<string> teamBList)
        {
            foreach(var teamA in teamAList)
                foreach(var teamB in teamBList)
                {
                    string[] teamAWords = teamA.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    string[] teamBWords = teamB.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (teamAWords.Length != teamBWords.Length) continue;

                    if(!IsSameTeam(teamAWords, teamBWords)) continue;

                    return true;
                }

            return false;

            bool IsSameTeam(string[] teamAWords, string[] teamBWords)
            {
                if (teamAWords.Length == 0) return true;

                var bWords = teamBWords.Where(word => IsSameWord(teamAWords[0], word));

                foreach(var word in bWords)
                {
                    List<string> bList = teamBWords.ToList();
                    bList.Remove(word);
                    List<string> aList = teamAWords.ToList();
                    aList.RemoveAt(0);

                    if (IsSameTeam(aList.ToArray(), bList.ToArray())) return true;
                }

                return false;
            }
        }

        private List<string> RemoveSymbol(List<string> names, string symbol)
        {
            List<string> result = new List<string>();

            foreach(var name in names)
            {
                if(!name.Contains(symbol)) { result.Add(name); continue; }
                result.Add(name.Replace(symbol, string.Empty));
                result.Add(name.Replace(symbol, " "));
            }

            return result;
        }

        private bool IsSameWord(string wordA, string wordB)
        {
            wordA = wordA.Trim();
            wordB = wordB.Trim();
            if(wordA.Length < wordB.Length)
            {
                string tmp;
                tmp = wordA;
                wordA = wordB;
                wordB = tmp;
            }

            if (wordA.Length == wordB.Length) return wordA == wordB;

            if (wordA.Length < 1 || wordB.Length < 1) return false;
            if (wordA[0] != wordB[0]) return false;

            wordA = wordA.Remove(0, 1);
            wordB = wordB.Remove(0, 1);

            for(int b = 0; b < wordB.Length; b++)
            {
                int index = wordA.IndexOf(wordB[b]);
                if (index == -1) return false;
                wordA.Remove(0, index + 1);
            }

            return true;

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
            return IsSameTeamName(FirstTeamNormalized, name.FirstTeamNormalized) && IsSameTeamName(SecondTeamNormalized, name.SecondTeamNormalized);
        }
        
    }
}
