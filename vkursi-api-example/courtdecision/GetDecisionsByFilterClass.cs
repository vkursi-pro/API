using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionsByFilterClass
    {

        /*
         
        67. Запит на отримання повних реквізитів та контенту судових документів організації за критеріями
        [POST] /api/1.0/courtdecision/getdecisionsbyfilter

        */


        public static GetDecisionByIdResponseModel GetDecisionsByFilter(string edrpou, int? skip, int? typeSide, int? justiceKindId, List<string> npas, string token)
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
                    TypeSide = typeSide,                                        // Тип сторони в судомому документі
                    JusticeKindId = justiceKindId,                              // Форма судочинства
                    Npas = npas                                                 // Фільтр по статтям до НПА
                };

                string body = JsonConvert.SerializeObject(GDBFRequestBody);                // Example body: {"Edrpou":"14360570","Skip":0,"TypeSide":null,"JusticeKindId":null,"Npas":["F545D851-6015-455D-BFE7-01201B629774"]}

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

    public class GetDecisionsByFilterRequestBodyModel                           // Модель запиту
    {
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ
        public int? TypeSide { get; set; }                                      // null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        public int? JusticeKindId { get; set; }                                 // null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        public List<string> Npas { get; set; }                                  // Id статті НПА (можна отримати в розробника)
        public string ScrollToken { get; set; }                                 // Скрол для отримання наступних документів
        public int? JudgmentFormId { get; set; }                                // 1  Вирок, 2 Постанова, 3 Рішення, 4 "Судовий наказ", 5 Ухвала, 6 "Окрема ухвала", 10 "Окрема думка"
        public DateTime? AdjudicationDateFrom { get; set; }                     // Дата рішення від
        public DateTime? AdjudicationDateTo { get; set; }                       // Дата рішення до

    }


    // Модель відповіди ідентична Методу № 26. Рекізити судового документа [POST] /api/1.0/courtdecision/getdecisionbyid

    // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetDecisionByIdClass.cs#L110

}
