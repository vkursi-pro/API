using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using vkursi_api_example.organizations;
using vkursi_api_example.token;

namespace vkursi_api_example.person
{
    public class CheckFopStatusClass
    {
        /*
        
        Метод:
            77. Перевірка ФОП/не ФОП, наявність ліцензій адвокат/нотаріус, наявність обтяжень ДРОРМ (доступно в конструкторі API №77)
            [POST] /api/1.0/person/CheckFopStatus

        Приклад запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/person/CheckFopStatus' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Codes":["2674001651"],"GetDrorm":true}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/CheckFopStatusResponse.json
        
        */

        public static CheckFopStatusResponseModel CheckFopStatus(ref string token, string code, bool needDrorm)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/CheckFopStatus");
                RestRequest request = new RestRequest(Method.POST);

                CheckFopStatusRequestBodyModel CIOIARequest = new CheckFopStatusRequestBodyModel
                {
                    Codes = new List<string>() { "2674001651" },
                    GetDrorm = needDrorm
                };

                string body = JsonConvert.SerializeObject(CIOIARequest); // Example: {"Codes":["2674001651"],"GetDrorm":true}

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

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            CheckFopStatusResponseModel CFSResponseRow = new CheckFopStatusResponseModel();

            CFSResponseRow = JsonConvert.DeserializeObject<CheckFopStatusResponseModel>(responseString);

            // Тут можна переглянути json відповіді
            string сheckFopStatusResponseString = JsonConvert.SerializeObject(CFSResponseRow, Formatting.Indented);

            return CFSResponseRow;
        }
    }

    /*
     
    // Java - OkHttp example:   
    
    OkHttpClient client = new OkHttpClient().newBuilder()
      .build();
    MediaType mediaType = MediaType.parse("application/json");
    RequestBody body = RequestBody.create(mediaType, "{\"Codes\":[\"2674001651\"],\"GetDrorm\":true}");
    Request request = new Request.Builder()
      .url("https://vkursi-api.azurewebsites.net/api/1.0/person/CheckFopStatus")
      .method("POST", body)
      .addHeader("ContentType", "application/json")
      .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsIn...")
      .addHeader("Content-Type", "application/json")
      .build();
    Response response = client.newCall(request).execute();


    // Python - http.client example:
    
    import http.client
    import json

    conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
    payload = json.dumps({
      "Codes": [
        "2674001651"
      ],
      "GetDrorm": True
    })
    headers = {
      'ContentType': 'application/json',
      'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cC...',
      'Content-Type': 'application/json',
    }
    conn.request("POST", "/api/1.0/person/CheckFopStatus", payload, headers)
    res = conn.getresponse()
    data = res.read()
    print(data.decode("utf-8"))

    */
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class CheckFopStatusRequestBodyModel                                 // 
    {   /// <summary>
        /// Перелік кодів ЄДРПОУ / ІПН (обмеження 5 кодів)
        /// </summary>
        public List<string> Codes { get; set; }                                 // 
        /// <summary>
        /// Чи потрібен ДРОРМ
        /// </summary>
        public bool GetDrorm { get; set; }                                      // 
        /// <summary>
        /// ПІБ фізичної особи (обмеження 5 кодів)
        /// </summary>
        public List<string> Names { get; set; }
    }
    /// <summary>
    /// Відповідь на запит
    /// </summary>
    public class CheckFopStatusResponseModel                                    // 
    {/// <summary>
     /// Запит виконано успішно (true - так / false - ні)
     /// </summary>
        public bool IsSuccess { get; set; }                                     // 
        /// <summary>
        /// Статус запиту (maxLength:128)
        /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public List<CheckFopStatusModelAnswerData> Data { get; set; }           // 
    }
    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class CheckFopStatusModelAnswerData                                  // 
    {/// <summary>
     /// Код ІПН
     /// </summary>
        public string Code { get; set; }                                        // 
        /// <summary>
        /// ФОП / не ФОП (true - так ФОП / false - ні не ФОП)
        /// </summary>
        public bool IsFop { get; set; }                                         // 
        /// <summary>
        /// Стан ФОПа (зареєстровано, припинено, в стані припинення)
        /// </summary>
        public string FopState { get; set; }                                    // 
        /// <summary>
        /// Обтяження
        /// </summary>
        public List<MovableViewModel> Movables { get; set; }                    // 
        /// <summary>
        /// Відомості про наявні ліцензії адвокатів / нотаріусів
        /// </summary>
        public List<CheckFopStatusModelAnswerDataReestrInfo> ReestrInfo { get; set; }   // 
        /// <summary>
        /// Статус реестрації за інформацією з ДПС
        /// </summary>
        public CabinetTaxRegistrationResponse RegistrationData { get; set; }    // 
        /// <summary>
        /// Дані знайдено / Не стоїть на обліку ДФС / Дані не знайдено
        /// </summary>
        public string RegistrationDataStatus { get; set; }                      // 
    }
    /// <summary>
    /// Відомості про наявні ліцензії адвокатів / нотаріусів
    /// </summary>
    public class CheckFopStatusModelAnswerDataReestrInfo                        // 
    {/// <summary>
     /// Номер ліцензії
     /// </summary>
        public string ReestrNumber { get; set; }                                // 
        /// <summary>
        /// Дата початку дії
        /// </summary>
        public DateTime? StartDate { get; set; }                                // 
        /// <summary>
        /// Дата закінчення дії
        /// </summary>
        public DateTime? EndDate { get; set; }                                  // 
        /// <summary>
        /// Стан ліцензії (1 - діюча / 2 - не діюча)
        /// </summary>
        public int StatusOfLicenseInt { get; set; }                             // 
        /// <summary>
        /// Тип ліцензії
        /// </summary>
        public string TypeOfLicense { get; set; }                               // 
        /// <summary>
        /// Id типу ліцензії (TypeOfLicense)
        /// </summary>
        public int TypeOfLicenseInt { get; set; }                               // 
        /// <summary>
        /// Орган що видав
        /// </summary>
        public string Organ { get; set; }                                       // 
        /// <summary>
        /// Id органу що видав
        /// </summary>
        public int? OrganInt { get; set; }                                      // 
        /// <summary>
        /// ПІБ власника ліцензії
        /// </summary>
        public string PersonName { get; set; }                                  // 
        /// <summary>
        /// Відосмості про ліцензію
        /// </summary>
        public CheckFopStatusLicensesInfoModel Info { get; set; }               // 
    }
    /// <summary>
    /// Статус реестрації за інформацією з ДПС
    /// </summary>
    public class CabinetTaxRegistrationResponse                                     // 
    {/// <summary>
     /// Назва
     /// </summary>
        [JsonProperty("FULL_NAME")]
        public string FullName { get; set; }                                        // 
        /// <summary>
        /// ЄДРПОУ
        /// </summary>
        [JsonProperty("TIN_S")]
        public object TinS { get; set; }                                            // 
        /// <summary>
        /// Юридична адреса
        /// </summary>
        [JsonProperty("ADRESS")]
        public object Adress { get; set; }                                          // 
        /// <summary>
        /// Дата реєстрації в органах ДПС
        /// </summary>
        [JsonProperty("D_REG_STI")]
        public string DRegSti { get; set; }                                         // 
        /// <summary>
        /// Номер реєстрації в органах ДПС
        /// </summary>
        [JsonProperty("N_REG_STI")]
        public string NRegSti { get; set; }                                         // 
        /// <summary>
        /// Назва органу ДПС де зареєстрований платник
        /// </summary>
        [JsonProperty("C_STI_MAIN_NAME")]
        public string CStiMainName { get; set; }                                    // 
        /// <summary>
        /// Ознака зовнішньоекономічної діяльності
        /// </summary>
        [JsonProperty("VED_LIC")]
        public object VedLic { get; set; }                                          // 
        /// <summary>
        /// Тип платника
        /// </summary>
        [JsonProperty("FACE_MODE")]
        public long? FaceMode { get; set; }                                         // 
        /// <summary>
        /// Стан платника
        /// </summary>
        [JsonProperty("C_STAN")]
        public long? CStan { get; set; }                                            // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("D_ZAKR_STI")]
        public string DZakrSti { get; set; }                                        // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("C_KIND")]
        public long? CKind { get; set; }                                            // 
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("C_CLOSE")]
        public long? CClose { get; set; }                                           // 
        /// <summary>
        /// Відомості про помилку
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }                                           // 
        /// <summary>
        /// Опис помилки
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }                                // 
    }
    /// <summary>
    /// Відосмості про ліцензію
    /// </summary>
    public class CheckFopStatusLicensesInfoModel                                // 
    {
        // Для адвокатів
        public string Name { get; set; }
        public string name_Name { get; set; }
        public string CouncilName { get; set; }
        public string name_CouncilName { get; set; }
        public string CertDate { get; set; }
        public string name_CertDate { get; set; }
        public string AuthCert { get; set; }
        public string name_AuthCert { get; set; }
        public string NumbDecision { get; set; }
        public string name_NumbDecision { get; set; }
        public string PersonName { get; set; }
        public string DateDecision { get; set; }
        public string name_DateDecision { get; set; }
        public string GenExp { get; set; }
        public string name_GenExp { get; set; }
        public string WorkAddres { get; set; }
        public string name_WorkAddres { get; set; }
        public string WorkMobile { get; set; }
        public string name_WorkMobile { get; set; }
        public string WorkPhonee { get; set; }
        public string name_WorkPhonee { get; set; }
        public string Email { get; set; }
        public string name_Email { get; set; }
        public string FormyDiyalnosti { get; set; }
        public string name_FormyDiyalnosti { get; set; }
        public string Address { get; set; }
        public string name_Address { get; set; }
        public string Phone { get; set; }
        public string name_Phone { get; set; }
        public string AnulInfo { get; set; }
        public string name_AnulInfo { get; set; }
        public string InshiVidomosti { get; set; }
        public string name_InshiVidomosti { get; set; }
        public string HelpInfo { get; set; }
        public string name_HelpInfo { get; set; }
        public string Link { get; set; }
        public string name_Link { get; set; }
    }
}
