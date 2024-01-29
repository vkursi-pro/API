using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class DigitalNaftoGazBusinessPartnersClass
    {

        /// <summary>
        /// Індивідуальний метод верифікації бізнес партнерів
        /// </summary>
        /// <param name="code"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        /*
        
        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/digitalNaftoGazBusinessPartners' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":"35268354","needUpdate":true}'
        
        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/DigitalNaftoGazBusinessPartnersResponse.json
        
         */

        public static DigitalNaftoGazBusinessPartnersResponseModel DigitalNaftoGazBusinessPartners(string code, ref string token)
        {
            if (string.IsNullOrEmpty(token)) {
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                DigitalNaftoGazBusinessPartnersRequestBodyModel GAORequestBody = new DigitalNaftoGazBusinessPartnersRequestBodyModel
                {
                    Code = code,                                             // Код ЄДРПОУ або ІПН
                    NeedUpdate = true
                };

                string body = JsonConvert.SerializeObject(GAORequestBody);  // Example body: {"Code":"21560045","needUpdate":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/digitalNaftoGazBusinessPartners");
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

            DigitalNaftoGazBusinessPartnersResponseModel GAOResponse = new DigitalNaftoGazBusinessPartnersResponseModel();

            GAOResponse = JsonConvert.DeserializeObject<DigitalNaftoGazBusinessPartnersResponseModel>(responseString);

            return GAOResponse;
        }
    }

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class DigitalNaftoGazBusinessPartnersRequestBodyModel
    {
        /// <summary>
        /// Код ЕДРПОУ
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Чи потрібно оновлювати дані в онлайн на Nais(ЕДР): true - так / false - ні
        /// </summary>
        public bool NeedUpdate { get; set; }
        /// <summary>
        /// Отримати тільки дані Nais(ЕДР): true - так / false - ні
        /// </summary>
        public bool GetOnlyNaisData { get; set; }
    }
    /// <summary>
    /// Модель відповіді GetAdvancedOrganization
    /// </summary>
    public class DigitalNaftoGazBusinessPartnersResponseModel
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішна выдповідь: true - так / false - ні
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
        /// Дані ЕДР від Nais за посиланням: 
        /// https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L173
        /// </summary>
        public OrganizationaisElModel EdrData { get; set; }
    }
    /// <summary>
    /// Відомості про платника ПДВ
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponsePdv
    {
        /// <summary>
        /// Дата реєстрації свідоцтва платника ПДВ
        /// </summary>
        public DateTime? RegInn { get; set; }
        /// <summary>
        /// Код ПДВ (maxLength:10)
        /// </summary>
        public string Inn { get; set; }
        /// <summary>
        /// Дата анулювання свідоцтва платника ПДВ 
        /// </summary>
        public DateTime? DateCanceledInn { get; set; }
    }
    /// <summary>
    /// Відомості про санкції
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseSanction
    {
        /// <summary>
        /// Чи наявні відомості про санкції: true - так / false - ні
        /// </summary>
        public bool SanctionsExist { get; set; }
        /// <summary>
        /// Посилання на розділ в vkursi.pro
        /// </summary>
        public string SanctionUrl { get; set; }
    }
    /// <summary>
    /// Дані по судовим документам
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseCourt
    {
        /// <summary>
        /// Чи наявні судові документи по кримінальних справах: true - так / false - ні
        /// </summary>
        public bool CourtExist { get; set; }
        /// <summary>
        /// Посилання на розділ в vkursi.pro
        /// </summary>
        public string CourtUrl { get; set; }
    }
    /// <summary>
    /// Дані по зв'язкам з російськими/білоруськими бенефіціарами/засновниками
    /// </summary>
    public class GetAdvancedOrganizationExtendedModelResponseRFBRBeneficiar
    {
        /// <summary>
        /// Чи наявні зв'язки з російськими/білоруськими бенефіціарами/засновниками: true - так / false - ні
        /// </summary>
        public bool BeneficiarExist { get; set; }
        /// <summary>
        /// Посилання на розділ в vkursi.pro
        /// </summary>
        public string BeneficiarUrl { get; set; }
    }

}
