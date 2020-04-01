using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class RemoveFromControlClass
    {
        // 13.	Видилити контрагентів зі списку
        // [POST] /api/1.0/Monitoring/removeFromControl

        public static void RemoveFromControl(string code, string reestrId, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/monitoring/removeFromControl", Method.POST);

            string body = "{\"codes\":[\""+ code + "\"],\"reestrId\":\"" + reestrId + "\"}";
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

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
        }
    }
}
