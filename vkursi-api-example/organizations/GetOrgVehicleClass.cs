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
        public List<VehicleOrgApiAnswerModelDataVehicle> Vehicles { get; set; } // 
    }
    /// <summary>
    /// Перелік ТЗ (транспортних засобів)
    /// </summary>
    public class VehicleOrgApiAnswerModelDataVehicle                            // 
    {/// <summary>
     /// Марка ТЗ (maxLength:128)
     /// </summary>
        public string Brand { get; set; }                                       // 
        /// <summary>
        /// Модель (maxLength:128)
        /// </summary>
        public string Name { get; set; }                                        // 
        /// <summary>
        /// Год випуска
        /// </summary>
        public int? ReleaseYear { get; set; }                                   // 
        /// <summary>
        /// Об'єм двигуна 
        /// </summary>
        public int? Capacity { get; set; }                                      // 
        /// <summary>
        /// Дата реєстраційної дії
        /// </summary>
        public DateTime? RegDate { get; set; }                                  // 
        /// <summary>
        /// Тип реєстраційної дії (maxLength:512)
        /// </summary>
        public string OperationName { get; set; }                               // 
    }
}
