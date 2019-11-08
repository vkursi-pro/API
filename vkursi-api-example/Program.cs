using System;
using System.Collections.Generic;
using vkursi_api_example.changes;
using vkursi_api_example.courtdecision;
using vkursi_api_example.estate;
using vkursi_api_example.monitoring;
using vkursi_api_example.organizations;

namespace vkursi_api_example
{
    public class Program
    {
        public static string token = null;

        static void Main()
        {
            // 1. Получаем токен
            token = AuthorizeClass.Authorize();

            // 2.	Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getorganizations

            List<GetOrganizationsResponseModel> OrganizationsShortList = GetOrganizationsClass.GetOrganizations("40073472", token);

            // 3.	Запит на отримання коротких даних по ФОП за кодом ІПН
            // [POST] /api/1.0/organizations/getfops

            List<GetFopsResponseModel> FopsShortList = GetFopsClass.GetFops("3334800417", token);

            // 4.	Розширений запит. Запит на отримання розширених даних про ЮР / ФІЗ осіб
            // [POST] /api/1.0/organizations/getadvancedorganization

            GetAdvancedOrganizationResponseModel OrganizatioAdvancedRow = GetAdvancedOrganizationClass.GetAdvancedOrganization("00131305", token);

            // 5.	Нерухомість по ФОП або ЮО
            // [GET] /api/1.0/estate/getestatebycode

            GetRealEstateRightsResponseModel RealEstateRightRpw = GetEstateByCodeClass.GetRealEstateRights("00131305", token);

            // 6.	Історія змін по компаніям які додані на моніторинг
            // [GET] /api/1.0/changes/getchanges

            List<GetChangesResponseModel> ChangesResponseList = GetChangesClass.GetChanges("28.10.2019", token);

            // 7.	Отримати перелік списків контрагентів
            // [GET]   /api/1.0/monitoring/getAllReestr

            List<GetAllReestrResponseModel> AllReestrList = GetAllReestrClass.GetAllReestr(token);

            // 8.	Додати новий список контрагентів
            // [POST] /api/1.0/monitoring/addNewReestr

            string reestrId = AddNewReestrClass.AddNewReestr("Назва нового реєстру", token);

            // 9.	Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
            // [POST] /api/1.0/organizations/getanalytic

            GetAnalyticResponseModel GetAnalyticRow = GetAnalyticClass.GetAnalytic("00131305", token);

            // 10.	Запит на отримання даних по судовим рішенням організації
            // [POST] /api/1.0/courtdecision/getdecisions

            GetDecisionsResponseModel DecisionsResponseRow = GetDecisionsClass.GetDecisions("00131305", 100, 1, 2, token);

            // 11.	Запит на отримання контенту судового рішення
            // [POST] /api/1.0/courtdecision/getcontent

            string courtDecisionContent = GetContentClass.GetContent("84583482", token);

            // 12.	Додати контрагентів до списку
            // [POST] /api/1.0/Monitoring/addToControl

            List<AddToControlResponseModel> AddToControlRow = AddToControlClass.AddToControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 13.	Видилити контрагентів зі списку
            // [POST] /api/1.0/Monitoring/removeFromControl

            RemoveFromControlClass.RemoveFromControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 14.	Запит на отримання списку компаній по коду КОАТУУ
            // [POST] api/1.0/organizations/getinfobykoatuu

            List<GetInfoByKoatuuResponseModel> InfoByKoatuuResponseList = GetInfoByKoatuuClass.GetInfoByKoatuu("510900000", "1", token);

            // 15. Запит на отримання списку нових компаній(компаній / ФОП)
            // [POST] api/1.0/organizations/getnewregistrationі

            List<GetAdvancedOrganizationResponseModel> NewRegistrationList = GetNewRegistrationClass.GetNewRegistration("29.10.2019", "1", token);

            // 16. Видалити список контрагентів
            // [DELETE] /api/1.0/monitoring/removeReestr

            RemoveReestrClass.RemoveReestr("1c891112-b022-4a83-ad34-d1f976c60a0b", token);

            // 17.	Перелік статусів відповідей API
        }
    }
}
