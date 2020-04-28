using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
namespace vkursi_api_example.estate
{
    public class EstateInCreaseMonitoringPeriodClass
    {
        /*
         
        46. Змінити період (sms rrp) моніторингу нерухомості 
        [POST]  /api/1.0/estate/estateincreasemonitoringperiod

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estateincreasemonitoringperiod' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"OnmNumbers":[1260724348000],"CadastrNumbers":null,"DateTimeEnd":"2020-11-30T00:00:00"}'

         */

        public static EstateInCreaseMonitoringPeriodResponseModel EstateInCreaseMonitoringPeriod(string token, string dateTimeEnd)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                EstateInCreaseMonitoringPeriodRequestBodyModel EICMPRequestBody = new EstateInCreaseMonitoringPeriodRequestBodyModel
                {
                    DateTimeEnd = DateTime.Parse("30.11.2020"),
                    OnmNumbers = new List<long>
                    {
                        1260724348000
                    }
                };

                string body = JsonConvert.SerializeObject(EICMPRequestBody);

                // Example Body: {"OnmNumbers":[1260724348000],"CadastrNumbers":null,"DateTimeEnd":"2020-11-30T00:00:00"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateincreasemonitoringperiod");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstateInCreaseMonitoringPeriodResponseModel EICMPResponseRow = new EstateInCreaseMonitoringPeriodResponseModel();

            EICMPResponseRow = JsonConvert.DeserializeObject<EstateInCreaseMonitoringPeriodResponseModel>(responseString);

            return EICMPResponseRow;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null,\"DateTimeEnd\":\"2020-11-30T00:00:00\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/estate/estateincreasemonitoringperiod", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null,\"DateTimeEnd\":\"2020-11-30T00:00:00\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateincreasemonitoringperiod")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class EstateInCreaseMonitoringPeriodRequestBodyModel                         // Модель запиту 
    {
        public List<long> OnmNumbers { get; set; }                                      // Перелік номерів ОНМ
        public List<string> CadastrNumbers { get; set; }                                // Перелік кадастрових номерів
        public DateTime DateTimeEnd { get; set; }                                       // Дата до якої діє моніторинг
    }


    public class EstateInCreaseMonitoringPeriodResponseModel                            // Модель на відповідь
    {
        public bool IsSuccess { get; set; }                                             // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public List<string> NotFoundCadastrsOnMonitoring { get; set; }                  // Перелік не знайдених кадастрових номерів
        public List<string> NotFoundOnmOnMonitoring { get; set; }                       // Перелік не знайдених номерів ОНМ
    }
}
