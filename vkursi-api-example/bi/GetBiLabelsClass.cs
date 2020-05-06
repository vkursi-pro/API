using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.bi
{
    public class GetBiLabelsClass
    {
        /*
         
        Отримати перелік Label доступних в модулі BI
        [GET] api/1.0/bi/GetBiLabels

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetBiLabels' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \

        */

        public static GetBiLabelsResponsModel GetBiLabels(string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetBiLabels");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetBiLabelsResponsModel GBILRespons = new GetBiLabelsResponsModel();

            GBILRespons = JsonConvert.DeserializeObject<GetBiLabelsResponsModel>(responseString);

            return GBILRespons;

        }
    }

    public class GetBiLabelsResponsModel                                        // Модель відповіді GetBiImportLabels
    {
        public bool isSuccess { get; set; }                                     // Успішно виконано?
        public string status { get; set; }                                      // success, error
        public List<LabelsData> data { get; set; }                              // Перелік доступних Labels
    }

    public class LabelsData                                                     // Перелік доступних Labels
    {
        public Guid id { get; set; }                                            // Id label
        public string name { get; set; }                                        // Назва label
        public string searchParam { get; set; }                                 // Пошукові параметри за якими label був сформований 
        public int? countRows { get; set; }                                     // Загальна кількість записів
        public DateTime? minDate { get; set; }                                  // Мінімальна дата додавання компанії
        public DateTime dateCreate { get; set; }                                // Дата створення label
    }
}
