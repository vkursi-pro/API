using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.dictionary
{
    public class GetDictionaryClass
    {
        /*

        31. Основні словники сервісу
        [POST] /api/1.0/dictionary/getdictionary
         
        */

        public static Dictionary<int, string> GetDictionary(ref string token)
        {
            Dictionary<int, string> AllDictionaryDict = new Dictionary<int, string>
            {
                { 0, "AllDictionaryDict" },                 // Перелік всіх словників
                { 1, "OrganizationStateDict" },             // Стан суб'єкта
                { 2, "NaisPersonRoleDict" },                // Роль по відношенню до пов'язаного суб'єкта
                { 3, "FieldTypeDict" },                     // Типи змін
                { 4, "RegulatorDict" },                     // Перевіряючий орган regulatorId
                { 5, "AuditsResultStatusDict" },            // Статус перевірки
                { 6, "OrganOfLicensesDict" },               // Орган ліцензування
                { 7, "TypeOfLicensesDict" },                // Тип ліцензії
                { 8, "CountryDict" },                       // Країни
                { 9, "RegionDict" },                        // Регіони
                { 10, "BankruptcyPublicationTypeDict" },    // Тип публікації про банкрутство BankruptcyPublicationType
                { 11, "PurposeDict" },                      // Цільве призначення 
                { 12, "SanctionTypeDict" },                 // Тип санкцій
            };


            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDictionaryRequestBodyModel GDRequestBody = new GetDictionaryRequestBodyModel
                {
                    DictionaryName = "AllDictionaryDict"                    // Назва словника 
                };

                string body = JsonConvert.SerializeObject(GDRequestBody);   // Example body: {"DictionaryName":"AllDictionaryDict"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/Dictionary/getdictionary");
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
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            Dictionary<int, string> DResponseDict = new Dictionary<int, string>();

            DResponseDict = JsonConvert.DeserializeObject<Dictionary<int, string>>(responseString);

            return DResponseDict;
        }
    }

    public class GetDictionaryRequestBodyModel                          // Модель Body запиту
    {
        public string DictionaryName { get; set; }                      // Назва словника
    }
}
