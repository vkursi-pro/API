using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.Court
{
    public class GetOrganizationCourtStatisticsClass
    {
        /// <summary>
        /// 188. Судова статистика по організації
        /// [POST] api/1.0/organizations/GetOrganizationCourtStatistics
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrganizationCourtStatistics' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetOrganizationCourtStatisticsResponse.json

        */
        public GetOrganizationCourtStatisticsModelResponse GetOrganizationCourtStatistics(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CheckCorruptionSbuRegistryModel COLRBodyModel = new CheckCorruptionSbuRegistryModel
                {
                    Codes = new List<string> { code }     // Код ЄДРПОУ
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel,                // Example body: {"Codes":["00131305"]}
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrganizationCourtStatistics");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");
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
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, total objects"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, try again later"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
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

            GetOrganizationCourtStatisticsModelResponse GCEEResponseRow = new GetOrganizationCourtStatisticsModelResponse();

            GCEEResponseRow = JsonConvert.DeserializeObject<GetOrganizationCourtStatisticsModelResponse>(responseString);

            return GCEEResponseRow;
        }
    }




   

    /// <summary>
    /// Вхідні параметри запиту (перелік кодів)
    /// </summary>
    public class GetOrganizationCourtStatisticsModel
    {
        public List<string> Codes { get; set; }
    }
    /// <summary>
    /// Відповідь
    /// </summary>
    public class GetOrganizationCourtStatisticsModelResponse
    {/// <summary>
     /// Статус запиту
     /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи вдалий запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані
        /// </summary>
        public List<GetOrganizationCourtStatisticsModelResponseData>? Data { get; set; }
    }
    public class GetOrganizationCourtStatisticsModelResponseData
    { /// <summary>
      /// Код ЄДРПОУ
      /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дані судові справи
        /// </summary>
        public CourtDecisionAggregationModel Data { get; set; }
    }

    public class CourtDecisionAggregationModel
    {
        /// <summary>
        /// Загальна кількість справ.
        /// </summary>
        public long? TotalCount { get; set; }

        /// <summary>
        /// Кількість справ, де особа є позивачем.
        /// </summary>
        public long? PlaintiffCount { get; set; }

        /// <summary>
        /// Кількість справ, де особа є відповідачем.
        /// </summary>
        public long? DefendantCount { get; set; }

        /// <summary>
        /// Кількість справ, де є інша сторона (не позивач і не відповідач).
        /// </summary>
        public long? OtherSideCount { get; set; }

        /// <summary>
        /// Кількість справ, що були програні.
        /// </summary>
        public long? LoserCount { get; set; }

        /// <summary>
        /// Кількість справ, що були виграні.
        /// </summary>
        public long? WinCount { get; set; }

        /// <summary>
        /// Кількість справ, призначених до розгляду.
        /// </summary>
        public long? IntendedCount { get; set; }

        /// <summary>
        /// Кількість справ, що перебувають на розгляді.
        /// </summary>
        public long? CaseCount { get; set; }

        /// <summary>
        /// Кількість справ, що знаходяться в процесі.
        /// </summary>
        public long? InProcess { get; set; }

        /// <summary>
        /// Загальна кількість документів, пов'язаних із справами.
        /// </summary>
        public long? TotalDocuments { get; set; }

        /// <summary>
        /// Список видів судочинства (наприклад, кримінальні, цивільні, господарські тощо).
        /// </summary>
        public List<JusticeKinds> ListJusticeKindses { get; set; }
    }

    public class JusticeKinds
    {
        /// <summary>
        /// Унікальний ідентифікатор виду судочинства.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Назва виду судочинства.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Кількість справ для даного виду судочинства.
        /// </summary>
        public long? Count { get; set; }
    }

}
