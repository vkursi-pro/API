using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetRegistrationActionsHistoryClass
    {
        /// <summary>
        /// 155. Aрі історії реєстраційних дій
        /// [POST] /api/1.0/organizations/GetRegistrationActionsHistory
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*
        
        cURL запиту:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationActionsHistory' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
            --header 'ContentType: application/json' \
            --header 'Content-Type: application/json' \
            --data '{"code":"00131305"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRegistrationActionsHistoryResponse.json
        */
        public static GetRegistrationActionsHistoryResponseModel GetRegistrationActionsHistory(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetRegistrationActionsHistoryRequestBodyModel COLRBodyModel = new GetRegistrationActionsHistoryRequestBodyModel
                {
                    Code = code                               // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel);           // Example body: {"Codes":["00131305"]}

                RestClient client = new RestClient(
                    "https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRegistrationActionsHistory");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetRegistrationActionsHistoryResponseModel GNWEResponse = 
                JsonConvert.DeserializeObject<GetRegistrationActionsHistoryResponseModel>(responseString);

            return GNWEResponse;
        }
    }

    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetRegistrationActionsHistoryRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дата проведення змін (ВІД)
        /// </summary>
        [JsonProperty("dateStart")]
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Дата проведення змін (ДО)
        /// </summary>
        [JsonProperty("dateEnd")]
        public DateTime? DateEnd { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetRegistrationActionsHistoryResponseModel
    {
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<GetRegistrationActionsHistoryData> Data { get; set; }
    }

    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class GetRegistrationActionsHistoryData
    {
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// Дата проведення змін
        /// </summary>
        [JsonProperty("dateOfChange")]
        public DateTime DateOfChange { get; set; }
        /// <summary>
        /// Опис змін
        /// </summary>
        [JsonProperty("changes")]
        public EdrChangesInfoModelChange[] Changes { get; set; }
        /// <summary>
        /// Попередній об'єкт (передається тільки той розділ (об'єкт/поле) в якому відбулись зміни)
        /// Опис повної моделі: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        [JsonProperty("oldNaisValue")]
        public OrganizationaisElModel OldNaisValue { get; set; }
        /// <summary>
        /// Новий об'єкт (передається тільки той розділ (об'єкт/поле) в якому відбулись зміни)
        /// Опис повної моделі: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        [JsonProperty("newNaisValue")]
        public OrganizationaisElModel NewNaisValue { get; set; }
    }

    /// <summary>
    /// Опис змін
    /// </summary>
    public class EdrChangesInfoModelChange
    {
        /// <summary>
        /// Id зміни.
        /// Визначає тип зміни, яка відбулася.
        ///
        /// Основні типи змін:
        /// 1 – Зміна стану (статусу реєстрації)
        /// 2 – Зміна найменування юридичної особи
        /// 4 – Зміна організаційно-правової форми
        /// 5 – Зміна місцезнаходження юридичної особи
        /// 6 – Зміна розміру статутного капіталу
        /// 7 – Зміна складу засновників
        /// 8 – Зміна кінцевого бенефіціарного власника
        /// 9 – Зміна структури власності
        /// 10 – Зміна видів діяльності
        /// 11 – Зміна керівника
        /// 12 – Зміна контактної інформації
        /// 13 – Зміна органів управління
        /// 14 – Зміна установчого документа
        /// 15 – Зміна коду модельного статуту
        /// 16 – Зміна відокремлених підрозділів
        /// 17 – Зміна інформації про банкрутство
        /// 18 – Зміна процесу припинення
        /// 19 – Зміна даних про смерть/відсутність
        /// 20 – Зміна строку заявлення вимог
        /// 21 – Зміна реєстрації припинення
        /// 22 – Відміна реєстрації припинення
        /// 23 – Зміна даних про правонаступників
        /// 24 – Зміна даних про правонаступників (інші)
        /// 25 – Зміна місцезнаходження реєстраційної справи
        /// 26 – Зміна даних з інших державних органів
        ///
        /// Підтипи змін:
        /// 101 – Зміна коду стану
        /// 102 – Зміна назви стану
        /// 201 – Зміна повної назви суб'єкта
        /// 202 – Зміна назви для відображення
        /// 203 – Зміна короткої назви суб'єкта
        /// 204 – Зміна повної назви (англ.)
        /// 205 – Зміна короткої назви (англ.)
        /// 401 – Зміна коду організаційно-правової форми
        /// 402 – Зміна назви організаційно-правової форми
        /// 501 – Зміна поштового індексу
        /// 502 – Зміна країни місцезнаходження
        /// 503 – Зміна повної адреси
        /// 601 – Зміна суми капіталу
        /// 602 – Зміна дати формування капіталу
        /// 701 – Додано/вийшов засновник
        /// 702 – Зміна частки капіталу
        /// 703 – Зміна адреси засновника
        /// 801 – Додано/вийшов бенефіціар
        /// 802 – Зміна адреси бенефіціара
        /// 901 – Зміна дати структури власності
        /// 902 – Зміна номеру структури
        /// 903 – Зміна підписанта структури
        /// 1001 – Додано/виключено КВЕД
        /// 1101 – Зміна керівника
        /// 1102 – Зміна адреси керівника
        /// 1201 – Зміна e-mail
        /// 1202 – Зміна телефону
        /// 1203 – Зміна номеру факсу
        /// 1204 – Зміна веб-сторінки
        /// 1301 – Зміна органу управління
        /// 1401 – Зміна типу документа
        /// 1402 – Зміна імені документа
        /// 1403 – Зміна коду документа
        /// 1601 – Додано/ліквідовано підрозділ
        /// 1602 – Зміна адреси підрозділу
        /// 1701 – Зміна дати запису про банкрутство
        /// 1702 – Зміна стану (код)
        /// 1703 – Зміна стану (текст)
        /// 1704 – Зміна номеру провадження
        /// 1705 – Зміна дати провадження
        /// 1706 – Зміна дати судового рішення
        /// 1707 – Зміна найменування суду
        /// 2001 – Зміна строку для вимог
        /// 2101 – Зміна дати реєстрації
        /// 2102 – Зміна запису реєстрації
        /// 2103 – Зміна дати запису
        /// 2201 – Зміна дати скасування реєстрації
        /// 2202 – Зміна стану суб’єкта (код)
        /// 2203 – Зміна стану суб’єкта (назва)
        /// 2301 – Зміна правонаступника (назва)
        /// 2302 – Зміна правонаступника (код)
        /// 2401 – Зміна правонаступника (назва)
        /// 2402 – Зміна правонаступника (код)
        /// 2501 – Зміна місцезнаходження справи
        /// 2601 – Новий/старий орган статистики
        /// 2605 – Зміна дати взяття на облік
        /// 2606 – Зміна номеру взяття на облік
        /// 2607 – Зміна дати зняття з обліку
        /// 2608 – Зміна номеру зняття з обліку
        /// </summary>
        [JsonProperty("changeTypeId")]
        public int ChangeTypeId { get; set; }
        /// <summary>
        /// Назва категорії зміни
        /// </summary>
        [JsonProperty("changeType")]
        public string ChangeType { get; set; }
        /// <summary>
        /// Перелік змін відповідно до категорії
        /// </summary>
        [JsonProperty("changes")]
        public ChangeChange[] Changes { get; set; }
    }

    /// <summary>
    /// Перелік змін відповідно до категорії
    /// </summary>
    public class ChangeChange
    {
        /// <summary>
        /// Id зміни
        /// </summary>
        [JsonProperty("changeTypeId")]
        public int ChangeTypeId { get; set; }
        /// <summary>
        /// Назва зміни
        /// </summary>
        [JsonProperty("changeType")]
        public string ChangeType { get; set; }
        /// <summary>
        /// Попереднє значення
        /// </summary>
        [JsonProperty("oldValue")]
        public string OldValue { get; set; }
        /// <summary>
        /// Нове значення
        /// </summary>
        [JsonProperty("newValue")]
        public string NewValue { get; set; }
    }
}
