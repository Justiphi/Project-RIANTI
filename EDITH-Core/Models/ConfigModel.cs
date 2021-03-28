using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDITH_Core.Models
{
    public class ConfigModel
    {
        public string WolframAlphaAPIKey { get; set; }
        public string AzureApiKey { get; set; }
        public string AzureRegion { get; set; }
        public string OpenWeatherAPIKey { get; set; }
        public string DbConnectionString { get; set; }
    }
}
