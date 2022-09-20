using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionsByFilterClass
    {
        /// <summary>
        /// 67. Запит на отримання повних реквізитів та контенту судових документів організації за критеріями [POST] /api/1.0/courtdecision/getdecisionsbyfilter
        /// </summary>
        /// <param name="edrpou"></param>
        /// <param name="typeSide"></param>
        /// <param name="justiceKindId"></param>
        /// <param name="npas"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        /*
            cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionsByFilter' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1...' \
                --header 'Content-Type: application/json' \
                --data-raw '{"Edrpou":"00131305","TypeSide":null,"JusticeKindId":null,"Npas":null,"ScrollToken":null,"JudgmentFormId":null,"AdjudicationDateFrom":null,"AdjudicationDateTo":null}'
        
            Приклад відповіді:
                    https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetDecisionsByFilterResponse.json
        */


        public static GetDecisionByIdResponseModel GetDecisionsByFilter(string edrpou, int? typeSide, int? justiceKindId, List<string> npas, string token)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDecisionsByFilterRequestBodyModel GDBFRequestBody = new GetDecisionsByFilterRequestBodyModel
                {
                    Edrpou = edrpou,                                            // Код ЄДРПОУ
                    //TypeSide = typeSide,                                        // Тип сторони в судомому документі
                    //JusticeKindId = justiceKindId                              // Форма судочинства
                    //Npas = npas                                                 // Фільтр по статтям до НПА
                };

                string body = JsonConvert.SerializeObject(GDBFRequestBody);                // Example body: {"Edrpou":"14360570","TypeSide":null,"JusticeKindId":null,"Npas":["F545D851-6015-455D-BFE7-01201B629774"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionsByFilter");
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
                    Console.WriteLine("За вказаними параметрами інформації не знайдено");
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

            // Модель відповіди ідентична Методу № 26. Рекізити судового документа [POST] /api/1.0/courtdecision/getdecisionbyid
            // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetDecisionByIdClass.cs#L110

            GetDecisionByIdResponseModel DecisionsByFilterResponseRow = new GetDecisionByIdResponseModel();

            DecisionsByFilterResponseRow = JsonConvert.DeserializeObject<GetDecisionByIdResponseModel>(responseString);

            return DecisionsByFilterResponseRow;
        }
    }
    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetDecisionsByFilterRequestBodyModel                           // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }                                      // 
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }                                 // 
        /// <summary>
        /// Id статті НПА (можна отримати в розробника)
        /// </summary>
        public List<string> Npas { get; set; }                                  // 
        /// <summary>
        /// Скрол для отримання наступних документів
        /// </summary>
        public string ScrollToken { get; set; }                                 // 
        /// <summary>
        /// 1  Вирок, 2 Постанова, 3 Рішення, 4 "Судовий наказ", 5 Ухвала, 6 "Окрема ухвала", 10 "Окрема думка"
        /// </summary>
        public int? JudgmentFormId { get; set; }                                // 
        /// <summary>
        /// Дата рішення від
        /// </summary>
        public DateTime? AdjudicationDateFrom { get; set; }
        /// <summary>
        /// Дата рішення до
        /// </summary>
        public DateTime? AdjudicationDateTo { get; set; }
        /// <summary>
        /// Перелік номерів судових документів
        /// </summary>
        public List<string> Numbers { get; set; }

    }
    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetDecisionsByFilterResponseModel
    {
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Чи успішний запит (maxLength:128)
        /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Скрол для отримання наступних документів (100)
        /// </summary>
        public string ScrollToken { get; set; }                                 // 
        /// <summary>
        /// Загальна кількясть документів за запитом
        /// </summary>
        public long DecisionsCount { get; set; }                                // 
        /// <summary>
        /// Перелік судових документів (Модель відповіді ідентична Методу № 26. Рекізити судового документа [POST] /api/1.0/courtdecision/getdecisionbyid https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetDecisionByIdClass.cs#L110)
        /// </summary>
        public List<CourtDecisionElasticModel> Data { get; set; }               // 
    }
}
