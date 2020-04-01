using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class GetAllReestrClass
    {
        // 7.	Отримати перелік списків контрагентів
        // [GET]   /api/1.0/monitoring/getAllReestr

        public static List<GetAllReestrResponseModel> GetAllReestr(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/monitoring/getAllReestr", Method.GET);

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
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

            List<GetAllReestrResponseModel> getAllReestrResponse = JsonConvert.DeserializeObject<List<GetAllReestrResponseModel>>(responseString);

            return getAllReestrResponse;
        }
    }

    public class GetAllReestrResponseModel
    {
        public Guid Id { get; set; }            // Id списку
        public string Name { get; set; }        // Номер списку
    }
}
