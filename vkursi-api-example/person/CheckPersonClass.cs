using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;
using vkursi_api_example.organizations;

namespace vkursi_api_example.person
{
    public class CheckPersonClass
    {
        /*
            29. Отримання інформації по фізичній особі
            [POST] /api/1.0/person/checkperson

            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/person/checkperson' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Id":null,"FullName":"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ","FirstName":null,"SecondName":null,"LastName":null,"Ipn":"2301715013","Doc":null,"Birthday":null,"RuName":null}'

        */

        public static CheckPersonResponseModel CheckPerson(string token, CheckPersonRequestBodyModel CheckPersonRequestBodyRow)
        {
            if (string.IsNullOrEmpty(token))
            { 
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            System.Console.WriteLine(DateTime.Now);

            while (string.IsNullOrEmpty(responseString))
            {

                //CheckPersonRequestBodyModel CheckPersonRequestBodyRow = new CheckPersonRequestBodyModel
                //{
                //    FullName = personName,              // ПІБ (ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ)
                //    Ipn = ipn                           // ІПН (2301715013)
                //};


                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/checkperson");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(CheckPersonRequestBodyRow);

                // Example Body: {"Id":null,"FullName":"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ","FirstName":null,"SecondName":null,"LastName":null,"Ipn":"2301715013","Doc":null,"Birthday":null,"RuName":null}

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                System.Console.WriteLine(DateTime.Now);

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

            CheckPersonResponseModel CPResponseRow = new CheckPersonResponseModel();

            CPResponseRow = JsonConvert.DeserializeObject<CheckPersonResponseModel>(responseString);
            
            return CPResponseRow;

        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Id\":null,\"FullName\":\"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ\",\"FirstName\":null,\"SecondName\":null,\"LastName\":null,\"Ipn\":\"2301715013\",\"Doc\":null,\"Birthday\":null,\"RuName\":null}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/person/checkperson", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Id\":null,\"FullName\":\"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ\",\"FirstName\":null,\"SecondName\":null,\"LastName\":null,\"Ipn\":\"2301715013\",\"Doc\":null,\"Birthday\":null,\"RuName\":null}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/person/checkperson")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    /// <summary>
    /// Модель запиту (обмеження 1)
    /// </summary>
    public class CheckPersonRequestBodyModel                                            // 
    {
        /// <summary>
        /// ПІБ
        /// </summary>
        /// <example>ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ</example>
        public string FullName { get; set; }                                            // 
        /// <summary>
        /// Призвище (необовязкове якщо вказаний FullName)
        /// </summary>
        public string FirstName { get; set; }                                           // 
        /// <summary>
        /// Ім'я (необовязкове якщо вказаний FullName)
        /// </summary>
        public string SecondName { get; set; }                                          // 
        /// <summary>
        /// По батькові (необовязкове)
        /// </summary>
        public string LastName { get; set; }                                            // 
        /// <summary>
        /// ІНП
        /// </summary>
        public string Ipn { get; set; }                                                 // 
        /// <summary>
        /// Серія номер пасторта (XX123456)
        /// </summary>
        public string Doc { get; set; }                                                 // 
        /// <summary>
        /// Серія номер пасторта (XX123456)
        /// </summary>
        public DateTime? Birthday { get; set; }                                         // 
        /// <summary>
        /// ПІБ на російській мові
        /// </summary>
        public string RuName { get; set; }                                              // 
    }

    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class CheckPersonResponseModel                                               // 
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSucces { get; set; }                                              // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// ІНП
        /// </summary>
        public string Ipn { get; set; }                                                 // 
        /// <summary>
        /// Серія номер пасторта (XX123456)
        /// </summary>
        public string Passport { get; set; }                                            // 
        /// <summary>
        /// ПІБ на російській мові
        /// </summary>
        public string NameRu { get; set; }                                              // 
        /// <summary>
        /// Дата народження
        /// </summary>
        public DateTime? DateBirth { get; set; }                                        // 
        /// <summary>
        /// Відомості про наявність боргу
        /// </summary>
        public PersonsInBorgModel Borg { get; set; }                                    // 
        /// <summary>
        /// Обтяження (ДРОРМ)
        /// </summary>
        public List<MovableViewModel> Movables { get; set; }                            // 
        /// <summary>
        /// Перелік знайдених ФОП
        /// </summary>
        public List<PersonsInFopsModel> PersonsInFopsModelList { get; set; }            // 
        /// <summary>
        /// Відомосто про перевірки контролюючих органів 
        /// </summary>
        public List<AuditsResult> Audits { get; set; }                                  // 
        /// <summary>
        /// Виконавчі провадження
        /// </summary>
        public PersonEnforcementsModel Enforcements { get; set; }                       // 
        /// <summary>
        /// Об'єкти нерухомого майна
        /// </summary>
        public PersonsInEstates Estates { get; set; }                                   // 
        /// <summary>
        /// Відомості про транспорт (реєстраційні дії)
        /// </summary>
        public List<VehicleViewModel> Vehicles { get; set; }                            // 
        /// <summary>
        /// Перелік знайдених компаний
        /// </summary>
        public List<PersonsInOrganizationsModel> PersonsInOrganizationsModelsList { get; set; } // 
        /// <summary>
        /// Судові рішення (стан розгляду справ)
        /// </summary>
        public List<FairModelView> FairModels { get; set; }                             // 
        /// <summary>
        /// Інтелектуальна власність
        /// </summary>
        public PersonInIntellectualProperty IntellectualProperty { get; set; }          // 
        /// <summary>
        /// Декларації
        /// </summary>
        public List<PersonsInDeclarationsModel> Declarations { get; set; }              // 
        /// <summary>
        /// Ліцензії, дозволи, реєстри
        /// </summary>
        public PersonInLicenses Licenses { get; set; }                                  // 
        /// <summary>
        /// Відомості про осіб що переховуються від органів влади
        /// </summary>
        public List<PersonsHidingViewModel> Hiding { get; set; }                        //  
        /// <summary>
        /// Відомості про втрачені документи
        /// </summary>
        public List<PersonLostDoc> LostDocuments { get; set; }                          // 
        /// <summary>
        /// Судові рішення (призначені по розгляду)
        /// </summary>
        public CourtDecisionAggregationModel CourtAnalytic { get; set; }                // 
        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        public List<PersonInBancrutcy> Bancrutcy { get; set; }                          // 
        /// <summary>
        /// Санкції
        /// </summary>
        public List<PersonSanctionsData> SanctionsDetails { get; set; }                 // 
        /// <summary>
        /// Статус публічної особи (ПЕП) (// 0 - не публічна особа,1 - звязана с публічною особою, 2 - публічная особо, 3 - невідомо, null - нема інформації)
        /// </summary>
        public bool? PublicPerson { get; set; }                                         //  
        /// <summary>
        /// Перелік реєстрів з яких ми успішно/не успішно перевірили дані по ФО
        /// </summary>
        public PersonApiResponseDataRecived CheckList { get; set; }                     // 
        /// <summary>
        /// Санкції перелік id (розширена інформація в SanctionsDetails)
        /// </summary>
        public List<int> Sanctions { get; set; }                                        // 
        /// <summary>
        /// Внутрішній id Vkursi
        /// </summary>
        public Guid? Id { get; set; }                                                   // 

        /// <summary>
        /// Список справ призначених до рогзляду
        /// </summary>
        public List<AssigmentModelView> AssigmentModels { get; set; }

    }
    /// <summary>
    /// Відомості про наявність боргу
    /// </summary>
    public class PersonsInBorgModel                                                     // 
    {/// <summary>
     /// Податковий борг
     /// </summary>
        public List<PodatokBorg> PadatokBorg { get; set; }                              // 
        /// <summary>
        /// Заборгованість по заробітній платі
        /// </summary>
        public List<PaymentBorg> PaymentBorg { get; set; }                              // 
        /// <summary>
        /// Аліменти
        /// </summary>
        public List<ChildBorg> ChildBorg { get; set; }                                  // 
    }
    /// <summary>
    /// Податковий борг
    /// </summary>
    public class PodatokBorg                                                            // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Назва податкової (перед якою є заборгованість)
        /// </summary>
        public string DFSUName { get; set; }                                            // 
        /// <summary>
        /// Заборгованість перед місцевим бюджетом
        /// </summary>
        public double? LocalDebt { get; set; }                                          // 
        /// <summary>
        /// Заборгованість перед державним бюджетом
        /// </summary>
        public double? TotalDebt { get; set; }                                          // 
    }
    /// <summary>
    /// Заборгованість по заробітній платі
    /// </summary>
    public class PaymentBorg                                                             // 
    {/// <summary>
     /// ПІБ боржника
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Кількість проваджень
        /// </summary>
        public int? EnforcementsCount { get; set; }                                     // 
        /// <summary>
        /// Сума боргу
        /// </summary>
        public double? PaymentDebt { get; set; }                                        // 
    }
    /// <summary>
    /// Аліменти
    /// </summary>
    public class ChildBorg                                                              // 
    {/// <summary>
     /// ПІБ боржника
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Дата народження
        /// </summary>
        public DateTime? BirthDay { get; set; }                                         // 
        /// <summary>
        /// Тип заборгованності
        /// </summary>
        public string Type { get; set; }                                                // 
    }
    /// <summary>
    /// Обтяження (ДРОРМ)
    /// </summary>
    public class MovableViewModel                                                       // 
    {/// <summary>
     /// Номер обтяження
     /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Дата реїстрації обтяження
        /// </summary>
        public DateTime? RegDate { get; set; }                                          // 
        /// <summary>
        /// Активно (true - так/ false - ні)
        /// </summary>
        public bool IsActive { get; set; }                                              // 
        /// <summary>
        /// Відомості про обтяжувачів (ДРОРМ)
        /// </summary>
        public List<MovableViewModelSidesBurden> SidesBurdenList { get; set; }          // 
        /// <summary>
        /// Відомості про боржників (ДРОРМ)
        /// </summary>
        public List<MovableViewModelSidesDebtor> SidesDebtorList { get; set; }          // 
        /// <summary>
        /// ???
        /// </summary>
        public string Property { get; set; }
    }
    /// <summary>
    /// Відомості про обтяжувачів (ДРОРМ)
    /// </summary>
    public class MovableViewModelSidesBurden                                            // 
    {/// <summary>
     /// Системний id Vkursi
     /// </summary>
        public Guid? SideId { get; set; }                                               // 
        /// <summary>
        /// Назва обтяжувача
        /// </summary>
        public string BurdenName { get; set; }                                          // 
        /// <summary>
        /// Код обтяжувача
        /// </summary>
        public string BurdenCode { get; set; }                                          // 
    }/// <summary>
     /// Відомості про боржників (ДРОРМ)
     /// </summary>
    public class MovableViewModelSidesDebtor                                            // 
    {/// <summary>
     /// Системний id Vkursi
     /// </summary>
        public Guid? SideId { get; set; }                                               // 
        /// <summary>
        /// Назва боржника
        /// </summary>
        public string DebtorName { get; set; }                                          // 
        /// <summary>
        /// Код боржника
        /// </summary>
        public string DebtorCode { get; set; }                                          // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public List<Guid> OrganizationIdList { get; set; }                              // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public List<Guid> PersonCardIdList { get; set; }                                // 
    }
    /// <summary>
    /// Перелік знайдених ФОП
    /// </summary>
    public class PersonsInFopsModel                                                     // 
    {/// <summary>
     /// Id ФОП-а
     /// </summary>
        public Guid FopId { get; set; }                                                 // 
        /// <summary>
        /// Назва ФОП-а (ПІБ)
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Область
        /// </summary>
        public string RegionName { get; set; }                                          // 
        /// <summary>
        /// Квед
        /// </summary>
        public string Kved { get; set; }                                                // 
        /// <summary>
        /// Статус реєстрації
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Reference { get; set; }                                           // 
    }
    /// <summary>
    /// Відомосто про перевірки контролюючих органів
    /// </summary>
    public class AuditsResult                                                           //  
    {/// <summary>
     /// Системний id Vkursi
     /// </summary>
        public string Id { get; set; }                                                  // 
        /// <summary>
        /// Планова перевірка (true - так/ false - ні)
        /// </summary>
        public int IsPlanned { get; set; }                                              // 
        /// <summary>
        /// Контролюючай орган
        /// </summary>
        public string Regulator { get; set; }                                           // 
        /// <summary>
        /// Id контролюючого органу
        /// </summary>
        public int? RegulatorId { get; set; }                                           // 
        /// <summary>
        /// Код ІПН
        /// </summary>
        public string Code { get; set; }                                                // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Guid? OrganizationId { get; set; }                                       // 
        /// <summary>
        /// Назва суб'єкта перевірки
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Статус перевірки (проведена, не буде проведена, ...)
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Id статуса перевірки
        /// </summary>
        public int? StatusInt { get; set; }                                             // 
        /// <summary>
        /// Id перевірки
        /// </summary>
        public int InternalId { get; set; }                                             // 
        /// <summary>
        /// Дата початку перевірки
        /// </summary>
        public DateTime? DateStart { get; set; }                                        // 
        /// <summary>
        /// Дата завершення перевірки
        /// </summary>
        public DateTime? DateFinish { get; set; }                                       // 
        /// <summary>
        /// Дата останньої зміни
        /// </summary>
        public DateTime? LastModify { get; set; }                                       // 
        /// <summary>
        /// Тип застосованіх санкцій
        /// </summary>
        public string SanctionType { get; set; }                                        // 
        /// <summary>
        /// Сума санкцій
        /// </summary>
        public int? SanctionAmount { get; set; }                                        // 
        /// <summary>
        /// Наявність апеляції на результат перевірки
        /// </summary>
        public int? AppealStage { get; set; }                                           // 
        /// <summary>
        /// Повний об'єкт перевірки
        /// </summary>
        public string AuditObject { get; set; }                                         // 
        /// <summary>
        /// Мета перевірки
        /// </summary>
        public string ActivityType { get; set; }                                        // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public int? MonitoringCheck { get; set; }                                       // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Guid? PersonNameId { get; set; }                                         // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Guid? FopId { get; set; }                                                // 
    }
    /// <summary>
    /// Виконавчі провадження
    /// </summary>
    public class PersonEnforcementsModel                                                // 
    {/// <summary>
     /// Кількість ВП
     /// </summary>
        public int Count { get; set; }                                                  // 
        /// <summary>
        /// Кількість ВП в яких стягувач
        /// </summary>
        public int CreditorCount { get; set; }                                          // 
        /// <summary>
        /// Кількість ВП в яких боржник
        /// </summary>
        public int DebtorCount { get; set; }                                            // 
        /// <summary>
        /// Кількість відкритих ВП
        /// </summary>
        public int OpenCount { get; set; }                                              // 
        /// <summary>
        /// Кількість закритих ВП
        /// </summary>
        public int CloseCount { get; set; }                                             // 
        /// <summary>
        /// Кількість ВП з інштм статусом
        /// </summary>
        public int OtherCount { get; set; }                                             // 
        /// <summary>
        /// Список ВП
        /// </summary>
        public List<PersonEnforcement> PersonEnforcementsList { get; set; }             // 

    }
    /// <summary>
    /// Список ВП
    /// </summary>
    public class PersonEnforcement                                                      // 
    {/// <summary>
     /// Дата відкриття ВП
     /// </summary>
        public DateTime? DateOpened { get; set; }                                       // 
        /// <summary>
        /// Номер ВП
        /// </summary>
        public string VpNumber { get; set; }                                            // 
        /// <summary>
        /// Назва стягувача
        /// </summary>
        public string CreditorName { get; set; }                                        // 
        /// <summary>
        /// Код стягувача
        /// </summary>
        public string CreditorCode { get; set; }                                        // 
        /// <summary>
        /// Посилання на стягувача
        /// </summary>
        public string CreditorReference { get; set; }                                   // 
        /// <summary>
        /// Тип стягувача
        /// </summary>
        public string CreditorType { get; set; }                                        // 
        /// <summary>
        /// Категорія ВП
        /// </summary>
        public string Category { get; set; }                                            // 
        /// <summary>
        /// Стан ВП
        /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Назва боржника
        /// </summary>
        public string DebtorName { get; set; }                                          // 
        /// <summary>
        /// Код боржника
        /// </summary>
        public string DebtorCode { get; set; }                                          // 
        /// <summary>
        /// Посилання на боржника
        /// </summary>
        public string DebtorReference { get; set; }                                     // 
        /// <summary>
        /// Тип боржника
        /// </summary>
        public string DebtorType { get; set; }                                          // 
    }
    /// <summary>
    /// Об'єкти нерухомого майна
    /// </summary>
    public class PersonsInEstates                                                       // 
    {/// <summary>
     /// Об'єкти нерухомого майна наявні
     /// </summary>
        public bool StatusExist { get; set; }                                           // 
        /// <summary>
        /// Земельні ділянки
        /// </summary>
        public List<PersonEstate> PersonEstatesLandList { get; set; }                   // 
        /// <summary>
        /// Об'єкти нерухомості
        /// </summary>
        public List<PersonEstate> PersonEstatesObjectList { get; set; }                 // 
        /// <summary>
        /// Статискика за цільовим призначення
        /// </summary>
        public List<SwotEstatePurposeNRegionStatisticByCadastrModel> Purposes { get; set; }     // 
        /// <summary>
        /// Статискика за регіоном
        /// </summary>
        public List<SwotEstatePurposeNRegionStatisticByCadastrModel> Regions { get; set; }      // 
        /// <summary>
        /// Тим права власності 
        /// </summary>
        public Dictionary<string, int> OwnershipType { get; set; }                      // 
        /// <summary>
        /// В процесі оновлення
        /// </summary>
        public bool UpdateInProgres { get; set; }                                       // 
        /// <summary>
        /// Помилка при оновленні
        /// </summary>
        public bool Error { get; set; }                                                 // 
    }
    /// <summary>
    /// Земельні ділянки | Об'єкти нерухомості
    /// </summary>
    public class PersonEstate                                                           // 
    {/// <summary>
     /// Системний id Vkursi
     /// </summary>
        public Guid Id { get; set; }                                                    // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Guid PersonCardId { get; set; }                                          // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Кількість судових рішень
        /// </summary>
        public int Court { get; set; }                                                  // 
        /// <summary>
        /// Системна інформація Vkursi
        /// </summary>
        public bool LocationChecked { get; set; }                                       // 
        /// <summary>
        /// Розташування 
        /// </summary>
        public string LocationName { get; set; }                                        // 
        /// <summary>
        /// Координати (широта / довгота)
        /// </summary>
        public string LocationCoordinates { get; set; }                                 // 
        /// <summary>
        /// Формма власності
        /// </summary>
        public string FormaVlasnosti { get; set; }                                      // 
        /// <summary>
        /// Площа
        /// </summary>
        public double? Area { get; set; }                                               // 
        /// <summary>
        /// Цільове призначення
        /// </summary>
        public string Purpose { get; set; }                                             // 
        /// <summary>
        /// Перелік категорій 
        /// </summary>
        public List<int> CategoryList { get; set; }                                     // 
        /// <summary>
        /// Перелік статусів
        /// </summary>
        public List<int> StatusList { get; set; }                                       // 
    }/// <summary>
     /// Статискика за цільовим призначення | Статискика за регіоном
     /// </summary>
    public class SwotEstatePurposeNRegionStatisticByCadastrModel                        // 
    {/// <summary>
     /// Назва (регіону | цільового призначення) 
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Кількість об'єктів
        /// </summary>
        public int LandCount { get; set; }                                              // 
        /// <summary>
        /// Площа, га
        /// </summary>
        public double Area { get; set; }                                                // 
        /// <summary>
        /// Кількість судових рішень
        /// </summary>
        public int CourtCount { get; set; }                                             // 
    }
    /// <summary>
    /// Відомості про транспорт (реєстраційні дії)
    /// </summary>
    public class VehicleViewModel                                                       // 
    {/// <summary>
     /// Назва ТЗ (транспортного засобу)
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Тип ТЗ
        /// </summary>
        public string Type { get; set; }                                                // 
        /// <summary>
        /// Марка ТЗ
        /// </summary>
        public string Brand { get; set; }                                               // 
        /// <summary>
        /// Рік реєстраційної дії
        /// </summary>
        public int? ReleaseYear { get; set; }                                           // 
        /// <summary>
        /// Об'єм двигуна
        /// </summary>
        public int? Capacity { get; set; }                                              // 
        /// <summary>
        /// Дата реєстраційної дії
        /// </summary>
        public DateTime? RegDate { get; set; }                                          // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string PersonName { get; set; }                                          // 
        /// <summary>
        /// Назва реєстраційної дії
        /// </summary>
        public string OperationName { get; set; }                                       // 
    }
    /// <summary>
    /// Перелік знайдених компаний
    /// </summary>
    public class PersonsInOrganizationsModel                                            // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Назва компанії
        /// </summary>
        public string OrgName { get; set; }                                             // 
        /// <summary>
        /// Код компанії
        /// </summary>
        public string OrgCode { get; set; }                                             // 
        /// <summary>
        /// Стан компанії
        /// </summary>
        public string OrgStatus { get; set; }                                           // 
        /// <summary>
        /// Країна
        /// </summary>
        public int? CountryCode { get; set; }                                           // 
        /// <summary>
        /// Тип відношення (керівник, власник, ...)
        /// </summary>
        public string ConnectionType { get; set; }                                      // 
        /// <summary>
        /// Адреса
        /// </summary>
        public string Address { get; set; }                                             // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Reference { get; set; }                                           // 
    }
    /// <summary>
    /// Судові рішення (стан розгляду справ)
    /// </summary>
    public class FairModelView                                                          // 
    {/// <summary>
     /// Номер рішення
     /// </summary>
        public string CaseNumber { get; set; }                                          // 
        /// <summary>
        /// Назва суду
        /// </summary>
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Дата назходження судового докуммента
        /// </summary>
        public DateTime? DateNadhodgennia { get; set; }                                 // 
        /// <summary>
        /// Id документа
        /// </summary>
        public int? CaseId { get; set; }                                                // 
        /// <summary>
        /// Номер провадження
        /// </summary>
        public string NumberProvadgennia { get; set; }                                  // 
        /// <summary>
        /// Сторон (перелік)
        /// </summary>
        public string Sides { get; set; }                                               // 
        /// <summary>
        /// Тип сторони в справі
        /// </summary>
        public string PersonSide { get; set; }                                          // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string PersonName { get; set; }                                          // 
        /// <summary>
        /// Предмет позову
        /// </summary>
        public string PredmetPozovu { get; set; }                                       // 
        /// <summary>
        /// Стадія розгляду
        /// </summary>
        public string Stadia { get; set; }                                              // 
        /// <summary>
        /// Категорія рішення
        /// </summary>
        public string DecisionCategory { get; set; }                                    // 
        /// <summary>
        /// Дата рішення
        /// </summary>
        public DateTime? Date { get; set; }                                             // 
    }
    /// <summary>
    /// Інтелектуальна власність
    /// </summary>
    public class PersonInIntellectualProperty                                           // 
    {/// <summary>
     /// Торгові марки
     /// </summary>
        public List<PersonTrademark> PersonTrademarkList { get; set; }                  // 
        /// <summary>
        /// Патенти
        /// </summary>
        public List<PersonPatent> PersonPatentList { get; set; }                        // 
        /// <summary>
        /// Корисні моделі
        /// </summary>
        public List<PersonUsefulModel> UsefulModelList { get; set; }                    // 
    }
    /// <summary>
    /// Торгові марки
    /// </summary>
    public class PersonTrademark                                                        // 
    {/// <summary>
     /// Номер торгової марки
     /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime? RegistrationDate { get; set; }                                 // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string OwnerName { get; set; }                                           // 
        /// <summary>
        /// Кількість власників
        /// </summary>
        public int TotalOwners { get; set; }                                            // 
        /// <summary>
        /// Посилання яна зображення
        /// </summary>
        public string ImageUrl { get; set; }                                            // 
        /// <summary>
        /// Перелік категорій за МКТП
        /// </summary>
        public List<string> MKTPIndex { get; set; }                                     // 
    }/// <summary>
     /// Патенти
     /// </summary>
    public class PersonPatent                                                           // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Кількість власників
        /// </summary>
        public int? TotalOwners { get; set; }                                           // 
        /// <summary>
        /// Кількість заявників
        /// </summary>
        public int? TotalDeclarants { get; set; }                                       // 
        /// <summary>
        /// Кількість винахідників
        /// </summary>
        public int? TotalInventors { get; set; }                                        // 
        /// <summary>
        /// Назва патенту
        /// </summary>
        public string PatentName { get; set; }                                          // 
        /// <summary>
        /// Номер патенту
        /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime? RegistrationDate { get; set; }                                 // 
    }/// <summary>
     /// Корисні моделі
     /// </summary>
    public class PersonUsefulModel                                                      // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Кількість власників
        /// </summary>
        public int? TotalOwners { get; set; }                                           // 
        /// <summary>
        /// Кількість заявників
        /// </summary>
        public int? TotalDeclarants { get; set; }                                       // 
        /// <summary>
        /// Кількість винахідників
        /// </summary>
        public int? TotalInventors { get; set; }                                        // 
        /// <summary>
        /// Назва корисної моделі
        /// </summary>
        public string PatentName { get; set; }                                          // 
        /// <summary>
        /// Номер корисної моделі
        /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime? RegistrationDate { get; set; }                                 // 
    }
    /// <summary>
    /// Декларації
    /// </summary>
    public class PersonsInDeclarationsModel                                             // 
    {/// <summary>
     /// ПіБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Reference { get; set; }                                           // 
        /// <summary>
        /// Рік
        /// </summary>
        public string Year { get; set; }                                                   // 
        /// <summary>
        /// Тип
        /// </summary>
        public string Type { get; set; }                                                // 
        /// <summary>
        /// Посада
        /// </summary>
        public string WorkPost { get; set; }                                            // 
        /// <summary>
        /// Місце роботі
        /// </summary>
        public string WorkPlace { get; set; }                                           // 
        /// <summary>
        /// Наявна нерухомість (true - так / false - ні)
        /// </summary>
        public bool Estates { get; set; }                                               // 
        /// <summary>
        /// Наявні об'єкти незавершеного будівництва (true - так / false - ні)
        /// </summary>
        public bool NotFinishedBuildings { get; set; }                                  // 
        /// <summary>
        /// Наявні обтяження (true - так / false - ні)
        /// </summary>
        public bool Movables { get; set; }                                              // 
        /// <summary>
        /// Наявні транспортні засоби (true - так / false - ні)
        /// </summary>
        public bool Transport { get; set; }                                             // 
        /// <summary>
        /// Наявні цінні папери (true - так / false - ні)
        /// </summary>
        public bool Securities { get; set; }                                            // 
        /// <summary>
        /// Наявні корморативні права (true - так / false - ні)
        /// </summary>
        public bool CorporateRights { get; set; }                                       // 
        /// <summary>
        /// Наявні нематеріальні активи (true - так / false - ні)
        /// </summary>
        public bool Intangible { get; set; }                                            // 
        /// <summary>
        /// Наявні відомості про доходи (true - так / false - ні)
        /// </summary>
        public bool Income { get; set; }                                                // 
        /// <summary>
        /// Статус ПЕП:
        /// 0 - Не публічна особа (ПІБ вказано в декларації і він не ПЕП)
        /// 1 - Особа яку вказано в декларації публічної особи (ПІБ вказано в декларації особи яка є ПЕП)
        /// 2 - Публічна особа (ПІБ вказано в декларації і він є ПЕП)
        /// </summary>
        public int? PublicStatus { get; set; }                                                                                                                          
    }
    /// <summary>
    /// Ліцензії, дозволи, реєстри
    /// </summary>
    public class PersonInLicenses                                                       // 
    {/// <summary>
     /// Кількість діючих ліцензій
     /// </summary>
        public int ActualLicenseCount { get; set; }                                     // 
        /// <summary>
        /// Кількість не діючих ліцензій
        /// </summary>
        public int NonActualLicenseCount { get; set; }                                  // 
        /// <summary>
        /// Перелік ліцензій
        /// </summary>
        public List<PersonLicense> LicensesList { get; set; }                           // 
        /// <summary>
        /// Відомості з реєстру корупційних правопорушень
        /// </summary>
        public List<PersonCorruption> CorruptionsList { get; set; }                     // 
        /// <summary>
        /// Відомості з реєстру про люстрацію
        /// </summary>
        public List<PersonLustration> LustrationsList { get; set; }                     // 
    }
    /// <summary>
    /// Перелік ліцензій
    /// </summary>
    public class PersonLicense                                                          // 
    {/// <summary>
     /// Назва ліцензії
     /// </summary>
        public string LicenseName { get; set; }                                         // 
        /// <summary>
        /// Номер
        /// </summary>
        public string LicenseNumber { get; set; }                                       // 
        /// <summary>
        /// Дата закінчення
        /// </summary>
        public DateTime? DateEnd { get; set; }                                          // 
        /// <summary>
        /// Системне поле
        /// </summary>
        public bool Actual { get; set; }                                                // 
        /// <summary>
        /// Системне поле
        /// </summary>
        public Guid LicenseId { get; set; }                                             // 
        /// <summary>
        /// Є співпадіння по ІПН
        /// </summary>
        public bool? SearchByIpn { get; set; }                                          // 
        /// <summary>
        /// Є співпадіння по ПІБ
        /// </summary>
        public bool? SearchByBirthDay { get; set; }                                     // 
        /// <summary>
        /// ПІБ
        /// </summary>
        public string PersonName { get; set; }                                          // 
    }/// <summary>
     /// Корупційні правопорушення
     /// </summary>
    public class PersonCorruption                                                       // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Місце роботи
        /// </summary>
        public string PlaceOfWork { get; set; }                                         // 
        /// <summary>
        /// Посада
        /// </summary>
        public string Position { get; set; }                                            // 
        /// <summary>
        /// Системний Id
        /// </summary>
        public Guid LicenseId { get; set; }                                             // 
    }/// <summary>
     /// Люстрація
     /// </summary>
    public class PersonLustration                                                       // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Місце роботи
        /// </summary>
        public string PlaceOfWork { get; set; }                                         // 
        /// <summary>
        /// Системний Id
        /// </summary>
        public Guid LicenseId { get; set; }                                             // 
    }
    /// <summary>
    /// Відомості про осіб що переховуються від органів влади
    /// </summary>
    public class PersonsHidingViewModel                                                 // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Дата народження
        /// </summary>
        public DateTime DateBirth { get; set; }                                         // 
        /// <summary>
        /// Орган
        /// </summary>
        public string Ovd { get; set; }                                                 // 
        /// <summary>
        /// Категорія
        /// </summary>
        public string Category { get; set; }                                            // 
        /// <summary>
        /// Дата зникнення
        /// </summary>
        public DateTime? DateLost { get; set; }                                         // 
        /// <summary>
        /// Стаття
        /// </summary>
        public string Paragraph { get; set; }                                           // 
        /// <summary>
        /// Обмеження
        /// </summary>
        public string Restraint { get; set; }                                           // 
        /// <summary>
        /// Контакта інформація відповідного органу
        /// </summary>
        public string ContactInfo { get; set; }                                         // 
        /// <summary>
        /// Посаління на Фото
        /// </summary>
        public string ImageUrl { get; set; }                                            // 
    }
    /// <summary>
    /// Відомості про втрачені документи
    /// </summary>
    public class PersonLostDoc                                                          // 
    {/// <summary>
     /// Серія номер документа
     /// </summary>
        public string SerialNumber { get; set; }                                        // 
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime ActualDate { get; set; }                                        // 
        /// <summary>
        /// Орган
        /// </summary>
        public string Ovd { get; set; }                                                 // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime? RegisterDate { get; set; }                                     // 
    }
    /// <summary>
    /// Судові рішення (призначені по розгляду)
    /// </summary>
    public class CourtDecisionAggregationModel                                          // 
    {/// <summary>
     /// Всього справ
     /// </summary>
        public long? TotalCount { get; set; }                                           // 
        /// <summary>
        /// Позивач
        /// </summary>
        public long? PlaintiffCount { get; set; }                                       // 
        /// <summary>
        /// Відповідач
        /// </summary>
        public long? DefendantCount { get; set; }                                       // 
        /// <summary>
        /// Інша сторона
        /// </summary>
        public long? OtherSideCount { get; set; }                                       // 
        /// <summary>
        /// Програно
        /// </summary>
        public long? LoserCount { get; set; }                                           // 
        /// <summary>
        /// Виграно
        /// </summary>
        public long? WinCount { get; set; }                                             // 
        /// <summary>
        /// Призначено до розгляду
        /// </summary>
        public long? IntendedCount { get; set; }                                        // 
        /// <summary>
        /// Унікальних справ
        /// </summary>
        public long? CaseCount { get; set; }                                            // 
        /// <summary>
        /// В процесі розгляду
        /// </summary>
        public long? InProcess { get; set; }                                            // 
        /// <summary>
        /// загальна кількість документів
        /// </summary>
        public long? TotalDocuments { get; set; }                                       // 
        /// <summary>
        /// Форма судочинства (кримінальні, цивільні, господарські і.т.д)
        /// </summary>
        public IEnumerable<JusticeKinds> ListJusticeKindses { get; set; }               // 
    }
    /// <summary>
    /// Відомості про банкрутство
    /// </summary>
    public class PersonInBancrutcy                                                      // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Номер рішення про банкрутство 
        /// </summary>
        public string Number { get; set; }                                              // 
        /// <summary>
        /// Назва суду
        /// </summary>
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Відомості про публікації на сайте ВГСУ (Вищого господарського суду)
        /// </summary>
        public List<Tuple<DateTime?, string>> Publications { get; set; }                // 
    }
    /// <summary>
    /// Перелік реєстрів з яких ми успішно/не успішно перевірили дані по ФО (true - успішно перевырили / false - не успішно перевырили)
    /// </summary>
    public class PersonApiResponseDataRecived                                           // 
    {/// <summary>
     /// Борг
     /// </summary>
        public bool BorgCheck { get; set; }                                             //
        /// <summary>
        /// ДРОРМ
        /// </summary>
        public bool MovableCheck { get; set; }                                          // 
        /// <summary>
        /// ФОП / не ФОП
        /// </summary>
        public bool PersonsInFopsModelCheck { get; set; }                               // 
        /// <summary>
        /// Виконавчі провадження
        /// </summary>
        public bool EnforcementsCheck { get; set; }                                     // 
        /// <summary>
        /// Нерухомість
        /// </summary>
        public bool EstatesCheck { get; set; }                                          // 
        /// <summary>
        /// Автотранспорт
        /// </summary>
        public bool VehiclesCheck { get; set; }                                         // 
        /// <summary>
        /// По компаніях
        /// </summary>
        public bool PersonsInOrganizationsModelsCheck { get; set; }                     // 
        /// <summary>
        /// Санкції
        /// </summary>
        public bool SanctionsCheck { get; set; }                                        // 
        /// <summary>
        /// Судові справи призначені до розгляду
        /// </summary>
        public bool FairCheck { get; set; }                                             // 
        /// <summary>
        /// Інтелектуальна власність
        /// </summary>
        public bool IntellectualPropertyCheck { get; set; }                             // 
        /// <summary>
        /// Декларації
        /// </summary>
        public bool DeclarationsCheck { get; set; }                                     // 
        /// <summary>
        /// Ліцензії
        /// </summary>
        public bool LicensesCheck { get; set; }                                         // 
        /// <summary>
        /// Прихотуються від влади
        /// </summary>
        public bool HidingCheck { get; set; }                                           // 
        /// <summary>
        /// Втрачені документи
        /// </summary>
        public bool LostDocumentsCheck { get; set; }                                    // 
        /// <summary>
        /// Банкрутство
        /// </summary>
        public bool BancrutcyCheck { get; set; }                                        // 
        /// <summary>
        /// Судові документи
        /// </summary>
        public bool CourtAnalyticCheck { get; set; }                                    // 
        /// <summary>
        /// План графік перевірок
        /// </summary>
        public bool AuditsCheck { get; set; }                                           // 
        /// <summary>
        /// Санкції детально
        /// </summary>
        public bool SanctionsDetailsCheck { get; set; }                                 // 
    }/// <summary>
     /// Санкції
     /// </summary>
    public class PersonSanctionsData                                                    // 
    {/// <summary>
     /// Id санкції
     /// </summary>
        public int SanctionType { get; set; }                                           // 
        /// <summary>
        /// Чи знайдено по ІПН
        /// </summary>
        public bool SearchByIpn { get; set; }                                           // 
        /// <summary>
        /// Дата початку
        /// </summary>
        public DateTime? SanctionStart { get; set; }                                    // 
        /// <summary>
        /// Назва санкції
        /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Додаткові відомості в форматі json
        /// </summary>
        public object Details { get; set; }                                             // 
    }

    /// <summary>
    /// Список справ призначених до рогзляду
    /// </summary>
    public class AssigmentModelView
    {
        public string CourtName { get; set; }
        public string CaseNumber { get; set; }
        public string Sides { get; set; }
        public string Judges { get; set; }
        public string SideType { get; set; }
        public string SideName { get; set; }
        public string PredmetPozovu { get; set; }
        public DateTime? Date { get; set; }
    }
}
