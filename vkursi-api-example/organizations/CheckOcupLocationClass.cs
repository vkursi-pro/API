using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class CheckOcupLocationClass
    {
        /*
        
        Метод:
            151. Чи знаходиться підприємство на окупованій території
            [POST] /api/1.0/organizations/CheckOcupLocation

        cURL запиту:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckOcupLocation' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
            --header 'ContentType: application/json' \
            --header 'Content-Type: application/json' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckOcupLocationResponse.json

        */
        public static CheckOcupLocationResponseModel CheckOcupLocation(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CheckOcupLocationRequestBodyModel COLRBodyModel = new CheckOcupLocationRequestBodyModel
                {
                    Codes = new List<string> { code }                                   // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel);           // Example body: {"Codes":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckOcupLocation");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            CheckOcupLocationResponseModel GNWEResponse = new CheckOcupLocationResponseModel();

            GNWEResponse = JsonConvert.DeserializeObject<CheckOcupLocationResponseModel>(responseString);

            return GNWEResponse;
        }
    }


    /// <summary>
    /// Модель запиту
    /// </summary>
    public class CheckOcupLocationRequestBodyModel
    {
        /// <summary>
        /// Перелік кодів ЄДРПОУ або ІПН
        /// </summary>
        public List<string> Codes { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class CheckOcupLocationResponseModel
    {
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<CheckOcupLocationModelResponseItem> Data { get; set; }
    }

    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class CheckOcupLocationModelResponseItem
    {
        /// <summary>
        /// Статус(Інформацію не знайдено / Знаходиться на окупованій території / Знаходиться не на окупованій території) 
        /// </summary>
        public string OcupStatus { get; set; }

        /// <summary>
        /// 1 - Інформацію не знайдено / 2 - Знаходиться на окупованій території / 3 - Знаходиться не на окупованій території
        /// </summary>
        public int OcupStatusCode { get; set; }
    }
}
