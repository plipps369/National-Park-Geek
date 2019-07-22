using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Park
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
    }
}
