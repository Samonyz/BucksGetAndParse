using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;

namespace BucksGetAndParse
{
    public class CBRService : INotifyPropertyChanged
    {
        private decimal lastQueriedRate;
        private string val;
        private Label l;
        public CBRService(string value, Label L)
        {
            val = value;
            l = L;
        }

        public string GetVal
        {
            get
            {
                return val;
            }
        }

        private ru.cbr.www.DailyInfo _cbrClient;

        public ru.cbr.www.DailyInfo Cbr
        {
            get
            {
                if (_cbrClient == null)
                {
                    _cbrClient = new ru.cbr.www.DailyInfo();
                    _cbrClient.GetCursOnDateCompleted += new ru.cbr.www.GetCursOnDateCompletedEventHandler(_cbrClient_GetCursOnDateCompleted);
                }
                return _cbrClient;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                l.Text = LastQueriedRate.ToString();
            }
        }

        public decimal LastQueriedRate
        {
            get
            {
                return lastQueriedRate;
            }
            set
            {
                if (lastQueriedRate == value) return;
                lastQueriedRate = value;
                NotifyPropertyChanged("LastQueriedRate");
            }
        }



        void _cbrClient_GetCursOnDateCompleted(object sender, ru.cbr.www.GetCursOnDateCompletedEventArgs e)
        {
            LastQueriedRate = ExtractCurrencyRate(e.Result, (string)e.UserState);
        }

        public void AsyncGetCurrencyRateOnDate(DateTime dateTime, string currencyCode)
        {
            Cbr.GetCursOnDateAsync(dateTime, currencyCode);
        }

        private static decimal ExtractCurrencyRate(DataSet ds, string currencyCode)
        {
            if (ds == null)
                throw new ArgumentNullException("ds", "Параметр ds не может быть null.");
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException("currencyCode", "Параметр currencyCode не может быть null.");
            DataTable dt = ds.Tables["ValuteCursOnDate"];
            DataRow[] rows = dt.Select(string.Format("VchCode=\'{0}\'", currencyCode));
            if (rows.Length > 0)
            {
                decimal result;
                if (decimal.TryParse(rows[0]["Vcurs"].ToString(), out result))
                    return result;
                throw new InvalidCastException("От службы ожидалось значение курса валют.");
            }
            throw new KeyNotFoundException("Для заданной валюты не найден курс.");
        }

        //public decimal GetCurrencyRateOnDate()
        //{
        //    DataSet ds = Cbr.GetCursOnDate(DateTime.Now);
        //    return ExtractCurrencyRate(ds, GetVal);
        //}
    }
}
