using System.Collections.Generic;

namespace TestCognitiveService.Models
{
    public class SentenceLevelTranscription
    {
        public string FileName { get; set; }
        public List<SegmentDetails> Segments { get; set; }
    }
}
