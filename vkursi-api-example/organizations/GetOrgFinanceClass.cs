using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetOrgFinanceClass
    {
        /*

        57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
        [POST] api/1.0/organizations/GetOrgFinance

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":["00131305"]}'

        */

        public static GetOrgFinanceResponseModel GetOrgFinance(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance");
                RestRequest request = new RestRequest(Method.POST);

                GetOrgFinanceRequestBodyModel GOFRequesRow = new GetOrgFinanceRequestBodyModel
                {
                    Code = new List<string> {
                        edrpou
                    }
                };

                string body = JsonConvert.SerializeObject(GOFRequesRow); // Example: {"Code":["00131305"]}

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

            GetOrgFinanceResponseModel GOFResponseRow = new GetOrgFinanceResponseModel();

            GOFResponseRow = JsonConvert.DeserializeObject<GetOrgFinanceResponseModel>(responseString);

            return GOFResponseRow;
        }
    }
    /*

        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":[\"41462280\"]}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ik...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetOrgFinance", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"41462280\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVC...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetOrgFinanceRequestBodyModel      // Модель запиту 
    {
        public List<string> Code { get; set; }      // Перелік кодів ЄДРПОУ (обмеження 1)
    }

    public class GetOrgFinanceResponseModel         // Відповідь на запит
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }


    public partial class Datum
    {
        [JsonProperty("date_of_request")]
        public DateTime DateOfRequest { get; set; }

        [JsonProperty("name")]
        public long? Name { get; set; }

        [JsonProperty("age")]
        public List<Age> Age { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("likvid")]
        public string Likvid { get; set; }

        [JsonProperty("main_active")]
        public List<YearSum> MainActive { get; set; }

        [JsonProperty("actives")]
        public List<YearSum> Actives { get; set; }

        [JsonProperty("current_liabilities")]
        public List<YearSum> CurrentLiabilities { get; set; }

        [JsonProperty("net_income")]
        public List<YearSum> NetIncome { get; set; }

        [JsonProperty("net_profit")]
        public List<YearSum> NetProfit { get; set; }

        [JsonProperty("balance_small")]
        public List<Balance> BalanceSmall { get; set; }

        [JsonProperty("balance_normal")]
        public List<Balance> BalanceNormal { get; set; }

        [JsonProperty("tend_num")]
        public List<YearNum> TendNum { get; set; }

        [JsonProperty("tend_sum")]
        public List<YearSum> TendSum { get; set; }

        [JsonProperty("tend_num_solo")]
        public List<YearNum> TendNumSolo { get; set; }

        [JsonProperty("tend_percent_solo")]
        public List<YearPerc> TendPercentSolo { get; set; }

        [JsonProperty("tend_zamov_num")]
        public List<YearNum> TendZamovNum { get; set; }

        [JsonProperty("tend_zamov_sum")]
        public List<YearSum> TendZamovSum { get; set; }

        [JsonProperty("tend_zamov_num_solo")]
        public List<YearNum> TendZamovNumSolo { get; set; }

        [JsonProperty("tend_zamov_percent_solo")]
        public List<YearPerc> TendZamovPercentSolo { get; set; }

        [JsonProperty("exp_num")]
        public List<YearNum> ExpNum { get; set; }

        [JsonProperty("exp_sum")]
        public List<YearSum> ExpSum { get; set; }

        [JsonProperty("exp_group_num")]
        public List<YearNum> ExpGroupNum { get; set; }

        [JsonProperty("exp_biggest_group_percent")]
        public List<YearPerc> ExpBiggestGroupPercent { get; set; }

        [JsonProperty("imp_num")]
        public List<YearNum> ImpNum { get; set; }

        [JsonProperty("imp_sum")]
        public List<YearSum> ImpSum { get; set; }

        [JsonProperty("imp_group_num")]
        public List<YearPerc> ImpGroupNum { get; set; }

        [JsonProperty("imp_biggest_group_percent")]
        public List<YearPerc> ImpBiggestGroupPercent { get; set; }
    }

    public partial class YearSum
    {
        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("sum")]
        public double Sum { get; set; }
    }

    public partial class Age
    {
        [JsonProperty("age_text")]
        public string AgeText { get; set; }

        [JsonProperty("age_year")]
        public long? AgeYear { get; set; }

        [JsonProperty("age_month")]
        public long? AgeMonth { get; set; }
    }

    public partial class YearPerc
    {
        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("perc")]
        public double Perc { get; set; }
    }

    public partial class YearNum
    {
        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("num")]
        public long? Num { get; set; }
    }

    public partial class Balance
    {
        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("code")]
        public long? Code { get; set; }

        [JsonProperty("D1_1000_01")]
        public double? D11000_01 { get; set; }

        [JsonProperty("D1_1000_02")]
        public double? D11000_02 { get; set; }

        [JsonProperty("D1_1001_01")]
        public double? D11001_01 { get; set; }

        [JsonProperty("D1_1001_02")]
        public double? D11001_02 { get; set; }

        [JsonProperty("D1_1002_01")]
        public double? D11002_01 { get; set; }

        [JsonProperty("D1_1002_02")]
        public double? D11002_02 { get; set; }

        [JsonProperty("D1_1005_01")]
        public double? D11005_01 { get; set; }

        [JsonProperty("D1_1005_02")]
        public double? D11005_02 { get; set; }

        [JsonProperty("D1_1010_01")]
        public double? D11010_01 { get; set; }

        [JsonProperty("D1_1010_02")]
        public double? D11010_02 { get; set; }

        [JsonProperty("D1_1011_01")]
        public double? D11011_01 { get; set; }

        [JsonProperty("D1_1011_02")]
        public double? D11011_02 { get; set; }

        [JsonProperty("D1_1012_01")]
        public double? D11012_01 { get; set; }

        [JsonProperty("D1_1012_02")]
        public double? D11012_02 { get; set; }

        [JsonProperty("D1_1015_01")]
        public double? D11015_01 { get; set; }

        [JsonProperty("D1_1015_02")]
        public double? D11015_02 { get; set; }

        [JsonProperty("D1_1016_01")]
        public double? D11016_01 { get; set; }

        [JsonProperty("D1_1016_02")]
        public double? D11016_02 { get; set; }

        [JsonProperty("D1_1017_01")]
        public double? D11017_01 { get; set; }

        [JsonProperty("D1_1017_02")]
        public double? D11017_02 { get; set; }

        [JsonProperty("D1_1020_01")]
        public double? D11020_01 { get; set; }

        [JsonProperty("D1_1020_02")]
        public double? D11020_02 { get; set; }

        [JsonProperty("D1_1021_01")]
        public double? D11021_01 { get; set; }

        [JsonProperty("D1_1021_02")]
        public double? D11021_02 { get; set; }

        [JsonProperty("D1_1022_01")]
        public double? D11022_01 { get; set; }

        [JsonProperty("D1_1022_02")]
        public double? D11022_02 { get; set; }

        [JsonProperty("D1_1030_01")]
        public double? D11030_01 { get; set; }

        [JsonProperty("D1_1030_02")]
        public double? D11030_02 { get; set; }

        [JsonProperty("D1_1035_01")]
        public double? D11035_01 { get; set; }

        [JsonProperty("D1_1035_02")]
        public double? D11035_02 { get; set; }

        [JsonProperty("D1_1040_01")]
        public double? D11040_01 { get; set; }

        [JsonProperty("D1_1040_02")]
        public double? D11040_02 { get; set; }

        [JsonProperty("D1_1045_01")]
        public double? D11045_01 { get; set; }

        [JsonProperty("D1_1045_02")]
        public double? D11045_02 { get; set; }

        [JsonProperty("D1_1050_01")]
        public double? D11050_01 { get; set; }

        [JsonProperty("D1_1050_02")]
        public double? D11050_02 { get; set; }

        [JsonProperty("D1_1060_01")]
        public double? D11060_01 { get; set; }

        [JsonProperty("D1_1060_02")]
        public double? D11060_02 { get; set; }

        [JsonProperty("D1_1065_01")]
        public double? D11065_01 { get; set; }

        [JsonProperty("D1_1065_02")]
        public double? D11065_02 { get; set; }

        [JsonProperty("D1_1090_01")]
        public double? D11090_01 { get; set; }

        [JsonProperty("D1_1090_02")]
        public double? D11090_02 { get; set; }

        [JsonProperty("D1_1095_01")]
        public double? D11095_01 { get; set; }

        [JsonProperty("D1_1095_02")]
        public double? D11095_02 { get; set; }

        [JsonProperty("D1_1100_01")]
        public double? D11100_01 { get; set; }

        [JsonProperty("D1_1100_02")]
        public double? D11100_02 { get; set; }

        [JsonProperty("D1_1101_01")]
        public double? D11101_01 { get; set; }

        [JsonProperty("D1_1101_02")]
        public double? D11101_02 { get; set; }

        [JsonProperty("D1_1102_01")]
        public double? D11102_01 { get; set; }

        [JsonProperty("D1_1102_02")]
        public double? D11102_02 { get; set; }

        [JsonProperty("D1_1103_01")]
        public double? D11103_01 { get; set; }

        [JsonProperty("D1_1103_02")]
        public double? D11103_02 { get; set; }

        [JsonProperty("D1_1104_01")]
        public double? D11104_01 { get; set; }

        [JsonProperty("D1_1104_02")]
        public double? D11104_02 { get; set; }

        [JsonProperty("D1_1110_01")]
        public double? D11110_01 { get; set; }

        [JsonProperty("D1_1110_02")]
        public double? D11110_02 { get; set; }

        [JsonProperty("D1_1115_01")]
        public double? D11115_01 { get; set; }

        [JsonProperty("D1_1115_02")]
        public double? D11115_02 { get; set; }

        [JsonProperty("D1_1120_01")]
        public double? D11120_01 { get; set; }

        [JsonProperty("D1_1120_02")]
        public double? D11120_02 { get; set; }

        [JsonProperty("D1_1125_01")]
        public double? D11125_01 { get; set; }

        [JsonProperty("D1_1125_02")]
        public double? D11125_02 { get; set; }

        [JsonProperty("D1_1130_01")]
        public double? D11130_01 { get; set; }

        [JsonProperty("D1_1130_02")]
        public double? D11130_02 { get; set; }

        [JsonProperty("D1_1135_01")]
        public double? D11135_01 { get; set; }

        [JsonProperty("D1_1135_02")]
        public double? D11135_02 { get; set; }

        [JsonProperty("D1_1136_01")]
        public double? D11136_01 { get; set; }

        [JsonProperty("D1_1136_02")]
        public double? D11136_02 { get; set; }

        [JsonProperty("D1_1140_01")]
        public double? D11140_01 { get; set; }

        [JsonProperty("D1_1140_02")]
        public double? D11140_02 { get; set; }

        [JsonProperty("D1_1145_01")]
        public double? D11145_01 { get; set; }

        [JsonProperty("D1_1145_02")]
        public double? D11145_02 { get; set; }

        [JsonProperty("D1_1155_01")]
        public double? D11155_01 { get; set; }

        [JsonProperty("D1_1155_02")]
        public double? D11155_02 { get; set; }

        [JsonProperty("D1_1160_01")]
        public double? D11160_01 { get; set; }

        [JsonProperty("D1_1160_02")]
        public double? D11160_02 { get; set; }

        [JsonProperty("D1_1165_01")]
        public double? D11165_01 { get; set; }

        [JsonProperty("D1_1165_02")]
        public double? D11165_02 { get; set; }

        [JsonProperty("D1_1166_01")]
        public double? D11166_01 { get; set; }

        [JsonProperty("D1_1166_02")]
        public double? D11166_02 { get; set; }

        [JsonProperty("D1_1167_01")]
        public double? D11167_01 { get; set; }

        [JsonProperty("D1_1167_02")]
        public double? D11167_02 { get; set; }

        [JsonProperty("D1_1170_01")]
        public double? D11170_01 { get; set; }

        [JsonProperty("D1_1170_02")]
        public double? D11170_02 { get; set; }

        [JsonProperty("D1_1180_01")]
        public double? D11180_01 { get; set; }

        [JsonProperty("D1_1180_02")]
        public double? D11180_02 { get; set; }

        [JsonProperty("D1_1181_01")]
        public double? D11181_01 { get; set; }

        [JsonProperty("D1_1181_02")]
        public double? D11181_02 { get; set; }

        [JsonProperty("D1_1182_01")]
        public double? D11182_01 { get; set; }

        [JsonProperty("D1_1182_02")]
        public double? D11182_02 { get; set; }

        [JsonProperty("D1_1183_01")]
        public double? D11183_01 { get; set; }

        [JsonProperty("D1_1183_02")]
        public double? D11183_02 { get; set; }

        [JsonProperty("D1_1184_01")]
        public double? D11184_01 { get; set; }

        [JsonProperty("D1_1184_02")]
        public double? D11184_02 { get; set; }

        [JsonProperty("D1_1190_01")]
        public double? D11190_01 { get; set; }

        [JsonProperty("D1_1190_02")]
        public double? D11190_02 { get; set; }

        [JsonProperty("D1_1195_01")]
        public double? D11195_01 { get; set; }

        [JsonProperty("D1_1195_02")]
        public double? D11195_02 { get; set; }

        [JsonProperty("D1_1200_01")]
        public double? D11200_01 { get; set; }

        [JsonProperty("D1_1200_02")]
        public double? D11200_02 { get; set; }

        [JsonProperty("D1_1300_01")]
        public double? D11300_01 { get; set; }

        [JsonProperty("D1_1300_02")]
        public double? D11300_02 { get; set; }

        [JsonProperty("D1_1400_01")]
        public double? D11400_01 { get; set; }

        [JsonProperty("D1_1400_02")]
        public double? D11400_02 { get; set; }

        [JsonProperty("D1_1401_01")]
        public double? D11401_01 { get; set; }

        [JsonProperty("D1_1401_02")]
        public double? D11401_02 { get; set; }

        [JsonProperty("D1_1405_01")]
        public double? D11405_01 { get; set; }

        [JsonProperty("D1_1405_02")]
        public double? D11405_02 { get; set; }

        [JsonProperty("D1_1410_01")]
        public double? D11410_01 { get; set; }

        [JsonProperty("D1_1410_02")]
        public double? D11410_02 { get; set; }

        [JsonProperty("D1_1411_01")]
        public double? D11411_01 { get; set; }

        [JsonProperty("D1_1411_02")]
        public double? D11411_02 { get; set; }

        [JsonProperty("D1_1412_01")]
        public double? D11412_01 { get; set; }

        [JsonProperty("D1_1412_02")]
        public double? D11412_02 { get; set; }

        [JsonProperty("D1_1415_01")]
        public double? D11415_01 { get; set; }

        [JsonProperty("D1_1415_02")]
        public double? D11415_02 { get; set; }

        [JsonProperty("D1_1420_01")]
        public double? D11420_01 { get; set; }

        [JsonProperty("D1_1420_02")]
        public double? D11420_02 { get; set; }

        [JsonProperty("D1_1425_01")]
        public double? D11425_01 { get; set; }

        [JsonProperty("D1_1425_02")]
        public double? D11425_02 { get; set; }

        [JsonProperty("D1_1430_01")]
        public double? D11430_01 { get; set; }

        [JsonProperty("D1_1430_02")]
        public double? D11430_02 { get; set; }

        [JsonProperty("D1_1435_01")]
        public double? D11435_01 { get; set; }

        [JsonProperty("D1_1435_02")]
        public double? D11435_02 { get; set; }

        [JsonProperty("D1_1495_01")]
        public double? D11495_01 { get; set; }

        [JsonProperty("D1_1495_02")]
        public double? D11495_02 { get; set; }

        [JsonProperty("D1_1500_01")]
        public double? D11500_01 { get; set; }

        [JsonProperty("D1_1500_02")]
        public double? D11500_02 { get; set; }

        [JsonProperty("D1_1505_01")]
        public double? D11505_01 { get; set; }

        [JsonProperty("D1_1505_02")]
        public double? D11505_02 { get; set; }

        [JsonProperty("D1_1510_01")]
        public double? D11510_01 { get; set; }

        [JsonProperty("D1_1510_02")]
        public double? D11510_02 { get; set; }

        [JsonProperty("D1_1515_01")]
        public double? D11515_01 { get; set; }

        [JsonProperty("D1_1515_02")]
        public double? D11515_02 { get; set; }

        [JsonProperty("D1_1520_01")]
        public double? D11520_01 { get; set; }

        [JsonProperty("D1_1520_02")]
        public double? D11520_02 { get; set; }

        [JsonProperty("D1_1525_01")]
        public double? D11525_01 { get; set; }

        [JsonProperty("D1_1525_02")]
        public double? D11525_02 { get; set; }

        [JsonProperty("D1_1526_01")]
        public double? D11526_01 { get; set; }

        [JsonProperty("D1_1526_02")]
        public double? D11526_02 { get; set; }

        [JsonProperty("D1_1530_01")]
        public double? D11530_01 { get; set; }

        [JsonProperty("D1_1530_02")]
        public double? D11530_02 { get; set; }

        [JsonProperty("D1_1531_01")]
        public double? D11531_01 { get; set; }

        [JsonProperty("D1_1531_02")]
        public double? D11531_02 { get; set; }

        [JsonProperty("D1_1532_01")]
        public double? D11532_01 { get; set; }

        [JsonProperty("D1_1532_02")]
        public double? D11532_02 { get; set; }

        [JsonProperty("D1_1533_01")]
        public double? D11533_01 { get; set; }

        [JsonProperty("D1_1533_02")]
        public double? D11533_02 { get; set; }

        [JsonProperty("D1_1534_01")]
        public double? D11534_01 { get; set; }

        [JsonProperty("D1_1534_02")]
        public double? D11534_02 { get; set; }

        [JsonProperty("D1_1535_01")]
        public double? D11535_01 { get; set; }

        [JsonProperty("D1_1535_02")]
        public double? D11535_02 { get; set; }

        [JsonProperty("D1_1540_01")]
        public double? D11540_01 { get; set; }

        [JsonProperty("D1_1540_02")]
        public double? D11540_02 { get; set; }

        [JsonProperty("D1_1545_01")]
        public double? D11545_01 { get; set; }

        [JsonProperty("D1_1545_02")]
        public double? D11545_02 { get; set; }

        [JsonProperty("D1_1595_01")]
        public double? D11595_01 { get; set; }

        [JsonProperty("D1_1595_02")]
        public double? D11595_02 { get; set; }

        [JsonProperty("D1_1600_01")]
        public double? D11600_01 { get; set; }

        [JsonProperty("D1_1600_02")]
        public double? D11600_02 { get; set; }

        [JsonProperty("D1_1605_01")]
        public double? D11605_01 { get; set; }

        [JsonProperty("D1_1605_02")]
        public double? D11605_02 { get; set; }

        [JsonProperty("D1_1610_01")]
        public double? D11610_01 { get; set; }

        [JsonProperty("D1_1610_02")]
        public double? D11610_02 { get; set; }

        [JsonProperty("D1_1615_01")]
        public double? D11615_01 { get; set; }

        [JsonProperty("D1_1615_02")]
        public double? D11615_02 { get; set; }

        [JsonProperty("D1_1620_01")]
        public double? D11620_01 { get; set; }

        [JsonProperty("D1_1620_02")]
        public double? D11620_02 { get; set; }

        [JsonProperty("D1_1621_01")]
        public double? D11621_01 { get; set; }

        [JsonProperty("D1_1621_02")]
        public double? D11621_02 { get; set; }

        [JsonProperty("D1_1625_01")]
        public double? D11625_01 { get; set; }

        [JsonProperty("D1_1625_02")]
        public double? D11625_02 { get; set; }

        [JsonProperty("D1_1630_01")]
        public double? D11630_01 { get; set; }

        [JsonProperty("D1_1630_02")]
        public double? D11630_02 { get; set; }

        [JsonProperty("D1_1635_01")]
        public double? D11635_01 { get; set; }

        [JsonProperty("D1_1635_02")]
        public double? D11635_02 { get; set; }

        [JsonProperty("D1_1640_01")]
        public double? D11640_01 { get; set; }

        [JsonProperty("D1_1640_02")]
        public double? D11640_02 { get; set; }

        [JsonProperty("D1_1645_01")]
        public double? D11645_01 { get; set; }

        [JsonProperty("D1_1645_02")]
        public double? D11645_02 { get; set; }

        [JsonProperty("D1_1650_01")]
        public double? D11650_01 { get; set; }

        [JsonProperty("D1_1650_02")]
        public double? D11650_02 { get; set; }

        [JsonProperty("D1_1660_01")]
        public double? D11660_01 { get; set; }

        [JsonProperty("D1_1660_02")]
        public double? D11660_02 { get; set; }

        [JsonProperty("D1_1665_01")]
        public double? D11665_01 { get; set; }

        [JsonProperty("D1_1665_02")]
        public double? D11665_02 { get; set; }

        [JsonProperty("D1_1670_01")]
        public double? D11670_01 { get; set; }

        [JsonProperty("D1_1670_02")]
        public double? D11670_02 { get; set; }

        [JsonProperty("D1_1690_01")]
        public double? D11690_01 { get; set; }

        [JsonProperty("D1_1690_02")]
        public double? D11690_02 { get; set; }

        [JsonProperty("D1_1695_01")]
        public double? D11695_01 { get; set; }

        [JsonProperty("D1_1695_02")]
        public double? D11695_02 { get; set; }

        [JsonProperty("D1_1700_01")]
        public double? D11700_01 { get; set; }

        [JsonProperty("D1_1700_02")]
        public double? D11700_02 { get; set; }

        [JsonProperty("D1_1800_01")]
        public double? D11800_01 { get; set; }

        [JsonProperty("D1_1800_02")]
        public double? D11800_02 { get; set; }

        [JsonProperty("D1_1900_01")]
        public double? D11900_01 { get; set; }

        [JsonProperty("D1_1900_02")]
        public double? D11900_02 { get; set; }

        [JsonProperty("D2_2000_01")]
        public double? D22000_01 { get; set; }

        [JsonProperty("D2_2010_01")]
        public double? D22010_01 { get; set; }

        [JsonProperty("D2_2011_01")]
        public double? D22011_01 { get; set; }

        [JsonProperty("D2_2012_01")]
        public double? D22012_01 { get; set; }

        [JsonProperty("D2_2013_01")]
        public double? D22013_01 { get; set; }

        [JsonProperty("D2_2014_01")]
        public double? D22014_01 { get; set; }

        [JsonProperty("D2_2050_01")]
        public double? D22050_01 { get; set; }

        [JsonProperty("D2_2070_01")]
        public double? D22070_01 { get; set; }

        [JsonProperty("D2_2090_01")]
        public double? D22090_01 { get; set; }

        [JsonProperty("D2_2095_01")]
        public double? D22095_01 { get; set; }

        [JsonProperty("D2_2111_01")]
        public double? D22111_01 { get; set; }

        [JsonProperty("D2_2112_01")]
        public double? D22112_01 { get; set; }

        [JsonProperty("D2_2120_01")]
        public double? D22120_01 { get; set; }

        [JsonProperty("D2_2121_01")]
        public double? D22121_01 { get; set; }

        [JsonProperty("D2_2122_01")]
        public double? D22122_01 { get; set; }

        [JsonProperty("D2_2130_01")]
        public double? D22130_01 { get; set; }

        [JsonProperty("D2_2150_01")]
        public double? D22150_01 { get; set; }

        [JsonProperty("D2_2180_01")]
        public double? D22180_01 { get; set; }

        [JsonProperty("D2_2181_01")]
        public double? D22181_01 { get; set; }

        [JsonProperty("D2_2182_01")]
        public double? D22182_01 { get; set; }

        [JsonProperty("D2_2190_01")]
        public double? D22190_01 { get; set; }

        [JsonProperty("D2_2195_01")]
        public double? D22195_01 { get; set; }

        [JsonProperty("D2_2200_01")]
        public double? D22200_01 { get; set; }

        [JsonProperty("D2_2220_01")]
        public double? D22220_01 { get; set; }

        [JsonProperty("D2_2240_01")]
        public double? D22240_01 { get; set; }

        [JsonProperty("D2_2241_01")]
        public double? D22241_01 { get; set; }

        [JsonProperty("D2_2250_01")]
        public double? D22250_01 { get; set; }

        [JsonProperty("D2_2255_01")]
        public double? D22255_01 { get; set; }

        [JsonProperty("D2_2270_01")]
        public double? D22270_01 { get; set; }

        [JsonProperty("D2_2275_01")]
        public double? D22275_01 { get; set; }

        [JsonProperty("D2_2290_01")]
        public double? D22290_01 { get; set; }

        [JsonProperty("D2_2295_01")]
        public double? D22295_01 { get; set; }

        [JsonProperty("D2_2300_01")]
        public double? D22300_01 { get; set; }

        [JsonProperty("D2_2305_01")]
        public double? D22305_01 { get; set; }

        [JsonProperty("D2_2350_01")]
        public double? D22350_01 { get; set; }

        [JsonProperty("D2_2355_01")]
        public double? D22355_01 { get; set; }

        [JsonProperty("D2_2400_01")]
        public double? D22400_01 { get; set; }

        [JsonProperty("D2_2405_01")]
        public double? D22405_01 { get; set; }

        [JsonProperty("D2_2410_01")]
        public double? D22410_01 { get; set; }

        [JsonProperty("D2_2415_01")]
        public double? D22415_01 { get; set; }

        [JsonProperty("D2_2445_01")]
        public double? D22445_01 { get; set; }

        [JsonProperty("D2_2450_01")]
        public double? D22450_01 { get; set; }

        [JsonProperty("D2_2455_01")]
        public double? D22455_01 { get; set; }

        [JsonProperty("D2_2460_01")]
        public double? D22460_01 { get; set; }

        [JsonProperty("D2_2465_01")]
        public double? D22465_01 { get; set; }

        [JsonProperty("D2_2500_01")]
        public double? D22500_01 { get; set; }

        [JsonProperty("D2_2505_01")]
        public double? D22505_01 { get; set; }

        [JsonProperty("D2_2510_01")]
        public double? D22510_01 { get; set; }

        [JsonProperty("D2_2515_01")]
        public double? D22515_01 { get; set; }

        [JsonProperty("D2_2520_01")]
        public double? D22520_01 { get; set; }

        [JsonProperty("D2_2550_01")]
        public double? D22550_01 { get; set; }


        [JsonProperty("D2_2160_01")]
        public double? D22160_01 { get; set; }

        [JsonProperty("D2_2165_01")]
        public double? D22165_01 { get; set; }

        [JsonProperty("D2_2280_01")]
        public double? D22280_01 { get; set; }

        [JsonProperty("D2_2285_01")]
        public double? D22285_01 { get; set; }

        [JsonProperty("D2_2310_01")]
        public double? D22310_01 { get; set; }
    }
}
