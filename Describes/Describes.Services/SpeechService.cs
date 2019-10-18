using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Threading.Tasks;

namespace Describes.Services
{
    public class SpeechService
    {
        public async Task<string> RecognizeSpeechFromFileAsync(string path)
        {
            var config = SpeechConfig.FromSubscription("3e3aea608df74736855bf7bf92596e43", "eastus2");
            config.SpeechRecognitionLanguage = "es-ES";
            var audioConfig = AudioConfig.FromWavFileInput(path); // "24375.wav");

            using (var recognizer = new SpeechRecognizer(config, audioConfig))
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    return $"Text recognized: {result.Text}";
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    return $"No speech recognized";
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellationDetails = CancellationDetails.FromResult(result);
                    
                    if (cancellationDetails.Reason == CancellationReason.Error)
                    {
                        return $"ErrorCode {cancellationDetails.ErrorCode}\n" +
                            $"ErrorDetails {cancellationDetails.ErrorDetails}";
                    }

                    return $"Speech recognition canceled: {cancellationDetails.Reason}";
                }

                return "Unknown error";
            }
        }

    }
}
