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

                var result = googleMaps.GetGeoCoordinates(searchTerm).Result;

                if (!result.Status)
                {
                    // TODO Handle zero found
                }

                searchResult = result.Result;

                Console.WriteLine();
                Console.WriteLine($"Address: {searchResult.Address}");
                Console.WriteLine($"Lat: {searchResult.Latitude}");
                Console.WriteLine($"Long: {searchResult.Longitude}");
                Console.WriteLine();

                Console.Write("Accept this result? (Y/N) ");
                accept = Console.ReadKey().KeyChar.ToString();
            }

            return new ResultContainer<LocationDetails>
                   {
                       Status = true,
                       Result = searchResult
            };
        }
    }
}
