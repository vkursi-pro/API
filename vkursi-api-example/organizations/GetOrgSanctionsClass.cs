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
                    Code = new List<string> { code },                       // Код ЄДРПОУ або ІПН
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
    /// <summary>
    /// Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetOrgSanctionsRequestBodyModel                                // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                  // 
        /// <summary>
        /// Розширений пошук санкцій (включає пошук по засновниках та бенефіціарах)
        /// </summary>
        public bool? IncludeFounders { get; set; }                              // 
    }
    /// <summary>
    /// Модель на відповідь GetKilkistPracivnukiv
    /// </summary>
    public class GetOrgSanctionsResponseModel                                   // 
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }                                     // 
        /// <summary>
        /// Дані відповіді по санкціях
        /// </summary>
        public List<OrganizationSanctionDetail> Data { get; set; }              // 
    }/// <summary>
     /// Дані відповіді по санкціях
     /// </summary>
    public class OrganizationSanctionDetail                                     // 
    {/// <summary>
     /// Дата початку санкцій
     /// </summary>
        public DateTime? SanctionStart { get; set; }                            // 
        /// <summary>
        /// Дата закійнченн санкцій
        /// </summary>
        public DateTime? SanctionEnd { get; set; }                              // 
        /// <summary>
        /// Назва санкцыйного списку
        /// </summary>
        public string SanctionName { get; set; }                                // 
        /// <summary>
        /// Код ЄДРПОУ (по якому санкції)
        /// </summary>
        public string Code { get; set; }                                        // 
        /// <summary>
        /// Чи активна санкція
        /// </summary>
        public bool IsActive { get; set; }                                      // 
        /// <summary>
        /// Ід санкції
        /// </summary>
        public int? SanctionId { get; set; }                                    // 
        /// <summary>
        /// Json з додатковими атрибумами саккції
        /// </summary>
        public object Details { get; set; }                                     // 

        public List<string> PersonNames { get; set; }
        public HashSet<string> SearchedByNames { get; set; }
        public List<string> OrganizationNames { get; set; }

        [JsonProperty("typeList")]
        public List<int> TypeList { get; set; }

    }
}
