using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.person;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.MVS
{
    public class CheckMVSWantedRegistryClass
    {
        /// <summary>
        /// 185. Службові особи підприємства, КБВ, які перебувають в розшуку за МВС: які переховуються від органів влади або безвісті зниклі громадяни
        /// [POST] api/1.0/organizations/CheckSBUWantedRegistry
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckMVSWantedRegistry' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckMVSWantedRegistryResponse.json

        */
        public CheckMVSWantedRegistryModelResponse GetMVSWantedRegistryClass(ref string token, string code)
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

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckMVSWantedRegistry");
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

            CheckMVSWantedRegistryModelResponse GCEEResponseRow = new CheckMVSWantedRegistryModelResponse();

            GCEEResponseRow = JsonConvert.DeserializeObject<CheckMVSWantedRegistryModelResponse>(responseString);

            return GCEEResponseRow;
        }
    }
    /// <summary>
    /// Вхідні параметри запиту (перелік кодів)
    /// </summary>
    public class CheckMVSWantedRegistryModel
    {
        public List<string> Codes { get; set; }
    }

    /// <summary>
    /// Відповідь
    /// </summary>
    public class CheckMVSWantedRegistryModelResponse
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
        public List<CheckMVSWantedRegistryModelResponseData>? Data { get; set; }
    }
    public class CheckMVSWantedRegistryModelResponseData
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дані про осіб
        /// </summary>
        public List<PersonsHidingViewModel> HidingData { get; set; }
    }
    /// <summary>
    /// Відомості про осіб що переховуються від органів влади
    /// </summary>
    public class PersonsHidingViewModel                                                 // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Дата народження
        /// </summary>
        public DateTime DateBirth { get; set; }                                         // 
        /// <summary>
        /// Орган
        /// </summary>
        public string Ovd { get; set; }                                                 // 
        /// <summary>
        /// Категорія
        /// </summary>
        public string Category { get; set; }                                            // 
        /// <summary>
        /// Дата зникнення
        /// </summary>
        public DateTime? DateLost { get; set; }                                         // 
        /// <summary>
        /// Стаття
        /// </summary>
        public string Paragraph { get; set; }                                           // 
        /// <summary>
        /// Обмеження
        /// </summary>
        public string Restraint { get; set; }                                           // 
        /// <summary>
        /// Контакта інформація відповідного органу
        /// </summary>
        public string ContactInfo { get; set; }                                         // 
        /// <summary>
        /// Посаління на Фото
        /// </summary>
        public string ImageUrl { get; set; }                                            // 
    }
}
