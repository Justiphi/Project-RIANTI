using EDITH_Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RIANTI_UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Modules.RiantiCore.InitializeCoreAssistant();
            //Modules.RiantiCore.StartAssistantListener();
        }

        public string Welcome => $"Welcome, The current keyword is: {Config.Keyword}!";

        private string Input = "";
        private string Weather = "Please initialize for temperature";
        private string InitButton = "Initialize";

        private static bool initialized = false;

        public string txtInput
        {
            get => Input;
            set => this.RaiseAndSetIfChanged(ref Input, value);
        }

        public string txtInitButton
        {
            get => InitButton;
            set => this.RaiseAndSetIfChanged(ref InitButton, value);
        }

        public string txtWeather
        {
            get => Weather;
            set => this.RaiseAndSetIfChanged(ref Weather, value);
        }


        public void ExitClick()
        {
            Environment.Exit(0);
        }

        public void QueryClick()
        {
            Modules.RiantiCore.QueryText(Input);
        }

        public void SayClick()
        {
            Modules.RiantiCore.SpeakText(Input);
        }

        public async void InitClick()
        {
            if (!initialized)
            {
                Config.configure();
                Modules.RiantiCore.StartAssistantListener();
                initialized = true;
                txtInitButton = "Refresh";
            }

            var weatherModule = new Modules.Weather(Config.config.OpenWeatherAPIKey);

            var weather = await weatherModule.QueryAsync("Tauranga");

            txtWeather = $"The Current Temperature in Tauranga is {weather.Temperature}°C\nThe weather is {weather.WeatherDesc}";
        }
    }
}
