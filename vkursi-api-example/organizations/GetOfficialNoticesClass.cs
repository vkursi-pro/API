using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetOfficialNoticesClass
    {
        /*
         
        44. Офіційні повідомлення (ЄДР, SMIDA, Банкрутство)
        [POST] /api/1.0/organizations/getofficialnotices

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getofficialnotices' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["00131305"]}'

        */

        public static GetOfficialNoticesResponseModel GetOfficialNotices(string token, string edrpou)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOfficialNoticesRequestBodyModel GONRequestBody = new GetOfficialNoticesRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        edrpou
                    }
                };

                string body = JsonConvert.SerializeObject(GONRequestBody);

                // Example Body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getofficialnotices");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
                }

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetOfficialNoticesResponseModel GONResponseRow = new GetOfficialNoticesResponseModel();

            GONResponseRow = JsonConvert.DeserializeObject<GetOfficialNoticesResponseModel>(responseString);

            return GONResponseRow;
        }
    }

    /*
     
        // Python - http.client example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getofficialnotices")
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
        payload = "{\"Edrpou\":[\"00131305\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getofficialnotices", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */


    public class GetOfficialNoticesRequestBodyModel                                     // Модель запиту 
    {
        public List<string> Edrpou { get; set; }                                        // Перелік кодів ЄДРПОУ (обмеження 1)
    }

    public class GetOfficialNoticesResponseModel                                        // Модель на відповідь
    {
        public bool IsSucces { get; set; }                                              // Статус відповіді по API
        public string Succes { get; set; }                                              // Чи успішний запит (maxLength:128)
        public List<GetOfficialNoticesResponseModelData> Data { get; set; }             // Перелік даних
    }

    public class GetOfficialNoticesResponseModelData                                    // Перелік даних
    {
        public string Edrpou { get; set; }                                              // Код ЄДРПОУ (maxLength:12)
        public List<ApiOrganizationMessageAnswerModelDataVgsu> Vgsu { get; set; }       // Дані ВГСУ
        public List<ApiOrganizationMessageAnswerModelDataEdr> Edr { get; set; }         // Дані ЄДР
        public List<ApiOrganizationMessageAnswerModelDataSMIDA> Smida { get; set; }     // Дані SMIDA
    }

    public class ApiOrganizationMessageAnswerModelDataVgsu                              // Дані ВГСУ
    {
        public string CaseNumber { get; set; }                                          // Номер судового рішення (maxLength:64)
        public string CourtName { get; set; }                                           // Назва суду (maxLength:256)
        public DateTime? DateProclamation { get; set; }                                 // Дата пудлікації
        public string Edrpou { get; set; }                                              // (maxLength:64)
        public DateTime? EndDate { get; set; }                                          // 
        public string Link { get; set; }                                                // Посилання на ВГСУ (maxLength:256)
        public string NameDebtor { get; set; }                                          // (maxLength:512)
        public string NumberAdvert { get; set; }                                        // (maxLength:64)
        public string PublicationType { get; set; }                                     // Тип інформації (maxLength:128)
        public DateTime? StartDate { get; set; }                                        // 
    }

    public class ApiOrganizationMessageAnswerModelDataEdr                               // Дані ЄДР
    {
        public string RegistrationAction { get; set; }                                  // Тип події (maxLength:256)
        public DateTime? DateActual { get; set; }                                       // Дата події
        public string ReorganizationType { get; set; }                                  // Тип реорганізаційної події (maxLength:128)
        public string NameLiquidator { get; set; }                                      // (maxLength:256)
        public string NameLiquidationCommission { get; set; }                           // (maxLength:256)
        public DateTime? CreateDate { get; set; }                                       // 
    }

    public class ApiOrganizationMessageAnswerModelDataSMIDA                             // Дані SMIDA
    {
        public string Edrpou { get; set; }                                              // Код ЄДРПОУ (maxLength:12)
        public string Name { get; set; }                                                // Назва (maxLength:512)
        public DateTime? Std { get; set; }                                              // 
        public DateTime? Fid { get; set; }                                              // 
        public bool Nreg { get; set; }                                                  // 
        public string Ttype { get; set; }                                               // (maxLength:64)
        public int? IdRow { get; set; }                                                 // 
        public DateTime? Timestamp { get; set; }                                        // Дата виникнення події
        public string Href { get; set; }                                                // Посинання на SMIDA (maxLength:256)
        public string InformationType { get; set; }                                     // Тип інформації (maxLength:256)
        public string Period { get; set; }                                              // Періодичність подання (maxLength:64)
    }

}