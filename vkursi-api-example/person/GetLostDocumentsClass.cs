using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.person
{
    internal class GetLostDocumentsClass
    {
        /// <summary>
        /// 92. Перевірка втрачених документів
        /// [POST] /api/1.0/person/getLostDocuments
        /// </summary>

        public static GetLostDocumentsResponseModel GetLostDocuments(ref string token, string passportInfo)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetLostDocumentsRequestBodyModel GLDRequestBody = new GetLostDocumentsRequestBodyModel
                {
                    PassportInfo = new List<string> { passportInfo }         // Номер документу
                };

                string body = JsonConvert.SerializeObject(GLDRequestBody);   // Example body: {"PassportInfo":["AA101010"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/getLostDocuments");
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

            GetLostDocumentsResponseModel organizationAnalytic = new GetLostDocumentsResponseModel();

            organizationAnalytic = JsonConvert.DeserializeObject<GetLostDocumentsResponseModel>(responseString);

            return organizationAnalytic;
        }
    }

    public class GetLostDocumentsRequestBodyModel
    {
        /// <summary>
        /// Номери документів (Обмеження 10 паспортів)
        /// </summary>
        public List<string> PassportInfo { get; set; }
    }

    public class GetLostDocumentsResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public List<GetLostDocumentsDetails> Data { get; set; }
    }

    public class GetLostDocumentsDetails
    {
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime ActualDate { get; set; }
        public string Ovd { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}
