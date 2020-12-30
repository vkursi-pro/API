using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class EditExpressScoreWeightClass
    {

        /*

        Метод:
            61. Редагування відомостей вагу ризиків в Експрес оцінці
            [POST] /api/1.0/organizations/editexpressscoreweight         

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/EditExpressScoreWeight' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIs...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"borgZarPlati":[{"indicatorValue":">=200000","weight":1},{"indicatorValue":">100000#=<200000","weight":2},{"indicatorValue":"=<100000","weight":3}],"vidkrytiVp":[{"indicatorValue":">=5","weight":1},{"indicatorValue":">1#<5","weight":2},{"indicatorValue":"=0","weight":3}]}'

        Приклад відповіді: 

            {
                "success": true,
                "status": "Json збережно"
            }

        */

        public static EditExpressScoreWeightResponseModel EditExpressScoreWeight(ref string token, string jsonStr)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                string body = jsonStr;  // Example body: {\"borgZarPlati\":[{\"indicatorValue\":\">=200000\",\"weight\":1},{\"indicatorValue\":\">100000#=<200000\",\"weight\":2},{\"indicatorValue\":\"=<100000\",\"weight\":3}],\"vidkrytiVp\":[{\"indicatorValue\":\">=5\",\"weight\":1},{\"indicatorValue\":\">1#<5\",\"weight\":2},{\"indicatorValue\":\"=0\",\"weight\":3}]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/EditExpressScoreWeight");
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

            EditExpressScoreWeightResponseModel EESWResponse = new EditExpressScoreWeightResponseModel();

            EESWResponse = JsonConvert.DeserializeObject<EditExpressScoreWeightResponseModel>(responseString);

            return EESWResponse;
        }
    }

    /*

    // Python - http.client example:

        import http.client

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "[{\"borgZarPlati\":[{\"indicatorValue\":\">=200000\",\"weight\":1},{\"indicatorValue\":\">100000#=<200000\",\"weight\":2},{\"indicatorValue\":\"=<100000\",\"weight\":3}],\"vidkrytiVp\":[{\"indicatorValue\":\">=5\",\"weight\":1},{\"indicatorValue\":\">1#<5\",\"weight\":2},{\"indicatorValue\":\"=0\",\"weight\":3}],}]"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIs...',
          'Content-Type': 'application/json',
          'Cookie': 'TiPMix=86.9081103647631; x-ms-routing-name=self'
        }
        conn.request("POST", "/api/1.0/organizations/EditExpressScoreWeight", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


    // Java - OkHttp example:   
    
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "[{\"borgZarPlati\":[{\"indicatorValue\":\">=200000\",\"weight\":1},{\"indicatorValue\":\">100000#=<200000\",\"weight\":2},{\"indicatorValue\":\"=<100000\",\"weight\":3}],\"vidkrytiVp\":[{\"indicatorValue\":\">=5\",\"weight\":1},{\"indicatorValue\":\">1#<5\",\"weight\":2},{\"indicatorValue\":\"=0\",\"weight\":3}],}]");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/EditExpressScoreWeight")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs")
          .addHeader("Content-Type", "application/json")
          .addHeader("Cookie", "TiPMix=86.9081103647631; x-ms-routing-name=self")
          .build();
        Response response = client.newCall(request).execute();

    */


    public class EditExpressScoreWeightRequestBodyModel                 // Модель Body запиту EditExpressScoreWeight
    {
        [JsonProperty("pdv")]
        public List<WeightList> Pdv { get; set; }

        [JsonProperty("yedynyyPodatok")]
        public List<WeightList> YedynyyPodatok { get; set; }

        [JsonProperty("opf")]
        public List<WeightList> Opf { get; set; }

        [JsonProperty("dataReyestratsiyi")]
        public List<WeightList> DataReyestratsiyi { get; set; }

        [JsonProperty("upovnovazheniOsoby")]
        public List<WeightList> UpovnovazheniOsoby { get; set; }

        [JsonProperty("statutnyyKapital")]
        public List<WeightList> StatutnyyKapital { get; set; }

        [JsonProperty("statusReyestratsiyi")]
        public List<WeightList> StatusReyestratsiyi { get; set; }

        [JsonProperty("adresaOkupTerytoriyi")]
        public List<WeightList> AdresaOkupTerytoriyi { get; set; }

        [JsonProperty("adresaMasReyestratsiyi")]
        public List<WeightList> AdresaMasReyestratsiyi { get; set; }

        [JsonProperty("rezydentDerzhavyzSanktsiyamy")]
        public List<WeightList> RezydentDerzhavyzSanktsiyamy { get; set; }

        [JsonProperty("obmezhPosadovykhOsib")]
        public List<WeightList> ObmezhPosadovykhOsib { get; set; }

        [JsonProperty("novyyKerivnykPidpysant")]
        public List<WeightList> NovyyKerivnykPidpysant { get; set; }

        [JsonProperty("sanktsiyiRnbo")]
        public List<WeightList> SanktsiyiRnbo { get; set; }

        [JsonProperty("kintseviBenefitsiary")]
        public List<WeightList> KintseviBenefitsiary { get; set; }

        [JsonProperty("sanktsiyiSdn")]
        public List<WeightList> SanktsiyiSdn { get; set; }

        [JsonProperty("sanktsiyiAmku")]
        public List<WeightList> SanktsiyiAmku { get; set; }

        [JsonProperty("sanktsiyiYes")]
        public List<WeightList> SanktsiyiYes { get; set; }

        [JsonProperty("sanktsiyiVb")]
        public List<WeightList> SanktsiyiVb { get; set; }

        [JsonProperty("sanktsiyiMert")]
        public List<WeightList> SanktsiyiMert { get; set; }

        [JsonProperty("kryminalniSpravavy")]
        public List<WeightList> KryminalniSpravavy { get; set; }

        [JsonProperty("kryminalniSpravavySt199")]
        public List<WeightList> KryminalniSpravavySt199 { get; set; }

        [JsonProperty("kryminalniSpravavySt200")]
        public List<WeightList> KryminalniSpravavySt200 { get; set; }

        [JsonProperty("kryminalniSpravavySt201")]
        public List<WeightList> KryminalniSpravavySt201 { get; set; }

        [JsonProperty("kryminalniSpravavySt205p1")]
        public List<WeightList> KryminalniSpravavySt205P1 { get; set; }

        [JsonProperty("kryminalniSpravavySt206")]
        public List<WeightList> KryminalniSpravavySt206 { get; set; }

        [JsonProperty("kryminalniSpravavySt206p2")]
        public List<WeightList> KryminalniSpravavySt206P2 { get; set; }

        [JsonProperty("kryminalniSpravavySt209")]
        public List<WeightList> KryminalniSpravavySt209 { get; set; }

        [JsonProperty("kryminalniSpravavySt209p1")]
        public List<WeightList> KryminalniSpravavySt209P1 { get; set; }

        [JsonProperty("kryminalniSpravavySt212")]
        public List<WeightList> KryminalniSpravavySt212 { get; set; }

        [JsonProperty("kryminalniSpravavySt212p1")]
        public List<WeightList> KryminalniSpravavySt212P1 { get; set; }

        [JsonProperty("kryminalniSpravavySt222")]
        public List<WeightList> KryminalniSpravavySt222 { get; set; }

        [JsonProperty("kryminalniSpravavySt222p1")]
        public List<WeightList> KryminalniSpravavySt222P1 { get; set; }

        [JsonProperty("kryminalniSpravavySt222p2")]
        public List<WeightList> KryminalniSpravavySt222P2 { get; set; }

        [JsonProperty("kryminalniSpravavySt224")]
        public List<WeightList> KryminalniSpravavySt224 { get; set; }

        [JsonProperty("kryminalniSpravavySt231")]
        public List<WeightList> KryminalniSpravavySt231 { get; set; }

        [JsonProperty("kryminalniSpravavySt232")]
        public List<WeightList> KryminalniSpravavySt232 { get; set; }

        [JsonProperty("kryminalniSpravavySt232p1")]
        public List<WeightList> KryminalniSpravavySt232P1 { get; set; }

        [JsonProperty("kryminalniSpravavySt232p2")]
        public List<WeightList> KryminalniSpravavySt232P2 { get; set; }

        [JsonProperty("kryminalniSpravavySt233")]
        public List<WeightList> KryminalniSpravavySt233 { get; set; }

        [JsonProperty("kryminalniSpravavySt258p5")]
        public List<WeightList> KryminalniSpravavySt258P5 { get; set; }

        [JsonProperty("podatkoviSporiryDps")]
        public List<WeightList> PodatkoviSporiryDps { get; set; }

        [JsonProperty("hospodarskiSpravy")]
        public List<WeightList> HospodarskiSpravy { get; set; }

        [JsonProperty("administratyvipravy")]
        public List<WeightList> Administratyvipravy { get; set; }

        [JsonProperty("vidkrytiVp")]
        public List<WeightList> VidkrytiVp { get; set; }

        [JsonProperty("drorm")]
        public List<WeightList> Drorm { get; set; }

        [JsonProperty("obtyazhennyaNerukhMayna")]
        public List<WeightList> ObtyazhennyaNerukhMayna { get; set; }

        [JsonProperty("ahrarnіRozpysky")]
        public List<WeightList> AhrarnіRozpysky { get; set; }

        [JsonProperty("reyestrBorzhnykiv")]
        public List<WeightList> ReyestrBorzhnykiv { get; set; }

        [JsonProperty("borgZarPlati")]
        public List<WeightList> BorgZarPlati { get; set; }

        [JsonProperty("podatkovyyBorh")]
        public List<WeightList> PodatkovyyBorh { get; set; }

        [JsonProperty("podatkovyyBorhMisyatsiv")]
        public List<WeightList> PodatkovyyBorhMisyatsiv { get; set; }

        [JsonProperty("anulPdv")]
        public List<WeightList> AnulPdv { get; set; }

        [JsonProperty("zvyazkyKompbankrutamy")]
        public List<WeightList> ZvyazkyKompbankrutamy { get; set; }

        [JsonProperty("zvyazkyPubDiyachamy")]
        public List<WeightList> ZvyazkyPubDiyachamy { get; set; }

        [JsonProperty("operatsiyizOfshoramy")]
        public List<WeightList> OperatsiyizOfshoramy { get; set; }

        [JsonProperty("operatsiyizSanktKrayinamy")]
        public List<WeightList> OperatsiyizSanktKrayinamy { get; set; }

        [JsonProperty("deklaratsiyProDokhody")]
        public List<WeightList> DeklaratsiyProDokhody { get; set; }

        [JsonProperty("reyestrTerorystiv")]
        public List<WeightList> ReyestrTerorystiv { get; set; }

        [JsonProperty("totalStatus")]
        public List<WeightList> TotalStatus { get; set; }
    }

    public partial class WeightList
    {
        [JsonProperty("indicatorValue")]
        public string IndicatorValue { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }
    }

    public partial class EditExpressScoreWeightResponseModel                    // Модель відповіді EditExpressScoreWeight
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
