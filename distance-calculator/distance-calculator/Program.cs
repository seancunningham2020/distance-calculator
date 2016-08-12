using System;
using System.Configuration;
using System.Device.Location;

namespace distance_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKeyFile = ConfigurationManager.AppSettings["googleMapsApiKeyFile"];
            var getApiKey = new GetApiKey();
            var apiKey = getApiKey.GetKeyFromFile(apiKeyFile);

            var googleMapsBaseUrl = ConfigurationManager.AppSettings["googleMapsBaseUrl"];

            var googleMaps = new GoogleMaps(apiKey, googleMapsBaseUrl);

            Console.Write("Enter First Location: ");

            var searchTerm1 = Console.ReadLine();

            var searchResult1 = googleMaps.GetGeoCoordinates(searchTerm1).Result;

            Console.WriteLine();
            Console.WriteLine($"Address: {searchResult1.Address}");
            Console.WriteLine($"Lat: {searchResult1.Latitude}");
            Console.WriteLine($"Long: {searchResult1.Longitude}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Console.WriteLine();
            Console.Write("Enter Second Location: ");

            var searchTerm2 = Console.ReadLine();

            var searchResult2 = googleMaps.GetGeoCoordinates(searchTerm2).Result;

            Console.WriteLine();
            Console.WriteLine($"Address: {searchResult2.Address}");
            Console.WriteLine($"Lat: {searchResult2.Latitude}");
            Console.WriteLine($"Long: {searchResult2.Longitude}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            var startCoord = new GeoCoordinate(searchResult1.Latitude, searchResult1.Longitude);
            var endCoord = new GeoCoordinate(searchResult2.Latitude, searchResult2.Longitude);

            var distanceInMetres = startCoord.GetDistanceTo(endCoord);

            Console.WriteLine();
            Console.WriteLine($"Distance: {distanceInMetres/1000:0.00} km");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
