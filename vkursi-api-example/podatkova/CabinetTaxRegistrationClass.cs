using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.podatkova
{
    public class CabinetTaxRegistrationClass
    {
        /*
        
        Метод:
            73. Відомості про субєктів господарювання які стоять на обліку в ДФС
            [POST] /api/1.0/podatkova/cabinettaxregistration

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/podatkova/cabinettaxregistration' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"tin":"21560766"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CabinetTaxRegistration.json

        */

        public static CabinetTaxRegistrationResponseModel CabinetTaxRegistration(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CabinetTaxRegistrationRequestBodyModel CTRRBody = new CabinetTaxRegistrationRequestBodyModel
                {
                    Tin = code                                              // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(CTRRBody);        // Example body: {"tin":"21560766"}

                body = "{\"tin\":\"38715972\",\"name\":\"ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ \"МУН КОНЦЕРТ\"\"}";

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/podatkova/cabinettaxregistration");
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

            CabinetTaxRegistrationResponseModel CTRResponse = new CabinetTaxRegistrationResponseModel();

            CTRResponse = JsonConvert.DeserializeObject<CabinetTaxRegistrationResponseModel>(responseString);

            return CTRResponse;
        }
    }

    /// <summary>
    /// Модель запиту (Example: {"tin":"21560766"})
    /// </summary>
    public class CabinetTaxRegistrationRequestBodyModel                             // 
    {/// <summary>
     /// Код ЄДРПОУ або ІПН
     /// </summary>
        public string Tin { get; set; }                                             // 
    }
    /// <summary>
    /// Модель на відповідь CabinetTaxRegistration
    /// </summary>
    public class CabinetTaxRegistrationResponseModel                                // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool Success { get; set; }                                           // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                          // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public CabinetTaxRegistrationResponse Response { get; set; }                // 
    }
    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class CabinetTaxRegistrationResponse                                     // 
    {/// <summary>
     /// Назва
     /// </summary>
        [JsonProperty("FULL_NAME")]
        public string FullName { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ
        /// </summary>
        [JsonProperty("TIN_S")]
        public object TinS { get; set; }                                            // 
        /// <summary>
        /// Юридична адреса
        /// </summary>
        [JsonProperty("ADRESS")]
        public object Adress { get; set; }                                          // 
        /// <summary>
        /// Дата реєстрації в органах ДПС
        /// </summary>
        [JsonProperty("D_REG_STI")]
        public string DRegSti { get; set; }                                         // 
        /// <summary>
        /// Номер реєстрації в органах ДПС
        /// </summary>
        [JsonProperty("N_REG_STI")]
        public string NRegSti { get; set; }                                         // 
        /// <summary>
        /// Назва органу ДПС де зареєстрований платник
        /// </summary>
        [JsonProperty("C_STI_MAIN_NAME")]
        public string CStiMainName { get; set; }                                    // 
        /// <summary>
        /// Ознака зовнішньоекономічної діяльності
        /// </summary>
        [JsonProperty("VED_LIC")]
        public object VedLic { get; set; }                                          // 
        /// <summary>
        /// Тип платника
        /// </summary>
        [JsonProperty("FACE_MODE")]
        public long? FaceMode { get; set; }                                         // 
        /// <summary>
        /// Стан платника
        /// </summary>
        [JsonProperty("C_STAN")]
        public long? CStan { get; set; }                                            // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("D_ZAKR_STI")]
        public string DZakrSti { get; set; }                                        // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("C_KIND")]
        public long? CKind { get; set; }                                            // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("C_CLOSE")]
        public long? CClose { get; set; }                                           // 
        /// <summary>
        /// Відомості про помилку
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }                                           // 
        /// <summary>
        /// Опис помилки
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }                                // 
    }


}
