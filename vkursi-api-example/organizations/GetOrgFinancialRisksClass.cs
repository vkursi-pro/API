using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgFinancialRisksClass
    {
        /*
         
        35. Фінансові ризики
        [POST] /api/1.0/organizations/getorgFinancialRisks
         
        */

        public static GetOrgFinancialRisksResponseModel GetOrgFinancialRisks(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgFinancialRisksRequestBodyModel GOFRRequestBody = new GetOrgFinancialRisksRequestBodyModel
                {
                    Edrpou = new List<string> 
                    {
                        code                                                    // Код ЄДРПОУ або ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOFRRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgFinancialRisks");
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

            GetOrgFinancialRisksResponseModel GOFRResponse = new GetOrgFinancialRisksResponseModel();

            GOFRResponse = JsonConvert.DeserializeObject<GetOrgFinancialRisksResponseModel>(responseString);

            return GOFRResponse;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetOrgFinancialRisksRequestBodyModel                           // 
    {/// <summary>
     /// Перелік ЄДРПОУ / ІПН (обмеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                // 
    }
    /// <summary>
    /// Модель відповіді GetOrgFinancialRisks
    /// </summary>
    public class GetOrgFinancialRisksResponseModel                              // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSucces { get; set; }                                      // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<OrgFinancialRisksApiAnswerModelData> Data { get; set; }     // 
    }/// <summary>
     /// Дані
     /// </summary>
    public class OrgFinancialRisksApiAnswerModelData                            // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// Дані по рокам
        /// </summary>
        public List<OrgFinancialRisksApiAnswerModelDataPerYear> DataPerYear { get; set; }       // 
    }
    /// <summary>
    /// Дані по рокам
    /// </summary>
    public class OrgFinancialRisksApiAnswerModelDataPerYear                     // 
    {/// <summary>
     /// Рік
     /// </summary>
        public int Year { get; set; }                                           // 
        /// <summary>
        /// Клас боржника
        /// </summary>
        public string BorgClass { get; set; }                                   // 
        /// <summary>
        /// Фінансовий стан
        /// </summary>
        public string FinanceState { get; set; }                                // 
        /// <summary>
        /// Міжнародна рейтингова оцінка
        /// </summary>
        public string Rating { get; set; }                                      // 
        /// <summary>
        /// Зобов'язання
        /// </summary>
        public string Obligation { get; set; }                                  // 
    }
}
