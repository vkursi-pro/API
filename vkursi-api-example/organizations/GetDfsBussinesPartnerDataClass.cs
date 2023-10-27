using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetDfsBussinesPartnerDataClass
    {
        /// <summary>
        /// 82. Реєстр ДФС “Дізнайся більше про свого бізнес-партнера” [POST] /api/1.0/organizations/GetDfsBussinesPartnerData
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
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
                    Code = new List<string> { code }                        // Код ЄДРПОУ або ІПН
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
    /// <summary>
    /// Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetDfsBussinesPartnerDataRequestBodyModel                   
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                              
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetDfsBussinesPartnerDataResponseModel                      
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public string Status { get; set; }                                   
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }                                  
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<OrgDfsInfo> Data { get; set; }                          
    }/// <summary>
     /// Дані відповіді
     /// </summary>
    public class OrgDfsInfo                                                  
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }                                    
        /// <summary>
        /// Текст інформації з ДФС
        /// </summary>
        public string Info { get; set; }                                     
    }
}
