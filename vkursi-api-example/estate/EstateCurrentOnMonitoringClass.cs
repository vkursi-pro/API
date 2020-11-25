using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.estate
{
    public class EstateCurrentOnMonitoringClass
    {
        /*

        50. Отримати перелік обєктів ОНМ які встановлено на моніторинг нерухомого майна (SMS RRP)
        [GET] /api/1.0/estate/estateCurrentOnMonitoring

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estateCurrentOnMonitoring' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX...' \

        */

        public static EstateCurrentOnMonitoringResponseModel EstateCurrentOnMonitoring(string token)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateCurrentOnMonitoring");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);

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

            EstateCurrentOnMonitoringResponseModel ECOMResponse = new EstateCurrentOnMonitoringResponseModel();

            ECOMResponse = JsonConvert.DeserializeObject<EstateCurrentOnMonitoringResponseModel>(responseString);

            return ECOMResponse;
        }
    }

    /*
     
        // Python - http.client example:

        import http.client

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = ''
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsI...'
        }
        conn.request("GET", "/api/1.0/estate/estateCurrentOnMonitoring", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateCurrentOnMonitoring")
          .method("GET", null)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .build();
        Response response = client.newCall(request).execute();    
     
    */



    public partial class EstateCurrentOnMonitoringResponseModel                         // Модель відповіді EstateCurrentOnMonitoring
    {
        public bool IsSuccess { get; set; }                                             // Успішний запит (true - так / false - ні)
        public string Status { get; set; }                                              // Статус відповіді (maxLength:128)
        public List<MonitoringListData> MonitoringList { get; set; }                    // Дані
    }

    public partial class MonitoringListData                                             // Дані
    {
        public string RegNumber { get; set; }                                           // ОНМ
        public string RegName { get; set; }                                             // Назва обьекта 
        public DateTime? EndDate { get; set; }                                          // Дата початку моніторинга
        public DateTime? StartDate { get; set; }                                        // Дата закінчення моніторинга
    }
}
