using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgLicensesInfoClass
    {
        /// <summary>
        /// 37. Перелік ліцензій, та дозволів
        /// [POST] /api/1.0/organizations/getorglicensesinfo
        /// </summary>
        /// <param name="token"></param>
        /// <param name="edrpou"></param>
        /// <returns></returns>
        
        /*
            cURL запиту:
                 curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorglicensesinfo' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1N' \
                --header 'Content-Type: application/json' \
                --data-raw '{"Edrpou":["00131305"]}'

            Приклад відповіді:
                https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetOrgLicensesInfoResponse.json
        */
        public static GetOrgLicensesInfoResponseModel GetOrgLicensesInfo(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgLicensesInfoRequestBodyModel GOLIRequestBody = new GetOrgLicensesInfoRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        edrpou                                                    // Код ЄДРПОУ або ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOLIRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorglicensesinfo");
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetOrgLicensesInfoResponseModel GOLIResponse = new GetOrgLicensesInfoResponseModel();

            GOLIResponse = JsonConvert.DeserializeObject<GetOrgLicensesInfoResponseModel>(responseString);

            return GOLIResponse;
        }
    }

    /*
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorglicensesinfo")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUz")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute(); 


        // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Edrpou": [
            "00131305"
          ]
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJ',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getorglicensesinfo", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
    */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetOrgLicensesInfoRequestBodyModel                             // 
    {/// <summary>
     /// Перелік ЄДРПОУ / ІПН (обмеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                // 
    }
    /// <summary>
    /// Модель відповіді GetOrgLicensesInfo
    /// </summary>
    public class GetOrgLicensesInfoResponseModel                                // 
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public bool IsSucces { get; set; }                                      // 
        /// <summary>
        /// Чи успішний запит (maxLength:128)
        /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<OrgLicensesApiApiAnswerModelData> Data { get; set; }        // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class OrgLicensesApiApiAnswerModelData                               // 
    {/// <summary>
     /// ЄДРПОУ / ІПН (maxLength:12)
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// Перелік ліцензій
        /// </summary>
        public List<OrgLicensesApiApiAnswerModelDataObject> Licenses { get; set; }      // 
    }/// <summary>
     /// Перелік ліцензій
     /// </summary>
    public class OrgLicensesApiApiAnswerModelDataObject                         // 
    {/// <summary>
     /// Назва (maxLength:256)
     /// </summary>
        public string Name { get; set; }                                        // 
        /// <summary>
        /// Номер (maxLength:64)
        /// </summary>
        public string Number { get; set; }                                      // 
        /// <summary>
        /// Дата закінчення
        /// </summary>
        public DateTime? DateEnd { get; set; }                                  // 
        /// <summary>
        /// Дата початку
        /// </summary>
        public DateTime? DateStart { get; set; }                                // 
        /// <summary>
        /// Стан (maxLength:32)
        /// </summary>
        public string State { get; set; }                                       // 
        /// <summary>
        /// Орган який видав ліцензію
        /// </summary>
        public string Publisher { get; set; }                                   // 

        /// <summary>
        /// Детальна інформація про ліцензщію 
        /// Містить динамічні поля в залежності від конкретного ресєтру/джерела походження. У зв'язку з чим, надати детальний опис моделі не надаєтсья можливим
        /// </summary>
        public object Info { get; set; }
    }
}
