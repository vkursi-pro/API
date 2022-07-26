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
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
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

    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetOfficialNoticesRequestBodyModel                                     // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ (обмеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                        // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetOfficialNoticesResponseModel                                        // 
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public bool IsSucces { get; set; }                                              // 
        /// <summary>
        /// Чи успішний запит (maxLength:128)
        /// </summary>
        public string Succes { get; set; }                                              // 
        /// <summary>
        /// Перелік даних
        /// </summary>
        public List<GetOfficialNoticesResponseModelData> Data { get; set; }             // 
    }
    /// <summary>
    /// Перелік даних
    /// </summary>
    public class GetOfficialNoticesResponseModelData                                    // 
    {/// <summary>
     /// Код ЄДРПОУ (maxLength:12)
     /// </summary>
        public string Edrpou { get; set; }                                              // 
        /// <summary>
        /// Дані ВГСУ 
        /// </summary>
        public List<ApiOrganizationMessageAnswerModelDataVgsu> Vgsu { get; set; }       // 
        /// <summary>
        /// Дані ЄДР
        /// </summary>
        public List<ApiOrganizationMessageAnswerModelDataEdr> Edr { get; set; }         // 
        /// <summary>
        /// Дані SMIDA
        /// </summary>
        public List<ApiOrganizationMessageAnswerModelDataSMIDA> Smida { get; set; }     // 
    }
    /// <summary>
    /// Дані ВГСУ
    /// </summary>
    public class ApiOrganizationMessageAnswerModelDataVgsu                              // 
    {/// <summary>
     /// Номер судового рішення (maxLength:64)
     /// </summary>
        public string CaseNumber { get; set; }                                          // 
        /// <summary>
        /// Назва суду (maxLength:256)
        /// </summary>
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Дата пудлікації
        /// </summary>
        public DateTime? DateProclamation { get; set; }                                 // 
        /// <summary>
        /// (maxLength:64)
        /// </summary>
        public string Edrpou { get; set; }                                              // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? EndDate { get; set; }                                          // 
        /// <summary>
        /// Посилання на ВГСУ (maxLength:256)
        /// </summary>
        public string Link { get; set; }                                                // 
        /// <summary>
        /// (maxLength:512)
        /// </summary>
        public string NameDebtor { get; set; }                                          // 
        /// <summary>
        /// (maxLength:64)
        /// </summary>
        public string NumberAdvert { get; set; }                                        // 
        /// <summary>
        /// Тип інформації (maxLength:128)
        /// </summary>
        public string PublicationType { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? StartDate { get; set; }                                        // 
    }
    /// <summary>
    /// Дані ЄДР
    /// </summary>
    public class ApiOrganizationMessageAnswerModelDataEdr                               // 
    {/// <summary>
     /// Тип події (maxLength:256)
     /// </summary>
        public string RegistrationAction { get; set; }                                  // 
        /// <summary>
        /// Дата події
        /// </summary>
        public DateTime? DateActual { get; set; }                                       // 
        /// <summary>
        /// Тип реорганізаційної події (maxLength:128)
        /// </summary>
        public string ReorganizationType { get; set; }                                  // 
        /// <summary>
        /// (maxLength:256)
        /// </summary>
        public string NameLiquidator { get; set; }                                      // 
        /// <summary>
        /// (maxLength:256)
        /// </summary>
        public string NameLiquidationCommission { get; set; }                           // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? CreateDate { get; set; }                                       // 
    }
    /// <summary>
    /// Дані SMIDA
    /// </summary>
    public class ApiOrganizationMessageAnswerModelDataSMIDA                             // 
    {/// <summary>
     /// Код ЄДРПОУ (maxLength:12)
     /// </summary>
        public string Edrpou { get; set; }                                              // 
        /// <summary>
        /// Назва (maxLength:512)
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? Std { get; set; }                                              // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime? Fid { get; set; }                                              // 
        /// <summary>
        /// ???
        /// </summary>
        public bool Nreg { get; set; }                                                  // 
        /// <summary>
        /// (maxLength:64)
        /// </summary>
        public string Ttype { get; set; }                                               // 
        /// <summary>
        /// ???
        /// </summary>
        public int? IdRow { get; set; }                                                 // 
        /// <summary>
        /// Дата виникнення події
        /// </summary>
        public DateTime? Timestamp { get; set; }                                        // 
        /// <summary>
        /// Посинання на SMIDA (maxLength:256)
        /// </summary>
        public string Href { get; set; }                                                // 
        /// <summary>
        /// Тип інформації (maxLength:256)
        /// </summary>
        public string InformationType { get; set; }                                     // 
        /// <summary>
        /// Періодичність подання (maxLength:64)
        /// </summary>
        public string Period { get; set; }                                              // 
    }

}