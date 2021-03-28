using System;
using System.Collections.Generic;
using System.Text;
using EDITH_Core;

namespace RIANTI_UI.Modules
{
    public static class RiantiCore
    {

        public static void InitializeCoreAssistant()
        {
            Core.InitializeEdith();
        }

        public async static void StartAssistantListener()
        {
            await Core.ContinuousRecognitionWithKeywordSpottingAsync();
        }

        public async static void QueryText(string textToSpeak)
        {
            await Core.ProcessAndGetResponse(textToSpeak);
        }

        public async static void SpeakText(string textToSpeak)
        {
            await Speech.SynthesisToSpeakerAsync(textToSpeak);
        }
    }
}
