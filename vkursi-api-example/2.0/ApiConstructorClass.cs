using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;
using vkursi_api_example.organizations;
using System.ComponentModel.DataAnnotations;
using vkursi_api_example.courtdecision;
using vkursi_api_example.person;
using vkursi_api_example.changes;
using vkursi_api_example.movableloads;

namespace vkursi_api_example._2._0
{
    public class ApiConstructorClass
    {
        /*

        Метод:
            69. API 2.0 Конструктор API 
            [POST] /api/2.0/ApiConstructor

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/ApiConstructorResponse.json
        
         */

        public static ApiConstructorResponseModel ApiConstructor(ref string token, string edrpou, HashSet<int> methodsToExecute)
        {
            if (string.IsNullOrEmpty(token)) 
            {
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/2.0/ApiConstructor");
                RestRequest request = new RestRequest(Method.POST);

                ApiConstructorRequestBodyModel GOFSRequest = new ApiConstructorRequestBodyModel
                {
                    Edrpou = new List<string> { edrpou },
                    MethodsToExecute = methodsToExecute
                };

                string body = JsonConvert.SerializeObject(GOFSRequest); // Example: {"Edrpou":["41462280"],"Ipn":null,"MethodsToExecute":[4,9,41,37,57,66,32,39,70],"GetAdvancedOrganizationFilter":null,"GetRelationsFilter":null,"ShortFinanceYearFilter":null}

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

            ApiConstructorResponseModel ACResponseRow = new ApiConstructorResponseModel();

            ACResponseRow = JsonConvert.DeserializeObject<ApiConstructorResponseModel>(responseString);

            // Тут можна переглянути json відповіді
            string apiConstructorResponseString = JsonConvert.SerializeObject(ACResponseRow, Formatting.Indented);

            return ACResponseRow;
        }
    }

    /// <summary>
    /// Модель Body на API конструктор api/2.0/ApiConstructor
    /// </summary>
    public class ApiConstructorRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public List<string> Edrpou { get; set; }
        /// <summary>
        /// Код ІПН
        /// </summary>
        public List<string> Ipn { get; set; }
        /// <summary>
        /// Перелік методів по яким буде віконано пошук
        /// </summary>
        public HashSet<int> MethodsToExecute { get; set; }
        /// <summary>
        /// Додатковый параметр. Чи потрібній прямій запит на Nais (true - так / false - ні)
        /// </summary>
        public Api2GetAdvancedOrganizationFilter GetAdvancedOrganizationFilter { get; set; }
        /// <summary>
        /// Додатковый параметр. Для звязків 
        /// </summary>
        public Api2GetRelationsFilter GetRelationsFilter { get; set; }
        /// <summary>
        /// Додатковый параметр. Для фінансів 
        /// </summary>
        public HashSet<int> ShortFinanceYearFilter { get; set; }
        /// <summary>
        /// Фільтр для отримання істориї змін 
        /// </summary>
        public ApiConstructorGetChangesByCodeFilter GetChangesByCodeFilter { get; set; }
        /// <summary>
        /// Фільтр для отримання судових рішень
        /// </summary>
        public Api2GetCourtDecisionFilterScroll CourtDecisionFilter { get; set; }
        /// <summary>
        /// Фільтр для отримання вімостей по фізичним особам
        /// </summary>
        public PersonApiRequest CheckPersonFilter { get; set; }
    }

    /// <summary>
    /// Додатковый параметр. Чи потрыбный прямий запит на Nais (true - так / false - ні)
    /// </summary>
    public class Api2GetAdvancedOrganizationFilter
    {
        /// <summary>
        /// Чи потрібний прямий запит на Nais (true - так / false - ні)
        /// </summary>
        public bool? NeedUpdate { get; set; } = true;
    }

    /// <summary>
    /// Додаткові параметри для звязків 
    /// </summary>
    public class Api2GetRelationsFilter
    {
        /// <summary>
        /// Фільтр по тіпу звязків
        /// </summary>
        public HashSet<int> FilterRelationType { get; set; }
        /// <summary>
        /// Кількість рівнів
        /// </summary>
        public int? MaxRelationLevel { get; set; } = 2;
        /// <summary>
        /// Id звязку
        /// </summary>
        public List<string> RelationId { get; set; }
    }

    /// <summary>
    /// Відповідь на API запит конструктор api/2.0/ApiConstructor
    /// </summary>
    public class ApiConstructorResponseModel
    {
        /// <summary>
        /// Перелік методів по яким віявлені помилки
        /// </summary>
        public List<int> ErrorList { get; set; }
        /// <summary>
        /// Відповідь по методу 41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
        /// </summary>
        public List<Api2AnswerModelRelationData> GetRelationsData { get; set; }
        /// <summary>
        /// Відповідь по методу 37. Перелік ліцензій, та дозволів
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgLicensesInfoClass.cs#L84
        /// </summary>
        public List<OrgLicensesApiApiAnswerModelData> GetOrgLicensesInfoData { get; set; }
        /// <summary>
        /// Відповідь по методу 57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgFinanceClass.cs#L123
        /// </summary>
        public List<Api2AnswerModelOrgFinanceData> GetOrgFinanceData { get; set; }
        /// <summary>
        /// Відповідь по методу 9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAnalyticClass.cs#L132
        /// </summary>
        public List<GetAnalyticResponseModel> GetAnalyticData { get; set; }
        /// <summary>
        /// Відповідь по методу 4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L124
        /// </summary>
        public List<OrganizationaisElasticModel> GetAdvancedOrganizationData { get; set; }
        /// <summary>
        /// Відповідь по методу 66. Отримати дані реквізитів для строреня картки ФОП / ЮО
        /// </summary>
        public List<Api2AnswerModelGetRequisites> GetRequisitesData { get; set; }
        /// <summary>
        /// Відповідь по методу 32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
        /// </summary>
        public List<Api2AnswerModelVehicleData> VehicleData { get; set; }
        /// <summary>
        /// Відповідь по методу 39. Відомості про власників пакетів акцій (від 5%)
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgShareholdersClass.cs#L121
        /// </summary>
        public List<OrgShareHoldersApiAnswerModelData> ShareHoldersData { get; set; }
        /// <summary>
        /// Відповідь по методу 69. Скорочені основні фінансові показники діяльності підприємства 
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgFinanceShortClass.cs#L88
        /// </summary>
        public List<GetOrgFinanceShortAnswerData> ShortFinanceData { get; set; }
        /// <summary>
        /// Фінансово промислові групи
        /// </summary>
        public List<ApiGetFinancialIndustrialGroupModelAnswerData> FinancialIndustrialGroupData { get; set; }
        /// <summary>
        /// Перевірка чи знаходиться аблеса компанії/ФОП в зоні АТО (доступно в конструкторі API №76)
        /// </summary>
        public List<CheckIfOrganizationsInAtoAnswerData> CheckIfOrganizationsInAtoAnswerData { get; set; }
        /// <summary>
        /// Перевірка ФОП/не ФОП, наявність ліцензій адвокат/нотаріус, наявність обтяжень ДРОРМ
        /// /api/1.0/person/CheckFopStatus
        /// </summary>
        public List<CheckFopStatusModelAnswerData> CheckFopStatusModelAnwserData { get; set; }
        /// <summary>
        /// Перевірка контрагента в списках санкції
        /// /api/1.0/organizations/GetOrgSanctions
        /// </summary>
        public List<OrganizationSanctionDetail> OrganizationSanctionDetailData { get; set; }
        /// <summary>
        /// Реєстр платників ПДВ
        /// /api/1.0/organizations/GetOrgPdv
        /// </summary>
        public List<OrgPdvInfo> OrgPdvInfoData { get; set; }
        /// <summary>
        /// Анульована реєстрація платником ПДВ
        /// /api/1.0/organizations/GetOrgAnulPdv
        /// </summary>
        public List<OrgAnulPdvInfo> OrgAnulPdvInfoData { get; set; }
        /// <summary>
        /// Реєстр платників Єдиного податку
        /// /api/1.0/organizations/GetYedynyyPodatok
        /// </summary>
        public List<OrgYedunuyPodatok> OrgYedunuyPodatokData { get; set; }
        /// <summary>
        /// Реєстр ДФС “Дізнайся більше про свого бізнес-партнера”
        /// /api/1.0/organizations/GetDfsBussinesPartnerData
        /// </summary>
        public List<OrgDfsInfo> OrgDfsInfoData { get; set; }
        /// <summary>
        /// Єдиний реєстр боржників
        /// /api/1.0/organizations/GetOpenEnforcements
        /// </summary>
        public List<EnforcementsData> EnforcementsData { get; set; }
        /// <summary>
        /// Єдиний реєстр боржників
        /// /api/1.0/organizations/GetOpenEnforcements
        /// </summary>
        public List<EnforcementsData> OpenEnforcementsData { get; set; }
        /// <summary>
        /// Штатна чисельність працівників
        /// [POST] /api/1.0/organizations/GetKilkistPracivnukiv
        /// </summary>
        public List<EmployeesData> EmployeesData { get; set; }
        /// <summary>
        /// URL на API для отримання відомостей по нерухомості
        /// </summary>
        public List<EstateData> EstateData { get; set; }
        /// <summary>
        /// ДРОРМ отримання скороченных даних про наявні обтяження на рухоме майно по ІПН / ЄДРПОУ
        /// </summary>
        public List<MovableData> MovableData { get; set; }
        /// <summary>
        /// Реєстр ДФС “Стан розрахунків з бюджетом”
        /// /api/organizations/GetOrganizationDbDebtorsDfs
        /// </summary>
        public List<DebtData> DebtData { get; set; }
        /// <summary>
        /// 4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
        /// /api/1.0/organizations/getadvancedorganization
        /// </summary>
        public List<OrganizationaisElasticModel> NaisEdrData { get; set; }
        /// <summary>
        /// 86. Відомості про банкрутство ВГСУ
        /// /api/1.0/organizations/getBankruptcyByCode
        /// </summary>
        public List<GetBankruptcyByCodeModelAnswerData> BankruptcyVgsuData { get; set; }
        /// <summary>
        /// Дані щоденного моніторингу по фізичним, юриличним особам та об'єктам нерухомого майна які додані на моніторинг
        /// </summary>
        public List<ChangeListData> ChangeListData { get; set; }
        /// <summary>
        /// Судові рішення
        /// </summary>
        public List<CourtDecisionExtendedModelFilterData> CourtDecisionExtendedModelFilterData { get; set; }
        /// <summary>
        /// 72. API з історії зміни даних в ЄДР
        /// /api/1.0/organizations/getactivityorghistory
        /// </summary>
        public List<ActivityOrgHistoryApiModelAnswerData> ActivityOrgHistoryApiModelAnswerData { get; set; }
        /// <summary>
        /// 29. Отримання інформації по фізичній особі
        /// /api/1.0/person/checkperson
        /// </summary>
        public List<CheckPersonResponseModel> CheckPersonResponseData { get; set; }
        /// <summary>
        /// Повідомлення про помилку
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public int StatusCode { get; set; }
    }

    /// <summary>
    /// Дані щоденного моніторингу по фізичним, юриличним особам та об'єктам нерухомого майна які додані на моніторинг
    /// </summary>
    public class ChangeListData
    {
        /// <summary>
        /// Код Едрпоу
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Отримати дані щоденного моніторингу по фізичним, юриличним особам та об'єктам нерухомого майна які додані на моніторинг
        /// </summary>
        public List<GetChangesResponseModel> Data { get; set; }
    }

    /// <summary>
    /// Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
    /// </summary>
    public class Api2AnswerModelRelationData
    {
        public string Edrpou { get; set; }
        /// <summary>
        /// Відповідь по методу 41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetRelationsClass.cs#L137
        /// </summary>
        public List<GetRelationApiModelAnswerData> Data { get; set; }
    }
    /// <summary>
    /// Відповідь по методу 57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
    /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgFinanceClass.cs#L123
    /// </summary>
    public class Api2AnswerModelOrgFinanceData
    {
        public string Code { get; set; }
        public object Data { get; set; }
    }

    /// <summary>
    /// Отримати дані реквізитів для строреня картки
    /// </summary>
    public class Api2AnswerModelGetRequisites
    {
        public string Code { get; set; }

        /// <summary>
        /// Відповідь по методу 66. Отримати дані реквізитів для строреня картки ФОП / ЮО
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetRequisitesClass.cs#L137
        /// </summary>
        public GetRequisitesResponseData Data { get; set; }
    }

    /// <summary>
    /// Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
    /// </summary>
    public class Api2AnswerModelVehicleData
    {
        public string Code { get; set; }
        /// <summary>
        /// Відповідь по методу 32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetRequisitesClass.cs#L137
        /// </summary>
        public List<VehicleOrgApiAnswerModelDataVehicle> Data { get; set; }
    }
    
    /// <summary>
    /// Фільтр для отримання істориї змін 
    /// </summary>
    public class ApiConstructorGetChangesByCodeFilter
    {
        /// <summary>
        /// Дата від
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// Дата до
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// Тип зміни
        /// </summary>
        public int? FieldType { get; set; }
    }

    /// <summary>
    /// Судові рішення
    /// </summary>
    public class CourtDecisionExtendedModelFilterData
    {
        public string Code { get; set; }
        public string ScrollToken { get; set; }
        public long? DecisionsCount { get; set; }
        /// <summary>
        /// 67. Рекізити судового документа
        /// /api/1.0/courtdecision/getdecisionbyid
        /// </summary>
        public List<CourtDecisionElasticModel> Data { get; set; }
    }

    /// <summary>
    /// URL на API для отримання відомостей по нерухомості
    /// </summary>
    public class EstateData
    {
        public string Code { get; set; }
        public string HrefToGetData { get; set; }
    }

    /// <summary>
    /// Фільтр для отримання судових рішень
    /// </summary>
    public class Api2GetCourtDecisionFilterScroll
    {
        /// <summary>
        /// Тип сторони: 0 - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        /// </summary>
        public int? TypeSide { get; set; }
        /// <summary>
        /// Форма судочинства: 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        /// </summary>
        public int? JusticeKindId { get; set; }
        /// <summary>
        /// Тип документа: 1  Вирок, 2 Постанова, 3 Рішення, 4 "Судовий наказ", 5 Ухвала, 6 "Окрема ухвала", 10 "Окрема думка"
        /// </summary>
        public int? JudgmentFormId { get; set; }
        /// <summary>
        /// Дата від
        /// </summary>
        public DateTime? AdjudicationDateFrom { get; set; }
        /// <summary>
        /// Дата до
        /// </summary>
        public DateTime? AdjudicationDateTo { get; set; }
        /// <summary>
        /// Фільтр по НПА
        /// </summary>
        public List<string> Npas { get; set; }
        /// <summary>
        /// Скрол ід для отримання наступрої частини даних
        /// </summary>
        public string ScrollToken { get; set; }
    }
    /// <summary>
    /// Фільтр для отримання вімостей по фізичним особам
    /// </summary>
    public class PersonApiRequest
    {
        /// <summary>
        /// Повний ПІБ
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Призвище (необовязкове якщо вказаний FullName)
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Ім'я (необовязкове якщо вказаний FullName)
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// По батькові (необовязкове)
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// ІПН
        /// </summary>
        public string Ipn { get; set; }
        /// <summary>
        /// Серія номер пасторта (XX123456)
        /// </summary>
        public string Doc { get; set; }
        /// <summary>
        /// Дата народження
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// ПІБ на російській мові
        /// </summary>
        public string RuName { get; set; }
    }
    /// <summary>
    /// Фінансово промислові групи
    /// </summary>
    public class ApiGetFinancialIndustrialGroupModelAnswerData
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Фінансово промислові групи
        /// /api/1.0/organizations/GetFinancialIndustrialGroup
        /// </summary>
        public List<FinancialIndustrialGroupModel> Data { get; set; }
    }

    /// <summary>
    /// ДРОРМ отримання скороченных даних про наявні обтяження на рухоме майно по ІПН / ЄДРПОУ
    /// </summary>
    public class MovableData
    {
        public string Code { get; set; }
        /// <summary>
        /// ДРОРМ отримання скороченных даних про наявні обтяження на рухоме майно по ІПН / ЄДРПОУ
        /// /api/1.0/MovableLoads/getmovableloads
        /// </summary>
        public List<MovableLoadsDatum> MovableDataList { get; set; }
        public DateTime? ActualData { get; set; }
    }

    // 4.
    // api/1.0/organizations/getadvancedorganization
    // 9.
    // api/1.0/organizations/getanalytic
    // 41.
    // api/1.0/organizations/getrelations
    // 37.
    // api/1.0/organizations/getorglicensesinfo
    // 57.
    // api/1.0/organizations/GetOrgFinance
    // 66. 
    // api/1.0/organizations/GetRequisites
    // 32.
    // api/1.0/organizations/getorgvehicle
    // 39.
    // api/1.0/organizations/getorgshareholders
    // 70.
    // api/1.0/organizations/GetOrgFinanceShort
}
