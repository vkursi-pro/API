using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class GetAllReestrClass
    {
        /*
         
        7. Отримати перелік списків (які користувач створив на vkursi.pro/eventcontrol#/reestr). Списки в сервісі вікористовуються для зберігання контрагентів, витягів та довідок
        [GET] /api/1.0/monitoring/getAllReestr

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/monitoring/getallreestr' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cC...' \
        --header 'Content-Type: application/json' \

        */

        public static List<GetAllReestrResponseModel> GetAllReestr(string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/getallreestr");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            List<GetAllReestrResponseModel> getAllReestrResponse = new List<GetAllReestrResponseModel>();

            getAllReestrResponse = JsonConvert.DeserializeObject<List<GetAllReestrResponseModel>>(responseString);

            return getAllReestrResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"21560045\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1...',
          'Content-Type': 'application/json',
        }
        conn.request("GET", "/api/1.0/monitoring/getallreestr", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/getallreestr")
          .method("GET", null)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJ...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class GetAllReestrResponseModel                                      // Модель відповіді GetAllReestr
    {
        public Guid Id { get; set; }                                            // Id списку
        public string Name { get; set; }                                        // Назва списку
    }
}
