using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{

    public class GetDecisionsByEdrpousClass
    {
        /// <summary>
        /// 93. Основні реквізити судових докуметів за кодом ЕДРПОУ
        /// [POST] /api/1.0/courtDecision/getDecisionsByEdrpous
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        public static GetDecisionsByEdrpousResponseModel GetDecisionsByEdrpous(ref string token, List<string> codeLst)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDecisionsByEdrpousRequestBodyModel GRFABRequestBody = new GetDecisionsByEdrpousRequestBodyModel
                {
                    EdrpouList = codeLst                    // Код ЄДРПОУ 
                };

                string body = JsonConvert.SerializeObject(GRFABRequestBody);   // Example body: {"EdrpouList":["00222166"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtDecision/getDecisionsByEdrpous");
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
                    Console.WriteLine("За вказаным кодом організації не знайдено");
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

            GetDecisionsByEdrpousResponseModel organizationAnalytic = new GetDecisionsByEdrpousResponseModel();

            organizationAnalytic = JsonConvert.DeserializeObject<GetDecisionsByEdrpousResponseModel>(responseString);

            return organizationAnalytic;
        }

    }

    public class GetDecisionsByEdrpousRequestBodyModel
    {
        /// <summary>
        /// 50 шт обмеження
        /// </summary>
        public List<string> EdrpouList { get; set; }
    }

    public class GetDecisionsByEdrpousResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public List<DecisionsByEdrpousResponseModel> Data { get; set; }
    }

    public class DecisionsByEdrpousResponseModel
    {
        public string Edrpou { get; set; }
        public List<GetDecisionsByEdrpousData> Data { get; set; }
    }

    public class GetDecisionsByEdrpousData
    {
        /// <summary>
        /// ЕДРПОУ
        /// </summary>
        public string Edrpou { get; set; }
        /// <summary>
        /// № справи
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Назва
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Інстанція
        /// </summary>
        public string Instance { get; set; }
        /// <summary>
        /// Форма рішень
        /// </summary>
        public string JudgementForm { get; set; }
        /// <summary>
        /// Форма судочинства
        /// </summary>
        public string JusticeKind { get; set; }
        /// <summary>
        /// Суд
        /// </summary>
        public string CourtName { get; set; }
        /// <summary>
        /// Суддя
        /// </summary>
        public string Judge { get; set; }
        /// <summary>
        /// Всього документів
        /// </summary>
        public int? TotalDocumentsCount { get; set; }
        /// <summary>
        /// Перша
        /// </summary>
        public int? TotalDocumentsWithFirstInstanceCount { get; set; }
        /// <summary>
        /// Апеляційна
        /// </summary>
        public int? TotalDocumentsWithContainAppealCount { get; set; }
        /// <summary>
        /// Касаційна
        /// </summary>
        public int? TotalDocumentsWithContainCassationCount { get; set; }
        /// <summary>
        /// Призначено до розгляду
        /// </summary>
        public int? TotalDocumentsWithIntendedDocumentCount { get; set; }
        /// <summary>
        /// Тривалість
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Позивач(і)
        /// </summary>
        public string Plaintiff { get; set; }
        /// <summary>
        /// Відповідач(і)
        /// </summary>
        public string Defendant { get; set; }
        /// <summary>
        /// Інша сторона
        /// </summary>
        public string AnotherSide { get; set; }
        /// <summary>
        /// Суть справи
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Знайдено за
        /// </summary>
        public int? FindParam { get; set; }
        /// <summary>
        /// Дата додавання документу в сервіс
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Id судового документу
        /// </summary>
        public int Id { get; set; }
    }
}
