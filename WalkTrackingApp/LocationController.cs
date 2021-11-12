using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WalkTrackingApp
{
    class LocationController
    {

        CancellationTokenSource cts;
        List<Location> allLocationPoints;
        double distanceTraveled;

        public LocationController()
        {
            // constructor
        }
        public double getAverageSpeed()
        {
            // TODO
            double sum = 0;
            int iter = 0;

            foreach (Location loca in allLocationPoints)
            {
                if (loca.Speed != null)
                {
                    sum += (double)loca.Speed;
                    iter++;
                }
            }
            if (iter == 0)
            {
                // will error if all points do not contain any speed data
                return -1;
            }
            return Math.Round((sum / iter), 2);
        }

        public int getTrailBehindIteration()
        {
            // should get rid of this
            throw new NotImplementedException();
        }

        public double getDistanceTraveled()
        {
            // returns distance traveled between two points, rounded to the ones
            return Math.Round(distanceTraveled, 2);
        }


        public List<Location> GetAllLocationPoints()
        {
            return allLocationPoints;
        }

        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                // cts should be called to cancel upon reset or pause request
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}" + " --- speed: " + location.Speed);
                    if (location.Speed != null)
                    {
                        location.Speed = Math.Round((double)location.Speed, 1); 
                        // updates speed to value provided by current geolocation request
                        // Note: Overrides inital return because it's unneeded precision
                    }
                    allLocationPoints.Add(location);
                    if (allLocationPoints.Count > 0)
                    {
                        // calculate distance if there are 2 or more points in the array
                        distanceTraveled += Math.Round(Location.CalculateDistance(
                            allLocationPoints[allLocationPoints.Count - 2], allLocationPoints[allLocationPoints.Count-1],
                            (DistanceUnits)1), 3);
                    }
                    return location;
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

        public Boolean attemptGeoCancel()
        {
            if (LocationHandler.cts != null && !(LocationHandler.cts).IsCancellationRequested) cts.Cancel();
            return cts.IsCancellationRequested;
            // does cts need to be reset?
        }

    }
}
