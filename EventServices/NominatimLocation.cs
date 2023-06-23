using EventServices.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServices
{
    public class NominatimLocation: INominatimLocation
    {
        private const string NominatimBaseUrl = "https://nominatim.openstreetmap.org/";

        public async Task<string> GetMapUrlForAddress(string address)
        {


            var response = IfCorrectAddress(address);
            if (response.Result.IsSuccessful/* && response.Content!="[]"*/)
            {
                var mapUrl = $"https://maps.google.com/maps?q={address}";
                //var mapUrl = $"https://nominatim.openstreetmap.org/search?q={address}";
                //var mapUrl = GetMapUrlFromResponse(response.Content);
                return mapUrl;
            }

            return null;
        }
        public async Task<RestResponse> IfCorrectAddress(string address)
        {
            var client = new RestClient(NominatimBaseUrl);
            var request = new RestRequest("search", Method.Get);
            request.AddParameter("format", "json");
            request.AddParameter("q", address);
            var result = await client.ExecuteAsync(request);
            return result;
        } 
        private string GetMapUrlFromResponse(string jsonResponse)
        {
            // Parse the JSON response to extract the latitude and longitude
            // and construct the map URL for redirection
            // The JSON structure returned by Nominatim may vary, so adjust this code accordingly

            // Example code assuming the JSON response structure is like:
            // [
            //   {
            //     "lat": "51.5074",
            //     "lon": "-0.1278"
            //   }
            // ]

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
            if (result != null && result.Count > 0)
            {
                string lat = result[0].lat;
                string lon = result[0].lon;
                return $"https://maps.google.com/maps?q={lat},{lon}";
                //return $"https://www.openstreetmap.org/?mlat={lat}&mlon={lon}#map=16/{lat}/{lon}";
            }

            return null;
        }

    }
}
