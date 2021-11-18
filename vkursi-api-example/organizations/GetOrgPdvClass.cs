using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgPdvClass
    {
        /*
         
        79. Реєстр платників ПДВ
        [POST] /api/organizations/GetOrgPdv
         
        */

        public static GetOrgPdvResponseModel GetOrgPdv(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgPdvRequestBodyModel GKPRBody = new GetOrgPdvRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GKPRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/organizations/GetOrgPdv");
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

            GetOrgPdvResponseModel GOPResponse = new GetOrgPdvResponseModel();

            GOPResponse = JsonConvert.DeserializeObject<GetOrgPdvResponseModel>(responseString);

            return GOPResponse;
        }
    }

    public class GetOrgPdvRequestBodyModel                                          // Модель запиту (Example: {"code":["21560766"]})
    {
        public List<string> Code { get; set; }                                      // Код ЄДРПОУ
    }

    public class GetOrgPdvResponseModel                                             // Модель на відповідь GetKilkistPracivnukiv
    {
        public string Status { get; set; }                                          // Статус запиту (maxLength:128)
        public bool IsSuccess { get; set; }                                         // Запит виконано успішно (true - так / false - ні)
        public List<OrgPdvInfo> Data { get; set; }                                  // Дані відповіді
    }
    public class OrgPdvInfo                                                         // Дані відповіді
    {
        public string Code { get; set; }                                            // Код ЄДРПОУ
        public DateTime? DateRegInn { get; set; }                                   // Дата отримання свідоцтва ПДВ
        public string Inn { get; set; }                                             // ІПН
        public string Info { get; set; }                                            // Загальній статус (платник ПДВ / не платник ПДВ)
    }
}
