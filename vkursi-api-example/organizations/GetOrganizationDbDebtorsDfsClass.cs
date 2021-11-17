using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrganizationDbDebtorsDfsClass
    {
        /*
         
        84. Реєстр ДФС “Стан розрахунків з бюджетом”
        [POST] /api/organizations/GetOrganizationDbDebtorsDfs
         
        */

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
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
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

    public class GetOrganizationDbDebtorsDfsRequestBodyModel                        // Модель запиту (Example: {"code":["21560766"]})
    {
        public List<string> Code { get; set; }                                      // Код ЄДРПОУ
    }

    public class GetOrganizationDbDebtorsDfsResponseModel                           // Модель на відповідь GetOrganizationDbDebtorsDfs
    {
        public string Status { get; set; }                                          // Статус відповіді по API
        public bool IsSuccess { get; set; }                                         // Чи успішний запит
        public List<DebtData> Data { get; set; }                                    // Дані відповіді
    }
    public class DebtData                                                           // Дані відповіді
    {
        public string Code { get; set; }                                            // Код ЄДРПОУ
        public OrganizationDbDebtorsDfsAll Debt { get; set; }                       // Борг
    }
    public class OrganizationDbDebtorsDfsAll                                        // Борг
    {
        public double? BorgTotal { get; set; }                                      // Державний борг
        public double? LocalBorg { get; set; }                                      // Місцевий борг
    }
}
