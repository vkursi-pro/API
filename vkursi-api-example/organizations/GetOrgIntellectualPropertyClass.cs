using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgIntellectualPropertyClass
    {

        /*

        38. Відомості про інтелектуальну власність (патенти, торгові марки, корисні моделі) які повязані по ПІБ з бенеціціарами підприємства
        [POST] /api/1.0/organizations/getorgintellectualproperty       
         
        */

        public static GetOrgIntellectualPropertyResponseModel GetOrgIntellectualProperty(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgIntellectualPropertyRequestBodyModel GOIPRequestBody = new GetOrgIntellectualPropertyRequestBodyModel
                {
                    Edrpou = new List<string>
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GOIPRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgvehicle");
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

            GetOrgIntellectualPropertyResponseModel GOIPResponse = new GetOrgIntellectualPropertyResponseModel();

            GOIPResponse = JsonConvert.DeserializeObject<GetOrgIntellectualPropertyResponseModel>(responseString);

            return GOIPResponse;
        }
    }


    public class GetOrgIntellectualPropertyRequestBodyModel                     // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік ЄДРПОУ / ІПН (обмеження 1)
        public int? Take { get; set; }                                          // Кількість записів які будуть отримані
        public int? Skip { get; set; }                                          // Кількість записів які будуть пропущені
    }

    public class GetOrgIntellectualPropertyResponseModel                        // Модель відповіді GetOrgVehicle
    {
        public bool IsSucces { get; set; }                                      // Чи успішний запит
        public string Status { get; set; }                                      // Статус відповіді по API
        public List<OrgIntellectualPropertyApiAnswerModelData> Data { get; set; }   // Дані
    }

    public class OrgIntellectualPropertyApiAnswerModelData                      // Дані
    {
        public string Edrpou { get; set; }                                      // 
        public List<OrgIntellectualPropertyApiAnswerModelDataTradeMark> TradeMarks { get; set; }    // 
    }
    public class OrgIntellectualPropertyApiAnswerModelDataTradeMark             // 
    {
        public string RegistrationNumber { get; set; }                          // 
        public DateTime? RegistrationDate { get; set; }                         // 
        public string ApplicationNumber { get; set; }                           // 
        public DateTime? ApplicationDate { get; set; }                          // 
        public string Owner { get; set; }                                       // 
        public List<int> ICTPIndexes { get; set; }                              // 
    }
}
