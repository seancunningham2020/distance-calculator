using System;
using distance_calculator.Models;

namespace distance_calculator.Services
{
    public class LocationSearch
    {
        public ResultContainer<LocationDetails> SearchLocation(string apiKey, string googleMapsBaseUrl)
        {
            var googleMaps = new GoogleMaps(apiKey, googleMapsBaseUrl);
            var accept = "N";
            var searchResult = new LocationDetails();

            while (accept.ToUpper() == "N")
            {
                Console.Write(Environment.NewLine + "Enter Location: ");
                var searchTerm = Console.ReadLine();

                if (searchTerm == string.Empty)
                {
                    Console.WriteLine("Please enter a location for which to search.");
                    continue;
                }

                var result = googleMaps.GetGeoCoordinates(searchTerm).Result;

                if (result.Message == "ZERO_RESULTS")
                {
                    Console.WriteLine("No matches were found for your search term. Please try again.");
                    continue;
                }

                if (!result.Status)
                {
                    return result;
                }
                
                searchResult = result.Result;

                DisplayResults(searchResult);

                Console.Write("Accept this result? (Y/N) ");
                accept = Console.ReadLine();
            }

            return new ResultContainer<LocationDetails>
                   {
                       Status = true,
                       Result = searchResult
            };
        }

        private void DisplayResults(LocationDetails location)
        {
            Console.WriteLine();
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Lat: {location.Latitude}");
            Console.WriteLine($"Long: {location.Longitude}");
            Console.WriteLine();
        }
    }
}
