using System;

namespace distance_calculator.Services
{
    public class OutputService
    {

        public void DisplayLocation(LocationDetails location)
        {
            Console.WriteLine();
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Lat: {location.Latitude}");
            Console.WriteLine($"Long: {location.Longitude}");
            Console.WriteLine();
        }
    }
}
