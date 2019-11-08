using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionsClass
    {
        // 10.	Запит на отримання даних по судовим рішенням організації
        // [POST] /api/1.0/courtdecision/getdecisions

        public static GetDecisionsResponseModel GetDecisions(string edrpou, int skip, int typeSide, int justiceKindId, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/courtdecision/getdecisions", Method.POST);

            string body = "{\"edrpou\":\"" + edrpou + "\",\"skip\":"+ skip + ",\"typeSide\":"+ typeSide + ",\"justiceKindId\":"+ justiceKindId + "}";
            body = "{\"edrpou\":\"14360570\",\"skip\":100,\"typeSide\":1}";
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

            GetDecisionsResponseModel DecisionsResponseRow = JsonConvert.DeserializeObject<GetDecisionsResponseModel>(responseString);

            return DecisionsResponseRow;
        }
    }

    public class ListDecisions
    {
        public int id { get; set; }
        public DateTime adjudicationDate { get; set; }
    }

    public class GetDecisionsResponseModel
    {
        public double totalDecision { get; set; }
        public List<ListDecisions> list { get; set; }
    }
}
