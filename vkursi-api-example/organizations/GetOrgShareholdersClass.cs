using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;


namespace vkursi_api_example.organizations
{
    public class GetOrgShareholdersClass
    {
        /*

        39. Відомості про власників пакетів акцій (від 5%)
        [POST] /api/1.0/organizations/getorgshareholders

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgshareholders' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["00131305"]}'

        */
        public static GetOrgShareholdersResponseModel GetOrgShareholders(string token, string edrpou)
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

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgshareholders");
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

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"00131305\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getorgshareholders", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorgshareholders")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    public class GetOrgShareholdersRequestBodyModel                             // Модель Body запиту
    {
        public List<string> Edrpou { get; set; }                                // Перелік кодів ЄДРПОУ (обеження 1)
    }

    public class GetOrgShareholdersResponseModel                                // Модель відповіді GetEstates
    {
        public bool IsSucces { get; set; }                                      // Успішний запит (true - так / false - ні)
        public string Status { get; set; }                                      // Статус запиту
        public List<OrgShareHoldersApiAnswerModelData> Data { get; set; }       // Дані запиту
    }

    public class OrgShareHoldersApiAnswerModelData                                              // Дані запиту
    {
        public string Edrpou { get; set; }                                                      // Код ЄДРПОУ
        public List<OrgShareHoldersApiAnswerModelDataPerKvartal> PerYearKvartals { get; set; }  // Дані
    }
    public class OrgShareHoldersApiAnswerModelDataPerKvartal                                    // Дані
    {
        public int Year { get; set; }                                                           // Рік
        public int Kvartal { get; set; }                                                        // Квартал
        public List<OrgShareHoldersApiAnswerModelDataShareHolder> ShareHolders { get; set; }    // Перелік власників пакетів акцій
    }
    public class OrgShareHoldersApiAnswerModelDataShareHolder                   // Перелік власників пакетів акцій
    {
        public string Name { get; set; }                                        // Назва
        public string IdentifikatsiyniyKod { get; set; }                        // Код ЄДРПОУ
        public string Country { get; set; }                                     // Країна реєстрації
        public string TypeOfDepositor { get; set; }                             // Вид депонента
        public string ViewOfTheSecurity { get; set; }                           // Вид цінного паперу
        public string IsinCode { get; set; }                                    // Код ISIN
        public double? NominalValue { get; set; }                               // Номінальна вартість
        public double? Count { get; set; }                                      // Кількість
        public double? CapitalPercent { get; set; }                             // Відсоток акцій
    }
}
