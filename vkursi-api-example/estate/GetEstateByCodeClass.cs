using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetEstateByCodeClass
    {
        /*
        
        5. Отримання відомостей про наявні об'єкти нерухоммого майна у фізичних та юридичних осіб за кодом ЄДРПОУ або ІПН
        [GET] /api/1.0/estate/getestatebycode     
         
        curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getestatebycode?code=00131305' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGci...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"21560045"}'

         */

        public static GetRealEstateRightsResponseModel GetRealEstateRights(string code, string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

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
                    token = AuthorizeClass.Authorize();
                }
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаным кодом організації не знайдено");
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
        public EstateTotalApi Total { get; set; }                               // Загальна статистика по об'єктам нерухомого майна (НМ)
        public List<EstateApi> Estates { get; set; }                            // Об'єкти НМ
        public string Code { get; set; }                                        // Код ЄДРПОУ / ІПН
    }

    public class EstateTotalApi
    {
        public int LandsCount { get; set; }                                     // К-ть участків
        public int HousesCount { get; set; }                                    // К-ть об'єктів НМ
        public List<EstateTypeTotal> TypeCount { get; set; }                    // Об'єкти за типами
        public List<EstateTypeTotal> GlobalTypeCount { get; set; }              // Об'єкти за категоріями
    }

    public class EstateTypeTotal                                                // Об'єкти за типами
    {
        public int Type { get; set; }                                           // Id типу відповідно таблиці № 1
        public int Count { get; set; }                                          // К-ть об'ектів (указаногго типу)
    }

    public class EstateApi                                                      // 
    {
        public Guid Id { get; set; }                                            // 
        public string EstateObjectName { get; set; }                            // Назва об'єкта | кадастровий номер
        public EstateCoordinates Location { get; set; }                         // Центральна координата 
        public bool Land { get; set; }                                          // Тип об'єкта (true - земля)
        public DateTime? DateEnd { get; set; }                                  // Дата відчуження 
        public DateTime? DateStart { get; set; }                                // Дата створення запису по об'єкт (сервісне полк Vkursi)
        public DateTime DateModified { get; set; }                              // Дата модицікації (сервісне поле Vkursi)
        public List<int> TypeArray { get; set; }                                // Тип власності (Власник / Правонабувач / ... )
        public List<int> GlobalTypeArray { get; set; }                          // Об'єкти за категоріями
        public DetailsJObjectEstate DetailedCadastrInfo { get; set; }           // Детальна інформмація з ДЗК (Державного земельного кадастру)
        public int? CourtCount { get; set; }                                    // К-ть судових рішень
    }
    public class EstateCoordinates                                              // Центральна координата 
    {
        public decimal Longtitude { get; set; }                                 // Довгота
        public decimal Latitude { get; set; }                                   // Широта
    }
    public class DetailsJObjectEstate                                           // Детальна інформмація з ДЗК (Державного земельного кадастру)
    {
        public long? koatuu { get; set; }                                       // КОАТУУ
        public int? zona { get; set; }                                          // Зона
        public int? kvartal { get; set; }                                       // Квартал
        public int? parcel { get; set; }                                        // Парсель
        public string cadnum { get; set; }                                      // Кадастровый номер
        public int? ownershipcode { get; set; }                                 // Тип власності (100 Приватна власність | 200 Комунальна власність | 300 Державна власність)
        public string purpose { get; set; }                                     // Цільове призначення
        public string use { get; set; }                                         // Використання
        public string area { get; set; }                                        // Площа
        public string unit_area { get; set; }                                   // Одиниця площі
        public string ownershipvalue { get; set; }                              // Тип власності (назва)
        public int? id_office { get; set; }                                     // Офіс реїстрації
        public string region { get; set; }                                      // Область 
        public string district { get; set; }                                    // Район 
    }
}


// Таблиця № 1
// 1, //Вся земля
// null, //Власник
// 12, //Правонабувач
// 13, //Правокористувач
// 14, //Землевласник
// 15, //Землеволоділець
// 16, //Інший
// 17, //Наймач
// 18, //Орендар
// 19, //Наймодавець
// 20, //Орендодавець
// 21, //Управитель
// 22, //Вигодонабувач
// 23, //Установник
// 6, //Іпотекодержатель
// 7, //Майновий поручитель
// 8, //Іпотекодавець
// 9, //Боржник
// 3, //Обтяжувач
// 4, //Особа, майно/права якої обтяжуються
// 10 //Особа, в інтересах якої встановлено обтяження