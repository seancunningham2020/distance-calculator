﻿using System;
using distance_calculator.Models;
using distance_calculator.Helpers;

namespace distance_calculator.BusinessLogic
{
    public class LocationSearch
    {
        private GoogleMaps _googleMaps;

        public LocationSearch()
        {
            _googleMaps = new GoogleMaps();
        }

        public ResultContainer<LocationDetails> SearchLocation()
        {
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

                var result = _googleMaps.GetLocationGeoCoordinates(searchTerm).Result;

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

                var outputHelper = new OutputHelper();
                outputHelper.DisplayLocation("Search Results:", searchResult);

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
            return _googleMaps.FindGeoCoordinatesLocation(latitude, longitude).Result;
        }

        public ResultContainer<LocationDetails> FindPointsOfInterest(double latitude, double longitude)
        {
            return _googleMaps.FindPointsOfInterest(latitude, longitude, 5).Result;
        }
    }
}
