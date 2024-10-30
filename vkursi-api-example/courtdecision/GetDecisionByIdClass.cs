using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionByIdClass
    {
        /*
         
        26. Рекізити судового документа
        [POST] /api/1.0/courtdecision/getdecisionbyid

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Id":88234097}'

        */
        public static GetDecisionByIdResponseModel GetDecisionById(long courtDecisionId, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid");
                RestRequest request = new RestRequest(Method.POST);

                GetDecisionByIdBodyModel GDBIBody = new GetDecisionByIdBodyModel
                {
                    Id = courtDecisionId                                                // Id судового документа
                };

                string body = JsonConvert.SerializeObject(GDBIBody);

                // Example Body: {"Id":88234097}

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

            GetDecisionByIdResponseModel GDBIResponseRow = new GetDecisionByIdResponseModel();

            GDBIResponseRow = JsonConvert.DeserializeObject<GetDecisionByIdResponseModel>(responseString);

            return GDBIResponseRow;
        }
    }


    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Id\":88234097}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/courtdecision/getdecisionbyid", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Id\":88234097}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiI...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */

    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetDecisionByIdBodyModel                                               // 
    {/// <summary>
     /// Id судового документа
     /// </summary>
        public long Id { get; set; }                                                    // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetDecisionByIdResponseModel                                           // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дані судового документа
        /// </summary>
        public CourtDecisionElasticModel Data { get; set; }                             // 
    }
    /// <summary>
    /// Дані судового документа
    /// </summary>
    public class CourtDecisionElasticModel                                              // 
    {/// <summary>
     /// Дата ухвалення рішення 
     /// </summary>
        public DateTime AdjudicationDate { get; set; }                                  // 
        /// <summary>
        /// Дата імпорту документа до сервісу
        /// </summary>
        public DateTime DateCreated { get; set; }                                       // 
        /// <summary>
        /// Категорія справи (https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/dictionary/jsonFiles/causeCategory.json)
        /// </summary>
        public int CauseCategoryId { get; set; }                                        // 
        /// <summary>
        /// Контент судового документа 
        /// </summary>
        public Content Content { get; set; }                                            // 
        /// <summary>
        /// Id суду (https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/dictionary/jsonFiles/court.json)
        /// </summary>
        public int CourtId { get; set; }                                                // 
        /// <summary>
        /// Дата оф
        /// іційної публікації документа
        /// </summary>
        public DateTime DatePublished { get; set; }                                     // 
        /// <summary>
        /// Id документа
        /// </summary>
        public int Id { get; set; }                                                     // 
        /// <summary>
        /// Id інстанції (1 - Касаційна, 2 - Апеляційна, 3 - Перша)
        /// </summary>
        public int InstanceId { get; set; }                                             // 
        /// <summary>
        /// Судді
        /// </summary>
        public Judge Judge { get; set; }                                                // 
        /// <summary>
        /// Форма рішеня (1 - Вирок, 2 - Постанова, 3 - Рішення, 4 - Судовий наказ, 5 - Ухвала, 6 - Окрема ухвала, 10 - Окрема думка)
        /// </summary>
        public int JudgmentFormId { get; set; }                                         // 
        /// <summary>
        /// null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int JusticeKindId { get; set; }                                          // 
        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Дата надходження до реестра
        /// </summary>
        public DateTime ReceiptDate { get; set; }                                       // 
        /// <summary>
        /// Регіон
        /// </summary>
        public int RegionId { get; set; }                                               // 
        /// <summary>
        /// Результат
        /// </summary>
        public ResultPart ResultPart { get; set; }                                      // 
        /// <summary>
        /// Тема засідання
        /// </summary>
        public string Theme { get; set; }                                               // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Url { get; set; }                                                 // 
        /// <summary>
        /// Системний Id Vkursi
        /// </summary>
        public int? SideKasType { get; set; }                                           // 
        /// <summary>
        /// НПА в документі
        /// </summary>
        public List<string> NPA { get; set; }                                           // 
        /// <summary>
        /// Системний Id Vkursi
        /// </summary>
        public int? IsCompressed { get; set; }                                          // 
        /// <summary>
        /// Сума позову
        /// </summary>
        public AmountClime AmountClaim { get; set; }                                    // 
        /// <summary>
        /// Криминальні провадження
        /// </summary>
        public string[] CriminalNumb { get; set; }                                      // 
        /// <summary>
        /// Кадастрові номери
        /// </summary>
        public string[] CadastralNumb { get; set; }                                     // 
        /// <summary>
        /// Наявність апеляцій (за номером документа)
        /// </summary>
        public bool? ContainAppeal { get; set; }                                        // 
        /// <summary>
        /// Наявність касацій (за номером документа)
        /// </summary>
        public bool? ContainCassation { get; set; }                                     // 
        /// <summary>
        /// Сторони в справи
        /// </summary>
        public List<SidesAllModel> SidesAllNested { get; set; }                         // 
    }
    /// <summary>
    /// Сторони в справи
    /// </summary>
    public class SidesAllModel                                                          // 
    {/// <summary>
     /// Тип (2 - фізична / 1 - юридична особа)
     /// </summary>
        public int Type { get; set; }                                                   // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public bool IsActive { get; set; }                                              // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int? FindParam { get; set; }                                             // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int? KvedCodeInt { get; set; }                                           // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int? WinnerState { get; set; }                                           // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int? CategoryId { get; set; }                                            // 
        /// <summary>
        /// Тип сторони (1 - Позивачі, 2 - Відповідачі, 3 - Касаційні позивачі, 4 - Інші сторони)
        /// </summary>
        public int SidesType { get; set; }                                              // 
        /// <summary>
        /// Код ЕДРПОУ (в int)
        /// </summary>
        public int? RegNumberInt { get; set; }                                          // 
        /// <summary>
        /// Назва сторони (якщо розпізнана)
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Назва сторони (якщо не розпізнана)
        /// </summary>
        public int? Error { get; set; }                                                 // 
    }
     /// <summary>
     /// Сума позову
     /// </summary>
    public class AmountClime                                                            // 
    {   /// <summary>
        /// Сума
        /// </summary>
        public double Amount { get; set; }                                              // 
        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }                                            // 
    }
    /// <summary>
    /// Результат
    /// </summary>
    public class ResultPart                                                             // 
    {/// <summary>
     /// Результат
     /// </summary>
        public string[] Result { get; set; }                                            // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int Winner { get; set; }                                                 // 
    }
    /// <summary>
    /// Суді
    /// </summary>
    public class Judge                                                                  // 
    {/// <summary>
     /// ПІБ судді
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int Role { get; set; }                                                   // 
    }
    /// <summary>
    /// Контент
    /// </summary>
    public class Content                                                                // 
    {/// <summary>
     /// Розпізнана резолютивна частина
     /// </summary>
        public string Footer { get; set; }                                              // 
        /// <summary>
        /// Розпізнана шапка судового документа
        /// </summary>
        public string Header { get; set; }                                              // 
        /// <summary>
        /// Розпізнана мотивувальна
        /// </summary>
        public string Middle { get; set; }                                              // 
        /// <summary>
        /// Не розпізнаний
        /// </summary>
        public string NotFound { get; set; }                                            // 
    }
}
