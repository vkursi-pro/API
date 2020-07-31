using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;


namespace vkursi_api_example.estate
{
    public class GetEstatesAdvancedSearchClass
    {
        /*
        
        // 52. Оригінальний метод пошуку нерухомості Nais (короткі дані) 
        // [POST] api/1.0/Estate/GetEstatesAdvancedSearch
        
        */

        public static GetEstatesResponseModel GetEstatesAdvancedSearch(string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetEstatesAdvancedSearchBodyModel GEASBody = new GetEstatesAdvancedSearchBodyModel
                {
                    RequestList = new List<SubjectSearchInfoAdvanced>
                        {
                            new SubjectSearchInfoAdvanced {
                                sbjCode = "19124549",
                        }
                    }
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstatesAdvancedSearch");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GEASBody);    // Example Body: {"RequestList":[{"dcSbjRlNames":null,"sbjType":"2","sbjCode":"19124549"}]}

                body = "{\"RequestList\":[{\"dcSbjRlNames\":null,\"sbjType\":\"2\",\"sbjCode\":\"19124549\"}]}";

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

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetEstatesResponseModel GEASResponse = new GetEstatesResponseModel();

            GEASResponse = JsonConvert.DeserializeObject<GetEstatesResponseModel>(responseString);

            return GEASResponse;
        }
    }

    public class GetEstatesAdvancedSearchBodyModel                              // Модель Body запиту
    {
        public List<SubjectSearchInfoAdvanced> RequestList { get; set; }        // Параметри пошуку (обеження 1)
    }

    public class SubjectSearchInfoAdvanced                                      // Параметри пошуку (обеження 1)
    {
        public string dcSearchAlgorithm { get; set; }                           // Тип пошуку - Пошук за: «частковим співпадінням» (1) або «повним співпадінням» (2)
        public string sbjType { get; set; }                                     // Тип суб’єкта - "enum": ["1", "2"]
        public string sbjName { get; set; }                                     // Назва/ПІБ
        public string sbjCode { get; set; }                                     // РНОКПП/ЄДРПОУякщо обрано Тип особи = фізична особа, - {10} якщо обрано Тип особи = юридична особа, - {8, 9, 12}
        public string seriesNum { get; set; }                                   // Серія,номер документа
        public string idEddr { get; set; }                                      // УНЗР значення повинно відповідати YYYYYYYY-YYYYY, допустимі значення[0 - 9]та символ -
        public string codeAbsence { get; set; }                                 // ознака «Код РНОКПП відсутній»
        public string dcSbjRlNames { get; set; }                                // Роль суб’єкта
    }

    // Модель відповіді така ж як в 24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetEstatesClass.cs

}
