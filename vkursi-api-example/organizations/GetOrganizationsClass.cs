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
            if (string.IsNullOrEmpty(token)) { 
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();}

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
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

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    class GetOrganizationsRequestBodyModel                          // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ
     /// </summary>
        public List<string> code = new List<string>();              // 
    }
    /// <summary>
    /// Модель відповіді GetOrganizations
    /// </summary>
    public class GetOrganizationsResponseModel                      // 
    {/// <summary>
     /// Системний Id організації
     /// </summary>
        public Guid Id { get; set; }                                // 
        /// <summary>
        /// Повна назва організації (maxLength:512)
        /// </summary>
        public string Name { get; set; }                            // 
        /// <summary>
        /// Скорочена назва організації (maxLength:512)
        /// </summary>
        public string ShortName { get; set; }                       // 
        /// <summary>
        /// Код ЄДРПОУ (maxLength:8)
        /// </summary>
        public string Edrpou { get; set; }                          // 
        /// <summary>
        /// ПІБ Керівника (maxLength:256)
        /// </summary>
        public string ChiefName { get; set; }                       // 
        /// <summary>
        /// Статус реєстрації
        /// </summary>
        public string State { get; set; }                           // 
        /// <summary>
        /// Дата реєстрації платником ПДВ
        /// </summary>
        public DateTime? DateRegInn { get; set; }                   // 
        /// <summary>
        /// Код ІПН (ПДВ) (maxLength:12)
        /// </summary>
        public string Inn { get; set; }                             // 
        /// <summary>
        /// Дата анулючання свідоцтва платника ПДВ
        /// </summary>
        public DateTime? DateCanceledInn { get; set; }              // 
        /// <summary>
        /// Наявний податковий борг (true - так / false - ні)
        /// </summary>
        public bool? HasBorg { get; set; }                          // 
        /// <summary>
        /// Наявні санкції (true - так / false - ні)
        /// </summary>
        public bool? InSanctions { get; set; }                      // 
        /// <summary>
        /// Наявні виконавчі провадження
        /// </summary>
        public int? Introduction { get; set; }                      // 
        /// <summary>
        /// Загальна кількість ризиків
        /// </summary>
        public int? ExpressScore { get; set; }                      // 
        /// <summary>
        /// Відомості про платника ЄП
        /// </summary>
        public SingleTaxPayer singleTaxPayer { get; set; }          // 
        /// <summary>
        /// Відомості про платника ЄП
        /// </summary>
        public class SingleTaxPayer                                 // 
        {/// <summary>
         /// Дата реєстрації платником ЄП
         /// </summary>
            public DateTime dateStart { get; set; }                 // 
            /// <summary>
            /// Ставка
            /// </summary>
            public double rate { get; set; }                        // 
            /// <summary>
            /// Група
            /// </summary>
            public int group { get; set; }                          // 
            /// <summary>
            /// Дата анулювання
            /// </summary>
            public DateTime? dateEnd { get; set; }                  // 
            /// <summary>
            /// Вид діяльності (maxLength:256)
            /// </summary>
            public string kindOfActivity { get; set; }              // 
            /// <summary>
            /// Статус (true - платник ЄП / false - не платник ЄП)
            /// </summary>
            public bool status { get; set; }                        // 
        }
    }
}
