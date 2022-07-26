using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class AddNewReestrClass
    {
        /*
         
        8. Додати новий список контрагентів (список також можна створиты з інтерфейсу на сторінці vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок
        [POST] /api/1.0/monitoring/addNewReestr
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addNewReestr' \
        --header 'Content-Type: application/json' \
        --header 'Authorization: Bearer  eyJhbGciOiJIUzI1NiIsInR...' \
        --data-raw '{"reestrName":"Назва нового реєстру"}'


        */

        public static string AddNewReestr(string reestrName, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                AddNewReestrResponseModel AddNewReestrResponseModel = new AddNewReestrResponseModel
                {
                    reestrName = reestrName                                             // Назва нового списку (який буде створено)
                };

                string body = JsonConvert.SerializeObject(AddNewReestrResponseModel);   // Example body: {"reestrName":"Назва нового реєстру"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addnewreestr");
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

            return responseString.Replace("\"","");                                     // В відповідь проходить системный id реєстру в сервісі VKursi
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"reestrName\":\"Назва нового реєстру\"}"
        headers = {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer  eyJhbGciOiJIUzI1NiIsIn...'
        }
        conn.request("POST", "/api/1.0/monitoring/addNewReestr", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"reestrName\":\"Назва нового реєстру\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addNewReestr")
          .method("POST", body)
          .addHeader("Content-Type", "application/json")
          .addHeader("Authorization", "Bearer  eyJhbGciOiJIUzI1NiIsIn...")
          .build();
        Response response = client.newCall(request).execute();

     
    */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class AddNewReestrResponseModel                                              // 
    {/// <summary>
     /// Назва нового списку (який буде створено)
     /// </summary>
        public string reestrName { get; set; }                                          // 
    }
}
