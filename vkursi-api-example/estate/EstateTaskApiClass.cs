using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class EstateTaskApiClass
    {
        /* 
        
        19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам. Частина перша додавання в чергу
        [POST] api/1.0/estate/estatecreatetaskapi

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Cadastrs":["5621287500:03:001:0019"],"CalculateCost":true,"IsNeedUpdateAll":false,"IsReport":true,"TaskName":"Назва задачі","DzkOnly":false}'

         */

        public static string EstateCreateTaskApi(string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi");
                RestRequest request = new RestRequest(Method.POST);

                EstateCreateTaskApiRequestBodyModel ECTARequestBodyRow = new EstateCreateTaskApiRequestBodyModel
                {
                //    Cadastrs = new List<string>         // Кадастрові номери
                //{
                //    "5621287500:03:001:0019"
                //},
                //    Koatuus = new List<string>          // КОАТУУ (обмеження 10)
                //{
                //    "5621287500"
                //},
                    Edrpous = new List<string>          // Коди ЄДРПОУ (обмеження 10)
                {
                    "33768131"
                },
                //    Ipns = new List<string>             // Коди ІПН-и (обмеження 10)
                //{
                //    "3083707142"
                //},
                    CalculateCost = true,              // Якщо тільки порахувати вартість
                    IsNeedUpdateAll = false,            // Якщо true - оновлюємо всі дані в ДЗК і РРП
                    TaskName = "Назва задачі"           // Назва задачі (обов'язково)
                    // isDzkOnly                        // Перевірка ДЗК + НГО без РРП
                };


                string body = JsonConvert.SerializeObject(ECTARequestBodyRow);

                //body = "{\"Edrpous\":[\"33768131\"]}";

                // Example Body: {"Edrpous":["19124549"],"Ipns":["3083707142"],"Koatuus":["5621287500"],"Cadastrs":["5621287500:03:001:0019"],"CalculateCost":false,"IsNeedUpdateAll":false,"IsReport":true,"TaskName":"Назва задачі","DzkOnly":false}

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
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

            EstateCreateTaskApiResponseBodyModel ECTAResponseBody = new EstateCreateTaskApiResponseBodyModel();

            ECTAResponseBody = JsonConvert.DeserializeObject<EstateCreateTaskApiResponseBodyModel>(responseString);

            if (ECTAResponseBody.isSuccess == true) // ECTAResponseBody.isSuccess = true - задача створена успішно
            {
                return ECTAResponseBody.taskId;     // Id задачі за яким ми будемо перевіряти її виконання
            }
            else
            {
                Console.WriteLine("error: {0}", ECTAResponseBody.status);
                return null;

                /* ECTAResponseBody.status = "Not enough money" - недостатньо коштів
                 * ECTAResponseBody.status = "Unexpected server error" - непередвачувана помилка
                 */
            }

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


        /* 
        
        20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)
        [GET] api/1.0/estate/getestatetasklist

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatetasklist' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw ''

         */

        public static List<GetEstateTaskListResponseBodyModel> GetEstateTaskList(string token)
        {            
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatetasklist");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", "Bearer " + token);

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

            List<GetEstateTaskListResponseBodyModel> GETLResponseBody = new List<GetEstateTaskListResponseBodyModel>();

            GETLResponseBody = JsonConvert.DeserializeObject<List<GetEstateTaskListResponseBodyModel>>(responseString);

            return GETLResponseBody;
        }


        /* 

        21. Отримання розширеної інформації по ДРРП, НГО, ДЗК за TaskId або переліком кадастрових номерів
        [POST] api/1.0/estate/estategettaskdataapi

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estategettaskdataapi' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"taskId":"","skip":0,"take":100,"cadastr":["7424955100:04:001:0511"]}'

         */

        public static List<EstateGetTaskDataApiDataModel> EstateGetTaskDataApi(string token, string taskId, string cadastr)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                EstateGetTaskDataApiRequestBodyModel EGTDARequestBody = new EstateGetTaskDataApiRequestBodyModel
                {
                    TaskId = null,                                              // Id задачі (перелік доспупних taskId можна отриммати в  api/1.0/estate/getestatetasklist)
                    Skip = 0,                                                   // К-ть записів які будуть пропущені
                    Take = 100,                                                 // К-ть записів які будуть отримані (null - всі)
                    Сadastr = new List<string>                                // ВПерелік кадастрових номерів
                    {
                        cadastr
                    }
                };

                string body = JsonConvert.SerializeObject(EGTDARequestBody);    // Example Body: {"taskId":null,"skip":0,"take":100,"cadastr":["7424955100:04:001:0511"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estategettaskdataapi");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;


                if ((int)response.StatusCode == 401) // Вкажіть taskId
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                //else if ((int)response.StatusCode == 200 )
                //{
                //    Console.WriteLine("Вкажіть taskId");
                //    return null;
                //}

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstateGetTaskDataApiResponseModel EGTDAResponse = new EstateGetTaskDataApiResponseModel();

            EGTDAResponse = JsonConvert.DeserializeObject<EstateGetTaskDataApiResponseModel>(responseString);

            if (EGTDAResponse.isSuccess == true && EGTDAResponse.status == "Вкажіть taskId")
            {
                Console.WriteLine("Вкажіть taskId. TaskId можна отримати в [GET] api/1.0/estate/getestatetasklist");
                return null;
            }
            if (EGTDAResponse.isSuccess == true)
            {
                return EGTDAResponse.data;
            }
            else
            {
                Console.WriteLine("error: {0}", EGTDAResponse.status);
                return null;

                /* EGTDAResponse.status = ...
                 * EGTDAResponse.status = "Unexpected server error" - непередвачувана помилка
                 */
            }

            /*

                // Python - http.client example:

                OkHttpClient client = new OkHttpClient().newBuilder()
                  .build();
                MediaType mediaType = MediaType.parse("application/json");
                RequestBody body = RequestBody.create(mediaType, "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"СalculateСost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}");
                Request request = new Request.Builder()
                  .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi")
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
                payload = "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"СalculateСost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}"
                headers = {
                  'ContentType': 'application/json',
                  'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
                  'Content-Type': 'application/json'
                }
                conn.request("POST", "/api/1.0/estate/estatecreatetaskapi", payload, headers)
                res = conn.getresponse()
                data = res.read()
                print(data.decode("utf-8"))

            */

        }
    }


    // 19. EstateCreateTaskApi (Модель Body запиту)
    public class EstateCreateTaskApiRequestBodyModel                            // Модель Body запиту
    {
        public List<string> Edrpous { get; set; }                               // Коди ЄДРПОУ
        public List<string> Ipns { get; set; }                                  // ІПН-и
        public List<string> Koatuus { get; set; }                               // КОАТУУ
        public List<string> Cadastrs { get; set; }                              // Кадастрові номери
        public bool? CalculateCost { get; set; }                                // Якщо тільки порахувати вартість
        public bool IsNeedUpdateAll { get; set; }                               // Якщо true - оновлюємо всі дані в ДЗК і РРП
        public string TaskName { get; set; }                                    // Назва задачі
        public EstateApiCreateTaskParamRequest Param { get; set; }              // Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)

        // Керування моніторингом СМС РРП

        public int? SmsRrpMonitoring { get; set; }                              // Тип моніторингу // 1 - Моніторинг по нормалізованим / 2 - Моніторинг по повним
        public DateTime? DateTimeEnd { get; set; }                              // Дата закінчення моныторингу

        public List<string> SmsRrpMonitoringSectionsList { get; set; }          // Перелік розділів що будуть моніторитись 

                                                                                // Загальні відомості	PlotGeneralInfo
                                                                                // Право власності	PlotOwnershipInfo
                                                                                // Інше речове право	PlotUseRightInfo
                                                                                // Обтяження та іпотека	PlotEncumbranceInfo,PlotMortgageInfo
                                                                                // Нерухомість на ділянці	PlotRealtyInfo
                                                                                // Намір про продаж	PlotSaleIntention


    }

    public class EstateApiCreateTaskParamRequest                                // Вибір реестрів по яким необхідно здійснити перевірку (null - якщо по всім)
    {
        public bool IsWithDzk { get; set; }                                     // ДЗК (Державный земельний кадастр)
        public bool IsWithRrp { get; set; }                                     // РРП (Державний реєстр речових прав на нерухоме майно)
        public bool IsWithPkku { get; set; }                                    // ПККУ (Публічна кадастрова карта України)
        public bool IsWithNgo { get; set; }                                     // НГО (Нормативна грошова оцінка)
    }

    // 19. EstateCreateTaskApi (Модель відповіді)
    public class EstateCreateTaskApiResponseBodyModel                           // Модель відповіді EstateCreateTaskApi
    {
        public bool isSuccess { get; set; }                                     // Запит віконано успішно
        public string status { get; set; }                                      // Повідомлення
        public string taskId { get; set; }                                      // Id задачі за яким ми будемо перевіряти її виконання
        public string taskName { get; set; }                                    // Назва задачі
        public double? cost { get; set; }                                       // Вартість виконання запиту
    }

    // 20. GetEstateTaskList (Модель відповіді)
    public class GetEstateTaskListResponseBodyModel                             // Модель відповіді GetEstateTaskList (перелік створенних задач (задачі на виконання запитів до ДРРП, НГО, ДЗК))
    {
        public string Id { get; set; }                                          // Id задачі
        public string Name { get; set; }                                        // Назва задачі
        public DateTime DateStart { get; set; }                                 // Дата початку виконання
        public DateTime? DateEnd { get; set; }                                  // Дата закінчення виконання
        public bool Complete { get; set; }                                      // Задачу виконано (true - так / false - ні)
        public int State { get; set; }                                          // state = 0 отчет не готов, 1 готово ррп, 2 готово ррп + дзк, 3 готово ррп + дзк + нго
    }

    // 21.EstateGetTaskDataApi (Модель Body запиту)
    public class EstateGetTaskDataApiRequestBodyModel                           // Модель Body запиту
    {
        public string TaskId { get; set; }                                      // Id задачі
        public int? Skip { get; set; }                                          // К-ть записів які будуть пропущені
        public int? Take { get; set; }                                          // К-ть записів які будуть отримані (максимум MAX)
        public List<string> Сadastr { get; set; }                               // Перелік кадастрових номерів (які булі додані в задачі)
    }

    // 21. EstateGetTaskDataApi (Модель відповіді)
    public class EstateGetTaskDataApiResponseModel                              // Модель відповіді EstateGetTaskDataApi
    {
        public bool isSuccess { get; set; }                                     // Запит віконано успішно
        public string status { get; set; }                                      // Повідомлення
        public List<EstateGetTaskDataApiDataModel> data { get; set; }           // Перелік даних
    }

    public class EstateGetTaskDataApiDataModel                                  // Перелік даних
    {
        public string CadastrNumber { get; set; }                               // Кадастровий номер
        public ElasticPlot Plot { get; set; }                                   // ДЗК + ПКУУ (Просторові дані)
        public RealEstateAdvancedResponseModel RrpAdvanced { get; set; }        // РРП
        public List<List<Coordinate>> geometry { get; set; }                    // Геопросторові координати ділянки
    }

    public class RealEstateAdvancedResponseModel                                // РРП
    {
        public List<Realty> realty { get; set; }                                // Відомості про ОНМ
        public List<oldRealty> oldRealty { get; set; }                          // Відомості з реестру РПВН
        public List<oldMortgageJson> oldMortgageJson { get; set; }              // Відомості з реєстру ДРІ
        public List<oldLimitationJson> oldLimitationJson { get; set; }          // // Відомості з реєстру ЄРЗ
    }


    public class Realty                                                         // Відомості про ОНМ
    {
        public string regNum { get; set; }                                      // Реєстраційний номер ОНМ
        public DateTime regDate { get; set; }                                   // Дата реєстрації 
        public List<Irp> irps { get; set; }                                     // Масив об’єктів. Об’єкти у складі масиву описують відомості про інші речові права (0..n)
        public List<Property> properties { get; set; }                          // Масив об’єктів. Об’єкти у складі масиву описують відомості про права власності (0..n)
        public List<GroundArea> groundArea { get; set; }                        // Відомості про земельну ділянку
        public List<RealtyAddress> realtyAddress { get; set; }                  // Адреса
        public string reType { get; set; }                                      // Тип ОНМ
        public string reState { get; set; }                                     // Стан
        public string sectionType { get; set; }                                 // Тип розділу
        public string region { get; set; }                                      // Назва регіону до якого належить ОНМ
        public string additional { get; set; }                                  // Додаткові відомості
        public int hasProperties { get; set; }                                  // -
        public List<Irp> mortgage { get; set; }                                 // Масив об’єктів. Об’єкти у складі масиву описують відомості про іпотеки (0..n)
        public List<Irp> limitation { get; set; }                               // Масив об’єктів. Об’єкти у складі масиву описують відомості обтяження (0..n)

        [JsonProperty("reTypeExtension")]                                       // Доповнення до типу ОНМ
        public string reTypeExtension { get; set; }

        [JsonProperty("reSubType")]                                             // Підтип ОНМ
        public string reSubType { get; set; }

        [JsonProperty("reSubTypeExtension")]                                    // Доповнення до підтипу
        public string reSubTypeExtension { get; set; }

        [JsonProperty("reInCreation")]                                          // Ознака розділу, приймає значення: 1 - Розділ в процесі відкриття внаслідок поділу 2 - Розділ в процесі відкриття внаслідок виділу частки 3 - Розділ в процесі відкриття внаслідок об’єднання 4 - Розділ в процесі відкриття
        public string reInCreation { get; set; }

        [JsonProperty("isResidentialBuilding")]                                 // Об’єкт житлової нерухомості: 0 - Так 1 - Ні
        public string isResidentialBuilding { get; set; }

        [JsonProperty("techDescription")]                                       // Опис
        public string techDescription { get; set; }

        [JsonProperty("area")]                                                  // Загальна площа (кв.м)
        public double? area { get; set; }

        [JsonProperty("livingArea")]                                            // Житлова площа (кв.м)
        public double? livingArea { get; set; }

        [JsonProperty("wallMaterial")]                                          // Площа самочинно збудованого (кв.м) 
        public string wallMaterial { get; set; }

        [JsonProperty("depreciationPercent")]                                   // Відсоток зносу (%)
        public double? depreciationPercent { get; set; }

        [JsonProperty("selfBuildArea")]                                         // Площа самочинно збудованого (кв.м)
        public double? selfBuildArea { get; set; }

    }

    public class GroundArea                                                     // Відомості про земельну ділянку
    {
        public string cadNum { get; set; }                                      // Кадастровий номер
        public string area { get; set; }                                        // Площа
        public string areaUM { get; set; }                                      // Одиниця виміру площі
        public string targetPurpose { get; set; }                               // Цільове призначення
    }

    public class Property                                                       // Масив об’єктів. Об’єкти у складі масиву описують відомості про права власності (0..n)
    {
        public int rnNum { get; set; }                                          // Номер запису про право власності
        public DateTime regDate { get; set; }                                   // Дата державної реєстрації права власності
        public string partSize { get; set; }                                    // Розмір частки Розмір частки у праві спільної власності Використовуються символи: ,/0123456789. Дані зазначаються відповідно до шаблону: шаблон 1 (десятковий дріб): Х,У, де Х - 1 або 0; де У - значення з переліку[0123456789], якщо х = 1 то у = 0; шаблон 2 (звичайний дріб): Х/У, де Х - значення з[0123456789], внесення першого "0" недопустимо та(Х<=У), де У - значення з переліку[0123456789], внесення першого "0" недопустимо та(Х<=У)
        public List<Subject> subjects { get; set; }                             // Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів права власності (1..n)
        public List<CauseDocument> causeDocuments { get; set; }                 // Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави права власності (1..n)
        public string prType { get; set; }                                      // Форма власності 
        public string prState { get; set; }                                     // Стан
        public string registrar { get; set; }                                   // -
        public List<EntityLink> entityLinks { get; set; }                       // Зв’язок з записом з реєстрами до 2013 р.
        public string operationReason { get; set; }                             // -
        public string prCommonKind { get; set; }                                // Вид спільної власності приймає значення числового коду:  (1 - спільна сумісна | 2 - спільна часткова)
    }

    public class Subject                                                        // Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів обтяження (1..n)
    {
        public string sbjName { get; set; }                                     // Найменування /ПІБ суб’єкта
        public string dcSbjType { get; set; }                                   // Тип суб'єкта: 1 - (фіз.особа) 2 - (юр.особа)
        public string SbjSort { get; set; }                                     // Вид суб'єкт: 1 - особа, яка наділяється правом. 2 - особа, яка передає право
        public string countryName { get; set; }                                 // Назва країни
        public string sbjRlName { get; set; }                                   // Роль суб'єкта
        public string dcSbjKind { get; set; }                                   // -
        public int? isOwner { get; set; }                                       // Є власником
        public string sbjCode { get; set; }                                     // ЄДРПОУ 
        public string isState { get; set; }                                     // -
        public string additional { get; set; }                                  // Додаткові відомості

    }

    public class CauseDocument                                                  // Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави обтяження (1..n)
    {
        public string cdType { get; set; }                                      // Назва документа 
        public string cdTypeExtension { get; set; }                             // Доповнення до типу
        public DateTime docDate { get; set; }                                   // Дата документа
        public string publisher { get; set; }                                   // Видавник
        public string @enum { get; set; }                                       // Номер
    }

    public class RealtyAddress                                                  // Адреса
    {
        public string addressDetail { get; set; }                               // Адреса
    }

    public class Irp                                                            // Масив об’єктів. Об’єкти у складі масиву описують відомості про інші речові права (0..n)
    {
        public string lmType { get; set; }                                      // Tип обтяження
        public DateTime regDate { get; set; }                                   // Дата державної реєстрації іншого речового права
        public DateTime actTerm { get; set; }                                   // Строк дії
        public string objectDescription { get; set; }                           // Опис предмета іншого речового права
        public int rnNum { get; set; }                                          // Номер запису обтяження
        public List<Subject> subjects { get; set; }                             // Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів обтяження (1..n)
        public List<CauseDocument> causeDocuments { get; set; }                 // Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави обтяження (1..n)
        public List<Obligations> obligations { get; set; }                      // Масив об’єктів. Об’єкти у складі масиву описують відомості про зобов’язання (1..n)
        public string parentIrpRnNum { get; set; }                              // Перенесено із запису
        public string parentIrpOpID { get; set; }                               // Попередній номер запису про інше речове право
        public string moveDate { get; set; }                                    // Дата перенесення запису
        public string irpSort { get; set; }                                     // Вид іншого речового права 
        public string irpState { get; set; }                                    // - 
        public string mgState { get; set; }                                     // Стан іпотеки
        public string registrar { get; set; }                                   // -
        public List<EntityLink> entityLinks { get; set; }                       // Зв’язок з записом з реєстрами до 2013 р.
        public string operationReason { get; set; }                             // -

        [JsonProperty("holderObj")]                                             // Організація, що зареєструвала інше речове право
        public string holderObj { get; set; }

        [JsonProperty("IrpSort")]                                               // Вид іншого речового права 
        public string IrpSort { get; set; }

        [JsonProperty("irpSortExtension")]                                      // Доповнення до виду
        public string irpSortExtension { get; set; }

        [JsonProperty("irpDescription")]                                        // Зміст, характеристика іншого речового права
        public string irpDescription { get; set; }

        [JsonProperty("isIndefinitely")]                                        // Ознака безстрокове  1 - безстрокове / 0 - не безстрокове
        public string isIndefinitely { get; set; }

        [JsonProperty("actTermText")]                                           // Строк дії
        public string actTermText { get; set; }

        [JsonProperty("isRent")]                                                // Ознака «Піднайм»(суборенда)
        public string isRent { get; set; }

        [JsonProperty("isRightToRent")]                                         // Ознака «З правом передачі в піднайм (суборенду)» 1 - так / 0 - ні
        public string isRightToRent { get; set; }

        [JsonProperty("isRightProlongation")]                                   // Ознака «З правом пролонгації» 1 - так / 0 - ні
        public string isRightProlongation { get; set; } 

        public Realty realty { get; set; }                                      // Масив об’єктів. Об’єкти у складі масиву описують відомості про ОНМ
    }

    public class EntityLink                                                     // Зв’язок з записом з реєстрами до 2013 р.
    {
        [JsonProperty("rpvnReID")]                                              // Номер
        public string rpvnReID { get; set; }

        [JsonProperty("regDate")]                                               // Дата запису
        public string regDate { get; set; }

        [JsonProperty("registryType")]                                          // Назва реєстру
        public string registryType { get; set; }
    } 

    public class Obligations                                                    // Відомості про зобов’язання 
    {
        [JsonProperty("execTerm")]                                              // Строк виконання
        public string execTerm { get; set; }

        [JsonProperty("obligationSum")]                                         // Розмір основного зобов’язання
        public string obligationSum { get; set; }

        [JsonProperty("CurrencyType")]                                          // Тип валюти
        public string CurrencyType { get; set; }

        [JsonProperty("obligationSumText")]                                     // Розмір основного зобов’язання
        public string obligationSumText { get; set; }

        [JsonProperty("currencyText")]                                          // Тип валюти
        public string currencyText { get; set; }

        [JsonProperty("execTermText")]                                          // Строк виконання
        public string execTermText { get; set; }
    }

    public class oldRealty                                                      // Відомості з реестру РПВН
    {
        [JsonProperty("RE_ID")]                                                 // Реєстраційний номер майна
        public string RE_ID { get; set; }

        [JsonProperty("RE_TYPENAME")]                                           // Тип майна
        public string RE_TYPENAME { get; set; }

        [JsonProperty("ADDITIONAL")]                                            // Додаткові відомості
        public string ADDITIONAL { get; set; }

        [JsonProperty("RC_APPL")]                                               // Доповнення до типу майна
        public string RC_APPL { get; set; }

        [JsonProperty("RE_LAND_TYPE")]                                          // Тип земельної ділянки
        public string RE_LAND_TYPE { get; set; }

        [JsonProperty("RE_LAND_TYPE_EXTENSION")]                                // Доповнення до типу земельної ділянка
        public string RE_LAND_TYPE_EXTENSION { get; set; }

        [JsonProperty("CAD_NUM")]                                               // Кадастровий номер
        public string CAD_NUM { get; set; }

        [JsonProperty("AREA_ALL")]                                              // Загальна площа (кв.м)
        public string AREA_ALL { get; set; }

        [JsonProperty("AREA_HAB")]                                              // Житлова площа (кв.м)
        public string AREA_HAB { get; set; }

        [JsonProperty("BUILD_FROM")]                                            // Матеріали стін
        public string BUILD_FROM { get; set; }

        [JsonProperty("PLOT_AREA")]                                             // Площа земельної ділянки (кв.м)
        public string PLOT_AREA { get; set; }

        [JsonProperty("DESTROY_PERCENT")]                                       // Відсоток зносу
        public string DESTROY_PERCENT { get; set; }

        [JsonProperty("CUR_PRICE")]                                             // Загальна вартість нерухомого майна (грн.)
        public string CUR_PRICE { get; set; }

        [JsonProperty("SELF_BUILD_AREA")]                                       // Площа самочинно збудованого (кв.м)
        public string SELF_BUILD_AREA { get; set; }

        [JsonProperty("SELF_BUILD_PRICE")]                                      // Вартість самочинно збудованого (грн.)
        public string SELF_BUILD_PRICE { get; set; }

        [JsonProperty("TEXT_BODY")]                                             // Технічний опис майна
        public string TEXT_BODY { get; set; }

        [JsonProperty("REG_NUM")]                                               // Номер запису
        public string REG_NUM { get; set; }

        [JsonProperty("BOOK_NUM")]                                              // Номер в книзі
        public string BOOK_NUM { get; set; }

        [JsonProperty("ADD_DATE")]                                              // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                            // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("PROPADDRESS")]                                           // Адреса
        public string PROPADDRESS { get; set; }
    }

    public class oldMortgageJson                                                // Масив об’єктів з реєстру ДРІ
    {
        [JsonProperty("OP_OP_ID")]                                              // Реєстраційний номер
        public int? OP_OP_ID { get; set; }

        [JsonProperty("LM_TYPENAME")]                                           // Тип значення
        public string LM_TYPENAME { get; set; }

        [JsonProperty("LM_TYPE_EXTENSION")]                                     // Доповнення до типу
        public string LM_TYPE_EXTENSION { get; set; }

        [JsonProperty("REG_DATE")]                                              // Дата та час реєстрації
        public string REG_DATE { get; set; }

        [JsonProperty("EMP")]                                                   // Дані реєстратора
        public string EMP { get; set; }

        [JsonProperty("CONTRACT_SUM")]                                          // Розмір основного зобов’язання
        public string CONTRACT_SUM { get; set; }

        [JsonProperty("CURRENCY_TYPE")]                                         // Тип валюти
        public string CURRENCY_TYPE { get; set; }

        [JsonProperty("EXEC_TERM")]                                             // Строк виконання
        public string EXEC_TERM { get; set; }

        [JsonProperty("BONDSRNUM")]                                             // Заставна
        public string BONDSRNUM { get; set; }

        [JsonProperty("BONDOWNERNAME")]                                         // Власник заставної
        public string BONDOWNERNAME { get; set; }

        [JsonProperty("BONDCODE")]                                              // ЄДРПОУ
        public string BONDCODE { get; set; }

        [JsonProperty("BONDSBJTYPE")]                                           // Тип суб'єкта: 1 - (фіз. особа) 2 - (юр. особа)
        public string BONDSBJTYPE { get; set; }

        [JsonProperty("BONDADDITIONAL")]                                        // Додаткові дані по заставній 
        public string BONDADDITIONAL { get; set; }

        [JsonProperty("ADD_DATE")]                                              // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                            // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("ADDITIONAL")]                                            // Додаткові відомості
        public string ADDITIONAL { get; set; }
    }

    public class oldLimitationJson                                              // Відомості з реєстру ЄРЗ
    {
        [JsonProperty("OP_OP_ID")]                                              // Реєстраційний номер
        public int? OP_OP_ID { get; set; }

        [JsonProperty("LM_TYPENAME")]                                           // Тип значення
        public string LM_TYPENAME { get; set; }

        [JsonProperty("LM_TYPE_EXTENSION")]                                     // Доповнення до типу
        public string LM_TYPE_EXTENSION { get; set; }

        [JsonProperty("REG_DATE")]                                              // Дата та час реєстрації
        public string REG_DATE { get; set; }

        [JsonProperty("EMP")]                                                   // Дані реєстратора
        public string EMP { get; set; }

        [JsonProperty("ADD_DATE")]                                              // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                            // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("DATEADDITIONAL")]                                        // Додаткові відомості
        public string DATEADDITIONAL { get; set; }

        [JsonProperty("ACT_TERM")]                                              // Термін дії
        public string ACT_TERM { get; set; }

        [JsonProperty("ARCHIVE_NUM")]                                           // Архівний номер
        public string ARCHIVE_NUM { get; set; }

        [JsonProperty("ARCHIVE_DATE")]                                          // Архівна дата
        public string ARCHIVE_DATE { get; set; }

        [JsonProperty("REQUESTOR")]                                             // Заявник
        public string REQUESTOR { get; set; }

        [JsonProperty("START_DAY")]                                             // Дата виникнення (день)
        public string START_DAY { get; set; }

        [JsonProperty("START_MONTH")]                                           // Дата виникнення (місяць)
        public string START_MONTH { get; set; }

        [JsonProperty("START_YEAR")]                                            // Дата виникнення (рік)
        public string START_YEAR { get; set; }
    }

    public class ElasticPlot                                                    // ДЗК + НГО + Просторові дані
    {
        public string complexNumber { get; set; }                               // Кадастровий номер
        public double? area { get; set; }                                       // Площа
        //public ElasticPlotGeometry geometry { get; set; }                     // Видалено. Геопросторові координати ділянки
        public bool? isCorrectRegion { get; set; }                              // Відповідність регіону
        public bool? isGeomValid { get; set; }                                  // Відповідність просторовим даним
        public string koatuu { get; set; }                                      // Код за КОАТУУ
        public DzkLandInfo dzkLandInfo { get; set; }                            // Відомості про земельну ділянку за ДЗК 
        public RrpLandInfo rrpLandInfo { get; set; }                            // Основні скорочені дані з РРП
        public bool? vidpovidnistPloshchi { get; set; }                         // Відповідність площі (Площі ДЗК і РРП співпадають / Площі не співпадають)
        public bool? korrektnistRoztashuvannyai { get; set; }                   // Корректність розташування ділянки (Коректне розташування / Некоректне розташування)
        public int? kategoriya { get; set; }                                    // Категорія цільового призначення ділянки за довідником
        public int? tsilovePriznachennya { get; set; }                          // Цільове призначення ділянки за довідником
        public int? vidUgiddya { get; set; }                                    // Тип угідя
        public int? formaVlasnosti { get; set; }                                // Форма власності
        public int? vlasnikiDilyanok { get; set; }                              // Власники ділянок (null - Відомості відсутні; 1 - Власник; 2 -Розпорядник (VlasnikiDilyanokDict))
        public int? kilkistVlasnikiv { get; set; }                              // Кількість власників (1 власник/2 власники/3 власники/Більше 3х власників/Немає даних про власників)
        public int? tipPravaKoristuvannya { get; set; }                         // Тип права користування (Оренди/Суборенди/Емфітевзис/Сервітут/Спільного користування/Найму)
        public bool? pravoKoristuvannyaOrendariv { get; set; }                  // Право користування в розрізі Орендарів (Наявні дані про право користування/Дані про право користування відсутні)
        public int? terminDiyiOrendi { get; set; }                              // Термін дії оренди (в днях)
        public int? sudovikhSprav { get; set; }                                 // Наявність судових справ
        public int? sudovikhSpravVidkryto { get; set; }                         // Наявність відкритих судових справ
        public int? obmezhennyaObtyazhennya { get; set; }                       // Обмеження та обтяження
        public ElasticNGOModel ngo { get; set; }                                // НГО 
    }


    public class RrpLandInfo                                                    // Основні скорочені дані з РРП
    {
        public string purpose { get; set; }                                     // realty.groundArea.targetPurpose
        public double? area { get; set; }                                       // realty.groundArea.area через метод GetAreaFromRealEstateAdvice(StateRegisterRealEstateModel realEstate)

        public DateTime? irpsRegDate { get; set; }                              // realty.irps.regDate
        public DateTime? limitationRegDate { get; set; }                        // realty.limitation.regDate
        public DateTime? mortgageRegDate { get; set; }                          // realty.mortgage.regDate

        public DateTime? irpsEndDate { get; set; }                              // realty.irps.actTerm
        public DateTime? limitationEndDate { get; set; }                        // realty.limitation.actTerm
        public DateTime? mortgageEndDate { get; set; }                          // realty.mortgage.actTerm

        public List<RrpLandInfoSubject> subject { get; set; }                   // properties.subjects, irps.subjects, limitation.subjects, mortgage.subjects, 
    }

    public class RrpLandInfoSubject                                             // Системна інформація Vkursi
    {
        public string name { get; set; }                                        // Системна інформація Vkursi
        public string sbjRlName { get; set; }                                   // Системна інформація Vkursi
        public string code { get; set; }                                        // Системна інформація Vkursi
        public string type { get; set; }                                        // Системна інформація Vkursi
        public bool? isOwner { get; set; }                                      // Системна інформація Vkursi
        public int? subjectNestedTypeId { get; set; }                           // Системна інформація Vkursi  
        //[1 -  properties.subjects]
        //[2 -  irps.subjects]
        //[3 -  limitation.subjects]
        //[4 -  mortgage.subjects]
    }

    public class ElasticNGOModel                                                // НГО 
    {
        public double? area { get; set; }                                       // Оціночна площа
        public DateTime? dateModify { get; set; }                               // Дата перевірки НГО
        public string ownershipType { get; set; }                               // Тип власності
        public double? price { get; set; }                                      // Ціна
        public double? pricePerGekt { get; set; }                               // Ціна за гектар
        public string purpose { get; set; }                                     // Призначення
        public int? purposeInt { get; set; }                                    // Призначення int
    }

    public class ElasticPlotGeometry                                            // Геопросторові координати ділянки
    {
        public List<Coordinate> coordinates { get; set; }                       // Координати ділянки
        public string geometryType { get; set; }                                // Тип геометрії
    }

    public class Coordinate                                                     // Координати ділянки
    {
        public double? x { get; set; }                                          // 
        public double? y { get; set; }                                          // 
    }

    public class DzkLandInfo                                                    // Відомості про земельну ділянку за ДЗК 
    {
        public DateTime UpdateDate { get; set; }                                // Дата оновлення ділянки за ДЗК
        public string Purpose { get; set; }                                     // Цільове призначення
        public int? PurposeNum { get; set; }                                    // Цільове призначення INT
        public string Ownership { get; set; }                                   // Форма власності
        public int? OwnershipNum { get; set; }                                  // Форма власності
        public string LandArea { get; set; }                                    // Площа земельної ділянки
        public float? LandAreaNum { get; set; }                                 // Площа земельної ділянки
        public string Location { get; set; }                                    // Місце розташування
        public RegulatoryMonetaryValuationClass RegulatoryMonetaryValuation { get; set; }   // Нормативно грошова оцінка (ПККУ) 
        public List<OwnershipInfoClass> OwnershipInfo { get; set; }             // Відомості про суб'єктів права власності на земельну ділянку
        public List<OwnershipInfoClass> SubjectRealRightLand { get; set; }      // Відомості про суб'єктів права власності на земельну ділянку
        public RestrictionInfoClass RestrictionInfo { get; set; }               // Відомості про зареєстроване обмеження у використанні земельної ділянки
    }

    public class RegulatoryMonetaryValuationClass                               // Нормативно грошова оцінка (ПККУ) 
    {
        public string ValueUah { get; set; }                                    // Значення, гривень
        public string DateEvaluation { get; set; }                              // Дата оцінки ділянки
    }

    public class OwnershipInfoClass                                             // Відомості про суб'єктів права власності на земельну ділянку
    {
        public string PropertyRight { get; set; }                               // Вид речового права
        public string NameFo { get; set; }                                      // Найменування юридичної особи
        public string NameUo { get; set; }                                      // Прізвище, ім'я та по батькові фізичної особи
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ юридичної особи
        public string DateRegRight { get; set; }                                // Дата державної реєстрації права (в державному реєстрі прав)
        public string EntryRecordNumber { get; set; }                           // Номер запису про право (в державному реєстрі прав)
        public string RegAuthority { get; set; }                                // Орган, що здійснив державну реєстрацію права (в державному реєстрі прав)
        public string AreaCoveredSublease { get; set; }                         // Площа, на яку поширюється право суборенди
    }

    public class RestrictionInfoClass                                           // Відомості про зареєстроване обмеження у використанні земельної ділянки
    {
        public string RestrictionType { get; set; }                             // Вид обмеження
        public string RestrictionDate { get; set; }                             // Дата державної реєстрація обмеження
    }


}