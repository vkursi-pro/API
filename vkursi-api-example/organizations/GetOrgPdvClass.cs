using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOrgPdvClass
    {
        /*
        
        Метод:
            79. Реєстр платників ПДВ
            [POST] /api/1.0/organizations/GetOrgPdv

        cURL запиту:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgPdv' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1...' \
            --header 'Content-Type: application/json' \
            --data '{"Code":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetOrgPdvResponse.json
  
        */

        public static GetOrgPdvResponseModel GetOrgPdv(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOrgPdvRequestBodyModel GKPRBody = new GetOrgPdvRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GKPRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgPdv");
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
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetOrgPdvResponseModel GOPResponse = new GetOrgPdvResponseModel();

            GOPResponse = JsonConvert.DeserializeObject<GetOrgPdvResponseModel>(responseString);

            return GOPResponse;
        }
    }

    /*

    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Code": [
            "00131305"
          ]
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIU...',
          'Content-Type': 'application/json',
          'Cookie': 'TiPMix=55.11336512536021; x-ms-routing-name=self'
        }
        conn.request("POST", "/api/1.0/organizations/GetOrgPdv", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
    
    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"00131305\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgPdv")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIU...")
          .addHeader("Content-Type", "application/json")
          .addHeader("Cookie", "TiPMix=55.11336512536021; x-ms-routing-name=self")
          .build();
        Response response = client.newCall(request).execute();

    */



    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetOrgPdvRequestBodyModel                                          // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                      // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetOrgPdvResponseModel                                             // 
    {/// <summary>
     /// Статус запиту (maxLength:128)
     /// </summary>
        public string Status { get; set; }                                          // 
        /// <summary>
        /// Запит виконано успішно (true - так / false - ні)
        /// </summary>
        public bool IsSuccess { get; set; }                                         // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<OrgPdvInfo> Data { get; set; }                                  // 
    }/// <summary>
     /// Дані відповіді
     /// </summary>
    public class OrgPdvInfo                                                         // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }                                            // 
        /// <summary>
        /// Дата отримання свідоцтва ПДВ
        /// </summary>
        public DateTime? DateRegInn { get; set; }                                   // 
        /// <summary>
        /// ІПН
        /// </summary>
        public string Inn { get; set; }                                             // 
        /// <summary>
        /// Загальнмй статус (платник ПДВ / не платник ПДВ)
        /// </summary>
        public string Info { get; set; }                                            // 
        /// <summary>
        /// Перелік записів з реєстру платників ПДВ
        /// </summary>
        public List<VatPayersCabinetTaxUpdate> VatPayersCabinetTaxList { get; set; }
    }

    /// <summary>
    /// Перелік записів з реєстру платників ПДВ
    /// </summary>
    public class VatPayersCabinetTaxUpdate
    {
        /// <summary>
        /// Код ПДВ (текст)
        /// </summary>
        [JsonProperty("kodPdvs")]
        public string KodPdvs { get; set; }
        /// <summary>
        /// Код ЕДРПОУ (текст)
        /// </summary>
        [JsonProperty("tins")]
        public string Tins { get; set; }

        /// <summary>
        /// Назва суб'єкта господарювання
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Дата реєстрації платником ПДВ
        /// </summary>
        [JsonProperty("datReestr")]
        public DateTime? DatReestr { get; set; }
        /// <summary>
        /// Дата анулювання свідоцтва платника ПДВ
        /// </summary>
        [JsonProperty("datAnul")]
        public DateTime? DatAnul { get; set; }

        /// <summary>
        /// Дата реєстрації суб’єктом спецрежиму
        /// </summary>
        [JsonProperty("dreestrSg")]
        public DateTime? DreestrSg { get; set; }

        [JsonProperty("datSvd")]
        public DateTime? DatSvd { get; set; }

        /// <summary>
        /// Дата виключення з реєстру суб’єктів спецрежиму
        /// </summary>
        [JsonProperty("danulSg")]
        public DateTime? DanulSg { get; set; }

        /// <summary>
        /// Причина анулювання
        /// </summary>
        [JsonProperty("kodAnul")]
        public string KodAnul { get; set; }

        /// <summary>
        /// Підстава анулювання
        /// </summary>
        [JsonProperty("kodPid")]
        public string KodPid { get; set; }

        /// <summary>
        /// Код ПДВ (число)
        /// </summary>
        [JsonProperty("kodPdv")]
        public long? KodPdv { get; set; }

        /// <summary>
        /// Код ЕДРПОУ (число)
        /// </summary>
        [JsonProperty("tin")]
        public long? Tin { get; set; }

        [JsonProperty("datTerm")]
        public DateTime? DatTerm { get; set; }

        [JsonProperty("dpdvSg")]
        public DateTime? DpdvSg { get; set; }

        /// <summary>
        /// Системне поле
        /// </summary>
        public Guid Hash { get; set; }

        /// <summary>
        /// Системне поле
        /// </summary>
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Системне поле
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Системний ідентифікатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
