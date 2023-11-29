using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetOrgFinanceShortClass
    {
        /*
         
        70. Скорочені основні фінансові показники діяльності підприємства 
        [POST] /api/1.0/organizations/GetOrgFinanceShort
        
        */
        public static GetOrgFinanceShortResponseModel GetOrgFinanceShort(ref string token, string edrpou, List<int> years)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinanceShort");
                RestRequest request = new RestRequest(Method.POST);

                GetOrgFinanceShortRequestBodyModel GOFSRequest = new GetOrgFinanceShortRequestBodyModel
                {
                    Filter = new List<GetOrgFinanceShortInData> { 
                    new GetOrgFinanceShortInData{ Code = edrpou, Years = years}
                    }
                };

                string body = JsonConvert.SerializeObject(GOFSRequest); // Example: {"Filter":[{"Years":[2019, 2020],"Code":"41462280"}]}

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


            GetOrgFinanceShortResponseModel GOFSResponseRow = new GetOrgFinanceShortResponseModel();

            GOFSResponseRow = JsonConvert.DeserializeObject<GetOrgFinanceShortResponseModel>(responseString);

            return GOFSResponseRow;

        }
    }
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetOrgFinanceShortRequestBodyModel         // 
    {/// <summary>
     /// Фильтр
     /// </summary>
        public List<GetOrgFinanceShortInData> Filter { get; set; }  // 
    }
    /// <summary>
    /// Фильтр
    /// </summary>
    public class GetOrgFinanceShortInData                   // 
    {/// <summary>
     /// Рік
     /// </summary>
        public List<int> Years { get; set; }                // 
        /// <summary>
        /// Код Єдрпоу
        /// </summary>
        public string Code { get; set; }                    // 
    }


    /// <summary>
    /// Відповідь на запит
    /// </summary>
    public class GetOrgFinanceShortResponseModel             // 
    {/// <summary>
     /// Статус відповіді
     /// </summary>
        public string Status { get; set; }                  // 
        /// <summary>
        /// Чи успішна відповідь
        /// </summary>
        public bool IsSuccess { get; set; }                 // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<GetOrgFinanceShortAnswerData> Data { get; set; }    // 
    }
    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class GetOrgFinanceShortAnswerData               // 
    {/// <summary>
     /// Массив данніх в розрізі року
     /// </summary>
        public List<GetOrgFinanceShortAnswerDataByYear> Data { get; set; }  // 
        /// <summary>
        /// Код Єдрпоу
        /// </summary>
        public string Code { get; set; }                    // 
    }
    /// <summary>
    /// Массив данніх в розрізі року
    /// </summary>
    public class GetOrgFinanceShortAnswerDataByYear         // 
    {/// <summary>
     /// Рік
     /// </summary>
        public int Year { get; set; }                       // 
        /// <summary>
        /// Виручка (Дохід)
        /// </summary>
        public double? VyruchkaDohid { get; set; }          // 
        /// <summary>
        /// Основні засоби
        /// </summary>
        public double? OsnovniZasoby { get; set; }          // 
        /// <summary>
        /// Оборотні активи
        /// </summary>
        public double? OborotniAktyvy { get; set; }         // 
        /// <summary>
        /// Власний капітал
        /// </summary>
        public double? VlasnyyKapital { get; set; }         // 
        /// <summary>
        /// Довгострокові зобов'язання
        /// </summary>
        public double? DovhostrokoviZob { get; set; }       // 
        /// <summary>
        /// Поточні зобов'язання
        /// </summary>
        public double? PotochniZob { get; set; }            // 
    }
}
