using System;
using distance_calculator.Models;

namespace distance_calculator.Services
{
    public class LocationSearch
    {
        public ResultContainer<LocationDetails> SearchLocation()
        {
            var googleMaps = new GoogleMaps();
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

                var outputService = new OutputService();
                outputService.DisplayLocation(searchResult);

                Console.Write("Accept this result? (Y/N) ");
                accept = Console.ReadLine();
            }

            return new ResultContainer<LocationDetails>
                   {
                       Status = true,
                       Result = searchResult
            };
        }

        public ResultContainer<LocationDetails> FindLocationFromGeoCoordinates(double latitude, double longitude)
        {
            var googleMaps = new GoogleMaps();

            return googleMaps.FindGeoCoordinatesLocation(latitude, longitude).Result;
        }

        public ResultContainer<LocationDetails> FindPointsOfInterest(double latitude, double longitude)
        {
            var googleMaps = new GoogleMaps();

            return googleMaps.FindPointsOfInterest(latitude, longitude, 10).Result;
        }

    }
}
