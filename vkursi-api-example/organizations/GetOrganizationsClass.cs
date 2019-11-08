using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class GetOrganizationsClass
    {
        // 2.	Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
        // [POST] /api/1.0/organizations/getorganizations

        public static List<GetOrganizationsResponseModel> GetOrganizations(string code, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            string body = "[\"" + code + "\"]";

            //Example1: body = "{\"code\": [\"40073472\", \"41462280\"]}";
            //Example2: body = "[\"40073472\", \"41462280\"]";

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getorganizations", Method.POST);
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

            List<GetOrganizationsResponseModel> OrganizationsList = JsonConvert.DeserializeObject<List<GetOrganizationsResponseModel>>(responseString);

            return OrganizationsList;
        }
    }
    public class GetOrganizationsResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Edrpou { get; set; }
        public string ChiefName { get; set; }
        public string State { get; set; }
        public DateTime? DateRegInn { get; set; }
        public string Inn { get; set; }
        public DateTime? DateCanceledInn { get; set; }
        public bool? HasBorg { get; set; }
        public bool? InSanctions { get; set; }
        public int? Introduction { get; set; }
        public int? ExpressScore { get; set; }
        public SingleTaxPayer singleTaxPayer { get; set; }
    }
}
