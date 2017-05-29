using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InAGalaxyFarFarFarAway;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace UnitTests
{
    [TestClass]
    public class ValidInputs
    {
        [TestMethod]
        public void ValidInputAlphaNumericInputReturnsFalse()
        {
            // Can't convert from string to Int64 
            Program.Globals.MGLT = "@9828-792.0k";

            Assert.IsFalse(Program.ValidInput);
        }

        [TestMethod]
        public void ValidInputNumericNegativeReturnsFalse()
        {
            // Can't convert from string to Int64
            Program.Globals.MGLT = "-126000";

            Assert.IsFalse(Program.ValidInput);
        }

        [TestMethod]
        public void ValidInputNumericPositiveOutwithRangeReturnsFalse()
        {
            // Can't convert from string to Int64
            Program.Globals.MGLT = "9223372036854775808";

            Assert.IsFalse(Program.ValidInput);
        }

        [TestMethod]
        public void ValidInputNumericPositiveWithinRangeReturnsTrue()
        {
            // Can convert from string to Int64
            Program.Globals.MGLT = "12560";

            Assert.IsTrue(Program.ValidInput);
        }

        [TestMethod]
        public void CalculateResupplyFrequency_MillenniumFalcon()
        {
            Program.Globals.MGLT = "1000000";
            string resupply = (Convert.ToInt32(Program.Globals.MGLT) / (75 * 1460)).ToString();

            Starship milleniumFalcon = new Starship()
            {
                Consumables = "2 months",
                Manufacturer = "Corellian Engineering Corporation",
                Model = "YT-1300 light freighter",
                MGLT = "75",
                Name = "Millennium Falcon",
                Url = "http://swapi.co/api/starships/10/"
            };

            Assert.AreEqual(resupply, Program.CalculateResupplyFrequency(milleniumFalcon));
        }
    }

    [TestClass]
    public class APITests
    {

        [TestMethod]
        public async Task GetResponseFromStarshipApi()
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://swapi.co/api/starships/")
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress.ToString()),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await httpClient.SendAsync(request))
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

    }
}
