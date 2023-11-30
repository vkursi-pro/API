using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using vkursi_api_example.estate;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using vkursi_api_example.organizations;

namespace vkursi_api_example.enforcement
{
    public class GetCustomEnforcementsEdrClass
    {
        /// <summary>
        /// 157. Отримання відомостей про виконавчі провадження з ЕЦП
        /// [POST] /api/1.0/enforcement/GetCustomEnforcementsEdr
        /// </summary>
        /// <param name="code"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/enforcement/GetCustomEnforcementsEdr' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
            --header 'Content-Type: application/json' \
            --data '{"Method":3,"SearchParams":{"Edrpou":"00131305"}}' 
         
        */
        public static GetCustomEnforcementsEdrResponseModel GetCustomEnforcementsEdr(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetCustomEnforcementsEdrBodyModel COLRBodyModel = new GetCustomEnforcementsEdrBodyModel
                {
                    Method = 3,                                                         // Запит за: 3 - за кодом ЄДРПОУ боржника; Edrpou обов'язковий параметр
                    SearchParams = new OrganizationSearchParams { Edrpou = code },      // Код ЄДРПОУ
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel,                // Example body: {"Method":3,"SearchParams":{"Edrpou":"00131305"}}
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore});

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/enforcement/GetCustomEnforcementsEdr");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");
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

            GetCustomEnforcementsEdrResponseModel GCEEResponseRow = new GetCustomEnforcementsEdrResponseModel();

            GCEEResponseRow = JsonConvert.DeserializeObject<GetCustomEnforcementsEdrResponseModel>(responseString);

            return GCEEResponseRow;
        }
    }

    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetCustomEnforcementsEdrBodyModel
    { //*** Примітка у разі пошуку за назвою можливе отримання меншої кількості виконавчих проваджень,
        //  оскільки в реєстрі суб'єкт пошуку записаний у різній формі.
        //  У свою чергу запит по назві чекає повного співпадіння назви компанії з ОПФ
        //  Наприклад: АТ "Назва компанії", Акціонерне Товариство "Назва компанії", "Назва компанії"  

        /// <summary>
        /// Оберіть варіант пошуку за:
        /// 1 - asvp-searchAPI-bank-search1 - Запит за РНОКПП боржника; Inn обов'язковий параметр
        /// 2 - asvp-searchAPI-bank-search2 - Запит за ПІБ та Датою народження боржника; LastName і FirstName обов'язковий параметр
        /// 3 - asvp-searchAPI-bank-search3 - Запит за кодом ЄДРПОУ боржника; Edrpou обов'язковий параметр
        /// 4 - asvp-searchAPI-bank-search4 - Запит за Найменуванням боржника (компанії); firmName обов'язковий параметр
        /// 5 - asvp-searchAPI-bank-search5 - Запит за РНОКПП стягувача; Inn обов'язковий параметр
        /// 6 - asvp-searchAPI-bank-search6 - Запит за ПІБ та Датою народження стягувача; LastName і FirstName обов'язковий параметр
        /// 7 - asvp-searchAPI-bank-search7 - Запит за кодом ЄДРПОУ стягувача; Edrpou обов'язковий параметр
        /// 8 - asvp-searchAPI-bank-search8 - Запит за Найменування стягувача. firmName обов'язковий параметр
        /// 9 - asvp-searchAPI-bank-search9 - Запит по № АСВП; запит по номеру викон. провадж. VpNum обов'язковий параметр
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// Параметри пошуку
        /// </summary>
        public OrganizationSearchParams SearchParams { get; set; }
    }

    /// <summary>
    /// Параметри пошуку
    /// </summary>
    public class OrganizationSearchParams
    {
        /// <summary>
        /// ***Обов'язковий параметр для пошуку по ІНН***
        /// РНОКПП (до 10 символів) 
        /// </summary>
        [JsonPropertyName("inn")]
        public string? Inn { get; set; }
        /// <summary>
        /// ***Обов'язковий параметр для пошуку по ПІБ***
        /// Прізвище. Максимум 240 символів.
        /// </summary>
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        /// <summary>
        /// ***Обов'язковий параметр для пошуку по ПІБ***
        /// Ім'я. Максимум 100 символів.
        /// </summary>
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        /// <summary>
        /// По батькові. Максимум 100 символів.
        /// </summary>
        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Дата народження. У форматі YYYY-MM-DD.
        /// </summary>
        [JsonProperty("birthDate")]
        public string? BirthDate { get; set; }

        /// <summary>
        /// ЄДРПОУ. 8 символів.
        /// </summary>
        [JsonPropertyName("edrpou")]
        public string? Edrpou { get; set; }

        /// <summary>
        /// Найменування. Максимум 240 символів.
        /// </summary>
        [JsonPropertyName("firmName")]
        public string? FirmName { get; set; }


        /// <summary>
        /// Номер АСВП (від 1 до 20 символів)
        /// </summary>
        [JsonPropertyName("vpNum")]
        public string? VpNum { get; set; }

        /// <summary>
        /// Категорія стягнення. Код категорії ВП. Від 1 до 20 символів.
        /// словник csv по категоріям ASVP_INF.csv
        /// </summary>
        [JsonPropertyName("vdCategory")]
        public string? VdCategory { get; set; }

        /// <summary>
        /// Наявність відомостей про боржника в ЄРБ.
        /// </summary>
        [JsonPropertyName("isInErb")]
        public bool? IsInErb { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetCustomEnforcementsEdrResponseModel
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Оригінальні дані Наіс з ЕЦП
        /// </summary>
        public string OriginalResponse { get; set; }
        /// <summary>
        /// Відповідь на пошуковий запит
        /// </summary>
        public RegisterDebtorsResponseModel Data { get; set; }
    }

    /// <summary>
    /// Відповідь на пошуковий запит
    /// </summary>
    public class RegisterDebtorsResponseModel
    {
        /// <summary>
        /// Виконано без помилок
        /// </summary>
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// Дата обробки запиту
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Список ВП/ВД
        /// </summary>
        public List<SearchQueryResultItem>? Results { get; set; }
        /// <summary>
        /// requestId
        /// </summary>
        public long? RequestId { get; set; }
        /// <summary>
        /// Рядок запиту
        /// </summary>
        public string? Request { get; set; }
    }

    /// <summary>
    /// Інформація про ВП/ВД
    /// </summary>
    public class SearchQueryResultItem
    {
        /// <summary>
        /// Прізвище, ім’я, по батькові (за його наявності) боржника
        /// </summary>
        public string? DebtorPersonName { get; set; }

        /// <summary>
        /// Число, місяць, рік народження боржника
        /// </summary>
        public DateTime? DebtorBirthDate { get; set; }

        /// <summary>
        /// Найменування боржника
        /// </summary>
        public string? DebtorFirmName { get; set; }

        /// <summary>
        /// ЄДРПОУ боржника
        /// </summary>
        public string? DebtorFirmEdrpou { get; set; }

        /// <summary>
        /// Наявність боржника в ЄРБ (null для запитів 5-9)
        /// </summary>
        public bool? IsInErb { get; set; }

        /// <summary>
        /// Прізвище, ім’я, по батькові (за його наявності) стягувача
        /// </summary>
        public string? CreditorPersonName { get; set; }

        /// <summary>
        /// Найменування стягувача
        /// </summary>
        public string? CreditorFirmName { get; set; }

        /// <summary>
        /// ЄДРПОУ стягувача
        /// </summary>
        public string? CreditorFirmEdrpou { get; set; }

        /// <summary>
        /// Номер виконавчого провадження
        /// </summary>
        public string VpOrderNum { get; set; }

        /// <summary>
        /// Категорія стягнення (код)
        /// </summary>
        public string VdCategory { get; set; }

        /// <summary>
        /// Категорія стягнення
        /// </summary>
        public string VdCategoryName { get; set; }

        /// <summary>
        /// Дата відкриття виконавчого провадження
        /// </summary>
        public DateTime VpBeginDate { get; set; }

        /// <summary>
        /// Стан виконавчого провадження (код)
        /// </summary>
        public string VpState { get; set; }

        /// <summary>
        /// Стан виконавчого провадження
        /// </summary>
        public string VpStateName { get; set; }

        /// <summary>
        /// Найменування органу або прізвище, ім’я, по батькові (за його наявності) та посада посадової особи, яка видала виконавчий документ
        /// </summary>
        public string VpPublisher { get; set; }

        /// <summary>
        /// Найменування органу державної виконавчої служби або прізвище, ім’я, по батькові (за його наявності) приватного виконавця
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Номер засобу зв’язку (Телефон виконавця)
        /// </summary>
        public string EmployeePhone { get; set; }

        /// <summary>
        /// Адреса електронної пошти виконавця
        /// </summary>
        public string EmployeeEmail { get; set; }
    }
}
