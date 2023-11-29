using System;
using RestSharp;
using HtmlAgilityPack;
using vkursi_api_example.token;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace vkursi_api_example.monitoring
{
    public class GetContentMonitoringClass
    {
        public static List<GetContentMonitoringResponseModel> GetContent(ref string token, string reestrId)
        {
            if (string.IsNullOrEmpty(token)) { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getcontent");
                RestRequest request = new RestRequest(Method.POST);

                string body = "\"" + reestrId + "\"";                // Example body: "84583482"

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним Id документа не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            List<GetContentMonitoringResponseModel> GCMResponse = new List<GetContentMonitoringResponseModel>();

            GCMResponse = JsonConvert.DeserializeObject<List<GetContentMonitoringResponseModel>>(responseString);

            return GCMResponse;
        }
    }

    /// <summary>
    /// ???
    /// </summary>
    public class GetContentMonitoringResponseModel
    {/// <summary>
     /// Назва
     /// </summary>
        public string Name { get; set; }                        // 
        /// <summary>
        /// Тип (1 - компания, 2 - фоп, 3 - мониторинг по ПИБ, 4 - мониторинг по адресу, 5 - DueDiligence по отчёту,6 - DueDiligence по организации, 7 - отображаем отчёт по земле (estate), 8 - Судебные решения по критериям, 9 - пользовательские связи (страница newRelations))
        /// </summary>
        public int? Type { get; set; }                          //  
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }                        // 
        /// <summary>
        /// Дата додавання
        /// </summary>
        public DateTime? CreateDate { get; set; }               // 
        /// <summary>
        /// На моныторингу
        /// </summary>
        public bool IsOnMonitoring { get; set; }                // 
    }
}
