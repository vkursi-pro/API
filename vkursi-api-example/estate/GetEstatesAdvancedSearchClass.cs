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
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetEstatesAdvancedSearchBodyModel GEASBody = new GetEstatesAdvancedSearchBodyModel
                {
                    RequestList = new List<SubjectSearchInfoAdvanced>
                        {
                            new SubjectSearchInfoAdvanced {
                                //SbjCode = "19124549",
                                SeriesNum = "005798610",
                                SbjType = "2"
                        }
                    }
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstatesAdvancedSearch");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GEASBody);    // Example Body: {"RequestList":[{"dcSbjRlNames":null,"sbjType":"2","sbjCode":"19124549"}]}

                //body = "{\"RequestList\":[{\"dcSbjRlNames\":null,\"sbjType\":\"2\",\"sbjCode\":\"19124549\"}]}";

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
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetEstatesAdvancedSearchBodyModel                              // 
    {/// <summary>
     /// Параметри пошуку (обеження 1)
     /// </summary>
        public List<SubjectSearchInfoAdvanced> RequestList { get; set; }        // 
    }
    /// <summary>
    /// Параметри пошуку (обеження 1)
    /// </summary>
    public class SubjectSearchInfoAdvanced                                      // 
    {/// <summary>
     /// Тип пошуку - Пошук за: «частковим співпадінням» (1) або «повним співпадінням» (2)
     /// </summary>
        [JsonProperty("dcSearchAlgorithm", NullValueHandling = NullValueHandling.Ignore)]
        public string DcSearchAlgorithm { get; set; }                           // 
        /// <summary>
        /// Тип суб’єкта - "enum": ["1", "2"]
        /// </summary>
        [JsonProperty("sbjType", NullValueHandling = NullValueHandling.Ignore)]
        public string SbjType { get; set; }                                     // 
        /// <summary>
        /// Назва/ПІБ
        /// </summary>
        [JsonProperty("sbjName", NullValueHandling = NullValueHandling.Ignore)]
        public string SbjName { get; set; }                                     // 
        /// <summary>
        /// РНОКПП/ЄДРПОУякщо обрано Тип особи = фізична особа, - {10} якщо обрано Тип особи = юридична особа, - {8, 9, 12}
        /// </summary>
        [JsonProperty("sbjCode", NullValueHandling = NullValueHandling.Ignore)]
        public string SbjCode { get; set; }                                     // 
        /// <summary>
        /// Серія,номер документа
        /// </summary>
        [JsonProperty("seriesNum", NullValueHandling = NullValueHandling.Ignore)]
        public string SeriesNum { get; set; }                                   // 
        /// <summary>
        /// УНЗР значення повинно відповідати YYYYYYYY-YYYYY, допустимі значення[0 - 9]та символ -
        /// </summary>
        [JsonProperty("idEddr", NullValueHandling = NullValueHandling.Ignore)]
        public string IdEddr { get; set; }                                      // 
        /// <summary>
        /// ознака «Код РНОКПП відсутній»
        /// </summary>
        [JsonProperty("codeAbsence", NullValueHandling = NullValueHandling.Ignore)]
        public string CodeAbsence { get; set; }                                 // 
        /// <summary>
        /// Роль суб’єкта
        /// </summary>
        [JsonProperty("dcSbjRlNames", NullValueHandling = NullValueHandling.Ignore)]
        public string DcSbjRlNames { get; set; }                                // 
    }

    // Модель відповіді така ж як в 24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetEstatesClass.cs

}
