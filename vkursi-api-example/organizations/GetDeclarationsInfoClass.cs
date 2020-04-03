using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetDeclarationsInfoClass
    {
        /*
         
        36. Перелік декларантів повязаних з компаніями
        [POST] /api/1.0/organizations/getdeclarationsinfo      
        
        */

        public static GetDeclarationsInfoResponseModel GetDeclarationsInfo(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDeclarationsInfoRequestBodyModel GDIRequestBody = new GetDeclarationsInfoRequestBodyModel
                {
                    Edrpou = new List<string> 
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GDIRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getdeclarationsinfo");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
                }
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
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

            GetDeclarationsInfoResponseModel GDIResponse = new GetDeclarationsInfoResponseModel();

            GDIResponse = JsonConvert.DeserializeObject<GetDeclarationsInfoResponseModel>(responseString);

            return GDIResponse;
        }
    }

    public class GetDeclarationsInfoRequestBodyModel                            // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
    }

    public class GetDeclarationsInfoResponseModel                               // Модель відповіді GetDeclarationsInfo
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API
        public List<OrgEDRApiApiAnswerModelData> Data { get; set; }             // Дані
    }

    public class OrgEDRApiApiAnswerModelData                                    // Дані
    {
        public string Edrpou { get; set; }                                      // ЄДРПОУ / ІПН 
        public List<OrgEDRApiApiAnswerModelDataPerYear> DataPerYear { get; set; }       // Перелік декларантів по рокам
    }

    public class OrgEDRApiApiAnswerModelDataPerYear                             // Перелік декларантів по рокам
    {
        public int Year { get; set; }                                           // Рік
        public List<OrgEDRApiApiAnswerModelDataObj> List { get; set; }          // Перелік декларантів
    }

    public class OrgEDRApiApiAnswerModelDataObj                                 // Перелік декларантів
    {
        public string Name { get; set; }                                        // ПІБ + тип відношення
        public decimal Sum { get; set; }                                        // Сума
    }
}
