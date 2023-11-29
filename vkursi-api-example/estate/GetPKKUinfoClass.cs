using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetPKKUinfoClass
    {
        /*

        Метод:
            65. Отримати скорочені дані ДЗК за списком кадастрових номерів
            [POST] /api/1.0/estate/GetPKKUinfo

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/GetPKKUinfo' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"CadNumbers":["5321386400:00:042:0028","5321386400:00:020:0039"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetPKKUinfoResponse.json

        */

        public static GetPKKUinfoResponseModel GetPKKUinfo(ref string token, List<string> cadNumber)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPKKUinfoRequestBodyModel GPKKUIRequestBody = new GetPKKUinfoRequestBodyModel
                {
                    CadNumbers = new List<string>()                                     // Перелік кадастрових номерів (обеження 100)

                };

                GPKKUIRequestBody.CadNumbers = cadNumber;

                string body = JsonConvert.SerializeObject(GPKKUIRequestBody);           // Example Body: {"CadNumbers":["5321386400:00:042:0028","5321386400:00:020:0039"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/GetPKKUinfo");
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

            GetPKKUinfoResponseModel GPKKUIResponse = new GetPKKUinfoResponseModel();

            GPKKUIResponse = JsonConvert.DeserializeObject<GetPKKUinfoResponseModel>(responseString);

            return GPKKUIResponse;

        }
    }

    /*
    
    // Python - http.client example:
     
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"CadNumbers\":[\"5321386400:00:042:0028\",\"5321386400:00:020:0039\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/GetPKKUinfo")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();
     

    // Java - OkHttp example:

        import http.client

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"CadNumbers\":[\"5321386400:00:042:0028\",\"5321386400:00:020:0039\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVC...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/estate/GetPKKUinfo", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
     */

    public class GetPKKUinfoRequestBodyModel
    {
        public List<string> CadNumbers { get; set; }
    }

    public class GetPKKUinfoResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public List<KadastrMapApiDetailsEstate> Data { get; set; }
    }

    public class KadastrMapApiDetailsEstate
    {
        public string Id { get; set; }
        public long? Koatuu { get; set; }
        public int? Zona { get; set; }
        public int? Kvartal { get; set; }
        public int? Parcel { get; set; }
        public int? Ownershipcode { get; set; }
        public string Purpose { get; set; }
        public string Use { get; set; }
        public double? Area { get; set; }
        public string Ownershipvalue { get; set; }
        public string Region { get; set; }
    }
}
