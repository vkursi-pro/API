using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using System.Threading;

namespace vkursi_api_example.estate
{
    public class GetEstatesClass
    {
        /*
        
        24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ
        [POST] /api/1.0/Estate/GetEstates

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstates' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["36679626"],"Ipn":["3006679626"]}'

        */
        public static GetEstatesResponseModel GetEstates(string token, string edrpou, string ipn)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetEstatesRequestBodyModel GERequestBodyRow = new GetEstatesRequestBodyModel
                {
                    Edrpou = new List<string> {
                        edrpou                                                  // Масив кодів ЄДРПОУ (обеження 1)
                    }
                    //Ipn = new List<string> {
                    //    ipn                                                   // Масив кодів ІПН (обеження 1)
                    //}
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestates");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GERequestBodyRow);    // Example Body: {"Edrpou":["26444836"]}

                //body = "{\"Edrpou\":[\"26444836\"],\"EstateType\": 11}";

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
                // Якшо є інформація що обробка запиту почалась робимо помторний запит через 15 сек
                else if ((int)response.StatusCode == 200 && responseString.Contains("Почалася обробка даних, спробуйте їх отримати пізніше"))
                {
                    responseString = null;
                    Thread.Sleep(15000);
                }

                // Почалася обробка даних, спробуйте їх отримати пізніше
            }

            GetEstatesResponseModel GEResponseRow = new GetEstatesResponseModel();

            GEResponseRow = JsonConvert.DeserializeObject<GetEstatesResponseModel>(responseString);

            return GEResponseRow;
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"3006679626\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/Estate/GetEstates", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:
        
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"3006679626\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/Estate/GetEstates")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetEstatesRequestBodyModel                                     // 
    {/// <summary>
     /// Масив кодів ЄДРПОУ (обеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                // 
        /// <summary>
        /// Масив кодів ІПН (обеження 1)
        /// </summary>
        public List<string> Ipn { get; set; }                                   // 
    }

    /// <summary>
    /// Модель відповіді GetEstates
    /// </summary>
    public class GetEstatesResponseModel                                        // 
    {/// <summary>
     /// Успішний запит (true - так / false - ні)
     /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// Статус запиту (maxLength:256)
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// Дані
        /// </summary>
        public GetEstateApiFreeModelAnswerData data { get; set; }               // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class GetEstateApiFreeModelAnswerData                                // 
    {/// <summary>
     /// Оригінал відповіді від ДП Nais (1в1). https://nais.gov.ua/files/general/2019/07/30/20190730154716-22.docx (maxLength:1000000)
     /// </summary>
        public string dataObjectOriginal { get; set; }                          //                            // 
        /// <summary>
        /// Витяг (Оригінальні дані відповіді Nais перетворені в об'єкт). https://nais.gov.ua/files/general/2019/07/30/20190730154716-22.docx
        /// </summary>
        public EstateResponceWithParsedResultData dataObject { get; set; }      //    // 
        /// <summary>
        /// ???
        /// </summary>
        public Dictionary<string, List<int>> types { get; set; }                // 
    }/// <summary>
     /// Витяг (Оригінальні дані відповіді Nais перетворені в об'єкт). https://nais.gov.ua/files/general/2019/07/30/20190730154716-22.docx
     /// </summary>
    public class EstateResponceWithParsedResultData                             //    // 
    {/// <summary>
     /// Системна інформація Nais (maxLength:128)
     /// </summary>
        public string entity { get; set; }                                      // 
        /// <summary>
        /// Системна інформація Nais. Назва методу (maxLength:128)
        /// </summary>
        public string method { get; set; }                                      // 
        /// <summary>
        /// Системна інформація Nais. Підпис ЕЦП (maxLength:2048)
        /// </summary>
        public string sign { get; set; }                                        // 
        /// <summary>
        /// Системна інформація Nais. Параметри пошуку (за якими була знайдені об'єкти)
        /// </summary>
        public SearchParams searchParams { get; set; }                          // 
        /// <summary>
        /// Системна інформація Nais. 
        /// </summary>
        public int resultID { get; set; }                                       // 
        /// <summary>
        /// ???
        /// </summary>
        public ResponseEstateData resultData { get; set; }                      // 
        /// <summary>
        /// Системна інформація Nais. 
        /// </summary>
        public long reportResultID { get; set; }                                // 
        /// <summary>
        /// Системна інформація Nais. 
        /// </summary>
        public int groupID { get; set; }                                        // 
    }/// <summary>
     /// Системна інформація Nais. Параметри пошуку (за якими була знайдені об'єкти)
     /// </summary>
    public class SearchParams                                                   // 
    {/// <summary>
     /// Відображати історичність назв
     /// </summary>
        public bool isShowHistoricalNames { get; set; }                         // 
        /// <summary>
        /// Для пошуку по об’єкту ="1" для пошуку по суб’єкту = "2"
        /// </summary>
        public string searchType { get; set; }                                  // 
        /// <summary>
        /// ПІБ користувача/Назва
        /// </summary>
        public SubjectSearchInfo subjectSearchInfo { get; set; }                // 
        /// <summary>
        /// Підстава виникнення речового права
        /// </summary>
        public string reason { get; set; }                                      // 
        /// <summary>
        /// ???
        /// </summary>
        public bool isDelayed { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public string dcReqtypeSubject { get; set; }                            // 
        /// <summary>
        /// ???
        /// </summary>
        public bool isExternal { get; set; }                                    // 
        /// <summary>
        /// ???
        /// </summary>
        public bool isSuspend { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public int totalLength { get; set; }                                    // 
        /// <summary>
        /// ???
        /// </summary>
        public int resultCount { get; set; }                                    // 
        /// <summary>
        /// Реєстраційний номер ОНМ
        /// </summary>
        public int regNum { get; set; }                                         // 
        /// <summary>
        /// ???
        /// </summary>
        public DateTime date { get; set; }                                      // 
    }/// <summary>
    /// ???
    /// </summary>
    public class SubjectSearchInfo
    {/// <summary>
     /// Тип суб'єкта: 1 - (фіз.особа) / 2 - (юр.особа)
     /// </summary>
        public string sbjType { get; set; }                                     // 
        /// <summary>
        /// Роль суб’єкта Значення відповідно довидника StatusPropertyOwnersDict (14. Довідник статусів власників речового майна StatusPropertyOwners)
        /// </summary>
        public string dcSbjRlNames { get; set; }                                // 
        /// <summary>
        /// ЄДРПОУ, передається для значень юридична особа (maxLength:12)
        /// </summary>
        public string sbjCode { get; set; }                                     // 
        /// <summary>
        /// ???
        /// </summary>
        public bool generalSubjectSearch { get; set; }                          // 
        /// <summary>
        /// ???
        /// </summary>
        public int irIrID { get; set; }                                         // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ResponseEstateData
    {/// <summary>
    /// ???
    /// </summary>
        public int? resultID { get; set; }                                      // 
        /// <summary>
        /// Для пошуку по об’єкту ="1" для пошуку по суб’єкту = "2" // "enum": ["1", "2"]
        /// </summary>
        public long? reportResultID { get; set; }                               // 
        /// <summary>
        /// 2 >> 3 
        /// </summary>
        public List<GroupResult> groupResult { get; set; }                      // 
        /// <summary>
        /// (maxLength:32)
        /// </summary>
        public string code { get; set; }                                        // 
    }/// <summary>
     /// 2 >> 3 
     /// </summary>
    public class GroupResult                                                    // 
    {/// <summary>
     /// Номер групи, для повторного пошуку
     /// </summary>
        public int? id { get; set; }                                            // 
        /// <summary>
        /// Тип групи (назва в GroupResult.name) (maxLength:64)
        /// </summary>
        public string dcGroupType { get; set; }                                 // 
        /// <summary>
        /// Опис групи, передається значення адреси, або кадастровий номер, або невизначене майно (maxLength:128)
        /// </summary>
        public string name { get; set; }                                        // 
    }
}
