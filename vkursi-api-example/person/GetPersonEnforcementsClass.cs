using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.person
{
    public class GetPersonEnforcementsClass
    {
        /*

        55. Список виконавчих проваджень по фізичним особам за кодом ІПН
        [POST] /api/1.0/person/GetPersonEnforcements

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/person/GetPersonEnforcements' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1N...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"3015301315","LastName":"СТЕЛЬМАЩУК","FirstName":"АНДРІЙ","SecondName":"ВАСИЛЬОВИЧ"}'

        */

        public static GetPersonEnforcementsResponseModel GetPersonEnforcements(ref string token, string ipn, string LastName, string FirstName, string SecondName)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPersonEnforcementsBodyModel GPEBody = new GetPersonEnforcementsBodyModel
                {
                    Code = ipn,
                    LastName = LastName,                                                // Прізвище 
                    FirstName = FirstName,                                              // Ім'я
                    SecondName = SecondName                                             // По батькові
                };

                string body = JsonConvert.SerializeObject(GPEBody);

                // Example Body: {"Code":"3015301315","LastName":"СТЕЛЬМАЩУК","FirstName":"АНДРІЙ","SecondName":"ВАСИЛЬОВИЧ"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/GetPersonEnforcements");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetPersonEnforcementsResponseModel GPEResponseRow = new GetPersonEnforcementsResponseModel();

            GPEResponseRow = JsonConvert.DeserializeObject<GetPersonEnforcementsResponseModel>(responseString);

            return GPEResponseRow;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":\"3015301315\",\"LastName\":\"СТЕЛЬМАЩУК\",\"FirstName\":\"АНДРІЙ\",\"SecondName\":\"ВАСИЛЬОВИЧ\"}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiI...',
          'Content-Type': 'application/json',
          'Cookie': 'ARRAffinity=a42e49241337d9995315eeb84aeb99a9bb129de89814b49a54798016db5a2698'
        }
        conn.request("POST", "/api/1.0/person/GetPersonEnforcements", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":\"3015301315\",\"LastName\":\"СТЕЛЬМАЩУК\",\"FirstName\":\"АНДРІЙ\",\"SecondName\":\"ВАСИЛЬОВИЧ\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/person/GetPersonEnforcements")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5c...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();
     
    */

    public class GetPersonEnforcementsBodyModel                                         // Модель запиту 
    {
        public string Code { get; set; }                                                // Код ІПН
        public string LastName { get; set; }                                            // Прізвище 
        public string FirstName { get; set; }                                           // Ім'я
        public string SecondName { get; set; }                                          // По батькові
    }

    public class GetPersonEnforcementsResponseModel                                     // Модель на відповідь
    {
        public bool IsSucces { get; set; }                                              // Успішний запит (true - так / false - ні)
        public string Status { get; set; }                                              // Статус відповіді (maxLength:128)
        public List<OrgEnforcementApiAnswerModelData> Data { get; set; }                // Дані
    }

    public class OrgEnforcementApiAnswerModelData                                       // Дані
    {
        public string Edrpou { get; set; }                                              // Код ЄДРПОУ (за яким бувВП запит) (maxLength:12)
        public int TotalCount { get; set; }                                             // Загальна кількість 
        public List<OrgEnforcementApiAnswerModelDataVehicle> Enforcements { get; set; } // Пелелік виконавчих проваджень
    }

    public class OrgEnforcementApiAnswerModelDataVehicle                                // Пелелік виконавчих проваджень
    {
        public DateTime? DateOpen { get; set; }                                         // Дата відкриття ВП
        public string VpNumber { get; set; }                                            // омер ВП (maxLength:32)
        public string Status { get; set; }                                              // Статус ВП (Приклад: Боржник, Стягувач, ...) (maxLength:128)
        public string Category { get; set; }                                            // Категорія ВП (maxLength:256)
        public string State { get; set; }                                               // Стан (Приклад: Завершено, Примусове виконання, ...) (maxLength:128)
        public string Contractor { get; set; }                                          // Інша сторона (Приклад: Київське міжрегіональне управління укртрансбезпеки Код ЄДРПОУ: 37995466) (maxLength:512)
    }
}
