using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class BusinessPartnerCheckClass
    {
        /// <summary>
        /// Перевірка контрагента (агрегований метод) [POST] /api/1.0/organizations/businesspartnercheck
        /// Повертає реєстраційні дані ЄДР (Nais), відомості про ПДВ, санкції, кримінальні судові справи,
        /// зв'язки з російськими/білоруськими бенефіціарами/засновниками, дані з реєстру неприбуткових
        /// організацій, реєстру платників єдиного податку, категорію платника податків та наявність боргу
        /// в Єдиному реєстрі боржників (ЄРБ).
        /// Структурно відповідає індивідуальному методу №144 (Нафтогаз) і додатково містить секцію ErbDebtData.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        /*

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/businesspartnercheck' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":"00131305","needUpdate":true}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/BusinessPartnerCheckResponse.json

         */

        public static BusinessPartnerCheckResponseModel BusinessPartnerCheck(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                BusinessPartnerCheckRequestBodyModel BPCRequestBody = new BusinessPartnerCheckRequestBodyModel
                {
                    Code = code,                                            // Код ЄДРПОУ (8) або ІПН (10)
                    NeedUpdate = true                                       // Оновити дані в онлайн на Nais (ЕДР)
                };

                string body = JsonConvert.SerializeObject(BPCRequestBody);  // Example body: {"Code":"00131305","needUpdate":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/businesspartnercheck");
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
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

            BusinessPartnerCheckResponseModel BPCResponse = new BusinessPartnerCheckResponseModel();

            BPCResponse = JsonConvert.DeserializeObject<BusinessPartnerCheckResponseModel>(responseString);

            return BPCResponse;
        }
    }

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class BusinessPartnerCheckRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ (8) або ІПН (10)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Чи потрібно оновлювати дані в онлайн на Nais (ЕДР): true - так / false - ні
        /// </summary>
        public bool NeedUpdate { get; set; }
    }

    /// <summary>
    /// Модель відповіді BusinessPartnerCheck (відповідає GetBusinessPartnerCheckResponse в проєкті VkursiNet:
    /// UniversalApiResponse + GetAdvancedOrganizationExtendedModelResponse + секція ErbDebtData)
    /// </summary>
    public class BusinessPartnerCheckResponseModel
    {
        // --- Загальні поля відповіді (UniversalApiResponse) ---

        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Id відповіді по API (відповідно до enum ApiResponseStateEnum)
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// Чи успішний запит: true - так / false - ні
        /// </summary>
        public bool? IsSuccess { get; set; }
        /// <summary>
        /// Id запису в історії запитів
        /// </summary>
        public Guid? LogId { get; set; }
        /// <summary>
        /// Розширене повідомлення про помилку
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Загальна дата актуальності
        /// </summary>
        public DateTime? ActualDate { get; set; }

        // --- Дані контрагента (GetAdvancedOrganizationExtendedModelResponse) ---

        /// <summary>
        /// Дані ЕДР від Nais за посиланням:
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        public OrganizationaisElModel EdrData { get; set; }
        /// <summary>
        /// Відомості про платника ПДВ
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponsePdv PdvData { get; set; }
        /// <summary>
        /// Відомості про санкції
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseSanction SanctionData { get; set; }
        /// <summary>
        /// Дані по судовим документам
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseCourt CourtData { get; set; }
        /// <summary>
        /// Дані по зв'язкам з російськими/білоруськими бенефіціарами/засновниками
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseRFBRBeneficiar RFBRBeneficiarData { get; set; }
        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        public string Bankruptcy { get; set; }
        /// <summary>
        /// Контактні дані
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseContact ContactData { get; set; }
        /// <summary>
        /// Посилання на витяг з ЄДР в vkursi.pro
        /// </summary>
        public string ReportHref { get; set; }
        /// <summary>
        /// Відомості з реєстру неприбуткових установ та організацій
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseNonProfit NonProfitData { get; set; }
        /// <summary>
        /// Відомості з реєстру платників єдиного податку
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseCabinetTaxEdpod CabinetTaxEdpodData { get; set; }
        /// <summary>
        /// Дані з реєстру ІСІ (інститути спільного інвестування)
        /// </summary>
        public List<OrgLicensesApiAnswerModelDataObject> RegisterIsiData { get; set; }
        /// <summary>
        /// Категорія платника податків
        /// </summary>
        public GetAdvancedOrganizationExtendedModelResponseTaxpayerCategory TaxpayerCategoryData { get; set; }

        // --- Поля, унікальні для BusinessPartnerCheck ---

        /// <summary>
        /// Наявність боргу в Єдиному реєстрі боржників (ЄРБ)
        /// </summary>
        public GetBusinessPartnerCheckResponseErb ErbDebtData { get; set; }
        /// <summary>
        /// Резидентство в Дія.City
        /// </summary>
        public GetBusinessPartnerCheckResponseDiiaCity DiiaCityData { get; set; }
    }

    /// <summary>
    /// Контактні дані
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseContact
    {
        /// <summary>
        /// Список контактів
        /// </summary>
        public List<GetAdvancedOrganizationExtendedModelResponseContactItem> DataList { get; set; }
    }

    /// <summary>
    /// Контакт
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseContactItem
    {
        /// <summary>
        /// Значення (номер телефону, email тощо)
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Тип контакту
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Відомості з реєстру неприбуткових установ та організацій
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseNonProfit
    {
        /// <summary>
        /// Чи внесений контрагент до Реєстру неприбуткових установ та організацій: true - так / false - ні
        /// </summary>
        public bool NonProfitExist { get; set; }
        /// <summary>
        /// Дата реєстрації неприбутковою
        /// </summary>
        public DateTime? DRegNoPr { get; set; }
        /// <summary>
        /// Дата присвоєння ознаки неприбутковості
        /// </summary>
        public DateTime? DNonpr { get; set; }
        /// <summary>
        /// Ознака неприбутковості
        /// </summary>
        public string CNonpr { get; set; }
        /// <summary>
        /// Дата анулювання ознаки неприбутковості
        /// </summary>
        public DateTime? DAnul { get; set; }
    }

    /// <summary>
    /// Відомості з реєстру платників єдиного податку
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseCabinetTaxEdpod
    {
        /// <summary>
        /// Чи є контрагент платником єдиного податку: true - так / false - ні
        /// </summary>
        public bool EdpodExist { get; set; }
        /// <summary>
        /// Повне найменування
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Дата реєстрації платником єдиного податку
        /// </summary>
        public string DataN { get; set; }
        /// <summary>
        /// Дата видачі свідоцтва
        /// </summary>
        public string DataSv { get; set; }
        /// <summary>
        /// Ставка єдиного податку
        /// </summary>
        public decimal? Stavka { get; set; }
        /// <summary>
        /// Група платника єдиного податку
        /// </summary>
        public int? Grup { get; set; }
        /// <summary>
        /// Дата анулювання реєстрації платником єдиного податку
        /// </summary>
        public string DataK { get; set; }
    }

    /// <summary>
    /// Категорія платника податків:
    /// 2 - Інститут спільного інвестування
    /// 3 - Неприбуткова організація, внесена до Реєстру неприбуткових установ та організацій
    /// 4 - Юридична особа, що застосовує спрощену систему оподаткування
    /// 5 - Фізична особа - підприємець, що застосовує спрощену систему оподаткування
    /// null - якщо немає інформації про категорію
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseTaxpayerCategory
    {
        /// <summary>
        /// Ідентифікатор категорії платника податків
        /// </summary>
        public int? CategoryId { get; set; }
    }

    /// <summary>
    /// Об'єкт даних реєстру ІСІ / ліцензій
    /// </summary>
    public class OrgLicensesApiAnswerModelDataObject
    {
        /// <summary>
        /// Назва
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Номер ліцензії
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Дата закінчення дії ліцензії
        /// </summary>
        public DateTime? DateEnd { get; set; }
        /// <summary>
        /// Дата початку дії ліцензії
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Стан ліцензії
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Орган що видав ліцензію
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// Додаткова інформація
        /// </summary>
        public object Info { get; set; }
        /// <summary>
        /// Ознака архівної ліцензії
        /// </summary>
        public bool IsArchival { get; set; }
    }

    /// <summary>
    /// Наявність боргу в Єдиному реєстрі боржників (ЄРБ)
    /// </summary>
    public class GetBusinessPartnerCheckResponseErb
    {
        /// <summary>
        /// Чи є контрагент у Єдиному реєстрі боржників: true - так / false - ні
        /// </summary>
        public bool ErbDebtExist { get; set; }
        /// <summary>
        /// Посилання на розділ в vkursi.pro
        /// </summary>
        public string ErbDebtUrl { get; set; }
    }

    /// <summary>
    /// Резидентство в Дія.City (за даними реєстру резидентів Дія.City)
    /// </summary>
    public class GetBusinessPartnerCheckResponseDiiaCity
    {
        /// <summary>
        /// Чи є контрагент резидентом Дія.City: true - так / false - ні
        /// </summary>
        public bool DiiaCityResident { get; set; }
        /// <summary>
        /// Дата набуття статусу резидента Дія.City
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Дата втрати статусу резидента Дія.City
        /// </summary>
        public DateTime? DateEnd { get; set; }
        /// <summary>
        /// Стан запису (Діюча / Не діюча)
        /// </summary>
        public string Status { get; set; }
    }
}
