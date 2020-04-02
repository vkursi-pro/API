using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class RemoveReestrClass
    {
        /*

        16. Видалити список контрагентів
        [DELETE] /api/1.0/monitoring/removeReestr

        curl --location --request DELETE 'https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addNewReestr' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"reestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b"}'

        */

        public static string RemoveReestr(string reestrId, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RemoveReestrRequestBodyModel RRRequestBody = new RemoveReestrRequestBodyModel
                {
                    reestrId = reestrId                                         // Id реєстра який буде видалено (перелік всіх реєстрів можа отримати на api/1.0/monitoring/getAllReestr)
                };

                string body = JsonConvert.SerializeObject(RRRequestBody);       // Example body: {"reestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addNewReestr");
                RestRequest request = new RestRequest(Method.DELETE);

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
                    Console.WriteLine("За вказаними параметрами реєстр не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            return responseString;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"reestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...',
          'Content-Type': 'application/json'
        }
        conn.request("DELETE", "/api/1.0/monitoring/addNewReestr", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"reestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addNewReestr")
          .method("DELETE", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class RemoveReestrRequestBodyModel                                   // Модель Body запиту
    {
        public string reestrId { get; set; }                                    // Id реєстра який буде видалено (перелік всіх реєстрів можа отримати на api/1.0/monitoring/getAllReestr)
    }
}
