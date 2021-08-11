using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetActivityOrgHistoryClass
    {

        /*
         
        72. API з історії зміни даних в ЄДР
        [POST] /api/1.0/organizations/getactivityorghistory

        */

        public static GetActivityOrgHistoryResponseModel GetActivityOrgHistory(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetActivityOrgHistoryRequestBodyModel GAOHRBody = new GetActivityOrgHistoryRequestBodyModel
                {
                    Code = new List<string> { code },
                };

                string body = JsonConvert.SerializeObject(GAOHRBody);  // {"Codes":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getactivityorghistory");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }


            GetActivityOrgHistoryResponseModel GAOHResponse = new GetActivityOrgHistoryResponseModel();

            GAOHResponse = JsonConvert.DeserializeObject<GetActivityOrgHistoryResponseModel>(responseString);

            return GAOHResponse;
        }
    }

    public class GetActivityOrgHistoryRequestBodyModel                          // Модель запиту (Example: )
    {
        public List<string> Code { get; set; }                                  // Код ЕДРПОУ
    }

    public class GetActivityOrgHistoryResponseModel                             // Модель відповіді
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API
        public List<ActivityOrgHistoryApiModelAnswerData> Data { get; set; }    // Дані методу
    }

    public class ActivityOrgHistoryApiModelAnswerData                           // Дані методу
    {
        public string Code { get; set; }                                        // Код ЕДРПОУ
        public bool KvedChange { get; set; }                                    // Менялся ли КВЕД (за предыдущие 12 мес) 
        public bool ReorganizeChange { get; set; }                              // Реорганизация  (за предыдущие 12 мес) 

        public bool CompanyStructureChange { get; set; }                        // Зміни в структурі власності (за предыдущие 12 мес)   
        public bool StatCapitalChangeToLower { get; set; }                      // Уменьшался ли статутный  (за предыдущие 12 мес)  

    }
}
