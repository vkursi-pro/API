using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class EstateTaskApiClass
    {
        /// <summary>
        /// 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам. Частина перша додавання в чергу
        /// [POST] api/1.0/estate/estatecreatetaskapi
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// 

        /* 

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

    // 20. GetEstateTaskList (Модель відповіді)
    /// <summary>
    /// Модель відповіді GetEstateTaskList (перелік створенних задач (задачі на виконання запитів до ДРРП, НГО, ДЗК))
    /// </summary>
    public class GetEstateTaskListResponseBodyModel                             // 
    {/// <summary>
     /// Id задачі
     /// </summary>
        public string Id { get; set; }                                          // 
        /// <summary>
        /// Назва задачі
        /// </summary>
        public string Name { get; set; }                                        // 
        /// <summary>
        /// Дата початку виконання
        /// </summary>
        public DateTime DateStart { get; set; }                                 // 
        /// <summary>
        /// Дата закінчення виконання
        /// </summary>
        public DateTime? DateEnd { get; set; }                                  // 
        /// <summary>
        /// Задачу виконано (true - так / false - ні)
        /// </summary>
        public bool Complete { get; set; }                                      // 
        /// <summary>
        /// state = 0 отчет не готов, 1 готово ррп, 2 готово ррп + дзк, 3 готово ррп + дзк + нго
        /// </summary>
        public int State { get; set; }                                          // 
    }

    // 21.EstateGetTaskDataApi (Модель Body запиту)
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class EstateGetTaskDataApiRequestBodyModel                           // 
    {/// <summary>
     /// Id задачі
     /// </summary>
        public string TaskId { get; set; }                                      // 
        /// <summary>
        /// К-ть записів які будуть пропущені
        /// </summary>
        public int? Skip { get; set; }                                          // 
        /// <summary>
        /// К-ть записів які будуть отримані (максимум MAX)
        /// </summary>
        public int? Take { get; set; }                                          // 
        /// <summary>
        /// Перелік кадастрових номерів (які булі додані в задачі)
        /// </summary>
        public List<string> Сadastr { get; set; }                               // 
    }

    // 21. EstateGetTaskDataApi (Модель відповіді)
    /// <summary>
    /// Модель відповіді EstateGetTaskDataApi
    /// </summary>
    public class EstateGetTaskDataApiResponseModel                              // 
    {/// <summary>
     /// Запит віконано успішно
     /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// Повідомлення
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// Перелік даних
        /// </summary>
        public List<EstateGetTaskDataApiDataModel> data { get; set; }           // 
    }
    /// <summary>
    /// Перелік даних
    /// </summary>
    public class EstateGetTaskDataApiDataModel                                  // 
    {/// <summary>
     /// Кадастровий номер
     /// </summary>
        public string CadastrNumber { get; set; }                               // 
        /// <summary>
        /// ДЗК + ПКУУ (Просторові дані)
        /// </summary>
        public ElasticPlot Plot { get; set; }                                   // 
        /// <summary>
        /// РРП
        /// </summary>
        public RealEstateAdvancedResponseModel RrpAdvanced { get; set; }        // 
        /// <summary>
        /// Геопросторові координати ділянки
        /// </summary>
        public List<List<Coordinate>> geometry { get; set; }                    // 
    }
    /// <summary>
    /// РРП
    /// </summary>
    public class RealEstateAdvancedResponseModel                                // 
    {/// <summary>
     /// Відомості про ОНМ
     /// </summary>
        public List<Realty> realty { get; set; }                                // 
        /// <summary>
        /// Відомості з реестру РПВН
        /// </summary>
        public List<oldRealty> oldRealty { get; set; }                          // 
        /// <summary>
        /// Відомості з реєстру ДРІ
        /// </summary>
        public List<oldMortgageJson> oldMortgageJson { get; set; }              // 
        /// <summary>
        /// Відомості з реєстру ЄРЗ
        /// </summary>
        public List<oldLimitationJson> oldLimitationJson { get; set; }          
    }

    /// <summary>
    /// Відомості про ОНМ
    /// </summary>
    public class Realty                                                         // 
    {/// <summary>
     /// Реєстраційний номер ОНМ
     /// </summary>
        public string regNum { get; set; }                                      // 
        /// <summary>
        /// Дата реєстрації 
        /// </summary>
        public DateTime regDate { get; set; }                                   // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про інші речові права (0..n)
        /// </summary>
        public List<Irp> irps { get; set; }                                     // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про права власності (0..n)
        /// </summary>
        public List<Property> properties { get; set; }                          // 
        /// <summary>
        /// Відомості про земельну ділянку
        /// </summary>
        public List<GroundArea> groundArea { get; set; }                        // 
        /// <summary>
        /// Адреса
        /// </summary>
        public List<RealtyAddress> realtyAddress { get; set; }                  // 
        /// <summary>
        /// Тип ОНМ
        /// </summary>
        public string reType { get; set; }                                      // 
        /// <summary>
        /// Стан
        /// </summary>
        public string reState { get; set; }                                     // 
        /// <summary>
        /// Тип розділу
        /// </summary>
        public string sectionType { get; set; }                                 // 
        /// <summary>
        /// Назва регіону до якого належить ОНМ
        /// </summary>
        public string region { get; set; }                                      // 
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string additional { get; set; }                                  // 
        /// <summary>
        /// -
        /// </summary>
        public int hasProperties { get; set; }                                  // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про іпотеки (0..n)
        /// </summary>
        public List<Irp> mortgage { get; set; }                                 // 
        /// <summary>
        ///  Масив об’єктів. Об’єкти у складі масиву описують відомості обтяження (0..n)
        /// </summary>
        public List<Irp> limitation { get; set; }                               //
        /// <summary>
        /// Доповнення до типу ОНМ
        /// </summary>
        [JsonProperty("reTypeExtension")]                                       // 
     
        public string reTypeExtension { get; set; }
        /// <summary>
        /// Підтип ОНМ
        /// </summary>
        [JsonProperty("reSubType")]                                             // 
        public string reSubType { get; set; }
        /// <summary>
        /// Доповнення до підтипу
        /// </summary>
        [JsonProperty("reSubTypeExtension")]                                    // 
        public string reSubTypeExtension { get; set; }
        /// <summary>
        /// Ознака розділу, приймає значення: 1 - Розділ в процесі відкриття внаслідок поділу 2 - Розділ в процесі відкриття внаслідок виділу частки 3 - Розділ в процесі відкриття внаслідок об’єднання 4 - Розділ в процесі відкриття
        /// </summary>
        [JsonProperty("reInCreation")]                                          // 
        public string reInCreation { get; set; }
        /// <summary>
        /// Об’єкт житлової нерухомості: 0 - Так 1 - Ні
        /// </summary>
        [JsonProperty("isResidentialBuilding")]                                 // 
        public string isResidentialBuilding { get; set; }
        /// <summary>
        /// Опис
        /// </summary>
        [JsonProperty("techDescription")]                                       // 
        public string techDescription { get; set; }
        /// <summary>
        /// Загальна площа (кв.м)
        /// </summary>
        [JsonProperty("area")]                                                  // 
        public double? area { get; set; }
        /// <summary>
        /// Житлова площа (кв.м)
        /// </summary>
        [JsonProperty("livingArea")]                                            // 
        public double? livingArea { get; set; }
        /// <summary>
        /// Площа самочинно збудованого (кв.м) 
        /// </summary>
        [JsonProperty("wallMaterial")]                                          // 
        public string wallMaterial { get; set; }
        /// <summary>
        /// Відсоток зносу (%)
        /// </summary>
        [JsonProperty("depreciationPercent")]                                   // 
        public double? depreciationPercent { get; set; }
        /// <summary>
        /// Площа самочинно збудованого (кв.м)
        /// </summary>
        [JsonProperty("selfBuildArea")]                                         // 
        public double? selfBuildArea { get; set; }

    }
    /// <summary>
    /// Відомості про земельну ділянку
    /// </summary>
    public class GroundArea                                                     // 
    {/// <summary>
     /// Кадастровий номер
     /// </summary>
        public string cadNum { get; set; }                                      // 
        /// <summary>
        /// Площа
        /// </summary>
        public string area { get; set; }                                        // 
        /// <summary>
        /// ???
        /// </summary>
        public string areaUM { get; set; }                                      // 
        /// <summary>
        /// Цільове призначення
        /// </summary>
        public string targetPurpose { get; set; }                               // 
    }
    /// <summary>
    /// Масив об’єктів. Об’єкти у складі масиву описують відомості про права власності (0..n)
    /// </summary>
    public class Property                                                       // 
    {/// <summary>
     /// Номер запису про право власності
     /// </summary>
        public int rnNum { get; set; }                                          // 
        /// <summary>
        /// Дата державної реєстрації права власності
        /// </summary>
        public DateTime regDate { get; set; }                                   // 
        /// <summary>
        /// Розмір частки Розмір частки у праві спільної власності Використовуються символи: ,/0123456789. Дані зазначаються відповідно до шаблону: шаблон 1 (десятковий дріб): Х,У, де Х - 1 або 0; де У - значення з переліку[0123456789], якщо х = 1 то у = 0; шаблон 2 (звичайний дріб): Х/У, де Х - значення з[0123456789], внесення першого "0" недопустимо та(Х<=У), де У - значення з переліку[0123456789], внесення першого "0" недопустимо та(Х<=У)
        /// </summary>
        public string partSize { get; set; }                                    // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів права власності (1..n)
        /// </summary>
        public List<Subject> subjects { get; set; }                             // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави права власності (1..n)
        /// </summary>
        public List<CauseDocument> causeDocuments { get; set; }                 // 
        /// <summary>
        /// Форма власності 
        /// </summary>
        public string prType { get; set; }                                      // 
        /// <summary>
        /// Стан
        /// </summary>
        public string prState { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public string registrar { get; set; }                                   // 
        /// <summary>
        /// Зв’язок з записом з реєстрами до 2013 р.
        /// </summary>
        public List<EntityLink> entityLinks { get; set; }                       // 
        /// <summary>
        ///  ???
        /// </summary>
        public string operationReason { get; set; }                             //
        /// <summary>
        /// Вид спільної власності приймає значення числового коду:  (1 - спільна сумісна | 2 - спільна часткова)
        /// </summary>
        public string prCommonKind { get; set; }                                // 
    }
    /// <summary>
    /// Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів обтяження (1..n)
    /// </summary>
    public class Subject                                                        // 
    {
        public string sbjName { get; set; }                                     // Найменування /ПІБ суб’єкта
        /// <summary>
        /// Тип суб'єкта: 1 - (фіз.особа) 2 - (юр.особа)
        /// </summary>
        public string dcSbjType { get; set; }                                   // 
        /// <summary>
        /// Вид суб'єкт: 1 - особа, яка наділяється правом. 2 - особа, яка передає право
        /// </summary>
        public string SbjSort { get; set; }                                     // 
        /// <summary>
        /// Назва країни
        /// </summary>
        public string countryName { get; set; }                                 // 
        /// <summary>
        /// Роль суб'єкта
        /// </summary>
        public string sbjRlName { get; set; }                                   // 
        /// <summary>
        /// ???
        /// </summary>
        public string dcSbjKind { get; set; }                                   // 
        /// <summary>
        /// Є власником
        /// </summary>
        public int? isOwner { get; set; }                                       // 
        /// <summary>
        /// ЄДРПОУ 
        /// </summary>
        public string sbjCode { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public string isState { get; set; }                                     // 
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string additional { get; set; }                                  // 

    }
    /// <summary>
    /// Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави обтяження (1..n)
    /// </summary>
    public class CauseDocument                                                  // 
    {/// <summary>
     /// Назва документа 
     /// </summary>
        public string cdType { get; set; }                                      // 
        /// <summary>
        /// Доповнення до типу
        /// </summary>
        public string cdTypeExtension { get; set; }                             // 
        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime docDate { get; set; }                                   // 
        /// <summary>
        /// Видавник
        /// </summary>
        public string publisher { get; set; }                                   // 
        /// <summary>
        /// Номер
        /// </summary>
        public string @enum { get; set; }                                       // 
    }
    /// <summary>
    /// Адреса
    /// </summary>
    public class RealtyAddress                                                  // 
    {/// <summary>
     /// Адреса
     /// </summary>
        public string addressDetail { get; set; }                               // 
    }/// <summary>
     /// Масив об’єктів. Об’єкти у складі масиву описують відомості про інші речові права (0..n)
     /// </summary>

    public class Irp                                                            // 
    {/// <summary>
     /// Tип обтяження
     /// </summary>
        public string lmType { get; set; }                                      // 
        /// <summary>
        /// Дата державної реєстрації іншого речового права
        /// </summary>
        public DateTime regDate { get; set; }                                   // 
        /// <summary>
        /// Строк дії
        /// </summary>
        public DateTime actTerm { get; set; }                                   // 
        /// <summary>
        /// Опис предмета іншого речового права
        /// </summary>
        public string objectDescription { get; set; }                           // 
        /// <summary>
        /// Номер запису обтяження
        /// </summary>
        public int rnNum { get; set; }                                          // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про суб'єктів обтяження (1..n)
        /// </summary>
        public List<Subject> subjects { get; set; }                             // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про документи-підстави обтяження (1..n)
        /// </summary>
        public List<CauseDocument> causeDocuments { get; set; }                 // 
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про зобов’язання (1..n)
        /// </summary>
        public List<Obligations> obligations { get; set; }                      // 
        /// <summary>
        /// Перенесено із запису
        /// </summary>
        public string parentIrpRnNum { get; set; }                              // 
        /// <summary>
        /// Попередній номер запису про інше речове право
        /// </summary>
        public string parentIrpOpID { get; set; }                               // 
        /// <summary>
        /// Дата перенесення запису
        /// </summary>
        public string moveDate { get; set; }                                    // 
        /// <summary>
        /// Вид іншого речового права 
        /// </summary>
        public string irpSort { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public string irpState { get; set; }                                    // 
        /// <summary>
        /// Стан іпотеки
        /// </summary>
        public string mgState { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public string registrar { get; set; }                                   // 
        /// <summary>
        /// Зв’язок з записом з реєстрами до 2013 р.
        /// </summary>
        public List<EntityLink> entityLinks { get; set; }                       // 
        /// <summary>
        /// ???
        /// </summary>
        public string operationReason { get; set; }                             // 
        /// <summary>
        /// Організація, що зареєструвала інше речове право
        /// </summary>
        [JsonProperty("holderObj")]                                             // 
        public string holderObj { get; set; }
        /// <summary>
        /// Вид іншого речового права 
        /// </summary>
        [JsonProperty("IrpSort")]                                               // 
        public string IrpSort { get; set; }
        /// <summary>
        /// Доповнення до виду
        /// </summary>
        [JsonProperty("irpSortExtension")]                                      // 
        public string irpSortExtension { get; set; }
        /// <summary>
        /// Зміст, характеристика іншого речового права
        /// </summary>
        [JsonProperty("irpDescription")]                                        // 
        public string irpDescription { get; set; }
        /// <summary>
        /// Ознака безстрокове  1 - безстрокове / 0 - не безстрокове
        /// </summary>
        [JsonProperty("isIndefinitely")]                                        // 
        public string isIndefinitely { get; set; }
        /// <summary>
        /// Строк дії
        /// </summary>
        [JsonProperty("actTermText")]                                           // 
        public string actTermText { get; set; }
        /// <summary>
        /// Ознака «Піднайм»(суборенда)
        /// </summary>
        [JsonProperty("isRent")]                                                // 
        public string isRent { get; set; }
        /// <summary>
        /// Ознака «З правом передачі в піднайм (суборенду)» 1 - так / 0 - ні
        /// </summary>
        [JsonProperty("isRightToRent")]                                         // 
        public string isRightToRent { get; set; }
        /// <summary>
        /// Ознака «З правом пролонгації» 1 - так / 0 - ні
        /// </summary>
        [JsonProperty("isRightProlongation")]                                   // 
        public string isRightProlongation { get; set; }
        /// <summary>
        /// Масив об’єктів. Об’єкти у складі масиву описують відомості про ОНМ
        /// </summary>
        public Realty realty { get; set; }                                      // 
    }
    /// <summary>
    /// Зв’язок з записом з реєстрами до 2013 р.
    /// </summary>
    public class EntityLink                                                     // 
    {/// <summary>
     /// Номер
     /// </summary>
        [JsonProperty("rpvnReID")]                                              // 
        public string rpvnReID { get; set; }
        /// <summary>
        /// Дата запису
        /// </summary>
        [JsonProperty("regDate")]                                               // 
        public string regDate { get; set; }
        /// <summary>
        /// Назва реєстру
        /// </summary>
        [JsonProperty("registryType")]                                          // 
        public string registryType { get; set; }
    }
    /// <summary>
    /// Відомості про зобов’язання 
    /// </summary>
    public class Obligations                                                    // 
    {/// <summary>
     /// Строк виконання
     /// </summary>
        [JsonProperty("execTerm")]                                              // 
        public string execTerm { get; set; }
        /// <summary>
        /// Розмір основного зобов’язання
        /// </summary>
        [JsonProperty("obligationSum")]                                         // 
        public string obligationSum { get; set; }
        /// <summary>
        /// Тип валюти
        /// </summary>
        [JsonProperty("CurrencyType")]                                          // 
        public string CurrencyType { get; set; }
        /// <summary>
        /// Розмір основного зобов’язання
        /// </summary>
        [JsonProperty("obligationSumText")]                                     // 
        public string obligationSumText { get; set; }
        /// <summary>
        /// Тип валюти
        /// </summary>
        [JsonProperty("currencyText")]                                          // 
        public string currencyText { get; set; }
        /// <summary>
        /// Строк виконання
        /// </summary>
        [JsonProperty("execTermText")]                                          // 
        public string execTermText { get; set; }
    }
    /// <summary>
    /// Відомості з реестру РПВН
    /// </summary>
    public class oldRealty                                                      // 
    {
        /// <summary>
     /// Реєстраційний номер майна
     /// </summary>
        [JsonProperty("RE_ID")]                                                 // 
        public string RE_ID { get; set; }
        /// <summary>
        /// Тип майна
        /// </summary>
        [JsonProperty("RE_TYPENAME")]                                           // 

        public List<Owner> owners { get; set; }

        public string RE_TYPENAME { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        [JsonProperty("ADDITIONAL")]                                            // 
        public string ADDITIONAL { get; set; }
        /// <summary>
        /// Доповнення до типу майна
        /// </summary>
        [JsonProperty("RC_APPL")]                                               // 
        public string RC_APPL { get; set; }
        /// <summary>
        /// Тип земельної ділянки
        /// </summary>
        [JsonProperty("RE_LAND_TYPE")]                                          // 
        public string RE_LAND_TYPE { get; set; }
        /// <summary>
        /// Доповнення до типу земельної ділянка
        /// </summary>
        [JsonProperty("RE_LAND_TYPE_EXTENSION")]                                // 
        public string RE_LAND_TYPE_EXTENSION { get; set; }
        /// <summary>
        /// Кадастровий номер
        /// </summary>
        [JsonProperty("CAD_NUM")]                                               // 
        public string CAD_NUM { get; set; }
        /// <summary>
        /// Загальна площа (кв.м)
        /// </summary>
        [JsonProperty("AREA_ALL")]                                              // 
        public string AREA_ALL { get; set; }
        /// <summary>
        /// Житлова площа (кв.м)
        /// </summary>
        [JsonProperty("AREA_HAB")]                                              // 
        public string AREA_HAB { get; set; }
        /// <summary>
        /// Матеріали стін
        /// </summary>
        [JsonProperty("BUILD_FROM")]                                            // 
        public string BUILD_FROM { get; set; }
        /// <summary>
        /// Площа земельної ділянки (кв.м)
        /// </summary>
        [JsonProperty("PLOT_AREA")]                                             // 
        public string PLOT_AREA { get; set; }
        /// <summary>
        /// Відсоток зносу
        /// </summary>
        [JsonProperty("DESTROY_PERCENT")]                                       // 
        public string DESTROY_PERCENT { get; set; }
        /// <summary>
        /// Загальна вартість нерухомого майна (грн.)
        /// </summary>
        [JsonProperty("CUR_PRICE")]                                             // 
        public string CUR_PRICE { get; set; }
        /// <summary>
        /// Площа самочинно збудованого (кв.м)
        /// </summary>
        [JsonProperty("SELF_BUILD_AREA")]                                       // 
        public string SELF_BUILD_AREA { get; set; }
        /// <summary>
        /// Вартість самочинно збудованого (грн.)
        /// </summary>
        [JsonProperty("SELF_BUILD_PRICE")]                                      // 
        public string SELF_BUILD_PRICE { get; set; }
        /// <summary>
        /// Технічний опис майна
        /// </summary>
        [JsonProperty("TEXT_BODY")]                                             // 
        public string TEXT_BODY { get; set; }
        /// <summary>
        /// Номер запису
        /// </summary>
        [JsonProperty("REG_NUM")]                                               // 
        public string REG_NUM { get; set; }
        /// <summary>
        /// Номер в книзі
        /// </summary>
        [JsonProperty("BOOK_NUM")]                                              // 
        public string BOOK_NUM { get; set; }
        /// <summary>
        /// Дата погашення
        /// </summary>
        [JsonProperty("ADD_DATE")]                                              // 
        public string ADD_DATE { get; set; }
        /// <summary>
        /// Підстава погашення
        /// </summary>
        [JsonProperty("ADD_REASON")]                                            // 
        public string ADD_REASON { get; set; }
        /// <summary>
        /// Адреса
        /// </summary>
        [JsonProperty("PROPADDRESS")]                                           // 
        public string PROPADDRESS { get; set; }
    }
    /// <summary>
    /// Масив об’єктів з реєстру ДРІ
    /// </summary>
    public class oldMortgageJson                                                // 
    {/// <summary>
     /// Реєстраційний номер
     /// </summary>
        [JsonProperty("OP_OP_ID")]                                              // 
        public int? OP_OP_ID { get; set; }
        /// <summary>
        /// Тип значення
        /// </summary>
        [JsonProperty("LM_TYPENAME")]                                           // 
        public string LM_TYPENAME { get; set; }
        /// <summary>
        /// Доповнення до типу
        /// </summary>
        [JsonProperty("LM_TYPE_EXTENSION")]                                     // 
        public string LM_TYPE_EXTENSION { get; set; }
        /// <summary>
        /// Дата та час реєстрації
        /// </summary>
        [JsonProperty("REG_DATE")]                                              // 
        public string REG_DATE { get; set; }
        /// <summary>
        /// Дані реєстратора
        /// </summary>
        [JsonProperty("EMP")]                                                   // 
        public string EMP { get; set; }
        /// <summary>
        /// Розмір основного зобов’язання
        /// </summary>
        [JsonProperty("CONTRACT_SUM")]                                          // 
        public string CONTRACT_SUM { get; set; }
        /// <summary>
        /// Тип валюти
        /// </summary>
        [JsonProperty("CURRENCY_TYPE")]                                         // 
        public string CURRENCY_TYPE { get; set; }
        /// <summary>
        /// Строк виконання
        /// </summary>
        [JsonProperty("EXEC_TERM")]                                             // 
        public string EXEC_TERM { get; set; }
        /// <summary>
        /// Заставна
        /// </summary>
        [JsonProperty("BONDSRNUM")]                                             // 
        public string BONDSRNUM { get; set; }
        /// <summary>
        /// Власник заставної
        /// </summary>
        [JsonProperty("BONDOWNERNAME")]                                         // 
        public string BONDOWNERNAME { get; set; }
        /// <summary>
        /// ЄДРПОУ
        /// </summary>
        [JsonProperty("BONDCODE")]                                              // 
        public string BONDCODE { get; set; }
        /// <summary>
        /// Тип суб'єкта: 1 - (фіз. особа) 2 - (юр. особа)
        /// </summary>
        [JsonProperty("BONDSBJTYPE")]                                           // 
        public string BONDSBJTYPE { get; set; }
        /// <summary>
        /// Додаткові дані по заставній
        /// </summary>
        [JsonProperty("BONDADDITIONAL")]                                        //  
        public string BONDADDITIONAL { get; set; }
        /// <summary>
        /// Дата погашення
        /// </summary>
        [JsonProperty("ADD_DATE")]                                              // 
        public string ADD_DATE { get; set; }
        /// <summary>
        /// Підстава погашення
        /// </summary>
        [JsonProperty("ADD_REASON")]                                            // 
        public string ADD_REASON { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        [JsonProperty("ADDITIONAL")]                                            // 
        public string ADDITIONAL { get; set; }
    }
    /// <summary>
    /// Відомості з реєстру ЄРЗ
    /// </summary>
    public class oldLimitationJson                                              // 
    {/// <summary>
     /// Реєстраційний номер
     /// </summary>
        [JsonProperty("OP_OP_ID")]                                              // 
        public int? OP_OP_ID { get; set; }
        /// <summary>
        /// Тип значення
        /// </summary>
        [JsonProperty("LM_TYPENAME")]                                           // 
        public string LM_TYPENAME { get; set; }
        /// <summary>
        /// Доповнення до типу
        /// </summary>
        [JsonProperty("LM_TYPE_EXTENSION")]                                     // 
        public string LM_TYPE_EXTENSION { get; set; }
        /// <summary>
        /// Дата та час реєстрації
        /// </summary>
        [JsonProperty("REG_DATE")]                                              // 
        public string REG_DATE { get; set; }
        /// <summary>
        /// Дані реєстратора
        /// </summary>
        [JsonProperty("EMP")]                                                   // 
        public string EMP { get; set; }
        /// <summary>
        /// Дата погашення
        /// </summary>
        [JsonProperty("ADD_DATE")]                                              // 
        public string ADD_DATE { get; set; }
        /// <summary>
        /// Підстава погашення
        /// </summary>
        [JsonProperty("ADD_REASON")]                                            // 
        public string ADD_REASON { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        [JsonProperty("DATEADDITIONAL")]                                        // 
        public string DATEADDITIONAL { get; set; }
        /// <summary>
        /// Термін дії
        /// </summary>
        [JsonProperty("ACT_TERM")]                                              // 
        public string ACT_TERM { get; set; }
        /// <summary>
        /// Архівний номер
        /// </summary>
        [JsonProperty("ARCHIVE_NUM")]                                           // 
        public string ARCHIVE_NUM { get; set; }
        /// <summary>
        /// Архівна дата
        /// </summary>
        [JsonProperty("ARCHIVE_DATE")]                                          // 
        public string ARCHIVE_DATE { get; set; }
        /// <summary>
        /// Заявник
        /// </summary>
        [JsonProperty("REQUESTOR")]                                             // 
        public string REQUESTOR { get; set; }
        /// <summary>
        /// Дата виникнення (день)
        /// </summary>
        [JsonProperty("START_DAY")]                                             // 
        public string START_DAY { get; set; }
        /// <summary>
        /// Дата виникнення (місяць)
        /// </summary>
        [JsonProperty("START_MONTH")]                                           // 
        public string START_MONTH { get; set; }
        /// <summary>
        /// Дата виникнення (рік)
        /// </summary>
        [JsonProperty("START_YEAR")]                                            // 
        public string START_YEAR { get; set; }
    }
    /// <summary>
    /// ДЗК + НГО + Просторові дані
    /// </summary>
    public class ElasticPlot                                                    // 
    {/// <summary>
     /// Кадастровий номер
     /// </summary>
        public string complexNumber { get; set; }                               // 
        /// <summary>
        /// Площа
        /// </summary>
        public double? area { get; set; }                                       // 
        //public ElasticPlotGeometry geometry { get; set; }                     // Видалено. Геопросторові координати ділянки
        /// <summary>
        /// Відповідність регіону
        /// </summary>
        public bool? isCorrectRegion { get; set; }                              // 
        /// <summary>
        /// Відповідність просторовим даним
        /// </summary>
        public bool? isGeomValid { get; set; }                                  // 
        /// <summary>
        /// Код за КОАТУУ
        /// </summary>
        public string koatuu { get; set; }                                      // 
        /// <summary>
        /// Відомості про земельну ділянку за ДЗК 
        /// </summary>
        public DzkLandInfo dzkLandInfo { get; set; }                            // 
        /// <summary>
        /// Основні скорочені дані з РРП
        /// </summary>
        public RrpLandInfo rrpLandInfo { get; set; }                            // 
        /// <summary>
        /// Відповідність площі (Площі ДЗК і РРП співпадають / Площі не співпадають)
        /// </summary>
        public bool? vidpovidnistPloshchi { get; set; }                         // 
        /// <summary>
        /// Корректність розташування ділянки (Коректне розташування / Некоректне розташування)
        /// </summary>
        public bool? korrektnistRoztashuvannyai { get; set; }                   // 
        /// <summary>
        /// Категорія цільового призначення ділянки за довідником
        /// </summary>
        public int? kategoriya { get; set; }                                    // 
        /// <summary>
        /// Цільове призначення ділянки за довідником
        /// </summary>
        public int? tsilovePriznachennya { get; set; }                          // 
        /// <summary>
        /// Тип угідя
        /// </summary>
        public int? vidUgiddya { get; set; }                                    // 
        /// <summary>
        /// Форма власності
        /// </summary>
        public int? formaVlasnosti { get; set; }                                // 
        /// <summary>
        /// Власники ділянок (null - Відомості відсутні; 1 - Власник; 2 -Розпорядник (VlasnikiDilyanokDict))
        /// </summary>
        public int? vlasnikiDilyanok { get; set; }                              // 
        /// <summary>
        /// Кількість власників (1 власник/2 власники/3 власники/Більше 3х власників/Немає даних про власників)
        /// </summary>
        public int? kilkistVlasnikiv { get; set; }                              // 
        /// <summary>
        /// Тип права користування (Оренди/Суборенди/Емфітевзис/Сервітут/Спільного користування/Найму)
        /// </summary>
        public int? tipPravaKoristuvannya { get; set; }                         // 
        /// <summary>
        /// Право користування в розрізі Орендарів (Наявні дані про право користування/Дані про право користування відсутні)
        /// </summary>
        public bool? pravoKoristuvannyaOrendariv { get; set; }                  // 
        /// <summary>
        /// Право користування в розрізі Орендарів (Наявні дані про право користування/Дані про право користування відсутні)
        /// </summary>
        public int? terminDiyiOrendi { get; set; }                              // 
        /// <summary>
        /// Наявність судових справ
        /// </summary>
        public int? sudovikhSprav { get; set; }                                 // 
        /// <summary>
        /// Наявність відкритих судових справ
        /// </summary>
        public int? sudovikhSpravVidkryto { get; set; }                         // 
        /// <summary>
        /// Обмеження та обтяження
        /// </summary>
        public int? obmezhennyaObtyazhennya { get; set; }                       // 
        /// <summary>
        /// НГО 
        /// </summary>
        public ElasticNGOModel ngo { get; set; }                                // 
    }

    /// <summary>
    /// Основні скорочені дані з РРП
    /// </summary>
    public class RrpLandInfo                                                    // 
    {/// <summary>
     /// realty.groundArea.targetPurpose
     /// </summary>
        public string purpose { get; set; }                                     // 
        /// <summary>
        /// realty.groundArea.area через метод GetAreaFromRealEstateAdvice(StateRegisterRealEstateModel realEstate)
        /// </summary>
        public double? area { get; set; }                                       // 
        /// <summary>
        /// realty.irps.regDate
        /// </summary>
        public DateTime? irpsRegDate { get; set; }                              // 
        /// <summary>
        /// realty.limitation.regDate
        /// </summary>
        public DateTime? limitationRegDate { get; set; }                        // 
        /// <summary>
        /// realty.mortgage.regDate
        /// </summary>
        public DateTime? mortgageRegDate { get; set; }                          // 
        /// <summary>
        /// realty.irps.actTerm
        /// </summary>
        public DateTime? irpsEndDate { get; set; }                              // 
        /// <summary>
        /// realty.limitation.actTerm
        /// </summary>
        public DateTime? limitationEndDate { get; set; }                        // 
        /// <summary>
        /// realty.mortgage.actTerm
        /// </summary>
        public DateTime? mortgageEndDate { get; set; }                          // 
        /// <summary>
        /// properties.subjects, irps.subjects, limitation.subjects, mortgage.subjects, 
        /// </summary>
        public List<RrpLandInfoSubject> subject { get; set; }                   // 
    }
    /// <summary>
    /// properties.subjects, irps.subjects, limitation.subjects, mortgage.subjects, 
    /// </summary>
    public class RrpLandInfoSubject                                             // 
    {/// <summary>
     /// Системна інформація Vkursi
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public string sbjRlName { get; set; }                                   // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public string type { get; set; }                                        // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public bool? isOwner { get; set; }                                      // 
        /// <summary>
        /// Системна інформація Vkursi
        /// [1 -  properties.subjects]
        /// [2 -  irps.subjects]
        /// [3 -  limitation.subjects]
        /// [4 -  mortgage.subjects]
        /// </summary>
        public int? subjectNestedTypeId { get; set; }                           // 
        
        
        
        
    }
    /// <summary>
    /// НГО 
    /// </summary>
    public class ElasticNGOModel                                                // 
    {/// <summary>
     /// Оціночна площа
     /// </summary>
        public double? area { get; set; }                                       // 
        /// <summary>
        /// Дата перевірки НГО
        /// </summary>
        public DateTime? dateModify { get; set; }                               // 
        /// <summary>
        /// Тип власності
        /// </summary>
        public string ownershipType { get; set; }                               // 
        /// <summary>
        /// Ціна
        /// </summary>
        public double? price { get; set; }                                      // 
        /// <summary>
        /// Ціна за гектар
        /// </summary>
        public double? pricePerGekt { get; set; }                               // 
        /// <summary>
        /// Призначення
        /// </summary>
        public string purpose { get; set; }                                     // 
        /// <summary>
        /// Призначення int
        /// </summary>
        public int? purposeInt { get; set; }                                    // 
    }
    /// <summary>
    /// Геопросторові координати ділянки
    /// </summary>
    public class ElasticPlotGeometry                                            // 
    {/// <summary>
     /// Координати ділянки
     /// </summary>
        public List<Coordinate> coordinates { get; set; }                       // 
        /// <summary>
        /// Тип геометрії
        /// </summary>
        public string geometryType { get; set; }                                // 
    }
    /// <summary>
    /// Координати ділянки
    /// </summary>
    public class Coordinate                                                     // 
    {/// <summary>
    /// ???
    /// </summary>
        public double? x { get; set; }                                          // 
        /// <summary>
        /// ???
        /// </summary>
        public double? y { get; set; }                                          // 
    }
    /// <summary>
    /// Відомості про земельну ділянку за ДЗК 
    /// </summary>
    public class DzkLandInfo                                                    // 
    {/// <summary>
     /// Дата оновлення ділянки за ДЗК
     /// </summary>
        public DateTime UpdateDate { get; set; }                                // 
        /// <summary>
        /// Цільове призначення
        /// </summary>
        public string Purpose { get; set; }                                     // 
        /// <summary>
        /// Цільове призначення INT
        /// </summary>
        public int? PurposeNum { get; set; }                                    // 
        /// <summary>
        /// Форма власності
        /// </summary>
        public string Ownership { get; set; }                                   // 
        /// <summary>
        /// Форма власності
        /// </summary>
        public int? OwnershipNum { get; set; }                                  // 
        /// <summary>
        /// Площа земельної ділянки
        /// </summary>
        public string LandArea { get; set; }                                    // 
        /// <summary>
        /// Площа земельної ділянки
        /// </summary>
        public float? LandAreaNum { get; set; }                                 // 
        /// <summary>
        /// Місце розташування
        /// </summary>
        public string Location { get; set; }                                    // 
        /// <summary>
        /// Нормативно грошова оцінка (ПККУ) 
        /// </summary>
        public RegulatoryMonetaryValuationClass RegulatoryMonetaryValuation { get; set; }   // 
        /// <summary>
        /// Відомості про суб'єктів права власності на земельну ділянку
        /// </summary>
        public List<OwnershipInfoClass> OwnershipInfo { get; set; }             // 
        /// <summary>
        /// Відомості про суб'єктів права власності на земельну ділянку
        /// </summary>
        public List<OwnershipInfoClass> SubjectRealRightLand { get; set; }      // 
        /// <summary>
        /// Відомості про зареєстроване обмеження у використанні земельної ділянки
        /// </summary>
        public RestrictionInfoClass RestrictionInfo { get; set; }               // 
    }
    /// <summary>
    /// Нормативно грошова оцінка (ПККУ) 
    /// </summary>
    public class RegulatoryMonetaryValuationClass                               // 
    {/// <summary>
     /// Значення, гривень
     /// </summary>
        public string ValueUah { get; set; }                                    // 
        /// <summary>
        /// Дата оцінки ділянки
        /// </summary>
        public string DateEvaluation { get; set; }                              // 
    }/// <summary>
     /// Відомості про суб'єктів права власності на земельну ділянку
     /// </summary>

    public class OwnershipInfoClass                                             // 
    {/// <summary>
     /// Вид речового права
     /// </summary>
        public string PropertyRight { get; set; }                               // 
        /// <summary>
        /// Найменування юридичної особи
        /// </summary>
        public string NameFo { get; set; }                                      // 
        /// <summary>
        /// Прізвище, ім'я та по батькові фізичної особи
        /// </summary>
        public string NameUo { get; set; }                                      // 
        /// <summary>
        /// Код ЄДРПОУ юридичної особи
        /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// Дата державної реєстрації права (в державному реєстрі прав)
        /// </summary>
        public string DateRegRight { get; set; }                                // 
        /// <summary>
        /// Номер запису про право (в державному реєстрі прав)
        /// </summary>
        public string EntryRecordNumber { get; set; }                           // 
        /// <summary>
        /// Орган, що здійснив державну реєстрацію права (в державному реєстрі прав)
        /// </summary>
        public string RegAuthority { get; set; }                                // 
        /// <summary>
        /// Площа, на яку поширюється право суборенди
        /// </summary>
        public string AreaCoveredSublease { get; set; }                         // 
    }
    /// <summary>
    /// Відомості про зареєстроване обмеження у використанні земельної ділянки
    /// </summary>
    public class RestrictionInfoClass                                           // 
    {/// <summary>
     /// Вид обмеження
     /// </summary>
        public string RestrictionType { get; set; }                             // 
        /// <summary>
        /// Дата державної реєстрації обмеження
        /// </summary>
        public string RestrictionDate { get; set; }                             // 
    }

    public class Owner
    {
        public string DC_SBJ_TYPE { get; set; }
        public DateTime? RESHDATE { get; set; }
        public string NAME { get; set; }
        public string OSOW_TYPE { get; set; }
        public string OSPART { get; set; }
        public string PVDOC { get; set; }
        public string Code { get; set; }
        public string OWN_PR_SUBTYPE { get; set; }
    }

}