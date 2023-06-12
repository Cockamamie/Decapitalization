using System.Collections.Generic;

namespace Decapitalization.Encoder
{
    public class EncodingResult
    {
        public string Text { get; init; }
        public byte[] DeviationOffsets { get; init; }
        public List<(int, char)> LettersByDeviationPositions { get; init; }
    }
}