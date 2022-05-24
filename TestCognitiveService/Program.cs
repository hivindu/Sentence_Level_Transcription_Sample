using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TestCognitiveService.Constants;
using TestCognitiveService.Models;

namespace TestCognitiveService
{
    class Program
    {
        private static SpeechRecognitionResult _recognitionResult;
        private static SentenceLevelTranscription _sentenceLevelTranscription;
 
        async static Task FromFileAsync(SpeechConfig speechConfig)
        {
            speechConfig.OutputFormat = OutputFormat.Detailed;
            speechConfig.RequestWordLevelTimestamps();
            speechConfig.EnableDictation();
            var fileName = "anxiety_patient_demo.wav";
            using (var audioConfig = AudioConfig.FromWavFileInput(CognitiveServiceConstants.BaseFilePath+ fileName))
            using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
            {
                var stopRecognition = new TaskCompletionSource<int>();

                recognizer.Recognized += (s, e) =>
                {
                    _recognitionResult = e.Result;
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        var jsn = _recognitionResult.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);
                        var transcript = GetSentenceLevelTranscription.GetSentenceTranscription(jsn);
                        var transcription = new SentenceLevelTranscription()
                        {
                            FileName = fileName,
                            Segments = transcript,
                        };
                        _sentenceLevelTranscription = transcription;
                        var jsonResponse = JsonConvert.SerializeObject(_sentenceLevelTranscription);
                        Console.WriteLine(jsonResponse.ToString());
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"CANCELED: Reason={e.Reason}");

                    if (e.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the speech key and location/region info?");
                    }

                    stopRecognition.TrySetResult(0);
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("\n    Session stopped event.");
                    stopRecognition.TrySetResult(0);
                };

                await recognizer.StartContinuousRecognitionAsync();

                Task.WaitAny(new[] { stopRecognition.Task });

                await recognizer.StopContinuousRecognitionAsync();

            }
        }

        async static Task Main(string[] args)
       {
            var speechConfig = SpeechConfig.FromSubscription(CognitiveServiceConstants.SubscriptionKey, CognitiveServiceConstants.Region);
            await FromFileAsync(speechConfig);
            Console.ReadKey();
        }
    }
}
