using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Entities;
using System.Data;
namespace BusinessLogic
{
    public class CurrencyManager
    {
        string _ConnString;
        public CurrencyManager(string ConnString)
        {
            this._ConnString = ConnString;
        }

        public List<Currency> GetCurrencies()
        {
            List<Currency> Currencies = new List<Currency>();

            try
            {

                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("currency_GetAllCurrency", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Currency currency = new Currency();

                            currency.Code = dr["Name"].ToString();
                            currency.Name = dr["Description"].ToString();
                            currency.Symbol = dr["Symbol"].ToString();

                            Currencies.Add(currency);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Currencies;
        }

        public List<CurrencyRate> GetCurrencyRates(DateTime FromDate, DateTime ToDate, string Source)
        {
            List<CurrencyRate> currenciesRates = new List<CurrencyRate>();

            try
            {

                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("GetApiAllCurrencyConversions", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_FromDate", FromDate);
                    command.Parameters.AddWithValue("_ToDate", ToDate);
                    command.Parameters.AddWithValue("_Currency", Source);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CurrencyRate currencyRate = new CurrencyRate();

                            currencyRate.ConversionDate = Convert.ToString(Convert.ToDateTime(dr["CurrencyConversionDate"]).ToString("yyyy-MM-dd"));
                            currencyRate.ConversionRate = dr["CurrencyConversionRate"].ToString();
                            currencyRate.CurrencyFrom = dr["CurrencyFrom"].ToString();
                            currencyRate.CurrencyTo = dr["CurrencyTo"].ToString();
                            //if (!string.IsNullOrEmpty(dr["IsCorrect"].ToString()))
                              //  currencyRate.IsCorrect = Convert.ToBoolean(dr["IsCorrect"]);
                            currenciesRates.Add(currencyRate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return currenciesRates;
        }
    }
}
