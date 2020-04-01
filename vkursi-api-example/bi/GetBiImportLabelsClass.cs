using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;


namespace vkursi_api_example.bi
{
    public class GetBiImportLabelsClass
    {
        /* 18. Отримати перелік Label доступних по BI
         * [GET] api/1.0/bi/getbiimportlabels
         */

        public static List<string> GetBiImportLabels(string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/bi/getbiimportlabels", Method.GET);

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            GetBiImportLabelsResponsModel GetBiImportLabelsRespons = JsonConvert.DeserializeObject<GetBiImportLabelsResponsModel>(responseString);

            return GetBiImportLabelsRespons.data;
        }
    }

    public class GetBiImportLabelsResponsModel
    {
        public bool isSuccess { get; set; }                                 // Успішно виконано?
        public string status { get; set; }                                  // success, error
        public List<string> data { get; set; }                              // Список лейблов
    }
}