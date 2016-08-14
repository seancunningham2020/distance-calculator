using System;

namespace distance_calculator.Helpers
{
    public class OutputHelper
    {
        public void ClearScreen()
        {
            Console.Clear();
        }

        public void DisplayLocation(string label, LocationDetails location)
        {
            Console.WriteLine(Environment.NewLine + label + Environment.NewLine);
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Lat: {location.Latitude}");
            Console.WriteLine($"Long: {location.Longitude}");
            Console.WriteLine();
        }

        public void DisplayDistance(double distanceInMetres)
        {
            Console.WriteLine();
            Console.WriteLine($"Distance: {distanceInMetres / 1000:0.00} km");
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

        public void ExitWithError(string errorMessage)
        {
            Console.WriteLine("ERROR:: {0}", errorMessage);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
