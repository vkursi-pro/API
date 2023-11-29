using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetExpressScoreClass
    {
        /*

        60. Отримання відомостей по Експрес оцінку ризиків у ЮО, ФОП та ФО за ПІБ та кодом ІПН / ЄДРПОУ 
        [POST] /api/1.0/organizations/getExpressScore

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getExpressScore' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6I...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"searchType":1,"code":"32352162"}'
         
        */



        public static string GetExpressScoreTemp(string edrpou)
        {
            var client = new RestClient("https://89.184.66.70:7638/ExpressScore/GetExpressScore");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"UserId\": \"A98D501F-98F2-4497-B263-964BF5E04CDA\",\r\n    \"Request\": {\r\n        \"SearchType\": 1,\r\n        \"Name\": \"тищенко володимир сергійович\",\r\n        \"Code\": \"" + edrpou + "\"\r\n    }\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }



        public static GetExpressScoreResponseModel GetExpressScore(ref string token, int searchType, string code)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetExpressScoreRequestBodyModel GESRequestBody = new GetExpressScoreRequestBodyModel
                {
                    SearchType = searchType,                                            // Тип пошуку (1 - по ЮО / 2 - по ФО)
                    Code = code,                                                        // Код (ЄДРПОУ для ЮО / ІПН для ФО)
                    Name = null                                                         // ПІБ ФО (якщо searchType = 2 (якщо 1 не обов'язково))
                };

                string body = JsonConvert.SerializeObject(GESRequestBody);              // Example Body: {"searchType":1,"code":"32352162"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getExpressScore");
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

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetExpressScoreResponseModel GESResponse = new GetExpressScoreResponseModel();

            GESResponse = JsonConvert.DeserializeObject<GetExpressScoreResponseModel>(responseString);

            string test1 = JsonConvert.SerializeObject(GESResponse, Formatting.Indented);


            if (GESResponse.Status.Contains("Помилка"))
            {

            }
            else
            {
                Console.WriteLine(responseString);
            }

            string responseString2 = GetExpressScoreTemp(code);

            GetExpressScoreResponseModel GESResponse2 = new GetExpressScoreResponseModel();

            GESResponse2 = JsonConvert.DeserializeObject<GetExpressScoreResponseModel>(responseString2);

            string test2 = JsonConvert.SerializeObject(GESResponse2, Formatting.Indented);

            if (test1 == test2)
            {

            }


            return GESResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"searchType\":1,\"code\":\"32352162\"}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsIn...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getExpressScore", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"searchType\":1,\"code\":\"32352162\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getExpressScore")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель запиту (Example: {"searchType":1,"code":"32352162"})
    /// </summary>
    public class GetExpressScoreRequestBodyModel                                        // 
    {/// <summary>
     /// Тип пошуку (1 - по ЮО / 2 - по ФО) (обов'язковий)
     /// </summary>
        [JsonProperty("searchType")]
        public int SearchType { get; set; }                                             // 
        /// <summary>
        /// Код (ЄДРПОУ для ЮО / ІПН для ФО)
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }                                                // 
        /// <summary>
        /// ПІБ ФО (якщо searchType = 2 (якщо 1 не обов'язково))
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }                                                // 
    }

    /// <summary>
    /// Модель на відповідь GetExpressScore
    /// </summary>
    public class GetExpressScoreResponseModel                                           // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дані методу
        /// </summary>
        [JsonProperty("data")]
        public List<GetExpressScoreRequestData> Data { get; set; }                      // 
    }
    /// <summary>
    /// Дані методу
    /// </summary>
    public class GetExpressScoreRequestData                                             // 
    {/// <summary>
     /// Назва
     /// </summary>
        [JsonProperty("nazva")]
        public string Nazva { get; set; }                                               // 
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        [JsonProperty("edppou")]
        public string Edppou { get; set; }                                              // 
        /// <summary>
        /// Платник ПДВ
        /// </summary>
        [JsonProperty("pdv")]
        public ExpressScore Pdv { get; set; }                                           // 
        /// <summary>
        /// Група єдиного податку
        /// </summary>
        [JsonProperty("yedynyyPodatok")]
        public ExpressScore YedynyyPodatok { get; set; }                                // 
        /// <summary>
        /// Організаційно-правова форма
        /// </summary>
        [JsonProperty("opf")]
        public ExpressScore Opf { get; set; }                                           // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        [JsonProperty("dataReyestratsiyi")]
        public ExpressScore DataReyestratsiyi { get; set; }                             // 
        /// <summary>
        /// Уповноважені особи (керівники, підписанти)
        /// </summary>
        [JsonProperty("statutnyyKapital")]
        public ExpressScore StatutnyyKapital { get; set; }                              // 
        /// <summary>
        /// Статус реєстрації
        /// </summary>
        [JsonProperty("statusReyestratsiyi")]
        public ExpressScore StatusReyestratsiyi { get; set; }                           // 
        /// <summary>
        /// Розмір статутного капіталу
        /// </summary>
        [JsonProperty("bankrutsvo")]
        public ExpressScore Bankrutsvo { get; set; }                                    // 
        /// <summary>
        /// Місцезнаходження на тимчасово окупованій території
        /// </summary>
        [JsonProperty("adresaOkupTerytoriyi")]
        public ExpressScore AdresaOkupTerytoriyi { get; set; }                          // 
        /// <summary>
        /// Адреса, що належить до адрес масової реєстрації
        /// </summary>
        [JsonProperty("adresaMasReyestratsiyi")]
        public ExpressScore AdresaMasReyestratsiyi { get; set; }                        // 
        /// <summary>
        /// Засновник / учасник / кінцевий бенефіціар – резидент держави, що знаходиться під санкціями
        /// </summary>
        [JsonProperty("rezydentDerzhavyzSanktsiyamy")]
        public ExpressScore RezydentDerzhavyzSanktsiyamy { get; set; }                  // 
        /// <summary>
        /// Санкції РНБО (Ради національної безпеки і оборони України)
        /// </summary>
        [JsonProperty("sanktsiyiRnbo")]
        public ExpressScore SanktsiyiRnbo { get; set; }                                 // 
        /// <summary>
        /// Санкційний список Міністерства Фінансів США (SDN List)
        /// </summary>
        [JsonProperty("sanktsiyiSdn")]
        public ExpressScore SanktsiyiSdn { get; set; }                                  // 
        /// <summary>
        /// Чорний список АМКУ
        /// </summary>
        [JsonProperty("sanktsiyiAmku")]
        public ExpressScore SanktsiyiAmku { get; set; }                                 // 
        /// <summary>
        /// Санкційний список ЄС
        /// </summary>
        [JsonProperty("sanktsiyiYes")]
        public ExpressScore SanktsiyiYes { get; set; }                                  // 
        /// <summary>
        /// Санкційний список Великобританії
        /// </summary>
        [JsonProperty("sanktsiyiVb")]
        public ExpressScore SanktsiyiVb { get; set; }                                   // 
        /// <summary>
        /// Спеціальні санкції (МЕРТ)
        /// </summary>
        [JsonProperty("sanktsiyiMert")]
        public ExpressScore SanktsiyiMert { get; set; }                                 // 
        /// <summary>
        /// Згадується у кримінальних справах
        /// </summary>
        [JsonProperty("kryminalniSpravavy")]
        public ExpressScore KryminalniSpravavy { get; set; }                            // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("podatkoviSporiryDps")]   
        public ExpressScore PodatkoviSporiryDps { get; set; }                           // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("hospodarskiSpravy")]
        public ExpressScore HospodarskiSpravy { get; set; }                             // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("administratyvipravy")]
        public ExpressScore Administratyvipravy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("vidkrytiVp")]
        public ExpressScore VidkrytiVp { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("drorm")]
        public ExpressScore Drorm { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("obtyazhennyaNerukhMayna")]
        public ExpressScore ObtyazhennyaNerukhMayna { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("ahrarnіRozpysky")]
        public ExpressScore AhrarnіRozpysky { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("reyestrBorzhnykiv")]
        public ExpressScore ReyestrBorzhnykiv { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("borgZarPlati")]
        public ExpressScore BorgZarPlati { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("podatkovyyBorh")]
        public ExpressScore PodatkovyyBorh { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("podatkovyyBorhMisyatsiv")]
        public ExpressScore PodatkovyyBorhMisyatsiv { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("anulPdv")]
        public ExpressScore AnulPdv { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("zvyazkyKompbankrutamy")]
        public ExpressScore ZvyazkyKompbankrutamy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("zvyazkyPubDiyachamy")]
        public ExpressScore ZvyazkyPubDiyachamy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("operatsiyizOfshoramy")]
        public ExpressScore OperatsiyizOfshoramy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("operatsiyizSanktKrayinamy")]
        public ExpressScore OperatsiyizSanktKrayinamy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("deklaratsiyProDokhody")]
        public ExpressScore DeklaratsiyProDokhody { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("reyestrTerorystiv")]
        public ExpressScore ReyestrTerorystiv { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("totalStatus")]
        public ExpressScore TotalStatus { get; set; }
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ExpressScore
    {/// <summary>
    /// ???
    /// </summary>
        [JsonProperty("indicatorValue")]
        public string IndicatorValue { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public int? Weight { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("indicatorValueList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ExpressScore> IndicatorValueList { get; set; }
    }
}