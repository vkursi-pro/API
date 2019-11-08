using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.organizations;

namespace vkursi_api_example.estate
{
    public class GetEstateByCodeClass
    {
        // 5.	Нерухомість по ФОП або ЮО
        // [GET] /api/1.0/estate/getestatebycode

        public static GetRealEstateRightsResponseModel GetRealEstateRights(string code, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }

            link1:

            var client = new RestClient("https://vkursi-api.azurewebsites.net");
            var request = new RestRequest("api/1.0/estate/getestatebycode", Method.GET);

            request.AddParameter("code", code);
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            var responseString = response.Content;

            if (responseString == "Not found")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            GetRealEstateRightsResponseModel realEstateDeserialize = JsonConvert.DeserializeObject<GetRealEstateRightsResponseModel>(responseString);

            return realEstateDeserialize;
        }
    }

    public class GetRealEstateRightsResponseModel
    {
        public EstateTotalApi Total { get; set; }
        public List<EstateApi> Estates { get; set; }
        public string Code { get; set; }
    }

    public class EstateTotalApi
    {
        public int LandsCount { get; set; }
        public int HousesCount { get; set; }
        public List<EstateTypeTotal> TypeCount { get; set; }
        public List<EstateTypeTotal> GlobalTypeCount { get; set; }
    }

    public class EstateTypeTotal
    {
        public int Type { get; set; }
        public int Count { get; set; }
    }

    public class EstateApi
    {
        public Guid Id { get; set; }
        public string EstateObjectName { get; set; }
        public EstateCoordinates Location { get; set; }
        public bool Land { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime DateModified { get; set; }
        public List<int> TypeArray { get; set; }
        public List<int> GlobalTypeArray { get; set; }
        public DetailsJObjectEstate DetailedCadastrInfo { get; set; }
        public int? CourtCount { get; set; }
    }
    public class EstateCoordinates
    {
        public decimal Longtitude { get; set; }
        public decimal Latitude { get; set; }
    }
    public class DetailsJObjectEstate
    {
        public long? koatuu { get; set; }
        public int? zona { get; set; }
        public int? kvartal { get; set; }
        public int? parcel { get; set; }
        public string cadnum { get; set; }
        public int? ownershipcode { get; set; }
        public string purpose { get; set; }
        public string use { get; set; }
        public string area { get; set; }
        public string unit_area { get; set; }
        public string ownershipvalue { get; set; }
        public int? id_office { get; set; }
        public string region { get; set; }
        public string district { get; set; }
    }
}
