using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgAnulPdvClass
    {
        /*
         
        80. Анульована реєстрація платником ПДВ
        [POST] /api/1.0/organizations/GetOrgAnulPdv
         
        */

        public static GetOrgAnulPdvResponseModel GetOrgAnulPdv(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgAnulPdvRequestBodyModel GOAPRBody = new GetOrgAnulPdvRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GOAPRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgAnulPdv");
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

            GetOrgAnulPdvResponseModel GOAPResponse = new GetOrgAnulPdvResponseModel();

            GOAPResponse = JsonConvert.DeserializeObject<GetOrgAnulPdvResponseModel>(responseString);

            return GOAPResponse;
        }
    }

    public class GetOrgAnulPdvRequestBodyModel                                      // Модель запиту 
    {
        public List<string> Code { get; set; }                                      // Код ЄДРПОУ
    }

    public class GetOrgAnulPdvResponseModel                                         // Модель відповіді
    {
        public string Status { get; set; }                                          // Статус запиту (maxLength:128)
        public bool IsSuccess { get; set; }                                         // Запит виконано успішно (true - так / false - ні)
        public List<OrgAnulPdvInfo> Data { get; set; }                              // Дані відповіді
    }
    public class OrgAnulPdvInfo                                                     // Дані відповіді
    {
        public string Code { get; set; }                                            // Код ЕДРПОУ
        public DateTime? AnulDateTime { get; set; }                                 // Дата анулювання
    }
}
