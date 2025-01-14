using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CurrencyCalculator.Model;
namespace CurrencyCalculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel myModel = null;
        private HttpManager httpManager = new HttpManager();
        private Timer timer;

        public MainViewModel()
        {
            myModel = new MainModel();
            // myModel.exchangeRate = 1400f.ToString();

            timer = new Timer(new TimerCallback(TimerWork), 0, 0, 500);
        }
        private void TimerWork(object? state)
        {
            // Test 코드
            //float temp = 0f;
            //if (float.TryParse(myModel.exchangeRate, out temp))
            //{
            //    //ExchangeRate = temp + 0.1f + "";
            //    ExchangeRate = $"{temp + 0.1f:F1}";
            //}
            ExchangeRate = GetExchangeRate();
        }

        private string GetExchangeRate()
        {
            var task = Task.Run(async () => await httpManager.GetData());
            return task.Result;
        }
        public string ExchangeRate
        {
            get
            {
                CalculateCurrency(myModel.exchangeRate, myModel.dollar);
                return myModel.exchangeRate;
            }
            set
            {
                if (myModel.exchangeRate != value)
                {
                    myModel.exchangeRate = value;
                    OnPropertyChanged(nameof(ExchangeRate));
                }
            }
        }
        public string Dollar
        {
            get
            {
                CalculateCurrency(myModel.exchangeRate, myModel.dollar);
                return myModel.dollar;
            }
            set
            {
                if (myModel.dollar != value)
                {
                    myModel.dollar = value;
                    OnPropertyChanged(nameof(Dollar));
                }
            }
        }
        public string Won
        {
            get { return myModel.won; }
            set
            {
                myModel.won = value;
                OnPropertyChanged(nameof(Won));
            }
        }
        private void CalculateCurrency(string exchangeRateStr, string dollarStr)
        {
            int dollar = 0;

            if (!string.IsNullOrEmpty(dollarStr) && !int.TryParse(myModel.dollar, out dollar))
            {
                MessageBox.Show("Please insert number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                myModel.dollar = string.Empty;
            }

            if (string.IsNullOrEmpty(exchangeRateStr) || string.IsNullOrEmpty(dollarStr) || exchangeRateStr == "0" || dollarStr == "0")
            {
                Won = "?";
                return;
            }
            else
            {

                if (int.TryParse(myModel.dollar, out dollar) && float.TryParse(exchangeRateStr, out float exchangeRate))
                {
                    float result = dollar * exchangeRate;
                    Won = result.ToString();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    } // end of class
} // end of namespace