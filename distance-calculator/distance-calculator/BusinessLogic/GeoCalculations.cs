using System;
using System.Device.Location;

namespace distance_calculator.BusinessLogic
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


        public GeoCoordinate MidPoint(GeoCoordinate posA, GeoCoordinate posB)
        {
            GeoCoordinate midPoint = new GeoCoordinate();

            double dLon = DegreesToRadians(posB.Longitude - posA.Longitude);
            double Bx = Math.Cos(DegreesToRadians(posB.Latitude)) * Math.Cos(dLon);
            double By = Math.Cos(DegreesToRadians(posB.Latitude)) * Math.Sin(dLon);

            midPoint.Latitude = RadiansToDegrees(Math.Atan2(
                         Math.Sin(DegreesToRadians(posA.Latitude)) + Math.Sin(DegreesToRadians(posB.Latitude)),
                         Math.Sqrt(
                             (Math.Cos(DegreesToRadians(posA.Latitude)) + Bx) *
                             (Math.Cos(DegreesToRadians(posA.Latitude)) + Bx) + By * By)));

            midPoint.Longitude = posA.Longitude + RadiansToDegrees(Math.Atan2(By, Math.Cos(DegreesToRadians(posA.Latitude)) + Bx));

            return midPoint;
        }

        private double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
