using WalkTrackingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace WalkTrackingApp
{
    public partial class MainPage : ContentPage
    {

        List<runData> row;
        //public ListView listViewMainpage { get; set; }
        public MainPage()
        {
            InitializeComponent();
            
        }

        public async void noOfRows()
        {
            row = await App.Database.getRows();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            noOfRows();
            
        }

        private async void rows_Clicked(object sender, EventArgs e)
        {
            
            if (row.Count() == 0)
            {
                // no data populated. default values by XAML will be used. terminate
                await DisplayAlert("DataInfo", "No data in database", "Okay");
                return;
            }

            double averageSpeed = 0;
            double averageDistance = 0;
            // time will be cleansed into seconds, averaged, and then put back into it's inital format of 00:00:00
            // this list holds the time (in seconds) given by each data point
            List<int> secondsHolder = new List<int>();

            int number_of_rows = row.Count();
            int i = 10;
            if (i > number_of_rows)
            {
                // less data than ideally displayed. change interable to reflect how much data to pull
                i = number_of_rows;
            }

            // get most recent runs. they will have the larger indexs in List<runData>
            // hench use of **int j**
            for (int j = number_of_rows -1; 0 < i; i--)
            {
                runData processData = row[j];
                // calc average x of run [j]
                averageSpeed = processData.mph;
                averageDistance = processData.distance;

                // averageTime is stored as a string in the database
                // needs to be cleaned before processed. \d\d:\d\d:\d\d
                secondsHolder.Add(toSeconds(processData.time));
                j--;
            }
            averageSpeedXAML.Text = Math.Round((averageSpeed / number_of_rows), 2).ToString() + "mph";
            averageDistanceXAML.Text = Math.Round((averageDistance / number_of_rows), 2).ToString();
            int tmp = 0; // just a temp value which will divide total seconds by size of secondsHolder
            foreach (int seconds in secondsHolder)
            {
                tmp += seconds;
            }
            tmp /= secondsHolder.Count(); // still needs to be converted into the proper format
            averageTimeXAML.Text = toHhMmSs(tmp);

        }

        private int toSeconds(string time)
        {
            string[] timeSplit = time.Split(':');

            int totalSeconds = Int32.Parse(timeSplit[0]) * 60 * 60;
            totalSeconds += Int32.Parse(timeSplit[1]) * 60;
            totalSeconds += Int32.Parse(timeSplit[2]);
            return totalSeconds;
        }

        private string toHhMmSs(int time)
        {
            int hour = time / 3600;
            time -= hour * 3600;
            int minute = time / 60;
            time -= minute * 60;
            // what remains of time is just Seconds
            // possible format: 0h0m51s
                    // fixed by putZeroInFront(int)
            return (putZeroInFront(hour) + "h" + putZeroInFront(minute) + "m" + putZeroInFront(time) + "s");
        }

        public string putZeroInFront(int time)
            //just turns a 1 digit number into two digits
            //eg. 0 <= x <= 9, x --> 0x
        {
            if (time < 10)
            {
                return "0" + time.ToString();
            }
            else
            {
                return time.ToString();
            }
        }


            private async void OnBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WalkTrackingApp.TimerTracking());
        }
            private async void viewLogs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WalkTrackingApp.viewLogs());
        }
    }
}
