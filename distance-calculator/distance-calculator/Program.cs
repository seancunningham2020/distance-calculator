using System;
using distance_calculator.Services;

namespace distance_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var locationSearch = new LocationSearch();

            // Start Location
            Console.WriteLine("Start Location");
            var startSearchResult = locationSearch.SearchLocation();
            if (!startSearchResult.Status)
            {
                ExitWithError(startSearchResult.Message);
            }

            var startLocation = startSearchResult.Result;

            // End Location
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "End Location");
            var endSearchResult = locationSearch.SearchLocation();
            if (!endSearchResult.Status)
            {
                ExitWithError(endSearchResult.Message);
            }

            var endLocation = endSearchResult.Result;

            // Calculation
            var geoCalculations = new GeoCalculations();

            var startCoord = geoCalculations.FindCoordinates(startLocation.Latitude, startLocation.Longitude);
            var endCoord = geoCalculations.FindCoordinates(endLocation.Latitude, endLocation.Longitude);

            var distanceInMetres = geoCalculations.CalculateDistanceInMetres(startCoord, endCoord);

            
            OutputResults(startLocation, endLocation, distanceInMetres);


            // Midpoint
            var midPoint = geoCalculations.MidPoint(startCoord, endCoord);
            var midPointLocation = locationSearch.FindLocationFromGeoCoordinates(midPoint.Latitude, midPoint.Longitude);

            var outputService = new OutputService();
            outputService.DisplayLocation("Midpoint", midPointLocation.Result);


            // Points of interest
            var poi = locationSearch.FindPointsOfInterest(midPoint.Latitude, midPoint.Longitude);
            if (!poi.Status)
            {
                ExitWithError(poi.Message);
            }

            outputService.DisplayPOI(poi.Result);

            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }

        static void OutputResults(LocationDetails startLocation, LocationDetails endLocation, double distanceInMetres)
        {
            Console.Clear();

            var outputService = new OutputService();

            outputService.DisplayLocation("Start Location", startLocation);
            outputService.DisplayLocation("End Location", endLocation);

            Console.WriteLine();
            Console.WriteLine($"Distance: {distanceInMetres / 1000:0.00} km");
        }

        static void ExitWithError(string errorMessage)
        {
            Console.WriteLine("ERROR:: {0}", errorMessage);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
