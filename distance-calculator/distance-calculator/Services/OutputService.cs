using System;

namespace distance_calculator.Services
{
    public class OutputService
    {
        public void DisplayLocation(string label, LocationDetails location)
        {
            Console.WriteLine(Environment.NewLine + label + Environment.NewLine);
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Lat: {location.Latitude}");
            Console.WriteLine($"Long: {location.Longitude}");
            Console.WriteLine();
        }

        public void DisplayPOI(LocationDetails location)
        {
            Console.WriteLine(Environment.NewLine + "Nearest Point of Interest" + Environment.NewLine);
            Console.WriteLine($"Name: {location.Name}");
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Type: {location.LocationType}");
            Console.WriteLine($"Lat: {location.Latitude}");
            Console.WriteLine($"Long: {location.Longitude}");
            Console.WriteLine();
        }
    }
}
