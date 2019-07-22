using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Weather
    {
        public string Code { get; set; }
        public int DayValue { get; set; }
        public string Forecast { get; set; }
        public string WeatherImg { get; set; }
        public double Low { get; set; }
        public double High { get; set; }

        public string WeatherWarningForecast
        {
            get
            {
                string result = "";
                if (Forecast == "snow")
                {
                    result = "Pack snowshoes ";
                }
                if (Forecast == "rain")
                {
                    result = "Pack rain gear and wear waterproof shoes ";
                }
                if (Forecast == "thunderstorms")
                {
                    result = "Seek shelter and avoid hiking on exposed ridges ";
                }
                if (Forecast == "sunny")
                {
                    result = "Pack sunblock ";
                }
                return result;
            }
        }
        public string WeatherWarningTemp
        {
            get
            {
                string result = "";
                if (High > 75)
                {
                    result = "Bring an extra gallon of water";
                }
                if ((High - Low) > 20)
                {
                    result = "Wear Breathable Layers";
                }
                if (Low < 20)
                {
                    result = "Beware of frostbite and cold weather exposure";
                }
                return result;
            }
        }
        public void ConvertTempC()
        {
            Low = (int)((Low - 32) / 1.8);
            High = (int)((High - 32) / 1.8);
        }
    }
}
