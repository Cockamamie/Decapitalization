using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Decapitalization.Encoder
{
    public class TextEncoder
    {
        private static readonly Regex Regex = new(@"[.?!][ \n]|(.|\n)“|[A-Z]{2}|\n\n|\r\n\r\n", RegexOptions.Compiled);

        public EncodingResult Decapitalize(StreamReader sr)
        {
            var firstSymbol = (char)sr.Read();
            var secondSymbol = (char)sr.Read();
            var sb = new StringBuilder($"{char.ToLower(firstSymbol)}{char.ToLower(secondSymbol)}");
            var deviationOffsets = new List<byte>();
            var lettersByDeviationPositions = new List<(int, char)>();
            var currentPosition = 0;
            var currentOffset = 0;

            if (char.IsUpper(firstSymbol))
            {
                var encodedOffset = EncodeOffset(currentOffset);
                deviationOffsets.AddRange(encodedOffset);
                lettersByDeviationPositions.Add((currentPosition, firstSymbol));
                currentOffset = 0;
            }
            currentOffset++;
            currentPosition++;

            if (char.IsUpper(secondSymbol))
            {
                var encodedOffset = EncodeOffset(currentOffset);
                deviationOffsets.AddRange(encodedOffset);
                lettersByDeviationPositions.Add((currentPosition, secondSymbol));
                currentOffset = 0;
            }
            currentOffset++;
            currentPosition++;

            while (sr.Peek() >= 0)
            {
                currentOffset++;
                var currentSymbol = (char)sr.Read();
                
                if (char.IsLetter(currentSymbol))
                {
                    var isUpper = char.IsUpper(currentSymbol);
                    var isUnderRule = IsUnderRule(firstSymbol, secondSymbol);
                    if (!isUpper && isUnderRule || isUpper && !isUnderRule)
                    {
                        var encodedOffset = EncodeOffset(currentOffset);
                        deviationOffsets.AddRange(encodedOffset);
                        lettersByDeviationPositions.Add((currentPosition, currentSymbol));
                        currentOffset = 0;
                    }
                }

                currentPosition++;
                sb.Append(char.ToLower(currentSymbol));
                firstSymbol = secondSymbol;
                secondSymbol = currentSymbol;
            }

            return new EncodingResult
            {
                Text = sb.ToString(),
                DeviationOffsets = deviationOffsets.ToArray(),
                LettersByDeviationPositions = lettersByDeviationPositions
            };
        }

        private static byte[] EncodeOffset(int offset)
        {
            const byte delimiter = 255;
            const int oneByteBorder = 254;
            const int twoBytesBorder = 65789;

            if (offset > twoBytesBorder)
                throw new ArgumentException();

            if (offset <= oneByteBorder)
                return new[] { (byte)offset };

            var bytesRepresentation = BitConverter.GetBytes(offset - delimiter);
            return new[] { delimiter, bytesRepresentation[1], bytesRepresentation[0]};
        }

        private static bool IsUnderRule(params char[] previousTwoSymbols)
        {
            if (previousTwoSymbols.Length != 2)
                throw new ArgumentException();
            
            var s = new string(previousTwoSymbols);
            return Regex.IsMatch(s);
        }
    }
}