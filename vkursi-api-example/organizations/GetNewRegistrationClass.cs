using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations.GetNaisOrganizationInfoWithEcp;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetNewRegistrationClass
    {
        /*
        
        15. Новий бізнес. Запит на отримання списку новозареєстрованих фізичних та юридичних осіб
        [POST] /api/1.0/organizations/getnewregistration

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"DateReg":"23.11.2021","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true}'

        Приклад відповіді (скорочена версія IsShortModel = true) 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseShort.json
        Модель відповіді (скорочена версія IsShortModel = true) 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetNewRegistrationClass.cs#L161

        Приклад відповіді (повна версія IsShortModel = false)
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseFull.json 
        Модель відповіді (повна версія IsShortModel = false)
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L130
        */

        public static List<GetAdvancedOrganizationResponseModel> GetNewRegistration(ref string token, string dateReg, string type,
            int? skip, int? take, bool? IsShortModel = false, bool? isReturnAll = null, bool? changesOnly = null)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetNewRegistrationRequestBodyModel GNRRequestBody = new GetNewRegistrationRequestBodyModel
                {
                    DateReg = dateReg,                          // Дата державної реєстрації (фізичної або юридичної особи)
                    Type = type,                                // Тип особи (1 - юридична особа/ 2 - фізичної особа)
                    Skip = skip,                                // К-ть записів які траба пропустити
                    Take = take,                                // К-ть записів які траба взяти (якщо null будуть передані всі записи)
                    IsShortModel = false,                       // Коротка або повна модель відповіді
                    IsReturnAll = isReturnAll,                  // Повернуті всі записи або тільки ті які раніше не отримували по API (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
                    ChangesOnly = changesOnly,                  // Пармаметр який вказує повертати компанії у яких відбулись зміни в конкретну дату = true. Нові компанії = false
                };

                string body = JsonConvert.SerializeObject(GNRRequestBody);      // Example Body: {"DateReg":"29.10.2019","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration");
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
                    Console.WriteLine("За вказаними параметрами данных не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("dateReg less than a month ago"))
                {
                    Console.WriteLine("За вказаними параметрами данных не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    responseString = null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    responseString = null;
                }
            }

            List<GetAdvancedOrganizationResponseModel> NewRegistrationFullList = new List<GetAdvancedOrganizationResponseModel>();

            List<GetNewRegistrationResponseShortModel> GetNewRegistrationResponseShortList = new List<GetNewRegistrationResponseShortModel>();

            if ((IsShortModel == null || IsShortModel == false) && (responseString != "{\"status\":\"11. Запит успішно виконано. Дані не знайдено\",\"statusId\":11,\"isSuccess\":true}"))
            {
                NewRegistrationFullList = JsonConvert.DeserializeObject<List<GetAdvancedOrganizationResponseModel>>(responseString);
            }
            else if (IsShortModel == true)
            {
                GetNewRegistrationResponseShortList = JsonConvert.DeserializeObject<List<GetNewRegistrationResponseShortModel>>(responseString);
            }

            // Приклад відповіді(скорочена версія IsShortModel = true)
            // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseShort.json
            // Модель відповіді(скорочена версія IsShortModel = true)
            // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetNewRegistrationClass.cs#L161

            // Приклад відповіді(повна версія IsShortModel = false)
            // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNewRegistrationResponseFull.json 
            // Модель відповіді(повна версія IsShortModel = false)
            // https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L130

            return NewRegistrationFullList;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"DateReg\":\"29.10.2019\",\"Type\":\"1\",\"Skip\":0,\"Take\":10,\"IsShortModel\":true,\"IsReturnAll\":true}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getnewregistration", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"DateReg\":\"29.10.2019\",\"Type\":\"1\",\"Skip\":0,\"Take\":10,\"IsShortModel\":true,\"IsReturnAll\":true}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getnewregistration")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetNewRegistrationRequestBodyModel                             // 
    {/// <summary>
     /// Дата державної реєстрації (фізичної або юридичної особи)
     /// </summary>
        [JsonProperty("DateReg", NullValueHandling = NullValueHandling.Ignore)]
        public string DateReg { get; set; }                                     // 
        /// <summary>
        /// Тип особи (1 - юридична особа / 2 - фізичної особа)
        /// </summary>
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }                                        // 
        /// <summary>
        /// К-ть записів які траба пропустити
        /// </summary>
        [JsonProperty("Skip", NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }                                          // 
        /// <summary>
        /// К-ть записів які траба взяти
        /// </summary>
        [JsonProperty("Take", NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }                                          // 
        /// <summary>
        /// Коротка або повна модель відповіді
        /// </summary>
        [JsonProperty("IsShortModel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsShortModel { get; set; }                                 // 
        /// <summary>
        /// Повернуті всі записи або тільки ті які раніше не отримували по API (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
        /// </summary>
        [JsonProperty("IsReturnAll", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReturnAll { get; set; }                                  // 

        /// <summary>
        /// Тільки зміни
        /// </summary>
        [JsonProperty("ChangesOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ChangesOnly { get; set; }
    }
    /// <summary>
    /// Модель відповіді GetNewRegistration скорочена
    /// </summary>
    public class GetNewRegistrationResponseShortModel                           // 
    {/// <summary>
     /// Системний id сервісу Vkursi
     /// </summary>
        public int Id { get; set; }                                             // 
        /// <summary>
        /// Стан реєстрації (Dictionary.OrganizationStateDict)
        /// </summary>
        public int? State { get; set; }                                         // 
        /// <summary>
        /// Стан реєстрації текст
        /// </summary>
        public string State_text { get; set; }                                  // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public string Registration_date { get; set; }                           // 
        /// <summary>
        /// Тип особи (1 - юридична особа / 2 - фізичної особа)
        /// </summary>
        public int Type { get; set; }                                           // 
        /// <summary>
        /// Скорочене найменування
        /// </summary>
        public string Short_name { get; set; }                                  // 
        /// <summary>
        /// Повне найменування
        /// </summary>
        public string Full_name { get; set; }                                   // 
        /// <summary>
        /// Назва організаційно правової форми власності
        /// </summary>
        public string Olf_name { get; set; }                                    // 
        /// <summary>
        /// ???
        /// </summary>
        public string Display_name { get; set; }                                // 
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }                                        // 
        /// <summary>
        /// Інформація про КВЕД
        /// </summary>
        public OrganizationaisPrimaryActivityKind Activity { get; set; }        // 
        /// <summary>
        /// Інформація про КВЕД
        /// </summary>
        public class OrganizationaisPrimaryActivityKind                         // 
        {/// <summary>
         /// Назва КВЕД
         /// </summary>
            public string name { get; set; }                                    // 
            /// <summary>
            /// Код КВЕД
            /// </summary>
            public string code { get; set; }                                    // 
            /// <summary>
            /// Дані про реєстраційний номер платника єдиного внеску
            /// </summary>
            public string reg_number { get; set; }                              // 
            /// <summary>
            /// Дані про клас ризику
            /// </summary>
            [JsonProperty("class")]
            public string classProp { get; set; }                               // 
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }                                       // 
        /// <summary>
        /// Номери телефонів
        /// </summary>
        public string[] Phones { get; set; }                                    // 
        /// <summary>
        /// Адреса повна
        /// </summary>
        public string Location_full { get; set; }                               // 
        /// <summary>
        /// Адреса детальна
        /// </summary>
        public OrganizationaisAddress Location_parts { get; set; }              // 
        /// <summary>
        /// Адреса детальна
        /// </summary>
        public class OrganizationaisAddress                                     // 
        {/// <summary>
         /// Поштовий індекс
         /// </summary>
            public string zip { get; set; }                                     // 
            /// <summary>
            /// Назва країни
            /// </summary>
            public string country { get; set; }                                 // 
            /// <summary>
            /// Адреса
            /// </summary>
            public string address { get; set; }                                 // 
            /// <summary>
            /// Детальна адреса
            /// </summary>
            public OrganizationaisParts parts { get; set; }                     // 
            /// <summary>
            /// Детальна адреса
            /// </summary>
            public class OrganizationaisParts                                   // 
            {/// <summary>
             /// Адміністративна територіальна одиниця
             /// </summary>
                public string atu { get; set; }                                 // 
                /// <summary>
                /// Вулиця
                /// </summary>
                public string street { get; set; }                              // 
                /// <summary>
                /// Тип будівлі ('буд.', 'інше')
                /// </summary>
                public string house_type { get; set; }                          // 
                /// <summary>
                /// 
                /// </summary>
                public string house { get; set; }                               // 
                /// <summary>
                /// Тип будівлі
                /// </summary>
                public string building_type { get; set; }                       // 
                /// <summary>
                /// Номер будівлі
                /// </summary>
                public string building { get; set; }                            // 
                /// <summary>
                /// Тип приміщення
                /// </summary>
                public string num_type { get; set; }                            // 
                /// <summary>
                /// Номер приміщення
                /// </summary>
                public string num { get; set; }                                 // 
            }
        }
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public OrganizationAdviceFullApiShortDirectorModel Ceo_name { get; set; }// 
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public class OrganizationAdviceFullApiShortDirectorModel                // 
        {/// <summary>
         /// Призвице керівника
         /// </summary>
            public string first_name { get; set; }                              // 
            /// <summary>
            /// Ім'я керівника 
            /// </summary>
            public string last_name { get; set; }                               //             
            /// <summary>
            /// По батькові керівника
            /// </summary>
            public string middle_name { get; set; }                             // 
        }
    }

    public class OrganizationAdviceFullApiModel
    {
        /// <summary>
        /// Основні дані організації з ЄДР (Nais)
        /// </summary>
        public OrganizationaisElasticModel? Data { get; set; }

        /// <summary>
        /// Цифровий підпис даних
        /// </summary>
        public string? Sign { get; set; }

        /// <summary>
        /// Вихідні дані в оригінальному форматі
        /// </summary>
        public string? OriginalData { get; set; }

        /// <summary>
        /// Експрес-оцінка організації з показниками ризиків
        /// </summary>
        public OrganizationGageModel? ExpressScore { get; set; }

        /// <summary>
        /// Аналітика судових рішень, пов'язаних з організацією
        /// </summary>
        public CourtDecisionAnaliticViewModel? CourtAnalytic { get; set; }

        /// <summary>
        /// Дата реєстрації ІПН (індивідуального податкового номера)
        /// </summary>
        public DateTime? DateRegInn { get; set; }

        /// <summary>
        /// Індивідуальний податковий номер (ІПН)
        /// </summary>
        public string? Inn { get; set; }

        /// <summary>
        /// Дата анулювання ІПН (індивідуального податкового номера)
        /// </summary>
        public DateTime? DateCanceledInn { get; set; }

        /// <summary>
        /// Код КОАТУУ (Класифікатор об'єктів адміністративно-територіального устрою України)
        /// </summary>
        public string? Koatuu { get; set; }

        /// <summary>
        /// Інформація про головну організацію (якщо поточна організація є філією)
        /// </summary>
        public OrganizationAdviceFullApiModel? BranchMaster { get; set; }

        /// <summary>
        /// Посилання на сторінку з інформацією про організацію
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// Посилання на архів з усіма даними про організацію
        /// </summary>
        public string? HrefAllDataZip { get; set; }

        /// <summary>
        /// Посилання на PDF-документ з даними ЄДР про організацію
        /// </summary>
        public string? HrefEdrDataPdf { get; set; }
    }
    /// <summary>
    /// Відомості з ЕДР (Nais) по компанії (опис з офіційного Nais)
    /// </summary>
    public class OrganizationaisElasticModel
    {
        /// <summary>
        /// Унікальний ідентифікатор суб’єкта 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Стан суб’єкта (див.довідник: х)
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        /// Текстове відображення стану суб’єкта
        /// </summary>
        public string? state_text { get; set; }
        public string? country { get; set; }
        /// <summary>
        /// ЄДРПОУ
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// Найменування юридичної особи
        /// </summary>
        public OrganizationaisNamesModel? names { get; set; }
        /// <summary>
        /// Код організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        /// </summary>
        public string? olf_code { get; set; }
        /// <summary>
        /// Назва ОПФ суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        /// </summary>
        public string? olf_name { get; set; }
        /// <summary>
        /// Тип ОПФ
        /// </summary>
        public string? olf_subtype { get; set; }
        /// <summary>
        /// Назва установчого документа, якщо суб’єкт – юридична особа
        /// </summary>
        public string? founding_document { get; set; }
        public string? founding_document_code { get; set; }
        /// <summary>
        /// Діяльність на підставі: «1» - власного установчого документа «2» - модельного статуту (якщо суб’єкт юридична особа)
        /// </summary>
        public string? founding_document_type { get; set; }
        /// <summary>
        /// Назва установчого документа (якщо суб’єкт юридична особа)
        /// </summary>
        public string? founding_document_name { get; set; }
        /// <summary>
        /// Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
        /// </summary>
        public OrganizationaisExecutivePower? executive_power { get; set; }
        /// <summary>
        /// Місцезнаходження реєстраційної справи
        /// </summary>
        public string? object_name { get; set; }
        /// <summary>
        /// Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
        /// </summary>
        public List<OrganizationaisFounders>? founders { get; set; }
        public OrganizationaisFoundersGeneralInfo? founders_general_info { get; set; }
        /// <summary>
        /// Перелік КБВ юридичної особи
        /// </summary>
        public List<OrganizationaisFounders>? beneficiaries { get; set; }
        public OrganizationaisBeneficiariesGeneralInfo? beneficiaries_general_info { get; set; }
        /// <summary>
        /// Перелік відокремлених підрозділів юридичної особи
        /// </summary>
        public List<OrganizationaisBranches>? branches { get; set; }
        /// <summary>
        /// Дані про розмір статутного капіталу (статутного або складеного капіталу) та дату закінчення його формування, якщо суб’єкт – юридична особа
        /// </summary>
        public OrganizationaisAuthorisedCapital? authorised_capital { get; set; }
        /// <summary>
        /// Відомості про органи управління юридичної особи (вищий, виконавчий, інший)
        /// </summary>
        public string? management { get; set; }
        /// <summary>
        /// Найменування розпорядчого акту, якщо суб’єкт – юридична особа
        /// </summary>
        public string? managing_paper { get; set; }
        /// <summary>
        /// Дані про наявність відмітки про те, що юридична особа створюється та діє на підставі модельного статуту
        /// </summary>
        public bool? is_modal_statute { get; set; }
        /// <summary>
        /// Перелік видів економічної діяльності
        /// </summary>
        public List<OrganizationaisActivityKinds>? activity_kinds { get; set; }
        /// <summary>
        /// Прізвище, ім’я, по батькові, посада, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
        /// </summary>
        public List<OrganizationaisHeads>? heads { get; set; }
        /// <summary>
        /// Адреса юридичної особи або ФОП
        /// </summary>
        public OrganizationaisAddress? address { get; set; }
        /// <summary>
        /// Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        /// </summary>
        public OrganizationaisRegistration? registration { get; set; }
        /// <summary>
        /// Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
        /// </summary>
        public OrganizationaisBankruptcy? bankruptcy { get; set; }
        /// <summary>
        /// Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
        /// </summary>
        public OrganizationaisTermination? termination { get; set; }
        /// <summary>
        /// Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
        /// </summary>
        public OrganizationaisTerminationCancel? termination_cancel { get; set; }
        /// <summary>
        /// Дані про юридичних осіб – правонаступників
        /// </summary>
        public List<OrganizationaisAssignees>? assignees { get; set; }
        /// <summary>
        /// Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа
        /// </summary>
        public List<OrganizationaisAssignees>? predecessors { get; set; }
        /// <summary>
        /// Відомості, отримані в порядку взаємодії з інформаційними системами органів державної влади
        /// </summary>
        public List<OrganizationaisRegistrations>? registrations { get; set; }
        /// <summary>
        /// Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
        /// </summary>
        public OrganizationaisPrimaryActivityKind? primary_activity_kind { get; set; }
        /// <summary>
        /// Термін, до якого суб’єкт перебуває на обліку в податкових органах за місцем попередньої реєстрації (у разі зміни місцезнаходження)
        /// </summary>
        public string? prev_registration_end_term { get; set; }
        /// <summary>
        /// Контактна інформація
        /// </summary>
        public OrganizationaisContacts? contacts { get; set; }
        /// <summary>
        /// Дата відкриття виконавчого провадження (для незавершених виконавчих проваджень)
        /// </summary>
        public List<string>? open_enforcements { get; set; }
        /// <summary>
        /// Відомості про структуру власності юридичної особи
        /// </summary>
        public PropertyStruct property_struct { get; set; }

        /// <summary>
        /// Системне поле Vkursi: hash унікальності
        /// </summary>
        public Guid? hash { get; set; }
        /// <summary>
        /// Системне поле Vkursi: дата отримання запису
        /// </summary>
        public DateTime? createDate { get; set; }
        /// <summary>
        /// Системне поле Vkursi: дата зміни запису
        /// </summary>
        public DateTime? modifiedDate { get; set; }
    }
    public class OrganizationaisFoundersGeneralInfo
    {
        /// <summary>
        /// Признак ведення обліку засновників
        /// </summary>
        public bool? accounting { get; set; }

        /// <summary>
        /// Дата початку обліку засновників
        /// </summary>
        public DateTime? accounting_start_date { get; set; }

        /// <summary>
        /// Дата завершення обліку засновників
        /// </summary>
        public DateTime? accounting_end_date { get; set; }
    }
    public class OrganizationaisBeneficiariesGeneralInfo
    {
        /// <summary>
        /// Ознака виключення кінцевих бенефіціарів
        /// </summary>
        public bool? excluded { get; set; }

        /// <summary>
        /// Признак відсутності кінцевих бенефіціарів
        /// </summary>
        public bool? is_missing { get; set; }

        /// <summary>
        /// Причина відсутності кінцевих бенефіціарів
        /// </summary>
        public string? reason { get; set; }
    }
}