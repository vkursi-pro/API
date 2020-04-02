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

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"DateReg":"29.10.2019","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true}'

        */

        public static string GetNewRegistration(string dateReg, string type, int? skip, int? take, bool? IsShortModel, bool? IsReturnAll, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

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
                    IsReturnAll = IsShortModel                                  // Повернуті всі записи або тільки ті які раніше не отримували по API
                                                                                // (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
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
                    token = AuthorizeClass.Authorize();
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

            List<OrganizationAdviceFullApiShortModel> NewRegistrationShortList = new List<OrganizationAdviceFullApiShortModel>();

            if (IsShortModel == null || IsShortModel == false)
            {
                NewRegistrationFullList = JsonConvert.DeserializeObject<List<GetAdvancedOrganizationResponseModel>>(responseString);
            }
            else if (IsShortModel == true)
            {
                NewRegistrationShortList = JsonConvert.DeserializeObject<List<OrganizationAdviceFullApiShortModel>>(responseString);
            }

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

    public class GetNewRegistrationRequestBodyModel                             // Модель Body запиту
    {
        public string DateReg { get; set; }                                     // Дата державної реєстрації (фізичної або юридичної особи)
        public string Type { get; set; }                                        // Тип особи (1 - юридична особа / 2 - фізичної особа)
        public int? Skip { get; set; }                                          // К-ть записів які траба пропустити
        public int? Take { get; set; }                                          // К-ть записів які траба взяти
        public bool? IsShortModel { get; set; }                                 // Коротка або повна модель відповіді
        public bool? IsReturnAll { get; set; }                                  // Повернуті всі записи або тільки ті які раніше не отримували по API (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
    }

    public class OrganizationAdviceFullApiShortModel                            // Модель відповіді GetNewRegistration
    {
        public int Id { get; set; }                                             // Системний id сервісу Vkursi
        public int? State { get; set; }                                         // Стан реєстрації (Dictionary.OrganizationStateDict)
        public string State_text { get; set; }                                  // Стан реєстрації текст
        public string Registration_date { get; set; }                           // Дата реєстрації
        public int Type { get; set; }                                           // Тип особи (1 - юридична особа / 2 - фізичної особа)
        public string Short_name { get; set; }                                  // Скорочене найменування
        public string Full_name { get; set; }                                   // Повне найменування
        public string Olf_name { get; set; }                                    // Назва організаційно правової форми власності
        public string Display_name { get; set; }                                // 
        public string Code { get; set; }                                        // Код ЄДРПОУ
        public OrganizationaisPrimaryActivityKind Activity { get; set; }        // Інформація про КВЕД

        public class OrganizationaisPrimaryActivityKind                         // Інформація про КВЕД
        {
            public string name { get; set; }                                    // Назва КВЕД
            public string code { get; set; }                                    // Код КВЕД
            public string reg_number { get; set; }                              // Дані про реєстраційний номер платника єдиного внеску
            [JsonProperty("class")]
            public string classProp { get; set; }                               // Дані про клас ризику
        }

        public string Email { get; set; }                                       // Email
        public string[] Phones { get; set; }                                    // Номери телефонів
        public string Location_full { get; set; }                               // Адреса повна
        public OrganizationaisAddress Location_parts { get; set; }              // Адреса детальна

        public class OrganizationaisAddress                                     // Адреса детальна
        {
            public string zip { get; set; }                                     // Поштовий індекс
            public string country { get; set; }                                 // Назва країни
            public string address { get; set; }                                 // Адреса
            public OrganizationaisParts parts { get; set; }                     // Детальна адреса

            public class OrganizationaisParts                                   // Детальна адреса
            {
                public string atu { get; set; }                                 // Адміністративна територіальна одиниця
                public string street { get; set; }                              // Вулиця
                public string house_type { get; set; }                          // Тип будівлі ('буд.', 'інше')
                public string house { get; set; }                               // Номер будинку, якщо тип - 'буд.'
                public string building_type { get; set; }                       // Тип будівлі
                public string building { get; set; }                            // Номер будівлі
                public string num_type { get; set; }                            // Тип приміщення
                public string num { get; set; }                                 // Номер приміщення
            }
        }

        public OrganizationAdviceFullApiShortDirectorModel Ceo_name { get; set; }// ПІБ керівника

        public class OrganizationAdviceFullApiShortDirectorModel                // ПІБ керівника
        {
            public string first_name { get; set; }                              // Призвице керівника
            public string last_name { get; set; }                               // Ім'я керівника             
            public string middle_name { get; set; }                             // По батькові керівника
        }
    }
}
