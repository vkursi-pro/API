using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetSmsRrpDiffrenceCheckByDateRangeClass
    {
        /*

       140. Отримати дельту змін СМС РРП моніторингу за період
       [POST] /api/1.0/estate/smsrrpselectisrealtyexists

       curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/smsrrpdiffrencecheckbydaterange' \
       --header 'ContentType: application/json' \
       --header 'Authorization: Bearer eyJhbGciOiJIU...' \
       --header 'Content-Type: application/json' \
       --data-raw '{ "StartDate": "2024-01-15T00:00:00","EndDate": "2024-01-20T00:00:00"}'

       */

        public static SmsRrpDiffrenceCheckDateRangeModelAnswer SmsRrpSelectIsRealtyExists(string token, DateTime? StartDate, DateTime? EndDate, List<Guid>? TaskIdList = null)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                SmsRrpDiffrenceCheckDateRangeModel SRSIRERequestBody = new SmsRrpDiffrenceCheckDateRangeModel
                {
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TaskIdList = TaskIdList // необов'язковий параметр
                };

                string body = JsonConvert.SerializeObject(SRSIRERequestBody);

                // Example Body: { "StartDate": "2024-01-15T00:00:00","EndDate": "2024-01-20T00:00:00"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/smsrrpdiffrencecheckbydaterange");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            SmsRrpDiffrenceCheckDateRangeModelAnswer SRSIREResponseRow = new SmsRrpDiffrenceCheckDateRangeModelAnswer();

            SRSIREResponseRow = JsonConvert.DeserializeObject<SmsRrpDiffrenceCheckDateRangeModelAnswer>(responseString);

            return SRSIREResponseRow;
        }
        /// <summary>
        /// Запит
        /// </summary>
        public class SmsRrpDiffrenceCheckDateRangeModel
        {
            /// <summary>
            /// Фільтр по початковій даті створення завдання
            /// </summary>
            public DateTime? StartDate { get; set; }
            /// <summary>
            /// Фільтр по кінцевій даті створення завдання
            /// </summary>
            public DateTime? EndDate { get; set; }
            /// <summary>
            /// Фільтр по ід завдання
            /// </summary>
            public List<Guid>? TaskIdList { get; set; }
        }
        /// <summary>
        /// Відповідь
        /// </summary>
        public class SmsRrpDiffrenceCheckDateRangeModelAnswer
        { /// <summary>
          /// Статус відповіді по API
          /// </summary>
            public string? Status { get; set; }
            /// <summary>
            /// Id відповіді по API (відповідно до enum ApiResponseStateEnum)
            /// </summary>
            public int? StatusId { get; set; }
            /// <summary>
            /// Чи успішний запит
            /// </summary>
            public bool? IsSuccess { get; set; }
            /// <summary>
            /// Статус відповіді по API
            /// </summary>
            public Guid? LogId { get; set; }
            /// <summary>
            /// Розширене повідомлення про помилку
            /// </summary>
            public string? ErrorMessage { get; set; }
            /// <summary>
            /// Загальна дата актуальності
            /// </summary>
            public DateTime? ActualDate { get; set; }
            /// <summary>
            /// Дані
            /// </summary>
            public List<SmsRrpDiffrenceCheckDateRangeModelAnswerData> Data { get; set; }
        }

        public class SmsRrpDiffrenceCheckDateRangeModelAnswerData
        {
            /// <summary>
            /// Ід
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// Тип завдання
            /// </summary>
            public int Type { get; set; }
            /// <summary>
            /// Секції які моніторяться
            /// </summary>
            public List<string> Sections { get; set; }
            /// <summary>
            /// Кадастровий номер
            /// </summary>
            public string KadastrNumber { get; set; }
            /// <summary>
            /// Онм
            /// </summary>
            public long? Onm { get; set; }
            /// <summary>
            /// Дата створення завдання
            /// </summary>
            public DateTime CreateDate { get; set; }
            /// <summary>
            /// Назва завдання
            /// </summary>
            public string TaskName { get; set; }
            /// <summary>
            /// Ид звіту
            /// </summary>
            public Guid? TaskId { get; set; }
            /// <summary>
            /// Помилка
            /// </summary>
            public bool? Error { get; set; }
            /// <summary>
            /// Ид из таблицы RealEstateMonitoringChange
            /// </summary>
            public Guid? RealEstateMonitoringChangeId { get; set; }
        }
    }
}
