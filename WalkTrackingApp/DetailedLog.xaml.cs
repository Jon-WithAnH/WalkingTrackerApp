using WalkTrackingApp.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace WalkTrackingApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedLog : ContentPage
    {
        runData obja;
        List<Location> locationPoints;
        public DetailedLog(runData obj)
        {
            InitializeComponent();
            obja = obj;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            dayTime.Text = obja.dayTime;
            time.Text = obja.time;
            mph.Text = obja.mph.ToString() + "mph";
            distance.Text = obja.distance.ToString();

            locationPoints = JsonConvert.DeserializeObject<List<Location>>(obja.allLocationPoints);

        }

        private void generateLines()
        {
            // needs to grab location at i-1, making foreach loop slightly more complicated
            for (int i = 0; i < locationPoints.Count; i++ )
            {
                // skip first or the last?
                if (i == 0)
                {
                    //since google maps needs two of type Location to create a line
                    // we'll skip the first and revist it on second iteration
                    // do nothing here
                }
                else
                {
                    createLine((Location)locationPoints[i-1], (Location)locationPoints[i]);
                }
            }
        }

        void createLine(Location loca, Location loca2)
        {
            double lat = loca.Latitude;
            double longa = loca.Longitude;
            double lat2 = loca2.Latitude;
            double longa2 = loca2.Longitude;

            Polyline newLine = new Polyline();
            newLine.Positions.Add(new Position(lat, longa));
            newLine.Positions.Add(new Position(lat2, longa2));
            newLine.StrokeColor = Color.Blue;
            newLine.StrokeWidth = 5f;
            map.Polylines.Add(newLine);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (locationPoints.Count > 0)
            {
                Position user = new Position(locationPoints[0].Latitude, locationPoints[0].Longitude);
                await map.MoveCamera(CameraUpdateFactory.NewPositionZoom(user, 15.3));
                generateLines();
            }
            else
            {
                //no data to reference
                await DisplayAlert("No Data", "No starting point data found. Likely saved before a geolocation request finished.", "Okay");
            }
        }

    }
}