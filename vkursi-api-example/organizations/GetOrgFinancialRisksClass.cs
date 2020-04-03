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
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgFinancialRisksRequestBodyModel GOFRRequestBody = new GetOrgFinancialRisksRequestBodyModel
                {
                    Edrpou = new List<string> 
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
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
                    token = AuthorizeClass.Authorize();
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

    public class GetOrgFinancialRisksRequestBodyModel                           // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
    }

    public class GetOrgFinancialRisksResponseModel                              // Модель відповіді GetOrgFinancialRisks
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API
        public List<OrgFinancialRisksApiAnswerModelData> Data { get; set; }     // Дані
    }
    public class OrgFinancialRisksApiAnswerModelData                            // Дані
    {
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ
        public List<OrgFinancialRisksApiAnswerModelDataPerYear> DataPerYear { get; set; }       // Дані по рокам
    }

    public class OrgFinancialRisksApiAnswerModelDataPerYear                     // Дані по рокам
    {
        public int Year { get; set; }                                           // Рік
        public string BorgClass { get; set; }                                   // Клас боржника
        public string FinanceState { get; set; }                                // Фінансовий стан
        public string Rating { get; set; }                                      // Міжнародна рейтингова оцінка
        public string Obligation { get; set; }                                  // Зобов'язання
    }
}
