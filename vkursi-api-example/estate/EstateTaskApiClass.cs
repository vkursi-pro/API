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

        public static EstateCreateTaskApiResponseBodyModel EstateCreateTaskApi(
            ref string token,
            List<string> cadastrs = null,
            List<string> koatuus = null,
            List<string> edrpous = null,
            List<string> ipns = null,
            bool calculateCost = true,
            bool isNeedUpdateAll = false,
            string taskName = "Назва задачі",
            EstateApiCreateTaskParamRequest paramRequest = null)
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
                    TaskName = taskName                 // Назва задачі (обов'язково)
                    // isDzkOnly                        // Перевірка ДЗК + НГО без РРП
                };


                string body = JsonConvert.SerializeObject(ECTARequestBodyRow,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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

            EstateCreateTaskApiResponseBodyModel ECTAResponseBody = 
                JsonConvert.DeserializeObject<EstateCreateTaskApiResponseBodyModel>(responseString);

            return ECTAResponseBody;

            //if (ECTAResponseBody.isSuccess == true) // ECTAResponseBody.isSuccess = true - задача створена успішно
            //{
            //    return ECTAResponseBody.taskId;     // Id задачі за яким ми будемо перевіряти її виконання
            //}
            //else
            //{
            //    Console.WriteLine("error: {0}", ECTAResponseBody.status);
            //    return null;

            //    /* ECTAResponseBody.status = "Not enough money" - недостатньо коштів
            //     * ECTAResponseBody.status = "Unexpected server error" - непередвачувана помилка
            //     */
            //}

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

        public static List<GetEstateTaskListResponseBodyModel> GetEstateTaskList(ref string token)
        {            
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatetasklist");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", $"Bearer {token}");

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

            List<GetEstateTaskListResponseBodyModel> GETLResponseBody = 
                JsonConvert.DeserializeObject<List<GetEstateTaskListResponseBodyModel>>(responseString);

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
    {
        /// <summary>
        /// Id задачі
        /// </summary>
        public Guid Id { get; set; }                                          // 
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
    /// РРП ДЕТАЛЬНИЙ ОПИС В https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetAdvancedRrpReportClass.cs
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

}