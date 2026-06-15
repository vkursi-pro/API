using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class BusinessPartnerCheckClass
    {
        /// <summary>
        /// Перевірка контрагента (агрегований метод) [POST] /api/1.0/organizations/businesspartnercheck
        /// Повертає реєстраційні дані ЄДР (Nais), відомості про ПДВ, санкції, кримінальні судові справи,
        /// зв'язки з російськими/білоруськими бенефіціарами/засновниками та наявність боргу в Єдиному реєстрі боржників (ЄРБ).
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
    /// Модель відповіді BusinessPartnerCheck
    /// </summary>
    public class BusinessPartnerCheckResponseModel
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішна відповідь: true - так / false - ні
        /// </summary>
        public bool IsSuccess { get; set; }
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
        /// Наявність боргу в Єдиному реєстрі боржників (ЄРБ)
        /// </summary>
        public BusinessPartnerCheckResponseErb ErbDebtData { get; set; }
        /// <summary>
        /// Дані ЕДР від Nais за посиланням:
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        public OrganizationaisElModel EdrData { get; set; }
    }

    /// <summary>
    /// Наявність боргу в Єдиному реєстрі боржників (ЄРБ)
    /// </summary>
    public class BusinessPartnerCheckResponseErb
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
}
