using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetExpressScoreWeightClass
    {
        /*

        Метод:
            62. Отримання відомостей про вагу ризиків в Експрес оцінці
            [POST] /api/1.0/organizations/GetExpressScoreWeight

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetExpressScoreWeight' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsIn...' \
            --header 'Content-Type: application/json'

        Приклад відповіді:

        */

        public static GetExpressScoreWeightResponseModel GetExpressScoreWeight(ref string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetExpressScoreWeight");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
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

            GetExpressScoreWeightResponseModel GESWResponse = new GetExpressScoreWeightResponseModel();

            GESWResponse = JsonConvert.DeserializeObject<GetExpressScoreWeightResponseModel>(responseString);

            return GESWResponse;
        }
    }

    /// <summary>
    /// Модель відповіді GetExpressScoreWeight
    /// </summary>
    public class GetExpressScoreWeightResponseModel                                     // 
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
        public GetExpressScoreWeightRequestData Data { get; set; }                      // 
    }
    /// <summary>
    /// Дані методу
    /// </summary>
    public class GetExpressScoreWeightRequestData                                       // 
    {/// <summary>
    /// ???
    /// </summary>
        [JsonProperty("pdv")]
        public List<WeightList> Pdv { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("yedynyyPodatok")]
        public List<WeightList> YedynyyPodatok { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("opf")]
        public List<WeightList> Opf { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("dataReyestratsiyi")]
        public List<WeightList> DataReyestratsiyi { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("upovnovazheniOsoby")]
        public List<WeightList> UpovnovazheniOsoby { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("statutnyyKapital")]
        public List<WeightList> StatutnyyKapital { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("statusReyestratsiyi")]
        public List<WeightList> StatusReyestratsiyi { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("adresaOkupTerytoriyi")]
        public List<WeightList> AdresaOkupTerytoriyi { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("adresaMasReyestratsiyi")]
        public List<WeightList> AdresaMasReyestratsiyi { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("rezydentDerzhavyzSanktsiyamy")]
        public List<WeightList> RezydentDerzhavyzSanktsiyamy { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("obmezhPosadovykhOsib")]
        public List<WeightList> ObmezhPosadovykhOsib { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("novyyKerivnykPidpysant")]
        public List<WeightList> NovyyKerivnykPidpysant { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("sanktsiyiRnbo")]
        public List<WeightList> SanktsiyiRnbo { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("kintseviBenefitsiary")]
        public List<WeightList> KintseviBenefitsiary { get; set; }
        /// <summary>
        /// ???
        /// </summary>
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

    //public class WeightList
    //{
    //    [JsonProperty("indicatorValue")]
    //    public string IndicatorValue { get; set; }

    //    [JsonProperty("weight")]
    //    public long Weight { get; set; }
    //}
}
