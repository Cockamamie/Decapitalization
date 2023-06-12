using System.Collections.Generic;
using System.Linq;

namespace Decapitalization.ContextModels
{
    public class ContextModel
    {
        public ContextModel(string leftContext)
        {
            LeftContext = leftContext;
        }

        public string LeftContext { get; }
        public int ContextOccurrencesCount { get; set; }
        public int SymbolsWithContextCount { get; set; }
        public Dictionary<char, int> SymbolOccurrencesWithContext { get; } = new();

        public void AddData(char symbolWithContext)
        {
            ContextOccurrencesCount++;

            if (!SymbolOccurrencesWithContext.ContainsKey(symbolWithContext))
            {
                SymbolsWithContextCount++;
                SymbolOccurrencesWithContext.Add(symbolWithContext, 0);
            }

            SymbolOccurrencesWithContext[symbolWithContext]++;
        }

        public override string ToString()
        {
            var a = SymbolOccurrencesWithContext.Keys
                .Select(k => $"{k} = {SymbolOccurrencesWithContext[k]}");
            var b = string.Join("; ", a);
            return $"C({LeftContext}) = [{b}]";
        }
    }
}