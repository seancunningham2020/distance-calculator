using System;
using distance_calculator.Services;

namespace distance_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var locationSearch = new LocationSearch();
            var outputService = new OutputService();

            // Start Location
            Console.WriteLine("Start Location");
            var startSearchResult = locationSearch.SearchLocation();
            if (!startSearchResult.Status)
            {
                outputService.ExitWithError(startSearchResult.Message);
            }

            var startLocation = startSearchResult.Result;

            // End Location
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "End Location");
            var endSearchResult = locationSearch.SearchLocation();
            if (!endSearchResult.Status)
            {
                outputService.ExitWithError(endSearchResult.Message);
            }

            var endLocation = endSearchResult.Result;

            // Calculation
            var geoCalculations = new GeoCalculations();

            var startCoord = geoCalculations.FindCoordinates(startLocation.Latitude, startLocation.Longitude);
            var endCoord = geoCalculations.FindCoordinates(endLocation.Latitude, endLocation.Longitude);

            var distanceInMetres = geoCalculations.CalculateDistanceInMetres(startCoord, endCoord);


            outputService.ClearScreen();
            outputService.DisplayLocation("Start Location", startLocation);
            outputService.DisplayLocation("End Location", endLocation);
            outputService.DisplayDistance(distanceInMetres);


            // Midpoint
            var midPoint = geoCalculations.MidPoint(startCoord, endCoord);
            var midPointLocation = locationSearch.FindLocationFromGeoCoordinates(midPoint.Latitude, midPoint.Longitude);

            outputService.DisplayLocation("Midpoint", midPointLocation.Result);


            // Points of interest
            var poi = locationSearch.FindPointsOfInterest(midPoint.Latitude, midPoint.Longitude);
            if (!poi.Status)
            {
                outputService.ExitWithError(poi.Message);
            }

            outputService.DisplayPOI(poi.Result);

            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }
    }
}
