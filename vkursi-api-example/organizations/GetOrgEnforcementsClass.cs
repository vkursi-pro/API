using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetOrgEnforcementsClass
    {
        /*

        33. Список виконавчих проваджень по фізичним або юридичним особам за кодом ІПН / ЄДРПОУ
        [POST] /api/1.0/organizations/getorgenforcements

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgenforcements' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9....' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["00131305"],"Take":100,"Skip":0}'

        */

        public static GetOrgEnforcementsResponseModel GetOrgEnforcements(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgEnforcementsRequestBodyModel GOERequestBody = new GetOrgEnforcementsRequestBodyModel
                {
                    Edrpou = new List<string>                                   // Перелік кодів ЄДРПОУ (обеження 1)
                    {
                        code
                    },
                    Take = 3,                                                 // Кількість записів (ВП) які будуть отримані
                    Skip = 0                                                   // Кількість записів (ВП) які будуть пропущені
                };

                string body = JsonConvert.SerializeObject(GOERequestBody);      // Example Body: {"Edrpou":["00131305"],"Take":100,"Skip":0}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgenforcements");
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

            GetOrgEnforcementsResponseModel GOEResponse = new GetOrgEnforcementsResponseModel();

            GOEResponse = new GetOrgEnforcementsResponseModel();

            return GOEResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"00131305\"],\"Take\":100,\"Skip\":0}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getorgenforcements", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"00131305\"],\"Take\":100,\"Skip\":0}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgenforcements")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class GetOrgEnforcementsRequestBodyModel                             // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік кодів ЄДРПОУ (обеження 1)
        public int? Take { get; set; }                                          // Кількість записів (ВП) які будуть отримані
        public int? Skip { get; set; }                                          // Кількість записів (ВП) які будуть пропущені
    }

    public class GetOrgEnforcementsResponseModel                                // Модель відповіді GetOrgEnforcements
    {
        public bool IsSucces { get; set; }                                      // Успішний запит (true - так / false - ні)
        public string Succes { get; set; }                                      // Статус відповіді (maxLength:128)
        public OrgEnforcementApiAnswerModelData Data { get; set; }              // Дані
    }

    public class OrgEnforcementApiAnswerModelData                               // Дані
    {
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ (за яким бувВП запит) (maxLength:12)
        public int TotalCount { get; set; }                                     // Загальна кількість 
        public List<OrgEnforcementApiEnforcementsList> Enforcements { get; set; } // Пелелік виконавчих проваджень
    }

    public class OrgEnforcementApiEnforcementsList                              // Пелелік виконавчих проваджень
    {
        public DateTime? DateOpen { get; set; }                                 // Дата відкриття ВП
        public string VpNumber { get; set; }                                    // Номер ВП (maxLength:32)
        public string Status { get; set; }                                      // Статус ВП (Приклад: Боржник, Стягувач, ...) (maxLength:128)
        public string Category { get; set; }                                    // Категорія ВП (maxLength:256)
        public string State { get; set; }                                       // Стан (Приклад: Завершено, Примусове виконання, ...) (maxLength:128)
        public string Contractor { get; set; }                                  // Інша сторона (Приклад: Київське міжрегіональне управління укртрансбезпеки Код ЄДРПОУ: 37995466) (maxLength:512)
    }

}
