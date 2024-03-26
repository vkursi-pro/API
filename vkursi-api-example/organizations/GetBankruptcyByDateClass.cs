using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.enforcement;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.Bankruptcy
{
    public static class GetBankruptcyByDateClass
    {

        /// <summary>
        /// 162. Відомості про банкрутство ВГСУ по даті
        /// [POST] /api/1.0/organizations/getBankruptcyByDate
        /// </summary>
        /// <param name="token"></param>
        /// <param name="searchDate"></param>
        /// <returns></returns>
        /*
        
        cURL:
            curl --location 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getBankruptcyByDate' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
            --header 'Content-Type: application/json' \
            --data '{ "Date" : "2023-04-14T00:00:00" }' 
         
        */

        public static GetBankruptcyByDateModelAnswer GetBankruptcyByDate(ref string token, DateTime searchDate)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetBankruptcyByDateBodyModel requestBodyModel = new GetBankruptcyByDateBodyModel
                {
                    Date = searchDate.ToString("yyyy-MM-ddTHH:mm:ss"),                  // обов'язвоий параметр
                };

                string body = JsonConvert.SerializeObject(requestBodyModel,             // Example body: { "Date" : "2023-05-08T00:00:00" }
                              new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getBankruptcyByDate");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    // Якщо користувач не авторизований або токен застарів
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Unexpected server error"))
                {
                    Console.WriteLine("Unexpected server error");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Введіть коректний Date формату"))
                {
                    Console.WriteLine("Введіть коректний Date формату 2023-04-14T00:00:00");
                    return null;
                }

            }

            GetBankruptcyByDateModelAnswer BankruptcyByDateResponse = new GetBankruptcyByDateModelAnswer();

            BankruptcyByDateResponse = JsonConvert.DeserializeObject<GetBankruptcyByDateModelAnswer>(responseString);

            return BankruptcyByDateResponse;
        }
    }
    public class GetBankruptcyByDateBodyModel
    {
        /// <summary>
        /// дата має бути в форматі "2023-04-14T00:00:00" і це обов'язковий параметр
        /// </summary>
        public string Date { get; set; }
    }


    /// <summary>
    /// Модель відповіді на API 162. Відомості про банкрутство ВГСУ за вказану(в запиті) дату
    /// </summary>
    public class GetBankruptcyByDateModelAnswer
    {
        /// <summary>
        /// Чи виконано запит успішно
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Статус запиту
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Масив даних
        /// </summary>
        public List<GetBankruptcyByDateModelAnswerData> Data { get; set; }
    }

    public class GetBankruptcyByDateModelAnswerData
    {
        /// <summary>
        /// Код ЄДРПОУ або ІПН 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Колекція записів про банкрутство, асоційованих із суб'єктом, ідентифікованим за кодом ЄДРПОУ або ІПН (Code).
        /// </summary>
        public List<ApiOrganizationMessageAnswerModelDataVgsu> VgsuData { get; set; }
        
    }
    public class ApiOrganizationMessageAnswerModelDataVgsu
    {
        /// <summary>
        /// Номер судової справи
        /// </summary>
        public string CaseNumber { get; set; }
        /// <summary>
        /// Назва суду, який здійснює провадження у справі
        /// </summary>
        public string CourtName { get; set; }
        /// <summary>
        /// Дата публікації повідомлення
        /// </summary>
        public DateTime? DateProclamation { get; set; }
        /// <summary>
        /// Дата закінчення справи про банкрутсво
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Посилання на документ провідкриття судового ровадження
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Найменування суб'єкта банкрутсва
        /// </summary>
        public string NameDebtor { get; set; }
        /// <summary>
        /// Номер повідомлення
        /// </summary>
        public string NumberAdvert { get; set; }
        /// <summary>
        /// Тип публікації про банкрутсво
        /// </summary>
        public string PublicationType { get; set; }
        /// <summary>
        /// Дата початку справи про банкруцтво
        /// </summary>
        public DateTime? StartDate { get; set; }
    }
}
