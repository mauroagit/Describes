using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Threading.Tasks;

namespace SpeechTest
{
    class Program
    {
        public static async Task RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription("3e3aea608df74736855bf7bf92596e43", "eastus2");
            config.SpeechRecognitionLanguage = "es-ES";
            var audioConfig = AudioConfig.FromWavFileInput("24375.wav");

            using (var recognizer = new SpeechRecognizer(config, audioConfig))
            {
                Console.WriteLine("Say...");
                var result = await recognizer.RecognizeOnceAsync();
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"Text recognized {result.Text}");

                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine("No recognized");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellationDetails = CancellationDetails.FromResult(result);
                    Console.WriteLine($"Speech recognition canceled: {cancellationDetails.Reason}");

                    if (cancellationDetails.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"ErrorCode {cancellationDetails.ErrorCode}");
                        Console.WriteLine($"ErrorDetails {cancellationDetails.ErrorDetails}");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            RecognizeSpeechAsync().Wait();
            Console.WriteLine("Press key");
            Console.ReadLine();
        }
    }
}
