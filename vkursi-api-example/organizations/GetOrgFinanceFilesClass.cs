using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public static class GetOrgFinanceFilesClass
    {
        /*

        164. Отримання відповіді з файлом zip наповненим xml та pdf файлами
        фінансової звітності підприємств

        ЗВЕРНІТЬ БУДЬ-ЛАСКА УВАГУ!!!
        1. На даний момент отримання файлів фінансової звітності доступні лише за 2024 рік 
        Файли про фінансову звітність до 2024 року перебувають в процесі наповлення.

        [POST] api/1.0/organizations/GetOrgFinanceFiles

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinanceFiles' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"00131512","periodYear": 2024, "periodType": 3}'
        */

        public static GetXmlAndPdfZipResponse GetOrgFinanceFiles (ref string token, string code, int periodYear, int periodType)
        {

            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinanceFiles");
                RestRequest request = new RestRequest(Method.POST);

                GetXmlAndPdfZip GOFRequesRow = new GetXmlAndPdfZip
                {
                    Code = code,                                             //00131512
                    periodYear = periodYear,                                 //2024
                    periodType = periodType                                  // 3 - перший квартал, 6 - півріччя, 9 - дев'ять місяців, 12 - річна
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

            GetXmlAndPdfZipResponse GOFResponseRow = new GetXmlAndPdfZipResponse();

            GOFResponseRow = JsonConvert.DeserializeObject<GetXmlAndPdfZipResponse>(responseString);
            byte[] fileBytes = Convert.FromBase64String(GOFResponseRow.FileData);
            File.WriteAllBytes("C:\\WorkDirectory\\file.zip", fileBytes);

            return GOFResponseRow;
        }

    }
    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetXmlAndPdfZip
    {/// <summary>
    /// Код ЄДРПОУ
    /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Рік
        /// </summary>
        public int periodYear { get; set; }
        /// <summary>
        /// 3 - перший квартал, 6 - півріччя, 9 - дев'ять місяців, 12 - річна
        /// </summary>
        public int periodType { get; set; }
    }
    /// <summary>
    /// Модель відповіді 
    /// </summary>
    public class GetXmlAndPdfZipResponse
    {
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Формат даних base64, формат файлу .zip
        /// </summary>
        public string FileData { get; set; }
    }
}
