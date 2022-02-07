using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPi.Libraries.Localization.Azure
{
    internal class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public TextResult SourceText { get; set; }
        public Translation[] Translations { get; set; }
    }

    internal class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }

    internal class TextResult
    {
        public string Text { get; set; }
        public string Script { get; set; }
    }

    internal class Translation
    {
        public string Text { get; set; }
        public TextResult Transliteration { get; set; }
        public string To { get; set; }
        public Alignment Alignment { get; set; }
        public SentenceLength SentLen { get; set; }
    }

    internal class Alignment
    {
        public string Proj { get; set; }
    }

    internal class SentenceLength
    {
        public int[] SrcSentLen { get; set; }
        public int[] TransSentLen { get; set; }
    }
}
