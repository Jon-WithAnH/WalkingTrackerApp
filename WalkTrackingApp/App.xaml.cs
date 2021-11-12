using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WalkTrackingApp.Data;

namespace WalkTrackingApp
{
    public partial class App : Application
    {

        static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "runRecords.db3"));
                }
                return database;
            }
        }
        public App()
        {

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
