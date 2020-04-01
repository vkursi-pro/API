using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

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

    public class OwnerChangesInfo                                   // Інформация про організацію / ФОП по якому відбулась зміна
    {
        public string id { get; set; }                              // Системний Id
        public int type { get; set; }                               // Тип (1 - організация | 2 - фізична особа)
        public string name { get; set; }                            // Найменування
        public string code { get; set; }                            // Код ІНП / Єдрпоу
    }

    public class GetChangesResponseModel
    {
        public DateTime dateOfChange { get; set; }                  // Дата зміни
        public string changeType { get; set; }                      // Тип зміни
        public string change { get; set; }                          // Опис інформмації по зміну
        public OwnerChangesInfo ownerChangesInfo { get; set; }      // Інформация про організацію / ФОП по якому відбулась зміна
    }
}
