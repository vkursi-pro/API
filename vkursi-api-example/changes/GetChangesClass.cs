using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.changes
{
    public class GetChangesClass
    {
        /*
        
        6. Отримати дані щоденного моніторингу по фізичним, юриличним особам та об'єктам нерухомого майна які додані на моніторинг
        [GET] /api/1.0/changes/getchanges

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/changes/getchanges?addDate=25.09.2020' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJI...' \

        */

        public static List<GetChangesResponseModel> GetChanges(string date, string token, string addDate, bool clearHtml)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/changes/getchanges");
                RestRequest request = new RestRequest(Method.GET);

                request.AddParameter("addDate", addDate);                   // Дата в яку сервіс Vkursi виявив зміни

                request.AddParameter("date", date);                         // Дата зміни

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

            List<GetChangesResponseModel> ChangesResponseList = new List<GetChangesResponseModel>();

            ChangesResponseList = JsonConvert.DeserializeObject<List<GetChangesResponseModel>>(responseString);

            var sdsds = ChangesResponseList.Where(w => w.changeId == "4b3b01d3-1bdf-42a9-a7d4-561e76255d14").ToList();

            return ChangesResponseList;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = ''
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...'
        }
        conn.request("GET", "/api/1.0/changes/getchanges?date=28.10.2019", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/changes/getchanges?date=28.10.2019")
          .method("GET", null)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJ...")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class GetChangesResponseModel                            // Модель відповіді GetChanges
    {
        public string changeId { get; set; }                        // Id зміни
        public DateTime dateOfChange { get; set; }                  // Дата зміни
        public string changeType { get; set; }                      // Тип зміни
        public string addDate { get; set; }                         // Дата зміни (в сервісі Vkursi)
        public string change { get; set; }                          // Опис інформмації по зміну
        public OwnerChangesInfo ownerChangesInfo { get; set; }      // Інформация про організацію / ФОП по якому відбулась зміна
        public Guid? ReestrId { get; set; }                         // Id списку
        public string ReestrName { get; set; }                      // Назва списку
    }

    public class OwnerChangesInfo                                   // Інформация про організацію / ФОП по якому відбулась зміна
    {
        public string id { get; set; }                              // Системний Id
        public int type { get; set; }                               // Тип (1 - організация | 2 - фізична особа)
        public string name { get; set; }                            // Найменування
        public string code { get; set; }                            // Код ІНП / Єдрпоу
    }
}
