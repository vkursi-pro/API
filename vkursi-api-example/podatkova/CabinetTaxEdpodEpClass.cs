using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.podatkova
{
    public class CabinetTaxEdpodEpClass
    {

        /*
         
        74. Стан ФОПа та відомості про ЄП
        [POST] /api/1.0/podatkova/cabinettaxedpodep

        */

        public static CabinetTaxEdpodEpResponseModel CabinetTaxEdpodEp(string token, string code, bool? UpdateNais)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CabinetTaxEdpodEpRequestBodyModel CTEERequestBody = new CabinetTaxEdpodEpRequestBodyModel
                {
                    Tins = code,                                                    // Код ЄДРПОУ аба ІПН
                    UpdateNais = UpdateNais                                         // 
                };

                string body = JsonConvert.SerializeObject(CTEERequestBody);         // Example body: {"tins":"3334800417","updateNais":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/podatkova/cabinettaxedpodep");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            CabinetTaxEdpodEpResponseModel CTEEResponse = new CabinetTaxEdpodEpResponseModel();

            CTEEResponse = JsonConvert.DeserializeObject<CabinetTaxEdpodEpResponseModel>(responseString);

            return CTEEResponse;

        }
    }

    /// <summary>
    ///  Модель запиту (Example: {"tin":"21560766"}) 
    /// </summary>
    public class CabinetTaxEdpodEpRequestBodyModel                              //
    {/// <summary>
     /// Код ІПН
     /// </summary>
        [JsonProperty("tins")]
        public string Tins { get; set; }                                        // 
        /// <summary>
        /// Потрібна перевірка статуса ФОП Nais
        /// </summary>
        [JsonProperty("updateNais")]
        public bool? UpdateNais { get; set; }                                   // 
    }
    /// <summary>
    /// Модель на відповідь CabinetTaxRegistration
    /// </summary>
    public class CabinetTaxEdpodEpResponseModel                                // 
    {/// <summary>
     /// Назва / ПІБ (на Nais)
     /// </summary>
        [JsonProperty("NAIS_NAME")]
        public string NaisName { get; set; }                                    // 
        /// <summary>
        /// Стан (на Nais) { "Зареєстровано", 1},{"В стані припинення",2},{"Припинено", 3},{ "Відомості про банкрутство", 4},{ "Санація", 5},{ "Свідоцтво недійсне", 6},{ "Статус не визначено", 99}
        /// </summary>
        [JsonProperty("NAIS_STATE")]
        public int? NaisState { get; set; }                                     //  
        /// <summary>
        /// Стан Id (на Nais)
        /// </summary>
        [JsonProperty("NAIS_STATETEXT")]
        public string NaisStateText { get; set; }                               // 
        /// <summary>
        /// Назва / ПІБ (на ДФС)
        /// </summary>
        [JsonProperty("FULL_NAME")]
        public string FullName { get; set; }                                    // 
        /// <summary>
        /// Код
        /// </summary>
        [JsonProperty("TIN")]
        public string Tin { get; set; }                                         // 
        /// <summary>
        /// Дата включенння до реєстру
        /// </summary>
        [JsonProperty("DATA_N")]
        public string DataN { get; set; }                                       // 
        /// <summary>
        /// Ставка ЄП
        /// </summary>
        [JsonProperty("STAVKA")]
        public double? Stavka { get; set; }                                     // 
        /// <summary>
        /// Група ЄП
        /// </summary>
        [JsonProperty("GRUP")]
        public double? Grup { get; set; }                                       // 
        /// <summary>
        /// Дата виключення з реєстру
        /// </summary>
        [JsonProperty("DATA_K")]
        public string DataK { get; set; }                                       // 
        /// <summary>
        /// КВЕДи
        /// </summary>
        [JsonProperty("KVEDS")]
        public List<Kved> Kveds { get; set; }                                   // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class Kved
    {/// <summary>
    /// ???
    /// </summary>
        [JsonProperty("KVED")]
        public string KvedKved { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("NU")]
        public string Nu { get; set; }
    }
}
