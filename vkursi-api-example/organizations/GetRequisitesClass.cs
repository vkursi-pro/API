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

    public class GetRequisitesRequestBodyModel                                          // Модель запиту 
    {
        public List<string> Code { get; set; }                                          // Перелік кодів (обеження 100)
    }

    public class GetRequisitesResponseModel                                             // Модель на відповідь
    {
        public bool IsSucces { get; set; }                                              // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public List<GetRequisitesResponseData> Data { get; set; }                       // Перелік даних
    }

    public class GetRequisitesResponseData                                              // Перелік даних
    {
        public string Name { get; set; }                                                // Повна назва організації / ФОП (maxLength:512)
        public string ShortName { get; set; }                                           // Скорочена назва організації (maxLength:512)
        public string EngName { get; set; }                                             // Назва організації на англійський мові (maxLength:512)
        public string Opf { get; set; }                                                 // Організаційно-правова форма
        public string SgdStatus { get; set; }                                           // Статус реєстрації суб'єкта господарювання
        public string Address { get; set; }                                             // Адреса
        public string Kved { get; set; }                                                // КВЕД (основний)
        public string Chief { get; set; }                                               // Керівник ПІБ
        public GetRequisitesRequestAnswerDataReestrPdv ReestrPdv { get; set; }          // Відомості про ПДВ
        public GetRequisitesRequestAnswerDataReestrPdvCancel ReestrPdvCancel { get; set; }// Відомості про анульоване ПДВ
        public GetRequisitesRequestAnswerDataReestrPed ReestrPed { get; set; }          // Відомості з реєстру платників єдиного податку
        public NeprubutkovaUstanovaInfo ReestrNeprubutkovuhkUstanov { get; set; }       // Відомості з реєстру неприбуткових установ

        [JsonProperty("addressParts")]
        public AddressParts AddressParts { get; set; }                                  // Адреса в окремих полях
    }

    public class GetRequisitesRequestAnswerDataReestrPdv                                // Відомості про ПДВ
    {
        public string State { get; set; }                                               // Статус платника ПДВ
        public DateTime? RegDate { get; set; }                                          // Дата реєстрації платником ПДВ
        public string Ipn { get; set; }                                                 // ІПН
    }
    public class GetRequisitesRequestAnswerDataReestrPdvCancel                          // Відомості про анульоване ПДВ
    {
        public string State { get; set; }                                               // Статус анулювання ПДВ
        public DateTime? CancelDate { get; set; }                                       // Дата анулювання ПДВ
    }
    public class GetRequisitesRequestAnswerDataReestrPed                                // Відомості з реєстру платників єдиного податку (ЄП)
    {
        public string State { get; set; }                                               // Статус платника ЄП
        public DateTime? DateStart { get; set; }                                        // Дата включенння до реєстру
        public DateTime? DateEnd { get; set; }                                          // Дата виключення з реєстру
        public decimal? Stavka { get; set; }                                            // Ставка
        public int? Grupa { get; set; }                                                 // Група
    }

    public class NeprubutkovaUstanovaInfo                                               // Відомості з реєстру неприбуткових установ
    {
        [JsonProperty("DRegNoPr")]
        public string DRegNoPr { get; set; }                                            // Дата включення до Реєстру

        [JsonProperty("CNonpr")]
        public string CNonpr { get; set; }                                              // Код ознаки неприбутковості

        [JsonProperty("Nonpr")]
        public string Nonpr { get; set; }                                               // Назва ознаки неприбутковості

        [JsonProperty("DNonpr")]
        public string DNonpr { get; set; }                                              // Дата присвоєння ознаки або її зміни

        [JsonProperty("DRish")]
        public string DRish { get; set; }                                               // Дата прийняття рішення контролюючим органом

        [JsonProperty("NRish")]
        public string NRish { get; set; }                                               // Номер рішення контролюючого органу

        [JsonProperty("TRish")]
        public string TRish { get; set; }                                               // Тип рішення

        [JsonProperty("CSti")]
        public string CSti { get; set; }                                                // Код контролюючого органу, який прийняв рішення
    }

    public partial class AddressParts                                                   // Адреса в окремих полях
    {
        [JsonProperty("atu")]
        public string Atu { get; set; }                                                 // Область

        [JsonProperty("atu_code")]
        public string AtuCode { get; set; }                                             // Код КОАТУУ

        [JsonProperty("street")]
        public string Street { get; set; }                                              // Назва вулиці

        [JsonProperty("house_type")]
        public string HouseType { get; set; }                                           // Тип будівлі ('буд.', 'інше')

        [JsonProperty("house")]
        public string House { get; set; }                                               // Номер будинку, якщо тип - 'буд.'

        [JsonProperty("building_type")]
        public string BuildingType { get; set; }                                        // Тип будівлі

        [JsonProperty("building")]
        public string Building { get; set; }                                            // Номер будівлі

        [JsonProperty("num_type")]
        public string NumType { get; set; }                                             // Тип приміщення

        [JsonProperty("num")]
        public string Num { get; set; }                                                 // Номер приміщення
    }
}
