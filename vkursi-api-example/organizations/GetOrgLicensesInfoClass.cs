using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgLicensesInfoClass
    {
        /*

        37. Перелік ліцензій, та дозволів
        [POST] /api/1.0/organizations/getorglicensesinfo        
         
        */

        public static GetOrgLicensesInfoResponseModel GetOrgLicensesInfo(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgLicensesInfoRequestBodyModel GOLIRequestBody = new GetOrgLicensesInfoRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOLIRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorglicensesinfo");
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

            GetOrgLicensesInfoResponseModel GOLIResponse = new GetOrgLicensesInfoResponseModel();

            GOLIResponse = JsonConvert.DeserializeObject<GetOrgLicensesInfoResponseModel>(responseString);

            return GOLIResponse;
        }
    }

    public class GetOrgLicensesInfoRequestBodyModel                             // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
    }

    public class GetOrgLicensesInfoResponseModel                                // Модель відповіді GetOrgLicensesInfo
    {
        public bool IsSucces { get; set; }                                      // Статус відповіді по API
        public string Status { get; set; }                                      // Чи успішний запит
        public List<OrgLicensesApiApiAnswerModelData> Data { get; set; }        // Дані
    }

    public class OrgLicensesApiApiAnswerModelData                               // Дані
    {
        public string Edrpou { get; set; }                                      // ЄДРПОУ / ІПН 
        public List<OrgLicensesApiApiAnswerModelDataObject> Licenses { get; set; }      // Перелік ліцензій
    }
    public class OrgLicensesApiApiAnswerModelDataObject                         // Перелік ліцензій
    {
        public string Name { get; set; }                                        // Назва
        public string Number { get; set; }                                      // Номер
        public DateTime? DateEnd { get; set; }                                  // Дата початку
        public string State { get; set; }                                       // Дата закінчення
    }
}
