using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.monitoring
{
    public class RemoveReestrClass
    {
        // 16. Видалити список контрагентів
        // [DELETE] /api/1.0/monitoring/removeReestr

        public static string RemoveReestr(string reestrId, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            string body = "{\"reestrId\": \"" + reestrId + "\"}";

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/monitoring/addNewReestr", Method.POST);
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

            return responseString;
        }
    }
}
