using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.PEP
{
    public class CheckPepRegistryClass
    {
        /// <summary>
        /// 186. Пошук серезд службових осіб підприємства та КБВ, осіб які відносяться до публічних осіб, або пов'язаних з публічною особою
        /// [POST] api/1.0/organizations/CheckPepRegistry
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckPepRegistry' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckPepRegistryResponse.json

        */
        public CheckPepRegistryModelResponse GetPepRegistryClass(ref string token, string code)
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

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckPepRegistry");
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

            CheckPepRegistryModelResponse GCEEResponseRow = new CheckPepRegistryModelResponse();

            GCEEResponseRow = JsonConvert.DeserializeObject<CheckPepRegistryModelResponse>(responseString);

            return GCEEResponseRow;
        }
    }
    
    /// <summary>
    /// Вхідні параметри запиту (перелік кодів)
    /// </summary>
    public class CheckPepRegistryModel
    {
        public List<string> Codes { get; set; }
    }
    /// <summary>
    /// Відповідь
    /// </summary>
    public class CheckPepRegistryModelResponse
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
        public List<CheckPepRegistryModelResponseData>? Data { get; set; }
    }
    public class CheckPepRegistryModelResponseData
    { /// <summary>
      /// Код ЄДРПОУ
      /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дані про осіб
        /// </summary>
        public List<CheckPepRegistryModelResponseDataItem> Data { get; set; }
    }

    public class CheckPepRegistryModelResponseDataItem
    {
        /// <summary>
        /// ПІБ суб'єкта публічної/не пусблічної особи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Пов'язана особа (буде заповнено у разі якщо це пов'язана особа з декларантом
        /// </summary>
        public string RelatedName { get; set; }
        /// <summary>
        /// Тип зв'язку:
        /// </summary>
        public string ConnectionType { get; set; }

        /// <summary>
        /// Чи піблічна особа, або пов'язана з публічною 
        /// </summary>
        public bool IsPep { get; set; }

        /// <summary>
        /// Чи піблічна особа, або пов'язана з публічною 
        /// </summary>
        public bool IsPepAnalized { get; set; }
    }
}
