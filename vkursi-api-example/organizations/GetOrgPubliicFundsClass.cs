using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgPubliicFundsClass
    {
        /*
         
        34. Загальна статистики по Edata (по компанії)
        [POST] /api/1.0/organizations/getorgpubliicfunds    
        
        */

        public static GetOrgPubliicFundsResponseModel GetOrgPubliicFunds(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgPubliicFundsRequestBodyModel GOPFRequestBody = new GetOrgPubliicFundsRequestBodyModel
                {
                    Edrpou = new List<string>                                       // Перелік ЄДРПОУ / ІПН (обмеження 1)
                    {
                        code
                    }
                };

                string body = JsonConvert.SerializeObject(GOPFRequestBody);         // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgpubliicfunds");
                RestRequest request = new RestRequest(Method.POST);

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

            GetOrgPubliicFundsResponseModel GOPFResponse = new GetOrgPubliicFundsResponseModel();

            GOPFResponse = JsonConvert.DeserializeObject<GetOrgPubliicFundsResponseModel>(responseString);

            return GOPFResponse;
        }

        public class GetOrgPubliicFundsRequestBodyModel                             // Модель Body запиту
        {
            public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
        }

        public class GetOrgPubliicFundsResponseModel                                // Модель відповіді GetOrgPubliicFunds
        {
            public bool IsSucces { get; set; }                                      // Статус відповіді по API
            public string Status { get; set; }                                      // Чи успішний запит
            public List<OrgPublicFundsApiAnswerModelData> Data { get; set; }        // Дані
        }

        public class OrgPublicFundsApiAnswerModelData                               // Дані
        {
            public string Edrpou { get; set; }                                      // ЄДРПОУ / ІПН 
            public decimal FundsReceived { get; set; }                              // Сума отриманих коштів
            public int Payers { get; set; }                                         // Кількість унікальних платників
            public int Transactions { get; set; }                                   // кількість транзакцій
            public List<OrgPublicFundsApiAnswerModelDataDataPerYear> DataPerYear { get; set; }  // ТОР-3 і максімальний мінамальній платіж по рокам
        }

        public class OrgPublicFundsApiAnswerModelDataDataPerYear                    // ТОР-3 і максімальний мінамальній платіж по рокам
        {
            public int Year { get; set; }                                                           // Рік
            public List<OrgPublicFundsApiAnswerModelDataDataPerYearTopPay> TopPays { get; set; }    // ТОП-3 Платники
            public OrgPublicFundsApiAnswerModelDataDataPerYearMinMax Min { get; set; }              // Мінімальний платіж
            public OrgPublicFundsApiAnswerModelDataDataPerYearMinMax Max { get; set; }              // Максимальний платіж
        }


        public class OrgPublicFundsApiAnswerModelDataDataPerYearTopPay              // ТОП-3 Платники
        {
            public string Name { get; set; }                                        // Назва платника
            public string Code { get; set; }                                        // Код платника
            public decimal Sum { get; set; }                                        // Загальна сума транзакцій
            public int Count { get; set; }                                          // Кількість транзакцій
        }

        public class OrgPublicFundsApiAnswerModelDataDataPerYearMinMax              // Мінімальний / Максимальний платіж
        {
            public string namepayer { get; set; }                                   // Назва платника
            public string codepayer { get; set; }                                   // Код платника
            public DateTime? date { get; set; }                                     // Дата транзакції
            public string purpose { get; set; }                                     // Призначення
            public decimal? sum { get; set; }                                       // Сума
        }
    }
}
