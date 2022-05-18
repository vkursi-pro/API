using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetActivityOrgHistoryClass
    {
        /// <summary>
        /// 72. API з історії зміни даних в ЄДР [POST] /api/1.0/organizations/getactivityorghistory
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
            cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getactivityorghistory' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
                --header 'Content-Type: application/json' \
                --data-raw '{"Code":["00131305"]}'    
        
             Приклад відповіді:
                https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetActivityOrgHistoryResponse.json     
        */
        public static GetActivityOrgHistoryResponseModel GetActivityOrgHistory(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetActivityOrgHistoryRequestBodyModel GAOHRBody = new GetActivityOrgHistoryRequestBodyModel
                {
                    Code = new List<string> { code },
                };

                string body = JsonConvert.SerializeObject(GAOHRBody);  // Example: {"Codes":["21560766","3334800417"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getactivityorghistory");
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

            GetActivityOrgHistoryResponseModel GAOHResponse = new GetActivityOrgHistoryResponseModel();

            GAOHResponse = JsonConvert.DeserializeObject<GetActivityOrgHistoryResponseModel>(responseString);

            return GAOHResponse;
        }
    }

    /*
     
    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getactivityorghistory")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1N...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute(); 


    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Code": [
            "00131305"
          ]
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJI...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getactivityorghistory", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
    */

    public class GetActivityOrgHistoryRequestBodyModel                          // Модель запиту (Example: {"Codes":["21560766","3334800417"]})
    {
        public List<string> Code { get; set; }                                  // Перелік кодів ЕДРПОУ / ІПН
    }

    public class GetActivityOrgHistoryResponseModel                             // Модель відповіді
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API
        public List<ActivityOrgHistoryApiModelAnswerData> Data { get; set; }    // Дані методу
    }

    public class ActivityOrgHistoryApiModelAnswerData                           // Дані методу
    {
        public string Code { get; set; }                                        // Код ЕДРПОУ
        public bool KvedChange { get; set; }                                    // Чи змінювався КВЕД (за попередні 12 міс)
        public List<ActivityOrgHistoryApiModelAnswerDataChanges> ListKvedChange { get; set; } // Перелік записів про зміну КВЕД (за попередні 12 міс)
        public bool CompanyStructureChange { get; set; }                        // Чи були зміни в структурі власності (за попередні 12 міс)   
        public List<ActivityOrgHistoryApiModelAnswerDataChanges> ListCompanyStructureChange { get; set; }   // // Перелік записів про зміни в структурі власності (за попередні 12 міс)  
        public bool StatCapitalChangeToLower { get; set; }                      // Чи змінювався статутний капітал (за попередні 12 міс)  
        public List<ActivityOrgHistoryApiModelAnswerDataChanges> ListStatCapitalChangeToLower { get; set; } // Перелік записів про зміну статутного капіталу (за попередні 12 міс)  
        public bool ReorganizeChange { get; set; }                              // Реорганізація (за попередні 12 мес) 
        public List<ActivityOrgHistoryApiModelAnswerDataChanges> ListReorganizeChange { get; set; } // Перелік записів про реорганізаційні зміни (за попередні 12 мес) 
        public bool Error { get; set; }                                         // Перелік кодів ЕДРПОУ по яким метод відпрацював з помилкою
        public bool NotFound { get; set; }                                      // Перелік кодів ЕДРПОУ по яким компаній в ЕДР не знайдено
    }

    public class ActivityOrgHistoryApiModelAnswerDataChanges                    // Перелік записів про зімни
    {
        public Guid Id { get; set; }                                            // Унікальний id запису про зміну в системі Вкурсі
        public string CurrentValue { get; set; }                                // Попереднє значення
        public string PreviousValue { get; set; }                               // Нове значення
        public DateTime DataOfChange { get; set; }                              // Дата зміни
        public string ChangeType { get; set; }                                  // Тип зміни
    }
}
