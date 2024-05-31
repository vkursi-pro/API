using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using vkursi_api_example._2._0;
using vkursi_api_example.bi;
using vkursi_api_example.changes;
using vkursi_api_example.codeExample;
using vkursi_api_example.courtdecision;
using vkursi_api_example.dictionary;
using vkursi_api_example.enforcement;
using vkursi_api_example.estate;
using vkursi_api_example.monitoring;
using vkursi_api_example.movableloads;
using vkursi_api_example.organizations;
using vkursi_api_example.organizations.Bankruptcy;
using vkursi_api_example.organizations.FinanceKvartal;
using vkursi_api_example.organizations.GetNaisOrganizationInfoWithEcp;
using vkursi_api_example.person;
using vkursi_api_example.podatkova;
using vkursi_api_example.token;
using static vkursi_api_example.organizations.GetOrgFinanceKvartalOriginalClass;

namespace vkursi_api_example
{
    public class Program
    {
        public static string token = null;

        static void Main()
        {

            
            // 1. Отримання токена авторизації
            // [POST] /api/1.0/token/authorize

            AuthorizeClass _authorize = new AuthorizeClass();
            token = _authorize.Authorize();

            GetRegistrationStatusAndGeolocationClass.GetRegistrationStatusAndGeolocation(ref token, "30325480");

            // 2. Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getorganizations

            GetOrganizationsClass.GetOrganizations("00131305", ref token);

            // 3. Запит на отримання коротких даних по ФОП за кодом ІПН
            // [POST] /api/1.0/organizations/getfops

            GetFopsClass.GetFops(ref token, "3292516420"); // 3334800417

            // 4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
            // [POST] /api/1.0/organizations/getadvancedorganization

            GetAdvancedOrganizationClass.GetAdvancedOrganization("25412361", ref token); // 00131305

            // 5. Отримання відомостей про наявні об'єкти нерухоммого майна у фізичних та юридичних осіб за кодом ЄДРПОУ або ІПН
            // [GET] /api/1.0/estate/getestatebycode

            GetEstateByCodeClass.GetRealEstateRights("00131305", token);

            // 6. Отримати дані щоденного моніторингу по компаніям які додані на моніторинг (стрічка користувача)
            // [GET] /api/1.0/changes/getchanges

            GetChangesClass.GetChanges(date: "28.10.2019", token: token, null, false);

            // 7. Отримати перелік списків (які користувач створив на vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок
            // [GET] /api/1.0/monitoring/getAllReestr

            GetAllReestrClass.GetAllReestr(token);

            // 8. Додати новий список контрагентів (список також можна створиты з інтерфейсу на сторінці vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок
            // [POST] /api/1.0/monitoring/addNewReestr

            AddNewReestrClass.AddNewReestr("Назва нового реєстру", token);

            // 9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getanalytic

            GetAnalyticClass.GetAnalytic("00131305", token);

            // 10. Запит на отримання переліку судових документів організації за критеріями (контент та параметри документа можна отримати в методі /api/1.0/courtdecision/getdecisionbyid)
            // [POST] /api/1.0/courtdecision/getdecisions

            GetDecisionsClass.GetDecisions("00131305", 0, 1, 2, new List<string>() { "F545D851-6015-455D-BFE7-01201B629774" }, token);

            // 11. Запит на отримання контенту судового рішення за id документа (id документа можна отримати в api/1.0/courtdecision/getdecisions)
            // [POST] /api/1.0/courtdecision/getcontent

            GetContentClass.GetContent("84583482", token);

            // 12. Додати в "Мої списки" юридичну особу, фізичну особу, фізичну особу підприємця або КОАТУУ (до списку vkursi.pro/eventcontrol#/reestr)
            // [POST] /api/1.0/Monitoring/addToControl

            AddToControlClass.AddToControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 13. Видалити контрагентів зі списку 
            // [POST] /api/1.0/Monitoring/removeFromControl

            RemoveFromControlClass.RemoveFromControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 14. Отримання переліку кодів ЄДРПОУ або Id фізичних або юридичних осіб які знаходятся за певним КОАТУУ
            // [POST] /api/1.0/organizations/getinfobykoatuu

            GetInfoByKoatuuClass.GetInfoByKoatuu("510900000", "1", token);

            // 15. Новий бізнес. Запит на отримання списку новозареєстрованих фізичних та юридичних осіб
            // [POST] /api/1.0/organizations/getnewregistration

            GetNewRegistrationClass.GetNewRegistration(ref token, "08.09.2019", "1", 0, 10, false, true);

            // 16. Видалити список контрагентів
            // [DELETE] /api/1.0/monitoring/removeReestr

            RemoveReestrClass.RemoveReestr("1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 17. Отримати перелік компаний які відібрані в модулі BI
            // [POST] /api/1.0/bi/getbidata

            GetBiDataClass.GetBiData(null, 1000, token);
            // New
            GetDataBiInfoClass.GetDataBiInfo("1c891112-b022-4a83-ad34-d1f976c60a0b", 1000, DateTime.Parse("2019-11-28 19:00:52.059"), token);
            // New
            GetDataBiChangeInfoClass.GetDataBiChangeInfo(DateTime.Parse("2019-11-28 19:00:52.059"), "1c891112-b022-4a83-ad34-d1f976c60a0b", false, 100, token);
            // New
            GetDataBiOrganizationInfoClass.GetDataBiOrganizationInfo(new List<string> { "1c891112-b022-4a83-ad34-d1f976c60a0b" }, new List<string> { "00131305" }, token);

            // 18. Отримати перелік Label доступних в модулі BI
            // [GET] /api/1.0/bi/getbiimportlabels

            GetBiImportLabelsClass.GetBiImportLabels(token);
            // New
            GetBiLabelsClass.GetBiLabels(token);

            // 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам (додавання в чергу) та додавання об'єктів до моніторингу СМС РРП
            // [POST] /api/1.0/estate/estatecreatetaskapi

            EstateCreateTaskApiClass.EstateCreateTaskApi(token);

            // 20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)
            // [GET] /api/1.0/estate/getestatetasklist

            EstateTaskApiClass.GetEstateTaskList(token);


            // 21. Отримання інформації про виконання формування звіту та запитів до ДРРП, НГО, ДЗК за TaskId
            // [POST] /api/1.0/estate/estategettaskdataapi

            EstateTaskApiClass.EstateGetTaskDataApi(token, "taskId", "7424955100:04:001:0511");

            // 22. ДРОРМ отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/movableLoads/getmovableloads

            GetMovableLoadsClass.GetMovableLoads(ref token, "30839560", "3118708120");

            // 23. ДРОРМ отримання витяга
            // [POST] /api/1.0/MovableLoads/getpaymovableloads

            GetPayMovableLoadsClass.GetPayMovableLoads(token, 17374040);

            // 24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/Estate/GetEstates

            GetEstatesClass.GetEstates(token, "21326462", null);

            // 25. Отримання повного витяга з реєстру нерухомого майна (ДРРП)
            // [POST] /api/1.0/estate/getadvancedrrpreport

            GetAdvancedRrpReportClass.GetAdvancedRrpReport(token, 5001466269723, 68345530);

            // 26. Рекізити судового документа
            // [POST] /api/1.0/courtdecision/getdecisionbyid

            GetDecisionByIdClass.GetDecisionById(88234097, token);

            // 27. Обьем ресурсів доспупних користувачу відповідно до тарифного плану
            // [GET] /api/1.0/token/gettariff

            GetTariffClass.GetTariff(token);

            // 28. Метод АРІ, який віддає історію по компанії з можливістю обрати період.
            // [POST] /api/1.0/changes/getchangesbyCode

            GetChangesByCodeClass.GetChangesByCode(token, "00131305", "20.11.2018", "25.11.2019", null);

            // 29. Отримання інформації по фізичній особі
            // [POST] /api/1.0/person/checkperson

            CheckPersonClass.CheckPerson(token, JsonConvert.DeserializeObject<CheckPersonRequestBodyModel>("{\"Id\":null, \"FullName\":\"ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ\",\"FirstName\":null,\"SecondName\":null,\"LastName\":null, \"Ipn\":\"2301715013\",\"Doc\":\"BC481139\",\"Birthday\":null, \"RuName\":null}"));

            // 30. ДРОРМ отримання витягів які були замовлені раніше в сервісі Vkursi
            // [POST] /api/1.0/movableloads/getexistedmovableloads

            // 31. Основні словники сервісу
            // [POST] /api/1.0/dictionary/getdictionary

            GetDictionaryClass.GetDictionary(ref token, 0);

            // 32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
            // [POST] /api/1.0/organizations/getorgvehicle

            GetOrgVehicleClass.GetOrgVehicle(ref token, "09807750");

            // 33. Список виконавчих проваджень по юридичним особам за кодом ЄДРПОУ (55. Список виконавчих проваджень по фізичним особам за кодом ІПН)
            // [POST] /api/1.0/organizations/getorgenforcements

            GetOrgEnforcementsClass.GetOrgEnforcements(ref token, "00131305");

            // 34. Загальна статистики по Edata (по компанії)
            // [POST] /api/1.0/organizations/getorgpubliicfunds

            GetOrgPubliicFundsClass.GetOrgPubliicFunds(ref token, "00131305");

            // 35. Фінансові ризики
            // [POST] /api/1.0/organizations/getorgFinancialRisks

            GetOrgFinancialRisksClass.GetOrgFinancialRisks(ref token, "00131305");

            // 36. Перелік декларантів повязаних з компаніями
            // [POST] /api/1.0/organizations/getdeclarationsinfo

            GetDeclarationsInfoClass.GetDeclarationsInfo(ref token, "00131305");

            // 37. Перелік ліцензій, та дозволів
            // [POST] /api/1.0/organizations/getorglicensesinfo

            GetOrgLicensesInfoClass.GetOrgLicensesInfo(ref token, "00131305");

            // 38. Відомості про інтелектуальну власність (патенти, торгові марки, корисні моделі) які повязані по ПІБ з бенеціціарами підприємства
            // [POST] /api/1.0/organizations/getorgintellectualproperty

            GetOrgIntellectualPropertyClass.GetOrgIntellectualProperty(ref token, "00131305");

            // 39. Відомості про власників пакетів акцій (від 5%)
            // [POST] /api/1.0/organizations/getorgshareholders

            GetOrgShareholdersClass.GetOrgShareholders(token, "00131305");

            // 40. Частка державних коштів в доході
            // [POST] /api/1.0/organizations/getorgstatefundsstatistic

            GetOrgStateFundsStatisticClass.GetOrgStateFundsStatistic(token, "00131305");

            // 41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
            // [POST] /api/1.0/organizations/getrelations

            GetRelationsClass.GetRelations(ref token, "42556505", null);

            // 42. Запит на отримання геопросторових даних ПККУ
            // [POST] /api/1.0/Estate/GetCadastrCoordinates

            GetCadastrCoordinatesClass.GetCadastrCoordinates(token, "0521685603:01:004:0001", "geojson");

            // 43. Загальна характеристика по тендерам
            // [POST] /api/1.0/organizations/getorgtenderanalytic

            GetOrgTenderAnalyticClass.GetOrgTenderAnalytic(token, "00131305");

            // 44. Офіційні повідомлення (ЄДР, SMIDA, Банкрутство)
            // [POST] /api/1.0/organizations/getofficialnotices

            GetOfficialNoticesClass.GetOfficialNotices(token, "00131305");

            // 45. Додати об'єкт до моніторингу нерухомості за номером ОНМ (sms rrp) 
            // /api/1.0/estate/estateputonmonitoring

            EstatePutOnMonitoringClass.EstatePutOnMonitoring(token, "31.10.2020", new List<long> { 7248532000 });

            // 46. Змінити період моніторингу об'єкта нерухомості за номером ОНМ (sms rrp)
            // [POST] /api/1.0/estate/estateincreasemonitoringperiod

            EstateInCreaseMonitoringPeriodClass.EstateInCreaseMonitoringPeriod(token, 7248532000);

            // 47. Видалити об'єкт з мониторингу (sms rrp)
            // [POST] /api/1.0/estate/estateremovefrommonitoring

            EstateRemoveFromMonitoringClass.EstateRemoveFromMonitoring(token, 7248532000);

            // 48. Отримати зміни по об'єкту шо на мониторингу (можлимо через webhook)
            // [inprogress]

            // 49.Перевірка наявності об'єкта за ОНМ (sms rrp)
            // [POST] /api/1.0/estate/smsrrpselectisrealtyexists

            SmsRrpSelectIsRealtyExistsClass.SmsRrpSelectIsRealtyExists(token, 7248532000);

            // 50. Отримати перелік обєктів ОНМ які встановлено на моніторинг нерухомого майна (SMS RRP)
            // [GET] /api/1.0/estate/estateCurrentOnMonitoring

            EstateCurrentOnMonitoringClass.EstateCurrentOnMonitoring(token);

            // 51. Стан розгляду судових справ 
            // [POST] /api/1.0/CourtDecision/getStanRozgliaduSprav

            GetStanRozgliaduSpravClass.GetStanRozgliaduSprav(ref token);

            // 52. Оригінальний метод пошуку нерухомості Nais (короткі дані) 
            // [POST] /api/1.0/Estate/GetEstatesAdvancedSearch

            GetEstatesAdvancedSearchClass.GetEstatesAdvancedSearch(token);

            // 53. Фінансовий моніторинг пов'язаних осіб частина 1. Створення задачі
            // [POST] /api/1.0/Organizations/SetTaskCompanyDeclarationsAndCourts

            SetTaskCompanyDeclarationsAndCourtsClass.SetTaskCompanyDeclarationsAndCourts(ref token, "00131305");

            // 54. Фінансовий моніторинг пов'язаних осіб частина 2. Отримуємо результат виконання задачі
            // [POST] /api/1.0/Organizations/GetTaskCompanyDeclarationsAndCourts

            GetTaskCompanyDeclarationsAndCourtsClass.GetTaskCompanyDeclarationsAndCourts(ref token, Guid.Parse("691e940c-b61e-4feb-ad1f-fa22c365633f"));

            // 55. Список виконавчих проваджень по фізичним особам за кодом ІПН
            // [POST] /api/1.0/person/GetPersonEnforcements

            GetPersonEnforcementsClass.GetPersonEnforcements(ref token, "2951907234", "ЗАЙЧЕНКО", "МАКСИМ", "ВОЛОДИМИРОВИЧ");

            // 56. Отримання статуту підприємства
            // api/1.0/organizations/GetStatutnuyFileUrl

            GetStatutnuyFileUrlClass.GetStatutnuyFileUrl(ref token, "5159752801");

            // 57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
            // [POST] api/1.0/organizations/GetOrgFinance

            GetOrgFinanceClass.GetOrgFinance(ref token, "01982608");

            // 58. Оригинальний метод Nais, на отримання скороченних данних по коду ІПН / ЄДРПОУ 
            // [POST] /api/1.0/organizations/freenais

            FreeNaisClass.FreeNais(ref token, "00131305", true, "100");

            // 59. Оригинальний метод Nais, на отримання повних данних по ЮО або ФОП за кодом NaisId (який ми отримуємо з [POST] /api/1.0/organizations/freenais)
            // [GET] /api/1.0/organizations/paynais

            //PayNaisClass.PayNais(ref token, "10656661", "41462280", false);

            // 59.1 Оригінальний метод Nais, на отримання повних даних (з міткою ЕЦП) за кодом NaisId який ми отримуємо з [POST] /api/1.0/organizations/freenais
            // organizations/payNaisSign

            PayNaisSignClass.PayNaisSign(ref token, "10656661", "41462280", false);

            // 60. Отримання відомостей по Експрес оцінку ризиків у ЮО, ФОП та ФО за ПІБ та кодом ІПН / ЄДРПОУ 
            // [POST] /api/1.0/organizations/getExpressScore

            GetExpressScoreClass.GetExpressScore(ref token, 1, "37199162");

            // 61. Редагування відомостей вагу ризиків в Експрес оцінці
            // [POST] /api/1.0/organizations/EditExpressScoreWeight?type=1

            EditExpressScoreWeightClass.EditExpressScoreWeight(ref token, "[{\"borgZarPlati\":[{\"indicatorValue\":\">=200000\",\"weight\":1},{\"indicatorValue\":\">100000#=<200000\",\"weight\":2},{\"indicatorValue\":\"=<100000\",\"weight\":3}],\"vidkrytiVp\":[{\"indicatorValue\":\">=5\",\"weight\":1},{\"indicatorValue\":\">1#<5\",\"weight\":2},{\"indicatorValue\":\"=0\",\"weight\":3}],}]");

            // 62. Отримання відомостей про вагу ризиків в Експрес оцінці
            // [POST] /api/1.0/organizations/GetExpressScoreWeight?type=1

            GetExpressScoreWeightClass.GetExpressScoreWeight(ref token);

            // 63. Структура власності компанії
            // [POST] /api/1.0/organizations/GetOwnershipStructure

            GetOwnershipStructureClass.GetOwnershipStructure(ref token, "31077508");

            // 64. Перелік об'єктів в списках
            // [POST] /api/1.0/monitoring/getContent

            GetContentMonitoringClass.GetContent(ref token, "31077508" ); // string reestrId

            // 65. Отримати скорочені дані ДЗК за списком кадастрових номерів
            // [POST] /api/1.0/estate/GetPKKUinfo

            GetPKKUinfoClass.GetPKKUinfo(ref token, new List<string> { "5321386400:00:042:0028" });

            // 66. Отримати дані реквізитів для строреня картки ФОП / ЮО
            // [POST] /api/1.0/organizations/GetRequisites

            GetRequisitesClass.GetRequisites(ref token, "41462280");

            // 67. Запит на отримання повних реквізитів та контенту судових документів організації за критеріями
            // [POST] /api/1.0/courtdecision/getdecisionsbyfilter

            GetDecisionsByFilterClass.GetDecisionsByFilter("00131305", 1, 2, new List<string>() { "F545D851-6015-455D-BFE7-01201B629774" }, token);

            // 68. Анкета
            // [POST] /api/1.0/organizations/GetAnketa

            GetAnketaClass.GetAnketa(ref token, "41462280");

            // 69. API 2.0 Конструктор API
            // [POST] /api/2.0/ApiConstructor

            ApiConstructorClass.ApiConstructor(ref token, "25412361",  new HashSet<int>{ 4, 9, 41, 37, 66, 32, 39, 70, 71 }); // 57

            // 70. Скорочені основні фінансові показники діяльності підприємства 
            // [POST] /api/1.0/organizations/GetOrgFinanceShort

            GetOrgFinanceShortClass.GetOrgFinanceShort(ref token, "41462280", new List<int> { 2020, 2019 });

            // 71. Фінансово промислові групи
            // [POST] /api/1.0/organizations/GetFinancialIndustrialGroup

            GetFinancialIndustrialGroupClass.GetFinancialIndustrialGroup(ref token, "41462280");

            // 72. API з історії зміни даних в ЄДР
            // [POST] /api/1.0/organizations/getactivityorghistory

            GetActivityOrgHistoryClass.GetActivityOrgHistory(ref token, "44623955");

            // 73. Відомості про субєктів господарювання які стоять на обліку в ДФС
            // [POST] /api/1.0/podatkova/cabinettaxregistration

            CabinetTaxRegistrationClass.CabinetTaxRegistration(ref token, "41462280");

            // 74.Стан ФОПа та відомості про ЄП
            // [POST] /api/1.0/podatkova/cabinettaxedpodep

            CabinetTaxEdpodEpClass.CabinetTaxEdpodEp(ref token, "3334800417", true);

            // 75. API 2.0 Zemli конструктор API
            // [POST] /api/2.0/ApiConstructorZemli

            ApiConstructorClass.ApiConstructor(ref token, "25412361", new HashSet<int> { 4, 9, 41, 37, 66, 32, 39, 70, 71 }); // 57

            // 76. Перевірка чи знаходиться аблеса компанії/ФОП в зоні АТО (доступно в конструкторі API №76)
            // [POST] /api/1.0/organizations/CheckIfOrganizationsInAto

            CheckIfOrganizationsInAtoClass.CheckIfOrganizationsInAto(ref token, "25412361");

            // 77. Перевірка ФОП/не ФОП, наявність ліцензій адвокат/нотаріус, наявність обтяжень ДРОРМ (доступно в конструкторі API №77)
            // [POST] /api/1.0/person/CheckFopStatus

            CheckFopStatusClass.CheckFopStatus(ref token, "00131305", true);

            // 78. Перевірка контрагента в списках санкції (доступно в конструкторі API №78)
            // [POST] /api/1.0/organizations/GetOrgSanctions

            GetOrgSanctionsClass.GetOrgSanctions(ref token, "00131305");

            // 79. Реєстр платників ПДВ (доступно в конструкторі API №79)
            // [POST] /api/1.0/organizations/GetOrgPdv

            GetOrgPdvClass.GetOrgPdv(ref token, "00131305");

            // 80. Анульована реєстрація платником ПДВ (доступно в конструкторі API №80)
            // [POST] /api/1.0/organizations/GetOrgAnulPdv

            GetOrgAnulPdvClass.GetOrgAnulPdv(ref token, "00131305");

            // 81. Реєстр платників Єдиного податку (доступно в конструкторі API №81)
            // [POST] /api/1.0/organizations/GetYedynyyPodatok

            GetYedynyyPodatokClass.GetYedynyyPodatok(ref token, "00131305");

            // 82. Реєстр ДФС “Дізнайся більше про свого бізнес-партнера” (доступно в конструкторі API №82)
            // [POST] /api/1.0/organizations/GetDfsBussinesPartnerData

            GetDfsBussinesPartnerDataClass.GetDfsBussinesPartnerData(ref token, "00131305");

            // 83. Штатна чисельність працівників (доступно в конструкторі API №83)
            // [POST] /api/1.0/organizations/GetKilkistPracivnukiv

            GetKilkistPracivnukivClass.GetKilkistPracivnukiv(ref token, "00131305");

            // 84. Реєстр ДФС “Стан розрахунків з бюджетом” (доступно в конструкторі API №84)
            // [POST] /api/1.0/organizations/GetOrganizationDbDebtorsDfs

            GetOrganizationDbDebtorsDfsClass.GetOrganizationDbDebtorsDfs(ref token, "00131305");

            // 85. Єдиний реєстр боржників (доступно в конструкторі API №85)
            // [POST] /api/1.0/organizations/GetOpenEnforcements

            GetOpenEnforcementsClass.GetOpenEnforcements(ref token, "00131305");

            // 86. Відомості про банкрутство ВГСУ
            // [POST] /api/1.0/organizations/getBankruptcyByCode

            GetBankruptcyByCodeClass.GetBankruptcyByCode(ref token, "00131305");

            // 87. Єдиний державний реєстр юридичних осіб, фізичних осіб-підприємців та громадських формувань
            // [POST] /api/1.0/organizations/getadvancedorganizationOnlyNaisData

            GetAdvancedorganizationOnlyNaisDataClass.GetAdvancedOrganizationOnlyNaisData(ref token, "00131305");

            // 88. Список справм призначених до розгляду
            // [POST] /api/1.0/courtDecision/getCourtAssigment

            GetCourtAssigmentClass.GetCourtAssigment(ref token, "00222166");

            // 89. Отримання відомостей про наявних в компанії засновників / бенефіціарів / власників пакетів акцій пов'язаних з росією або білорусією
            // [POST] /api/1.0/organizations/getRussianFoundersAndBeneficiars

            GetRussianFoundersAndBeneficiarsClass.GetRussianFoundersAndBeneficiars(ref token, "00222166");

            // 90. Перевірка ПЕП
            // [POST] /api/1.0/person/CheckPep

            CheckPepClass.CheckPep(ref token, "ОЛІЙНИК ТЕТЯНА АНАТОЛІЇВНА");

            // 91. Отримання переліку санкцій по ФО
            // [POST] /api/1.0/person/GetPersonSanctions

            GetPersonSanctionsClass.GetPersonSanctions(ref token, "КОРОТКИЙ АЛЕКСАНДР ВЛАДИМИРОВИЧ");

            // 92. Перевірка втрачених документів
            // [POST] /api/1.0/person/getLostDocuments

            GetLostDocumentsClass.GetLostDocuments(ref token, "AA100110");

            // 150. Відомості (витяг) з ЄДР з електронною печаткою (КЕП) Державного підприємства “НАІС”
            // [POST] /api/1.0/organizations/GetNaisOrganizationInfoWithEcp
            
            GetNaisOrganizationInfoWithEcpClass.GetNaisOrganizationInfoWithEcp(ref token, "00131305");

            // 151. Чи знаходиться підприємство на окупованій території
            // [POST] /api/1.0/organizations/CheckOcupLocation

            CheckOcupLocationClass.CheckOcupLocation(ref token, "00131305");

            // 153. Довідник ДПС для подання повідомлень про відкриття/закриття рахунків платників податків у банках та інших фінансових установах до контролюючих органів
            // [POST] /api/1.0/organizations/CompanyDpsInfo

            CompanyDpsInfoClass.CompanyDpsInfo(ref token, "00131305");

            // 154.

            // 155. Aрі історії реєстраційних дій
            // [POST] /api/1.0/organizations/GetRegistrationActionsHistory

            GetRegistrationActionsHistoryClass.GetRegistrationActionsHistory(ref token, "00131305");

            // 156. Отримання статутних документів
            // [POST] /api/1.0/organizations/GetFoundingDocuments

            GetFoundingDocumentsClass.GetFoundingDocuments(ref token, "00131305");

            // 157. Отримання відомостей про виконавчі провадження з ЕЦП
            // [POST] /api/1.0/enforcement/GetEnforcementsWithEcp

            GetEnforcementsWithEcpClass.GetEnforcementsWithEcp(ref token, "00131305");

            // 162.Відомості про банкрутство ВГСУ по даті
            // [POST] /api/1.0/organizations/getBankruptcyByDate
            GetBankruptcyByDateClass.GetBankruptcyByDate(ref token, DateTime.Parse("2023-04-14T00:00:00"));

            // 163.Аналіз фінансових показників підприємства за кодом ЄДРПОУта обраним типом(перший квартал, півріччя, дев'ять місяців, річна)
            //[POST] api / 1.0 / organizations / GetOrgFinanceKvartal

            GetOrgFinanceKvartalClass.GetOrgFinanceKvartal(ref token, "00131512", 1);

            //164.Отримання відповіді з файлом zip наповненим xml та pdf файламифінансової звітності підприємств
            //[POST] api / 1.0 / organizations / GetOrgFinanceKvartal

            GetOrgFinanceFilesClass.GetOrgFinanceFiles(ref token, "00131050", 2024, 3);

            //165. Отримання відповіді з даними по фінансовій звітності юридичної особи за конкретний рік, та конкретний період
            //[POST] api / 1.0 / organizations / GetOrgFinanceOriginalData
            GetOrgFinanceKvartalOriginalClass.GetOrgFinanceKvartalOriginal(ref token, "00131050", 2024, 3);

            GetRegistrationStatusAndGeolocationClass.GetRegistrationStatusAndGeolocation(ref token, "00131050");

            // Перелік статусів відповідей API

        }
    }
}
