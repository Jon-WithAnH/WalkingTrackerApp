using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WalkTrackingApp
{
    public class LocationHandler
    {
        public static CancellationTokenSource cts;
        public Location[] prevLocations = new Location[5];
        ArrayList averageSpeedCalc = new ArrayList();
        List<Location> allLocationPoints = new List<Location>();
        public double distanceTraveled;
        public double? speed;

        public LocationHandler()
        {
            // Constructor
            int listIteration = 0;
           // GetCurrentLocation();
            // verify validity

        }
        private int listIteration = 0;
        private int trailBehindIteration = 0;
        
        public double getAverageSpeed()
        {
            double sum = 0;
            int iter = 0;
            
            foreach ( double i in averageSpeedCalc)
            {
                sum += i;
                iter++;
            }

            return Math.Round((sum / iter), 2);
        }

        public double getDistanceTraveled()
        {
            return Math.Round(distanceTraveled, 2);
        }

        public int getTrailBehindIteration()
        {
            return trailBehindIteration;
        }
        public List<Location> GetAllLocationPoints()
        {
            return allLocationPoints;
        }

        public void Clear()
        {
        Location[] prevLocations = new Location[5];
        double? speed;
        listIteration = 0;
        trailBehindIteration = 0;
        distanceTraveled = 0;
        averageSpeedCalc = new ArrayList();
        allLocationPoints.Clear();
        }

        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                   // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}" + " --- speed: " + location.Speed);
                   if (location.Speed != null)
                    {
                        speed = Math.Round((double)location.Speed, 1);
                        averageSpeedCalc.Add(speed);
                    }
                    AddToList(location);
                    allLocationPoints.Add(location);
                    return location;

                }
                else
                {
                    //shouldn't happen
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // not supported on device 
                return null;
               
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // not enabled on device 
                return null;
            }
            catch (PermissionException pEx)
            {
                // permission 
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Failed to get location or unexpected error: " + ex);
                return null;

            }
            return null;
        }


        void AddToList(Location location)
        {
            if (prevLocations[0] == null)
            {
                prevLocations[listIteration] = location;
            }
            else
            {
                prevLocations[listIteration] = location;
                distanceTraveled += Math.Round(Location.CalculateDistance(prevLocations[trailBehindIteration], prevLocations[listIteration], (DistanceUnits)1), 3);
                //Console.WriteLine("Distance traveled since last udpate: " + Math.Round(Location.CalculateDistance(prevLocations[trailBehindIteration], prevLocations[listIteration], (DistanceUnits)1), 3) + "---- Moved in total: " + distanceTraveled);
                if (trailBehindIteration == 4){ trailBehindIteration = 0;}
                else { trailBehindIteration++; }
            }
            
            
            // reset lists to replace older values first
            if (listIteration == 4) { listIteration = 0; }
            else { listIteration++; }
        }

    }
}
