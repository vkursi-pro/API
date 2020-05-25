using System;
using System.Collections.Generic;
using System.IO;
using vkursi_api_example.bi;
using vkursi_api_example.changes;
using vkursi_api_example.codeExample;
using vkursi_api_example.courtdecision;
using vkursi_api_example.dictionary;
using vkursi_api_example.estate;
using vkursi_api_example.monitoring;
using vkursi_api_example.movableloads;
using vkursi_api_example.organizations;
using vkursi_api_example.person;
using vkursi_api_example.token;

namespace vkursi_api_example
{
    public class Program
    {
        public static string token = null;

        static void Main()
        {
            // 1. Отримання токена авторизації
            // [POST] /api/1.0/token/authorize

            token = AuthorizeClass.Authorize();

            // 2. Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getorganizations

            GetOrganizationsClass.GetOrganizations("1841404820", ref token); // 40073472 

            // 3. Запит на отримання коротких даних по ФОП за кодом ІПН
            // [POST] /api/1.0/organizations/getfops

            GetFopsClass.GetFops("1841404820", token); // 3334800417

            // 4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
            // [POST] /api/1.0/organizations/getadvancedorganization

            GetAdvancedOrganizationClass.GetAdvancedOrganization("1841404820", ref token); // 00131305

            // 5. Отримання відомостей про наявні об'єкти нерухоммого майна у фізичних та юридичних осіб за кодом ЄДРПОУ або ІПН
            // [GET] /api/1.0/estate/getestatebycode

            GetEstateByCodeClass.GetRealEstateRights("00131305", token);

            // 6. Отримати дані щоденного моніторингу по компаніям які додані на моніторинг (стрічка користувача)
            // [GET] /api/1.0/changes/getchanges

            GetChangesClass.GetChanges("28.10.2019", token);

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

            // 12. Додати контрагентів до списку (до списку vkursi.pro/eventcontrol#/reestr)
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

            GetNewRegistrationClass.GetNewRegistration("29.10.2019", "1", 0, 10, true, true, token);

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

            DateTime dateTime = DateTime.Now;

            GetBiImportLabelsClass.GetBiImportLabels(token);
            // New
            GetBiLabelsClass.GetBiLabels(token);

            // 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам 
            // [POST] /api/1.0/estate/estatecreatetaskapi

            EstateTaskApiClass.EstateCreateTaskApi(token);

            // 20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)
            // [GET] /api/1.0/estate/getestatetasklist

            EstateTaskApiClass.GetEstateTaskList(token);

            // 21. Отримання інформації про виконання формування звіту та запитів до ДРРП, НГО, ДЗК за TaskId
            // [POST] /api/1.0/estate/estategettaskdataapi

            EstateTaskApiClass.EstateGetTaskDataApi(token, "taskId", "7424955100:04:001:0511");

            // 22. ДРОРМ отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/movableLoads/getmovableloads

            GetMovableLoadsClass.GetMovableLoads(token, "36679626", "1841404820");

            // 23. ДРОРМ отримання витяга
            // [POST] /api/1.0/MovableLoads/getpaymovableloads

            GetPayMovableLoadsClass.GetPayMovableLoads(token, 17374040);

            // 24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/Estate/GetEstates

            GetEstatesClass.GetEstates(token, "36679626", null);

            // [POST] /api/1.0/Estate/GetCadastrCoordinates

            // 25. Отримання повного витяга з реєстру нерухомого майна (ДРРП)
            // [POST] /api/1.0/estate/getadvancedrrpreport

            GetAdvancedRrpReportClass.GetAdvancedRrpReport(token, 5001466269723, 68345530);

            // 26. Рекізити судового документа
            // [POST] /api/1.0/courtdecision/getdecisionbyid

            GetDecisionByIdClass.GetDecisionById("88234097", token);

            // 27. Обьем ресурсів доспупних користувачу відповідно до тарифного плану
            // [GET] /api/1.0/token/gettariff

            GetTariffClass.GetTariff(token);

            // 28. Метод АРІ, який віддає історію по компанії з можливістю обрати період.
            // [POST] /api/1.0/changes/getchangesbyCode

            GetChangesByCodeClass.GetChangesByCode(token, "00131305", "20.11.2018", "25.11.2019", null);

            // 29. Отримання інформації по фізичній особі
            // [POST] /api/1.0/person/checkperson

            CheckPersonClass.CheckPerson(token, "ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ", "2301715013");

            // 30. ДРОРМ отримання витягів які були замовлені раніше в сервісі Vkursi
            // [POST] /api/1.0/movableloads/getexistedmovableloads

            // 31. Основні словники сервісу
            // [POST] /api/1.0/dictionary/getdictionary

            GetDictionaryClass.GetDictionary(ref token, 0);

            // 32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
            // [POST] /api/1.0/organizations/getorgvehicle

            GetOrgVehicleClass.GetOrgVehicle(ref token, "00131305");

            // 33. Список виконавчих проваджень по фізичним або юридичним особам за кодом ІПН / ЄДРПОУ
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

            GetRelationsClass.GetRelations(ref token, "00131305", null);

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

            EstatePutOnMonitoringClass.EstatePutOnMonitoring(token, "1260724348000");

            // 46. Змінити період моніторингу об'єкта нерухомості за номером ОНМ (sms rrp)
            // [POST] /api/1.0/estate/estateincreasemonitoringperiod

            EstateInCreaseMonitoringPeriodClass.EstateInCreaseMonitoringPeriod(token, 1260724348000);

            // 47. Видалити об'єкт з мониторингу (sms rrp)
            // [POST] /api/1.0/estate/estateremovefrommonitoring

            EstateRemoveFromMonitoringClass.EstateRemoveFromMonitoring(token, 1260724348000);

            // 48. Отримати зміни по об'єкту шо на мониторингу (можлимо через webhook)
            // [inprogress]

            // 49.Перевірка наявності об'єкта за ОНМ (sms rrp)
            // [POST] /api/1.0/estate/smsrrpselectisrealtyexists

            SmsRrpSelectIsRealtyExistsClass.SmsRrpSelectIsRealtyExists(token, 1260724348000);

            // ДРРП отримання витягів які були замовлені раніше в сервісі Vkursi
            // [inprogress] estate/GetRrp

            // Судові рішення по ФО
            // [inprogress]

            // Перелік статусів відповідей API
        }
    }
}
