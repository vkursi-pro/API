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
        [POST] /api/1.0/Dictionary/getdictionary

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/Dictionary/getdictionary' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"DictionaryName":"AllDictionaryDict"}'
         
        */

        public static GetDictionaryResponseModel GetDictionary(ref string token, int dictionaryCode)
        {
            Dictionary<int, string> AllDictionaryDict = new Dictionary<int, string>
            {
                { 0, "AllDictionaryDict" },                 // Перелік всіх словників
                { 1, "OrganizationStateDict" },             // Стан реєстрації суб'єкта
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

            string dictionaryName = AllDictionaryDict[dictionaryCode];


            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDictionaryRequestBodyModel GDRequestBody = new GetDictionaryRequestBodyModel
                {
                    DictionaryName = dictionaryName                         // Назва словника 
                };

                string body = JsonConvert.SerializeObject(GDRequestBody);   // Example body: {"DictionaryName":"AllDictionaryDict"} - AllDictionaryDict перелік всіх словників

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
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

            GetDictionaryResponseModel GDResponseModel = new GetDictionaryResponseModel();

            GDResponseModel = JsonConvert.DeserializeObject<GetDictionaryResponseModel>(responseString);

            return GDResponseModel;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"DictionaryName\":\"AllDictionaryDict\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/Dictionary/getdictionary", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"DictionaryName\":\"AllDictionaryDict\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/Dictionary/getdictionary")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetDictionaryRequestBodyModel                          // 
    {/// <summary>
     /// Назва словника
     /// </summary>
        public string DictionaryName { get; set; }                      // 
    }
    /// <summary>
    /// Модель відповіді GetDictionary
    /// </summary>
    public class GetDictionaryResponseModel                             // 
    {/// <summary>
     /// Запит виконано успішно (true - так / false - ні)
     /// </summary>
        public bool isSucces { get; set; }                              // 
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public string succes { get; set; }                              // 
        /// <summary>
        /// Зміст словника
        /// </summary>
        public Dictionary<int, string> data { get; set; }               // 
        /// <summary>
        /// Дата останнього оновлення словника
        /// </summary>
        public DateTime updateDate { get; set; }                        // 
    }
}
