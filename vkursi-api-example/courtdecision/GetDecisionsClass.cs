using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionsClass
    {
        // 10.	Запит на отримання даних по судовим рішенням організації
        // [POST] /api/1.0/courtdecision/getdecisions

        public static GetDecisionsResponseModel GetDecisions(string edrpou, int? skip, int? typeSide, int? justiceKindId, List<string> npas, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/courtdecision/getdecisions", Method.POST);

            GetDecisionsRequestBodyModel Body = new GetDecisionsRequestBodyModel
            {
                Edrpou = edrpou,
                Skip = skip,
                TypeSide = typeSide,
                JusticeKindId = justiceKindId,
                Npas = npas
            };

            string body = JsonConvert.SerializeObject(Body);

            // Body example: {"Edrpou":"14360570","Skip":0,"TypeSide":null,"JusticeKindId":null,"Npas":["F545D851-6015-455D-BFE7-01201B629774"]}

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "Not found")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            GetDecisionsResponseModel DecisionsResponseRow = JsonConvert.DeserializeObject<GetDecisionsResponseModel>(responseString);

            return DecisionsResponseRow;
        }
    }

    public class GetDecisionsRequestBodyModel
    {
        public string Edrpou { get; set; }                  // ЄДРПОУ
        public int? Skip { get; set; }                      // К-ть документів які необхідно пропустити (щоб взяти наступні 100)
        public int? TypeSide { get; set; }                  // null - all, 1 - Plaintiffs; 2 - Defendants; 3 - Other;
        public int? JusticeKindId { get; set; }             // null - all, 0 - "Iнше", 1 - "Цивільне", 2 - "Кримінальне", 3 - "Господарське", 4 - "Адміністративне", 5 - "Адмінправопорушення"
        public List<string> Npas { get; set; }              // фильтр по НПА
    }

    public class ListDecisions                              // Перелік Id судових документів
    {
        public int id { get; set; }                         // Id документа
        public DateTime adjudicationDate { get; set; }      // Дата судовога засідання
    }

    public class GetDecisionsResponseModel
    {
        public double totalDecision { get; set; }           // Загальна кількість судових документів
        public List<ListDecisions> list { get; set; }       // Перелік Id судових документів
    }
}
