using System;
using System.Device.Location;

namespace distance_calculator.Services
{
    public class GeoCalculations
    {
        public GeoCoordinate FindCoordinates(double latitude, double longitude)
        {
            return new GeoCoordinate(latitude, longitude);
        }

        public double CalculateDistanceInMetres(GeoCoordinate startCoordinates, GeoCoordinate endCoordinates)
        {
            return startCoordinates.GetDistanceTo(endCoordinates);
        }
    }
}
