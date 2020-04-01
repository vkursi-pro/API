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

    public class GetRealEstateRightsResponseModel // Нерухомість
    {
        public EstateTotalApi Total { get; set; } // Загальна статистика
        public List<EstateApi> Estates { get; set; } // Об'єкти
        public string Code { get; set; } // Код ЄДРПОУ / ІПН
    }

    public class EstateTotalApi
    {
        public int LandsCount { get; set; } // К-ть участків
        public int HousesCount { get; set; } // К-ть об'єктів нерухого майна
        public List<EstateTypeTotal> TypeCount { get; set; } // Об'єкти за типами
        public List<EstateTypeTotal> GlobalTypeCount { get; set; } // Об'єкти за категоріями
    }

    public class EstateTypeTotal // Об'єкти за типами
    {
        public int Type { get; set; } // Id типу відповідно таблиці № 1
        public int Count { get; set; } // К-ть об'ектів (указаногго типу)
    }

    public class EstateApi // 
    {
        public Guid Id { get; set; } // Шв
        public string EstateObjectName { get; set; } // Назва об'єкта | кадастровий номер
        public EstateCoordinates Location { get; set; }// Центральна координата 
        public bool Land { get; set; } // Тип об'єкта (true - земля)
        public DateTime? DateEnd { get; set; } // Дата відчуження 
        public DateTime? DateStart { get; set; } // Дата створення запису по об'єкт (сервісне полк Vkursi)
        public DateTime DateModified { get; set; } // Дата модицікації (сервісне поле Vkursi)
        public List<int> TypeArray { get; set; } // Тип власності (Власник / Правонабувач / ... )
        public List<int> GlobalTypeArray { get; set; } // Об'єкти за категоріями
        public DetailsJObjectEstate DetailedCadastrInfo { get; set; } // Детальна інформмація з ДЗК (Державного земельного кадастру)
        public int? CourtCount { get; set; } // К-ть судових рішень
    }
    public class EstateCoordinates // Центральна координата 
    {
        public decimal Longtitude { get; set; } // Довгота
        public decimal Latitude { get; set; } // Широта
    }
    public class DetailsJObjectEstate  // Детальна інформмація з ДЗК (Державного земельного кадастру)
    {
        public long? koatuu { get; set; } // КОАТУУ
        public int? zona { get; set; } // Зона
        public int? kvartal { get; set; } // Квартал
        public int? parcel { get; set; } // Парсель
        public string cadnum { get; set; } // Кадастровый номер
        public int? ownershipcode { get; set; } // Тип власності (100 Приватна власність | 200 Комунальна власність | 300 Державна власність)
        public string purpose { get; set; } // Цільове призначення
        public string use { get; set; } // Використання
        public string area { get; set; } // Площа
        public string unit_area { get; set; } // Одиниця площі
        public string ownershipvalue { get; set; } // Тип власності (назва)
        public int? id_office { get; set; } // Офіс реїстрації
        public string region { get; set; } // Область 
        public string district { get; set; } // Район 
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