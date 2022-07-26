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

        Метод:
            4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
            [POST] /api/1.0/organizations/getadvancedorganization

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getadvancedorganization' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":"21560045"}'


        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetadvancedorganizationResponse.json

         */

        public static GetAdvancedOrganizationResponseModel GetAdvancedOrganization(string code, ref string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

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

                request.AddParameter("needUpdate", "true");                 // true - прямий запит в Nais / false - історичні дані з сервісу

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
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetAdvancedOrganizationRequestBodyModel                        // 
    {/// <summary>
     /// Код ЄДРПОУ / ІПН (maxLength:10)
     /// </summary>
        public string Code { get; set; }                                        // 
    }
    /// <summary>
    /// Модель відповіді GetAdvancedOrganization
    /// </summary>
    public class GetAdvancedOrganizationResponseModel                           // 
    {/// <summary>
     /// Данні з ЄДРФЮО
     /// </summary>
        public OrganizationaisElasticModel Data { get; set; }                   // 
        /// <summary>
        /// Експрес оцінка ризиків
        /// </summary>
        public OrganizationGageModel ExpressScore { get; set; }                 // 
        /// <summary>
        /// Судові рішення
        /// </summary>
        public CourtDecisionAnaliticViewModel CourtAnalytic { get; set; }       // 
        /// <summary>
        /// Дата реєстрації свідоцтва платника ПДВ
        /// </summary>
        public DateTime? DateRegInn { get; set; }                               //  
        /// <summary>
        /// Код ПДВ (maxLength:10)
        /// </summary>
        public string Inn { get; set; }                                         // 
        /// <summary>
        /// Дата анулювання свідоцтва платника ПДВ 
        /// </summary>
        public DateTime? DateCanceledInn { get; set; }                          // 
        /// <summary>
        /// Код за КОАТУУ (maxLength:10)
        /// </summary>
        public string Koatuu { get; set; }                                      // 
        /// <summary>
        /// Якщо компанія ФІЛІЯ відомості про мотеринську компанію
        /// </summary>
        public OrganizationaisElasticModel BranchMaster { get; set; }           // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class OrganizationaisElasticModel
    {/// <summary>
     /// Унікальний ідентифікатор суб’єкта
     /// </summary>
        public int id { get; set; }                                             // 
        /// <summary>
        /// Таблица ###. Стан суб’єкта
        /// </summary>
        public int? state { get; set; }                                         // 
        /// <summary>
        /// Текстове відображення стану суб’єкта (maxLength:64)
        /// </summary>
        public string state_text { get; set; }                                  // 
        /// <summary>
        /// ЄДРПОУ (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Назва суб’єкта
        /// </summary>
        public OrganizationaisNamesModel names { get; set; }                    // 
        /// <summary>
        /// Код організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        /// </summary>
        public string olf_code { get; set; }                                    // 
        /// <summary>
        /// Назва організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        /// </summary>
        public string olf_name { get; set; }                                    // 
        /// <summary>
        /// Назва установчого документа, якщо суб’єкт – юридична особа (maxLength:128)
        /// </summary>
        public string founding_document { get; set; }                           // 
        /// <summary>
        /// Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави  у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
        /// </summary>
        public OrganizationaisExecutivePower executive_power { get; set; }      //  
        /// <summary>
        /// Місцезнаходження реєстраційної справи (maxLength:256)
        /// </summary>
        public string object_name { get; set; }                                 // 
        /// <summary>
        /// Array[Founder]. Перелік засновників (учасників) юридичної особи,у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи,якщо засновник – юридична особа
        /// </summary>
        public OrganizationaisFounders[] founders { get; set; }                 //   
        /// <summary>
        /// Перелік кінцевих бенефіціарних власників(КБВ) юридичної особи
        /// </summary>
        public OrganizationaisFounders[] beneficiaries { get; set; }            // 
        /// <summary>
        /// Array[Branch]. Перелік відокремлених підрозділів юридичної особи
        /// </summary>
        public OrganizationaisBranches[] branches { get; set; }                 // 
        /// <summary>
        /// Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
        /// </summary>
        public OrganizationaisAuthorisedCapital authorised_capital { get; set; }//  
        /// <summary>
        /// Відомості про органи управління юридичної особи (maxLength:256)
        /// </summary>
        public string management { get; set; }                                  // 
        /// <summary>
        /// Найменування розпорядчого акта, якщо суб’єкт – юридична особа (maxLength:256)
        /// </summary>
        public string managing_paper { get; set; }                              // 
        /// <summary>
        /// Дані про наявність відмітки про те, що юридична особа створюється та діє на підставі модельного статуту
        /// </summary>
        public bool? is_modal_statute { get; set; }                             //  
        /// <summary>
        /// Перелік видів економічної діяльності
        /// </summary>
        public OrganizationaisActivityKinds[] activity_kinds { get; set; }      // 
        /// <summary>
        /// Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб,які обираються (призначаються) до органу управління юридичної особи,уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
        /// </summary>
        public OrganizationaisHeads[] heads { get; set; }                       //     
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }                     // 
        /// <summary>
        /// inline_model_2. Дата державної реєстрації,дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        /// </summary>
        public OrganizationaisRegistration registration { get; set; }           //  
        /// <summary>
        /// inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
        /// </summary>
        public OrganizationaisBankruptcy bankruptcy { get; set; }               // 
        /// <summary>
        /// inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи,підстава для його внесення
        /// </summary>
        public OrganizationaisTermination termination { get; set; }             //  
        /// <summary>
        /// inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
        /// </summary>
        public OrganizationaisTerminationCancel termination_cancel { get; set; }//  
        /// <summary>
        /// RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        /// </summary>
        public OrganizationaisAssignees[] assignees { get; set; }               // 
        /// <summary>
        /// RelatedSubject. Дані про юридичних осіб-правонаступників:повне найменування та місцезнаходження юридичних осіб-правонаступників,їх ідентифікаційні коди 
        /// </summary>
        public OrganizationaisAssignees[] predecessors { get; set; }            //  
        /// <summary>
        /// inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
        /// </summary>
        public OrganizationaisRegistrations[] registrations { get; set; }       // 
        /// <summary>
        /// inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи,визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
        /// </summary>
        public OrganizationaisPrimaryActivityKind primary_activity_kind { get; set; }//   
        /// <summary>
        /// Термін, до якого юридична особа перебуває на обліку в органі Міндоходів за місцем попередньої реєстрації, у разі зміни місцезнаходження юридичної особи
        /// </summary>
        public string prev_registration_end_term { get; set; }                  // 
        /// <summary>
        /// Контактні дані
        /// </summary>
        public OrganizationaisContacts contacts { get; set; }                   // 
        /// <summary>
        /// Дата відкриття виконавчого провадження щодо юридичної особи (для незавершених виконавчих проваджень)
        /// </summary>
        public string[] open_enforcements { get; set; }                         // 
    }
    /// <summary>
    /// Назва суб’єкта
    /// </summary>
    public class OrganizationaisNamesModel                                      // 
    {/// <summary>
     /// Повна назва суб’єкта (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Вказує, чи треба додавати організаційно-правову форму до назви, якщо суб’єкт – юридична особа
        /// </summary>
        public bool? include_olf { get; set; }                                  // 
        /// <summary>
        /// Назва для відображення (з ОПФ чи без, в залежності від параметру include_olf),якщо суб’єкт – юридична особа (maxLength:512) 
        /// </summary>
        public string display { get; set; }                                     // 
        /// <summary>
        /// Коротка назва, якщо суб’єкт – юридична особа (maxLength:512)
        /// </summary>
        [JsonProperty("short")]
        public string shortName { get; set; }                                   // 
        /// <summary>
        /// Повна назва суб’єкта англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
        /// </summary>
        public string name_en { get; set; }                                     // 
        /// <summary>
        ///  Коротка назва англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
        /// </summary>
        public string short_en { get; set; }                                    //
    }
    /// <summary>
    /// Центральний чи місцевий орган виконавчої влади,до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
    /// </summary>
    public class OrganizationaisExecutivePower                                  //  
    {/// <summary>
     /// Назва (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
    }
    /// <summary>
    /// Відомості про кінцевих бенефіціарних власників, засновників (учасників) юридичної особи
    /// </summary>
    public class OrganizationaisFounders                                        // 
    {/// <summary>
     /// Повна назва суб’єкта (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }                     // 
        /// <summary>
        /// Прізвище (якщо суб’єкт - приватна особа); (maxLength:256)
        /// </summary>
        public string last_name { get; set; }                                   // 
        /// <summary>
        /// Ім’я та по-батькові (якщо суб’єкт – приватна особа) (maxLength:256)
        /// </summary>
        public string first_middle_name { get; set; }                           // 
        /// <summary>
        /// Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        /// </summary>
        public int? role { get; set; }                                          // 
        /// <summary>
        /// Текстове відображення ролі (maxLength:64)
        /// </summary>
        public string role_text { get; set; }                                   // 
        /// <summary>
        /// Ідентифікатор суб'єкта
        /// </summary>
        public int? id { get; set; }                                            // 
        /// <summary>
        /// Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        /// </summary>
        public string url { get; set; }                                         // 
        /// <summary>
        /// Розмір частки у статутному капіталі пов’язаного суб’єкта (лише для засновників) (maxLength:128)
        /// </summary>
        public string capital { get; set; }                                     // 
        /// <summary>
        /// Тип бенефіцарного володіння: «5» - Прямий вирішальний вплив; / «6» - Не прямий вирішальний вплив
        /// </summary>
        public string beneficiaries_type { get; set; }                          // 
        /// <summary>
        /// Відсоток частки статутного капіталу або відсоток права голосу
        /// </summary>
        public string interest { get; set; }                                    // 
        /// <summary>
        /// Причина відсутності КБВ (якщо у юридичної особи відсутні КБВ)
        /// </summary>
        public int? reason { get; set; }                                        // 
    }
    /// <summary>
    /// Адреса
    /// </summary>
    public class OrganizationaisAddress                                         // 
    {/// <summary>
     /// Поштовий індекс (maxLength:16)
     /// </summary>
        public string zip { get; set; }                                         // 
        /// <summary>
        /// Назва країни (maxLength:64)
        /// </summary>
        public string country { get; set; }                                     // 
        /// <summary>
        /// Адреса (maxLength:256)
        /// </summary>
        public string address { get; set; }                                     // 
        /// <summary>
        /// Детальна адреса
        /// </summary>
        public OrganizationaisParts parts { get; set; }                         // 
    }
    /// <summary>
    /// Детальна адреса
    /// </summary>
    public class OrganizationaisParts                                           // 
    {/// <summary>
     /// Адміністративна територіальна одиниця (maxLength:32)
     /// </summary>
        public string atu { get; set; }                                         // 
        /// <summary>
        /// Вулиця (maxLength:256)
        /// </summary>
        public string street { get; set; }                                      // 
        /// <summary>
        /// Тип будівлі ('буд.', 'інше') (maxLength:64)
        /// </summary>
        public string house_type { get; set; }                                  // 
        /// <summary>
        /// Номер будинку, якщо тип - 'буд.' (maxLength:64)
        /// </summary>
        public string house { get; set; }                                       // 
        /// <summary>
        /// Тип будівлі (maxLength:64)
        /// </summary>
        public string building_type { get; set; }                               // 
        /// <summary>
        /// Номер будівлі (maxLength:64)
        /// </summary>
        public string building { get; set; }                                    // 
        /// <summary>
        /// Тип приміщення (maxLength:64)
        /// </summary>
        public string num_type { get; set; }                                    // 
        /// <summary>
        /// Номер приміщення (maxLength:32)
        /// </summary>
        public string num { get; set; }                                         // 
    }
    /// <summary>
    /// Array[Branch]. Перелік відокремлених підрозділів юридичної особи
    /// </summary>
    public class OrganizationaisBranches                                        // 
    {/// <summary>
     /// Повна назва суб’єкта (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        /// </summary>
        public int? role { get; set; }                                          // 
        /// <summary>
        /// Текстове відображення ролі (maxLength:64)
        /// </summary>
        public string role_text { get; set; }                                   // 
        /// <summary>
        /// Тип відокремленого підрозділу
        /// </summary>
        public int? type { get; set; }                                          // 
        /// <summary>
        /// Текстове відображення типу відокремленого підрозділу (maxLength:512)
        /// </summary>
        public string type_text { get; set; }                                   // 
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }                     // 
    }
    /// <summary>
    /// Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
    /// </summary>
    public class OrganizationaisAuthorisedCapital                               // 
    {/// <summary>
     /// Сумма (maxLength:128)
     /// </summary>
        public string value { get; set; }                                       // 
        /// <summary>
        /// Дата формування
        /// </summary>
        public DateTime? date { get; set; }                                     // 
    }
    /// <summary>
    /// Перелік видів економічної діяльності
    /// </summary>
    public class OrganizationaisActivityKinds                                   // 
    {/// <summary>
     /// Код згідно КВЕД (maxLength:64)
     /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Найменування виду діяльності (maxLength:256)
        /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Вказує, чи є вид діяльності основним (згідно даних органів статистики про основний вид економічної діяльності юридичної особи)
        /// </summary>
        public bool? is_primary { get; set; }                                   //  
    }
    /// <summary>
    /// Array[Head]. Прізвище, ім’я, по батькові,дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами,або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності,у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
    /// </summary>
    public class OrganizationaisHeads                                           //    
    {/// <summary>
     /// Повна назва суб’єкта (maxLength:256)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }                     // 
        /// <summary>
        /// Прізвище (якщо суб’єкт - приватна особа) (maxLength:128)
        /// </summary>
        public string last_name { get; set; }                                   // 
        /// <summary>
        /// Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:128)
        /// </summary>
        public string first_middle_name { get; set; }                           // 
        /// <summary>
        /// Роль по відношенню до пов’язаного суб’єкта
        /// </summary>
        public int? role { get; set; }                                          // 
        /// <summary>
        /// Текстове відображення ролі (maxLength:64)
        /// </summary>
        public string role_text { get; set; }                                   // 
        /// <summary>
        /// Ідентифікатор суб'єкта
        /// </summary>
        public int? id { get; set; }                                            // 
        /// <summary>
        /// Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        /// </summary>
        public string url { get; set; }                                         // 
        /// <summary>
        /// Дата призначення
        /// </summary>
        public DateTime? appointment_date { get; set; }                         // 
        /// <summary>
        /// Обмеження (maxLength:512)
        /// </summary>
        public string restriction { get; set; }                                 // 
    }
    /// <summary>
    /// inline_model_2. Дата державної реєстрації,дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу 
    /// </summary>
    public class OrganizationaisRegistration                                    // 
    {/// <summary>
     /// Дата державної реєстрації
     /// </summary>
        public DateTime? date { get; set; }                                     // 
        /// <summary>
        /// Номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу (maxLength:32)
        /// </summary>
        public string record_number { get; set; }                               //  
        /// <summary>
        /// Дата запису в Єдиному державному реєстрі
        /// </summary>
        public DateTime? record_date { get; set; }                              // 
        /// <summary>
        /// Державна реєстрація юридичної особи шляхом виділу
        /// </summary>
        public bool? is_separation { get; set; }                                // 
        /// <summary>
        /// Державна реєстрація юридичної особи шляхом поділу
        /// </summary>
        public bool? is_division { get; set; }                                  // 
        /// <summary>
        /// Державна реєстрація юридичної особи шляхом злиття 
        /// </summary>
        public bool? is_merge { get; set; }                                     // 
        /// <summary>
        /// Державна реєстрація юридичної особи шляхом перетворення
        /// </summary>
        public bool? is_transformation { get; set; }                            // 
    }
    /// <summary>
    /// inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
    /// </summary>
    public class OrganizationaisBankruptcy                                      // 
    {/// <summary>
     /// Дата запису про державну реєстрацію провадження у справі про банкрутство
     /// </summary>
        public DateTime? date { get; set; }                                     // 
        /// <summary>
        /// Таблица ###. Стан суб’єкта
        /// </summary>
        public int? state { get; set; }                                         // 
        /// <summary>
        /// Текстове відображення стану суб’єкта (maxLength:128)
        /// </summary>
        public string state_text { get; set; }                                  // 
        /// <summary>
        /// Номер провадження про банкрутство (maxLength:64)
        /// </summary>
        public string doc_number { get; set; }                                  // 
        /// <summary>
        /// Дата провадження про банкрутство
        /// </summary>
        public DateTime? doc_date { get; set; }                                 // 
        /// <summary>
        /// Дата набуття чинності
        /// </summary>
        public DateTime? date_judge { get; set; }                               // 
        /// <summary>
        /// Найменування суду (maxLength:256)
        /// </summary>
        public string court_name { get; set; }                                  // 
    }
    /// <summary>
    /// inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
    /// </summary>
    public class OrganizationaisTermination                                     //  
    {/// <summary>
     /// Дата запису про державну реєстрацію припинення юридичної особи,або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)
     /// </summary>
        public DateTime? date { get; set; }                                     // 
        /// <summary>
        /// Таблица ###. Стан суб’єкта
        /// </summary>
        public int? state { get; set; }                                         // 
        /// <summary>
        /// Текстове відображення стану суб’єкта (maxLength:64)
        /// </summary>
        public string state_text { get; set; }                                  // 
        /// <summary>
        /// Номер запису про державну реєстрацію припинення юридичної особи (якщо в стані «припинено»); (maxLength:128)
        /// </summary>
        public string record_number { get; set; }                               // 
        /// <summary>
        /// Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом,що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог (maxLength:128)
        /// </summary>
        public string requirement_end_date { get; set; }                        //   
        /// <summary>
        /// Підстава для внесення запису про державну реєстрацію припинення юридичної особи (maxLength:512)
        /// </summary>
        public string cause { get; set; }                                       //  
    }
    /// <summary>
    /// inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
    /// </summary>
    public class OrganizationaisTerminationCancel                               //  
    {/// <summary>
     /// Дата запису про відміну державної реєстрації припинення юридичної особи
     /// </summary>
        public DateTime? date { get; set; }                                     // 
        /// <summary>
        ///Номер запису про відміну державної реєстрації припинення юридичної особи (maxLength:64) 
        /// </summary>
        public string record_number { get; set; }                               // 
        /// <summary>
        /// Номер провадження про банкрутство (maxLength:64)
        /// </summary>
        public string doc_number { get; set; }                                  // 
        /// <summary>
        /// Дата провадження про банкрутство
        /// </summary>
        public DateTime? doc_date { get; set; }                                 // 
        /// <summary>
        /// Дата набуття чинності
        /// </summary>
        public DateTime? date_judge { get; set; }                               // 
        /// <summary>
        /// Найменування суду (maxLength:256)
        /// </summary>
        public string court_name { get; set; }                                  // 
    }
    /// <summary>
    /// RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
    /// </summary>
    public class OrganizationaisAssignees                                       //  
    {/// <summary>
     /// Повна назва суб’єкта (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }                     // 
        /// <summary>
        /// Прізвище (якщо суб’єкт - приватна особа) (maxLength:256)
        /// </summary>
        public string last_name { get; set; }                                   // 
        /// <summary>
        /// Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:256)
        /// </summary>
        public string first_middle_name { get; set; }                           // 
        /// <summary>
        /// Роль по відношенню до пов’язаного суб’єкта
        /// </summary>
        public int? role { get; set; }                                          // 
        /// <summary>
        /// Текстове відображення ролі (maxLength:64)
        /// </summary>
        public string role_text { get; set; }                                   // 
        /// <summary>
        /// Ідентифікатор суб'єкта
        /// </summary>
        public int? id { get; set; }                                            // 
        /// <summary>
        /// Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        /// </summary>
        public string url { get; set; }                                         // 
    }
    /// <summary>
    /// inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
    /// </summary>
    public class OrganizationaisRegistrations                                   //  
    {/// <summary>
     /// Назва органу (maxLength:512)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Ідентифікаційний код органу (maxLength:10)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Тип відомчого реєстру (maxLength:64)
        /// </summary>
        public string type { get; set; }                                        // 
        /// <summary>
        /// Назва відомчого реєстру (maxLength:512)
        /// </summary>
        public string description { get; set; }                                 // 
        /// <summary>
        /// Дата взяття на облік
        /// </summary>
        public DateTime? start_date { get; set; }                               // 
        /// <summary>
        /// Номер взяття на облік (maxLength:64)
        /// </summary>
        public string start_num { get; set; }                                   // 
        /// <summary>
        /// Дата зняття з обліку
        /// </summary>
        public DateTime? end_date { get; set; }                                 // 
        /// <summary>
        /// Номер зняття з обліку (maxLength:64)
        /// </summary>
        public string end_num { get; set; }                                     // 
    }
    /// <summary>
    /// inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
    /// </summary>
    public class OrganizationaisPrimaryActivityKind                             //  
    {/// <summary>
     /// Назва КВЕД (maxLength:128)
     /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Код КВЕД (maxLength:64)
        /// </summary>
        public string code { get; set; }                                        // 
        /// <summary>
        /// Дані про реєстраційний номер платника єдиного внеску (maxLength:128)
        /// </summary>
        public string reg_number { get; set; }                                  // 
        /// <summary>
        /// Дані про клас ризику (maxLength:32)
        /// </summary>
        [JsonProperty("class")]
        public string classProp { get; set; }                                   // 
    }
    /// <summary>
    /// Контактні дані
    /// </summary>
    public class OrganizationaisContacts                                        // 
    {/// <summary>
     /// Електронна адреса (maxLength:128)
     /// </summary>
        public string email { get; set; }                                       // 
        /// <summary>
        /// Перелік контактних телефонів (maxLength:128)
        /// </summary>
        public string[] tel { get; set; }                                       // 
        /// <summary>
        /// Номер факсимільного апарату (maxLength:128)
        /// </summary>
        public string fax { get; set; }                                         // 
        /// <summary>
        /// Інтернет сайт (maxLength:128)
        /// </summary>
        public string web_page { get; set; }                                    // 
    }

    /// <summary>
    /// Оцінка ризиків
    /// </summary>
    public class OrganizationGageModel                                          // 
    {/// <summary>
     /// В процесі ліквідації (maxLength:32)
     /// </summary>
        public string liquidation { get; set; }                                 // 
        /// <summary>
        /// Відомості про банкрутство (maxLength:32)
        /// </summary>
        public string bankruptcy { get; set; }                                  // 
        /// <summary>
        /// Зв’язки з компаніями банкрутами (maxLength:64)
        /// </summary>
        public string badRelations { get; set; }                                // 
        /// <summary>
        /// Афілійовані зв’язки (maxLength:64)
        /// </summary>
        public string sffiliatedMore { get; set; }                              // 
        /// <summary>
        /// Санкційні списки (maxLength:32)
        /// </summary>
        public string sanctions { get; set; }                                   // 
        /// <summary>
        /// Виконавчі впровадження (maxLength:64)
        /// </summary>
        public string introduction { get; set; }                                // 
        /// <summary>
        /// Кримінальні справи (maxLength:64)
        /// </summary>
        public string criminal { get; set; }                                    // 
        /// <summary>
        /// Включено в план-графік перевірок (maxLength:32)
        /// </summary>
        public string audits { get; set; }                                      // 
        /// <summary>
        /// Анульована реєстрація платника ПДВ (maxLength:32)
        /// </summary>
        public string canceledVat { get; set; }                                 // 
        /// <summary>
        /// Податковий борг (maxLength:64)
        /// </summary>
        public string taxDebt { get; set; }                                     // 
        /// <summary>
        /// Заборгованість по ЗП (maxLength:64)
        /// </summary>
        public string wageArrears { get; set; }                                 // 
        /// <summary>
        /// Зміна основного КВЕД	(maxLength:32)
        /// </summary>
        public string kved { get; set; }                                        // 
        /// <summary>
        /// Новий директор (maxLength:32)
        /// </summary>
        public string newDirector { get; set; }                                 // 
        /// <summary>
        /// Адреса масової реєстрації (maxLength:64)
        /// </summary>
        public string massRegistration { get; set; }                            // 
        /// <summary>
        /// Нова компанія (maxLength:32)
        /// </summary>
        public string youngCompany { get; set; }                                // 
        /// <summary>
        /// Зареєстровано на окупованій території (maxLength:64)
        /// </summary>
        public string atoRegistered { get; set; }                               // 
        /// <summary>
        /// Стаття 205 ККУ. Фіктивне підприємництво (maxLength:64)
        /// </summary>
        public string fictitiousBusiness { get; set; }                          // 
        /// <summary>
        /// Керівник цієї компанії є керівником в інших компаніях (maxLength:64)
        /// </summary>
        public string headOtherCompanies { get; set; }                          // 
        /// <summary>
        /// Не діюча ліцензія (maxLength:32)
        /// </summary>
        public string notLicense { get; set; }                                  // 
        /// <summary>
        /// Статус платника ПДВ не перевищує 3 місяців (maxLength:32)
        /// </summary>
        public string vatLessThree { get; set; }                                // 
        /// <summary>
        /// Зв’язки з компаніями під санкціями (maxLength:64)
        /// </summary>
        public string sanctionsRelationships { get; set; }                      // 
        /// <summary>
        /// Зв’язки з компаніями з окупованих територій (maxLength:64)
        /// </summary>
        public string territoriesRelationships { get; set; }                    // 
        /// <summary>
        /// Зв’язки з компаніями, які мають кримінальні справи (maxLength:64)
        /// </summary>
        public string criminalRelationships { get; set; }                       // 
        /// <summary>
        /// Системна інформація Vkursi (maxLength:64)
        /// </summary>
        public string update_date { get; set; }                                 // 
        /// <summary>
        /// Системна інформація Vkursi (maxLength:64)
        /// </summary>
        public string relation_date { get; set; }                               // 
    }

    /// <summary>
    /// Судові рішення
    /// </summary>
    public class CourtDecisionAnaliticViewModel                                 // 
    {/// <summary>
     /// Судові рішення
     /// </summary>
        public CourtDecisionAggregationModel Aggregations { get; set; }         // 
        /// <summary>
        /// ЄДРПОУ (maxLength:10)
        /// </summary>
        public string Edrpo { get; set; }                                       // 
        /// <summary>
        /// Системна інформація Vkursi. Рівень доступу
        /// </summary>
        public OpenCardAccessState OpenCardAccessState { get; set; }            // 
    }/// <summary>
     /// Системна інформація Vkursi
     /// </summary>
    public enum OpenCardAccessState                                             // 
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
        public string Name { get; set; }                                        // Назва форми судочинства (maxLength:64)
        public long? Count { get; set; }                                        // Кількість документів
    }

}
