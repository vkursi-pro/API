using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.organizations;
namespace vkursi_api_example.estate
{
    public class GetEstatesClass
    {
        /*
        
        24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ
        [POST] /api/1.0/Estate/GetEstates

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstates' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["36679626"],"Ipn":["3006679626"]}'

        */
        public static GetEstatesResponseModel GetEstates(string token, string edrpou, string ipn)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestates");
                RestRequest request = new RestRequest(Method.POST);

                GetEstatesRequestBodyModel GERequestBodyRow = new GetEstatesRequestBodyModel
                {
                    Edrpou = new List<string> {
                        edrpou                      // Масив кодів ЄДРПОУ (обеження 1)
                    },
                    Ipn = new List<string> {
                        ipn                         // Масив кодів ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GERequestBodyRow);

                // Example Body: {"Edrpou":["36679626"],"Ipn":["3006679626"]}

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

            GetEstatesResponseModel GEResponseRow = new GetEstatesResponseModel();

            GEResponseRow = JsonConvert.DeserializeObject<GetEstatesResponseModel>(responseString);

            return GEResponseRow;
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"3006679626\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/Estate/GetEstates", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:
        
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"3006679626\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstates")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetEstatesRequestBodyModel
    {
        public List<string> Edrpou { get; set; }
        public List<string> Ipn { get; set; }
    }


    public class GetEstatesResponseModel
    {
        public bool isSuccess { get; set; }
        public string status { get; set; }
        public GetEstateApiFreeModelAnswerData data { get; set; }
    }

    public class GetEstateApiFreeModelAnswerData
    {
        public string dataObjectOriginal { get; set; }
        public EstateResponceWithParsedResultData dataObject { get; set; }
        public Dictionary<string, List<int>> types { get; set; }
    }
    public class EstateResponceWithParsedResultData
    {
        public string entity { get; set; }
        public string method { get; set; }
        public string sign { get; set; }
        public SearchParams searchParams { get; set; }
        public int resultID { get; set; }
        public ResponseEstateData resultData { get; set; }
        public long reportResultID { get; set; }
        public int groupID { get; set; }
    }
    public class SearchParams
    {
        public bool isShowHistoricalNames { get; set; }
        public string searchType { get; set; }
        public SubjectSearchInfo subjectSearchInfo { get; set; }
        public string reason { get; set; }
        public bool isDelayed { get; set; }
        public string dcReqtypeSubject { get; set; }
        public bool isExternal { get; set; }
        public bool isSuspend { get; set; }
        public int totalLength { get; set; }
        public int resultCount { get; set; }
        public int regNum { get; set; }
        public DateTime date { get; set; }
    }
    public class SubjectSearchInfo
    {
        public string sbjType { get; set; }
        public string dcSbjRlNames { get; set; }
        public string sbjCode { get; set; }
        public bool generalSubjectSearch { get; set; }
        public int irIrID { get; set; }
    }
    public class ResponseEstateData
    {
        public int? resultID { get; set; }
        public long? reportResultID { get; set; }                   // Для пошуку по об’єкту ="1" для пошуку по суб’єкту = "2" // "enum": ["1", "2"]
        public List<GroupResult> groupResult { get; set; }          // 2 >> 3 
        public string code { get; set; }                            // 
    }
    public class GroupResult                                        // 2 >> 3 
    {
        public int? id { get; set; }                                // Номер групи, для повторного пошуку
        public string dcGroupType { get; set; }                     // Тип групи
        public string name { get; set; }                            // Опис групи, передається значення адреси, або кадастровий номер, або невизначене майно
    }
}
