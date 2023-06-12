using System.IO;
using System.Linq;
using Decapitalization.ContextModels;
using Decapitalization.Encoder;

namespace Decapitalization
{
    class Program
    {
        private const string InputTextPath = @"..\..\..\Test\input.txt";
        private const string OutputTextPath = @"..\..\..\Test\Output\decapitalized.txt";
        private const string OutputDeviationsTextPath = @"..\..\..\Test\Output\deviations.txt";
        private const string OutputDeviationsBinaryPath = @"..\..\..\Test\Output\deviations";
        private const string OutputContextModelsPath = @"..\..\..\Test\Output\contextModels.txt";
        
        public static void Main(string[] args)
        {
            var encoder = new TextEncoder();

            using var sr = new StreamReader(InputTextPath);
            var encodingResult = encoder.Decapitalize(sr);

            using var sw = new StreamWriter(OutputTextPath);
            sw.Write(encodingResult.Text);

            File.WriteAllBytes(OutputDeviationsBinaryPath, encodingResult.DeviationOffsets);
            
            var deviationsText = string.Join("\n", encodingResult.LettersByDeviationPositions);
            using var deviationsTextSw = new StreamWriter(OutputDeviationsTextPath);
            deviationsTextSw.Write(deviationsText);
            
            var contextModelsBuilder = new ContextModelsBuilder(encodingResult.Text);
            var contextModels = contextModelsBuilder.Build().ToList();
            var totalCharsInModels = contextModels.Sum(m => m.SymbolsWithContextCount);
            var neededMemory = 3 * (contextModels.Count + totalCharsInModels);
            using var contextModelsSw = new StreamWriter(OutputContextModelsPath);
            var contextModelsText = string.Join(
                "\n",
                contextModels.Select(m => RemoveLineEndings(m.ToString())));
            contextModelsSw.Write($"Total taken memory to store models = [{neededMemory}] bytes\n");
            contextModelsSw.Write(contextModelsText);
        }
        
        public static string RemoveLineEndings(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char) 0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            var newValue = "\\n";
            return value.Replace("\r\n", newValue)
                .Replace("\n", newValue)
                .Replace("\r", newValue)
                .Replace(lineSeparator, newValue)
                .Replace(paragraphSeparator, newValue);
        }
    }
}