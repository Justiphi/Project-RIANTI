using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace EDITH_Core
{
    public class Core
    {
        private static bool IsInitialized = false;
        public static void InitializeEdith()
        {

            // point to the location of the keyword recognition model.                   SWAP COMMENTED LINE TO CHANGE Keyword

            //var keyword = "Edith";
            //var keyword = "Tiina";
            //var keyword = "Jarvis";
            //var keyword = "Peter";
            //var keyword = "Friday";
            //var keyword = "Ria";
            var keyword = "Rianti";

            // The phrase the keyword recognition model triggers on.
            Config.Keyword = keyword;

            IsInitialized = true;
        }

        // Continuous speech recognition with keyword spotting.
        public static async Task ContinuousRecognitionWithKeywordSpottingAsync()
        {
            if (!IsInitialized)
            {
                return;
            }


            // Creates an instance of a keyword recognition model. Update this to

            var model = KeywordRecognitionModel.FromFile($"Keywords/{Config.Keyword}.table");

            var stopRecognition = new TaskCompletionSource<int>();

            // Creates a speech recognizer using microphone as audio input.
            using (var recognizer = new SpeechRecognizer(Config.GetAzureSpeechConfig()))
            {
                // Subscribes to events.
                recognizer.Recognizing += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizingKeyword)
                    {
                        Console.WriteLine($"RECOGNIZING KEYWORD: Text={e.Result.Text}");
                    }
                    else if (e.Result.Reason == ResultReason.RecognizingSpeech)
                    {
                        Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                    }
                };

                recognizer.Recognized += async (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedKeyword)
                    {
                        Console.WriteLine($"RECOGNIZED KEYWORD: Text={e.Result.Text}");
                    }
                    else if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                        await ProcessAndGetResponse(e.Result.Text);
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine("NOMATCH: Speech could not be recognized.");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"CANCELED: Reason={e.Reason}");

                    if (e.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                    stopRecognition.TrySetResult(0);
                };

                recognizer.SessionStarted += (s, e) =>
                {
                    Console.WriteLine("\n    Session started event.");
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("\n    Session stopped event.");
                    Console.WriteLine("\nStop recognition.");
                    stopRecognition.TrySetResult(0);
                };

                // Starts recognizing.
                Console.WriteLine($"Say something starting with the keyword '{Config.Keyword}' followed by whatever you want...");

                // Starts continuous recognition using the keyword model. Use
                // StopKeywordRecognitionAsync() to stop recognition.
                await recognizer.StartKeywordRecognitionAsync(model).ConfigureAwait(false);

                // Waits for a single successful keyword-triggered speech recognition (or error).
                // Use Task.WaitAny to keep the task rooted.
                Task.WaitAny(new[] { stopRecognition.Task });

                // Stops recognition.
                await recognizer.StopKeywordRecognitionAsync().ConfigureAwait(false);
            }
        }

        public static async Task ProcessAndGetResponse(string InputString)
        {
            if (!IsInitialized)
            {
                return;
            }

            string response;
            string query;

            if (InputString.StartsWith(Config.Keyword))
            {
                InputString = InputString.Remove(0, Config.Keyword.Length);
                if (InputString.StartsWith(','))
                {
                    InputString = InputString.Remove(0, 1);
                }
                InputString = InputString.Trim();
            }

            response = CommandHandler.RunCommand(InputString);
            if (response != "")
            {
                if (response == "N/A")
                {
                    await Core.ContinuousRecognitionWithKeywordSpottingAsync();
                    return;
                }

                await Speech.SynthesisToSpeakerAsync(response);
                return;
            }

            query = WebUtility.UrlEncode(InputString);

            var client = new HttpClient();

            var url = Config.BaseURL;
            if(!string.IsNullOrEmpty(Config.CurrentHost))
            {
                url = $"http://{Config.CurrentHost}/api";
            }
            url += Config.WrConversation;

            client.BaseAddress = new Uri(url);
            //HTTP GET
            try
            {
                var responseTask = client.GetAsync(Config.GetWolframURLArgs(query, 0));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    //var readTask = result.Content.ReadAsAsync<WolfRamConversation[]>();
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var obj = JsonSerializer.Deserialize<WolfRamConversation>(readTask.Result);

                    response = obj.result;
                    Config.ConversationKey = obj.conversationID;
                    if (!string.IsNullOrEmpty(obj.s))
                    {
                        Config.LastS = obj.s;
                    }
                    if (!string.IsNullOrEmpty(obj.host))
                    {
                        Config.CurrentHost = obj.host;
                    }
                }
                else
                {
                    response = "An error occured, please try again later.";
                }
            }
            catch
            {
                response = "An A P I failure occured, please try again later.";
            }
            if(response == null)
            {
                await Speech.SynthesisToSpeakerAsync("I'm sorry, I did not understand, Please try again.");
            }
            else
            {
                await Speech.SynthesisToSpeakerAsync(response);
            }
        }

    }
}
