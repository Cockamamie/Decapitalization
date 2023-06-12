using System.Collections.Generic;

namespace Decapitalization.ContextModels
{
    public class ContextModelsBuilder
    {
        private readonly Dictionary<string, ContextModel> contextModels = new();
        private readonly string text;

        public ContextModelsBuilder(string text)
        {
            this.text = text;
        }

        public IEnumerable<ContextModel> Build()
        {
            for (var i = 3; i < text.Length; i++)
            {
                var leftContext = new string(new[] { text[i - 3], text[i - 2], text[i - 1] });
                AddData(leftContext, text[i]);
            }

            return contextModels.Values;
        }

        private void AddData(string leftContext, char symbolWithContext)
        {
            if (!contextModels.ContainsKey(leftContext))
            {
                var contextModel = new ContextModel(leftContext);
                contextModels.Add(leftContext, contextModel);
            }

            contextModels[leftContext].AddData(symbolWithContext);
        }
    }
}