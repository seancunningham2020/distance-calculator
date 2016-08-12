using System.IO;
using System.Text;

namespace distance_calculator
{
    public class GetApiKey
    {
        public string GetKeyFromFile(string fileName)
        {
            //TODO: Check file exists

            var pathAndFile = string.Format("{0}\\{1}", Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, fileName);
            
            return File.ReadAllText(pathAndFile, Encoding.UTF8);
        }
    }
}
