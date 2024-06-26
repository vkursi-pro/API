using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public static class GetRegistrationStatusAndGeolocationClass
    {
        /*
        166. Отри
        мання відповіді з даними місцезнаходження юр. особи розбиту по частинам (в тому числі координати)


        [POST] api/1.0/organizations/GetRegistrationStatusAndGeolocation

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationStatusAndGeolocation' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"00131512"}'
        */
        public static GetRegistrationStatusAndGeolocationModelOut GetRegistrationStatusAndGeolocation(ref string token, string code)
        {

            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationStatusAndGeolocation");
                RestRequest request = new RestRequest(Method.POST);

                GetOrgFinanceOriginalData GOFRequesRow = new GetOrgFinanceOriginalData()
                {
                    Code = code,                                             //00131512
                };

                string body = JsonConvert.SerializeObject(GOFRequesRow);      // Example: {"Code":["00131512"]}

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

            GetRegistrationStatusAndGeolocationModelOut GOFResponseRow = new();

            GOFResponseRow = JsonConvert.DeserializeObject<GetRegistrationStatusAndGeolocationModelOut>(responseString, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error });

            return GOFResponseRow;
        }

    }
    public class GetRegistrationStatusAndGeolocationModelOut
    {
        /// <summary>
        /// Статус запиту
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи був вдалий запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Основні дані
        /// </summary>
        public GetRegistrationStatusAndGeolocationModelOutData Data { get; set; }
    }

    public class GetRegistrationStatusAndGeolocationModelOutData
    {
        /// <summary>
        /// Код ЄДРПОУ юр. особи
        /// </summary>
        public string CodeClient { get; set; }
        /// <summary>
        /// Тип юридичної особи
        /// </summary>
        public int TypeClient { get; set; }
        /// <summary>
        /// Повна назва юр. особи
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Коротка назва юр. особи
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Повна адреса
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Адреса по частинам
        /// </summary>
        public GetRegistrationStatusAndGeolocationModelOutDataParts Parts { get; set; }
        /// <summary>
        /// Координати
        /// </summary>
        public GetRegistrationStatusAndGeolocationModelOutDataGeoData GeoData { get; set; }

    }

    public class GetRegistrationStatusAndGeolocationModelOutDataGeoData
    {
        /// <summary>
        /// Широта
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// Довгота
        /// </summary>
        public decimal Lon { get; set; }
    }

    public class GetRegistrationStatusAndGeolocationModelOutDataParts
    {
        /// <summary>
        /// Обл., район, населений пункт
        /// </summary>
        public string? atu { get; set; }
        /// <summary>
        /// Код за КАТОТТГ населеного пункту
        /// </summary>
        public string? atu_code { get; set; }
        /// <summary>
        /// ідентифікатор коду КАТОТТГ
        /// </summary>
        public long? atu_code_id { get; set; }
        /// <summary>
        /// назва об'єкту
        /// </summary>
        public string? obj_name { get; set; }
        /// <summary>
        /// ідентифікатор об'єкту
        /// </summary>
        public long? obj_id { get; set; }
        /// <summary>
        /// Вулиця
        /// </summary>
        public string? street { get; set; }
        /// <summary>
        /// Ідентифікатор вулиці
        /// </summary>
        public long? street_id { get; set; }
        /// <summary>
        /// Тип будинку
        /// </summary>
        public string? house_type { get; set; }
        /// <summary>
        /// Будинок
        /// </summary>
        public string? house { get; set; }
        /// <summary>
        /// Тип будівлі
        /// </summary>
        public string? building_type { get; set; }
        /// <summary>
        /// Будівля
        /// </summary>
        public string? building { get; set; }
        /// <summary>
        /// Тип приміщення
        /// </summary>
        public string? num_type { get; set; }
        /// <summary>
        /// Номер приміщення
        /// </summary>
        public string? num { get; set; }
        /// <summary>
        /// Індекс
        /// </summary>
        public string? zip { get; set; }
        /// <summary>
        /// Назва населеного пункту
        /// </summary>
        public string? city_name { get; set; }
        /// <summary>
        /// Тип населеного пункту
        /// </summary>
        public string? city_category { get; set; }
        /// <summary>
        /// Область
        /// </summary>
        public string? obl_name { get; set; }
        /// <summary>
        /// Район
        /// </summary>
        public string? region_name { get; set; }
    }
}
