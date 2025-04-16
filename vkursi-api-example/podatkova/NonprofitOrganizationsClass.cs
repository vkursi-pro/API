using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.podatkova
{
    internal class NonprofitOrganizationsClass
    {
        /*
        
        199. Дані про неприбуткові організації
        [POST] /api/1.0/podatkova/nonprofitorganizations

        */

        public static NonProfitOrgApiResponse NonprofitOrganizations(ref string token, string code, bool updateNais)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CabinetTaxEdpodEpRequestBodyModel CTEERequestBody = new CabinetTaxEdpodEpRequestBodyModel
                {
                    Tins = code,                                                    // Код ЄДРПОУ або ІПН
                    UpdateNais = updateNais                                         // 
                };

                string body = JsonConvert.SerializeObject(CTEERequestBody);

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/podatkova/nonprofitorganizations");

                RestRequest restRequest = new RestRequest(Method.POST);

                restRequest.AddHeader("ContentType", "application/json");
                restRequest.AddHeader("Authorization", $"Bearer {token}");
                restRequest.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(restRequest);
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            NonProfitOrgApiResponse apiResponse = JsonConvert.DeserializeObject<NonProfitOrgApiResponse>(responseString);

            return apiResponse;
        }
    }

    /// <summary>
    /// Відповідь API для пошуку неприбуткових організацій
    /// </summary>
    public class NonProfitOrgApiResponse
    {
        /// <summary>
        /// Дані про неприбуткові організації
        /// </summary>
        public NonProfitOrganization Data { get; set; }
    }

    /// <summary>
    /// Інформація про неприбуткові організації
    /// </summary>
    public class NonProfitOrganization
    {
        /// <summary>
        /// Системний Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата реєстрації у реєстрі неприбуткових організацій
        /// </summary>
        public DateTime? DRegNoPr { get; set; }

        /// <summary>
        /// Дата присвоєння ознаки неприбутковості
        /// </summary>
        public DateTime? DNonpr { get; set; }

        /// <summary>
        /// Дата рішення
        /// </summary>
        public DateTime? DRish { get; set; }

        /// <summary>
        /// Номер рішення
        /// </summary>
        public string? NRish { get; set; }

        /// <summary>
        /// Тип рішення
        /// </summary>
        public string TRish { get; set; }

        /// <summary>
        /// Дата анулювання
        /// </summary>
        public DateTime? DAnul { get; set; }

        /// <summary>
        /// Дата рішення про анулювання
        /// </summary>
        public DateTime? DRishAnul { get; set; }

        /// <summary>
        /// Номер рішення про анулювання
        /// </summary>
        public string? NRishAnul { get; set; }

        /// <summary>
        /// Назва ДПС
        /// </summary>
        public string CStiName { get; set; }

        /// <summary>
        /// Код ЄДРПОУ ДПС
        /// </summary>
        public long? CStiTin { get; set; }

        /// <summary>
        /// Ознака неприбутковості
        /// </summary>
        public string CNonpr { get; set; }

        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string TinS { get; set; }

        /// <summary>
        /// Повна назва організації
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Дата актуального запиту
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Код ЄДРПОУ за яким був запит
        /// </summary>
        public string Code { get; set; }
    }
}
