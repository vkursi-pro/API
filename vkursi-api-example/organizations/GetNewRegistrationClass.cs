using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class GetNewRegistrationClass
    {
        // 15. Запит на отримання списку нових компаній(компаній / ФОП)
        // [POST] api/1.0/organizations/getnewregistration

        public static string GetNewRegistration(string dateReg, string type, int? skip, int? take, bool? IsShortModel, bool? IsReturnAll, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getnewregistration", Method.POST);

            // Example Body: {"DateReg":"29.10.2019","Type":"1","Skip":0,"Take":10,"IsShortModel":true,"IsReturnAll":true};

            GetNewRegistrationBodyModel GetNewRegistrationRequestRow = new GetNewRegistrationBodyModel
            {
                DateReg = dateReg,              // Дата реїстрації компанії
                Type = type,                    // Тип (1 - Юридична особа / 2 - ФОП)
                Skip = skip,                    // К-ть записів які траба пропустити
                Take = take,                    // К-ть записів які траба взяти
                IsShortModel = IsShortModel,    // Коротка або повна модель відповіді
                IsReturnAll = IsShortModel      // Повернуті всі записи або тільки ті які раніше не отримували 
                                                // (IsReturnAll = false - будуть передаватись тільки ті записи які не передавались раніше)
            };

            string body = JsonConvert.SerializeObject(GetNewRegistrationRequestRow); // body = "{\"DateReg\":\"29.10.2019\",\"Type\":\"1\",\"Skip\":0,\"Take\":10,\"IsShortModel\":true,\"IsReturnAll\":true}";

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "Not found")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            List<GetAdvancedOrganizationResponseModel> NewRegistrationFullList = new List<GetAdvancedOrganizationResponseModel>();

            List<OrganizationAdviceFullApiShortModel> NewRegistrationShortList = new List<OrganizationAdviceFullApiShortModel>();

            if (IsShortModel == null || IsShortModel == false)
            {
                NewRegistrationFullList = JsonConvert.DeserializeObject<List<GetAdvancedOrganizationResponseModel>>(responseString);
            }
            else if (IsShortModel == true)
            {
                NewRegistrationShortList = JsonConvert.DeserializeObject<List<OrganizationAdviceFullApiShortModel>>(responseString);
            }

            return responseString;
        }
    }

    public class GetNewRegistrationBodyModel
    {
        public string DateReg { get; set; }
        public string Type { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? IsShortModel { get; set; }
        public bool? IsReturnAll { get; set; }
    }

    public class OrganizationAdviceFullApiShortModel
    {
        public int Id { get; set; }
        public int? State { get; set; }
        public string State_text { get; set; }
        public string Registration_date { get; set; }
        public int Type { get; set; }       //1 - компания; 2 - фоп
        public string Short_name { get; set; }
        public string Full_name { get; set; }
        public string Olf_name { get; set; }
        public string Display_name { get; set; }
        public string Code { get; set; }
        public OrganizationaisPrimaryActivityKind Activity { get; set; }
        public string Email { get; set; }
        public string[] Phones { get; set; }
        public string Location_full { get; set; }
        public OrganizationaisAddress Location_parts { get; set; }
        public OrganizationAdviceFullApiShortDirectorModel Ceo_name { get; set; }
    }
    public class OrganizationAdviceFullApiShortDirectorModel
    {
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
    }
}
