using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class PayNaisSignClass
    {
        /*
        
        Метод:
            59.1 Оригінальний метод Nais, на отримання повних даних (з міткою ЕЦП) за кодом NaisId який ми отримуємо з [POST] /api/1.0/organizations/freenais
            organizations/payNaisSign

        cURL запиту:

        Приклад відповіді: 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/PayNaisSignResponse.json
        */

        public static OrganizationaisElasticModel PayNaisSign(ref string token, string naisId, string code, bool needXml)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/payNaisSign");
                RestRequest request = new RestRequest(Method.GET);

                request.AddParameter("id", naisId);                             // Код Nais (який ми отримуємо з [POST] /api/1.0/organizations/freenais)
                request.AddParameter("xml", needXml);
                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                else if (!string.IsNullOrEmpty(responseString))
                {
                    // Перевіряємо на вміст помилки
                    PaynaisErrorResponseModel PaynaisErrorResponse = new PaynaisErrorResponseModel();

                    PaynaisErrorResponse = JsonConvert.DeserializeObject<PaynaisErrorResponseModel>(responseString);

                    if (PaynaisErrorResponse != null)
                    {
                        Console.WriteLine(PaynaisErrorResponse.Errors[0].Message);
                        return null;
                    }
                }
            }

            // responseString - Оригінальна відповідь Nais

            // Приклад відповіді: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/PayNaisSignResponse.json

            // Модель (опис) відповіді: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs#L130

            OrganizationaisElasticModel OrganizationaisElastic = new OrganizationaisElasticModel();

            OrganizationaisElastic = JsonConvert.DeserializeObject<OrganizationaisElasticModel>(responseString);

            return OrganizationaisElastic;
        }

    }
}
