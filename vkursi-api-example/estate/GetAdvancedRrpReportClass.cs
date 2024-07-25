using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetAdvancedRrpReportClass
    {
        /*
        
        25. Отримання повного витяга з реєстру нерухомого майна (ДРРП)
        [POST] /api/1.0/Estate/GetAdvancedRrpReport

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport' \
        --header 'ContentType: application/json' \
        --header 'Content-Type: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"GroupId":5001466269723,"ObjectId":68345530}'

        */

        public static GetAdvancedRrpReportResponseModel GetAdvancedRrpReport(ref string token, long? groupId, long? objectId) 
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport");
                RestRequest request = new RestRequest(Method.POST);

                GetAdvancedRrpReportRequestBodyModel GARRResponseBodyRow = new GetAdvancedRrpReportRequestBodyModel
                {
                    GroupId = groupId,
                    ObjectId = objectId
                };

                string body = JsonConvert.SerializeObject(GARRResponseBodyRow);

                // Example Body: {"GroupId":5001466269723,"ObjectId":68345530}

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Content-Type", "application/json");
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

            GetAdvancedRrpReportResponseModel GARRRequest = new GetAdvancedRrpReportResponseModel();

            GARRRequest = JsonConvert.DeserializeObject<GetAdvancedRrpReportResponseModel>(responseString);

            return GARRRequest;

        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"GroupId\":5001466269723,\"ObjectId\":68345530}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/estate/getadvancedrrpreport", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"GroupId\":5001466269723,\"ObjectId\":68345530}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetAdvancedRrpReportRequestBodyModel
    {
        public long? GroupId { get; set; }
        public long? ObjectId { get; set; }
        /// <summary>
        /// Перелік номерів ОНМ
        /// </summary>
        public List<long> OnmNumbers { get; set; }
        /// <summary>
        /// Перелік кадастрових номерів
        /// </summary>
        public List<string> CadastrNumbers { get; set; }
    }

    public class GetAdvancedRrpReportResponseModel
    {
        public bool isSuccess { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string reportHref { get; set; }
        public string dataObjectOriginal { get; set; }
        public DataObject dataObject { get; set; }
    }

    public class DataObject
    {
        public List<Realty> realty { get; set; }
        public List<oldRealty> oldRealty { get; set; }
        public List<oldMortgageJson> oldMortgageJson { get; set; }
        public List<oldLimitationJson> oldLimitationJson { get; set; }
        public List<AllAdress> allAdresses { get; set; }
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

    public class Owner
    {
        public string DC_SBJ_TYPE { get; set; }
        public DateTime? RESHDATE { get; set; }
        public string NAME { get; set; }
        public string OSOW_TYPE { get; set; }
        public string OSPART { get; set; }
        /// <summary>
        /// Підстава виникнення права власності
        /// </summary>
        public string PVDOC { get; set; }
        public string Code { get; set; }
        public string OWN_PR_SUBTYPE { get; set; }
    }

    public class AllAdress
    {
        public string name { get; set; }
        public string regTime { get; set; }
    }
}
