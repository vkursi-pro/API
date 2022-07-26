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
        /// <summary>
        /// Модель Body запиту
        /// </summary>
        public class GetOrgPubliicFundsRequestBodyModel                             // 
        {/// <summary>
         /// Перелік ЄДРПОУ / ІПН (обмеження 1)
         /// </summary>
            public List<string> Edrpou { get; set; }                                // 
        }
        /// <summary>
        /// Модель відповіді GetOrgPubliicFunds
        /// </summary>
        public class GetOrgPubliicFundsResponseModel                                // 
        {/// <summary>
         /// Статус відповіді по API
         /// </summary>
            public bool IsSucces { get; set; }                                      // 
            /// <summary>
            /// Чи успішний запит
            /// </summary>
            public string Status { get; set; }                                      // 
            /// <summary>
            /// Дані
            /// </summary>
            public List<OrgPublicFundsApiAnswerModelData> Data { get; set; }        // 
        }
        /// <summary>
        /// Дані
        /// </summary>
        public class OrgPublicFundsApiAnswerModelData                               // 
        {/// <summary>
         /// ЄДРПОУ / ІПН 
         /// </summary>
            public string Edrpou { get; set; }                                      // 
            /// <summary>
            /// Сума отриманих коштів
            /// </summary>
            public decimal FundsReceived { get; set; }                              // 
            /// <summary>
            /// Кількість унікальних платників
            /// </summary>
            public int Payers { get; set; }                                         // 
            /// <summary>
            /// кількість транзакцій
            /// </summary>
            public int Transactions { get; set; }                                   // 
            /// <summary>
            /// ТОР-3 і максімальний мінамальній платіж по рокам
            /// </summary>
            public List<OrgPublicFundsApiAnswerModelDataDataPerYear> DataPerYear { get; set; }  // 
        }
        /// <summary>
        /// ТОР-3 і максімальний мінамальній платіж по рокам
        /// </summary>
        public class OrgPublicFundsApiAnswerModelDataDataPerYear                    // 
        {/// <summary>
         /// Рік
         /// </summary>
            public int Year { get; set; }                                                           // 
            /// <summary>
            /// ТОП-3 Платники
            /// </summary>
            public List<OrgPublicFundsApiAnswerModelDataDataPerYearTopPay> TopPays { get; set; }    // 
            /// <summary>
            /// Мінімальний платіж
            /// </summary>
            public OrgPublicFundsApiAnswerModelDataDataPerYearMinMax Min { get; set; }              // 
            /// <summary>
            /// Максимальний платіж
            /// </summary>
            public OrgPublicFundsApiAnswerModelDataDataPerYearMinMax Max { get; set; }              // 
        }

        /// <summary>
        /// ТОП-3 Платники
        /// </summary>
        public class OrgPublicFundsApiAnswerModelDataDataPerYearTopPay              // 
        {/// <summary>
         ///  Назва платника
         /// </summary>
            public string Name { get; set; }                                        //
            /// <summary>
            /// Код платника
            /// </summary>
            public string Code { get; set; }                                        // 
            /// <summary>
            ///Загальна сума транзакцій 
            /// </summary>
            public decimal Sum { get; set; }                                        // 
            /// <summary>
            /// Кількість транзакцій
            /// </summary>
            public int Count { get; set; }                                          // 
        }
        /// <summary>
        ///Мінімальний / Максимальний платіж 
        /// </summary>
        public class OrgPublicFundsApiAnswerModelDataDataPerYearMinMax              // 
        {/// <summary>
         /// Назва платника
         /// </summary>
            public string namepayer { get; set; }                                   // 
            /// <summary>
            /// Код платника
            /// </summary>
            public string codepayer { get; set; }                                   // 
            /// <summary>
            /// Дата транзакції
            /// </summary>
            public DateTime? date { get; set; }                                     // 
            /// <summary>
            /// Призначення
            /// </summary>
            public string purpose { get; set; }                                     // 
            /// <summary>
            /// Сума
            /// </summary>
            public decimal? sum { get; set; }                                       // 
        }
    }
}
