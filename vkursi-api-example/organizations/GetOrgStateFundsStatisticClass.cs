using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgStateFundsStatisticClass
    {
        /*

        40. Частка державних коштів в доході
        [POST] /api/1.0/organizations/getorgstatefundsstatistic

        */

        public static GetOrgShareholdersResponseModel GetOrgStateFundsStatistic(string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgShareholdersRequestBodyModel GOSRequestBody = new GetOrgShareholdersRequestBodyModel
                {
                    Edrpou = new List<string>                                   // Перелік кодів ЄДРПОУ (обеження 1)
                    {
                        edrpou
                    }
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgstatefundsstatistic");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GOSRequestBody);      // Example Body: {"Edrpou":["00131305"]}

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

            GetOrgShareholdersResponseModel GOSResponseRow = new GetOrgShareholdersResponseModel();

            GOSResponseRow = JsonConvert.DeserializeObject<GetOrgShareholdersResponseModel>(responseString);

            return GOSResponseRow;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetOrgStateFundsStatisticRequestBodyModel                      // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ (обеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                // 
    }
    /// <summary>
    /// Модель відповіді GetEstates
    /// </summary>
    public class GetOrgStateFundsStatisticResponseModel                         // 
    {/// <summary>
     /// Успішний запит (true - так / false - ні)
     /// </summary>
        public bool IsSucces { get; set; }                                      // 
        /// <summary>
        /// Статус запиту
        /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Дані запиту
        /// </summary>
        public List<OrgStateFundsStatisticApiAnswerModelData> Data { get; set; }// 
    }
    /// <summary>
    /// Дані запиту
    /// </summary>
    public class OrgStateFundsStatisticApiAnswerModelData                       // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Edrpou { get; set; }                                      // 
        /// <summary>
        /// Дані
        /// </summary>
        public StateFundStatisticModel Data { get; set; }                       // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class StateFundStatisticModel                                        // 
    {/// <summary>
     /// Інформація про виграні тендери
     /// </summary>
        public List<StateFundInputStatistic> TenderAnalytic { get; set; }       // 
        /// <summary>
        /// Дохід (обороти)
        /// </summary>
        public List<OrganizationFinancalResultModel> Net_Income { get; set; }// 
        /// <summary>
        /// Публічні кошти
        /// </summary>
        public List<StateFundInputStatistic> Edata { get; set; }                // 
    }
    /// <summary>
    /// Інформація про виграні тендери
    /// </summary>
    public class StateFundInputStatistic                                        // 
    {/// <summary>
     /// Рік
     /// </summary>
        public string year { get; set; }                                        // 
        /// <summary>
        /// Сума
        /// </summary>
        public double? sum { get; set; }                                        // 
    }
    /// <summary>
    /// Дохід (обороти)
    /// </summary>
    public class OrganizationFinancalResultModel                             // 
    {/// <summary>
     /// Рік
     /// </summary>
        public string year { get; set; }                                        // 
        /// <summary>
        /// Сума
        /// </summary>
        public long? sum { get; set; }                                          // 
    }
}
