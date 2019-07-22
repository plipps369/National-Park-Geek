using Capstone.Web.DAO;
using Capstone.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
namespace IntegrationTests
{
    [TestClass]
    public class CapstoneTests
    {
        private TransactionScope _tran = null;
        private INPGeekDAO _db = null;

        [TestInitialize]
        public void Initialize()
        {
            _db = new NPGeekDAO("Data Source=localhost\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True");
            _tran = new TransactionScope();
            //creates an instance of a park for test purposes
            Park Pt = new Park()
            {
                Code = "AIK",
                ParkName = "LunaLand",
                State = "Hyped",
                acreage = 9001,
                elevationFt = 400,
                MilesOfTrail = 500,
                NumOfCampsites = 666,
                Climate = "Artic",
                YearFounded = 1969,
                AnnualVisitors = 2,
                InspirationQuote = "The rug really tied the room together",
                InspirationSource = "The Dude",
                ParkDescription = "A magical place where Lunicorns live",
                EntryFee = 350,
                NumOfAnimals = 1
            };
            //sends instance of park to database
            _db.CreatePark(Pt);
        }

        [TestCleanup]
        public void cleanup()
        {
            _tran.Dispose();
        }
        /// <summary>
        /// Tests methods in DAO involving Parks. Includes create park, retrieve all parks, and retrieve park by ID.
        /// </summary>
        [TestMethod()]
        public void TestParks()
        {
            var testResult = _db.GetParks();
            Assert.AreEqual(11, testResult.Count, "Count does not match");        
            var tr = _db.GetParkById("AIK");
            Assert.AreEqual("Hyped", tr[0].State);
            Assert.AreEqual(1969, tr[0].YearFounded);
            Assert.AreEqual(500, tr[0].MilesOfTrail);
            Assert.AreEqual(350, tr[0].EntryFee);
        }
        /// <summary>
        /// Tests methods in DAO involving weather function.  Includes create weather and retrieve weather by Park ID.
        /// </summary>
        [TestMethod()]
        public void TestWeather()
        {
            Weather wT = new Weather()
            {
                Code = "AIK",
                Low = 14,
                High = 130,
                Forecast = "Doom",
                DayValue = 6
            };
            _db.CreateWeather(wT);
            var tr = _db.GetWeatherById("AIK");
            Assert.AreEqual(14, tr[0].Low);
            Assert.AreEqual(6, tr[0].DayValue);
            Assert.AreEqual("Doom", tr[0].Forecast);
        }
        /// <summary>
        /// Tests methods in DAO involving survey function.  Includes creating surveys, and retreiving surveys by specific criteria.
        /// </summary>
        [TestMethod()]
        public void TestSurvey()
        {
            SurveyViewModel sT = new SurveyViewModel()
            {
                ParkCode = "AIK",
                Email = "Test@email.org",
                State = "DC",
                ActivityLevel = "Active"
            };
            bool success = _db.CreateSurvey(sT);
            Assert.AreEqual(true, success);
            var st = _db.GetSurveyByCode("AIK");
            Assert.AreEqual("DC", st[0].State);
            Assert.AreEqual("Test@email.org", st[0].Email);
            Assert.AreEqual("Active", st[0].ActivityLevel);

        }
        /// <summary>
        /// Tests the tempurature conversion method from Farenheit to Celsius
        /// </summary>
        [TestMethod]
        public void TempConvert()
        {
            Weather Tc = new Weather();
            Tc.Low = 32;
            Tc.High = 212;
            Tc.ConvertTempC();
            Assert.AreEqual(0, Tc.Low);
            Assert.AreEqual(100, Tc.High);
        }
    }
}
