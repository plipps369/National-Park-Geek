using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAO
{
    public class NPGeekDAO : INPGeekDAO
    {
        private string _connectionString;

        public NPGeekDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Parks
        public IList<Park> GetParks()
        {
            List<Park> result = new List<Park>();
            const string sql = "SELECT * FROM park;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Park park = GetParksFromReader(reader);
                    result.Add(park);
                }
            }
            return result;
        }
        public IList<Park> GetParkById(string parkId)
        {
            List<Park> result = new List<Park>();
            const string sql = "SELECT * FROM park WHERE parkCode = @Id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("Id", parkId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetParksFromReader(reader));
                }
            }
            return result;
        }


        public bool CreatePark(Park newPark)
        {
            bool result = false;
            const string sql = "INSERT INTO park (parkCode, parkName, acreage, state, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, " +
                                                   "inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) "
                            + "VALUES (@parkCode, @name, @acreage, @state, @elevationInFeet, @milesOfTrail, @numberOfCampsites, @climate, @yearFounded, @annualVistorCount, @inspirationalQuote, " +
                                       "@inspirationalQuoteSource, @parkDescription, @entryFee, @numberOfAnimalSpecicies);";


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkCode", newPark.Code);
                cmd.Parameters.AddWithValue("@name", newPark.ParkName);
                cmd.Parameters.AddWithValue("@acreage", newPark.acreage);
                cmd.Parameters.AddWithValue("@state", newPark.State);
                cmd.Parameters.AddWithValue("@milesOfTrail", newPark.MilesOfTrail);
                cmd.Parameters.AddWithValue("@numberOfCampsites", newPark.NumOfCampsites);
                cmd.Parameters.AddWithValue("@climate", newPark.Climate);
                cmd.Parameters.AddWithValue("@yearFounded", newPark.YearFounded);
                cmd.Parameters.AddWithValue("@annualVistorCount", newPark.AnnualVisitors);
                cmd.Parameters.AddWithValue("@inspirationalQuote", newPark.InspirationQuote);
                cmd.Parameters.AddWithValue("@inspirationalQuoteSource", newPark.InspirationSource);
                cmd.Parameters.AddWithValue("@elevationInFeet", newPark.elevationFt);
                cmd.Parameters.AddWithValue("@entryFee", newPark.EntryFee);
                cmd.Parameters.AddWithValue("@parkDescription", newPark.ParkDescription);
                cmd.Parameters.AddWithValue("@numberOfAnimalSpecicies", newPark.NumOfAnimals);

                int rowsAffected = cmd.ExecuteNonQuery();

                result = rowsAffected == 1;
            }
            return result;
        }

        private Park GetParksFromReader(SqlDataReader reader)
        {
            Park item = new Park();
            {
                item.Code = Convert.ToString(reader["parkCode"]);
                item.ParkName = Convert.ToString(reader["parkName"]);
                item.State = Convert.ToString(reader["state"]);
                item.acreage = Convert.ToInt32(reader["acreage"]);
                item.elevationFt = Convert.ToInt32(reader["elevationInFeet"]);
                item.MilesOfTrail = Convert.ToDouble(reader["milesOfTrail"]);
                item.NumOfCampsites = Convert.ToInt16(reader["numberOfCampsites"]);
                item.Climate = Convert.ToString(reader["climate"]);
                item.YearFounded = Convert.ToInt16(reader["yearFounded"]);
                item.AnnualVisitors = Convert.ToInt32(reader["annualVisitorCount"]);
                item.InspirationQuote = Convert.ToString(reader["inspirationalQuote"]);
                item.InspirationSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                item.ParkDescription = Convert.ToString(reader["parkDescription"]);
                item.EntryFee = Convert.ToDecimal(reader["entryFee"]);
                item.NumOfAnimals = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
                return item;
            }
        }
        #endregion

        #region Weather
        public IList<Weather> GetWeatherById(string parkId)
        {
            List<Weather> result = new List<Weather>();
            const string sql = "SELECT * FROM Weather WHERE parkCode = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("Id", parkId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetWeatherFromReader(reader));
                }
            }
            return result;
        }

        public bool CreateWeather(Weather newWeather)
        {
            bool result = false;
            const string sql = "INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) "
                                + "VALUES (@parkCode, @day, @low, @high, @forecast);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkCode", newWeather.Code);
                cmd.Parameters.AddWithValue("@day", newWeather.DayValue);
                cmd.Parameters.AddWithValue("@low", newWeather.Low);
                cmd.Parameters.AddWithValue("@high", newWeather.High);
                cmd.Parameters.AddWithValue("@forecast", newWeather.Forecast);
                int rowsAffected = cmd.ExecuteNonQuery();
                result = rowsAffected == 1;
            }
            return result;
        }

        private Weather GetWeatherFromReader(SqlDataReader reader)
        {
            Weather item = new Weather();
            {
                item.Code = Convert.ToString(reader["parkCode"]);
                item.DayValue = Convert.ToInt32(reader["fiveDayForecastValue"]);
                item.Low = Convert.ToDouble(reader["low"]);
                item.High = Convert.ToDouble(reader["high"]);
                item.Forecast = Convert.ToString(reader["forecast"]);

                return item;
            }
        }
        #endregion

        #region Survey
        public bool CreateSurvey(SurveyViewModel newSurvey)
        {
            bool result = false;
            const string sql = "INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) "
                + "VALUES (@parkCode, @emailAddress, @state, @activityLevel);";


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkCode", newSurvey.ParkCode);
                cmd.Parameters.AddWithValue("@emailAddress", newSurvey.Email);
                cmd.Parameters.AddWithValue("@state", newSurvey.State);
                cmd.Parameters.AddWithValue("@activityLevel", newSurvey.ActivityLevel);

                int rowsAffected = cmd.ExecuteNonQuery();

                result = rowsAffected == 1;
            }
            return result;
        }

        public IList<Survey> GetSurveys()
        {
            List<Survey> result = new List<Survey>();
            const string sql = "SELECT survey_result.parkCode, COUNT(*) as 'parkCount', park.parkName, " +
                                      "park.inspirationalQuote, park.inspirationalQuoteSource " +
                               "FROM survey_result " +
                               "JOIN park on survey_result.parkCode = park.parkCode " +
                               "GROUP BY survey_result.parkCode, park.parkName, park.inspirationalQuote, " +
                                        "park.inspirationalQuoteSource " +
                               "ORDER BY 'parkCount' DESC;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetResultFromReader(reader));
                }
            }
            return result;
        }

        private Survey GetResultFromReader(SqlDataReader reader)
        {
            Survey item = new Survey();
            {            
                item.ParkCode = Convert.ToString(reader["parkCode"]);
                item.ParkCount = Convert.ToInt32(reader["parkCount"]);
                item.ParkName = Convert.ToString(reader["parkName"]);
                item.ParkQuote = Convert.ToString(reader["inspirationalQuote"]);
                item.QuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                return item;
            }
        }

        public IList<Survey> GetSurveyByCode(string code)
        {
            List<Survey> result = new List<Survey>();
            const string sql = "SELECT * FROM survey_result WHERE parkCode = @code;" ;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@code", code);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetSurveyFromReader(reader));
                }
            }
            return result;
        }
        public IList<Survey> GetSurveyByEmail(string email)
        {
            List<Survey> result = new List<Survey>();
            const string sql = "SELECT * FROM survey_result WHERE emailAddress = @email;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetSurveyFromReader(reader));
                }
            }
            return result;
        }

        private Survey GetSurveyFromReader(SqlDataReader reader)
        {
            Survey item = new Survey();
            {
                item.Id = Convert.ToInt32(reader["surveyId"]);
                item.ParkCode = Convert.ToString(reader["parkCode"]);
                item.Email = Convert.ToString(reader["emailAddress"]);
                item.State = Convert.ToString(reader["state"]);
                item.ActivityLevel = Convert.ToString(reader["activityLevel"]);
                return item;
            }
        }
    }
    #endregion
}

