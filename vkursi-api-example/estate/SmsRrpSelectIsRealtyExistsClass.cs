using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.estate
{
    public class SmsRrpSelectIsRealtyExistsClass
    {
        /*
        
        49. Перевірка наявності об'єкта за ОНМ (sms rrp)
        [POST] /api/1.0/estate/smsrrpselectisrealtyexists

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/smsrrpselectisrealtyexists' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIU...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"OnmNumbers":[1260724348000]}'

        */

        public static SmsRrpSelectIsRealtyExistsResponseModel SmsRrpSelectIsRealtyExists(string token, long onmNumber)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                SmsRrpSelectIsRealtyExistsRequestBodyModel SRSIRERequestBody = new SmsRrpSelectIsRealtyExistsRequestBodyModel
                {
                    OnmNumbers = new List<long> 
                    {
                        onmNumber
                    }
                };

                string body = JsonConvert.SerializeObject(SRSIRERequestBody);

                // Example Body: 

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/smsrrpselectisrealtyexists");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            SmsRrpSelectIsRealtyExistsResponseModel SRSIREResponseRow = new SmsRrpSelectIsRealtyExistsResponseModel();

            SRSIREResponseRow = JsonConvert.DeserializeObject<SmsRrpSelectIsRealtyExistsResponseModel>(responseString);

            return SRSIREResponseRow;
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"OnmNumbers\":[1260724348000]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJWa3Vyc2kiLCJlbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYTk4ZDUwMWYtOThmMi00NDk3LWIyNjMtOTY0YmY1ZTA0Y2RhIiwianRpIjoiNWUzNWMwMjEtMTM5OS00ZjYzLTgzMDItYjRlZTJmYjU5MWE0IiwiZXhwIjoxNTg4MTUyODM3LCJpc3MiOiJodHRwczovL3ZrdXJzaS5wcm8vIiwiYXVkIjoiaHR0cHM6Ly92a3Vyc2kucHJvLyJ9.g-PNEs6HsQx6xAu9MX3UaTzqbfb3qkkZReap9BYmVW0',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/estate/smsrrpselectisrealtyexists", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json,text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"OnmNumbers\":[1260724348000]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/smsrrpselectisrealtyexists")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUz...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

 */

    public class SmsRrpSelectIsRealtyExistsRequestBodyModel                             // Модель запиту 
    {
        public List<long> OnmNumbers { get; set; }                                      // Перелік номерів ОНМ
    }

    public class SmsRrpSelectIsRealtyExistsResponseModel                                // Модель на відповідь
    {
        public bool IsSuccess { get; set; }                                             // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public List<string> NotFoundOnmOnMonitoring { get; set; }                       // Перелік не знайдених номерів ОНМ
    }
}
