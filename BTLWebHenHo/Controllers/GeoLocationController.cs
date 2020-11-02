using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Http;

namespace BTLWebHenHo.Controllers
{
    public class GeoLocationController : ApiController
    {

        [HttpGet]
        public string get_location(string address)
        {
            string location="";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.mapbox.com/geocoding/v5/mapbox.places/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(address+ "pk.eyJ1IjoiYmxhY2tob2xlbXRhIiwiYSI6ImNrZ3p1bWNtaDE3a24zMXJybm8wN2NxMXgifQ.GuFsSRfcxJ8fS5vY9rKMhw").Result;
            if (response.IsSuccessStatusCode)
            {
               location = response.Content.ToString();
                
            }
            return location;
        }
      
        // GET: api/GeoLocation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GeoLocation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GeoLocation
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/GeoLocation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GeoLocation/5
        public void Delete(int id)
        {
        }


    }
}
