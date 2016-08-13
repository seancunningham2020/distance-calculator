﻿using System;
using System.Configuration;
using distance_calculator.Services;

namespace distance_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configuration
            var apiKeyFile = ConfigurationManager.AppSettings["googleMapsApiKeyFile"];
            var getApiKey = new GetApiKey();
            var apiKeyResult = getApiKey.GetKeyFromFile(apiKeyFile);

            if (!apiKeyResult.Status)
            {
                ExitWithError(apiKeyResult.Message);
            }

            var googleMapsBaseUrl = ConfigurationManager.AppSettings["googleMapsBaseUrl"];

            var locationSearch = new LocationSearch();

            // Start Location
            Console.WriteLine("Start Location");
            var startSearchResult = locationSearch.SearchLocation(apiKeyResult.Result, googleMapsBaseUrl);
            if (!startSearchResult.Status)
            {
                ExitWithError(startSearchResult.Message);
            }

            var startLocation = startSearchResult.Result;

            // End Location
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "End Location");
            var endSearchResult = locationSearch.SearchLocation(apiKeyResult.Result, googleMapsBaseUrl);
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
            
            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }

        static void OutputResults(LocationDetails startLocation, LocationDetails endLocation, double distanceInMetres)
        {
            Console.Clear();

            var locationSearch = new LocationSearch();

            locationSearch.DisplayResults(startLocation);
            locationSearch.DisplayResults(endLocation);

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
