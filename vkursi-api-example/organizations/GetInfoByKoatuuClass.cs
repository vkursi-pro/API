using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetInfoByKoatuuClass
    {
        /*
         
        14. Отримання переліку кодів ЄДРПОУ або Id фізичних або юридичних осіб які знаходятся за певним КОАТУУ
        [POST] api/1.0/organizations/getinfobykoatuu

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getinfobykoatuu' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"koatuuCode":"510900000","type":"1"}'

        */

        public static List<GetInfoByKoatuuResponseModel> GetInfoByKoatuu(string koatuuCode, string type, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetInfoByKoatuuRequestBodyModel GIBKRequestBody = new GetInfoByKoatuuRequestBodyModel
                {
                    koatuuCode = koatuuCode,                                    // Код КОАТУУ
                    type = type                                                 // Тип особи (1 - юридичні / 2 - фізичні)
                };

                string body = JsonConvert.SerializeObject(GIBKRequestBody);     // Example body: {"koatuuCode":"510900000","type":"1"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getinfobykoatuu");
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
                    Console.WriteLine("За вказаним кодом дані на знайдено");
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

            List<GetInfoByKoatuuResponseModel> InfoByKoatuuResponseList = new List<GetInfoByKoatuuResponseModel>();

            InfoByKoatuuResponseList = JsonConvert.DeserializeObject<List<GetInfoByKoatuuResponseModel>>(responseString);

            return InfoByKoatuuResponseList;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"koatuuCode\":\"510900000\",\"type\":\"1\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getinfobykoatuu", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"koatuuCode\":\"510900000\",\"type\":\"1\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getinfobykoatuu")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class GetInfoByKoatuuRequestBodyModel                                // Модель Body запиту
    {
        public string koatuuCode { get; set; }                                  // Код КОАТУУ
        public string type { get; set; }                                        // Тип особи (1 - юридичні / 2 - фізичні)
    }

    public class GetInfoByKoatuuResponseModel                                   // Модель відповіді GetInfoByKoatuu
    {
        public string code { get; set; }                                        // Код ЄДРПОУ або Id фізичної особи
    }
}
