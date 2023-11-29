using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace vkursi_api_example.estate
{
    public class GetEstateByCodeClass
    {
        /// <summary>
        /// 5. Отримання відомостей про наявні об'єкти нерухоммого майна у фізичних та юридичних осіб за кодом ЄДРПОУ або ІПН
        /// При першому запиті у відповідь приходять дані станом на поточний момент і ініціюється процедура поновлення. Якщо процедура відновлення не завершена у відповідь при повторному запиті (за тими ж паараметрами) буде приходити: Update in progress, total objects {общее количество} try again later. - Спробуйте повторити запит через 30 секунд. Після оновлення прийде стандартна відповідь.
        /// [GET] /api/1.0/estate/getestatebycode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        /*
            cURL запиту:
                curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatebycode?code=3080213038' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1...' \
                --header 'Content-Type: application/json' \

            Приклад відповіді:
                https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetEstateByCodeResponse.json      
        */
        public static GetRealEstateRightsResponseModel GetRealEstateRights(string code, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatebycode");
                RestRequest request = new RestRequest(Method.GET);

                request.AddParameter("code", code);                     // Код ЄДРПОУ або ІПН
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, total objects"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, try again later"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
                    return null;
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

            GetRealEstateRightsResponseModel realEstateDeserialize = new GetRealEstateRightsResponseModel();

            realEstateDeserialize = JsonConvert.DeserializeObject<GetRealEstateRightsResponseModel>(responseString);

            //string jsonStr = JsonConvert.SerializeObject(realEstateDeserialize, Formatting.Indented);

            //realEstateDeserialize.Estates.RemoveAll(r => r.DateEnd != null);

            return realEstateDeserialize;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"21560045\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsIn...',
          'Content-Type': 'application/json',
        }
        conn.request("GET", "/api/1.0/estate/getestatebycode?code=00131305", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatebycode?code=00131305")
          .method("GET", null)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель відповіді GetRealEstateRights
    /// </summary>
    public class GetRealEstateRightsResponseModel                               // 
    {/// <summary>
     /// Загальна статистика по об'єктам нерухомого майна (НМ)
     /// </summary>
        [JsonPropertyName("total")]
        public EstateTotalApi Total { get; set; }                               // 
        /// <summary>
        /// Перелік об'єктів НМ
        /// </summary>
        [JsonPropertyName("estates")]
        public List<EstateApi> Estates { get; set; }                            // 
        /// <summary>
        /// Код ЄДРПОУ / ІПН (maxLength:10)
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }                                        // 
        
        [JsonPropertyName("actualDate")]
        public DateTime? ActualDate { get; set; }                               // 
    }

    public class EstateTotalApi
    {/// <summary>
     /// К-ть участків
     /// </summary>
        [JsonPropertyName("landsCount")]
        public int LandsCount { get; set; }                                     // 
        /// <summary>
        /// К-ть об'єктів НМ
        /// </summary>
        [JsonPropertyName("housesCount")]
        public int HousesCount { get; set; }                                    // 
        /// <summary>
        /// Об'єкти за типами
        /// </summary>
        [JsonPropertyName("typeCount")]
        public List<EstateTypeTotal> TypeCount { get; set; }                    // 
        /// <summary>
        /// Об'єкти за категоріями
        /// </summary>
        [JsonPropertyName("globalTypeCount")]
        public List<EstateTypeTotal> GlobalTypeCount { get; set; }              // 
    }
    /// <summary>
    /// Об'єкти за типами
    /// </summary>
    public class EstateTypeTotal                                                // 
    {/// <summary>
     /// Id типу відповідно таблиці № 1
     /// </summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }                                           // 
        /// <summary>
        /// К-ть об'ектів (указаногго типу)
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }                                          // 
    }
    /// <summary>
    /// Перелік об'єктів НМ
    /// </summary>
    public class EstateApi                                                      // 
    {   /// <summary>
        /// Системний id
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }                                            // 
        /// <summary>
        /// Назва об'єкта | кадастровий номер  (maxLength:256)
        /// </summary>
        [JsonPropertyName("estateObjectName")]
        public string EstateObjectName { get; set; }                            // 
        /// <summary>
        /// Центральна координата 
        /// </summary>
        [JsonPropertyName("location")]
        public EstateCoordinates Location { get; set; }                         // 
        /// <summary>
        /// Тип об'єкта (true - земля)
        /// </summary>
        [JsonPropertyName("land")]
        public bool Land { get; set; }                                          // 
        /// <summary>
        /// Дата відчуження 
        /// </summary>
        [JsonPropertyName("dateEnd")]
        public DateTime? DateEnd { get; set; }                                  // 
        /// <summary>
        /// Дата створення запису по об'єкт (сервісне полк Vkursi)
        /// </summary>
        [JsonPropertyName("dateStart")]
        public DateTime? DateStart { get; set; }                                // 
        /// <summary>
        /// Дата модицікації (сервісне поле Vkursi)
        /// </summary>
        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }                              // 
        /// <summary>
        /// Тип власності (Власник / Правонабувач / ... )
        /// </summary>
        [JsonPropertyName("typeArray")]
        public List<int> TypeArray { get; set; }                                // 
        /// <summary>
        /// Об'єкти за категоріями
        /// </summary>
        [JsonPropertyName("globalTypeArray")]
        public List<int> GlobalTypeArray { get; set; }                          // 
        /// <summary>
        /// Детальна інформмація з ДЗК (Державного земельного кадастру)
        /// </summary>
        [JsonPropertyName("detailedCadastrInfo")]
        public DetailsJObjectEstate DetailedCadastrInfo { get; set; }           // 
        /// <summary>
        /// К-ть судових рішень
        /// </summary>
        [JsonPropertyName("courtCount")]
        public int? CourtCount { get; set; }                                    // 
        /// <summary>
        /// Код ЄДРПОУ / ІПН (maxLength:10)
        /// </summary>
        public string Code { get; set; }                                        // 

        /// <summary>
        /// Id групи об'єктів Nais (для отримання витяга)
        /// </summary>
        public long? reportResultID { get; set; }

        /// <summary>
        /// Id об'єкта Nais (для отримання витяга)
        /// </summary>
        [JsonPropertyName("objectId")]
        public long? ObjectId { get; set; }                                      // 
    }
    /// <summary>
    /// Центральна координата 
    /// </summary>
    public class EstateCoordinates                                              // 
    {/// <summary>
     /// Довгота
     /// </summary>
        [JsonPropertyName("longtitude")]
        public decimal Longtitude { get; set; }                                 // 
        /// <summary>
        /// Широта
        /// </summary>
        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }                                   // 
    }
    /// <summary>
    /// Детальна інформмація з ДЗК (Державного земельного кадастру)
    /// </summary>
    public class DetailsJObjectEstate                                           // 
    {/// <summary>
     /// КОАТУУ
     /// </summary>
        [JsonPropertyName("koatuu")]
        public long? koatuu { get; set; }                                       // 
        /// <summary>
        /// Зона
        /// </summary>
        public int? zona { get; set; }                                          // 
        /// <summary>
        /// Квартал
        /// </summary>
        public int? kvartal { get; set; }                                       // 
        /// <summary>
        /// Парсель
        /// </summary>
        public int? parcel { get; set; }                                        // 
        /// <summary>
        /// Кадастровый номер (maxLength:64)
        /// </summary>
        public string cadnum { get; set; }                                      // 
        /// <summary>
        /// Тип власності (100 Приватна власність | 200 Комунальна власність | 300 Державна власність)
        /// </summary>
        public int? ownershipcode { get; set; }                                 // 
        /// <summary>
        /// Цільове призначення (maxLength:256)
        /// </summary>
        public string purpose { get; set; }                                     // 
        /// <summary>
        /// Використання (maxLength:256)
        /// </summary>
        public string use { get; set; }                                         // 
        /// <summary>
        /// Площа (maxLength:64)
        /// </summary>
        public string area { get; set; }                                        // 
        /// <summary>
        /// Одиниця площі (maxLength:64)
        /// </summary>
        public string unit_area { get; set; }                                   // 
        /// <summary>
        /// Тип власності (назва) (maxLength:64)
        /// </summary>
        public string ownershipvalue { get; set; }                              // 
        /// <summary>
        /// Офіс реїстрації
        /// </summary>
        public int? id_office { get; set; }                                     // 
        /// <summary>
        /// Область (maxLength:64)
        /// </summary>
        public string region { get; set; }                                      // 
        /// <summary>
        /// Район (maxLength:64)
        /// </summary>
        public string district { get; set; }                                    // 
    }
}
                                                                                // Таблиця № 1
                                                                                // 1, // Вся земля
                                                                                // 11, // Власник
                                                                                // 12, // Правонабувач
                                                                                // 13, // Правокористувач
                                                                                // 14, // Землевласник
                                                                                // 15, // Землеволоділець
                                                                                // 16, // Інший
                                                                                // 17, // Наймач
                                                                                // 18, // Орендар
                                                                                // 19, // Наймодавець
                                                                                // 20, // Орендодавець
                                                                                // 21, // Управитель
                                                                                // 22, // Вигодонабувач
                                                                                // 23, // Установник
                                                                                // 6, // Іпотекодержатель
                                                                                // 7, // Майновий поручитель
                                                                                // 8, // Іпотекодавець
                                                                                // 9, // Боржник
                                                                                // 3, // Обтяжувач
                                                                                // 4, // Особа, майно/права якої обтяжуються
                                                                                // 10, // Особа, в інтересах якої встановлено обтяження
                                                                                // 25, // Довірчій власник