using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.estate
{
    public class EstateRemoveFromMonitoringClass
    {
        /*

        47. Видалити об'єкт з мониторингу (sms rrp)
        [POST] /api/1.0/estate/estateremovefrommonitoring

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estateremovefrommonitoring' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGc...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"OnmNumbers":[1260724348000],"CadastrNumbers":null}'

        */

        public static EstateRemoveFromMonitoringResponseModel EstateRemoveFromMonitoring(string token, long onmNumber)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                EstateRemoveFromMonitoringBodyModel ERFMBodyModel = new EstateRemoveFromMonitoringBodyModel
                {
                    OnmNumbers = new List<long> 
                    {
                        onmNumber
                    }
                };

                string body = JsonConvert.SerializeObject(ERFMBodyModel);

                // Example Body: {"OnmNumbers":[1260724348000],"CadastrNumbers":null}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateremovefrommonitoring");
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

            EstateRemoveFromMonitoringResponseModel ERFMResponseRow = new EstateRemoveFromMonitoringResponseModel();

            ERFMResponseRow = JsonConvert.DeserializeObject<EstateRemoveFromMonitoringResponseModel>(responseString);

            return ERFMResponseRow;

        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1N...',
          'Content-Type': 'application/json',
          'Content-Type': 'text/plain',
          'Cookie': 'ARRAffinity=60c7763e47a70e864d73874a4687c10eb685afc08af8bda506303f7b37b172b8'
        }
        conn.request("POST", "/api/1.0/estate/estateremovefrommonitoring", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json,text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateremovefrommonitoring")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI")
          .addHeader("Content-Type", "application/json")
          .addHeader("Content-Type", "text/plain")
          .addHeader("Cookie", "ARRAffinity=60c7763e47a70e864d73874a4687c10eb685afc08af8bda506303f7b37b172b8")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class EstateRemoveFromMonitoringBodyModel                                    // 
    {/// <summary>
     /// Перелік номерів ОНМ
     /// </summary>
        public List<long> OnmNumbers { get; set; }                                      // 
        /// <summary>
        /// Перелік кадастрових номерів
        /// </summary>
        public List<string> CadastrNumbers { get; set; }                                // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class EstateRemoveFromMonitoringResponseModel                                // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Перелік не знайдених кадастрових номерів
        /// </summary>
        public List<string> NotFoundCadastrsOnMonitoring { get; set; }                  // 
        /// <summary>
        /// Перелік не знайдених номерів ОНМ
        /// </summary>
        public List<long> NotFoundOnmOnMonitoring { get; set; }                         // 
    }
}
