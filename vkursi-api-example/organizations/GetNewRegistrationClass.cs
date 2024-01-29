using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetNewRegistrationClass
    {
        /*
        
        15. Новий бізнес. Запит на отримання списку новозареєстрованих фізичних та юридичних осіб
        [POST] /api/1.0/organizations/getnewregistration

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"DateReg":"23.11.2021","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true}'

        Приклад відповіді (скорочена версія IsShortModel = true) 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseShort.json
        Модель відповіді (скорочена версія IsShortModel = true) 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetNewRegistrationClass.cs#L161

        Приклад відповіді (повна версія IsShortModel = false)
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseFull.json 
        Модель відповіді (повна версія IsShortModel = false)
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L130
        */

        public static string GetNewRegistration(string dateReg, string type, int? skip, int? take, bool? IsShortModel, bool? IsReturnAll, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetNewRegistrationRequestBodyModel GNRRequestBody = new GetNewRegistrationRequestBodyModel
                {
                    DateReg = dateReg,                                          // Дата державної реєстрації (фізичної або юридичної особи)
                    Type = type,                                                // Тип особи (1 - юридична особа/ 2 - фізичної особа)
                    Skip = skip,                                                // К-ть записів які траба пропустити
                    Take = take,                                                // К-ть записів які траба взяти (якщо null будуть передані всі записи)
                    IsShortModel = IsShortModel,                                // Коротка або повна модель відповіді
                    IsReturnAll = false                                         // Повернуті всі записи або тільки ті які раніше не отримували по API (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
                };

                string body = JsonConvert.SerializeObject(GNRRequestBody);      // Example Body: {"DateReg":"29.10.2019","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration");
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
                    Console.WriteLine("За вказаними параметрами данных не знайдено");
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

            List<GetAdvancedOrganizationResponseModel> NewRegistrationFullList = new List<GetAdvancedOrganizationResponseModel>();

            List<GetNewRegistrationResponseShortModel> GetNewRegistrationResponseShortList = new List<GetNewRegistrationResponseShortModel>();

            if (IsShortModel == null || IsShortModel == false)
            {
                NewRegistrationFullList = JsonConvert.DeserializeObject<List<GetAdvancedOrganizationResponseModel>>(responseString);
            }
            else if (IsShortModel == true)
            {
                GetNewRegistrationResponseShortList = JsonConvert.DeserializeObject<List<GetNewRegistrationResponseShortModel>>(responseString);
            }

            // Приклад відповіді(скорочена версія IsShortModel = true)
                // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseShort.json
            // Модель відповіді(скорочена версія IsShortModel = true)
                // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetNewRegistrationClass.cs#L161

            // Приклад відповіді(повна версія IsShortModel = false)
                // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseFull.json 
            // Модель відповіді(повна версія IsShortModel = false)
                // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L130

            return responseString;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"DateReg\":\"29.10.2019\",\"Type\":\"1\",\"Skip\":0,\"Take\":10,\"IsShortModel\":true,\"IsReturnAll\":true}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getnewregistration", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"DateReg\":\"29.10.2019\",\"Type\":\"1\",\"Skip\":0,\"Take\":10,\"IsShortModel\":true,\"IsReturnAll\":true}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetNewRegistrationRequestBodyModel                             // 
    {/// <summary>
     /// Дата державної реєстрації (фізичної або юридичної особи)
     /// </summary>
        [JsonProperty("DateReg", NullValueHandling = NullValueHandling.Ignore)]
        public string DateReg { get; set; }                                     // 
        /// <summary>
        /// Тип особи (1 - юридична особа / 2 - фізичної особа)
        /// </summary>
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }                                        // 
        /// <summary>
        /// К-ть записів які траба пропустити
        /// </summary>
        [JsonProperty("Skip", NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }                                          // 
        /// <summary>
        /// К-ть записів які траба взяти
        /// </summary>
        [JsonProperty("Take", NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }                                          // 
        /// <summary>
        /// Коротка або повна модель відповіді
        /// </summary>
        [JsonProperty("IsShortModel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsShortModel { get; set; }                                 // 
        /// <summary>
        /// Повернуті всі записи або тільки ті які раніше не отримували по API (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
        /// </summary>
        [JsonProperty("IsReturnAll", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReturnAll { get; set; }                                  // 

        /// <summary>
        /// Тільки зміни
        /// </summary>
        [JsonProperty("ChangesOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ChangesOnly { get; set; }
    }
    /// <summary>
    /// Модель відповіді GetNewRegistration скорочена
    /// </summary>
    public class GetNewRegistrationResponseShortModel                           // 
    {/// <summary>
     /// Системний id сервісу Vkursi
     /// </summary>
        public int Id { get; set; }                                             // 
        /// <summary>
        /// Стан реєстрації (Dictionary.OrganizationStateDict)
        /// </summary>
        public int? State { get; set; }                                         // 
        /// <summary>
        /// Стан реєстрації текст
        /// </summary>
        public string State_text { get; set; }                                  // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public string Registration_date { get; set; }                           // 
        /// <summary>
        /// Тип особи (1 - юридична особа / 2 - фізичної особа)
        /// </summary>
        public int Type { get; set; }                                           // 
        /// <summary>
        /// Скорочене найменування
        /// </summary>
        public string Short_name { get; set; }                                  // 
        /// <summary>
        /// Повне найменування
        /// </summary>
        public string Full_name { get; set; }                                   // 
        /// <summary>
        /// Назва організаційно правової форми власності
        /// </summary>
        public string Olf_name { get; set; }                                    // 
        /// <summary>
        /// ???
        /// </summary>
        public string Display_name { get; set; }                                // 
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }                                        // 
        /// <summary>
        /// Інформація про КВЕД
        /// </summary>
        public OrganizationaisPrimaryActivityKind Activity { get; set; }        // 
        /// <summary>
        /// Інформація про КВЕД
        /// </summary>
        public class OrganizationaisPrimaryActivityKind                         // 
        {/// <summary>
         /// Назва КВЕД
         /// </summary>
            public string name { get; set; }                                    // 
            /// <summary>
            /// Код КВЕД
            /// </summary>
            public string code { get; set; }                                    // 
            /// <summary>
            /// Дані про реєстраційний номер платника єдиного внеску
            /// </summary>
            public string reg_number { get; set; }                              // 
            /// <summary>
            /// Дані про клас ризику
            /// </summary>
            [JsonProperty("class")]
            public string classProp { get; set; }                               // 
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }                                       // 
        /// <summary>
        /// Номери телефонів
        /// </summary>
        public string[] Phones { get; set; }                                    // 
        /// <summary>
        /// Адреса повна
        /// </summary>
        public string Location_full { get; set; }                               // 
        /// <summary>
        /// Адреса детальна
        /// </summary>
        public OrganizationaisAddress Location_parts { get; set; }              // 
        /// <summary>
        /// Адреса детальна
        /// </summary>
        public class OrganizationaisAddress                                     // 
        {/// <summary>
         /// Поштовий індекс
         /// </summary>
            public string zip { get; set; }                                     // 
            /// <summary>
            /// Назва країни
            /// </summary>
            public string country { get; set; }                                 // 
            /// <summary>
            /// Адреса
            /// </summary>
            public string address { get; set; }                                 // 
            /// <summary>
            /// Детальна адреса
            /// </summary>
            public OrganizationaisParts parts { get; set; }                     // 
            /// <summary>
            /// Детальна адреса
            /// </summary>
            public class OrganizationaisParts                                   // 
            {/// <summary>
             /// Адміністративна територіальна одиниця
             /// </summary>
                public string atu { get; set; }                                 // 
                /// <summary>
                /// Вулиця
                /// </summary>
                public string street { get; set; }                              // 
                /// <summary>
                /// Тип будівлі ('буд.', 'інше')
                /// </summary>
                public string house_type { get; set; }                          // 
                /// <summary>
                /// 
                /// </summary>
                public string house { get; set; }                               // 
                /// <summary>
                /// Тип будівлі
                /// </summary>
                public string building_type { get; set; }                       // 
                /// <summary>
                /// Номер будівлі
                /// </summary>
                public string building { get; set; }                            // 
                /// <summary>
                /// Тип приміщення
                /// </summary>
                public string num_type { get; set; }                            // 
                /// <summary>
                /// Номер приміщення
                /// </summary>
                public string num { get; set; }                                 // 
            }
        }
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public OrganizationAdviceFullApiShortDirectorModel Ceo_name { get; set; }// 
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public class OrganizationAdviceFullApiShortDirectorModel                // 
        {/// <summary>
         /// Призвице керівника
         /// </summary>
            public string first_name { get; set; }                              // 
            /// <summary>
            /// Ім'я керівника 
            /// </summary>
            public string last_name { get; set; }                               //             
            /// <summary>
            /// По батькові керівника
            /// </summary>
            public string middle_name { get; set; }                             // 
        }
    }
}
