using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAO
{
    public interface INPGeekDAO
    {
        IList<Park> GetParks();

        IList<Park> GetParkById(string parkId);

        IList<Weather> GetWeatherById(string parkId);

        bool CreateSurvey(SurveyViewModel newSurvey);

        IList<Survey> GetSurveys();

        IList<Survey> GetSurveyByCode(string code);

        IList<Survey> GetSurveyByEmail(string email);

        bool CreatePark(Park newPark);
        bool CreateWeather(Weather newWeather);
    }
}
