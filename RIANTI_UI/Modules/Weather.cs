using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RIANTI_UI.Modules
{
    public class WeatherObject
    {
        public double Temperature { get; set; }
        public string WeatherDesc { get; set; }
    }

    class Weather
    {
        private string openWeatherAPIKey;

        public Weather(string apiKey)
        {
            openWeatherAPIKey = apiKey;
        }

        public async Task<WeatherObject> QueryAsync(string queryStr)
        {
            Uri uri = new Uri(string.Format("http://api.openweathermap.org/data/2.5/weather?appid={0}&q={1}", openWeatherAPIKey, queryStr).ToString());
            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(uri);
            JObject jsonData = JObject.Parse(data);
            if (jsonData.SelectToken("cod").ToString() == "200")
            {
                var mainData = jsonData.SelectToken("main");
                var weatherData = jsonData.SelectToken("weather(0)");

                var currentTemperature = convertToCelsius(double.Parse(mainData.SelectToken("temp").ToString()));
                var currentWeather = weatherData.SelectToken("description").ToString();

                var returnObject = new WeatherObject();
                returnObject.Temperature = currentTemperature;
                returnObject.WeatherDesc = currentWeather;

                return returnObject;
            }
            else
            {
                return new WeatherObject() { Temperature = 0, WeatherDesc = "Unable to Retrieve" };
            }

        }
        private double convertToCelsius(double kelvin)
        {
            return Math.Round(kelvin - 273.15, 3);
        }
    }
}
