using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.organizations;

namespace vkursi_api_example.token
{
    public class GetTariffClass
    {
        /*
        
        27. Обьем ресурсів доспупних користувачу відповідно до тарифного плану
        [GET] /api/1.0/token/gettariff

        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/token/gettariff' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...'

        */

        public static GetTariffResponseModel GetTariff(string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/token/gettariff");
                RestRequest request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", "Bearer " + token);

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

            GetTariffResponseModel GTResponse = new GetTariffResponseModel();

            GTResponse = JsonConvert.DeserializeObject<GetTariffResponseModel>(responseString);

            return GTResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = ''
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("GET", "/api/1.0/token/gettariff", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/token/gettariff")
          .method("GET", null)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetTariffResponseModel
    {
        public string tariffName { get; set; }
        public string tariff { get; set; }
        public string dossierLeftName { get; set; }
        public int dossierLeft { get; set; }
        public string dossierRightName { get; set; }
        public int dossierRight { get; set; }
        public string extendedDossierLeftName { get; set; }
        public int extendedDossierLeft { get; set; }
        public string extendedDossierRightName { get; set; }
        public int extendedDossierRight { get; set; }
        public string monitoringLeftName { get; set; }
        public int monitoringLeft { get; set; }
        public string monitoringRightName { get; set; }
        public int monitoringRight { get; set; }
        public string shortApiLeftName { get; set; }
        public int shortApiLeft { get; set; }
        public string shortApiRightName { get; set; }
        public int shortApiRight { get; set; }
        public string extendedApiLeftName { get; set; }
        public int expandedApiLeft { get; set; }
        public string extendedApiRightName { get; set; }
        public int expandedApiRight { get; set; }
        public string securityLeftName { get; set; }
        public int securityLeft { get; set; }
        public string securityRightName { get; set; }
        public int securityRight { get; set; }
        public string commonAccessLeftName { get; set; }
        public int commonAccessLeft { get; set; }
        public string commonAccessRightName { get; set; }
        public int commonAccessRight { get; set; }
        public string walletName { get; set; }
        public double walletRight { get; set; }
        public string nextUpdateDateName { get; set; }
        public string nextUpdateDat { get; set; }
        public string endDateTariffName { get; set; }
        public string endDateTariff { get; set; }
    }
}
