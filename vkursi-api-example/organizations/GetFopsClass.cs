using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class GetFopsClass
    {
        // 3.	Запит на отримання коротких даних по ФОП за кодом ІПН
        // [POST] /api/1.0/organizations/getfops

        public static List<GetFopsResponseModel> GetFops(string code, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            //Example1: body = "[\"3131916978\", \"3334800417\"]";
            //Example2: body = "{\"code\": [\"3131916978\", \"3334800417\"]}";

            string body = "[\"" + code + "\"]";

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getfops", Method.POST);

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "\"Not found\"")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            List<GetFopsResponseModel> FopsShortList = JsonConvert.DeserializeObject<List<GetFopsResponseModel>>(responseString);

            return FopsShortList;
        }
    }

    public class GetFopsResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string Inn { get; set; }
        public DateTime? DateCanceledInn { get; set; }
        public DateTime? DateRegInn { get; set; }
        public int? Introduction { get; set; }
        public int? ExpressScore { get; set; }
        public bool? HasBorg { get; set; }
        public bool? InSanctions { get; set; }
        public SingleTaxPayer singleTaxPayer { get; set; }
    }
    public class SingleTaxPayer
    {
        public DateTime dateStart { get; set; }
        public double rate { get; set; }
        public int group { get; set; }
        public object dateEnd { get; set; }
        public string kindOfActivity { get; set; }
        public bool status { get; set; }
    }
}
