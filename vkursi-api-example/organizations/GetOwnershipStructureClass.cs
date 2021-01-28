using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOwnershipStructureClass
    {
        /*

        Метод:
            63. Структура власності компанії
            [POST] /api/1.0/organizations/GetOwnershipStructure

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getownershipstructure' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"edrpou":"31077508"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetOwnershipStructureResponse.json

        */

        public static GetOwnershipStructureResponseModel GetOwnershipStructure(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOwnershipStructureBodyModel GOSRequestBody = new GetOwnershipStructureBodyModel
                {
                    Edrpou = code,                                                      // Код ЄДРПОУ
                };

                string body = JsonConvert.SerializeObject(GOSRequestBody);              // Example Body: {"edrpou":"31077508"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getownershipstructure");
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

            GetOwnershipStructureResponseModel GOSResponse = new GetOwnershipStructureResponseModel();

            GOSResponse = JsonConvert.DeserializeObject<GetOwnershipStructureResponseModel>(responseString);

            return GOSResponse;
        }
    }

    /*
     
    // Python - http.client example:

        import http.client

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"edrpou\":\"31077508\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getownershipstructure", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"edrpou\":\"31077508\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getownershipstructure")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();
     
     */

    public class GetOwnershipStructureBodyModel                                         // Модель запиту (Example: {"edrpou":"31077508"})
    {
        [JsonProperty("edrpou")]
        public string Edrpou { get; set; }                                              // Код ЄДРПОУ
    }

    public class GetOwnershipStructureResponseModel                                     // Модель на відповідь GetExpressScore
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }                                             // Чи успішний запит

        [JsonProperty("status")]
        public string Status { get; set; }                                              // Статус відповіді по API

        [JsonProperty("data")]
        public List<GetOwnershipStructureRequestData> Data { get; set; }                // Дані методу
    }

    public class GetOwnershipStructureRequestData                                       // Дані методу
    {
        public string Id { get; set; }                                                  // Id
        public string Name { get; set; }                                                // Назва
        public string Adress { get; set; }                                              // Адреса
        public string Country { get; set; }                                             // Країна
        public bool IsFounder { get; set; }                                             // Чи є бенефіціаром
        public string RelationType { get; set; }                                        // Тип зв'язку
        public string NodeType { get; set; }                                            // Тип об'єкта ("company" или "people")
        public double? Persent { get; set; }                                            // Відсоток власності до запитуваної компанії
        public double? PersentOriginal { get; set; }                                    // Відсоток власності до компанії
        public string FounderAdress { get; set; }                                       // Адреса засновника
        public string Code { get; set; }                                                // Код ЄДРПОУ
        public string DirectControl { get; set; }                                       // Вплив. (Непрямий вирішальний вплив ≥ 25% / Прямий вирішальний вплив ≥ 25% / Непрямий не вирішальний вплив / Прямий не вирішальний вплив)
        public int? CountDirectControlPower { get; set; }                               // Прямий вирішальний вплив ≥ 25%
        public int? CountNotDirectControlPower { get; set; }                            // Непрямий вирішальний вплив ≥ 25%
        public int? CountDirectLessControlPower { get; set; }                           // Прямий вплив
        public int? CountNotDirectLessControlPower { get; set; }                        // Непрямий вплив
        public int? CountTotal { get; set; }                                            // 
        public List<GetOwnershipStructureRequestData> Child { get; set; }               // Вкладені елементи
    }
}
