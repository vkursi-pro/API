using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.changes
{
    public class GetChangesByCodeClass
    {
        /*

        28. Метод АРІ, який віддає історію по компанії з можливістю обрати період.
        [POST] /api/1.0/changes/getchangesbyCode

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/changes/getchangesbyCode' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"00131305","FromDate":"20.11.2018","ToDate":"25.11.2019"}'

        */

        public static List<GetChangesByCodeResponseModel> GetChangesByCode(string token, string code, string fromDate, string toDate, int? fieldType)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetChangesByCodeRequestBodyModel GCBCResponseBodyRow = new GetChangesByCodeRequestBodyModel
                {
                    Code = code,                                                // Код ЄДРПОУ
                    FromDate = fromDate,                                        // Дата (зміни) від (включно)
                    ToDate = toDate,                                            // Дата (зміни) до (не включно)
                    FieldType = fieldType                                       // Тип зміни (словник FieldTypeDict можна отритами в  - 31. Основні словники сервісу [POST] /api/1.0/Dictionary/getdictionary)
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/changes/getchangesbyCode");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GCBCResponseBodyRow);

                // Example Body: {"Code":"00131305","FromDate":"20.11.2018","ToDate":"25.11.2019"}

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

            List<GetChangesByCodeResponseModel> GCBCResponse = new List<GetChangesByCodeResponseModel>();

            GCBCResponse = JsonConvert.DeserializeObject<List<GetChangesByCodeResponseModel>>(responseString);

            return GCBCResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"00131305\",\"FromDate\":\"20.11.2018\",\"ToDate\":\"25.11.2019\"}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJWa3Vyc2...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/changes/getchangesbyCode", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:


    */


    public class GetChangesByCodeRequestBodyModel                               // Модель Body запиту
    {
        public string Code { get; set; }                                        // Код ЄДРПОУ
        public string FromDate { get; set; }                                    // Дата від (включно)
        public string ToDate { get; set; }                                      // Дата до (не включно)
        public int? FieldType { get; set; }                                     // Тип зміни (словник FieldTypeDict можна отритами в  - 31. Основні словники сервісу [POST] /api/1.0/Dictionary/getdictionary)
    }

    public class GetChangesByCodeResponseModel                                  // Модель відповіді
    {
        public DateTime dateOfChange { get; set; }                              // Дата зміни
        public string changeType { get; set; }                                  // Тип зміни (текст)
        public string change { get; set; }                                      // Текст зміни
        public OwnerChangesInfoModel ownerChangesInfo { get; set; }             // Відомості про організацію 
    }

    public class OwnerChangesInfoModel                                          // Відомості про організацію                   
    {
        public string id { get; set; }                                          // Системий Id в сервісі Vkursi
        public int type { get; set; }                                           // Тип (організація / ФОП)
        public string name { get; set; }                                        // Назва 
        public string code { get; set; }                                        // Код ЄДРПОУ
    }
}
