using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrganizationDbDebtorsDfsClass
    {
        /// <summary>
        /// 84. Реєстр ДФС “Стан розрахунків з бюджетом”
        /// [POST] /api/organizations/GetOrganizationDbDebtorsDfs
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        public static GetOrganizationDbDebtorsDfsResponseModel GetOrganizationDbDebtorsDfs(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrganizationDbDebtorsDfsRequestBodyModel GODDDRBody = new GetOrganizationDbDebtorsDfsRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(GODDDRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/organizations/GetOrganizationDbDebtorsDfs");
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

            GetOrganizationDbDebtorsDfsResponseModel GODDDResponse = new GetOrganizationDbDebtorsDfsResponseModel();

            GODDDResponse = JsonConvert.DeserializeObject<GetOrganizationDbDebtorsDfsResponseModel>(responseString);

            return GODDDResponse;
        }
    }
    /// <summary>
    /// Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetOrganizationDbDebtorsDfsRequestBodyModel                        // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                      // 
    }
    /// <summary>
    /// Модель на відповідь GetOrganizationDbDebtorsDfs
    /// </summary>
    public class GetOrganizationDbDebtorsDfsResponseModel                           // 
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public string Status { get; set; }                                          // 
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }                                         // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<DebtData> Data { get; set; }                                    // 
    }/// <summary>
     /// Дані відповіді
     /// </summary>
    public class DebtData                                                           // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }                                            // 
        /// <summary>
        /// Борг
        /// </summary>
        public OrganizationDbDebtorsDfsAll Debt { get; set; }                       // 
    }/// <summary>
     /// Борг
     /// </summary>
    public class OrganizationDbDebtorsDfsAll                                        // 
    {/// <summary>
     /// Державний борг
     /// </summary>
        public double? BorgTotal { get; set; }                                      // 
        /// <summary>
        /// Місцевий борг
        /// </summary>
        public double? LocalBorg { get; set; }                                      // 
    }
}
