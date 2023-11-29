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
    /// <summary>
    /// Модель запиту (Example: {"edrpou":"31077508"})
    /// </summary>
    public class GetOwnershipStructureBodyModel                                         // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        [JsonProperty("edrpou")]
        public string Edrpou { get; set; }                                              // 
    }
    /// <summary>
    /// Модель на відповідь GetExpressScore
    /// </summary>
    public class GetOwnershipStructureResponseModel                                     // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дані методу
        /// </summary>
        [JsonProperty("data")]
        public List<GetOwnershipStructureRequestData> Data { get; set; }                // 
    }
    /// <summary>
    /// Дані методу
    /// </summary>
    public class GetOwnershipStructureRequestData                                       // 
    {/// <summary>
     /// Id
     /// </summary>
        public string Id { get; set; }                                                  // 
        /// <summary>
        /// Назва
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Адреса
        /// </summary>
        public string Adress { get; set; }                                              // 
        /// <summary>
        /// Країна
        /// </summary>
        public string Country { get; set; }                                             // 
        /// <summary>
        /// Чи є бенефіціаром
        /// </summary>
        public bool IsFounder { get; set; }                                             // 
        /// <summary>
        /// Тип зв'язку
        /// </summary>
        public string RelationType { get; set; }                                        // 
        /// <summary>
        /// Тип об'єкта ("company" или "people")
        /// </summary>
        public string NodeType { get; set; }                                            // 
        /// <summary>
        /// Відсоток власності до запитуваної компанії
        /// </summary>
        public double? Persent { get; set; }                                            // 
        /// <summary>
        /// Відсоток власності до компанії
        /// </summary>
        public double? PersentOriginal { get; set; }                                    // 
        /// <summary>
        /// Адреса засновника
        /// </summary>
        public string FounderAdress { get; set; }                                       // 
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }                                                // 
        /// <summary>
        /// Вплив. (Непрямий вирішальний вплив ≥ 25% / Прямий вирішальний вплив ≥ 25% / Непрямий не вирішальний вплив / Прямий не вирішальний вплив)
        /// </summary>
        public string DirectControl { get; set; }                                       // 
        /// <summary>
        /// Прямий вирішальний вплив ≥ 25%
        /// </summary>
        public int? CountDirectControlPower { get; set; }                               // 
        /// <summary>
        /// Непрямий вирішальний вплив ≥ 25%
        /// </summary>
        public int? CountNotDirectControlPower { get; set; }                            // 
        /// <summary>
        /// Прямий вплив
        /// </summary>
        public int? CountDirectLessControlPower { get; set; }                           // 
        /// <summary>
        /// Непрямий вплив
        /// </summary>
        public int? CountNotDirectLessControlPower { get; set; }                        // 
        /// <summary>
        /// ???
        /// </summary>
        public int? CountTotal { get; set; }                                            // 
        /// <summary>
        /// Вкладені елементи
        /// </summary>
        public List<GetOwnershipStructureRequestData> Child { get; set; }               // 
    }
}
