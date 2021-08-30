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


    public class CabinetTaxEdpodEpRequestBodyModel                              // Модель запиту (Example: {"tin":"21560766"}) 
    {
        [JsonProperty("tins")]
        public string Tins { get; set; }                                        // Код ІПН

        [JsonProperty("updateNais")]
        public bool? UpdateNais { get; set; }                                   // Потрібна перевірка статуса ФОП Nais
    }

    public class CabinetTaxEdpodEpResponseModel                                // Модель на відповідь CabinetTaxRegistration
    {
        [JsonProperty("NAIS_NAME")]
        public string NaisName { get; set; }                                    // Назва / ПІБ (на Nais)

        [JsonProperty("NAIS_STATE")]
        public int? NaisState { get; set; }                                     // Стан (на Nais) { "Зареєстровано", 1},{"В стані припинення", 2},{"Припинено", 3},{ "Відомості про банкрутство", 4},{ "Санація", 5},{ "Свідоцтво недійсне", 6},{ "Статус не визначено", 99}

        [JsonProperty("NAIS_STATETEXT")]
        public string NaisStateText { get; set; }                               // Стан Id (на Nais)

        [JsonProperty("FULL_NAME")]
        public string FullName { get; set; }                                    // Назва / ПІБ (на ДФС)

        [JsonProperty("TIN")]
        public string Tin { get; set; }                                         // Код

        [JsonProperty("DATA_N")]
        public string DataN { get; set; }                                       // Дата включенння до реєстру

        [JsonProperty("STAVKA")]
        public double? Stavka { get; set; }                                     // Ставка ЄП

        [JsonProperty("GRUP")]
        public double? Grup { get; set; }                                       // Група ЄП

        [JsonProperty("DATA_K")]
        public string DataK { get; set; }                                       // Дата виключення з реєстру

        [JsonProperty("KVEDS")]
        public List<Kved> Kveds { get; set; }                                   // КВЕДи
    }

    public class Kved
    {
        [JsonProperty("KVED")]
        public string KvedKved { get; set; }

        [JsonProperty("NU")]
        public string Nu { get; set; }
    }
}
