using System;
using System.Collections.Generic;
using vkursi_api_example.bi;
using vkursi_api_example.changes;
using vkursi_api_example.courtdecision;
using vkursi_api_example.estate;
using vkursi_api_example.monitoring;
using vkursi_api_example.movableloads;
using vkursi_api_example.organizations;
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

            GetChangesByCodeClass.GetChangesByCode(token, "00131305", "20.11.2018", "25.11.2019", null);

            // 2. Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getorganizations

            List<GetOrganizationsResponseModel> OrganizationsShortList = GetOrganizationsClass.GetOrganizations("1841404820", ref token); // 40073472

            // 3. Запит на отримання коротких даних по ФОП за кодом ІПН
            // [POST] /api/1.0/organizations/getfops

            List<GetFopsResponseModel> FopsShortList = GetFopsClass.GetFops("1841404820", token); // 3334800417

            // 4. Розширений запит. Запит на отримання розширених даних про ЮР / ФІЗ осіб
            // [POST] /api/1.0/organizations/getadvancedorganization

            GetAdvancedOrganizationResponseModel OrganizatioAdvancedRow = GetAdvancedOrganizationClass.GetAdvancedOrganization("1841404820", ref token); // 00131305

            // 5. Нерухомість по ФОП або ЮО
            // [GET] /api/1.0/estate/getestatebycodenew

            GetRealEstateRightsResponseModel RealEstateRightRpw = GetEstateByCodeClass.GetRealEstateRights("00131305", token);

            // 6. Історія змін по компаніям які додані на моніторинг
            // [GET] /api/1.0/changes/getchanges

            List<GetChangesResponseModel> ChangesResponseList = GetChangesClass.GetChanges("28.10.2019", token);

            // 7. Отримати перелік списків контрагентів
            // [GET] /api/1.0/monitoring/getAllReestr

            List<GetAllReestrResponseModel> AllReestrList = GetAllReestrClass.GetAllReestr(token);

            // 8. Додати новий список контрагентів
            // [POST] /api/1.0/monitoring/addNewReestr

            string reestrId = AddNewReestrClass.AddNewReestr("Назва нового реєстру", token);

            // 9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getanalytic

            GetAnalyticResponseModel GetAnalyticRow = GetAnalyticClass.GetAnalytic("00131305", token);

            // 10. Запит на отримання даних по судовим рішенням організації
            // [POST] /api/1.0/courtdecision/getdecisions

            GetDecisionsResponseModel DecisionsResponseRow = GetDecisionsClass.GetDecisions("00131305", 100, 1, 2, new List<string>() { "F545D851-6015-455D-BFE7-01201B629774" }, token);

            // 11. Запит на отримання контенту судового рішення
            // [POST] /api/1.0/courtdecision/getcontent

            string courtDecisionContent = GetContentClass.GetContent("84583482", token);

            // 12. Додати контрагентів до списку
            // [POST] /api/1.0/Monitoring/addToControl

            List<AddToControlResponseModel> AddToControlRow = AddToControlClass.AddToControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 13. Видилити контрагентів зі списку
            // [POST] /api/1.0/Monitoring/removeFromControl

            RemoveFromControlClass.RemoveFromControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 14. Запит на отримання списку компаній по коду КОАТУУ
            // [POST] /api/1.0/organizations/getinfobykoatuu

            List<GetInfoByKoatuuResponseModel> InfoByKoatuuResponseList = GetInfoByKoatuuClass.GetInfoByKoatuu("510900000", "1", token);

            // 15. Запит на отримання списку нових компаній(компаній / ФОП)
            // [POST] /api/1.0/organizations/getnewregistrationі

            string NewRegistrationList = GetNewRegistrationClass.GetNewRegistration("29.10.2019", "1", 0, 10, true, true, token);

            // 16. Видалити список контрагентів
            // [DELETE] /api/1.0/monitoring/removeReestr

            RemoveReestrClass.RemoveReestr("1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 17. Отримати перелік компаний які відібрані по BI
            // [POST] /api/1.0/bi/getbidata

            List<GetBiDataResponseModel> GetBiDataList = GetBiDataClass.GetBiData(null, 1000, token);

            // 18. Отримати перелік Label доступних по BI
            // [GET] /api/1.0/bi/getbiimportlabels

            List<string> GetBiImportLabelsList = GetBiImportLabelsClass.GetBiImportLabels(token);

            // 19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам 
            // [POST] /api/1.0/estate/estatecreatetaskapi

            EstateTaskApiClass.EstateCreateTaskApi(token);

            // 20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)
            // [GET] /api/1.0/estate/getestatetasklist

            EstateTaskApiClass.GetEstateTaskList(token);

            // 21. Отримання інформації про виконання формування звіту та запитів до ДРРП, НГО, ДЗК за TaskId
            // [POST] /api/1.0/estate/estategettaskdataapi

            EstateTaskApiClass.EstateGetTaskDataApi(token, "taskId");

            // 22. ДРОРМ отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/movableLoads/getmovableloads

            GetMovableLoadsClass.GetMovableLoads(token, "36679626", "1841404820");

            // 23. ДРОРМ отримання витяга
            // [POST] /api/1.0/MovableLoads/getpaymovableloads

            GetPayMovableLoadsClass.GetPayMovableLoads(token, 17374040);

            // 24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ
            // [POST] /api/1.0/Estate/GetEstates

            GetEstatesClass.GetEstates(token, "36679626", null);

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

            // Перелік статусів відповідей API
        }
    }
}
