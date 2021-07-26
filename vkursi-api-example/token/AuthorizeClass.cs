using Newtonsoft.Json;
using RestSharp;

namespace vkursi_api_example.token
{
    public class AuthorizeClass
    {
        /*
         
        1. Авторизація та отримання токену
        [POST] /api/1.0/token/authorize
            
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/token/authorize' \
        --header 'Content-Type: application/json' \
        --data-raw '{"email":"test@testemail.com","password":"123456"}'

        */

        public string Authorize()
        {
            // Вкажіть ваш логін та пароль від сервісу vkursi.pro які ви вводиди при реєстрации облікового запису vkursi.pro/account/register
           
            AuthorizeRequestBodyModel ARBody = new AuthorizeRequestBodyModel
            {
                Email = "admin@admin.com",       // Логін (Email)
                Password = "admin1qaz2wsx3EDC"                 // Пароль
            };

            AuthorizeResponseModel AuthorizeResponse = new AuthorizeResponseModel();

            AuthorizeResponse = GetRequest(ARBody);

            return AuthorizeResponse.Token;
        }

        public AuthorizeResponseModel GetRequest(AuthorizeRequestBodyModel ARBody)
        {
            string body = JsonConvert.SerializeObject(ARBody); // Example: {"email":"test@testemail.com","password":"123456"}
            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/token/authorize");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if ((int)response.StatusCode == 401)
            {

            }
            AuthorizeResponseModel AuthorizeResponse = JsonConvert.DeserializeObject<AuthorizeResponseModel>(responseString);

            string token = AuthorizeResponse.Token;

            // token при запитах додається в header Authorization: Bearer "token"

            return AuthorizeResponse;
        }

        public AuthorizeResponseModel Authorize(AuthorizeRequestBodyModel ARBody)
        {
            _ = new AuthorizeResponseModel();

            AuthorizeResponseModel AuthorizeResponse = GetRequest(ARBody);

            return AuthorizeResponse;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"email\":\"test@testemail.com\",\"password\":\"123456qwert\"}"
        headers = {
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/token/authorize", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"email\":\"test@testemail.com\",\"password\":\"123456qwert\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/token/authorize")
          .method("POST", body)
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    public class AuthorizeRequestBodyModel          // Модель запиту
    {
        [JsonProperty("email")]
        public string Email { get; set; }           // Логін від облікового запису vkursi.pro

        [JsonProperty("password")]
        public string Password { get; set; }        // Пароль
    }

    public class AuthorizeResponseModel             // Модель відповіді
    {
        [JsonProperty("Token")]
        public string Token { get; set; }           // Token
    }

}
