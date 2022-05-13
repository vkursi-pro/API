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

    public class GetRealEstateRightsResponseModel                               // Модель відповіді GetRealEstateRights
    {
        [JsonPropertyName("total")]
        public EstateTotalApi Total { get; set; }                               // Загальна статистика по об'єктам нерухомого майна (НМ)
        
        [JsonPropertyName("estates")]
        public List<EstateApi> Estates { get; set; }                            // Об'єкти НМ
        
        [JsonPropertyName("code")]
        public string Code { get; set; }                                        // Код ЄДРПОУ / ІПН (maxLength:10)
        
        [JsonPropertyName("actualDate")]
        public DateTime? ActualDate { get; set; }                               // 

        [JsonPropertyName("groupRequestId")]
        public long? GroupRequestId { get; set; }                               // 
    }

    public class EstateTotalApi
    {
        [JsonPropertyName("landsCount")]
        public int LandsCount { get; set; }                                     // К-ть участків
        
        [JsonPropertyName("housesCount")]
        public int HousesCount { get; set; }                                    // К-ть об'єктів НМ
        
        [JsonPropertyName("typeCount")]
        public List<EstateTypeTotal> TypeCount { get; set; }                    // Об'єкти за типами
        
        [JsonPropertyName("globalTypeCount")]
        public List<EstateTypeTotal> GlobalTypeCount { get; set; }              // Об'єкти за категоріями
    }

    public class EstateTypeTotal                                                // Об'єкти за типами
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }                                           // Id типу відповідно таблиці № 1
        
        [JsonPropertyName("count")]
        public int Count { get; set; }                                          // К-ть об'ектів (указаногго типу)
    }

    public class EstateApi                                                      // 
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }                                            // 
        
        [JsonPropertyName("estateObjectName")]
        public string EstateObjectName { get; set; }                            // Назва об'єкта | кадастровий номер  (maxLength:256)
        
        [JsonPropertyName("location")]
        public EstateCoordinates Location { get; set; }                         // Центральна координата 
        
        [JsonPropertyName("land")]
        public bool Land { get; set; }                                          // Тип об'єкта (true - земля)
        
        [JsonPropertyName("dateEnd")]
        public DateTime? DateEnd { get; set; }                                  // Дата відчуження 
        
        [JsonPropertyName("dateStart")]
        public DateTime? DateStart { get; set; }                                // Дата створення запису по об'єкт (сервісне полк Vkursi)

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }                              // Дата модицікації (сервісне поле Vkursi)
        
        [JsonPropertyName("typeArray")]
        public List<int> TypeArray { get; set; }                                // Тип власності (Власник / Правонабувач / ... )
        
        [JsonPropertyName("globalTypeArray")]
        public List<int> GlobalTypeArray { get; set; }                          // Об'єкти за категоріями

        [JsonPropertyName("detailedCadastrInfo")]
        public DetailsJObjectEstate DetailedCadastrInfo { get; set; }           // Детальна інформмація з ДЗК (Державного земельного кадастру)
        
        [JsonPropertyName("courtCount")]
        public int? CourtCount { get; set; }                                    // К-ть судових рішень

        [JsonPropertyName("objectId")]
        public long? ObjectId { get; set; }                                      // Id об'єкта Nais (для отримання витяга)
        public string Code { get; set; }                                        // Код ЄДРПОУ / ІПН (maxLength:10)
    }

    public class EstateCoordinates                                              // Центральна координата 
    {
        [JsonPropertyName("longtitude")]
        public decimal Longtitude { get; set; }                                 // Довгота

        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }                                   // Широта
    }

    public class DetailsJObjectEstate                                           // Детальна інформмація з ДЗК (Державного земельного кадастру)
    {
        [JsonPropertyName("koatuu")]
        public long? koatuu { get; set; }                                       // КОАТУУ
        public int? zona { get; set; }                                          // Зона
        public int? kvartal { get; set; }                                       // Квартал
        public int? parcel { get; set; }                                        // Парсель
        public string cadnum { get; set; }                                      // Кадастровый номер (maxLength:64)
        public int? ownershipcode { get; set; }                                 // Тип власності (100 Приватна власність | 200 Комунальна власність | 300 Державна власність)
        public string purpose { get; set; }                                     // Цільове призначення (maxLength:256)
        public string use { get; set; }                                         // Використання (maxLength:256)
        public string area { get; set; }                                        // Площа (maxLength:64)
        public string unit_area { get; set; }                                   // Одиниця площі (maxLength:64)
        public string ownershipvalue { get; set; }                              // Тип власності (назва) (maxLength:64)
        public int? id_office { get; set; }                                     // Офіс реїстрації
        public string region { get; set; }                                      // Область (maxLength:64)
        public string district { get; set; }                                    // Район (maxLength:64)
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