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
            { 
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
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

    /// <summary>
    /// Модель відповіді GetBiImportLabels
    /// </summary>

    public class GetBiLabelsResponsModel                                        // 
    {
        /// <summary>
        /// Успішно виконано?
        /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// success, error
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// Перелік доступних Labels
        /// </summary>
        public List<LabelsData> data { get; set; }                              // 
    }
        /// <summary>
        /// Перелік доступних Labels
        /// </summary>
    public class LabelsData                                                     // 
    {   /// <summary>
        /// Id label
        /// </summary>
        public Guid id { get; set; }                                            // 
        /// <summary>
        /// Назва label
        /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Пошукові параметри за якими label був сформований 
        /// </summary>
        public string searchParam { get; set; }                                 // 
        /// <summary>
        /// Загальна кількість записів
        /// </summary>
        public int? countRows { get; set; }                                     // 
        /// <summary>
        /// Мінімальна дата додавання компанії
        /// </summary>
        public DateTime? minDate { get; set; }                                  // 
        /// <summary>
        /// Дата створення label
        /// </summary>
        public DateTime dateCreate { get; set; }                                // 
    }
}
