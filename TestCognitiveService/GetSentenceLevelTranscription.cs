using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TestCognitiveService.Models;

namespace TestCognitiveService
{
    public class GetSentenceLevelTranscription
    {
        private static List<string> _sentenceCollection;
        private static List<SegmentDetails> _segementDetails;

        public static List<SegmentDetails> GetSentenceTranscription(string wordLevelTranscript)
        {
            var serializedObject = JsonConvert.DeserializeObject<DetailedTranscription>(wordLevelTranscript);
            GetSentences(serializedObject);

            var words = serializedObject.NBest[0].Words;

            GetSegementDetails(words);

            return _segementDetails;
        }

        private static void GetSegementDetails(List<WordDetails> words)
        {
            int wordCount = 0;
            int offsetTicks = 0;
            var segmentSet = new List<SegmentDetails>();

            foreach (var sentence in _sentenceCollection)
            {
                var wordSetInSentence = sentence.Split(' ');
                offsetTicks = words[wordCount].Offset;
                double seconds = Math.Round(offsetTicks * 0.000000001, 2);
                var segment = new SegmentDetails()
                {
                    Text = sentence,
                    OffsetSeconds = seconds,
                };
                segmentSet.Add(segment);

                wordCount += wordSetInSentence.Length;
            }
            
            _segementDetails = segmentSet;
        }

        private static void GetSentences(DetailedTranscription serializeObject)
        {
            var displayText = serializeObject.DisplayText;

            var sentences = displayText.Split('.');

            var collection = new List<string>();

            foreach (var sentence in sentences)
            {
                if (sentence != "")
                {
                    collection.Add(sentence);
                }
            }

            _sentenceCollection = collection;
        }
    }
}
