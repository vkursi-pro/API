using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPersonEnforcementsBodyModel GPEBody = new GetPersonEnforcementsBodyModel
                {
                    Code = ipn,
                    LastName = LastName,                                                // Прізвище 
                    FirstName = FirstName,                                              // Ім'я
                    SecondName = SecondName                                             // По батькові
                    // FilterStatusList                                                 // Стан ВП (Приклад: Завершено, Примусове виконання, ...)
                };

                string body = JsonConvert.SerializeObject(GPEBody);

                //body = "{\"Code\":\"2951907234\",\"LastName\":null,\"FirstName\":null,\"SecondName\":null}";

                // {"Code":"2951907234","LastName":null,"FirstName":null,"SecondName":null,"FilterStatusList":null}

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    continue;
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
        public List<int> FilterStatusList { get; set; }                                 // Стан ВП (Приклад: Завершено, Примусове виконання, ...) enum - FilterStatusList
    }

    public enum FilterStatusList
    {
        [Display(Name = "Відкрито")]
        Opened = 0,

        [Display(Name = "Завершено")]
        Ended = 1,

        [Display(Name = "Зупинено")]
        Stoped = 2,

        [Display(Name = "Примусове виконання")]
        Forced = 3,

        [Display(Name = "Відмовлено у відкритті")]
        Canceled = 4,

        [Display(Name = "Закінчено")]
        EndedToo = 5,

        [Display(Name = "Завершено мінюст")]
        EndedMinjust = 6,

        [Display(Name = "Інше")]
        Other = 100,

        [Display(Name = "Нема кредитора")]
        NoCreditor = 101
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
        public string Publisher { get; set; }                                           // Документ виданий
        public string DepartmentName { get; set; }                                      // Зв'язок з виконавцем: Назва виконавця
        public string DepartmentPhone { get; set; }                                     // Зв'язок з виконавцем: Номер телефону виконавця
        public string Executor { get; set; }                                            // ПІБ виконавця
        public string ExecutorPhone { get; set; }                                       // Номер телефону виконавця
        public string ExecutorEmail { get; set; }                                       // Email виконавця

        [JsonProperty("creditorType")]
        public string CreditorType { get; set; }                                        // Тип стягувача (Фізична / Юридична / Держава / ...)

        [JsonProperty("creditorName")]
        public string CreditorName { get; set; }                                        // Назва стягувача

        [JsonProperty("creditorBithday")]
        public object CreditorBithday { get; set; }                                     // Дата народження стягувача (якщо ФО)

        [JsonProperty("debtorName")]
        public string DebtorName { get; set; }                                          // Назва боржника

        [JsonProperty("debtorCode")]
        public object DebtorCode { get; set; }                                          // Код боржника

        [JsonProperty("debtorType")]
        public string DebtorType { get; set; }                                          // Тип боржника (Фізична / Юридична / Держава / ...)

        [JsonProperty("debtorBithday")]
        public DateTimeOffset DebtorBithday { get; set; }                               // Дата народження боржника (якщо ФО)

        [JsonProperty("requestDate")]
        public DateTimeOffset RequestDate { get; set; }                                 // Час запиту
    }
}
