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
                    Codes = new List<string> { code }                           // Код ЄДРПОУ або ІПН
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
    /// <summary>
    /// ???
    /// </summary>
    public class GetBankruptcyByCodeRequestBodyModel
    {/// <summary>
    /// ???
    /// </summary>
        public List<string> Codes { get; set; }
    }
    /// <summary>
    /// ???
    /// </summary>
    public class GetBankruptcyByCodeResponseModel
    {/// <summary>
    /// ???
    /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public List<GetBankruptcyByCodeModelAnswerData> Data { get; set; }
    }
    /// <summary>
    /// ???
    /// </summary>
    public class GetBankruptcyByCodeModelAnswerData
    {/// <summary>
    /// ???
    /// </summary>
        public List<GetBankruptcyByCodeData> VgsuData { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Code { get; set; }
    }
    /// <summary>
    /// Дані ВГСУ
    /// </summary>
    public class GetBankruptcyByCodeData                                                
    {/// <summary>
     /// Номер судового рішення (maxLength:64)
     /// </summary>
        public string CaseNumber { get; set; }                                          
        /// <summary>
        /// Назва суду (maxLength:256)
        /// </summary>
        public string CourtName { get; set; }                                           
        /// <summary>
        /// Дата публікації
        /// </summary>
        public DateTime? DateProclamation { get; set; }                                 
        /// <summary>
        /// (maxLength:64)
        /// </summary>
        public string Edrpou { get; set; }                                              
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? EndDate { get; set; }                                          
        /// <summary>
        /// Посилання на ВГСУ (maxLength:256)
        /// </summary>
        public string Link { get; set; }                                                
        /// <summary>
        /// (maxLength:512)
        /// </summary>
        public string NameDebtor { get; set; }                                          
        /// <summary>
        /// (maxLength:64)
        /// </summary>
        public string NumberAdvert { get; set; }                                        
        /// <summary>
        /// Тип інформації (maxLength:128)
        /// </summary>
        public string PublicationType { get; set; }                                     
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? StartDate { get; set; }                                        
    }


}
