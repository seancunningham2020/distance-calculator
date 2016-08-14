using System;
using distance_calculator.BusinessLogic;
using distance_calculator.Helpers;

namespace distance_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var locationSearch = new LocationSearch();
            var outputHelper = new OutputHelper();

            // Start Location
            Console.WriteLine("Start Location");
            var startSearchResult = locationSearch.SearchLocation();
            if (!startSearchResult.Status)
            {
                outputHelper.ExitWithError(startSearchResult.Message);
            }

            var startLocation = startSearchResult.Result;

            // End Location
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "End Location");
            var endSearchResult = locationSearch.SearchLocation();
            if (!endSearchResult.Status)
            {
                outputHelper.ExitWithError(endSearchResult.Message);
            }

            var endLocation = endSearchResult.Result;

            // Calculation
            var geoCalculations = new GeoCalculations();

            var startCoord = geoCalculations.FindCoordinates(startLocation.Latitude, startLocation.Longitude);
            var endCoord = geoCalculations.FindCoordinates(endLocation.Latitude, endLocation.Longitude);

            var distanceInMetres = geoCalculations.CalculateDistanceInMetres(startCoord, endCoord);


            outputHelper.ClearScreen();
            outputHelper.DisplayLocation("Start Location", startLocation);
            outputHelper.DisplayLocation("End Location", endLocation);
            outputHelper.DisplayDistance(distanceInMetres);


            // Midpoint
            var midPoint = geoCalculations.MidPoint(startCoord, endCoord);
            var midPointLocation = locationSearch.FindLocationFromGeoCoordinates(midPoint.Latitude, midPoint.Longitude);

            outputHelper.DisplayLocation("Midpoint", midPointLocation.Result);


            // Points of interest
            var poi = locationSearch.FindPointsOfInterest(midPoint.Latitude, midPoint.Longitude);
            if (!poi.Status)
            {
                outputHelper.ExitWithError(poi.Message);
            }

            outputHelper.DisplayPOI(poi.Result);

            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }
    }
}
