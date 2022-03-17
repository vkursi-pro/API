using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetDfsBussinesPartnerDataClass
    {
        /*
         
        82. Реєстр ДФС “Дізнайся більше про свого бізнес-партнера”
        [POST] /api/1.0/organizations/GetDfsBussinesPartnerData

        */

        public static GetDfsBussinesPartnerDataResponseModel GetDfsBussinesPartnerData(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDfsBussinesPartnerDataRequestBodyModel GDBPDRBody = new GetDfsBussinesPartnerDataRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GDBPDRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetDfsBussinesPartnerData");
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

            GetDfsBussinesPartnerDataResponseModel GDBPDResponse = new GetDfsBussinesPartnerDataResponseModel();

            GDBPDResponse = JsonConvert.DeserializeObject<GetDfsBussinesPartnerDataResponseModel>(responseString);

            return GDBPDResponse;
        }
    }

    public class GetDfsBussinesPartnerDataRequestBodyModel                  // Модель запиту (Example: {"code":["21560766"]})
    {
        public List<string> Code { get; set; }                              // Код ЄДРПОУ
    }

    public class GetDfsBussinesPartnerDataResponseModel                     // Модель на відповідь
    {
        public string Status { get; set; }                                  // Статус відповіді по API
        public bool IsSuccess { get; set; }                                 // Чи успішний запит
        public List<OrgDfsInfo> Data { get; set; }                          // Дані відповіді
    }
    public class OrgDfsInfo                                                 // Дані відповіді
    {
        public string Code { get; set; }                                    // Код ЄДРПОУ
        public string Info { get; set; }                                    // Текст інформації з ДФС
    }
}
