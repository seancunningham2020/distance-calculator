using System.IO;
using System.Text;
using distance_calculator.Models;

namespace distance_calculator
{
    public class GetApiKey
    {
        public ResultContainer<string> GetKeyFromFile(string fileName)
        {
            var pathAndFile = string.Format("{0}\\{1}", Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, fileName);

            if (!File.Exists(pathAndFile))
            {
                return new ResultContainer<string>()
                       {
                           Status = false,
                           Result = null,
                           Message = "Cannot find api key file"
                };
            }

            return new ResultContainer<string>()
                   {
                       Status = true,
                       Result = File.ReadAllText(pathAndFile, Encoding.UTF8)
                   };
        }
    }
}
