using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class ParkViewModel
    {
        public string Code { get; set; }
        public string ParkName { get; set; }
        public string State { get; set; }
        public int acreage { get; set; }
        public int elevationFt { get; set; }
        public double MilesOfTrail { get; set; }
        public int NumOfCampsites { get; set; }
        public string Climate { get; set; }
        public int YearFounded { get; set; }
        public int AnnualVisitors { get; set; }
        public string InspirationQuote { get; set; }
        public string InspirationSource { get; set; }
        public string ParkDescription { get; set; }
        public decimal EntryFee { get; set; }
        public int NumOfAnimals { get; set; }
        public string ParkImg { get; set; }
        public bool IsFarenheit { get; set; } = true;
        public int DayValue { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public string TempSymbol = "°F";
        public void ConvertTempC ()
        {
            TempSymbol = "°C";
        }

        public string Forecast { get; set; }
        public string WeatherImg { get; set; }

        public List<Weather> FiveDayForecast { get; set; } = new List<Weather>();
        public List<Park> Parks { get; set; } = new List<Park>();
    }
}
