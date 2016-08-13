using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using distance_calculator.Models;

namespace distance_calculator
{
    class GoogleMaps
    {
        private string API_KEY = string.Empty;
        private string googleMapsBaseUrl = string.Empty;
        private string googlePlacesBaseUrl = string.Empty;

        public GoogleMaps()
        {
            var apiKeyFile = ConfigurationManager.AppSettings["googleMapsApiKeyFile"];
            var getApiKey = new GetApiKey();
            var apiKeyResult = getApiKey.GetKeyFromFile(apiKeyFile);

            if (!apiKeyResult.Status)
            {
                // TODO: Fix this
                //ExitWithError(apiKeyResult.Message);
            }

            API_KEY = apiKeyResult.Result;

            googleMapsBaseUrl = ConfigurationManager.AppSettings["googleMapsBaseUrl"];
            googlePlacesBaseUrl = ConfigurationManager.AppSettings["googlePlacesBaseUrl"];
        }

        public GoogleMaps(string api_key, string googleMapsBaseUrl)
        {
            this.API_KEY = api_key;
            this.googleMapsBaseUrl = googleMapsBaseUrl;
        }
        
        public async Task<ResultContainer<LocationDetails>> GetGeoCoordinates(string addr)
        {
            var searchResult = new LocationDetails();

            var queryString = string.Format("?address={0}&key={1}", Uri.EscapeUriString(addr), this.API_KEY);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(googleMapsBaseUrl);
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

        public async Task<ResultContainer<LocationDetails>> FindGeoCoordinatesLocation(double latitude, double longitude)
        {
            var searchResult = new LocationDetails();

            var queryString = string.Format("?latlng={0},{1}&key={2}", Uri.EscapeUriString(latitude.ToString()), Uri.EscapeUriString(longitude.ToString()), this.API_KEY);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(googleMapsBaseUrl);
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

                    searchResult.Address = data.results[0].formatted_address;
                }
            }

            return new ResultContainer<LocationDetails>
            {
                Status = true,
                Result = searchResult
            };
        }

        public async Task<ResultContainer<LocationDetails>> FindPointsOfInterest(double latitude, double longitude, int radius)
        {
            var queryString = string.Format("?location={0},{1}&key={2}", Uri.EscapeUriString(latitude.ToString()), Uri.EscapeUriString(longitude.ToString()), this.API_KEY);

            var searchResult = new LocationDetails();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(googlePlacesBaseUrl);
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
                            Result = null
                        };
                    }

                    //searchResult.Address = data.results[0].formatted_address;
                }
            }

            return new ResultContainer<LocationDetails>
            {
                Status = true,
                Result = null //searchResult
            };
        }
    }
}
