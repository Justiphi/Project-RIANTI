using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace EDITH_Core
{
    public class Speech
    {
        public static async Task<Boolean> SynthesisToSpeakerAsync(string textToSpeak)
        {

            // Creates a speech synthesizer using the default speaker as audio output.
            using (var synthesizer = new SpeechSynthesizer(Config.GetAzureSpeechConfig()))
            {
                using (var result = await synthesizer.SpeakTextAsync(textToSpeak))
                {
                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized to speaker for text [{textToSpeak}]");
                        await Core.ContinuousRecognitionWithKeywordSpottingAsync();
                        return true;
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Unknown Error");
                        return false; 
                    }
                }
            }
        }
    }
}
