using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.enforcement;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public static class GetOccupiedTerritoriesModel
    {
        /*

       cURL:
           curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/enforcement/GetOccupiedTerritories' \
           --header 'ContentType: application/json' \
           --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
           --data '{ "codes": ["39214302"] }' 

       */
        /// <summary>
        /// 167. Перевірка реєстрації ЮО та ФОП, а також  їх власників, учасників, бенефіціарів на територіях, на яких ведуться (велися) бойові дії або тимчасово окупованих російською федерацією"
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        public static GetTerritoryInfoModelResponse GetOccupiedTerritories (ref string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {

                string body = "{ \"codes\": [\"39214302\"] }";// { \"codes\": [\"39214302\"] }         


                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOccupiedTerritories");
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

            GetTerritoryInfoModelResponse GCEEResponseRow = new GetTerritoryInfoModelResponse();

            GCEEResponseRow = JsonConvert.DeserializeObject<GetTerritoryInfoModelResponse>(responseString);

            return GCEEResponseRow;
        }
    }

    public class GetTerritoryInfoModel
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public List<string> Codes { get; set; }
    }

    public class GetTerritoryInfoModelResponse
    {
        /// <summary>
        /// Статус запиту
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи був вдалий запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Основні дані
        /// </summary>
        public List<GetTerritoryInfoModelResponseData> Data { get; set; }
        /// <summary>
        /// Актуальна дата реєстру
        /// </summary>
        public DateTime? ActualRegisterDate { get; set; }
    }

    public class GetTerritoryInfoModelResponseData
    {
        /// <summary>
        /// Код бенефіціара/засновника
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// Код ЄДРПОУ компанії
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Наіменування бенефіціара/засновника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип бенефіціара/засновника
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Адреса бенефіціара/засновника
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Список інформації про території
        /// </summary>
        public List<GetTerritoryInfoModelResponseDataTerretoryInfo> Data { get; set; }
    }

    public class GetTerritoryInfoModelResponseDataTerretoryInfo
    {
        /// <summary>
        /// Код території на якій ведуться/велися бойові дії
        /// </summary>
        public int CodeClientСommunity { get; set; }

        /// <summary>
        /// Тип території
        /// </summary>
        public string NameClientСommunity { get; set; }

        /// <summary>
        /// Початок бойових дій
        /// </summary>
        public DateTime StartClientDate { get; set; }

        /// <summary>
        /// Закінчення бойових дій
        /// </summary>
        public DateTime? EndClientDate { get; set; }
    }
}
