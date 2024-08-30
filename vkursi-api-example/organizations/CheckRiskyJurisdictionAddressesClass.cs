using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class CheckRiskyJurisdictionAddressesClass
    {
        /// <summary>
        /// 187. Адреса реєстрації засновників або кінцевих бенефіціарних власників у ризикових юрисдикціях
        /// [POST] api/1.0/organizations/CheckRiskyJurisdictionAddresses
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckRiskyJurisdictionAddresses' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckRiskyJurisdictionAddressesResponse.json

        */
        public CheckCorruptionSbuRegistryModelResponse GetRiskyJurisdictionAddresses(ref string token, string code)
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

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckRiskyJurisdictionAddresses");
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

            CheckCorruptionSbuRegistryModelResponse GCEEResponseRow = new CheckCorruptionSbuRegistryModelResponse();

            GCEEResponseRow = JsonConvert.DeserializeObject<CheckCorruptionSbuRegistryModelResponse>(responseString);

            return GCEEResponseRow;
        }
    }
    /// <summary>
    /// Вхідні параметри запиту (перелік кодів)
    /// </summary>
    public class CheckRiskyJurisdictionAddressesModel
    {
        public List<string> Codes { get; set; }
    }
    /// <summary>
     /// Відповідь
     /// </summary>
    public class CheckRiskyJurisdictionAddressesModelResponse
    {
        /// <summary>
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
        public List<CheckRiskyJurisdictionAddressesModelResponseData>? Data { get; set; }
    }
    public class CheckRiskyJurisdictionAddressesModelResponseData
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Інформація про адреси КБВ та щзасновників да їх адреси. 
        /// </summary>
        public List<CheckRiskyJurisdictionAddressesModelResponseDataItem> Data { get; set; }
    }

    public class CheckRiskyJurisdictionAddressesModelResponseDataItem
    {
        /// <summary>
        /// Ім'я 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Чи відносится до ризикових (офшорних зон)
        /// </summary>
        public bool IsRisky { get; set; }
        /// <summary>
        /// Перелік країн
        /// </summary>
        public HashSet<string> AllCountryRegistrations { get; set; }
        /// <summary>
        /// Всі адреси реєстрації суб'єкта
        /// </summary>
        public HashSet<string> AllAddresses { get; set; }
        /// <summary>
        /// Ризикові країни
        /// </summary>
        public HashSet<string> RiskyCountries { get; set; }
    }
    
}
