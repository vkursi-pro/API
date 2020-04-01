using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{
    public class GetContentClass
    {
        // 11.	Запит на отримання контенту судового рішення
        // [POST] /api/1.0/courtdecision/getcontent

        public static string GetContent(string courtDecisionId, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/courtdecision/getcontent", Method.POST);

            string body = "\"" + courtDecisionId + "\"";

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

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            return responseString;
        }
    }
}
