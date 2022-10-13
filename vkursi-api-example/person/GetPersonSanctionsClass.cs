using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;
using vkursi_api_example.token;

namespace vkursi_api_example.person
{
    public class GetPersonSanctionsClass
    {
        /// <summary>
        /// 91. Отримання переліку санкцій по ФО
        /// [POST] /api/1.0/person/GetPersonSanctions
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        
        public static GetPersonSanctionsResponseModel GetPersonSanctions(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPersonSanctionsRequestBodyModel GRFABRequestBody = new GetPersonSanctionsRequestBodyModel
                {
                    EdrpouList = new List<string> { code }                     // Код ЄДРПОУ / ІПН
                };

                string body = JsonConvert.SerializeObject(GRFABRequestBody);   // Example body: {"edrpouList":"00222166"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/GetPersonSanctions");
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
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetPersonSanctionsResponseModel organizationAnalytic = new GetPersonSanctionsResponseModel();

            organizationAnalytic = JsonConvert.DeserializeObject<GetPersonSanctionsResponseModel>(responseString);

            return organizationAnalytic;
        }
    }


    public class GetPersonSanctionsRequestBodyModel
    {
        public string Text { get; set; }
    }

    public class GetPersonSanctionsResponseModel
    {

    }

}
