using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetAdvancedRrpReportClass
    {
        /*
        
        25. Отримання повного витяга з реєстру нерухомого майна (ДРРП)
        [POST] /api/1.0/Estate/GetAdvancedRrpReport

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"GroupId":5001466269723,"ObjectId":68345530}'

        */

        public static GetAdvancedRrpReportResponseModel GetAdvancedRrpReport(string token, long? groupId, int? objectId) 
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport");
                RestRequest request = new RestRequest(Method.POST);

                GetAdvancedRrpReportRequestBodyModel GARRResponseBodyRow = new GetAdvancedRrpReportRequestBodyModel
                {
                    GroupId = groupId,
                    ObjectId = objectId
                };

                string body = JsonConvert.SerializeObject(GARRResponseBodyRow);

                // Example Body: {"GroupId":5001466269723,"ObjectId":68345530}

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

            GetAdvancedRrpReportResponseModel GARRRequest = new GetAdvancedRrpReportResponseModel();

            GARRRequest = JsonConvert.DeserializeObject<GetAdvancedRrpReportResponseModel>(responseString);

            return GARRRequest;

        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"GroupId\":5001466269723,\"ObjectId\":68345530}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/estate/getadvancedrrpreport", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"GroupId\":5001466269723,\"ObjectId\":68345530}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/getadvancedrrpreport")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

    */


    public class GetAdvancedRrpReportRequestBodyModel
    {
        public long? GroupId { get; set; }
        public int? ObjectId { get; set; }
    }

    public class GetAdvancedRrpReportResponseModel
    {
        public bool isSuccess { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string reportHref { get; set; }
        public string dataObjectOriginal { get; set; }
        public DataObject dataObject { get; set; }
    }

    public class DataObject
    {
        public List<Realty> realty { get; set; }
        public List<oldRealty> oldRealty { get; set; }
        public List<oldMortgageJson> oldMortgageJson { get; set; }
        public List<oldLimitationJson> oldLimitationJson { get; set; }
        public List<AllAdress> allAdresses { get; set; }
    }

    public class AllAdress
    {
        public string name { get; set; }
        public object regTime { get; set; }
    }
}
