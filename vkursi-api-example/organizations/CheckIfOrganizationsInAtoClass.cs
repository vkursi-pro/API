using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example._2._0;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class CheckIfOrganizationsInAtoClass
    {
        /*
        
        Метод:
            76. Перевірка чи знаходиться аблеса компанії/ФОП в зоні АТО (доступно в конструкторі API №76)
            [POST] /api/1.0/organizations/CheckIfOrganizationsInAto

        Приклад запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckIfOrganizationsInAto' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIs...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":["25412361", "2674001651"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckIfOrganizationsInAtoResponse.json
        
        */

        public static CheckIfOrganizationsInAtoResponseModel CheckIfOrganizationsInAto(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckIfOrganizationsInAto");
                RestRequest request = new RestRequest(Method.POST);

                CheckIfOrganizationsInAtoRequestBodyModel CIOIARequest = new CheckIfOrganizationsInAtoRequestBodyModel
                {
                    Code = new List<string>() { "25412361", "2674001651" }
                };

                string body = JsonConvert.SerializeObject(CIOIARequest); // Example: {"Code":["25412361", "2674001651"]}

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

            CheckIfOrganizationsInAtoResponseModel CIOIARow = new CheckIfOrganizationsInAtoResponseModel();

            CIOIARow = JsonConvert.DeserializeObject<CheckIfOrganizationsInAtoResponseModel>(responseString);

            // Тут можна переглянути json відповіді
            string сheckIfOrganizationsInAtoResponseString = JsonConvert.SerializeObject(CIOIARow, Formatting.Indented);

            return CIOIARow;
        }
    }


    /*

    // Java - OkHttp example:

    OkHttpClient client = new OkHttpClient().newBuilder()
      .build();
    MediaType mediaType = MediaType.parse("application/json");
    RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"25412361\", \"2674001651\"]}");
    Request request = new Request.Builder()
      .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CheckIfOrganizationsInAto")
      .method("POST", body)
      .addHeader("ContentType", "application/json")
      .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cC...")
      .addHeader("Content-Type", "application/json")
      .build();
    Response response = client.newCall(request).execute(); 



    // Python - http.client example:

    import http.client
    import json

    conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
    payload = json.dumps({
      "Code": [
        "25412361",
        "2674001651"
      ]
    })
    headers = {
      'ContentType': 'application/json',
      'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...',
      'Content-Type': 'application/json'
    }
    conn.request("POST", "/api/1.0/organizations/CheckIfOrganizationsInAto", payload, headers)
    res = conn.getresponse()
    data = res.read()
    print(data.decode("utf-8"))
     
    */


    public class CheckIfOrganizationsInAtoRequestBodyModel                              // Модель запиту 
    {
        public List<string> Code { get; set; }                                          // Перелік кодів ЄДРПОУ / ІПН
    }


    public class CheckIfOrganizationsInAtoResponseModel                                 // Відповідь на запит
    {
        public bool IsSuccess { get; set; }                                             // Запит виконано успішно (true - так / false - ні)
        public string Status { get; set; }                                              // Статус запиту (maxLength:128)
        public List<CheckIfOrganizationsInAtoAnswerData> Data { get; set; }             // Перелік суб'єктів
    }

    /// <summary>
    /// Перевірка чи знаходиться аблеса компанії/ФОП в зоні АТО
    /// </summary>
    public class CheckIfOrganizationsInAtoAnswerData                                    // Перелік суб'єктів
    {
        public string Code { get; set; }                                                // Код
        public bool? RegisterInAto { get; set; }                                        // Чи знаходиться в зоні АТО (true - так / false - ні)
    }
}
