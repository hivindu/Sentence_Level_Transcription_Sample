using System.Collections.Generic;

namespace TestCognitiveService.Models
{
    public class DetailedTranscription
    {
        public string Id { get; set; }

        public string RecognitionStatus { get; set; }

        public int Offset { get; set; }

        public int Duration { get; set; }

        public string DisplayText { get; set; }

        public List<NBests> NBest { get; set; }
    }
}
