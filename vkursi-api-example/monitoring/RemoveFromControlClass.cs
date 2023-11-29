using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class RemoveFromControlClass
    {
        /*
         
        13. Видалити контрагентів зі списку
        [POST] /api/1.0/Monitoring/removeFromControl     

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/monitoring/removeFromControl' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIs...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"codes":["00131305"],"reestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b"}'

        */

        public static bool RemoveFromControl(string code, string reestrId, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            bool isSuccessful = false;

            while (string.IsNullOrEmpty(responseString))
            {
                RemoveFromControlRequestBodyModel RFCRequestBody = new RemoveFromControlRequestBodyModel
                {
                    codes = new List<string>                                    // Перелік кодів ЄДРПОУ / ІПН які будуть видалені
                    {
                        code
                    },
                    reestrId = reestrId                                         // Id реєстра з якого будуть видалені
                };


                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/removeFromControl");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(RFCRequestBody);      // Example body: {"codes":["00131305"],"reestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b"}

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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return isSuccessful;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return isSuccessful;
                }
            }

            isSuccessful = true;                                                // Виконано успішно

            return isSuccessful;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"codes\":[\"00131305\"],\"reestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXV...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/monitoring/removeFromControl", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"codes\":[\"00131305\"],\"reestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/removeFromControl")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsIn...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class RemoveFromControlRequestBodyModel                              // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ / ІПН які будуть видалені
     /// </summary>
        public List<string> codes { get; set; }                                 // 
        /// <summary>
        /// Id реєстра
        /// </summary>
        public string reestrId { get; set; }                                    // 
    }
}
