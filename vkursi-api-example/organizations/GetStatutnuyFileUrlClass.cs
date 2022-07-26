using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetStatutnuyFileUrlClass
    {

        /*
         
        56. Отримання статуту підприємства
        api/1.0/organizations/GetStatutnuyFileUrl
         
        */
        public static GetStatutnuyFileUrlResponseModel GetStatutnuyFileUrl(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetStatutnuyFileUrlBodyModel GSFUBody = new GetStatutnuyFileUrlBodyModel
                {
                    Code = code
                };

                string body = JsonConvert.SerializeObject(GSFUBody);

                // Example Body: {"Code":"5159752801"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetStatutnuyFileUrl");
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

            GetStatutnuyFileUrlResponseModel GSFUResponse = new GetStatutnuyFileUrlResponseModel();

            GSFUResponse = JsonConvert.DeserializeObject<GetStatutnuyFileUrlResponseModel>(responseString);

            return GSFUResponse;
        }
    }
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetStatutnuyFileUrlBodyModel                                    // 
    {/// <summary>
    /// ???
    /// </summary>
        public string Code { get; set; }
    }
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetStatutnuyFileUrlResponseModel                                // 
    {/// <summary>
    /// ???
    /// </summary>
        public bool IsSucces { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Url { get; set; }
    }
}
