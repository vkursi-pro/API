using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{
    public class GetCourtAssigmentClass
    {
        /// <summary>
        /// 89. Список справм призначених до розгляду
        /// </summary>
        /// <param name="code">Код ЄДРПОУ</param>
        /// <param name="token">Токен</param>
        /// <returns></returns>

        /*

        cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/CourtDecision/getCourtAssigment' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX...' \
                --header 'Content-Type: application/json' \
                --data-raw '{"CompanyEdrpou":"42721869","Size":100}'
 
        */
        public static GetCourtAssigmentResponseModel GetCourtAssigment(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetCourtAssigmentRequestBodyModel GCARBodyModel = new GetCourtAssigmentRequestBodyModel
                {
                    CompanyEdrpou = code                     // Код ЄДРПОУ / ІПН
                };

                string body = JsonConvert.SerializeObject(GCARBodyModel);   // Example body: 

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtDecision/getCourtAssigment");
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

            GetCourtAssigmentResponseModel GCAResponse = new GetCourtAssigmentResponseModel();

            GCAResponse = JsonConvert.DeserializeObject<GetCourtAssigmentResponseModel>(responseString);

            return GCAResponse;
        }
    }

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetCourtAssigmentRequestBodyModel
    {
        /// <summary>
        /// ПІБ якщо пошку по ФО
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// ЄДРПОУ якщо пошук по компанії
        /// </summary>
        public string CompanyEdrpou { get; set; }
        /// <summary>
        /// К-ть документів в відповіді за один запит
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Тип сторони
        /// </summary>
        public int? TypeSide { get; set; }
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }
        /// <summary>
        /// Дата від
        /// </summary>
        public DateTime DateStart { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetCourtAssigmentResponseModel
    {
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Чи успішний запит (maxLength:128)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Дані
        /// </summary>
        public GetCourtAssigmentResponseData Data { get; set; }
    }

    /// <summary>
    /// Дані
    /// </summary>
    public class GetCourtAssigmentResponseData
    {
        /// <summary>
        /// Максимальна дата справи в списку (для отримання наступного списку справ) 
        /// </summary>
        public DateTime? MaxDate { get; set; }
        /// <summary>
        /// Перелік справ
        /// </summary>
        public List<ApiRozgliadSpravData> DateList { get; set; }
    }

    /// <summary>
    /// Перелік справ
    /// </summary>
    public class ApiRozgliadSpravData
    {
        /// <summary>
        /// Назва суду
        /// </summary>
        public string CourtName { get; set; }
        /// <summary>
        /// Єдиний унікальний номер справи
        /// </summary>
        public string CaseNumber { get; set; }
        /// <summary>
        /// № провадження
        /// </summary>
        public string CaseProc { get; set; }
        /// <summary>
        /// Дата рішення
        /// </summary>
        public DateTime? RegistrationDate { get; set; }
        /// <summary>
        /// Головуючий суддя
        /// </summary>
        public string Judge { get; set; }
        /// <summary>
        /// Суддя-доповідач
        /// </summary>
        public string Judges { get; set; }
        /// <summary>
        /// Учасники
        /// </summary>
        public string Participants { get; set; }
        /// <summary>
        /// Дата наступної події
        /// </summary>
        public DateTime? StageDate { get; set; }
        /// <summary>
        /// Стадія розгляду
        /// </summary>
        public string StageName { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        public string CauseResult { get; set; }
        /// <summary>
        /// Тип заяви
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Предмет позову
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ид документа на Vsudi пример https://vkursi.pro/vsudi/decision/90283013
        /// </summary>
        public string DecisionId { get; set; }
        /// <summary>
        /// Тип сторони к справі: null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }

    }
}
