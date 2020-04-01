using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.courtdecision
{
    public class GetDecisionByIdClass
    {
        /*
         
        26. Рекізити судового документа
        [POST] /api/1.0/courtdecision/getdecisionbyid

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '"88234097"'

        */
        public static GetDecisionByIdResponseModel GetDecisionById(string courtDecisionId, string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid");
                RestRequest request = new RestRequest(Method.POST);

                string body = "\"" + courtDecisionId + "\"";

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

            GetDecisionByIdResponseModel GDBIResponseRow = new GetDecisionByIdResponseModel();

            GDBIResponseRow = JsonConvert.DeserializeObject<GetDecisionByIdResponseModel>(responseString);

            return GDBIResponseRow;
        }
    }


    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "\"88234097\""
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/courtdecision/getdecisionbyid", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "\"88234097\"");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/courtdecision/getdecisionbyid")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetDecisionByIdResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public CourtDecisionElasticModel Data { get; set; }
    }

    public class CourtDecisionElasticModel
    {
        public DateTime AdjudicationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int CauseCategoryId { get; set; }
        public Content Content { get; set; }
        public int CourtId { get; set; }
        public DateTime DatePublished { get; set; }
        public int Id { get; set; }
        public int InstanceId { get; set; }
        public Judge Judge { get; set; }
        public int JudgmentFormId { get; set; }
        public int JusticeKindId { get; set; }
        public string Number { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int RegionId { get; set; }
        public ResultPart ResultPart { get; set; }
        public string Theme { get; set; }
        public string Url { get; set; }
        public int? SideKasType { get; set; }
        public List<string> NPA { get; set; }
        public int? IsCompressed { get; set; }
        public AmountClime AmountClaim { get; set; }
        public string[] CriminalNumb { get; set; }
        public string[] CadastralNumb { get; set; }

        public bool? ContainAppeal { get; set; }
        public bool? ContainCassation { get; set; }
        public List<SidesAllModel> SidesAllNested { get; set; }
    }

    public class SidesAllModel
    {
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public int? FindParam { get; set; }
        public int? KvedCodeInt { get; set; }
        public int? WinnerState { get; set; }
        public int? CategoryId { get; set; }
        public int SidesType { get; set; }
        public int? Error { get; set; }
        public int? RegNumberInt { get; set; }
        public string Name { get; set; }
    }

    public class AmountClime
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
    }

    public class ResultPart
    {
        public string[] Result { get; set; }
        public int Winner { get; set; }
    }

    public class Judge
    {
        public string Name { get; set; }
        public int Role { get; set; }
    }

    public class Content
    {
        public string Footer { get; set; }
        public string Header { get; set; }
        public string Middle { get; set; }
        public string NotFound { get; set; }
    }
}
