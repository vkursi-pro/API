using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetInfoByKoatuuClass
    {
        // 14.	Запит на отримання списку компаній по коду КОАТУУ
        // [POST] api/1.0/organizations/getinfobykoatuu

        public static List<GetInfoByKoatuuResponseModel> GetInfoByKoatuu(string koatuuCode, string type, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getinfobykoatuu", Method.POST);

            // Example: {"koatuuCode":"510900000","type":"1"}

            string body = "{\"koatuuCode\":\"" + koatuuCode + "\",\"type\":\"" + type + "\"}";

            List<GetInfoByKoatuuResponseModel> InfoByKoatuuResponseList = new List<GetInfoByKoatuuResponseModel>();

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "\"Not found\"")
            {
                Console.WriteLine("Not found");
                return InfoByKoatuuResponseList;
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            InfoByKoatuuResponseList = JsonConvert.DeserializeObject<List<GetInfoByKoatuuResponseModel>>(responseString);

            return InfoByKoatuuResponseList;
        }
    }

    public class GetInfoByKoatuuResponseModel
    {
        public string code { get; set; }
    }
}
