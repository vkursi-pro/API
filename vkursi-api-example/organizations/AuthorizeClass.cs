using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class AuthorizeClass
    {
        // 1.	Авторизація та отримання токену
        // [POST] /api/1.0/token/authorize

        public static string Authorize()
        {
            // Вкажіть Ваш логін / пароль
            string body = "{\"email\": \"test@testemail.com\", \"password\": \"123456qwert\"}";
            body = "{\"email\": \"admin@admin.com\", \"password\": \"88888888\"}";

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/token/authorize", Method.POST);
            request.AddHeader("ContentType", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            TokenDeserializeResponseModel tokenDeserialize = JsonConvert.DeserializeObject<TokenDeserializeResponseModel>(responseString);
            
            string token = tokenDeserialize.Token;

            return token;
        }
    }
    class TokenDeserializeResponseModel
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
    }
}
