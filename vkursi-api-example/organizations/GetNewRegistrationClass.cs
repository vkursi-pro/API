using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class GetNewRegistrationClass
    {
        // 15. Запит на отримання списку нових компаній(компаній / ФОП)
        // [POST] api/1.0/organizations/getnewregistration

        public static List<GetAdvancedOrganizationResponseModel> GetNewRegistration(string dateReg, string type, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getnewregistration", Method.POST);

            // Example: {"dateReg":"29.10.2019","type":"1"}

            string body = "{\"dateReg\":\"29.10.2019\",\"type\":\"1\"}";

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

            List<GetAdvancedOrganizationResponseModel> NewRegistrationList = JsonConvert.DeserializeObject<List<GetAdvancedOrganizationResponseModel>>(responseString);

            return NewRegistrationList;
        }
    }
}
