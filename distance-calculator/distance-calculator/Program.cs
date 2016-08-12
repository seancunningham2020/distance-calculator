using System;
using System.Configuration;
using System.Device.Location;
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
            var searchResult1 = locationSearch.SearchLocation(apiKeyResult.Result, googleMapsBaseUrl);
            if (!searchResult1.Status)
            {
                ExitWithError(searchResult1.Message);
            }

            var startLocation = searchResult1.Result;

            // Second Location
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Second Location");
            var searchResult2 = locationSearch.SearchLocation(apiKeyResult.Result, googleMapsBaseUrl);
            if (!searchResult2.Status)
            {
                ExitWithError(searchResult2.Message);
            }

            var endLocation = searchResult2.Result;

            // Calculation
            var startCoord = new GeoCoordinate(startLocation.Latitude, startLocation.Longitude);
            var endCoord = new GeoCoordinate(endLocation.Latitude, endLocation.Longitude);

            var distanceInMetres = startCoord.GetDistanceTo(endCoord);

            Console.WriteLine();
            Console.WriteLine($"Distance: {distanceInMetres/1000:0.00} km");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
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
