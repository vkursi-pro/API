using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.monitoring
{
    public class AddNewReestrClass
    {
        /*
         
        8. Додати новий список контрагентів (список також можна створиты з інтерфейсу на сторінці vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок
        [POST] /api/1.0/monitoring/addNewReestr
        

        */

        public static string AddNewReestr(string reestrName, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                AddNewReestrResponseModel AddNewReestrResponseModel = new AddNewReestrResponseModel
                {
                    reestrName = reestrName                                             // Назва нового списку (який буде створено)
                };

                string body = JsonConvert.SerializeObject(AddNewReestrResponseModel);   // Example body: {"reestrName":"Назва нового реєстру"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/monitoring/addnewreestr");
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            return responseString.Replace("\"","");                                     // В відповідь проходить системный id реєстру в сервісі VKursi
        }
    }

    public class AddNewReestrResponseModel                                              // Модель Body запиту
    {
        public string reestrName { get; set; }                                          // Назва нового списку (який буде створено)
    }
}
