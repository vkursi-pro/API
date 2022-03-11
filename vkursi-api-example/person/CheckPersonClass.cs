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

    public class CheckPersonRequestBodyModel                                            // Модель запиту (обмеження 1)
    {
        /// <summary>
        /// ПІБ
        /// </summary>
        /// <example>ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ</example>
        public string FullName { get; set; }                                            // ПІБ
        public string FirstName { get; set; }                                           // Призвище (необовязкове якщо вказаний FullName)
        public string SecondName { get; set; }                                          // Ім'я (необовязкове якщо вказаний FullName)
        public string LastName { get; set; }                                            // По батькові (необовязкове)
        public string Ipn { get; set; }                                                 // ІНП
        public string Doc { get; set; }                                                 // Серія номер пасторта (XX123456)
        public DateTime? Birthday { get; set; }                                         // Дата народження
        public string RuName { get; set; }                                              // ПІБ на російській мові
    }


    public class CheckPersonResponseModel                                               // Модель на відповідь
    {
        public string Status { get; set; }                                              // Статус відповіді по API
        public bool IsSucces { get; set; }                                              // Чи успішний запит
        public string Name { get; set; }                                                // ПІБ
        public string Ipn { get; set; }                                                 // ІНП
        public string Passport { get; set; }                                            // Серія номер пасторта (XX123456)
        public string NameRu { get; set; }                                              // ПІБ на російській мові
        public DateTime? DateBirth { get; set; }                                        // Дата народження
        public PersonsInBorgModel Borg { get; set; }                                    // Відомості про наявність боргу
        public List<MovableViewModel> Movables { get; set; }                            // Обтяження (ДРОРМ)
        public List<PersonsInFopsModel> PersonsInFopsModelList { get; set; }            // Перелік знайдених ФОП
        public List<AuditsResult> Audits { get; set; }                                  // Відомосто про перевірки контролюючих органів 
        public PersonEnforcementsModel Enforcements { get; set; }                       // Виконавчі провадження
        public PersonsInEstates Estates { get; set; }                                   // Об'єкти нерухомого майна
        public List<VehicleViewModel> Vehicles { get; set; }                            // Відомості про транспорт (реєстраційні дії)
        public List<PersonsInOrganizationsModel> PersonsInOrganizationsModelsList { get; set; } // Перелік знайдених компаний
        public List<FairModelView> FairModels { get; set; }                             // Судові рішення (стан розгляду справ)
        public PersonInIntellectualProperty IntellectualProperty { get; set; }          // Інтелектуальна власність
        public List<PersonsInDeclarationsModel> Declarations { get; set; }              // Декларації
        public PersonInLicenses Licenses { get; set; }                                  // Ліцензії, дозволи, реєстри
        public List<PersonsHidingViewModel> Hiding { get; set; }                        //  Відомості про осіб що переховуються від органів влади
        public List<PersonLostDoc> LostDocuments { get; set; }                          // Відомості про втрачені документи
        public CourtDecisionAggregationModel CourtAnalytic { get; set; }                // Судові рішення (призначені по розгляду)
        public List<PersonInBancrutcy> Bancrutcy { get; set; }                          // Відомості про банкрутство
        public List<PersonSanctionsData> SanctionsDetails { get; set; }                 // Санкції
        public bool? PublicPerson { get; set; }                                         // Статус публічної особи (ПЕП) (// 0 - не публічна особа, 1 - звязана с публічною особою, 2 - публічная особо, 3 - невідомо, null - нема інформації)
        public PersonApiResponseDataRecived CheckList { get; set; }                     // Перелік реєстрів з яких ми успішно/не успішно перевірили дані по ФО
        public List<int> Sanctions { get; set; }                                        // Санкції перелік id (розширена інформація в SanctionsDetails)
        public Guid? Id { get; set; }                                                   // Внутрішній id Vkursi

    }

    public class PersonsInBorgModel                                                     // Відомості про наявність боргу
    {
        public List<PodatokBorg> PadatokBorg { get; set; }                              // Податковий борг
        public List<PaymentBorg> PaymentBorg { get; set; }                              // Заборгованість по заробітній платі
        public List<ChildBorg> ChildBorg { get; set; }                                  // Аліменти
    }

    public class PodatokBorg                                                            // Податковий борг
    {
        public string Name { get; set; }                                                // ПІБ
        public string DFSUName { get; set; }                                            // Назва податкової (перед якою є заборгованість)
        public double? LocalDebt { get; set; }                                          // Заборгованість перед місцевим бюджетом
        public double? TotalDebt { get; set; }                                          // Заборгованість перед державним бюджетом
    }

    public class PaymentBorg                                                             // Заборгованість по заробітній платі
    {
        public string Name { get; set; }                                                // ПІБ боржника
        public int? EnforcementsCount { get; set; }                                     // Кількість проваджень
        public double? PaymentDebt { get; set; }                                        // Сума боргу
    }

    public class ChildBorg                                                              // Аліменти
    {
        public string Name { get; set; }                                                // ПІБ боржника
        public DateTime? BirthDay { get; set; }                                         // Дата народження
        public string Type { get; set; }                                                // Тип заборгованності
    }

    public class MovableViewModel                                                       // Обтяження (ДРОРМ)
    {
        public string Number { get; set; }                                              // Номер обтяження
        public DateTime? RegDate { get; set; }                                          // Дата реїстрації обтяження
        public bool IsActive { get; set; }                                              // Активно (true - так/ false - ні)
        public List<MovableViewModelSidesBurden> SidesBurdenList { get; set; }          // Відомості про обтяжувачів (ДРОРМ)
        public List<MovableViewModelSidesDebtor> SidesDebtorList { get; set; }          // Відомості про боржників (ДРОРМ)
        public string Property { get; set; }
    }

    public class MovableViewModelSidesBurden                                            // Відомості про обтяжувачів (ДРОРМ)
    {
        public Guid? SideId { get; set; }                                               // Системний id Vkursi
        public string BurdenName { get; set; }                                          // Назва обтяжувача
        public string BurdenCode { get; set; }                                          // Код обтяжувача
    }
    public class MovableViewModelSidesDebtor                                            // Відомості про боржників (ДРОРМ)
    {
        public Guid? SideId { get; set; }                                               // Системний id Vkursi
        public string DebtorName { get; set; }                                          // Назва боржника
        public string DebtorCode { get; set; }                                          // Код боржника
        public List<Guid> OrganizationIdList { get; set; }                              // Системна інформація Vkursi
        public List<Guid> PersonCardIdList { get; set; }                                // Системна інформація Vkursi
    }

    public class PersonsInFopsModel                                                     // Перелік знайдених ФОП
    {
        public Guid FopId { get; set; }                                                 // Id ФОП-а
        public string Name { get; set; }                                                // Назва ФОП-а (ПІБ)
        public string RegionName { get; set; }                                          // Область
        public string Kved { get; set; }                                                // Квед
        public string Status { get; set; }                                              // Статус реєстрації
        public string Reference { get; set; }                                           // Посилання
    }

    public class AuditsResult                                                           // Відомосто про перевірки контролюючих органів 
    {
        public string Id { get; set; }                                                  // Системний id Vkursi
        public int IsPlanned { get; set; }                                              // Планова перевірка (true - так/ false - ні)
        public string Regulator { get; set; }                                           // Контролюючай орган
        public int? RegulatorId { get; set; }                                           // Id контролюючого органу
        public string Code { get; set; }                                                // Код ІПН
        public Guid? OrganizationId { get; set; }                                       // Системний id Vkursi
        public string Name { get; set; }                                                // Назва суб'єкта перевірки
        public string Status { get; set; }                                              // Статус перевірки (проведена, не буде проведена, ...)
        public int? StatusInt { get; set; }                                             // Id статуса перевірки
        public int InternalId { get; set; }                                             // Id перевірки
        public DateTime? DateStart { get; set; }                                        // Дата початку перевірки
        public DateTime? DateFinish { get; set; }                                       // Дата завершення перевірки
        public DateTime? LastModify { get; set; }                                       // Дата останньої зміни
        public string SanctionType { get; set; }                                        // Тип застосованіх санкцій
        public int? SanctionAmount { get; set; }                                        // Сума санкцій
        public int? AppealStage { get; set; }                                           // Наявність апеляції на результат перевірки
        public string AuditObject { get; set; }                                         // Повний об'єкт перевірки
        public string ActivityType { get; set; }                                        // Мета перевірки
        public int? MonitoringCheck { get; set; }                                       // Системний id Vkursi
        public Guid? PersonNameId { get; set; }                                         // Системний id Vkursi
        public Guid? FopId { get; set; }                                                // Системний id Vkursi
    }

    public class PersonEnforcementsModel                                                // Виконавчі провадження
    {
        public int Count { get; set; }                                                  // Кількість ВП
        public int CreditorCount { get; set; }                                          // Кількість ВП в яких стягувач
        public int DebtorCount { get; set; }                                            // Кількість ВП в яких боржник
        public int OpenCount { get; set; }                                              // Кількість відкритих ВП
        public int CloseCount { get; set; }                                             // Кількість закритих ВП
        public int OtherCount { get; set; }                                             // Кількість ВП з інштм статусом
        public List<PersonEnforcement> PersonEnforcementsList { get; set; }             // Список ВП

    }

    public class PersonEnforcement                                                      // Список ВП
    {
        public DateTime? DateOpened { get; set; }                                       // Дата відкриття ВП
        public string VpNumber { get; set; }                                            // Номер ВП
        public string CreditorName { get; set; }                                        // Назва стягувача
        public string CreditorCode { get; set; }                                        // Код стягувача
        public string CreditorReference { get; set; }                                   // Посилання на стягувача
        public string CreditorType { get; set; }                                        // Тип стягувача
        public string Category { get; set; }                                            // Категорія ВП
        public string State { get; set; }                                               // Стан ВП
        public string DebtorName { get; set; }                                          // Назва боржника
        public string DebtorCode { get; set; }                                          // Код боржника
        public string DebtorReference { get; set; }                                     // Посилання на боржника
        public string DebtorType { get; set; }                                          // Тип боржника
    }

    public class PersonsInEstates                                                       // Об'єкти нерухомого майна
    {
        public bool StatusExist { get; set; }                                           // Об'єкти нерухомого майна наявні
        public List<PersonEstate> PersonEstatesLandList { get; set; }                   // Земельні ділянки
        public List<PersonEstate> PersonEstatesObjectList { get; set; }                 // Об'єкти нерухомості
        public List<SwotEstatePurposeNRegionStatisticByCadastrModel> Purposes { get; set; }     // Статискика за цільовим призначення
        public List<SwotEstatePurposeNRegionStatisticByCadastrModel> Regions { get; set; }      // Статискика за регіоном
        public Dictionary<string, int> OwnershipType { get; set; }                      // Тим права власності 
        public bool UpdateInProgres { get; set; }                                       // В процесі оновлення
        public bool Error { get; set; }                                                 // Помилка при оновленні
    }

    public class PersonEstate                                                           // Земельні ділянки | Об'єкти нерухомості
    {
        public Guid Id { get; set; }                                                    // Системний id Vkursi
        public Guid PersonCardId { get; set; }                                          // Системний id Vkursi
        public string Name { get; set; }                                                // ПІБ
        public int Court { get; set; }                                                  // Кількість судових рішень
        public bool LocationChecked { get; set; }                                       // Системна інформація Vkursi
        public string LocationName { get; set; }                                        // Розташування 
        public string LocationCoordinates { get; set; }                                 // Координати (широта / довгота)
        public string FormaVlasnosti { get; set; }                                      // Формма власності
        public double? Area { get; set; }                                               // Площа
        public string Purpose { get; set; }                                             // Цільове призначення
        public List<int> CategoryList { get; set; }                                     // Перелік категорій 
        public List<int> StatusList { get; set; }                                       // Перелік статусів
    }
    public class SwotEstatePurposeNRegionStatisticByCadastrModel                        // Статискика за цільовим призначення | Статискика за регіоном
    {
        public string Name { get; set; }                                                // Назва (регіону | цільового призначення) 
        public int LandCount { get; set; }                                              // Кількість об'єктів
        public double Area { get; set; }                                                // Площа, га
        public int CourtCount { get; set; }                                             // Кількість судових рішень
    }

    public class VehicleViewModel                                                       // Відомості про транспорт (реєстраційні дії)
    {
        public string Name { get; set; }                                                // Назва ТЗ (транспортного засобу)
        public string Type { get; set; }                                                // Тип ТЗ
        public string Brand { get; set; }                                               // Марка ТЗ
        public int? ReleaseYear { get; set; }                                           // Рік реєстраційної дії
        public int? Capacity { get; set; }                                              // Об'єм двигуна
        public DateTime? RegDate { get; set; }                                          // Дата реєстраційної дії
        public string PersonName { get; set; }                                          // ПІБ
        public string OperationName { get; set; }                                       // Назва реєстраційної дії
    }

    public class PersonsInOrganizationsModel                                            // Перелік знайдених компаний
    {
        public string Name { get; set; }                                                // ПІБ
        public string OrgName { get; set; }                                             // Назва компанії
        public string OrgCode { get; set; }                                             // Код компанії
        public string OrgStatus { get; set; }                                           // Стан компанії
        public int? CountryCode { get; set; }                                           // Країна
        public string ConnectionType { get; set; }                                      // Тип відношення (керівник, власник, ...)
        public string Address { get; set; }                                             // Адреса
        public string Reference { get; set; }                                           // Посилання
    }

    public class FairModelView                                                          // Судові рішення (стан розгляду справ)
    {
        public string CaseNumber { get; set; }                                          // Номер рішення
        public string CourtName { get; set; }                                           // Назва суду
        public DateTime? DateNadhodgennia { get; set; }                                 // Дата назходження судового докуммента
        public int? CaseId { get; set; }                                                // Id документа
        public string NumberProvadgennia { get; set; }                                  // Номер провадження
        public string Sides { get; set; }                                               // Сторон (перелік)
        public string PersonSide { get; set; }                                          // Тип сторони в справі
        public string PersonName { get; set; }                                          // ПІБ
        public string PredmetPozovu { get; set; }                                       // Предмет позову
        public string Stadia { get; set; }                                              // Стадія розгляду
        public string DecisionCategory { get; set; }                                    // Категорія рішення
        public DateTime? Date { get; set; }                                             // Дата рішення
    }

    public class PersonInIntellectualProperty                                           // Інтелектуальна власність
    {
        public List<PersonTrademark> PersonTrademarkList { get; set; }                  // Торгові марки
        public List<PersonPatent> PersonPatentList { get; set; }                        // Патенти
        public List<PersonUsefulModel> UsefulModelList { get; set; }                    // Корисні моделі
    }

    public class PersonTrademark                                                        // Торгові марки
    {
        public string Number { get; set; }                                              // Номер торгової марки
        public DateTime? RegistrationDate { get; set; }                                 // Дата реєстрації
        public string OwnerName { get; set; }                                           // ПІБ
        public int TotalOwners { get; set; }                                            // Кількість власників
        public string ImageUrl { get; set; }                                            // Посилання яна зображення
        public List<string> MKTPIndex { get; set; }                                     // Перелік категорій за МКТП
    }
    public class PersonPatent                                                           // Патенти
    {
        public string Name { get; set; }                                                // ПІБ
        public int? TotalOwners { get; set; }                                           // Кількість власників
        public int? TotalDeclarants { get; set; }                                       // Кількість заявників
        public int? TotalInventors { get; set; }                                        // Кількість винахідників
        public string PatentName { get; set; }                                          // Назва патенту
        public string Number { get; set; }                                              // Номер патенту
        public DateTime? RegistrationDate { get; set; }                                 // Дата реєстрації
    }
    public class PersonUsefulModel                                                      // Корисні моделі
    {
        public string Name { get; set; }                                                // ПІБ
        public int? TotalOwners { get; set; }                                           // Кількість власників
        public int? TotalDeclarants { get; set; }                                       // Кількість заявників
        public int? TotalInventors { get; set; }                                        // Кількість винахідників
        public string PatentName { get; set; }                                          // Назва корисної моделі
        public string Number { get; set; }                                              // Номер корисної моделі
        public DateTime? RegistrationDate { get; set; }                                 // Дата реєстрації
    }

    public class PersonsInDeclarationsModel                                             // Декларації
    {
        public string Name { get; set; }                                                // ПіБ
        public string Reference { get; set; }                                           // Посилання
        public int Year { get; set; }                                                   // Рік
        public string Type { get; set; }                                                // Тип
        public string WorkPost { get; set; }                                            // Посада
        public string WorkPlace { get; set; }                                           // Місце роботі
        public bool Estates { get; set; }                                               // Наявна нерухомість (true - так / false - ні)
        public bool NotFinishedBuildings { get; set; }                                  // Наявні об'єкти незавершеного будівництва (true - так / false - ні)
        public bool Movables { get; set; }                                              // Наявні обтяження (true - так / false - ні)
        public bool Transport { get; set; }                                             // Наявні транспортні засоби (true - так / false - ні)
        public bool Securities { get; set; }                                            // Наявні цінні папери (true - так / false - ні)
        public bool CorporateRights { get; set; }                                       // Наявні корморативні права (true - так / false - ні)
        public bool Intangible { get; set; }                                            // Наявні нематеріальні активи (true - так / false - ні)
        public bool Income { get; set; }                                                // Наявні відомості про доходи (true - так / false - ні)

        public int? PublicStatus { get; set; }                                          // Статус ПЕП:
                                                                                        // 0 - Не публічна особа (ПІБ вказано в декларації і він не ПЕП) 
                                                                                        // 1 - Особа яку вказано в декларації публічної особи (ПІБ вказано в декларації особи яка є ПЕП)
                                                                                        // 2 - Публічна особа (ПІБ вказано в декларації і він є ПЕП)
    }

    public class PersonInLicenses                                                       // Ліцензії, дозволи, реєстри
    {
        public int ActualLicenseCount { get; set; }                                     // Кількість діючих ліцензій
        public int NonActualLicenseCount { get; set; }                                  // Кількість не діючих ліцензій
        public List<PersonLicense> LicensesList { get; set; }                           // Перелік ліцензій
        public List<PersonCorruption> CorruptionsList { get; set; }                     // Відомості з реєстру корупційних правопорушень
        public List<PersonLustration> LustrationsList { get; set; }                     // Відомості з реєстру про люстрацію
    }

    public class PersonLicense                                                          // Перелік ліцензій
    {
        public string LicenseName { get; set; }                                         // Назва ліцензії
        public string LicenseNumber { get; set; }                                       // Номер
        public DateTime? DateEnd { get; set; }                                          // Дата закінчення
        public bool Actual { get; set; }                                                // Системне поле
        public Guid LicenseId { get; set; }                                             // Системне поле
        public bool? SearchByIpn { get; set; }                                          // Є співпадіння по ІПН
        public bool? SearchByBirthDay { get; set; }                                     // Є співпадіння по ПІБ
        public string PersonName { get; set; }                                          // ПІБ
    }
    public class PersonCorruption                                                       // Корупційні правопорушення
    {
        public string Name { get; set; }                                                // ПІБ
        public string PlaceOfWork { get; set; }                                         // Місце роботи
        public string Position { get; set; }                                            // Посада
        public Guid LicenseId { get; set; }                                             // Системний Id
    }
    public class PersonLustration                                                       // Люстрація
    {
        public string Name { get; set; }                                                // ПІБ
        public string PlaceOfWork { get; set; }                                         // Місце роботи
        public Guid LicenseId { get; set; }                                             // Системний Id
    }

    public class PersonsHidingViewModel                                                 // Відомості про осіб що переховуються від органів влади
    {
        public string Name { get; set; }                                                // ПІБ
        public DateTime DateBirth { get; set; }                                         // Дата народження
        public string Ovd { get; set; }                                                 // Орган
        public string Category { get; set; }                                            // Категорія
        public DateTime? DateLost { get; set; }                                         // Дата зникнення
        public string Paragraph { get; set; }                                           // Стаття
        public string Restraint { get; set; }                                           // Обмеження
        public string ContactInfo { get; set; }                                         // Контакта інформація відповідного органу
        public string ImageUrl { get; set; }                                            // Посаління на Фото
    }

    public class PersonLostDoc                                                          // Відомості про втрачені документи
    {
        public string SerialNumber { get; set; }                                        // Серія номер документа
        public string Status { get; set; }                                              // Статус
        public DateTime ActualDate { get; set; }                                        // Дата
        public string Ovd { get; set; }                                                 // Орган
        public DateTime? RegisterDate { get; set; }                                     // Дата реєстрації
    }

    public class CourtDecisionAggregationModel                                          // Судові рішення (призначені по розгляду)
    {
        public long? TotalCount { get; set; }                                           // Всього справ
        public long? PlaintiffCount { get; set; }                                       // Позивач
        public long? DefendantCount { get; set; }                                       // Відповідач
        public long? OtherSideCount { get; set; }                                       // Інша сторона
        public long? LoserCount { get; set; }                                           // Програно
        public long? WinCount { get; set; }                                             // Виграно
        public long? IntendedCount { get; set; }                                        // Призначено до розгляду

        public long? CaseCount { get; set; }                                            // Унікальних справ
        public long? InProcess { get; set; }                                            // В процесі розгляду

        public long? TotalDocuments { get; set; }                                       // загальна кількість документів

        public IEnumerable<JusticeKinds> ListJusticeKindses { get; set; }               // Форма судочинства (кримінальні, цивільні, господарські і.т.д)
    }

    public class PersonInBancrutcy                                                      // Відомості про банкрутство
    {
        public string Name { get; set; }                                                // ПІБ
        public string Number { get; set; }                                              // Номер рішення про банкрутство 
        public string CourtName { get; set; }                                           // Назва суду
        public List<Tuple<DateTime?, string>> Publications { get; set; }                // Відомості про публікації на сайте ВГСУ (Вищого господарського суду)
    }

    public class PersonApiResponseDataRecived                                           // Перелік реєстрів з яких ми успішно/не успішно перевірили дані по ФО (true - успішно перевырили / false - не успішно перевырили)
    {
        public bool BorgCheck { get; set; }                                             //Борг
        public bool MovableCheck { get; set; }                                          // ДРОРМ
        public bool PersonsInFopsModelCheck { get; set; }                               // ФОП / не ФОП
        public bool EnforcementsCheck { get; set; }                                     // Виконавчі провадження
        public bool EstatesCheck { get; set; }                                          // Нерухомість
        public bool VehiclesCheck { get; set; }                                         // Автотранспорт
        public bool PersonsInOrganizationsModelsCheck { get; set; }                     // По компаніяї
        public bool SanctionsCheck { get; set; }                                        // Санкції
        public bool FairCheck { get; set; }                                             // Судові справи призначені до розгляду
        public bool IntellectualPropertyCheck { get; set; }                             // Інтелектуальна власність
        public bool DeclarationsCheck { get; set; }                                     // Декларації
        public bool LicensesCheck { get; set; }                                         // Ліцензії
        public bool HidingCheck { get; set; }                                           // Прихотуються від влади
        public bool LostDocumentsCheck { get; set; }                                    // Втрачені документи
        public bool BancrutcyCheck { get; set; }                                        // Банкрутство
        public bool CourtAnalyticCheck { get; set; }                                    // Судові документи
        public bool AuditsCheck { get; set; }                                           // План графік перевірок
        public bool SanctionsDetailsCheck { get; set; }                                 // Санкції детально
    }
    public class PersonSanctionsData                                                    // Санкції
    {
        public int SanctionType { get; set; }                                           // Id санкції
        public bool SearchByIpn { get; set; }                                           // Чи знайдено по ІПН
        public DateTime? SanctionStart { get; set; }                                    // Дата початку
        public string Name { get; set; }                                                // Назва санкції
        public object Details { get; set; }                                             // Додаткові відомості в форматі json
    }
}
