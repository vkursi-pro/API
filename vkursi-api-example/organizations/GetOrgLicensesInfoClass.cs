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
                        edrpou                                                    // Код ЄДРПОУ аба ІПН
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

    public class GetOrgLicensesInfoRequestBodyModel                             // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
    }

    public class GetOrgLicensesInfoResponseModel                                // Модель відповіді GetOrgLicensesInfo
    {
        public bool IsSucces { get; set; }                                      // Статус відповіді по API
        public string Status { get; set; }                                      // Чи успішний запит (maxLength:128)
        public List<OrgLicensesApiApiAnswerModelData> Data { get; set; }        // Дані
    }

    public class OrgLicensesApiApiAnswerModelData                               // Дані
    {
        public string Edrpou { get; set; }                                      // ЄДРПОУ / ІПН (maxLength:12)
        public List<OrgLicensesApiApiAnswerModelDataObject> Licenses { get; set; }      // Перелік ліцензій
    }
    public class OrgLicensesApiApiAnswerModelDataObject                         // Перелік ліцензій
    {
        public string Name { get; set; }                                        // Назва (maxLength:256)
        public string Number { get; set; }                                      // Номер (maxLength:64)
        public DateTime? DateEnd { get; set; }                                  // Дата закінчення
        public DateTime? DateStart { get; set; }                                // Дата початку
        public string State { get; set; }                                       // Стан (maxLength:32)
    }
}
