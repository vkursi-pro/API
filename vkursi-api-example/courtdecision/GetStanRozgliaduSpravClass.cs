using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.courtdecision
{
    public class GetStanRozgliaduSpravClass
    {

        /*

        // 51. Судові документи по ЮО/ФО
        // [POST] api/1.0/CourtDecision/getStanRozgliaduSprav

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/CourtDecision/getStanRozgliaduSprav' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"CompanyEdrpou":"42721869","Size":100}'

        */


        public static GetStanRozgliaduSpravResponseModel GetStanRozgliaduSprav(ref string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
             GetStanRozgliaduSpravBodyModel GSRSBody = new GetStanRozgliaduSpravBodyModel
                {
                    //CompanyEdrpou = "31729918",
                    PersonName = "Колесник Анна Олексіївна",
                    Size = 100,    
                    //DateStart = DateTime.Parse("2020-04-13T00:00:00.000Z")
                };

                string body = JsonConvert.SerializeObject(GSRSBody);

                // Example Body: {"CompanyEdrpou":"42721869","Size":100}

                //body = "{\"CompanyEdrpou\":\"42721869\",\"Size\":100}";

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/CourtDecision/getStanRozgliaduSprav");
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

            GetStanRozgliaduSpravResponseModel GSRSResponse = new GetStanRozgliaduSpravResponseModel();

            GSRSResponse = JsonConvert.DeserializeObject<GetStanRozgliaduSpravResponseModel>(responseString);

            return GSRSResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"CompanyEdrpou\":\"42721869\",\"Size\":100}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVC...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/CourtDecision/getStanRozgliaduSprav", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:  

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"CompanyEdrpou\":\"42721869\",\"Size\":100}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/CourtDecision/getStanRozgliaduSprav")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVC...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();
    
    
    */

    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetStanRozgliaduSpravBodyModel                                         // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string PersonName { get; set; }                                          // 
        /// <summary>
        /// ЄДРПОУ
        /// </summary>
        public string CompanyEdrpou { get; set; }                                       // 
        /// <summary>
        /// Кількість
        /// </summary>
        public int Size { get; set; }                                                   // 
        /// <summary>
        /// Дата від
        /// </summary>
        public DateTime DateStart { get; set; }                                         // 
        /// <summary>
        /// null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }                                              // 
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }                                         // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetStanRozgliaduSpravResponseModel                                     // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Код відповіді по API
        /// </summary>
        public string Code { get; set; }                                                // 
        /// <summary>
        /// Максимальна дата
        /// </summary>
        public DateTime? MaxDate { get; set; }                                          // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<GetStanRozgliaduSpravDataModel> Data { get; set; }                  // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class GetStanRozgliaduSpravDataModel                                         // 
    {/// <summary>
     /// Назва суду
     /// </summary>
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Єдиний унікальний номер справи
        /// </summary>
        public string CaseNumber { get; set; }                                          // 
        /// <summary>
        /// № провадження
        /// </summary>
        public string CaseProc { get; set; }                                            // 
        /// <summary>
        /// Дата рішення
        /// </summary>
        public DateTime? RegistrationDate { get; set; }                                 // 
        /// <summary>
        /// Головуючий суддя
        /// </summary>
        public string Judge { get; set; }                                               // 
        /// <summary>
        /// Суддя-доповідач
        /// </summary>
        public string Judges { get; set; }                                              // 
        /// <summary>
        /// Учасники
        /// </summary>
        public string Participants { get; set; }                                        // 
        /// <summary>
        /// Дата наступної події
        /// </summary>
        public DateTime? StageDate { get; set; }                                        // 
        /// <summary>
        /// Стадія розгляду
        /// </summary>
        public string StageName { get; set; }                                           // 
        /// <summary>
        /// Результат
        /// </summary>
        public string CauseResult { get; set; }                                         // 
        /// <summary>
        /// Тип заяви
        /// </summary>
        public string Type { get; set; }                                                // 
        /// <summary>
        /// Предмет позову
        /// </summary>
        public string Description { get; set; }                                         // 
        /// <summary>
        /// Ид документа на Vsudi пример https://vkursi.pro/vsudi/decision/90283013
        /// </summary>
        public string DecisionId { get; set; }                                          // 
        /// <summary>
        /// null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }                                              //  
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }                                         //  
    }
}
