using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetBankruptcyByCodeClass
    {
        /*

        Метод:
            86. Відомості про банкрутство ВГСУ
            [POST] /api/1.0/organizations/getBankruptcyByCode

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getBankruptcyByCode' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetBankruptcyByCodeResponse.json

        */


        public static GetBankruptcyByCodeResponseModel GetBankruptcyByCode(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetBankruptcyByCodeRequestBodyModel GBBCRequest = new GetBankruptcyByCodeRequestBodyModel
                {
                    Codes = new List<string> { code }                           // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GBBCRequest);         // Example body: {"codes":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getBankruptcyByCode");
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

            GetBankruptcyByCodeResponseModel GBBCResponse = new GetBankruptcyByCodeResponseModel();

            GBBCResponse = JsonConvert.DeserializeObject<GetBankruptcyByCodeResponseModel>(responseString);

            return GBBCResponse;
        }
    }

    /*
    
    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"codes\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getBankruptcyByCode")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();


    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "codes": [
            "00131305"
          ]
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getBankruptcyByCode", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
    */

    public class GetBankruptcyByCodeRequestBodyModel
    {
        public List<string> Codes { get; set; }
    }

    public class GetBankruptcyByCodeResponseModel
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public List<GetBankruptcyByCodeModelAnswerData> Data { get; set; }
    }

    public class GetBankruptcyByCodeModelAnswerData
    {
        public List<GetBankruptcyByCodeData> VgsuData { get; set; }
        public string Code { get; set; }
    }

    public class GetBankruptcyByCodeData                                                // Дані ВГСУ
    {
        public string CaseNumber { get; set; }                                          // Номер судового рішення (maxLength:64)
        public string CourtName { get; set; }                                           // Назва суду (maxLength:256)
        public DateTime? DateProclamation { get; set; }                                 // Дата публікації
        public string Edrpou { get; set; }                                              // (maxLength:64)
        public DateTime? EndDate { get; set; }                                          // 
        public string Link { get; set; }                                                // Посилання на ВГСУ (maxLength:256)
        public string NameDebtor { get; set; }                                          // (maxLength:512)
        public string NumberAdvert { get; set; }                                        // (maxLength:64)
        public string PublicationType { get; set; }                                     // Тип інформації (maxLength:128)
        public DateTime? StartDate { get; set; }                                        // 
    }


}
