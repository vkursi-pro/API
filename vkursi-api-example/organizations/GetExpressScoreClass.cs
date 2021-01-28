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

    public class GetExpressScoreRequestBodyModel                                        // Модель запиту (Example: {"searchType":1,"code":"32352162"})
    {
        [JsonProperty("searchType")]
        public int SearchType { get; set; }                                             // Тип пошуку (1 - по ЮО / 2 - по ФО) (обов'язковий)

        [JsonProperty("code")]
        public string Code { get; set; }                                                // Код (ЄДРПОУ для ЮО / ІПН для ФО)

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }                                                // ПІБ ФО (якщо searchType = 2 (якщо 1 не обов'язково))
    }


    public class GetExpressScoreResponseModel                                           // Модель на відповідь GetExpressScore
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }                                             // Чи успішний запит

        [JsonProperty("status")]
        public string Status { get; set; }                                              // Статус відповіді по API

        [JsonProperty("data")]
        public List<GetExpressScoreRequestData> Data { get; set; }                      // Дані методу
    }

    public class GetExpressScoreRequestData                                             // Дані методу
    {
        [JsonProperty("nazva")]
        public string Nazva { get; set; }                                               // Назва

        [JsonProperty("edppou")]
        public string Edppou { get; set; }                                              // Код ЄДРПОУ / ІПН

        [JsonProperty("pdv")]
        public ExpressScore Pdv { get; set; }                                           // Платник ПДВ

        [JsonProperty("yedynyyPodatok")]
        public ExpressScore YedynyyPodatok { get; set; }                                // Група єдиного податку

        [JsonProperty("opf")]
        public ExpressScore Opf { get; set; }                                           // Організаційно-правова форма

        [JsonProperty("dataReyestratsiyi")]
        public ExpressScore DataReyestratsiyi { get; set; }                             // Дата реєстрації

        [JsonProperty("statutnyyKapital")]
        public ExpressScore StatutnyyKapital { get; set; }                              // Уповноважені особи (керівники, підписанти)

        [JsonProperty("statusReyestratsiyi")]
        public ExpressScore StatusReyestratsiyi { get; set; }                           // Статус реєстрації

        [JsonProperty("bankrutsvo")]
        public ExpressScore Bankrutsvo { get; set; }                                    // Розмір статутного капіталу

        [JsonProperty("adresaOkupTerytoriyi")]
        public ExpressScore AdresaOkupTerytoriyi { get; set; }                          // Місцезнаходження на тимчасово окупованій території

        [JsonProperty("adresaMasReyestratsiyi")]
        public ExpressScore AdresaMasReyestratsiyi { get; set; }                        // Адреса, що належить до адрес масової реєстрації

        [JsonProperty("rezydentDerzhavyzSanktsiyamy")]
        public ExpressScore RezydentDerzhavyzSanktsiyamy { get; set; }                  // Засновник / учасник / кінцевий бенефіціар – резидент держави, що знаходиться під санкціями

        [JsonProperty("sanktsiyiRnbo")]
        public ExpressScore SanktsiyiRnbo { get; set; }                                 // Санкції РНБО (Ради національної безпеки і оборони України)

        [JsonProperty("sanktsiyiSdn")]
        public ExpressScore SanktsiyiSdn { get; set; }                                  // Санкційний список Міністерства Фінансів США (SDN List)

        [JsonProperty("sanktsiyiAmku")]
        public ExpressScore SanktsiyiAmku { get; set; }                                 // Чорний список АМКУ

        [JsonProperty("sanktsiyiYes")]
        public ExpressScore SanktsiyiYes { get; set; }                                  // Санкційний список ЄС

        [JsonProperty("sanktsiyiVb")]
        public ExpressScore SanktsiyiVb { get; set; }                                   // Санкційний список Великобританії

        [JsonProperty("sanktsiyiMert")]
        public ExpressScore SanktsiyiMert { get; set; }                                 // Спеціальні санкції (МЕРТ)

        [JsonProperty("kryminalniSpravavy")]
        public ExpressScore KryminalniSpravavy { get; set; }                            // Згадується у кримінальних справах

        [JsonProperty("podatkoviSporiryDps")]   
        public ExpressScore PodatkoviSporiryDps { get; set; }                           // 

        [JsonProperty("hospodarskiSpravy")]
        public ExpressScore HospodarskiSpravy { get; set; }                             // 

        [JsonProperty("administratyvipravy")]
        public ExpressScore Administratyvipravy { get; set; }

        [JsonProperty("vidkrytiVp")]
        public ExpressScore VidkrytiVp { get; set; }

        [JsonProperty("drorm")]
        public ExpressScore Drorm { get; set; }

        [JsonProperty("obtyazhennyaNerukhMayna")]
        public ExpressScore ObtyazhennyaNerukhMayna { get; set; }

        [JsonProperty("ahrarnіRozpysky")]
        public ExpressScore AhrarnіRozpysky { get; set; }

        [JsonProperty("reyestrBorzhnykiv")]
        public ExpressScore ReyestrBorzhnykiv { get; set; }

        [JsonProperty("borgZarPlati")]
        public ExpressScore BorgZarPlati { get; set; }

        [JsonProperty("podatkovyyBorh")]
        public ExpressScore PodatkovyyBorh { get; set; }

        [JsonProperty("podatkovyyBorhMisyatsiv")]
        public ExpressScore PodatkovyyBorhMisyatsiv { get; set; }

        [JsonProperty("anulPdv")]
        public ExpressScore AnulPdv { get; set; }

        [JsonProperty("zvyazkyKompbankrutamy")]
        public ExpressScore ZvyazkyKompbankrutamy { get; set; }

        [JsonProperty("zvyazkyPubDiyachamy")]
        public ExpressScore ZvyazkyPubDiyachamy { get; set; }

        [JsonProperty("operatsiyizOfshoramy")]
        public ExpressScore OperatsiyizOfshoramy { get; set; }

        [JsonProperty("operatsiyizSanktKrayinamy")]
        public ExpressScore OperatsiyizSanktKrayinamy { get; set; }

        [JsonProperty("deklaratsiyProDokhody")]
        public ExpressScore DeklaratsiyProDokhody { get; set; }

        [JsonProperty("reyestrTerorystiv")]
        public ExpressScore ReyestrTerorystiv { get; set; }

        [JsonProperty("totalStatus")]
        public ExpressScore TotalStatus { get; set; }
    }

    public class ExpressScore
    {
        [JsonProperty("indicatorValue")]
        public string IndicatorValue { get; set; }

        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public int? Weight { get; set; }

        [JsonProperty("indicatorValueList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ExpressScore> IndicatorValueList { get; set; }
    }
}