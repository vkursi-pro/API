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

    public class GetOrgFinanceShortRequestBodyModel         // Модель запиту 
    {
        public List<GetOrgFinanceShortInData> Filter { get; set; }  // Фильтр
    }

    public class GetOrgFinanceShortInData                   // Фильтр
    {
        public List<int> Years { get; set; }                // Рік
        public string Code { get; set; }                    // Код Єдрпоу
    }



    public class GetOrgFinanceShortResponseModel             // Відповідь на запит
    {
        public string Status { get; set; }                  // Статус відповіді
        public bool IsSuccess { get; set; }                 // Чи успішна відповідь
        public List<GetOrgFinanceShortAnswerData> Data { get; set; }    // Дані відповіді
    }

    public class GetOrgFinanceShortAnswerData               // Дані відповіді
    {
        public List<GetOrgFinanceShortAnswerDataByYear> Data { get; set; }  // Массив данніх в розрізі року
        public string Code { get; set; }                    // Код Єдрпоу
    }

    public class GetOrgFinanceShortAnswerDataByYear         // Массив данніх в розрізі року
    {
        public int Year { get; set; }                       // Рік
        public double? VyruchkaDohid { get; set; }          // Виручка (Дохід)
        public double? OsnovniZasoby { get; set; }          // Основні засоби
        public double? OborotniAktyvy { get; set; }         // Оборотні активи
        public double? VlasnyyKapital { get; set; }         // Власний капітал
        public double? DovhostrokoviZob { get; set; }       // Довгострокові зобов'язання
        public double? PotochniZob { get; set; }            // Поточні зобов'язання
    }
}
