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
                body = "{\"code\":\"3116320127\"}"; //             GetPersonEnforcementsClass.GetPersonEnforcements(ref token, "2951907234", "ЗАЙЧЕНКО", "МАКСИМ", "ВОЛОДИМИРОВИЧ");

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
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetPersonEnforcementsBodyModel                                         // 
    {/// <summary>
     /// Код ІПН
     /// </summary>
        public string Code { get; set; }                                                // 
        /// <summary>
        /// Прізвище 
        /// </summary>
        public string LastName { get; set; }                                            // 
        /// <summary>
        /// Ім'я
        /// </summary>
        public string FirstName { get; set; }                                           // 
        /// <summary>
        /// По батькові
        /// </summary>
        public string SecondName { get; set; }                                          // 
        /// <summary>
        /// Стан ВП (Приклад: Завершено, Примусове виконання, ...) enum - FilterStatusList
        /// </summary>
        public List<int> FilterStatusList { get; set; }                                 // 
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
    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetPersonEnforcementsResponseModel                                     // 
    {/// <summary>
     /// Успішний запит (true - так / false - ні)
     /// </summary>
        public bool IsSucces { get; set; }                                              // 
        /// <summary>
        /// Статус відповіді (maxLength:128)
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<OrgEnforcementApiAnswerModelData> Data { get; set; }                // 
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class OrgEnforcementApiAnswerModelData                                       // 
    {/// <summary>
     /// Код ЄДРПОУ (за яким бувВП запит) (maxLength:12)
     /// </summary>
        public string Edrpou { get; set; }                                              // 
        /// <summary>
        /// Загальна кількість 
        /// </summary>
        public int TotalCount { get; set; }                                             // 
        /// <summary>
        /// Пелелік виконавчих проваджень
        /// </summary>
        public List<OrgEnforcementApiAnswerModelDataVehicle> Enforcements { get; set; } // 
    }
    /// <summary>
    /// Пелелік виконавчих проваджень
    /// </summary>
    public class OrgEnforcementApiAnswerModelDataVehicle                                // 
    {/// <summary>
     /// Дата відкриття ВП
     /// </summary>
        public DateTime? DateOpen { get; set; }                                         // 
        /// <summary>
        /// Номер ВП (maxLength:32)
        /// </summary>
        public string VpNumber { get; set; }                                            // 
        /// <summary>
        /// Статус ВП (Приклад: Боржник, Стягувач, ...) (maxLength:128)
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Категорія ВП (maxLength:256)
        /// </summary>
        public string Category { get; set; }                                            // 
        /// <summary>
        /// Стан (Приклад: Завершено, Примусове виконання, ...) (maxLength:128)
        /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Інша сторона (Приклад: Київське міжрегіональне управління укртрансбезпеки Код ЄДРПОУ: 37995466) (maxLength:512)
        /// </summary>
        public string Contractor { get; set; }                                          // 
        /// <summary>
        /// Документ виданий
        /// </summary>
        public string Publisher { get; set; }                                           // 
        /// <summary>
        /// Зв'язок з виконавцем: Назва виконавця
        /// </summary>
        public string DepartmentName { get; set; }                                      // 
        /// <summary>
        /// Зв'язок з виконавцем: Номер телефону виконавця
        /// </summary>
        public string DepartmentPhone { get; set; }                                     // 
        /// <summary>
        /// ПІБ виконавця
        /// </summary>
        public string Executor { get; set; }                                            // 
        /// <summary>
        /// Номер телефону виконавця
        /// </summary>
        public string ExecutorPhone { get; set; }                                       // 
        /// <summary>
        /// Email виконавця
        /// </summary>
        public string ExecutorEmail { get; set; }                                       // 
        /// <summary>
        /// Тип стягувача (Фізична / Юридична / Держава / ...)
        /// </summary>
        [JsonProperty("creditorType")]
        public string CreditorType { get; set; }                                        // 
        /// <summary>
        /// Назва стягувача
        /// </summary>
        [JsonProperty("creditorName")]
        public string CreditorName { get; set; }                                        // 
        /// <summary>
        /// Дата народження стягувача (якщо ФО)
        /// </summary>
        [JsonProperty("creditorBithday")]
        public object CreditorBithday { get; set; }                                     // 
        /// <summary>
        /// Назва боржника
        /// </summary>
        [JsonProperty("debtorName")]
        public string DebtorName { get; set; }                                          // 
        /// <summary>
        /// Код боржника
        /// </summary>
        [JsonProperty("debtorCode")]
        public object DebtorCode { get; set; }                                          // 
        /// <summary>
        /// Тип боржника (Фізична / Юридична / Держава / ...)
        /// </summary>
        [JsonProperty("debtorType")]
        public string DebtorType { get; set; }                                          // 
        /// <summary>
        /// Дата народження боржника (якщо ФО)
        /// </summary>
        [JsonProperty("debtorBithday")]
        public DateTimeOffset DebtorBithday { get; set; }                               // 
        /// <summary>
        /// Час запиту
        /// </summary>
        [JsonProperty("requestDate")]
        public DateTimeOffset RequestDate { get; set; }                                 // 
    }
}
