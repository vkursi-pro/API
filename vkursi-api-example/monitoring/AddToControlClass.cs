using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.monitoring
{
    public class AddToControlClass
    {
        // 12.	Додати контрагентів до списку
        // Monitoring/addToControl

        public static List<AddToControlResponseModel> AddToControl(string code, string ReestrId, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/Monitoring/addToControl", Method.POST);

            // Example: "{\"Codes\":[\"00131305\", \"41462280\"]},\"ReestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\""
            string body = "{\"Codes\":[\"" + code + "\"],\"ReestrId\":\"" + ReestrId + "\"}";

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var responseString = response.Content;

            if (responseString == "Not found")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            List<AddToControlResponseModel> AddToControlRow = JsonConvert.DeserializeObject<List<AddToControlResponseModel>>(responseString);

            return AddToControlRow;
        }
    }

    public class AddToControlResponseModel
    {
        public string name { get; set; }
        public string shortName { get; set; }
        public string code { get; set; }
        public string boss { get; set; }
        public string location { get; set; }
        public string kved { get; set; }
        public string state { get; set; }
        public string dateAddedToMonitoring { get; set; }
    }
}
