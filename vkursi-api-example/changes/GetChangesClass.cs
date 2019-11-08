using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.changes
{
    public class GetChangesClass
    {
        // 6.	Історія змін по компаніям які додані на моніторинг
        // [GET] /api/1.0/changes/getchanges

        public static List<GetChangesResponseModel> GetChanges(string date, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/changes/getchanges", Method.GET);

            request.AddParameter("date", date);
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

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

            List<GetChangesResponseModel> ChangesResponseList = JsonConvert.DeserializeObject<List<GetChangesResponseModel>>(responseString);

            return ChangesResponseList;
        }
    }

    public class OwnerChangesInfo
    {
        public string id { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

    public class GetChangesResponseModel
    {
        public DateTime dateOfChange { get; set; }
        public string changeType { get; set; }
        public string change { get; set; }
        public OwnerChangesInfo ownerChangesInfo { get; set; }
    }
}
