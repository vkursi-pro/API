using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class EstateTaskApiClass
    {
        /* 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам. Частина перша додавання в чергу
         * [POST] api/1.0/estate/estatecreatetaskapi
         * 
         */

        public static string EstateCreateTaskApi(string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
                RestRequest request = new RestRequest("api/1.0/estate/estatecreatetaskapi", Method.POST);

                EstateCreateTaskApiRequestBodyModel ECTARequestBodyRow = new EstateCreateTaskApiRequestBodyModel
                {
                    Cadastrs = new List<string>         // Кадастрові номери
                {
                    "5621287500:03:001:0019"
                },
                    Koatuus = new List<string>          // КОАТУУ (обмеження 10)
                {
                    "5621287500"
                },
                    Edrpous = new List<string>          // Коди ЄДРПОУ (обмеження 10)
                {
                    "19124549"
                },
                    Ipns = new List<string>             // Коди ІПН-и (обмеження 10)
                {
                    "3083707142"
                },
                    СalculateСost = false,              // Якщо тільки порахувати вартість
                    IsNeedUpdateAll = false,            // Якщо true - оновлюємо всі дані в ДЗК і РРП
                    IsReport = true,                    // Якщо true - формуємо звіт, false / null - повертаємо тільки json
                    TaskName = "Назва задачі"           // Назва задачі
                };

                string body = JsonConvert.SerializeObject(ECTARequestBodyRow);

                // Example Body: 

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401) 
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену");
                    token = AuthorizeClass.Authorize();
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
        }


        /* 20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)
         * [GET] api/1.0/estate/getestatetasklist
         * 
         */

        public static List<ApiTaskListEstateAnswerModel> GetEstateTaskList(string token)
        {            
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

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
                    token = AuthorizeClass.Authorize();
                }

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            List<ApiTaskListEstateAnswerModel> ApiTaskListEstateAnswerList = new List<ApiTaskListEstateAnswerModel>();

            ApiTaskListEstateAnswerList = JsonConvert.DeserializeObject<List<ApiTaskListEstateAnswerModel>>(responseString);

            return ApiTaskListEstateAnswerList;
        }


        /* 21. Отримання інформації про виконання формування звіту та запитів до ДРРП, НГО, ДЗК за TaskId
         * [POST] api/1.0/estate/estategettaskdataapi
         * 
         */

        public static List<EstateGetTaskDataApiDataModel> EstateGetTaskDataApi(string token, string taskId)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                EstateGetTaskDataApiRequestBodyModel EGTDARequestBody = new EstateGetTaskDataApiRequestBodyModel
                {
                    taskId = taskId,        // Id задачі
                    skip = 0,               // К-ть записів які будуть пропущені
                    take = 100              // К-ть записів які будуть отримані (максимум 1000)
                };

                string body = JsonConvert.SerializeObject(EGTDARequestBody);

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estategettaskdataapi");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену");
                    token = AuthorizeClass.Authorize();
                }

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstateGetTaskDataApiResponseModel EGTDAResponse = new EstateGetTaskDataApiResponseModel();

            EGTDAResponse = JsonConvert.DeserializeObject<EstateGetTaskDataApiResponseModel>(responseString);

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
        }
    }


    // 19.
    public class EstateCreateTaskApiRequestBodyModel                            // Модель Body запиту
    {
        public List<string> Edrpous { get; set; }                               // Коди ЄДРПОУ
        public List<string> Ipns { get; set; }                                  // ІПН-и
        public List<string> Koatuus { get; set; }                               // КОАТУУ
        public List<string> Cadastrs { get; set; }                              // Кадастрові номери
        public bool? СalculateСost { get; set; }                                // Якщо тільки порахувати вартість
        public bool IsNeedUpdateAll { get; set; }                               // Якщо true - оновлюємо всі дані в ДЗК і РРП
        public bool IsReport { get; set; }                                      // Якщо true - формуємо звіт, false / null - повертаємо тільки json
        public string TaskName { get; set; }                                    // Назва задачі
        public bool DzkOnly { get; set; }                                       // Запити тільки по ДЗК
    }

    public class EstateCreateTaskApiResponseBodyModel                           // Модель відповіді EstateCreateTaskApi
    {
        public bool isSuccess { get; set; }                                     // Запит віконано успішно
        public string status { get; set; }                                      // Повідомлення
        public string taskId { get; set; }                                      // Id задачі за яким ми будемо перевіряти її виконання
        public string taskName { get; set; }                                    // Назва задачі
        public double? cost { get; set; }                                       // Вартість виконання запиту
    }

    // 20.
    public class ApiTaskListEstateAnswerModel                                   // Модель Body запиту (перелік створенних задач (задачі на виконання запитів до ДРРП, НГО, ДЗК))
    {
        public string Id { get; set; }                                          // Id задачі
        public string Name { get; set; }                                        // Назва задачі
        public DateTime DateStart { get; set; }                                 // Дата початку виконання
        public DateTime? DateEnd { get; set; }                                  // Дата закінчення виконання
        public bool Complete { get; set; }                                      // Задачу виконано (true - так / false - ні)
        public bool WithReport { get; set; }                                    // Сформовано звіт в сервісі VkursiLand (true - так / false - ні)
    }


    // 21.

    public class EstateGetTaskDataApiRequestBodyModel                           // Модель Body запиту
    {
        public string taskId { get; set; }                                      // Id задачі
        public int? skip { get; set; }                                          // К-ть записів які будуть пропущені
        public int? take { get; set; }                                          // К-ть записів які будуть отримані (максимум 1000)
    }



    public class EstateGetTaskDataApiResponseModel                              // Модель відповіді EstateGetTaskDataApi
    {
        public bool isSuccess { get; set; }                                     // Запит віконано успішно
        public string status { get; set; }                                      // Повідомлення
        public List<EstateGetTaskDataApiDataModel> data { get; set; }           // EstateGetTaskDataApiDataModel
    }

    public class EstateGetTaskDataApiDataModel
    {
        public string CadastrNumber { get; set; }                               // Кадастровий номер
        public ElasticPlot Plot { get; set; }                                   // ДЗК + ПКУУ (гео дані)
        public StateRegisterRealEstateModel RrpAdvanced { get; set; }           // РРП
    }

    public class RealEstateAdvancedResponseModel                                // 
    {
        public List<Realty> realty { get; set; }                                // 
        public List<oldRealty> oldRealty { get; set; }                          // 
        public List<oldMortgageJson> oldMortgageJson { get; set; }              // 
        public List<oldLimitationJson> oldLimitationJson { get; set; }          // 
    }

    public class StateRegisterRealEstateModel                                   // 
    {
        public List<Realty> realty { get; set; }                                // 
        public List<object> oldRealty { get; set; }                             // 
        public List<object> oldMortgageJson { get; set; }                       // 
        public List<object> oldLimitationJson { get; set; }                     // 
        public int CourtCount { get; set; }                                     // 
        public string CadastrNumber { get; set; }                               // 
        public long LocationId { get; set; }                                    // 
        public string OwnerForm { get; set; }                                   // 
        public string Address { get; set; }                                     // 
        public Guid Id { get; set; }                                            // Id из таблицы RealEstateRightsAdvanced
        public double? AreaFromFreeQuery { get; set; }                          // Площадь из бесплатного запроса

    }

    public class Realty
    {
        public string regNum { get; set; }
        public DateTime regDate { get; set; }
        public List<Irp> irps { get; set; }
        public List<Property> properties { get; set; }
        public List<GroundArea> groundArea { get; set; }
        public List<RealtyAddress> realtyAddress { get; set; }
        public string reType { get; set; }
        public string reState { get; set; }
        public string sectionType { get; set; }
        public string region { get; set; }
        public string additional { get; set; }
        public int hasProperties { get; set; }
        //public List<Mortgage> mortgage { get; set; }
        //public List<Limitation> limitation { get; set; }

        public List<Irp> mortgage { get; set; }
        public List<Irp> limitation { get; set; }

        [JsonProperty("reTypeExtension")]                               // Доповнення до типу ОНМ
        public string reTypeExtension { get; set; }

        [JsonProperty("reSubType")]                                     // Підтип ОНМ
        public string reSubType { get; set; }

        [JsonProperty("reSubTypeExtension")]                            // Доповнення до підтипу
        public string reSubTypeExtension { get; set; }

        [JsonProperty("reInCreation")]                                  // Ознака розділу, приймає значення: 1 - Розділ в процесі відкриття внаслідок поділу 2 - Розділ в процесі відкриття внаслідок виділу частки 3 - Розділ в процесі відкриття внаслідок об’єднання 4 - Розділ в процесі відкриття
        public string reInCreation { get; set; }

        [JsonProperty("isResidentialBuilding")]                         // Об’єкт житлової нерухомості: 0 - Так 1 - Ні
        public string isResidentialBuilding { get; set; }

        [JsonProperty("techDescription")]                               // Опис
        public string techDescription { get; set; }

        [JsonProperty("area")]                                          // Загальна площа (кв.м)
        public double? area { get; set; }

        [JsonProperty("livingArea")]                                    // Житлова площа (кв.м)
        public double? livingArea { get; set; }

        [JsonProperty("wallMaterial")]                                  // Площа самочинно збудованого (кв.м) 
        public string wallMaterial { get; set; }

        [JsonProperty("depreciationPercent")]                           // Відсоток зносу (%)
        public double? depreciationPercent { get; set; }

        [JsonProperty("selfBuildArea")]                                 // Площа самочинно збудованого (кв.м)
        public double? selfBuildArea { get; set; }

    }

    public class GroundArea
    {
        public string cadNum { get; set; }
        public string area { get; set; }
        public string areaUM { get; set; }
        public string targetPurpose { get; set; }
    }

    public class Property
    {
        public int rnNum { get; set; }
        public DateTime regDate { get; set; }
        public string partSize { get; set; }
        public List<Subject> subjects { get; set; }
        public List<CauseDocument> causeDocuments { get; set; }
        public string prType { get; set; }
        public string prState { get; set; }
        public string registrar { get; set; }
        public List<object> entityLinks { get; set; }
        public string operationReason { get; set; }
    }

    public class Subject
    {
        public string sbjName { get; set; }
        public string dcSbjType { get; set; }
        public string SbjSort { get; set; }
        public string countryName { get; set; }
        public string sbjRlName { get; set; }
        public string dcSbjKind { get; set; }
        public int? isOwner { get; set; }
        public string sbjCode { get; set; }
        public string isState { get; set; }
        public string additional { get; set; }

    }

    public class CauseDocument
    {
        public string cdType { get; set; }
        public string cdTypeExtension { get; set; }
        public DateTime docDate { get; set; }
        public string publisher { get; set; }
        public string @enum { get; set; }
    }

    public class RealtyAddress
    {
        public string addressDetail { get; set; }
    }

    public class Irp
    {
        public string lmType { get; set; }
        public DateTime regDate { get; set; }
        public DateTime actTerm { get; set; }
        public string objectDescription { get; set; }
        public int rnNum { get; set; }
        public List<Subject> subjects { get; set; }
        public List<CauseDocument> causeDocuments { get; set; }
        public List<Obligations> obligations { get; set; }
        public string irpSort { get; set; }
        public string irpState { get; set; }
        public string mgState { get; set; }
        public string registrar { get; set; }
        public List<object> entityLinks { get; set; }
        public string operationReason { get; set; }

        [JsonProperty("holderObj")]                                     // Організація, що зареєструвала інше речове право
        public string holderObj { get; set; }

        [JsonProperty("IrpSort")]                                       // Вид іншого речового права 
        public string IrpSort { get; set; }

        [JsonProperty("irpSortExtension")]                              // Доповнення до виду
        public string irpSortExtension { get; set; }

        [JsonProperty("irpDescription")]                                // Зміст, характеристика іншого речового права
        public string irpDescription { get; set; }

        [JsonProperty("isIndefinitely")]                                // Ознака безстрокове  1 - безстрокове / 0 - не безстрокове
        public string isIndefinitely { get; set; }

        [JsonProperty("actTermText")]                                   // Строк дії
        public string actTermText { get; set; }

        [JsonProperty("isRent")]                                        // Ознака «Піднайм»(суборенда)
        public string isRent { get; set; }

        [JsonProperty("isRightToRent")]                                 // Ознака «З правом передачі в піднайм (суборенду)» 1 - так / 0 - ні
        public string isRightToRent { get; set; }

        [JsonProperty("isRightProlongation")]                           // Ознака «З правом пролонгації» 1 - так / 0 - ні
        public string isRightProlongation { get; set; }

        public Realty realty { get; set; }

    }

    public class Obligations                                            // Відомості про зобов’язання 
    {
        [JsonProperty("execTerm")]                                      // Строк виконання
        public string execTerm { get; set; }

        [JsonProperty("obligationSum")]                                 // Розмір основного зобов’язання
        public string obligationSum { get; set; }

        [JsonProperty("CurrencyType")]                                  // Тип валюти
        public string CurrencyType { get; set; }

        [JsonProperty("obligationSumText")]                             // Розмір основного зобов’язання
        public string obligationSumText { get; set; }

        [JsonProperty("currencyText")]                                  // Тип валюти
        public string currencyText { get; set; }

        [JsonProperty("execTermText")]                                  // Строк виконання
        public string execTermText { get; set; }
    }

    public class oldRealty                                              // Відомості з реестру РПВН
    {
        [JsonProperty("RE_ID")]                                         // Реєстраційний номер майна
        public string RE_ID { get; set; }

        [JsonProperty("RE_TYPENAME")]                                   // Тип майна
        public string RE_TYPENAME { get; set; }

        [JsonProperty("ADDITIONAL")]                                    // Додаткові відомості
        public string ADDITIONAL { get; set; }

        [JsonProperty("RC_APPL")]                                       // Доповнення до типу майна
        public string RC_APPL { get; set; }

        [JsonProperty("RE_LAND_TYPE")]                                  // Тип земельної ділянки
        public string RE_LAND_TYPE { get; set; }

        [JsonProperty("RE_LAND_TYPE_EXTENSION")]                        // Доповнення до типу земельної ділянка
        public string RE_LAND_TYPE_EXTENSION { get; set; }

        [JsonProperty("CAD_NUM")]                                       // Кадастровий номер
        public string CAD_NUM { get; set; }

        [JsonProperty("AREA_ALL")]                                      // Загальна площа (кв.м)
        public string AREA_ALL { get; set; }

        [JsonProperty("AREA_HAB")]                                      // Житлова площа (кв.м)
        public string AREA_HAB { get; set; }

        [JsonProperty("BUILD_FROM")]                                    // Матеріали стін
        public string BUILD_FROM { get; set; }

        [JsonProperty("PLOT_AREA")]                                     // Площа земельної ділянки (кв.м)
        public string PLOT_AREA { get; set; }

        [JsonProperty("DESTROY_PERCENT")]                               // Відсоток зносу
        public string DESTROY_PERCENT { get; set; }

        [JsonProperty("CUR_PRICE")]                                     // Загальна вартість нерухомого майна (грн.)
        public string CUR_PRICE { get; set; }

        [JsonProperty("SELF_BUILD_AREA")]                               // Площа самочинно збудованого (кв.м)
        public string SELF_BUILD_AREA { get; set; }

        [JsonProperty("SELF_BUILD_PRICE")]                              // Вартість самочинно збудованого (грн.)
        public string SELF_BUILD_PRICE { get; set; }

        [JsonProperty("TEXT_BODY")]                                     // Технічний опис майна
        public string TEXT_BODY { get; set; }

        [JsonProperty("REG_NUM")]                                       // Номер запису
        public string REG_NUM { get; set; }

        [JsonProperty("BOOK_NUM")]                                      // Номер в книзі
        public string BOOK_NUM { get; set; }

        [JsonProperty("ADD_DATE")]                                      // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                    // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("PROPADDRESS")]                                   // Адреса
        public string PROPADDRESS { get; set; }
    }

    public class oldMortgageJson                                        // Масив об’єктів з реєстру ДРІ
    {
        [JsonProperty("OP_OP_ID")]                                      // Реєстраційний номер
        public int? OP_OP_ID { get; set; }

        [JsonProperty("LM_TYPENAME")]                                   // Тип значення
        public string LM_TYPENAME { get; set; }

        [JsonProperty("LM_TYPE_EXTENSION")]                             // Доповнення до типу
        public string LM_TYPE_EXTENSION { get; set; }

        [JsonProperty("REG_DATE")]                                      // Дата та час реєстрації
        public string REG_DATE { get; set; }

        [JsonProperty("EMP")]                                           // Дані реєстратора
        public string EMP { get; set; }

        [JsonProperty("CONTRACT_SUM")]                                  // Розмір основного зобов’язання
        public string CONTRACT_SUM { get; set; }

        [JsonProperty("CURRENCY_TYPE")]                                 // Тип валюти
        public string CURRENCY_TYPE { get; set; }

        [JsonProperty("EXEC_TERM")]                                     // Строк виконання
        public string EXEC_TERM { get; set; }

        [JsonProperty("BONDSRNUM")]                                     // Заставна
        public string BONDSRNUM { get; set; }

        [JsonProperty("BONDOWNERNAME")]                                 // Власник заставної
        public string BONDOWNERNAME { get; set; }

        [JsonProperty("BONDCODE")]                                      // ЄДРПОУ
        public string BONDCODE { get; set; }

        [JsonProperty("BONDSBJTYPE")]                                   // Тип суб'єкта: 1 - (фіз. особа) 2 - (юр. особа)
        public string BONDSBJTYPE { get; set; }

        [JsonProperty("BONDADDITIONAL")]                                // Додаткові дані по заставній 
        public string BONDADDITIONAL { get; set; }

        [JsonProperty("ADD_DATE")]                                      // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                    // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("ADDITIONAL")]                                    // Додаткові відомості
        public string ADDITIONAL { get; set; }
    }

    public class oldLimitationJson                                      // Відомості з реєстру ЄРЗ
    {
        [JsonProperty("OP_OP_ID")]                                      // Реєстраційний номер
        public int? OP_OP_ID { get; set; }

        [JsonProperty("LM_TYPENAME")]                                   // Тип значення
        public string LM_TYPENAME { get; set; }

        [JsonProperty("LM_TYPE_EXTENSION")]                             // Доповнення до типу
        public string LM_TYPE_EXTENSION { get; set; }

        [JsonProperty("REG_DATE")]                                      // Дата та час реєстрації
        public string REG_DATE { get; set; }

        [JsonProperty("EMP")]                                           // Дані реєстратора
        public string EMP { get; set; }

        [JsonProperty("ADD_DATE")]                                      // Дата погашення
        public string ADD_DATE { get; set; }

        [JsonProperty("ADD_REASON")]                                    // Підстава погашення
        public string ADD_REASON { get; set; }

        [JsonProperty("DATEADDITIONAL")]                                // Додаткові відомості
        public string DATEADDITIONAL { get; set; }

        [JsonProperty("ACT_TERM")]                                      // Термін дії
        public string ACT_TERM { get; set; }

        [JsonProperty("ARCHIVE_NUM")]                                   // Архівний номер
        public string ARCHIVE_NUM { get; set; }

        [JsonProperty("ARCHIVE_DATE")]                                  // Архівна дата
        public string ARCHIVE_DATE { get; set; }

        [JsonProperty("REQUESTOR")]                                     // Заявник
        public string REQUESTOR { get; set; }

        [JsonProperty("START_DAY")]                                     // Дата виникнення (день)
        public string START_DAY { get; set; }

        [JsonProperty("START_MONTH")]                                   // Дата виникнення (місяць)
        public string START_MONTH { get; set; }

        [JsonProperty("START_YEAR")]                                    // Дата виникнення (рік)
        public string START_YEAR { get; set; }
    }

    public class ElasticPlot                                            // ДЗК + НГО + гео дані
    {
        public string complexNumber { get; set; }
        public double? area { get; set; }
        public ElasticPlotGeometry geometry { get; set; }
        public bool? isCorrectRegion { get; set; }
        public bool? isGeomValid { get; set; }
        public string koatuu { get; set; }

        public DzkLandInfo dzkLandInfo { get; set; }
        public RrpLandInfo rrpLandInfo { get; set; }
        public bool? vidpovidnistPloshchi { get; set; }
        public bool? korrektnistRoztashuvannyai { get; set; }
        public int? kategoriya { get; set; }
        public int? tsilovePriznachennya { get; set; }
        public int? vidUgiddya { get; set; }
        public int? formaVlasnosti { get; set; }
        public int? vlasnikiDilyanok { get; set; }
        public int? kilkistVlasnikiv { get; set; }
        public int? tipPravaKoristuvannya { get; set; }
        public bool? pravoKoristuvannyaOrendariv { get; set; }
        public int? terminDiyiOrendi { get; set; }
        public int? sudovikhSprav { get; set; }
        public int? sudovikhSpravVidkryto { get; set; }
        public decimal? ngoZaDilyanku { get; set; }
        public decimal? ngoZaGektar { get; set; }
        public int? obmezhennyaObtyazhennya { get; set; }

        public ElasticNGOModel ngo { get; set; }                        // НГО 
    }


    public class RrpLandInfo
    {
        public string purpose { get; set; }            //realty.groundArea.targetPurpose
        public double? area { get; set; }             //realty.groundArea.area через метод GetAreaFromRealEstateAdvice(StateRegisterRealEstateModel realEstate)

        public DateTime? irpsRegDate { get; set; }     //realty.irps.regDate
        public DateTime? limitationRegDate { get; set; }   //realty.limitation.regDate
        public DateTime? mortgageRegDate { get; set; }     //realty.mortgage.regDate

        public DateTime? irpsEndDate { get; set; }         //realty.irps.actTerm
        public DateTime? limitationEndDate { get; set; }    //realty.limitation.actTerm
        public DateTime? mortgageEndDate { get; set; }      //realty.mortgage.actTerm

        public List<RrpLandInfoSubject> subject { get; set; }      //properties.subjects, irps.subjects, limitation.subjects, mortgage.subjects, 
    }

    public class RrpLandInfoSubject
    {
        public string name { get; set; }
        public string sbjRlName { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public bool? isOwner { get; set; }
        public int? subjectNestedTypeId { get; set; }    //для определения откуда взяли subject    
        //[1 -  properties.subjects]
        //[2 -  irps.subjects]
        //[3 -  limitation.subjects]
        //[4 -  mortgage.subjects]
    }

    public class ElasticNGOModel                                // НГО 
    {
        public double? area { get; set; }
        public DateTime? dateModify { get; set; }
        public string ownershipType { get; set; }
        public double? price { get; set; }
        public double? pricePerGekt { get; set; }
        public string purpose { get; set; }
        public int? purposeInt { get; set; }
    }

    public class ElasticPlotGeometry
    {
        public List<Coordinate> coordinates { get; set; }
        public string geometryType { get; set; }
    }

    public class Coordinate
    {
        public double? x { get; set; }
        public double? y { get; set; }
    }

    public class DzkLandInfo                                    // ДЗК - Відомості про земельну ділянку 
    {
        public DateTime UpdateDate { get; set; }
        public string Purpose { get; set; } // Цільове призначення
        public int? PurposeNum { get; set; } // Цільове призначення INT
        public string Ownership { get; set; } // Форма власності
        public int? OwnershipNum { get; set; } // Форма власності
        public string LandArea { get; set; } // Площа земельної ділянки
        public float? LandAreaNum { get; set; } // Площа земельної ділянки
        public string Location { get; set; } // Місце розташування
        public RegulatoryMonetaryValuationClass RegulatoryMonetaryValuation { get; set; }
        public List<OwnershipInfoClass> OwnershipInfo { get; set; } // Відомості про суб'єктів права власності на земельну ділянку
        public List<OwnershipInfoClass> SubjectRealRightLand { get; set; } // Відомості про суб'єктів права власності на земельну ділянку
        public RestrictionInfoClass RestrictionInfo { get; set; } // Відомості про зареєстроване обмеження у використанні земельної ділянки
    }

    public class RegulatoryMonetaryValuationClass
    {
        public string ValueUah { get; set; } // Значення, гривень
        public string DateEvaluation { get; set; } // Дата оцінки ділянки
    }

    public class OwnershipInfoClass // Відомості про суб'єктів права власності на земельну ділянку
    {
        public string PropertyRight { get; set; } // Вид речового права
        public string NameFo { get; set; } // Найменування юридичної особи
        public string NameUo { get; set; } // Прізвище, ім'я та по батькові фізичної особи
        public string Edrpou { get; set; } // Код ЄДРПОУ юридичної особи
        public string DateRegRight { get; set; } // Дата державної реєстрації права (в державному реєстрі прав)
        public string EntryRecordNumber { get; set; } // Номер запису про право (в державному реєстрі прав)
        public string RegAuthority { get; set; } // Орган, що здійснив державну реєстрацію права (в державному реєстрі прав)
        public string AreaCoveredSublease { get; set; } // Площа, на яку поширюється право суборенди
    }

    public class RestrictionInfoClass // Відомості про зареєстроване обмеження у використанні земельної ділянки
    {
        public string RestrictionType { get; set; } // Вид обмеження
        public string RestrictionDate { get; set; } // Дата державної реєстрація обмеження
    }


}