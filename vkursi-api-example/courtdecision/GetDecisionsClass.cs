using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionsClass
    {
        /*

        10. Запит на отримання переліку судових документів організації за критеріями (контент та параметри документа можна отримати в методі /api/1.0/courtdecision/getdecisionbyid)
        [POST] /api/1.0/courtdecision/getdecisions
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisions' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cC...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":"14360570","Skip":0,"TypeSide":null,"JusticeKindId":null,"Npas":["F545D851-6015-455D-BFE7-01201B629774"]}'

        */

        public static GetDecisionsResponseModel GetDecisions(string edrpou, int? skip, int? typeSide, int? justiceKindId, List<string> npas, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDecisionsRequestBodyModel Body = new GetDecisionsRequestBodyModel
                {
                    Edrpou = edrpou,                                            // Код ЄДРПОУ
                    Skip = skip,                                                // К-ть документів які необхідно пропустити (щоб взяти наступні 100)
                    TypeSide = typeSide,                                        // Тип сторони в судомому документі
                    JusticeKindId = justiceKindId,                              // Форма судочинства
                    Npas = npas                                                 // Фільтр по статтям до НПА
                };

                string body = JsonConvert.SerializeObject(Body);                // Example body: {"Edrpou":"14360570","Skip":0,"TypeSide":null,"JusticeKindId":null,"Npas":["F545D851-6015-455D-BFE7-01201B629774"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisions");
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
                    Console.WriteLine("За вказаними параметрами інформації не знайдено");
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

            GetDecisionsResponseModel DecisionsResponseRow = new GetDecisionsResponseModel();

            DecisionsResponseRow = JsonConvert.DeserializeObject<GetDecisionsResponseModel>(responseString);

            return DecisionsResponseRow;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"21560045\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getadvancedorganization", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":\"14360570\",\"Skip\":0,\"TypeSide\":null,\"JusticeKindId\":null,\"Npas\":[\"F545D851-6015-455D-BFE7-01201B629774\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisions")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsIn...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetDecisionsRequestBodyModel                                   // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// К-ть документів які необхідно пропустити (щоб взяти наступні 100)
        /// </summary>
        public int? Skip { get; set; }                                          // 
        /// <summary>
        /// null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }                                      // 
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }                                 // 
        /// <summary>
        /// Id статті НПА (можна отримати в розробника)
        /// </summary>
        public List<string> Npas { get; set; }                                  // 
    }
    /// <summary>
    /// Модель відповіді GetDecisions
    /// </summary>
    public class GetDecisionsResponseModel                                      // 
    {/// <summary>
     /// Загальна кількість судових документів
     /// </summary>
        public double totalDecision { get; set; }                               // 
        /// <summary>
        /// Перелік Id судових документів
        /// </summary>
        public List<ListDecisions> list { get; set; }                           // 
    }/// <summary>
     /// Перелік Id судових документів
     /// </summary>

    public class ListDecisions                                                  // 
    {/// <summary>
     /// Id документа
     /// </summary>
        public int id { get; set; }                                             // 
        /// <summary>
        /// Дата судовога засідання
        /// </summary>
        public DateTime adjudicationDate { get; set; }                          // 
    }
}
