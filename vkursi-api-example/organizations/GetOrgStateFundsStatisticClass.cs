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
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

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
                    token = AuthorizeClass.Authorize();
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

    public class GetOrgStateFundsStatisticRequestBodyModel                      // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік кодів ЄДРПОУ (обеження 1)
    }

    public class GetOrgStateFundsStatisticResponseModel                         // Модель відповіді GetEstates
    {
        public bool IsSucces { get; set; }                                      // Успішний запит (true - так / false - ні)
        public string Status { get; set; }                                      // Статус запиту
        public List<OrgStateFundsStatisticApiAnswerModelData> Data { get; set; }// Дані запиту
    }

    public class OrgStateFundsStatisticApiAnswerModelData                       // Дані запиту
    {
        public string Edrpou { get; set; }                                      // Код ЄДРПОУ
        public StateFundStatisticModel Data { get; set; }                       // Дані
    }

    public class StateFundStatisticModel                                        // Дані
    {
        public List<StateFundInputStatistic> TenderAnalytic { get; set; }       // Інформація про виграні тендери
        public List<OrganizationFinancalResultModel> Net_Income { get; set; }// Дохід (обороти)
        public List<StateFundInputStatistic> Edata { get; set; }                // Публічні кошти
    }

    public class StateFundInputStatistic                                        // Інформація про виграні тендери
    {
        public string year { get; set; }                                        // Рік
        public double? sum { get; set; }                                        // Сума
    }

    public class OrganizationFinancalResultModel                             // Дохід (обороти)
    {
        public string year { get; set; }                                        // Рік
        public long? sum { get; set; }                                          // Сума
    }
}
