using Newtonsoft.Json;
using RestSharp;
using System.IO;

namespace vkursi_api_example.token
{
    public class AuthorizeClass
    {
        /// <summary>
        /// 1. Авторизація (отримання токену)
        /// [POST] /api/1.0/token/authorize
        /// </summary>
        /// <returns></returns>

        /*      
            cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/token/authorize' \
                --header 'Content-Type: application/json' \
                --data-raw '{"email":"test@testemail.com","password":"123456"}'

            * - Термін дії токену 30 хв (після цого ви отримаєте помилку 401 unauthorized)

        */
        public string Authorize()
        {
            // Вкажіть ваш логін та пароль від сервісу Vkursi які ви вводили при реєстрації облікового запису vkursi.pro/account/register або зарееструйте новий

            AuthorizeRequestBodyModel ARBody = new AuthorizeRequestBodyModel
            {
                Email = "Логін (Email)",       // Логін (Email)
                Password = "Пароль"            // Пароль
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

        public AuthorizeClass()
        {
            string directory = Directory.GetCurrentDirectory();

            FileInfo fileInfo = new FileInfo($"{directory}\\files\\authParam.txt");

            if (fileInfo.Exists)
            {
                string[] authParam = File.ReadAllText(fileInfo.FullName).Split('\t');
                Email = authParam[0];
                Password = authParam[1];
            }
        }
        public string Email { get; set; }
        public string Password { get; set; }
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
    /// <summary>
    /// Модель запиту
    /// </summary>
    public class AuthorizeRequestBodyModel          // 
    {/// <summary>
     /// Логін від облікового запису vkursi.pro
     /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }           // 
        /// <summary>
        /// Пароль
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }        // 
    }
    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class AuthorizeResponseModel             // 
    {/// <summary>
     /// Token
     /// </summary>
        [JsonProperty("Token")]
        public string Token { get; set; }           // 
    }

}