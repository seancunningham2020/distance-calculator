using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using distance_calculator.Models;
using distance_calculator.Helpers;

namespace distance_calculator
{
    class GoogleMaps
    {
        private string _apiKey = string.Empty;
        private string _googleMapsBaseUrl = string.Empty;
        private string _googlePlacesBaseUrl = string.Empty;

        public GoogleMaps()
        {
            var apiKeyFile = ConfigurationManager.AppSettings["googleMapsApiKeyFile"];
            var getApiKey = new GetApiKey();
            var apiKeyResult = getApiKey.GetKeyFromFile(apiKeyFile);

            if (!apiKeyResult.Status)
            {
                var outputHelper = new OutputHelper();
                outputHelper.ExitWithError(apiKeyResult.Message);
            }

            _apiKey = apiKeyResult.Result;

            _googleMapsBaseUrl = ConfigurationManager.AppSettings["googleMapsBaseUrl"];
            _googlePlacesBaseUrl = ConfigurationManager.AppSettings["googlePlacesBaseUrl"];
        }
        
        public async Task<ResultContainer<LocationDetails>> GetLocationGeoCoordinates(string address)
        {
            var queryString = string.Format("?address={0}&key={1}", Uri.EscapeUriString(address), _apiKey);

            return await CallGoogleMapsApi(queryString);
        }

        public async Task<ResultContainer<LocationDetails>> FindGeoCoordinatesLocation(double latitude, double longitude)
        {
            var queryString = string.Format("?latlng={0},{1}&key={2}", Uri.EscapeUriString(latitude.ToString()), Uri.EscapeUriString(longitude.ToString()), _apiKey);

            return await CallGoogleMapsApi(queryString);
        }

        public async Task<ResultContainer<LocationDetails>> FindPointsOfInterest(double latitude, double longitude, int radius)
        {
            var queryString = string.Format("?location={0},{1}&radius{2}&key={3}&&rankby=distance&type=point_of_interest", 
                Uri.EscapeUriString(latitude.ToString()), 
                Uri.EscapeUriString(longitude.ToString()), 
                Uri.EscapeUriString(radius.ToString()),
                _apiKey);

            var nearestPOI = new LocationDetails();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_googlePlacesBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(queryString);
                if (!response.IsSuccessStatusCode)
                {
                    return new ResultContainer<LocationDetails>
                    {
                        Status = false,
                        Result = null,
                        Message = $"ERROR {response.StatusCode}:: Cannot read from Google Maps API"
                    };
                }

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<PoiResults>();

                    if (data.status != "OK")
                    {
                        return new ResultContainer<LocationDetails>
                        {
                            Status = false,
                            Message = data.status,
                        };
                    }

                    nearestPOI.Name = data.results[0].name;
                    nearestPOI.Address = data.results[0].vicinity;
                    nearestPOI.Latitude = data.results[0].geometry.location.lat;
                    nearestPOI.Longitude = data.results[0].geometry.location.lng;
                    nearestPOI.LocationType = string.Join(",", data.results[0].types);
                }
            }

            return new ResultContainer<LocationDetails>
            {
                Status = true,
                Result = nearestPOI
            };
        }

        private async Task<ResultContainer<LocationDetails>> CallGoogleMapsApi(string queryString)
        {
            var searchResult = new LocationDetails();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_googleMapsBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(queryString);
                if (!response.IsSuccessStatusCode)
                {
                    return new ResultContainer<LocationDetails>
                    {
                        Status = false,
                        Result = null,
                        Message = $"ERROR {response.StatusCode}:: Cannot read from Google Maps API"
                    };
                }

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<GeoResults>();

                    if (data.status != "OK")
                    {
                        return new ResultContainer<LocationDetails>
                        {
                            Status = false,
                            Message = data.status,
                            Result = null
                        };
                    }

                    searchResult.Latitude = data.results[0].geometry.location.lat;
                    searchResult.Longitude = data.results[0].geometry.location.lng;
                    searchResult.Address = data.results[0].formatted_address;
                }
            }

            return new ResultContainer<LocationDetails>
            {
                Status = true,
                Result = searchResult
            };
        }
    }
}
