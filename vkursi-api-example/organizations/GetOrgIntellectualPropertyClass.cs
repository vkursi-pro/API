using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgIntellectualPropertyClass
    {
        /*

        38. Відомості про інтелектуальну власність (патенти, торгові марки, корисні моделі) які повязані по ПІБ з бенеціціарами підприємства
        [POST] /api/1.0/organizations/getorgintellectualproperty       

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgintellectualproperty' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["22962627"],"SkipTradeMarks":null,"TakeTradeMarks":null,"SkipPatents":null,"TakePatents":null,"SkipUsefullModels":null,"TakeUsefullModels":null}'
         
        */

        public static GetOrgIntellectualPropertyResponseModel GetOrgIntellectualProperty(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgIntellectualPropertyRequestBodyModel GOIPRequestBody = new GetOrgIntellectualPropertyRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOIPRequestBody);      // Example body: {"Edrpou":["22962627"],"SkipTradeMarks":null,"TakeTradeMarks":null,"SkipPatents":null,"TakePatents":null,"SkipUsefullModels":null,"TakeUsefullModels":null}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgintellectualproperty");
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

            GetOrgIntellectualPropertyResponseModel GOIPResponse = new GetOrgIntellectualPropertyResponseModel();

            GOIPResponse = JsonConvert.DeserializeObject<GetOrgIntellectualPropertyResponseModel>(responseString);

            return GOIPResponse;
        }
    }

    /*
     
        // Python - http.client example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"22962627\"],\"SkipTradeMarks\":null,\"TakeTradeMarks\":null,\"SkipPatents\":null,\"TakePatents\":null,\"SkipUsefullModels\":null,\"TakeUsefullModels\":null}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgintellectualproperty")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();


        // Java - OkHttp example:
        
        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"22962627\"],\"SkipTradeMarks\":null,\"TakeTradeMarks\":null,\"SkipPatents\":null,\"TakePatents\":null,\"SkipUsefullModels\":null,\"TakeUsefullModels\":null}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getorgintellectualproperty", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */


    public class GetOrgIntellectualPropertyRequestBodyModel                     // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
        public int? SkipTradeMarks { get; set; }                                // Кількість записів ТМ які будуть пропущені
        public int? TakeTradeMarks { get; set; }                                // Кількість записів ТМ які будуть отримані
        public int? SkipPatents { get; set; }                                   // Кількість записів патентів які будуть пропущені
        public int? TakePatents { get; set; }                                   // Кількість записів патентів які будуть отримані
        public int? SkipUsefullModels { get; set; }                             // Кількість записів кориснх моделей які будуть пропущені
        public int? TakeUsefullModels { get; set; }                             // Кількість записів кориснх моделей які будуть отримані
    }

    public class GetOrgIntellectualPropertyResponseModel                        // Модель відповіді GetOrgVehicle
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API (maxLength:128)
        public List<OrgIntellectualPropertyApiAnswerModelData> Data { get; set; }   // Дані
    }

    public class OrgIntellectualPropertyApiAnswerModelData                      // Дані
    {
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ (maxLength:12)
        public int TotalTradeMarksCount { get; set; }                                                   // Кількість торгових марок
        public List<OrgIntellectualPropertyApiAnswerModelDataTradeMark> TradeMarks { get; set; }        // Торгові марки
        public int TotalPatentsCount { get; set; }                                                      // Кількість патентів
        public List<OrgIntellectualPropertyApiAnswerModelDataPatent> Patents { get; set; }              // Патенти
        public int TotalUsefullModelsCount { get; set; }                                                // Кількість кориснх моделей
        public List<OrgIntellectualPropertyApiAnswerModelDataPatent> UsefullModels { get; set; }        // Корисні моделі
    }
    public class OrgIntellectualPropertyApiAnswerModelDataTradeMark             // Торгові марки
    {
        public string RegistrationNumber { get; set; }                          // Реєстраційний номер (maxLength:64)
        public DateTime? RegistrationDate { get; set; }                         // Дата реєстрації
        public string ApplicationNumber { get; set; }                           // (maxLength:64)
        public DateTime? ApplicationDate { get; set; }                          // 
        public string Owner { get; set; }                                       // Власники (maxLength:512)
        public List<int> ICTPIndexes { get; set; }                              // Індекс МКТП 
    }

    public class OrgIntellectualPropertyApiAnswerModelDataPatent                // Патенти / Корисні моделі
    {
        public string Number { get; set; }                                      // Реєстраційний номер  (maxLength:256)
        public string OwnerNames { get; set; }                                  // Власники (maxLength:512)
        public string Name { get; set; }                                        // Назва (патента / корисної моделі) (maxLength:512)
        public DateTime? RegistrationDate { get; set; }                         // Дата реєстрації
    }
}
