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
        --header 'Authorization: Bearer eyJhbGciOiJ...' \
        --header 'Content-Type: application/json' \
        --data-raw '"88234097"'

        */
        public static GetDecisionByIdResponseModel GetDecisionById(string courtDecisionId, string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid");
                RestRequest request = new RestRequest(Method.POST);

                string body = "\"" + courtDecisionId + "\"";

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
        payload = "\"88234097\""
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/courtdecision/getdecisionbyid", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "88234097"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cC...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/courtdecision/getdecisionbyid", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */

    public class GetDecisionByIdResponseModel                                           // Модель запиту 
    {
        public bool IsSuccess { get; set; }                                             // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public CourtDecisionElasticModel Data { get; set; }                             // Модель на відповідь
    }

    public class CourtDecisionElasticModel                                              // Модель на відповідь
    {
        public DateTime AdjudicationDate { get; set; }                                  // Дата ухвалення рішення 
        public DateTime DateCreated { get; set; }                                       // Дата імпорту документа до сервісу
        public int CauseCategoryId { get; set; }                                        // Категорія справи (додати довіник)
        public Content Content { get; set; }                                            // Контент судового документа 
        public int CourtId { get; set; }                                                // Id суду (додати довіник)
        public DateTime DatePublished { get; set; }                                     // Дата офіційної публікації документа
        public int Id { get; set; }                                                     // Id документа
        public int InstanceId { get; set; }                                             // Id інстанції (1 - Касаційна, 2 - Апеляційна, 3 - Перша)
        public Judge Judge { get; set; }                                                // Судді
        public int JudgmentFormId { get; set; }                                         // Форма рішеня (1 - Вирок, 2 - Постанова, 3 - Рішення, 4 - Судовий наказ, 5 - Ухвала, 6 - Окрема ухвала, 10 - Окрема думка)
        public int JusticeKindId { get; set; }                                          // null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        public string Number { get; set; }                                              // Номер документа
        public DateTime ReceiptDate { get; set; }                                       // Дата надходження до реестра
        public int RegionId { get; set; }                                               // Регіон
        public ResultPart ResultPart { get; set; }                                      // Результат
        public string Theme { get; set; }                                               // Тема засідання
        public string Url { get; set; }                                                 // Посилання
        public int? SideKasType { get; set; }                                           // Системний Id Vkursi
        public List<string> NPA { get; set; }                                           // НПА в документі
        public int? IsCompressed { get; set; }                                          // Системний Id Vkursi
        public AmountClime AmountClaim { get; set; }                                    // Сума позову
        public string[] CriminalNumb { get; set; }                                      // Криминальні провадження
        public string[] CadastralNumb { get; set; }                                     // Кадастрові номери

        public bool? ContainAppeal { get; set; }                                        // Наявність касацій (за номером документа)
        public bool? ContainCassation { get; set; }                                     // Наявність апеляцій (за номером документа)
        public List<SidesAllModel> SidesAllNested { get; set; }                         // Сторони в справи
    }

    public class SidesAllModel                                                          // Сторони в справи
    {
        public int Type { get; set; }                                                   // Тип (2 - фізична / 1 - юридична особа)
        public bool IsActive { get; set; }                                              // Системний id Vkursi
        public int? FindParam { get; set; }                                             // Системний id Vkursi
        public int? KvedCodeInt { get; set; }                                           // Системний id Vkursi
        public int? WinnerState { get; set; }                                           // Системний id Vkursi
        public int? CategoryId { get; set; }                                            // Системний id Vkursi
        public int SidesType { get; set; }                                              // Тип сторони (1 - Позивачі, 2 - Відповідачі, 3 - Касаційні позивачі, 4 - Інші сторони)
        public int? RegNumberInt { get; set; }                                          // Код ЕДРПОУ (в int)
        public string Name { get; set; }                                                // Назва сторони (якщо розпізнана)
        public int? Error { get; set; }                                                 // Назва сторони (якщо не розпізнана)
    }

    public class AmountClime                                                            // Сума позову
    {
        public double Amount { get; set; }                                              // Сума
        public string Currency { get; set; }                                            // Валюта
    }

    public class ResultPart                                                             // Результат
    {
        public string[] Result { get; set; }                                            // Результат
        public int Winner { get; set; }                                                 // Системний id Vkursi
    }

    public class Judge                                                                  // Суді
    {
        public string Name { get; set; }                                                // ПІБ судді
        public int Role { get; set; }                                                   // Системний id Vkursi
    }

    public class Content                                                                // Контент
    {
        public string Footer { get; set; }                                              // Розпізнана резолютивна частина
        public string Header { get; set; }                                              // Розпізнана шапка судового документа
        public string Middle { get; set; }                                              // Розпізнана мотивувальна
        public string NotFound { get; set; }                                            // Не розпізнаний
    }
}
