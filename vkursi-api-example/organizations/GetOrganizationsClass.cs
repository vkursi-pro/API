using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrganizationsClass
    {
        /*
        
        2. Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ
        [POST] /api/1.0/organizations/getorganizations        
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorganizations' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"code": ["40073472"]}'

        */

        public static List<GetOrganizationsResponseModel> GetOrganizations(string code, ref string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString)) 
            {
                GetOrganizationsRequestBodyModel GORequestBody = new GetOrganizationsRequestBodyModel
                {
                    code = new List<string>
                    {
                        code                                                // Перелік кодів ЄДРПОУ
                    }
                };

                string body = JsonConvert.SerializeObject(GORequestBody);   // Example body: {"code": ["40073472"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorganizations");
                RestRequest request = new RestRequest(Method.POST);

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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            List<GetOrganizationsResponseModel> OrganizationsList = new List<GetOrganizationsResponseModel>();

            OrganizationsList = JsonConvert.DeserializeObject<List<GetOrganizationsResponseModel>>(responseString);

            return OrganizationsList;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"code\": [\"40073472\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getorganizations", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"code\": [\"40073472\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorganizations")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    class GetOrganizationsRequestBodyModel                          // Модель Body запиту
    {
        public List<string> code = new List<string>();              // Перелік кодів ЄДРПОУ
    }

    public class GetOrganizationsResponseModel                      // Модель відповіді GetOrganizations
    {
        public Guid Id { get; set; }                                // Системний Id організації
        public string Name { get; set; }                            // Повна назва організації
        public string ShortName { get; set; }                       // Скорочена назва організації
        public string Edrpou { get; set; }                          // Код ЄДРПОУ
        public string ChiefName { get; set; }                       // ПІБ Керівника
        public string State { get; set; }                           // Статус реєстрації
        public DateTime? DateRegInn { get; set; }                   // Дата реєстрації платником ПДВ
        public string Inn { get; set; }                             // Код ІПН (ПДВ)
        public DateTime? DateCanceledInn { get; set; }              // Дата анулючання свідоцтва платника ПДВ
        public bool? HasBorg { get; set; }                          // Наявний податковий борг (true - так / false - ні)
        public bool? InSanctions { get; set; }                      // Наявні санкції (true - так / false - ні)
        public int? Introduction { get; set; }                      // Наявні виконавчі провадження
        public int? ExpressScore { get; set; }                      // Загальна кількість ризиків
        public SingleTaxPayer singleTaxPayer { get; set; }          // Відомості про платника ЄП

        public class SingleTaxPayer                                 // Відомості про платника ЄП
        {
            public DateTime dateStart { get; set; }                 // Дата реєстрації платником ЄП
            public double rate { get; set; }                        // Ставка
            public int group { get; set; }                          // Група
            public object dateEnd { get; set; }                     // Дата анулювання
            public string kindOfActivity { get; set; }              // Вид діяльності
            public bool status { get; set; }                        // Статус (true - платник ЄП / false - не платник ЄП)
        }
    }
}
