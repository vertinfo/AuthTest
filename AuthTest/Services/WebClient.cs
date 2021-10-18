using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AuthTest.Services
{
    public class WebClient : IwebClient
    {
        //private ILogger<WebClient> logger = new Logger<WebClient>(new LoggerFactory());
        private readonly ILogger<WebClient> logger;

        JObject jo = new JObject();

        private string tag = "WebClient";
        //Azure endpoint
        private string deviceapi = "https://devicedetails.datahoist.com/api/";
        //local endpoint
        //private string localapi = "http://localhost:5051/api/";
        string qi = "13e15c3d-dbf0-4cd8-8b73-0b2ccbf00cc3";

        public WebClient(ILogger<WebClient> _logger)
        {
            logger = _logger;
        }

        //Test method
        public async Task<string> getData(string end)
        {
            try
            {
                
                HttpClient client = new HttpClient();
                
                //client.DefaultRequestHeaders.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                //client.DefaultRequestHeaders.Add("pass", qi);

                var response = await client.GetStringAsync(deviceapi + end);
                
                return response.ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{tag}: {ex.Message.ToString()}");
                logger.LogWarning($"{tag}: {ex.Message.ToString()}");
            }

            return "fail";
        }
        
        public string sendPost(string api, string pars)
        {
            try
            {
                HttpClient client = new HttpClient();
                logger.LogWarning($"****VxApi body**********{tag}: {pars}");
                HttpContent content = new StringContent(pars, Encoding.UTF8, "application/json");
                var response = client.PostAsync(api, content);
                //string resp = response.Result.Content.ReadAsStringAsync().Result;
                string resp = response.Result.StatusCode.ToString();
                logger.LogWarning($"****VxApi Response**********{tag}: {resp}");
                string result = resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"sendPost exc: {ex.Message.ToString()}");
                logger.LogWarning($"{tag}: {ex.Message.ToString()}");
            }

            return "fail";
        }

        public string sendPut(string api, string pars)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(pars, Encoding.UTF8, "application/json");
                var response = client.PutAsync(api, content);
                return response.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"sendPutt exc: {ex.Message.ToString()}");
                logger.LogWarning($"{tag}: {ex.Message.ToString()}");
            }

            return "fail";
        }

        //public string sendDelete(string table, string id)
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();
              
        //        var response = client.DeleteAsync(table, id);
        //        return response.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message.ToString());
        //    }

        //    return "fail";
        //}


    }

public interface IwebClient
    {
        Task<string> getData(string id);
        string sendPost(string api, string pars);
        string sendPut(string api, string pars);
    }
}
