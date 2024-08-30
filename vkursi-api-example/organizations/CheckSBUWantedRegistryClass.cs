using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.person;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.SBU
{
    public class CheckSBUWantedRegistryClass
    {
        /// <summary>
        /// 184. Службові особи підприємства, КБВ, які перебувають в розшуку за СБУ
        /// [POST] api/1.0/organizations/CheckSBUWantedRegistry
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckSBUWantedRegistry' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckSBUWantedRegistryResponse.json

        */
        public CheckCorruptionSbuRegistryModelResponse GetSBUWantedRegistryClass (ref string token, string code)
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

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckSBUWantedRegistry");
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
    public class CheckCorruptionSbuRegistryModel
    {
        public List<string> Codes { get; set; }
    }

    /// <summary>
    /// Відповідь
    /// </summary>
    public class CheckCorruptionSbuRegistryModelResponse
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
        public List<CheckCorruptionSbuRegistryModelResponseData>? Data { get; set; }
    }

    public class CheckCorruptionSbuRegistryModelResponseData
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дані про осіб які перебувають в розшуку за СБУ 
        /// </summary>
        public List<PersonLicense>? SBUWantedData { get; set; }
    }
    public class PersonLicense
    {
        /// <summary>
        /// Назва категорії розшуку
        /// </summary>
        public string LicenseName { get; set; }
        /// <summary>
        /// Порядковий номер (у разі наявності)
        /// </summary>
        public string LicenseNumber { get; set; }
        /// <summary>
        /// Дата закінчення розшуку
        /// </summary>
        public DateTime? DateEnd { get; set; }
        /// <summary>
        /// Чи актуально (знятий з розшуку чи ні)
        /// </summary>
        public bool Actual { get; set; }
        /// <summary>
        /// ID категорії розушку по вказаній особі
        /// </summary>
        public Guid LicenseId { get; set; }
        /// <summary>
        /// Ім'я за яким знайдено співпадіння
        /// </summary>
        public string SearchedString { get; set; }
        /// <summary>
        /// За яки критерієм пошуку було знайдено - якщо було знайдено за РНОКПП (ІПН) 
        /// </summary>
        public bool? SearchByIpn { get; set; }
        /// <summary>
        /// За яки критерієм пошуку було знайдено - якщо було знайдено за датою народження
        /// </summary>
        public bool? SearchByBirthDay { get; set; }
        /// <summary>
        /// ПІБ суб'єкта розушку
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// Додаткова інформація
        /// </summary>
        public object Info { get; set; }
    }




}
