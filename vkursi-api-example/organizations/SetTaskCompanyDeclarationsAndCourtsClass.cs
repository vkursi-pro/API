using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class SetTaskCompanyDeclarationsAndCourtsClass
    {
        /*
        
        53. Фінансовий моніторинг пов'язаних осіб частина 1. Створення задачі
        [POST] api/1.0/Organizations/SetTaskCompanyDeclarationsAndCourts
        
        */

        public static SetTaskCompanyDeclarationsAndCourtsResponseModel SetTaskCompanyDeclarationsAndCourts(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                SetTaskCompanyDeclarationsAndCourtsBodyModel STCDACBody = new SetTaskCompanyDeclarationsAndCourtsBodyModel
                {
                    Edrpou = edrpou
                };

                string body = JsonConvert.SerializeObject(STCDACBody);

                // Example Body: {"Edrpou":"00131305"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/SetTaskCompanyDeclarationsAndCourts");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            SetTaskCompanyDeclarationsAndCourtsResponseModel STCDACResponseRow = new SetTaskCompanyDeclarationsAndCourtsResponseModel();

            STCDACResponseRow = JsonConvert.DeserializeObject<SetTaskCompanyDeclarationsAndCourtsResponseModel>(responseString);

            return STCDACResponseRow;
        }
    }
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class SetTaskCompanyDeclarationsAndCourtsBodyModel                           // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ (обмеження 1)
     /// </summary>
        public string Edrpou { get; set; }                                              // 
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class SetTaskCompanyDeclarationsAndCourtsResponseModel                       // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Id задачі який буде використовуватися для отримання даних в api/1.0/Organizations/GetTaskCompanyDeclarationsAndCourts
        /// </summary>
        public Guid? TaskId { get; set; }                                               // 
    }
}
