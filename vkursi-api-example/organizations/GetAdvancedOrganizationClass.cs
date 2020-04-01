using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetAdvancedOrganizationClass
    {
        /*

        4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
        [POST] /api/1.0/organizations/getadvancedorganization



         */

        public static GetAdvancedOrganizationResponseModel GetAdvancedOrganization(string code, ref string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetAdvancedOrganizationRequestBodyModel GAORequestBody = new GetAdvancedOrganizationRequestBodyModel
                {
                    Code = code                                             // Код ЄДРПОУ / ІПН
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
                    Console.WriteLine("За вказаным кодом організації не знайдено");
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


    public class GetAdvancedOrganizationRequestBodyModel                    // Модель Body запиту
    {
        public string Code { get; set; }                                    // Код ЄДРПОУ / ІПН
    }

    public class GetAdvancedOrganizationResponseModel                       // Модель відповіді GetAdvancedOrganization
    {
        public OrganizationaisElasticModel Data { get; set; }               // 
        public OrganizationGageModel ExpressScore { get; set; }             // 
        public CourtDecisionAnaliticViewModel CourtAnalytic { get; set; }   // 
        public DateTime? DateRegInn { get; set; }                           // 
        public string Inn { get; set; }                                     // 
        public DateTime? DateCanceledInn { get; set; }                      // 
        public string Koatuu { get; set; }                                  // 
    }

    public class OrganizationaisElasticModel
    {
        public int id { get; set; }
        public int? state { get; set; }
        public string state_text { get; set; }
        public string code { get; set; }
        public OrganizationaisNamesModel names { get; set; }
        public string olf_code { get; set; }
        public string olf_name { get; set; }
        public string founding_document { get; set; }
        public OrganizationaisExecutivePower executive_power { get; set; }
        public string object_name { get; set; }
        public OrganizationaisFounders[] founders { get; set; }
        public OrganizationaisBranches[] branches { get; set; }
        public OrganizationaisAuthorisedCapital authorised_capital { get; set; }
        public string management { get; set; }
        public string managing_paper { get; set; }
        public bool? is_modal_statute { get; set; }
        public OrganizationaisActivityKinds[] activity_kinds { get; set; }
        public OrganizationaisHeads[] heads { get; set; }
        public OrganizationaisAddress address { get; set; }
        public OrganizationaisRegistration registration { get; set; }
        public OrganizationaisBankruptcy bankruptcy { get; set; }
        public OrganizationaisTermination termination { get; set; }
        public OrganizationaisTerminationCancel termination_cancel { get; set; }
        public OrganizationaisAssignees[] assignees { get; set; }
        public OrganizationaisAssignees[] predecessors { get; set; }
        public OrganizationaisRegistrations[] registrations { get; set; }
        public OrganizationaisPrimaryActivityKind primary_activity_kind { get; set; }
        public string prev_registration_end_term { get; set; }
        public OrganizationaisContacts contacts { get; set; }
        public string[] open_enforcements { get; set; }
    }

    public class OrganizationaisNamesModel
    {
        public string name { get; set; }
        public bool? include_olf { get; set; }
        public string display { get; set; }

        [JsonProperty("short")]
        public string shortName { get; set; }

        public string name_en { get; set; }
        public string short_en { get; set; }
    }

    public class OrganizationaisExecutivePower
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class OrganizationaisFounders
    {
        public string name { get; set; }
        public string code { get; set; }
        public OrganizationaisAddress address { get; set; }
        public string last_name { get; set; }
        public string first_middle_name { get; set; }
        public int? role { get; set; }
        public string role_text { get; set; }
        public int? id { get; set; }
        public string url { get; set; }
        public string capital { get; set; }
    }

    public class OrganizationaisAddress
    {
        public string zip { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public OrganizationaisParts parts { get; set; }
    }

    public class OrganizationaisParts
    {
        public string atu { get; set; }
        public string street { get; set; }
        public string house_type { get; set; }
        public string house { get; set; }
        public string building_type { get; set; }
        public string building { get; set; }
        public string num_type { get; set; }
        public string num { get; set; }
    }

    public class OrganizationaisBranches
    {
        public string name { get; set; }
        public string code { get; set; }
        public int? role { get; set; }
        public string role_text { get; set; }
        public int? type { get; set; }
        public string type_text { get; set; }
        public OrganizationaisAddress address { get; set; }
    }

    public class OrganizationaisAuthorisedCapital
    {
        public string value { get; set; }
        public DateTime? date { get; set; }
    }

    public class OrganizationaisActivityKinds
    {
        public string code { get; set; }
        public string name { get; set; }
        public bool? is_primary { get; set; }
    }

    public class OrganizationaisHeads
    {
        public string name { get; set; }
        public string code { get; set; }
        public OrganizationaisAddress address { get; set; }
        public string last_name { get; set; }
        public string first_middle_name { get; set; }
        public int? role { get; set; }
        public string role_text { get; set; }
        public int? id { get; set; }
        public string url { get; set; }
        public DateTime? appointment_date { get; set; }
        public string restriction { get; set; }
    }

    public class OrganizationaisRegistration
    {
        public DateTime? date { get; set; }
        public string record_number { get; set; }
        public DateTime? record_date { get; set; }
        public bool? is_separation { get; set; }
        public bool? is_division { get; set; }
        public bool? is_merge { get; set; }
        public bool? is_transformation { get; set; }
    }

    public class OrganizationaisBankruptcy
    {
        public DateTime? date { get; set; }
        public int? state { get; set; }
        public string state_text { get; set; }
        public string doc_number { get; set; }
        public DateTime? doc_date { get; set; }
        public DateTime? date_judge { get; set; }
        public string court_name { get; set; }
    }

    public class OrganizationaisTermination
    {
        public DateTime? date { get; set; }
        public int? state { get; set; }
        public string state_text { get; set; }
        public string record_number { get; set; }
        public string requirement_end_date { get; set; }
        public string cause { get; set; }
    }

    public class OrganizationaisTerminationCancel
    {
        public DateTime? date { get; set; }
        public string record_number { get; set; }
        public string doc_number { get; set; }
        public DateTime? doc_date { get; set; }
        public DateTime? date_judge { get; set; }
        public string court_name { get; set; }
    }

    public class OrganizationaisAssignees
    {
        public string name { get; set; }
        public string code { get; set; }
        public OrganizationaisAddress address { get; set; }
        public string last_name { get; set; }
        public string first_middle_name { get; set; }
        public int? role { get; set; }
        public string role_text { get; set; }
        public int? id { get; set; }
        public string url { get; set; }
    }

    public class OrganizationaisRegistrations
    {
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime? start_date { get; set; }
        public string start_num { get; set; }
        public DateTime? end_date { get; set; }
        public string end_num { get; set; }
    }

    public class OrganizationaisPrimaryActivityKind
    {
        public string name { get; set; }
        public string code { get; set; }
        public string reg_number { get; set; }
        [JsonProperty("class")]
        public string classProp { get; set; }
    }

    public class OrganizationaisContacts
    {
        public string email { get; set; }
        public string[] tel { get; set; }
        public string fax { get; set; }
        public string web_page { get; set; }
    }


    public class OrganizationGageModel
    {
        public string liquidation { get; set; }
        public string bankruptcy { get; set; }
        public string badRelations { get; set; }
        public string sffiliatedMore { get; set; }
        public string sanctions { get; set; }
        public string introduction { get; set; }
        public string criminal { get; set; }
        public string audits { get; set; }
        public string canceledVat { get; set; }
        public string taxDebt { get; set; }
        public string wageArrears { get; set; }
        public string kved { get; set; }
        public string newDirector { get; set; }
        public string massRegistration { get; set; }
        public string youngCompany { get; set; }
        public string atoRegistered { get; set; }
        public string fictitiousBusiness { get; set; }
        public string headOtherCompanies { get; set; }
        public string notLicense { get; set; }
        public string vatLessThree { get; set; }
        public string sanctionsRelationships { get; set; }
        public string territoriesRelationships { get; set; }
        public string criminalRelationships { get; set; }
        public string update_date { get; set; }
        public string relation_date { get; set; }
    }


    public class CourtDecisionAnaliticViewModel
    {
        public CourtDecisionAggregationModel Aggregations { get; set; }
        public string Edrpo { get; set; }
        public OpenCardAccessState OpenCardAccessState { get; set; }
    }
    public enum OpenCardAccessState
    {
        [Display(Name = "Бесплатный доступ")]     //Не авторизированный пользователь
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
        public long? TotalCount { get; set; }       //Всього справ
        public long? PlaintiffCount { get; set; }   //Позивач
        public long? DefendantCount { get; set; }   //Відповідач
        public long? OtherSideCount { get; set; }   //Інша сторона
        public long? LoserCount { get; set; }       //Програно
        public long? WinCount { get; set; }         //Виграно
        public long? IntendedCount { get; set; }    //Призначено до розгляду
        public long? CaseCount { get; set; }
        public long? InProcess { get; set; }
        public IEnumerable<JusticeKinds> ListJusticeKindses { get; set; }
    }

    public class JusticeKinds
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? Count { get; set; }
    }

}
