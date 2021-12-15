using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetAdvancedorganizationOnlyNaisDataClass
    {
        /*

        Метод:
            87. Єдиний державний реєстр юридичних осіб, фізичних осіб-підприємців та громадських формувань
            [POST] /api/1.0/organizations/getadvancedorganizationOnlyNaisData

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganizationOnlyNaisData' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ik...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":"00131305"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetAdvancedOrganizationOnlyNaisDataResponse.json

        */

        public static GetAdvancedOrganizationOnlyNaisDataResponseModel GetAdvancedOrganizationOnlyNaisData(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetAdvancedorganizationOnlyNaisDataRequestBodyModel GAOONDRequestBody = new GetAdvancedorganizationOnlyNaisDataRequestBodyModel
                {
                    Code =  code                                                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GAOONDRequestBody);           // Example body: {"Code":"00131305"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganizationOnlyNaisData");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetAdvancedOrganizationOnlyNaisDataResponseModel GAOONDResponse = new GetAdvancedOrganizationOnlyNaisDataResponseModel();

            GAOONDResponse = JsonConvert.DeserializeObject<GetAdvancedOrganizationOnlyNaisDataResponseModel>(responseString);

            return GAOONDResponse;
        }
    }

    /*
    
    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":\"00131305\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganizationOnlyNaisData")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();


    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Code": "00131305"
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIs...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getadvancedorganizationOnlyNaisData", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
    */

    public class GetAdvancedorganizationOnlyNaisDataRequestBodyModel
    {
        public string Code { get; set; }
    }

    public class GetAdvancedOrganizationOnlyNaisDataResponseModel       // Відповідь на запит
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public GetAdvancedOrganizationOnlyNaisDataResponseData Data { get; set; }
    }

    public class GetAdvancedOrganizationOnlyNaisDataResponseData
    {
        public int id { get; set; }                                             // Унікальний ідентифікатор суб’єкта
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:64)
        public string code { get; set; }                                        // ЄДРПОУ (maxLength:10)
        public OrgNamesModel names { get; set; }                                // Назва суб’єкта
        public string olf_code { get; set; }                                    // Код організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        public string olf_name { get; set; }                                    // Назва організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        public string founding_document { get; set; }                           // Назва установчого документа, якщо суб’єкт – юридична особа (maxLength:128)
        public OrgExecutivePower executive_power { get; set; }          // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
        public string object_name { get; set; }                                 // Місцезнаходження реєстраційної справи (maxLength:256)
        public OrgFounders[] founders { get; set; }                     // Array[Founder]. Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
        public OrgBranches[] branches { get; set; }                     // Array[Branch]. Перелік відокремлених підрозділів юридичної особи
        public OrgAuthorisedCapital authorised_capital { get; set; }    // Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
        public string management { get; set; }                                  // Відомості про органи управління юридичної особи (maxLength:256)
        public string managing_paper { get; set; }                              // Найменування розпорядчого акта, якщо суб’єкт – юридична особа (maxLength:256)
        public bool? is_modal_statute { get; set; }                             // Дані про наявність відмітки про те, що юридична особа створюється та діє на підставі модельного статуту
        public OrgActivityKinds[] activity_kinds { get; set; }          // Перелік видів економічної діяльності
        public OrgNaisHeads[] heads { get; set; }                       // Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
        public OrgNaisAddress address { get; set; }                     // Адреса
        public OrgNaisRegistration registration { get; set; }           // inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        public OrgNaisBankruptcy bankruptcy { get; set; }               // inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
        public OrgNaisTermination termination { get; set; }             // inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
        public OrgNaisTerminationCancel termination_cancel { get; set; }// inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
        public OrgNaisAssignees[] assignees { get; set; }               // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrgNaisAssignees[] predecessors { get; set; }            // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrgNaisRegistrations[] registrations { get; set; }       // inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
        public OrgNaisPrimaryActivityKind primary_activity_kind { get; set; }// inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
        public string prev_registration_end_term { get; set; }                  // Термін, до якого юридична особа перебуває на обліку в органі Міндоходів за місцем попередньої реєстрації, у разі зміни місцезнаходження юридичної особи
        public OrgNaisContacts contacts { get; set; }                   // Контактні дані
        public string[] open_enforcements { get; set; }                         // Дата відкриття виконавчого провадження щодо юридичної особи (для незавершених виконавчих проваджень)
    }

    public class OrgNamesModel                                          // Назва суб’єкта
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public bool? include_olf { get; set; }                                  // Вказує, чи треба додавати організаційно-правову форму до назви, якщо суб’єкт – юридична особа
        public string display { get; set; }                                     // Назва для відображення (з ОПФ чи без, в залежності від параметру include_olf), якщо суб’єкт – юридична особа (maxLength:512)
        
        [JsonProperty("short")]
        public string shortName { get; set; }                                   // Коротка назва, якщо суб’єкт – юридична особа (maxLength:512)
        public string name_en { get; set; }                                     // Повна назва суб’єкта англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
        public string short_en { get; set; }                                    // Коротка назва англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
    }

    public class OrgExecutivePower                                              // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
    {
        public string name { get; set; }                                        // Назва (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ (maxLength:10)
    }

    public class OrgFounders                                                    // Array[Founder]. Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        public OrgNaisAddress address { get; set; }                             // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа); (maxLength:256)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт – приватна особа) (maxLength:256)
        public int? role { get; set; }                                          // Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        public string capital { get; set; }                                     // Розмір частки у статутному капіталі пов’язаного суб’єкта (лише для засновників) (maxLength:128)
    }

    public class OrgNaisAddress                                                 // Адреса
    {
        public string zip { get; set; }                                         // Поштовий індекс (maxLength:16)
        public string country { get; set; }                                     // Назва країни (maxLength:64)
        public string address { get; set; }                                     // Адреса (maxLength:256)
        public OrgNaisParts parts { get; set; }                                 // Детальна адреса
    }

    public class OrgNaisParts                                                   // Детальна адреса
    {
        public string atu { get; set; }                                         // Адміністративна територіальна одиниця (maxLength:32)
        public string street { get; set; }                                      // Вулиця (maxLength:256)
        public string house_type { get; set; }                                  // Тип будівлі ('буд.', 'інше') (maxLength:64)
        public string house { get; set; }                                       // Номер будинку, якщо тип - 'буд.' (maxLength:64)
        public string building_type { get; set; }                               // Тип будівлі (maxLength:64)
        public string building { get; set; }                                    // Номер будівлі (maxLength:64)
        public string num_type { get; set; }                                    // Тип приміщення (maxLength:64)
        public string num { get; set; }                                         // Номер приміщення (maxLength:32)
    }

    public class OrgBranches                                                    // Array[Branch]. Перелік відокремлених підрозділів юридичної особи
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        public int? role { get; set; }                                          // Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? type { get; set; }                                          // Тип відокремленого підрозділу
        public string type_text { get; set; }                                   // Текстове відображення типу відокремленого підрозділу (maxLength:512)
        public OrgNaisAddress address { get; set; }                             // Адреса
    }

    public class OrgAuthorisedCapital                                           // Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
    {
        public string value { get; set; }                                       // Сумма (maxLength:128)
        public DateTime? date { get; set; }                                     // Дата формування
    }

    public class OrgActivityKinds                                               // Перелік видів економічної діяльності
    {
        public string code { get; set; }                                        // Код згідно КВЕД (maxLength:64)
        public string name { get; set; }                                        // Найменування виду діяльності (maxLength:256)
        public bool? is_primary { get; set; }                                   // Вказує, чи є вид діяльності основним (згідно даних органів статистики про основний вид економічної діяльності юридичної особи)
    }

    public class OrgNaisHeads                                                   // Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:256)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        public OrgNaisAddress address { get; set; }                             // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа) (maxLength:128)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:128)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        public DateTime? appointment_date { get; set; }                         // Дата призначення
        public string restriction { get; set; }                                 // Обмеження (maxLength:512)
    }

    public class OrgNaisRegistration                                            // inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
    {
        public DateTime? date { get; set; }                                     // Дата державної реєстрації
        public string record_number { get; set; }                               // Номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу (maxLength:32)
        public DateTime? record_date { get; set; }                              // Дата запису в Єдиному державному реєстрі
        public bool? is_separation { get; set; }                                // Державна реєстрація юридичної особи шляхом виділу
        public bool? is_division { get; set; }                                  // Державна реєстрація юридичної особи шляхом поділу
        public bool? is_merge { get; set; }                                     // Державна реєстрація юридичної особи шляхом злиття
        public bool? is_transformation { get; set; }                            // Державна реєстрація юридичної особи шляхом перетворення
    }

    public class OrgNaisBankruptcy                                              // inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію провадження у справі про банкрутство
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:128)
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство (maxLength:64)
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду (maxLength:256)
    }

    public class OrgNaisTermination                                             // inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:64)
        public string record_number { get; set; }                               // Номер запису про державну реєстрацію припинення юридичної особи (якщо в стані «припинено»); (maxLength:128)
        public string requirement_end_date { get; set; }                        // Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог (maxLength:128)
        public string cause { get; set; }                                       // Підстава для внесення запису про державну реєстрацію припинення юридичної особи (maxLength:512)
    }

    public class OrgNaisTerminationCancel                                       // inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
    {
        public DateTime? date { get; set; }                                     // Дата запису про відміну державної реєстрації припинення юридичної особи
        public string record_number { get; set; }                               // Номер запису про відміну державної реєстрації припинення юридичної особи (maxLength:64)
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство (maxLength:64)
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду (maxLength:256)
    }

    public class OrgNaisAssignees                                       // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        public OrgNaisAddress address { get; set; }                     // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа) (maxLength:256)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:256)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
    }

    public class OrgNaisRegistrations                                   // inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
    {
        public string name { get; set; }                                        // Назва органу (maxLength:512)
        public string code { get; set; }                                        // Ідентифікаційний код органу (maxLength:10)
        public string type { get; set; }                                        // Тип відомчого реєстру (maxLength:64)
        public string description { get; set; }                                 // Назва відомчого реєстру (maxLength:512)
        public DateTime? start_date { get; set; }                               // Дата взяття на облік
        public string start_num { get; set; }                                   // Номер взяття на облік (maxLength:64)
        public DateTime? end_date { get; set; }                                 // Дата зняття з обліку
        public string end_num { get; set; }                                     // Номер зняття з обліку (maxLength:64)
    }

    public class OrgNaisPrimaryActivityKind                             // inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
    {
        public string name { get; set; }                                        // Назва КВЕД (maxLength:128)
        public string code { get; set; }                                        // Код КВЕД (maxLength:64)
        public string reg_number { get; set; }                                  // Дані про реєстраційний номер платника єдиного внеску (maxLength:128)
        [JsonProperty("class")]
        public string classProp { get; set; }                                   // Дані про клас ризику (maxLength:32)
    }

    public class OrgNaisContacts                                        // Контактні дані
    {
        public string email { get; set; }                                       // Електронна адреса (maxLength:128)
        public string[] tel { get; set; }                                       // Перелік контактних телефонів (maxLength:128)
        public string fax { get; set; }                                         // Номер факсимільного апарату (maxLength:128)
        public string web_page { get; set; }                                    // Інтернет сайт (maxLength:128)
    }


}
