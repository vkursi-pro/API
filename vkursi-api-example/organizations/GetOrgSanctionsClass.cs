using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgSanctionsClass
    {
        /*
         
        78. Перевірка контрагента в списках санкції
        [POST] /api/1.0/organizations/GetOrgSanctions
         
        */

        public static GetOrgSanctionsResponseModel GetOrgSanctions(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                //code = "33447980";

                GetOrgSanctionsRequestBodyModel GOSRBody = new GetOrgSanctionsRequestBodyModel
                {
                    Code = new List<string> { code },                       // Код ЄДРПОУ аба ІПН
                    IncludeFounders = true                                  // Розширений пошук санкцій (включає пошук по засновниках та бенефіціарах)
                };

                string body = JsonConvert.SerializeObject(GOSRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgSanctions");
                RestRequest request = new RestRequest(Method.POST);

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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetOrgSanctionsResponseModel GORSanctions = new GetOrgSanctionsResponseModel();

            GORSanctions = JsonConvert.DeserializeObject<GetOrgSanctionsResponseModel>(responseString);

            return GORSanctions;
        }
    }

    public class GetOrgSanctionsRequestBodyModel                                // Модель запиту (Example: {"code":["21560766"]})
    {
        public List<string> Code { get; set; }                                  // Код ЄДРПОУ
        public bool? IncludeFounders { get; set; }                              // Розширений пошук санкцій (включає пошук по засновниках та бенефіціарах)
    }

    public class GetOrgSanctionsResponseModel                                   // Модель на відповідь GetKilkistPracivnukiv
    {
        public string Status { get; set; }                                      // Статус відповіді по API
        public bool IsSuccess { get; set; }                                     // Чи успішний запит
        public List<OrganizationSanctionDetail> Data { get; set; }              // Дані відповіді по санкціях
    }
    public class OrganizationSanctionDetail                                     // Дані відповіді по санкціях
    {
        public DateTime? SanctionStart { get; set; }                            // Дата початку санкцій
        public DateTime? SanctionEnd { get; set; }                              // Дата закійнченн санкцій
        public string SanctionName { get; set; }                                // Назва санкцыйного списку
        public string Code { get; set; }                                        // Код ЄДРПОУ (по якому санкції)
        public bool IsActive { get; set; }                                      // Чи активна санкція
        public int? SanctionId { get; set; }                                    // Ід санкції
        public object Details { get; set; }                                     // Json з додатковими атрибумами саккції

        public List<string> PersonNames { get; set; }
        public HashSet<string> SearchedByNames { get; set; }
        public List<string> OrganizationNames { get; set; }

        [JsonProperty("typeList")]
        public List<int> TypeList { get; set; }

    }
}
