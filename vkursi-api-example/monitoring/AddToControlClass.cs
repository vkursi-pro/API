using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class AddToControlClass
    {
        /*
         
         12. Додавання фізичних та юридичних осіб до списку(реєстру) та моніторингу
         [POST] api/1.0/Monitoring/addToControl

         curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/Monitoring/addToControl' \
         --header 'ContentType: application/json' \
         --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
         --header 'Content-Type: application/json' \
         --data-raw '{"Codes":["00131305"],"ReestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b","IsOnMonitoring":true,"Persons":[{"Name":"Шевченко Тарас Григорович","Ipn":"3334725058","Birthday":"09.03.1978","PassportCode":"HM156253"}]}'

         */

        public static List<AddToControlResponseModel> AddToControl(string code, string ReestrId, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                AddToControlRequestBodyModel ATCRBodyRow = new AddToControlRequestBodyModel
                {
                    Codes = new List<string>                                // Перелік кодів ЄДРПОУ
                    {
                        code
                    },
                    ReestrId = ReestrId,                                    // Id списка(реєстра) в який будуть додані записи (перелік мона отримати api/1.0/monitoring/getAllReestr)
                    IsOnMonitoring = true,                                  // Автоматично додати до моніторингу (true - так / false - ні)
                    Persons = new List<EventControlPersonAddItemModel>      // Перелік фізичних осіб для додавання в списки
                    {
                        new EventControlPersonAddItemModel
                        {
                            Name = "Шевченко Тарас Григорович",             // ПІБ фізичної особи
                            Ipn = "3334725058",                             // ІПН фізичної особи
                            Birthday = "09.03.1978",                        // Дата народження
                            PassportCode = "HM156253"                       // Серія та номер паспорта
                        }
                    }
                };
                
                string body = JsonConvert.SerializeObject(ATCRBodyRow);     // Example body: {"Codes":["00131305"],"ReestrId":"1c891112-b022-4a83-ad34-d1f976c60a0b","IsOnMonitoring":true,"Persons":[{"Name":"Шевченко Тарас Григорович","Ipn":"3334725058","Birthday":"09.03.1978","PassportCode":"HM156253"}]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/Monitoring/addToControl");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if (responseString == "\"reestrId not found\"")
                {
                    Console.WriteLine("За вказаним reestrId реєстр не знайдено. Отримати перелік реєстрів api/1.0/monitoring/getAllReestr");
                    return null;
                }

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

            List<AddToControlResponseModel> ATCResponse = new List<AddToControlResponseModel>();

            ATCResponse = JsonConvert.DeserializeObject<List<AddToControlResponseModel>>(responseString);

            return ATCResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Codes\":[\"00131305\"],\"ReestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\",\"IsOnMonitoring\":true,\"Persons\":[{\"Name\":\"Шевченко Тарас Григорович\",\"Ipn\":\"3334725058\",\"Birthday\":\"09.03.1978\",\"PassportCode\":\"HM156253\"}]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/Monitoring/addToControl", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
        .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Codes\":[\"00131305\"],\"ReestrId\":\"1c891112-b022-4a83-ad34-d1f976c60a0b\",\"IsOnMonitoring\":true,\"Persons\":[{\"Name\":\"Шевченко Тарас Григорович\",\"Ipn\":\"3334725058\",\"Birthday\":\"09.03.1978\",\"PassportCode\":\"HM156253\"}]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/Monitoring/addToControl")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class AddToControlRequestBodyModel                                   // Модель Body запиту
    {
        public List<string> Codes { get; set; }                                 // Перелік кодів ЄДРПОУ
        public string ReestrId { get; set; }                                    // Id списка(реєстра) в який будуть додані записи
        public bool? IsOnMonitoring { get; set; }                               // Автоматично додати до моніторингу (true - так / false - ні)
        public List<EventControlPersonAddItemModel> Persons { get; set; }       // Перелік фізичних осіб для додавання в списки
    }

    public class EventControlPersonAddItemModel                                 // Перелік фізичних осіб для додавання в списки
    {
        public string Name { get; set; }                                        // ПІБ фізичної особи
        public string Ipn { get; set; }                                         // ІПН фізичної особи
        public string Birthday { get; set; }                                    // Дата народження
        public string PassportCode { get; set; }                                // Серія та номер паспорта
    }

    public class AddToControlResponseModel                                      // Модель відповіді AddToControl
    {
        public string name { get; set; }                                        // Назва 
        public string shortName { get; set; }                                   // Скорочена назва
        public string code { get; set; }                                        // Код
        public string boss { get; set; }                                        // Керівник
        public string location { get; set; }                                    // Адреса
        public string kved { get; set; }                                        // КВЕД
        public string state { get; set; }                                       // Стан
        public string dateAddedToMonitoring { get; set; }                       // Дата додавання
    }
}
