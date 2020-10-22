using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{
    public class GetContentClass
    {

        /*
        
        11. Запит на отримання контенту судового рішення за id документа (id документа можна отримати в api/1.0/courtdecision/getdecisions)
        [POST] /api/1.0/courtdecision/getcontent       

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getcontent' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5c...' \
        --header 'Content-Type: application/json' \
        --data-raw '"84583482"'      

        */

        public static string GetContent(string courtDecisionId, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getcontent");
                RestRequest request = new RestRequest(Method.POST);

                string body = "\"" + courtDecisionId + "\"";                // Example body: "84583482"

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
                    Console.WriteLine("За вказаним Id документа не знайдено");
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

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(responseString);

            return responseString;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "\"84583482\""
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/courtdecision/getcontent", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "\"84583482\"");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getcontent")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

}
