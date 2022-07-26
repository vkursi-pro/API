using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetRequisitesClass
    {
        /*

        Метод:
            66. Отримати дані реквізитів для строреня картки ФОП / ЮО
            [POST] /api/1.0/organizations/GetRequisites

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRequisites' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cC...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":["41462280"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRequisitesResponse.json

        */
        public static GetRequisitesResponseModel GetRequisites(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetRequisitesRequestBodyModel GRRequestBody = new GetRequisitesRequestBodyModel
                {
                    Code = new List<string> { code }                                    // Перелік кодів (обеження 100)
                };

                string body = JsonConvert.SerializeObject(GRRequestBody);           // Example Body: {"Code":["41462280"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRequisites");
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
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetRequisitesResponseModel GRResponse = new GetRequisitesResponseModel();

            GRResponse = JsonConvert.DeserializeObject<GetRequisitesResponseModel>(responseString);

            return GRResponse;
        }
    }


    /*
    
    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Code": [
            "41462280"
          ]
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5c...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetRequisites", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"41462280\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRequisites")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();
     
     */
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetRequisitesRequestBodyModel                                          // 
    {/// <summary>
     /// Перелік кодів (обеження 100)
     /// </summary>
        public List<string> Code { get; set; }                                          // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetRequisitesResponseModel                                             // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSucces { get; set; }                                              // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Перелік даних
        /// </summary>
        public List<GetRequisitesResponseData> Data { get; set; }                       // 
    }
    /// <summary>
    /// Перелік даних
    /// </summary>
    public class GetRequisitesResponseData                                              // 
    {/// <summary>
     /// Повна назва організації / ФОП (maxLength:512)
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Скорочена назва організації (maxLength:512)
        /// </summary>
        public string ShortName { get; set; }                                           // 
        /// <summary>
        /// Назва організації на англійський мові (maxLength:512)
        /// </summary>
        public string EngName { get; set; }                                             // 
        /// <summary>
        /// Організаційно-правова форма
        /// </summary>
        public string Opf { get; set; }                                                 // 
        /// <summary>
        /// Статус реєстрації суб'єкта господарювання
        /// </summary>
        public string SgdStatus { get; set; }                                           // 
        /// <summary>
        /// Адреса
        /// </summary>
        public string Address { get; set; }                                             // 
        /// <summary>
        /// КВЕД (основний)
        /// </summary>
        public string Kved { get; set; }                                                // 
        /// <summary>
        /// Керівник ПІБ
        /// </summary>
        public string Chief { get; set; }                                               // 
        /// <summary>
        /// Відомості про ПДВ
        /// </summary>
        public GetRequisitesRequestAnswerDataReestrPdv ReestrPdv { get; set; }          // 
        /// <summary>
        /// Відомості про анульоване ПДВ
        /// </summary>
        public GetRequisitesRequestAnswerDataReestrPdvCancel ReestrPdvCancel { get; set; }// 
        /// <summary>
        /// Відомості з реєстру платників єдиного податку
        /// </summary>
        public GetRequisitesRequestAnswerDataReestrPed ReestrPed { get; set; }          // 
        /// <summary>
        ///Відомості з реєстру неприбуткових установ 
        /// </summary>
        public NeprubutkovaUstanovaInfo ReestrNeprubutkovuhkUstanov { get; set; }       // 
        /// <summary>
        /// Адреса в окремих полях
        /// </summary>
        [JsonProperty("addressParts")]
        public AddressParts AddressParts { get; set; }                                  // 
    }
    /// <summary>
    /// Відомості про ПДВ
    /// </summary>
    public class GetRequisitesRequestAnswerDataReestrPdv                                // 
    {/// <summary>
     /// Статус платника ПДВ
     /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Дата реєстрації платником ПДВ
        /// </summary>
        public DateTime? RegDate { get; set; }                                          // 
        /// <summary>
        /// ІПН
        /// </summary>
        public string Ipn { get; set; }                                                 // 
    }/// <summary>
     /// Відомості про анульоване ПДВ
     /// </summary>
    public class GetRequisitesRequestAnswerDataReestrPdvCancel                          // 
    {/// <summary>
     /// Статус анулювання ПДВ
     /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Дата анулювання ПДВ
        /// </summary>
        public DateTime? CancelDate { get; set; }                                       // 
    }/// <summary>
     /// Відомості з реєстру платників єдиного податку (ЄП)
     /// </summary>
    public class GetRequisitesRequestAnswerDataReestrPed                                // 
    {/// <summary>
     /// Статус платника ЄП
     /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Дата включенння до реєстру
        /// </summary>
        public DateTime? DateStart { get; set; }                                        // 
        /// <summary>
        /// Дата виключення з реєстру
        /// </summary>
        public DateTime? DateEnd { get; set; }                                          // 
        /// <summary>
        /// Ставка
        /// </summary>
        public decimal? Stavka { get; set; }                                            // 
        /// <summary>
        /// Група
        /// </summary>
        public int? Grupa { get; set; }                                                 // 
    }
    /// <summary>
    /// Відомості з реєстру неприбуткових установ
    /// </summary>
    public class NeprubutkovaUstanovaInfo                                               // 
    {/// <summary>
     /// Дата включення до Реєстру
     /// </summary>
        [JsonProperty("DRegNoPr")]
        public string DRegNoPr { get; set; }                                            // 
        /// <summary>
        /// Код ознаки неприбутковості
        /// </summary>
        [JsonProperty("CNonpr")]
        public string CNonpr { get; set; }                                              // 
        /// <summary>
        /// Назва ознаки неприбутковості
        /// </summary>
        [JsonProperty("Nonpr")]
        public string Nonpr { get; set; }                                               // 
        /// <summary>
        /// Дата присвоєння ознаки або її зміни
        /// </summary>
        [JsonProperty("DNonpr")]
        public string DNonpr { get; set; }                                              // 
        /// <summary>
        /// Дата прийняття рішення контролюючим органом
        /// </summary>
        [JsonProperty("DRish")]
        public string DRish { get; set; }                                               // 
        /// <summary>
        /// Номер рішення контролюючого органу
        /// </summary>
        [JsonProperty("NRish")]
        public string NRish { get; set; }                                               // 
        /// <summary>
        /// Тип рішення
        /// </summary>
        [JsonProperty("TRish")]
        public string TRish { get; set; }                                               // 
        /// <summary>
        /// Код контролюючого органу, який прийняв рішення
        /// </summary>
        [JsonProperty("CSti")]
        public string CSti { get; set; }                                                // 
    }
    /// <summary>
    /// Адреса в окремих полях
    /// </summary>
    public partial class AddressParts                                                   // 
    {/// <summary>
     /// Область
     /// </summary>
        [JsonProperty("atu")]
        public string Atu { get; set; }                                                 // 
        /// <summary>
        /// Код КОАТУУ
        /// </summary>
        [JsonProperty("atu_code")]
        public string AtuCode { get; set; }                                             // 
        /// <summary>
        /// Назва вулиці
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }                                              // 
        /// <summary>
        /// Тип будівлі ('буд.', 'інше')
        /// </summary>
        [JsonProperty("house_type")]
        public string HouseType { get; set; }                                           // 
        /// <summary>
        /// Номер будинку, якщо тип - 'буд.'
        /// </summary>
        [JsonProperty("house")]
        public string House { get; set; }                                               // 
        /// <summary>
        /// Тип будівлі
        /// </summary>
        [JsonProperty("building_type")]
        public string BuildingType { get; set; }                                        // 
        /// <summary>
        /// Номер будівлі
        /// </summary>
        [JsonProperty("building")]
        public string Building { get; set; }                                            // 
        /// <summary>
        /// Тип приміщення
        /// </summary>
        [JsonProperty("num_type")]
        public string NumType { get; set; }                                             // 
        /// <summary>
        /// Номер приміщення
        /// </summary>
        [JsonProperty("num")]
        public string Num { get; set; }                                                 // 
    }
}
