﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace distance_calculator
{
    class GoogleMaps
    {
        private string API_KEY = string.Empty;
        private string googleMapsBaseUrl = string.Empty;

        public GoogleMaps(string api_key, string googleMapsBaseUrl)
        {
            this.API_KEY = api_key;
            this.googleMapsBaseUrl = googleMapsBaseUrl;
        }

        public void SetApiKey(string key)
        {
            if ((key == null) || (key == string.Empty))
            {
                throw new ArgumentException("API Key is invalid");
            }

            API_KEY = key;
        }

        public void SetGoogleMapsBaseUrl(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                throw new ArgumentException("Google Maps URL Invalid");
            }

            googleMapsBaseUrl = url;
        }

        public async Task<LocationDetails> GetGeoCoordinates(string addr)
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
                    // TODO:: Return error
                }
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<GeoResults>();

                    if (data.status != "OK")
                    {
                        // TODO: Return error
                    }

                    searchResult.Latitude = data.results[0].geometry.location.lat;
                    searchResult.Longitude = data.results[0].geometry.location.lng;
                    searchResult.Address = data.results[0].formatted_address.ToString();
                }
            }

            return searchResult;
        }
    }
}