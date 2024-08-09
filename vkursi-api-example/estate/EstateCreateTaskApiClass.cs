using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class EstateCreateTaskApiClass
    {
        /// <summary>
        /// 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам (додавання в чергу) та додавання об'єктів до моніторингу СМС РРП
        /// [POST] api/1.0/estate/estatecreatetaskapi
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="cadastrs"></param>
        /// <param name="koatuus"></param>
        /// <param name="edrpous"></param>
        /// <param name="ipns"></param>
        /// <param name="calculateCost"></param>
        /// <param name="isNeedUpdateAll"></param>
        /// <param name="taskName"></param>
        /// <param name="paramRequest"></param>
        /// <param name="getHistoricalData">Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)</param>
        /// <returns></returns>
        /* 

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Cadastrs":["5621287500:03:001:0019"],"CalculateCost":true,"IsNeedUpdateAll":false,"IsReport":true,"TaskName":"Назва задачі","DzkOnly":false}'

         */

        public static EstateCreateTaskApiResponseBodyModel EstateCreateTaskApi(
            ref string token, 
            List<string> cadastrs = null,
            List<string> koatuus = null,
            List<string> edrpous = null,
            List<string> ipns = null,
            bool calculateCost = true, 
            bool isNeedUpdateAll = false, 
            string taskName = "Назва задачі",
            EstateApiCreateTaskParamRequest paramRequest = null, 
            bool getHistoricalData = false)
        {
            if (string.IsNullOrEmpty(token)) 
            {
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi");
                RestRequest request = new RestRequest(Method.POST);

                EstateCreateTaskApiRequestBodyModel ECTARequestBodyRow = new EstateCreateTaskApiRequestBodyModel
                {
                    Cadastrs = cadastrs,                // Кадастрові номери
                    Koatuus = koatuus,                  // КОАТУУ (обмеження 10)
                    Edrpous = edrpous,                  // Коди ЄДРПОУ (обмеження 10)
                    Ipns = ipns,                        // Коди ІПН-и (обмеження 10)
                    CalculateCost = calculateCost,      // Якщо тільки порахувати вартість
                    IsNeedUpdateAll = isNeedUpdateAll,  // Якщо true - оновлюємо всі дані в ДЗК і РРП
                    TaskName = taskName,                // Назва задачі (обов'язково)
                    GetHistoricalData = getHistoricalData,
                    Param = paramRequest                // Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)
                    // isDzkOnly                        // 

                };


                string body = JsonConvert.SerializeObject(ECTARequestBodyRow, 
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                // body = "{\"Edrpous\":[\"33768131\"]}";

                // Example Body: {"Edrpous":["19124549"],"Ipns":["3083707142"],"Koatuus":["5621287500"],"Cadastrs":["5621287500:03:001:0019"],"CalculateCost":false,"IsNeedUpdateAll":false,"IsReport":true,"TaskName":"Назва задачі","DzkOnly":false}

                // Example Body with SMS RRP: {"Cadastrs":["5123955400:01:001:0590"],"CalculateCost":false,"InNeedUpdateAll":true,"IsReportTrue":true,"TaskName":"5123955400:01:001:0590","SmsRrpMonitoring":{"SmsRrpMonitoringType":1,"SmsRrpMonitoringTest":false,"SmsRrpMonitoringSections":{"AllInfo":true},"EndOfMonitoringDate":"2022-08-31T00:00:00"}}

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstateCreateTaskApiResponseBodyModel ECTAResponseBody = 
                JsonConvert.DeserializeObject<EstateCreateTaskApiResponseBodyModel>(responseString);

            return ECTAResponseBody;

            /*

                // Python - http.client example:

                import http.client
                import mimetypes
                conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
                payload = "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"CalculateCost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}"
                headers = {
                  'ContentType': 'application/json',
                  'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
                  'Content-Type': 'application/json',
                  'Cookie': 'ARRAffinity=60c7763e47a70e864d73874a4687c10eb685afc08af8bda506303f7b37b172b8'
                }
                conn.request("POST", "/api/1.0/estate/estatecreatetaskapi", payload, headers)
                res = conn.getresponse()
                data = res.read()
                print(data.decode("utf-8"))


                // Java - OkHttp example:

                OkHttpClient client = new OkHttpClient().newBuilder()
                  .build();
                MediaType mediaType = MediaType.parse("application/json");
                RequestBody body = RequestBody.create(mediaType, "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"CalculateCost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}");
                Request request = new Request.Builder()
                  .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi")
                  .method("POST", body)
                  .addHeader("ContentType", "application/json")
                  .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
                  .addHeader("Content-Type", "application/json")
                  .build();
                Response response = client.newCall(request).execute();

            */
        }
    }

    /// <summary>
    /// Модель Body запиту 19. EstateCreateTaskApi (Модель Body запиту)
    /// </summary>
    public class EstateCreateTaskApiRequestBodyModel                            // 
    {/// <summary>
     /// Коди ЄДРПОУ
     /// </summary>
        public List<string> Edrpous { get; set; }                               // 
        /// <summary>
        /// ІПН-и
        /// </summary>
        public List<string> Ipns { get; set; }                                  // 
        /// <summary>
        /// КОАТУУ
        /// </summary>
        public List<string> Koatuus { get; set; }                               // 
        /// <summary>
        /// Кадастрові номери
        /// </summary>
        public List<string> Cadastrs { get; set; }                              // 
        /// <summary>
        /// Якщо тільки порахувати вартість
        /// </summary>
        public bool? CalculateCost { get; set; }                                // 
        /// <summary>
        /// Якщо true - оновлюємо всі дані в ДЗК і РРП
        /// </summary>
        public bool IsNeedUpdateAll { get; set; }                               // 
        /// <summary>
        /// Назва задачі
        /// </summary>
        public string TaskName { get; set; }                                    // 
        /// <summary>
        /// Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)
        /// </summary>
        public EstateApiCreateTaskParamRequest Param { get; set; }              // 
        /// <summary>
        /// Дата закінчення моніторингу
        /// </summary>
        public DateTime? DateTimeEnd { get; set; }                              // 
        /// <summary>
        /// Перелік розділів що будуть моніторитись
        /// "0" - Всі дані по ділянці
        /// "1" - Загальні відомості
        /// "2" - Право власності
        /// "3" - Інше речове право
        /// "4" - Обтяження та  іпотека
        /// "5" - Намір про продаж
        /// "6" - Нерухомість на ділянці
        /// </summary>
        public List<string> SmsRrpMonitoringSectionsList { get; set; }          // 

        /// <summary>
        /// Додаткові фільтри по ділянкам
        /// </summary>
        public EstateApiCreateTaskRequestFilter Filter { get; set; }
        /// <summary>
        /// Параметри додавання до моніторингу
        /// </summary>
        public EstateApiCreateTaskRequestSmsRrp SmsRrpMonitoring { get; set; }
        /// <summary>
        /// Використовувати ТІЛЬКИ історичні дані (звіт буде сформовано тільки по ти м кадастромим по яки є історичні в сервісі). Застосомується якщо true
        /// </summary>
        public bool? GetHistoricalData { get; set; }
    }

    /// <summary>
    /// Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)
    /// </summary>
    public class EstateApiCreateTaskParamRequest                                // 
    {
        /// <summary>
        /// ДЗК (Державный земельний кадастр)
        /// </summary>
        public bool IsWithDzk { get; set; }                                     // 
        /// <summary>
        /// РРП (Державний реєстр речових прав на нерухоме майно)
        /// </summary>
        public bool IsWithRrp { get; set; }                                     // 
        /// <summary>
        /// ПККУ (Публічна кадастрова карта України)
        /// </summary>
        public bool IsWithPkku { get; set; }                                    // 
        /// <summary>
        /// НГО (Нормативна грошова оцінка)
        /// </summary>
        public bool IsWithNgo { get; set; }                                     // 
    }

    // 19. EstateCreateTaskApi (Модель відповіді)
    /// <summary>
    /// Модель відповіді EstateCreateTaskApi
    /// </summary>
    public class EstateCreateTaskApiResponseBodyModel                           // 
    {/// <summary>
     /// Запит віконано успішно
     /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// Повідомлення
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// Id задачі за яким ми будемо перевіряти її виконання
        /// </summary>
        public Guid taskId { get; set; }                                      // 
        /// <summary>
        /// Назва задачі
        /// </summary>
        public string taskName { get; set; }                                    // 
        /// <summary>
        /// Вартість виконання запиту
        /// </summary>
        public double? cost { get; set; }                                       // 
        public int? objectCount { get; set; }
        public int? DzkObjectCount { get; set; }
        public int? RrpObjectCount { get; set; }
        public int? NgoObjectCount { get; set; }
        public int? PkkuObjectCount { get; set; }
        public decimal? SmsRrpMonitoringCost { get; set; }
    }

    /// <summary>
    /// Додаткові фільтри по ділянкам
    /// </summary>
    public class EstateApiCreateTaskRequestFilter
    {
        /// <summary>
        /// Площа від
        /// </summary>
        public double? MinArea { get; set; }
        /// <summary>
        /// Площа до
        /// </summary>
        public double? MaxArea { get; set; }
        /// <summary>
        /// В межах населеного пункту
        /// </summary>
        public bool? WithInSetlement { get; set; }
        /// <summary>
        /// Цільове призначення
        /// </summary>
        public HashSet<int> PurposeIdList { get; set; }
        /// <summary>
        /// Форма власності
        /// </summary>
        public HashSet<int> OwnershipIdList { get; set; }
    }

    /// <summary>
    /// Параметри додавання до моніторингу
    /// </summary>
    public class EstateApiCreateTaskRequestSmsRrp
    {
        /// <summary>
        /// Тип моніторингу // 1 - Моніторинг по нормалізованим / 2 - Моніторинг по повним Керування моніторингом СМС РРП
        /// </summary>
        public int? SmsRrpMonitoringType { get; set; }
        /// <summary>
        /// Перелік даних які будуть моніторитись
        /// </summary>
        public EstateApiCreateTaskRequestSmsRrpMonitoringSections SmsRrpMonitoringSections { get; set; }
        /// <summary>
        /// Дата закінчення моніторингу
        /// </summary>
        public DateTime EndOfMonitoringDate { get; set; }
        /// <summary>
        /// Для тестоаого webhook-а
        /// </summary>
        public bool? SmsRrpMonitoringTest { get; set; }
    }

    /// <summary>
    /// Перелік даних які будуть моніторитись
    /// </summary>
    public class EstateApiCreateTaskRequestSmsRrpMonitoringSections
    {
        /// <summary>
        /// Всі дані по ділянці
        /// </summary>
        public bool AllInfo { get; set; }
        /// <summary>
        /// Загальні відомості
        /// </summary>
        public bool GeneralInfo { get; set; }
        /// <summary>
        /// Право власності
        /// </summary>
        public bool OwnershipInfo { get; set; }
        /// <summary>
        /// Інше речове право
        /// </summary>
        public bool UseRightInfo { get; set; }
        /// <summary>
        /// Обтяження та  іпотека
        /// </summary>
        public bool EncumbranceMortgageInfo { get; set; }
        /// <summary>
        /// Намір про продаж
        /// </summary>
        public bool SaleIntention { get; set; }
        /// <summary>
        /// Нерухомість на ділянці
        /// </summary>
        public bool RealtyInfo { get; set; }
    }
}
