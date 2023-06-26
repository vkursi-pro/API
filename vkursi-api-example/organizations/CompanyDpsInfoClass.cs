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
    public class CompanyDpsInfoClass
    {
        /*
        
        Метод:
            153. Дані про ДПС платника за кодом ЕДРПОУ
            [POST] /api/1.0/organizations/CompanyDpsInfo

        cURL запиту:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/CompanyDpsInfo' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...' \
            --header 'ContentType: application/json' \
            --header 'Content-Type: application/json' \
            --data '{"Codes":["00131305"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CompanyDpsInfoResponse.json

        */

        public static CompanyDpsInfoResponseModel CompanyDpsInfo(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CompanyDpsInfoRequestBodyModel COLRBodyModel = new CompanyDpsInfoRequestBodyModel
                {
                    Codes = new List<string> { code }                               // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel);           // Example body: {"Codes":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/CompanyDpsInfo");
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

            CompanyDpsInfoResponseModel GNWEResponse = new CompanyDpsInfoResponseModel();

            GNWEResponse = JsonConvert.DeserializeObject<CompanyDpsInfoResponseModel>(responseString);

            return GNWEResponse;
        }
    }
    /// <summary>
    /// Модель запиту
    /// </summary>
    public class CompanyDpsInfoRequestBodyModel
    {
        /// <summary>
        /// Перелік кодів ЄДРПОУ
        /// </summary>
        public List<string> Codes { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class CompanyDpsInfoResponseModel
    {
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<CompanyDpsInfoApiModelResponseData> Data { get; set; }
    }

    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class CompanyDpsInfoApiModelResponseData
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Дані про ДПС платника
        /// </summary>
        public TaxDepartmentForOrganizationModel Data { get; set; }
    }

    /// <summary>
    /// Дані про ДПС платника
    /// </summary>
    public class TaxDepartmentForOrganizationModel
    {
        /// <summary>
        /// Ім'я територіальної одиниці ДПС
        /// </summary>
        public string? NameSti { get; set; }
        /// <summary>
        /// Код ЄДРПОУ ДПС
        /// </summary>
        public int? Code { get; set; }
        /// <summary>
        /// Індетифікатор
        /// </summary>
        public int? TaxDepartmentId { get; set; }
        /// <summary>
        /// Ідентифікаційний номер територіальної одиниці обласного рівня
        /// </summary>
        public int? CReg { get; set; }
        /// <summary>
        /// Код територіальної одиниці, де розташований головний офіс ДПС
        /// </summary>
        public int? CDst { get; set; }
        /// <summary>
        /// Код територіальної одиниці, де розташовано орган ДПС
        /// </summary>
        public int? CRaj { get; set; }
        /// <summary>
        /// Назва територіальної одиниці, де розташовано орган ДПС
        /// </summary>
        public string? NameRaj { get; set; }
        /// <summary>
        /// Тип органу  ДПС:
        /// 1 - обласне головне управління,
        /// 2 - ДПІ,
        /// 6 - об'єднана ДПI,
        /// 8 - спецiалiзованi ДПI
        /// </summary>
        public int? TSti { get; set; }
        /// <summary>
        /// Код органу ДПС за ЄДРПОУ
        /// </summary>
        public int? CSti { get; set; }
        /// <summary>
        /// Код регіонального органу податків
        /// </summary>
        public int? RegionTaxDepartmentCode { get; set; }
        /// <summary>
        /// Код КОАТУУ (Державний класифікатор об'єктів адміністративно-територіального устрою України)
        /// </summary>
        public string? KoatuuCode { get; set; }
    }
}
