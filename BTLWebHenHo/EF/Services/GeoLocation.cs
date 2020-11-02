using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

using System.Web.Script.Serialization;

namespace BTLWebHenHo.EF.Services
{
    public class GeoLocation
    {
        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
                string uri = "https://maps.googleapis.com/maps/api/geocode/json?address=&#8221" + region + "&key=AIzaSyApc0VOnZfvogwncEglVuRkJkAcIWtuJp8";

                string geocodeInfo = client.DownloadString(uri);
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                GoogleGeoCodeResp latlongdata = oJS.Deserialize<GoogleGeoCodeResp>(geocodeInfo);

                return new Coordinate(Convert.ToDouble(latlongdata.results[0].geometry.location.lat), Convert.ToDouble(latlongdata.results[0].geometry.location.lng));
            }
        }

        public struct Coordinate
        {
            private double lat;
            private double lng;

            public Coordinate(double latitude, double longitude)
            {
                lat = latitude;
                lng = longitude;

            }

            public double Latitude { get { return lat; } set { lat = value; } }
            public double Longitude { get { return lng; } set { lng = value; } }

        }
    }
}