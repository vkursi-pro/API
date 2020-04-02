using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;


namespace vkursi_api_example.bi
{
    public class GetBiImportLabelsClass
    {
        /*

        18. Отримати перелік Label доступних в модулі BI
        [GET] api/1.0/bi/getbiimportlabels

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/bi/getbiimportlabels' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \

         */

        public static List<string> GetBiImportLabels(string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/getbiimportlabels");
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

            GetBiImportLabelsResponsModel GBILRespons = new GetBiImportLabelsResponsModel();

            GBILRespons = JsonConvert.DeserializeObject<GetBiImportLabelsResponsModel>(responseString);

            return GBILRespons.data;
        }
    }

    /*
        // Python - http.client example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/bi/getbiimportlabels")
          .method("GET", null)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .build();
        Response response = client.newCall(request).execute();


        // Java - OkHttp example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = ''
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...'
        }
        conn.request("GET", "/api/1.0/bi/getbiimportlabels", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

     */

    public class GetBiImportLabelsResponsModel                                  // Модель відповіді GetBiImportLabels
    {
        public bool isSuccess { get; set; }                                     // Успішно виконано?
        public string status { get; set; }                                      // success, error
        public List<string> data { get; set; }                                  // Перелік доступних Labels
    }
}