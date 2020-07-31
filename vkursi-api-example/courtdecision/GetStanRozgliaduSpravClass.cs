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
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
             GetStanRozgliaduSpravBodyModel GSRSBody = new GetStanRozgliaduSpravBodyModel
                {
                    CompanyEdrpou = "42721869",
                    Size = 100,    
                    //DateStart = DateTime.Parse("2020-04-13T00:00:00.000Z")
                };

                string body = JsonConvert.SerializeObject(GSRSBody);

                // Example Body: {"CompanyEdrpou":"42721869","Size":100}

                body = "{\"CompanyEdrpou\":\"42721869\",\"Size\":100}";

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
                    token = AuthorizeClass.Authorize();
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


    public class GetStanRozgliaduSpravBodyModel                                         // Модель запиту 
    {
        public string PersonName { get; set; }                                          // ПІБ
        public string CompanyEdrpou { get; set; }                                       // ЄДРПОУ
        public int Size { get; set; }                                                   // Кількість
        public DateTime DateStart { get; set; }                                         // Дата від
        public int? TypeSide { get; set; }                                              // null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        public int? JusticeKindId { get; set; }                                         // null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
    }

    public class GetStanRozgliaduSpravResponseModel                                     // Модель на відповідь
    {
        public bool IsSuccess { get; set; }                                             // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public string Code { get; set; }                                                // Код відповіді по API
        public DateTime? MaxDate { get; set; }                                          // Максимальна дата
        public List<GetStanRozgliaduSpravDataModel> Data { get; set; }                  // Дані
    }

    public class GetStanRozgliaduSpravDataModel                                         // Дані
    {
        public string CourtName { get; set; }                                           // Назва суду
        public string CaseNumber { get; set; }                                          // Єдиний унікальний номер справи
        public string CaseProc { get; set; }                                            // № провадження
        public DateTime? RegistrationDate { get; set; }                                 // Дата рішення
        public string Judge { get; set; }                                               // Головуючий суддя
        public string Judges { get; set; }                                              // Суддя-доповідач
        public string Participants { get; set; }                                        // Учасники
        public DateTime? StageDate { get; set; }                                        // Дата наступної події
        public string StageName { get; set; }                                           // Стадія розгляду
        public string CauseResult { get; set; }                                         // Результат
        public string Type { get; set; }                                                // Тип заяви
        public string Description { get; set; }                                         // Предмет позову
        public string DecisionId { get; set; }                                          // Ид документа на Vsudi пример https://vkursi.pro/vsudi/decision/90283013
        public int? TypeSide { get; set; }                                              //  null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        public int? JusticeKindId { get; set; }                                         //  null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
    }
}
