using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetAnketaClass
    {
        /*
        
        68

        */

        public static GetAnketaResponseModel GetAnketa(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetAnketaRequestBodyModel GARBody = new GetAnketaRequestBodyModel
                {
                    Code = new List<string> { edrpou }, 
                };

                string body = JsonConvert.SerializeObject(GARBody);


                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetAnketa");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetAnketaResponseModel GetAnketaResponseRow = new GetAnketaResponseModel();

            GetAnketaResponseRow = JsonConvert.DeserializeObject<GetAnketaResponseModel>(responseString);

            return GetAnketaResponseRow;
        }
    }

    public class GetAnketaRequestBodyModel
    {
        public List<string> Code { get; set; }
    }

    public class GetAnketaResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public List<ApiOrganizationAnketa> Data { get; set; }
    }

    public class ApiOrganizationAnketa
    {
        [JsonProperty("kod")]
        public string Kod { get; set; }
        [JsonProperty("nazva")]
        public string Nazva { get; set; }
        [JsonProperty("stan")]
        public string Stan { get; set; }
        [JsonProperty("formaVlasnosti")]
        public string FormaVlasnosti { get; set; }
        public string Opf { get; set; }
        [JsonProperty("kved")]
        public string Kved { get; set; }
        [JsonProperty("ktUpovnOsoby")]
        public int? KtUpovnOsoby { get; set; }
        [JsonProperty("ktZasnovnykiv")]
        public int? KtZasnovnykiv { get; set; }
        [JsonProperty("ktBenefitsiariv")]
        public int? KtBenefitsiariv { get; set; }
        [JsonProperty("kodPdv")]
        public string KodPdv { get; set; }
        [JsonProperty("dataReyestrPdv")]
        public DateTime? DataReyestrPdv { get; set; }
        [JsonProperty("dataAnulPdv")]
        public DateTime? DataAnulPdv { get; set; }
        [JsonProperty("dokhid")]
        public List<RikSuma> Dokhid { get; set; }
        [JsonProperty("ktPratsivnykiv")]
        public string KtPratsivnykiv { get; set; }
        [JsonProperty("email")]
        public List<string> Email { get; set; }
        [JsonProperty("nomerTelefony")]
        public List<string> NomerTelefony { get; set; }
        public string Capital { get; set; }
        [JsonProperty("zvyazky")]
        public List<GetRelationApiModelAnswerData> Zvyazky { get; set; }
        public List<AktsiyModel> Aktsiy { get; set; }
    }
    public class RikSuma
    {
        public int Rik { get; set; }
        public double Suma { get; set; }
    }
    public class AktsiyModel
    {
        public string VydDeponenta { get; set; }
        public string VydTsinnohoPaperu { get; set; }
        public string KodIsin { get; set; }
        public string NominalVartist { get; set; }
        public string KilkistAktsiy { get; set; }
        public string VidsotokAktsiy { get; set; }
        public DateTime? DataZvitnogoPeriodu { get; set; }
        public string EdrpouEmitenta { get; set; }
        public string NaymenuvannyaEmitenta { get; set; }
        public string EdrpouVlasnika { get; set; }
        public string NaymenuvannyaVlasnika { get; set; }
        public string SkorocheneNaymenuvannyaimya { get; set; }
        public string IdentifikatsiyniyKod { get; set; }
        public string CountryNameUa { get; set; }
    }
}
