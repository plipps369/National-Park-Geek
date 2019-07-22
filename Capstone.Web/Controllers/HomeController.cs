using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Web.Models;
using Security.DAO;
using Microsoft.AspNetCore.Http;
using Capstone.Web.DAO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capstone.Web.Controllers
{
    public class HomeController : AuthenticationController
    {
        private INPGeekDAO _db;

        public HomeController(IUserSecurityDAO userDb, INPGeekDAO db, IHttpContextAccessor httpContext) : base(userDb, httpContext)
        {
            _db = db;
        }

        /// <summary>
        /// Passes in a populated ParkViewModel to display all parks on the home page.  
        /// If user is not authenticated, user is redirected to login screen.
        /// </summary>
        /// <returns>Home view for page</returns>
        public IActionResult Index()
        {
            IActionResult result = null;
            if (GetUserManager().IsAuthenticated)
            {
                ParkViewModel vm = CreateViewModel();
                result = View("Index", vm);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }
            return result;
        }

        /// <summary>
        /// Displays a more detailed view of selected park.  Selection made by park ID using DAO method.
        /// Temperature symbols are converted based on session data.
        /// Session data is toggled in convert action.
        /// </summary>
        /// <param name="id">Park ID passed from Anchor tag within Index view</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Detail(string id)
        {
            IActionResult result = null;
            if (GetUserManager().IsAuthenticated)
            {
                ParkViewModel vm = PopulateParkDetails(id);
                result = View("Detail", vm);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }

            return result;
        }

        /// <summary>
        /// Displays results for user survey.  Creates an instance of survey with drop down menus populated.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Survey()
        {
            IActionResult result = null;
            if (GetUserManager().IsAuthenticated)
            {
                SurveyViewModel survey = PopulateSurveyResults();
                result = View(survey);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }

            return result;
        }

        /// <summary>
        /// Submits a new survey to database.  Survey email is set to the email of user from login.  
        /// User is redirected to same page where survey was filled out with updated results.
        /// </summary>
        /// <param name="newSurvey">Instance of new survey passed to action via form in Survey View</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Survey(SurveyViewModel newSurvey)
        {
            IActionResult result = null;
            if (GetUserManager().IsAuthenticated)
            {
                newSurvey.Email = GetUserManager().User.Email;
                _db.CreateSurvey(newSurvey);
                result = RedirectToAction("Survey");
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }

            return result;
        }

        /// <summary>
        /// Recieves request for tempurature conversion and sets session data based on tempurature requested.  
        /// Passes along Park ID to Detail action to display same park detail view with tempurature converted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns
        [HttpGet]
        public IActionResult Convert (string id)
        {
            IActionResult result = null;
            if (GetUserManager().IsAuthenticated)
            {   
                if(GetSessionData<bool>("IsFarenhiet"))
                {
                    SetSessionData("IsFarenhiet", false);
                }
                else
                {
                    SetSessionData("IsFarenhiet", true);
                }
                result = RedirectToAction("Detail", "Home", new {id=id});
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }

            return result;
        }

        //*******************************Extracted Methods*****************************************************

        /// <summary>
        /// Creates a new instance of survey view model with drop down menus populated,
        /// Previous results added to list to display in Survey View.
        /// Email of current user is checked to set bool.  Bool determines if User has already submitted form for new survey.
        /// </summary>
        /// <returns></returns>
        private SurveyViewModel PopulateSurveyResults()
        {
            SurveyViewModel survey = new SurveyViewModel();
            //sets email in ViewModel to email user used when registering login.
            survey.Email = GetUserManager().User.Email;
            var surveys = _db.GetSurveys();
            foreach (Survey surv in surveys)
            {
                survey.Surveys.Add(surv);
                survey.ParkCode = surv.ParkCode;
            }
            //populates park list from data base for use in drop down menu
            populateParksList(survey);
            //bool is set to true if a survey with user email exists in database
            survey.SurveySubmitted = _db.GetSurveyByEmail(survey.Email).Count > 0;
            return survey;
        }

        /// <summary>
        /// Populates ParkViewModel park details and weather forecast with one specific park obtained by Park ID.
        /// Uses two DAO methods.  One to obtain specific park details, the other to obtain five day weather forecast of that park.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ParkViewModel PopulateParkDetails(string id)
        {
            var park = _db.GetParkById(id);
            var forecast = _db.GetWeatherById(id);
            ParkViewModel vm = new ParkViewModel();
            populatePark(vm, park[0]);
            SetDegreeSymbol(forecast, vm);

            return vm;
        }
        /// <summary>
        /// Sets Temp Symbol to °F or °C based on session data.
        /// Sets bool IsFarenheit to determine button display within Detail View.
        /// </summary>
        /// <param name="forecast"></param>
        /// <param name="vm"></param>
        private void SetDegreeSymbol(IList<Weather> forecast, ParkViewModel vm)
        {
            var degreeType = GetSessionData<bool>("IsFarenhiet");
            if (degreeType)
            {
                vm.IsFarenheit = true;
                populateWeather(forecast, vm, degreeType);
            }
            else
            {
                vm.IsFarenheit = false;
                vm.ConvertTempC();
                populateWeather(forecast, vm, degreeType);
            }
        }

        /// <summary>
        /// Populates list of parks for SurveyViewModel park selection dropdown.
        /// </summary>
        /// <param name="survey"></param>
        private void populateParksList(SurveyViewModel survey)
        {
            var parkList = _db.GetParks();
            foreach (Park park in parkList)
            {
                survey.Parks.Add(new SelectListItem() { Text = park.ParkName, Value = park.Code });
            }
        }
        /// <summary>
        /// Populates Weather Model List inside of ParkViewModel from List produced by DAO method.  
        /// If degree is true, degrees will be displayed in Fahrenheit, Celsius if false.
        /// </summary>
        /// <param name="forecast"></param>
        /// <param name="vm"></param>
        /// <param name="degree"></param>
        private static void populateWeather(IList<Weather> forecast, ParkViewModel vm, bool degree)
        {
            foreach (Weather weather in forecast)
            {
                if (degree)
                {
                    vm.DayValue = weather.DayValue;
                    vm.Low = weather.Low;
                    vm.High = weather.High;
                    vm.WeatherImg = weather.Forecast.ToLower() + ".png";
                    weather.WeatherImg = vm.WeatherImg;
                    vm.FiveDayForecast.Add(weather);
                }
                else
                {
                    weather.ConvertTempC();
                    vm.DayValue = weather.DayValue;
                    vm.Low = weather.Low;
                    vm.High = weather.High;
                    vm.WeatherImg = weather.Forecast.ToLower() + ".png";
                    weather.WeatherImg = vm.WeatherImg;
                    vm.FiveDayForecast.Add(weather);
                }
            }
        }
        /// <summary>
        /// populates ParkViewModel from Park model using DAO GetParks method
        /// </summary>
        /// <returns></returns>
        private ParkViewModel CreateViewModel()
        {
            ParkViewModel vm = new ParkViewModel();
            var listOfParks = _db.GetParks();
            
            foreach (Park park in listOfParks)
            {
                populatePark(vm, park);
            }
            return vm;
        }
        /// <summary>
        /// fully populates ParkViewModel from Park model
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="park"></param>
        private static void populatePark(ParkViewModel vm, Park park)
        {
            vm.Code = park.Code;
            vm.ParkName = park.ParkName;
            vm.State = park.State;
            vm.acreage = park.acreage;
            vm.elevationFt = park.elevationFt;
            vm.MilesOfTrail = park.MilesOfTrail;
            vm.NumOfCampsites = park.NumOfCampsites;
            vm.Climate = park.Climate;
            vm.YearFounded = park.YearFounded;
            vm.AnnualVisitors = park.AnnualVisitors;
            vm.InspirationQuote = park.InspirationQuote;
            vm.InspirationSource = park.InspirationSource;
            vm.ParkDescription = park.ParkDescription;
            vm.EntryFee = park.EntryFee;
            vm.NumOfAnimals = park.NumOfAnimals;
            vm.ParkImg = park.Code.ToLower() + ".jpg";
            vm.Parks.Add(park);
            park.ParkImg = vm.ParkImg;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
