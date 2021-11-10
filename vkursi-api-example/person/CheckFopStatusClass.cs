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

    public class CheckFopStatusRequestBodyModel                                 // Модель запиту 
    {
        public List<string> Codes { get; set; }                                 // Перелік кодів ЄДРПОУ / ІПН (обмеження 5 кодів)
        public bool GetDrorm { get; set; }                                      // Чи потрібен ДРОРМ
    }

    public class CheckFopStatusResponseModel                                    // Відповідь на запит
    {
        public bool IsSuccess { get; set; }                                     // Запит виконано успішно (true - так / false - ні)
        public string Status { get; set; }                                      // Статус запиту (maxLength:128)
        public List<CheckFopStatusModelAnswerData> Data { get; set; }           // Дані відповіді
    }

    public class CheckFopStatusModelAnswerData                                  // Дані відповіді
    {
        public string Code { get; set; }                                        // Код ІПН
        public bool IsFop { get; set; }                                         // ФОП / не ФОП (true - так ФОП / false - ні не ФОП)
        public string FopState { get; set; }                                    // Стан ФОПа (зареєстровано, припинено, в стані припинення)
        public List<MovableViewModel> Movables { get; set; }                    // Обтяження
        public List<CheckFopStatusModelAnswerDataReestrInfo> ReestrInfo { get; set; }   // Відомості про наявні ліцензії адвокатів / нотаріусів
        public CabinetTaxRegistrationResponse RegistrationData { get; set; }    // Статус реестрації за інформацією з ДПС
        public string RegistrationDataStatus { get; set; }                      // Дані знайдено / Не стоїть на обліку ДФС / Дані не знайдено
    }

    public class CheckFopStatusModelAnswerDataReestrInfo                        // Відомості про наявні ліцензії адвокатів / нотаріусів
    {
        public string ReestrNumber { get; set; }                                // Номер ліцензії
        public DateTime? StartDate { get; set; }                                // Дата початку дії
        public DateTime? EndDate { get; set; }                                  // Дата закінчення дії
        public int StatusOfLicenseInt { get; set; }                             // Стан ліцензії (1 - діюча / 2 - не діюча)
        public string TypeOfLicense { get; set; }                               // Тип ліцензії
        public int TypeOfLicenseInt { get; set; }                               // Id типу ліцензії (TypeOfLicense)
        public string Organ { get; set; }                                       // Орган що видав
        public int? OrganInt { get; set; }                                      // Id органу що видав
        public string PersonName { get; set; }                                  // ПІБ власника ліцензії
        public CheckFopStatusLicensesInfoModel Info { get; set; }               // Відосмості про ліцензію
    }

    public class CabinetTaxRegistrationResponse                                     // Статус реестрації за інформацією з ДПС
    {
        [JsonProperty("FULL_NAME")]
        public string FullName { get; set; }                                        // Назва

        [JsonProperty("TIN_S")]
        public object TinS { get; set; }                                            // ЄДРПОУ

        [JsonProperty("ADRESS")]
        public object Adress { get; set; }                                          // Юридична адреса

        [JsonProperty("D_REG_STI")]
        public string DRegSti { get; set; }                                         // Дата реєстрації в органах ДПС

        [JsonProperty("N_REG_STI")]
        public string NRegSti { get; set; }                                         // Номер реєстрації в органах ДПС

        [JsonProperty("C_STI_MAIN_NAME")]
        public string CStiMainName { get; set; }                                    // Назва органу ДПС де зареєстрований платник

        [JsonProperty("VED_LIC")]
        public object VedLic { get; set; }                                          // Ознака зовнішньоекономічної діяльності

        [JsonProperty("FACE_MODE")]
        public long? FaceMode { get; set; }                                         // Тип платника

        [JsonProperty("C_STAN")]
        public long? CStan { get; set; }                                            // Стан платника

        [JsonProperty("D_ZAKR_STI")]
        public string DZakrSti { get; set; }                                        // 

        [JsonProperty("C_KIND")]
        public long? CKind { get; set; }                                            // 

        [JsonProperty("C_CLOSE")]
        public long? CClose { get; set; }                                           // 

        [JsonProperty("error")]
        public string Error { get; set; }                                           // Відомості про помилку

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }                                // Опис помилки
    }

    public class CheckFopStatusLicensesInfoModel                                // Відосмості про ліцензію
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
