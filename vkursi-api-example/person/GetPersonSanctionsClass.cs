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
        
        public static GetPersonSanctionsResponseModel GetPersonSanctions(ref string token, string text)
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
                    Text = text                     // ПІБ ЮО або назва (якщо компанія) 
                };

                string body = JsonConvert.SerializeObject(GRFABRequestBody);   // Example body: {"text":"КОРОТКИЙ АЛЕКСАНДР ВЛАДИМИРОВИЧ"}

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

    /// <summary>
    /// Модель Body запиту 
    /// </summary>
    public class GetPersonSanctionsRequestBodyModel
    {
        /// <summary>
        /// ПІБ ЮО або назва (якщо компанія) 
        /// </summary>
        public string Text { get; set; }
        public double? Percent { get; set; } = 0.95;
        public int? MaxOffset { get; set; } = 0;
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetPersonSanctionsResponseModel
    {
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        public List<PersonSanctions> Data { get; set; }
    }

    public class PersonSanctions
    {
        public Guid Id { get; set; }
        public List<GetPersonSanctionsData> Sanctions { get; set; }
    }

    public class GetPersonSanctionsData
    {
        public int SanctionType { get; set; }
        public bool SearchByIpn { get; set; }
        public DateTime? SanctionStart { get; set; }
        public string Name { get; set; }
        public string NameFromSanction { get; set; }
        public DateTime? BirthDate { get; set; }
        public object Details { get; set; }
        public List<string> PersonNames { get; set; }
    }

}
