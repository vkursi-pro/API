using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vkursi_api_example.organizations
{
    public class GetAdvancedOrganizationClass
    {
        /*

        4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
        [POST] /api/1.0/organizations/getadvancedorganization

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganization' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"21560045"}'

         */

        public static GetAdvancedOrganizationResponseModel GetAdvancedOrganization(string code, ref string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetAdvancedOrganizationRequestBodyModel GAORequestBody = new GetAdvancedOrganizationRequestBodyModel
                {
                    Code = code                                             // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GAORequestBody);  // Example body: {"Code":"21560045"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganization");
                RestRequest request = new RestRequest(Method.POST);

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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetAdvancedOrganizationResponseModel GAOResponse = new GetAdvancedOrganizationResponseModel();

            GAOResponse = JsonConvert.DeserializeObject<GetAdvancedOrganizationResponseModel>(responseString);

            return GAOResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"21560045\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getadvancedorganization", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":\"21560045\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganization")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class GetAdvancedOrganizationRequestBodyModel                        // Модель Body запиту
    {
        public string Code { get; set; }                                        // Код ЄДРПОУ / ІПН
    }

    public class GetAdvancedOrganizationResponseModel                           // Модель відповіді GetAdvancedOrganization
    {
        public OrganizationaisElasticModel Data { get; set; }                   // Данні з ЄДРФЮО
        public OrganizationGageModel ExpressScore { get; set; }                 // Експрес оцінка ризиків
        public CourtDecisionAnaliticViewModel CourtAnalytic { get; set; }       // Судові рішення
        public DateTime? DateRegInn { get; set; }                               // Дата реєстрації свідоцтва платника ПДВ 
        public string Inn { get; set; }                                         // Код ПДВ
        public DateTime? DateCanceledInn { get; set; }                          // Дата анулювання свідоцтва платника ПДВ 
        public string Koatuu { get; set; }                                      // Код за КОАТУУ
    }

    public class OrganizationaisElasticModel
    {
        public int id { get; set; }                                             // Унікальний ідентифікатор суб’єкта
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта
        public string code { get; set; }                                        // ЄДРПОУ
        public OrganizationaisNamesModel names { get; set; }                    // Назва суб’єкта
        public string olf_code { get; set; }                                    // Код організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа
        public string olf_name { get; set; }                                    // Назва організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа
        public string founding_document { get; set; }                           // Назва установчого документа, якщо суб’єкт – юридична особа
        public OrganizationaisExecutivePower executive_power { get; set; }      // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
        public string object_name { get; set; }                                 // Місцезнаходження реєстраційної справи
        public OrganizationaisFounders[] founders { get; set; }                 // Array[Founder]. Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
        public OrganizationaisBranches[] branches { get; set; }                 // Array[Branch]. Перелік відокремлених підрозділів юридичної особи
        public OrganizationaisAuthorisedCapital authorised_capital { get; set; }// Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
        public string management { get; set; }                                  // Відомості про органи управління юридичної особи
        public string managing_paper { get; set; }                              // Найменування розпорядчого акта, якщо суб’єкт – юридична особа
        public bool? is_modal_statute { get; set; }                             // Дані про наявність відмітки про те, що юридична особа створюється та діє на підставі модельного статуту
        public OrganizationaisActivityKinds[] activity_kinds { get; set; }      // Перелік видів економічної діяльності
        public OrganizationaisHeads[] heads { get; set; }                       // Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
        public OrganizationaisAddress address { get; set; }                     // Адреса
        public OrganizationaisRegistration registration { get; set; }           // inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        public OrganizationaisBankruptcy bankruptcy { get; set; }               // inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
        public OrganizationaisTermination termination { get; set; }             // inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
        public OrganizationaisTerminationCancel termination_cancel { get; set; }// inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
        public OrganizationaisAssignees[] assignees { get; set; }               // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrganizationaisAssignees[] predecessors { get; set; }            // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrganizationaisRegistrations[] registrations { get; set; }       // inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
        public OrganizationaisPrimaryActivityKind primary_activity_kind { get; set; }// inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
        public string prev_registration_end_term { get; set; }                  // Термін, до якого юридична особа перебуває на обліку в органі Міндоходів за місцем попередньої реєстрації, у разі зміни місцезнаходження юридичної особи
        public OrganizationaisContacts contacts { get; set; }                   // Контактні дані
        public string[] open_enforcements { get; set; }                         // Дата відкриття виконавчого провадження щодо юридичної особи (для незавершених виконавчих проваджень)
    }

    public class OrganizationaisNamesModel                                      // Назва суб’єкта
    {
        public string name { get; set; }                                        // Повна назва суб’єкта
        public bool? include_olf { get; set; }                                  // Вказує, чи треба додавати організаційно-правову форму до назви, якщо суб’єкт – юридична особа
        public string display { get; set; }                                     // Назва для відображення (з ОПФ чи без, в залежності від параметру include_olf), якщо суб’єкт – юридична особа
        [JsonProperty("short")]
        public string shortName { get; set; }                                   // Коротка назва, якщо суб’єкт – юридична особа
        public string name_en { get; set; }                                     // Повна назва суб’єкта англійською мовою, якщо суб’єкт – юридична особа
        public string short_en { get; set; }                                    // Коротка назва англійською мовою, якщо суб’єкт – юридична особа
    }

    public class OrganizationaisExecutivePower                                  // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
    {
        public string name { get; set; }                                        // Назва
        public string code { get; set; }                                        // ЄДРПОУ
    }

    public class OrganizationaisFounders                                        // Array[Founder]. Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
    {
        public string name { get; set; }                                        // Повна назва суб’єкта
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт – юридична особа
        public OrganizationaisAddress address { get; set; }                     // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа);
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт – приватна особа)
        public int? role { get; set; }                                          // Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт
        public string capital { get; set; }                                     // Розмір частки у статутному капіталі пов’язаного суб’єкта (лише для засновників)
    }

    public class OrganizationaisAddress                                         // Адреса
    {
        public string zip { get; set; }                                         // Поштовий індекс
        public string country { get; set; }                                     // Назва країни
        public string address { get; set; }                                     // Адреса
        public OrganizationaisParts parts { get; set; }                         // Детальна адреса
    }

    public class OrganizationaisParts                                           // Детальна адреса
    {
        public string atu { get; set; }                                         // Адміністративна територіальна одиниця
        public string street { get; set; }                                      // Вулиця
        public string house_type { get; set; }                                  // Тип будівлі ('буд.', 'інше')
        public string house { get; set; }                                       // Номер будинку, якщо тип - 'буд.'
        public string building_type { get; set; }                               // Тип будівлі
        public string building { get; set; }                                    // Номер будівлі
        public string num_type { get; set; }                                    // Тип приміщення
        public string num { get; set; }                                         // Номер приміщення
    }

    public class OrganizationaisBranches                                        // Array[Branch]. Перелік відокремлених підрозділів юридичної особи
    {
        public string name { get; set; }                                        // Повна назва суб’єкта
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа
        public int? role { get; set; }                                          // Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі
        public int? type { get; set; }                                          // Тип відокремленого підрозділу
        public string type_text { get; set; }                                   // Текстове відображення типу відокремленого підрозділу
        public OrganizationaisAddress address { get; set; }                     // Адреса
    }

    public class OrganizationaisAuthorisedCapital                               // Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
    {
        public string value { get; set; }                                       // Сумма
        public DateTime? date { get; set; }                                     // Дата формування
    }

    public class OrganizationaisActivityKinds                                   // Перелік видів економічної діяльності
    {
        public string code { get; set; }                                        // Код згідно КВЕД
        public string name { get; set; }                                        // Найменування виду діяльності
        public bool? is_primary { get; set; }                                   // Вказує, чи є вид діяльності основним (згідно даних органів статистики про основний вид економічної діяльності юридичної особи)
    }

    public class OrganizationaisHeads                                           // Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
    {
        public string name { get; set; }                                        // Повна назва суб’єкта
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа
        public OrganizationaisAddress address { get; set; }                     // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт
        public DateTime? appointment_date { get; set; }                         // Дата призначення
        public string restriction { get; set; }                                 // Обмеження
    }

    public class OrganizationaisRegistration                                    // inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
    {
        public DateTime? date { get; set; }                                     // Дата державної реєстрації
        public string record_number { get; set; }                               // Номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        public DateTime? record_date { get; set; }                              // Дата запису в Єдиному державному реєстрі
        public bool? is_separation { get; set; }                                // Державна реєстрація юридичної особи шляхом виділу
        public bool? is_division { get; set; }                                  // Державна реєстрація юридичної особи шляхом поділу
        public bool? is_merge { get; set; }                                     // Державна реєстрація юридичної особи шляхом злиття
        public bool? is_transformation { get; set; }                            // Державна реєстрація юридичної особи шляхом перетворення
    }

    public class OrganizationaisBankruptcy                                      // inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію провадження у справі про банкрутство
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду
    }

    public class OrganizationaisTermination                                     // inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта
        public string record_number { get; set; }                               // Номер запису про державну реєстрацію припинення юридичної особи (якщо в стані «припинено»);
        public string requirement_end_date { get; set; }                        // Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог
        public string cause { get; set; }                                       // Підстава для внесення запису про державну реєстрацію припинення юридичної особи
    }

    public class OrganizationaisTerminationCancel                               // inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
    {
        public DateTime? date { get; set; }                                     // Дата запису про відміну державної реєстрації припинення юридичної особи
        public string record_number { get; set; }                               // Номер запису про відміну державної реєстрації припинення юридичної особи
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду
    }

    public class OrganizationaisAssignees                                       // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
    {
        public string name { get; set; }                                        // Повна назва суб’єкта
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт – юридична особа
        public OrganizationaisAddress address { get; set; }                     // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт
    }

    public class OrganizationaisRegistrations                                   // inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
    {
        public string name { get; set; }                                        // Назва органу
        public string code { get; set; }                                        // Ідентифікаційний код органу
        public string type { get; set; }                                        // Тип відомчого реєстру
        public string description { get; set; }                                 // Назва відомчого реєстру
        public DateTime? start_date { get; set; }                               // Дата взяття на облік
        public string start_num { get; set; }                                   // Номер взяття на облік
        public DateTime? end_date { get; set; }                                 // Дата зняття з обліку
        public string end_num { get; set; }                                     // Номер зняття з обліку
    }

    public class OrganizationaisPrimaryActivityKind                             // inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
    {
        public string name { get; set; }                                        // Назва КВЕД
        public string code { get; set; }                                        // Код КВЕД
        public string reg_number { get; set; }                                  // Дані про реєстраційний номер платника єдиного внеску
        [JsonProperty("class")]
        public string classProp { get; set; }                                   // Дані про клас ризику
    }

    public class OrganizationaisContacts                                        // Контактні дані
    {
        public string email { get; set; }                                       // Електронна адреса
        public string[] tel { get; set; }                                       // Перелік контактних телефонів
        public string fax { get; set; }                                         // Номер факсимільного апарату
        public string web_page { get; set; }                                    // Інтернет сайт
    }


    public class OrganizationGageModel                                          // Оцінка ризиків
    {
        public string liquidation { get; set; }                                 // В процесі ліквідації
        public string bankruptcy { get; set; }                                  // Відомості про банкрутство
        public string badRelations { get; set; }                                // Зв’язки з компаніями банкрутами
        public string sffiliatedMore { get; set; }                              // Афілійовані зв’язки
        public string sanctions { get; set; }                                   // Санкційні списки
        public string introduction { get; set; }                                // Виконавчі впровадження
        public string criminal { get; set; }                                    // Кримінальні справи
        public string audits { get; set; }                                      // Включено в план-графік перевірок
        public string canceledVat { get; set; }                                 // Анульована реєстрація платника ПДВ
        public string taxDebt { get; set; }                                     // Податковий борг
        public string wageArrears { get; set; }                                 // Заборгованість по ЗП
        public string kved { get; set; }                                        // Зміна основного КВЕД	
        public string newDirector { get; set; }                                 // Новий директор
        public string massRegistration { get; set; }                            // Адреса масової реєстрації
        public string youngCompany { get; set; }                                // Нова компанія
        public string atoRegistered { get; set; }                               // Зареєстровано на окупованій території
        public string fictitiousBusiness { get; set; }                          // Стаття 205 ККУ. Фіктивне підприємництво
        public string headOtherCompanies { get; set; }                          // Керівник цієї компанії є керівником в інших компаніях
        public string notLicense { get; set; }                                  // Не діюча ліцензія
        public string vatLessThree { get; set; }                                // Статус платника ПДВ не перевищує 3 місяців 
        public string sanctionsRelationships { get; set; }                      // Зв’язки з компаніями під санкціями
        public string territoriesRelationships { get; set; }                    // Зв’язки з компаніями з окупованих територій 
        public string criminalRelationships { get; set; }                       // Зв’язки з компаніями, які мають кримінальні справи
        public string update_date { get; set; }                                 // Системна інформація Vkursi
        public string relation_date { get; set; }                               // Системна інформація Vkursi
    }


    public class CourtDecisionAnaliticViewModel                                 // Судові рішення
    {
        public CourtDecisionAggregationModel Aggregations { get; set; }         // Судові рішення
        public string Edrpo { get; set; }                                       // ЄДРПОУ
        public OpenCardAccessState OpenCardAccessState { get; set; }            // Рівень доступу
    }
    public enum OpenCardAccessState                                             // Системна інформація Vkursi
    {
        [Display(Name = "Бесплатный доступ")]
        IsFree = 0,

        [Display(Name = "Доступ к своей компании")]
        IsMyCompany = 1,

        [Display(Name = "Доступ ограничен тарифом S")]
        IsCardS = 2,

        [Display(Name = "Доступ ограничен тарифом xl")]
        IsCardXl = 3,

        [Display(Name = "Доступ ограничен тарифом xxl")]
        IsCardXxl = 4
    }

    public class CourtDecisionAggregationModel
    {
        public long? TotalCount { get; set; }                                   // Всього документів
        public long? PlaintiffCount { get; set; }                               // Позивач
        public long? DefendantCount { get; set; }                               // Відповідач
        public long? OtherSideCount { get; set; }                               // Інша сторона
        public long? LoserCount { get; set; }                                   // Програно
        public long? WinCount { get; set; }                                     // Виграно
        public long? IntendedCount { get; set; }                                // Призначено до розгляду
        public long? CaseCount { get; set; }                                    // Кількість справ
        public long? InProcess { get; set; }                                    // В просесі
        public IEnumerable<JusticeKinds> ListJusticeKindses { get; set; }       // Справи за форма судочинства
    }

    public class JusticeKinds                                                   // Справи за форма судочинства
    {
        public long Id { get; set; }                                            // Системна інформація Vkursi
        public string Name { get; set; }                                        // Назва форми судочинства
        public long? Count { get; set; }                                        // Кількість документів
    }

}
