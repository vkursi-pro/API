using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetRegistrationActionsHistoryClass
    {
        /// <summary>
        /// 155. Aрі історії реєстраційних дій
        /// [POST] /api/1.0/organizations/GetRegistrationActionsHistory
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL запиту:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationActionsHistory' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
            --header 'ContentType: application/json' \
            --header 'Content-Type: application/json' \
            --data '{"code":"00131305"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRegistrationActionsHistoryResponse.json
        */
        public static GetRegistrationActionsHistoryResponseModel CompanyDpsInfo(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetRegistrationActionsHistoryRequestBodyModel COLRBodyModel = new GetRegistrationActionsHistoryRequestBodyModel
                {
                    Code = code                               // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel);           // Example body: {"Codes":["00131305"]}

                RestClient client = new RestClient(
                    "https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationActionsHistory");
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

            GetRegistrationActionsHistoryResponseModel GNWEResponse = 
                JsonConvert.DeserializeObject<GetRegistrationActionsHistoryResponseModel>(responseString);

            return GNWEResponse;
        }
    }

    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetRegistrationActionsHistoryRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дата проведення змін (ВІД)
        /// </summary>
        [JsonProperty("dateStart")]
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Дата проведення змін (ДО)
        /// </summary>
        [JsonProperty("dateEnd")]
        public DateTime? DateEnd { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetRegistrationActionsHistoryResponseModel
    {
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<GetRegistrationActionsHistoryData> Data { get; set; }
    }

    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class GetRegistrationActionsHistoryData
    {
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// Дата проведення змін
        /// </summary>
        [JsonProperty("dateOfChange")]
        public DateTime DateOfChange { get; set; }
        /// <summary>
        /// Опис змін
        /// </summary>
        [JsonProperty("changes")]
        public EdrChangesInfoModelChange[] Changes { get; set; }
        /// <summary>
        /// Попередній об'єкт (передається тільки той розділ (об'єкт/поле) в якому відбулись зміни)
        /// Опис повної моделі: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        [JsonProperty("oldNaisValue")]
        public OrganizationaisElasticModel OldNaisValue { get; set; }
        /// <summary>
        /// Новий об'єкт (передається тільки той розділ (об'єкт/поле) в якому відбулись зміни)
        /// Опис повної моделі: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        [JsonProperty("newNaisValue")]
        public OrganizationaisElasticModel NewNaisValue { get; set; }
    }

    /// <summary>
    /// Опис змін
    /// </summary>
    public class EdrChangesInfoModelChange
    {
        /// <summary>
        /// Id категорії зміни
        /// </summary>
        [JsonProperty("changeTypeId")]
        public int ChangeTypeId { get; set; }
        /// <summary>
        /// Назва категорії зміни
        /// </summary>
        [JsonProperty("changeType")]
        public string ChangeType { get; set; }
        /// <summary>
        /// Перелік змін відповідно до категорії
        /// </summary>
        [JsonProperty("changes")]
        public ChangeChange[] Changes { get; set; }
    }

    /// <summary>
    /// Перелік змін відповідно до категорії
    /// </summary>
    public class ChangeChange
    {
        /// <summary>
        /// Id зміни
        /// </summary>
        [JsonProperty("changeTypeId")]
        public int ChangeTypeId { get; set; }
        /// <summary>
        /// Назва зміни
        /// </summary>
        [JsonProperty("changeType")]
        public string ChangeType { get; set; }
        /// <summary>
        /// Попереднє значення
        /// </summary>
        [JsonProperty("oldValue")]
        public string OldValue { get; set; }
        /// <summary>
        /// Нове значення
        /// </summary>
        [JsonProperty("newValue")]
        public string NewValue { get; set; }
    }
}
