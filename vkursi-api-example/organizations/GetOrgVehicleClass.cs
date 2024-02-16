using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgVehicleClass
    {
        /*
         
        32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
        [POST] /api/1.0/organizations/getorgvehicle

        curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgvehicle' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1N...' \
        --header 'Content-Type: application/json' \
        --data '{"Edrpou":["00728629"]}'

        */

        public static GetOrgVehicleResponseModel GetOrgVehicle(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgVehicleRequestBodyModel GOVRequestBody = new GetOrgVehicleRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        code                                                    // Код ЄДРПОУ або ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOVRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgvehicle");
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
                    Console.WriteLine("За вказаним кодом організації не знайдено");
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

            GetOrgVehicleResponseModel GOVResponse = new GetOrgVehicleResponseModel();

            GOVResponse = JsonConvert.DeserializeObject<GetOrgVehicleResponseModel>(responseString);

            return GOVResponse;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetOrgVehicleRequestBodyModel                                  // 
    {/// <summary>
     /// Перелік ЄДРПОУ / ІПН (обмеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                // 
    }
    /// <summary>
    /// Модель відповіді GetOrgVehicle
    /// </summary>
    public class GetOrgVehicleResponseModel                                     // 
    {/// <summary>
     /// Статус відповіді по API (maxLength:128)
     /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSucces { get; set; }                                      // 
        /// <summary>
        /// Дані
        /// </summary>
        public VehicleOrgApiAnswerModelData Data { get; set; }                  // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class VehicleOrgApiAnswerModelData                                   // 
    {/// <summary>
     /// ЄДРПОУ / ІПН (maxLength:12)
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// Перелік ТЗ (транспортних засобів)
        /// </summary>
        public List<VehicleOrgApiAnswerModelDataVehicle> Vehicles { get; set; }
    }
    /// <summary>
    /// Перелік ТЗ (транспортних засобів)
    /// </summary>
    public class VehicleOrgApiAnswerModelDataVehicle
    {
        /// <summary>
        /// Назва марки транспортного засобу (maxLength:128)
        /// </summary>
        /// <example>BMW</example>
        public string Brand { get; set; }
        /// <summary>
        /// Назва моделі транспортного засобу (maxLength:128)
        /// </summary>
        /// <example>X5</example>
        public string Name { get; set; }
        /// <summary>
        /// Рік виготовлення
        /// </summary>
        public int? ReleaseYear { get; set; }
        /// <summary>
        /// Об`єм двигуна 
        /// </summary>
        /// <example>2993</example>
        public int? Capacity { get; set; }
        /// <summary>
        /// Дата проведення останньої реєстрації за власником
        /// </summary>
        /// <example>2023-06-06T00:00:00</example>
        public DateTime? RegDate { get; set; }
        /// <summary>
        /// Тип реєстраційної дії (maxLength:512)
        /// </summary>
        /// <example>ПЕРЕРЕЄСТРАЦІЯ ПРИ ЗАМІНІ НОМЕРНОГО ЗНАКУ</example>
        public string OperationName { get; set; }
        /// <summary>
        /// Колір транспортного засобу
        /// </summary>
        /// <example>БІЛИЙ</example>
        public string Color { get; set; }
        /// <summary>
        /// Тип транспортного засобу
        /// </summary>
        /// <example>ЛЕГКОВИЙ</example>
        public string Type { get; set; }
        /// <summary>
        /// Тип кузову траспортного засобу
        /// </summary>
        /// <example>УНІВЕРСАЛ</example>
        public string Body { get; set; }
        /// <summary>
        /// Тип палива транспортного засобу 
        /// </summary>
        /// <example>ДИЗЕЛЬНЕ ПАЛИВО</example>
        public string Fuel { get; set; }
        /// <summary>
        /// Призначення
        /// </summary>
        /// <example>ЗАГАЛЬНИЙ</example>
        public string Purpose { get; set; }
        /// <summary>
        /// Власна вага транспортного засобу 
        /// </summary>
        /// <example>2185</example>
        public double? OwnWeight { get; set; }
        /// <summary>
        /// Загальна вага транспортного засобу
        /// </summary>
        /// <example>2860</example>
        public double? TotalWeight { get; set; }
        /// <summary>
        /// Код операції
        /// </summary>
        /// <example>540</example>
        public int? OperCode { get; set; }
    }
}
