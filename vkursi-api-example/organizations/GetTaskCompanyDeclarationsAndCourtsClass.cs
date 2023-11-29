using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using vkursi_api_example.organizations;

namespace vkursi_api_example.organizations
{
    public class GetTaskCompanyDeclarationsAndCourtsClass
    {
        /*
        
        54. Фінансовий моніторинг пов'язаних осіб частина 2. Отримуємо результат виконання задачі
        [POST] api/1.0/Organizations/GetTaskCompanyDeclarationsAndCourts

        ВАЖЛИВО: При отриманні відповіді звертайте увагу на "Status":"Задача виконана" в даному полі відобразається статус виконання

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/Organizations/GetTaskCompanyDeclarationsAndCourts' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{
            "taskId": "a455040a-15e3-438c-87c4-93481a5f292b"                            // taskId отриманий з 53. [POST] api/1.0/Organizations/SetTaskCompanyDeclarationsAndCourts
        }'

        */

        public static GetTaskCompanyDeclarationsAndCourtsResponseModel GetTaskCompanyDeclarationsAndCourts(ref string token, Guid taskId)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetTaskCompanyDeclarationsAndCourtsBodyModel GTCDACBody = new GetTaskCompanyDeclarationsAndCourtsBodyModel
                {
                    TaskId = taskId                                                     // taskId отриманий з 53. [POST] api/1.0/Organizations/SetTaskCompanyDeclarationsAndCourts
                };

                string body = JsonConvert.SerializeObject(GTCDACBody);

                // Example Body: {"TaskId":"691e940c-b61e-4feb-ad1f-fa22c365633f"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/gettaskcompanydeclarationsandcourts");
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

            GetTaskCompanyDeclarationsAndCourtsResponseModel GTCDACResponseRow = new GetTaskCompanyDeclarationsAndCourtsResponseModel();

            GTCDACResponseRow = JsonConvert.DeserializeObject<GetTaskCompanyDeclarationsAndCourtsResponseModel>(responseString);

            return GTCDACResponseRow;
        }
    }
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetTaskCompanyDeclarationsAndCourtsBodyModel                           // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ (обмеження 1)
     /// </summary>
        public Guid TaskId { get; set; }                                                // 
    }

    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class GetTaskCompanyDeclarationsAndCourtsResponseModel                       // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        [JsonProperty("isSuccess")] 
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        [JsonProperty("status")] 
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Дані відповіді
        /// </summary>
        [JsonProperty("data")] 
        public List<Datum> Data { get; set; }                                           // 
    }
    /// <summary>
    /// Дані відповіді
    /// </summary>
    public partial class Datum                                                          // 
    {/// <summary>
     /// Початкові параметри запиту
     /// </summary>
        [JsonProperty("zapyt")] 
        public Zapyt Zapyt { get; set; }                                                // 
        /// <summary>
        /// Зв'язки компанії
        /// </summary>
        [JsonProperty("companyRelations")] 
        public List<CompanyRelation> CompanyRelations { get; set; }                     // 
        /// <summary>
        /// Операції ЗЕД по країнам з офшорних зон
        /// </summary>
        [JsonProperty("zedKrayiny")] 
        public List<ZedKrayiny> ZedKrayiny { get; set; }                                // 
        /// <summary>
        /// Зв'язки з деклараціями
        /// </summary>
        [JsonProperty("deklaratsiyi")] 
        public List<Deklaratsiyi> Deklaratsiyi { get; set; }                            // 
        /// <summary>
        /// Судові рішення
        /// </summary>
        [JsonProperty("sudoviRishennya")] 
        public List<SudoviRishennya> SudoviRishennya { get; set; }                      // 
        /// <summary>
        /// Судові справи призначені до розгляду
        /// </summary>
        [JsonProperty("sudoviPryznacheni")] 
        public List<SudoviPryznacheni> SudoviPryznacheni { get; set; }                  // 

    }
    /// <summary>
    /// Зв'язки компанії
    /// </summary>
    public partial class CompanyRelation                                                // 
    {/// <summary>
     /// Назва ЮО / ФО
     /// </summary>
        [JsonProperty("nazva")] 
        public string Nazva { get; set; }                                               // 
        /// <summary>
        /// Тип звязку:
        /// FounderType Бенефіціар
        /// LocationType Адреса
        /// ChiefType Керівник
        /// OldNodeAdressType Попередня адреса
        /// OldNodeFounder Попередній бенефіціар
        /// OldNodeChiefType Попередній керівник
        /// OldNodeNameType Попередня назва
        /// Branch Філія
        /// Shareholder Власники пакетів акцій
        /// Assignee Правонаступник
        /// </summary>
        [JsonProperty("typZvyazku")] 
        public string TypZvyazku { get; set; }

        /// <summary>
        /// Це компанія? true - так / false - ні
        /// </summary>
        [JsonProperty("tseKompaniya")] 
        public bool TseKompaniya { get; set; }                                          // 
        /// <summary>
        /// Адреса
        /// </summary>
        [JsonProperty("adresa")] 
        public string Adresa { get; set; }                                              // 
        /// <summary>
        /// Частка в статутному капіталі, грн
        /// </summary>
        [JsonProperty("chastka")] 
        public double? Chastka { get; set; }                                            // 
        /// <summary>
        /// Частка в статутному капіталі, %
        /// </summary>
        [JsonProperty("chastkaVidsotok")] 
        public double? ChastkaVidsotok { get; set; }                                    // 
        /// <summary>
        /// Країна
        /// </summary>
        [JsonProperty("krayina")] 
        public string Krayina { get; set; }                                             // 
        /// <summary>
        /// Публічна особа true - так / false - ні
        /// </summary>
        [JsonProperty("publichnaOsoba")] 
        public long? PublichnaOsoba { get; set; }                                       // 
        /// <summary>
        /// Назва країни офшорної зони
        /// </summary>
        [JsonProperty("ofshornaZona")] 
        public string OfshornaZona { get; set; }                                        // 
        /// <summary>
        /// Перелік оффшорних списків до яких входить дана країна через ";" Приклад: FATF;APG;EAG
        /// </summary>
        [JsonProperty("ofshornaZonaTyp")] 
        public string OfshornaZonaTyp { get; set; }                                     // 
        /// <summary>
        /// Перелік санкційних реестрів
        /// </summary>
        public List<int> SanctionsList { get; set; }                                    // 
        /// <summary>
        /// Код ЄДРПОУ / ІПН
        /// </summary>
        [JsonProperty("cod")]
        public string Cod { get; set; }                                                 // 
    }
    /// <summary>
    /// Зв'язки з деклараціями
    /// </summary>
    public partial class Deklaratsiyi                                                   // 
    {/// <summary>
     /// ПІБ
     /// </summary>
        [JsonProperty("pib")] 
        public string Pib { get; set; }                                                 // 
        /// <summary>
        /// Піб особи яка подавала декларацію
        /// </summary>
        [JsonProperty("pibDeklaranta")] 
        public string PibDeklaranta { get; set; }                                       // 
        /// <summary>
        /// Період подання
        /// </summary>
        [JsonProperty("rikDeklaratsiyi")] 
        public string RikDeklaratsiyi { get; set; }                                     // 
        /// <summary>
        /// Тип декларації (Щорічна / Кандидата на посаду / Перед звільненням / Після звільнення )
        /// </summary>
        [JsonProperty("typDklaratsiyi")] 
        public string TypDklaratsiyi { get; set; }                                      // 
        /// <summary>
        /// Тип особи
        /// </summary>
        [JsonProperty("typOsoby")] 
        public string TypOsoby { get; set; }                                            // 
        /// <summary>
        /// Публічна особа 1 - особа яка вказана в декларації публічної особи / 2-  публічна особи / null - не публічна/не визначено
        /// </summary>
        [JsonProperty("publichnaOsoba")] 
        public int? PublichnaOsoba { get; set; }                                        //  
        /// <summary>
        /// Посада
        /// </summary>
        [JsonProperty("posada")] 
        public string Posada { get; set; }                                              // 
        /// <summary>
        /// Розділ декларації в якому знайдено інформацію 
        /// </summary>
        [JsonProperty("vkazanyyRozdil")] 
        public string VkazanyyRozdil { get; set; }                                      // 
        /// <summary>
        /// Тип операцій
        /// </summary>
        [JsonProperty("typRozdily")] 
        public string TypRozdily { get; set; }                                          // 
        /// <summary>
        /// Зазальна сума
        /// </summary>
        [JsonProperty("zahalnaSuma")] 
        public double? ZahalnaSuma { get; set; }                                        // 
        /// <summary>
        /// Валюта
        /// </summary>
        [JsonProperty("valyuta")] 
        public string Valyuta { get; set; }                                             // 
        /// <summary>
        /// Id декларації (можна сформувати посилання:https://public.nazk.gov.ua/declaration/8bfb39b3-7ebf-43e3-91db-c6c27836b443)
        /// </summary>
        [JsonProperty("idDeklaratsiyi")] 
        public Guid? IdDeklaratsiyi { get; set; }                                       //  
    }
    /// <summary>
    /// Судові рішення
    /// </summary>
    public partial class SudoviRishennya                                                // 
    {/// <summary>
     /// ПІБ особи по якій знайдені судові рішення
     /// </summary>
        [JsonProperty("pib")] 
        public string Pib { get; set; }                                                 // 
        /// <summary>
        /// Назва суду
        /// </summary>
        [JsonProperty("courtName")] 
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Форма судочинства (Кримінальне / Цівільне / ...)
        /// </summary>
        [JsonProperty("courtType")] 
        public string CourtType { get; set; }                                           // 
        /// <summary>
        /// Номер документа
        /// </summary>
        [JsonProperty("caseNumber")] 
        public string CaseNumber { get; set; }                                          // 
        /// <summary>
        /// № провадження
        /// </summary>
        [JsonProperty("caseProc")] 
        public string CaseProc { get; set; }                                            // 
        /// <summary>
        /// Дата рішення
        /// </summary>
        [JsonProperty("registrationDate")] 
        public DateTime? RegistrationDate { get; set; }                                 // 
        /// <summary>
        /// Головуючий суддя
        /// </summary>
        [JsonProperty("judge")] 
        public string Judge { get; set; }                                               // 
        /// <summary>
        /// Суддя-доповідач
        /// </summary>
        [JsonProperty("judges")] 
        public string Judges { get; set; }                                              // 
        /// <summary>
        /// Учасники
        /// </summary>
        [JsonProperty("participants")] 
        public string Participants { get; set; }                                        // 
        /// <summary>
        /// Дата наступної події
        /// </summary>
        [JsonProperty("stageDate")] 
        public DateTime? StageDate { get; set; }                                        // 
        /// <summary>
        /// Стадія розгляду
        /// </summary>
        [JsonProperty("stageName")] 
        public string StageName { get; set; }                                           // 
        /// <summary>
        /// Результат
        /// </summary>
        [JsonProperty("causeResult")] 
        public string CauseResult { get; set; }                                         // 
        /// <summary>
        /// Тип заяви
        /// </summary>
        [JsonProperty("type")] 
        public int? Type { get; set; }                                                  // 
        /// <summary>
        /// Предмет позову
        /// </summary>
        [JsonProperty("description")] 
        public string Description { get; set; }                                         // 
        /// <summary>
        /// Id судового документа (за ким можна отримати контент)
        /// </summary>
        [JsonProperty("id")] 
        public string Id { get; set; }                                                  // 
    }
    /// <summary>
    /// Судові справи призначені до розгляду
    /// </summary>
    public partial class SudoviPryznacheni                                              // 
    {/// <summary>
     /// ПІБ особи по якій знайдені судові рішення
     /// </summary>
        [JsonProperty("pib")]
        public string Pib { get; set; }                                                 // 
        /// <summary>
        /// Назва суду
        /// </summary>
        [JsonProperty("courtName")]
        public string CourtName { get; set; }                                           // 
        /// <summary>
        /// Форма судочинства (Кримінальне / Цівільне / ...)
        /// </summary>
        [JsonProperty("courtType")]
        public string CourtType { get; set; }                                           // 
        /// <summary>
        /// Номер документа
        /// </summary>
        [JsonProperty("caseNumber")]
        public string CaseNumber { get; set; }                                          // 
        /// <summary>
        /// № провадження
        /// </summary>
        [JsonProperty("caseProc")]
        public string CaseProc { get; set; }                                            // 
        /// <summary>
        /// Дата рішення
        /// </summary>
        [JsonProperty("registrationDate")]
        public DateTime? RegistrationDate { get; set; }                                 // 
        /// <summary>
        /// Головуючий суддя
        /// </summary>
        [JsonProperty("judge")]
        public string Judge { get; set; }                                               // 
        /// <summary>
        /// Суддя-доповідач
        /// </summary>
        [JsonProperty("judges")]
        public string Judges { get; set; }                                              // 
        /// <summary>
        /// Учасники
        /// </summary>
        [JsonProperty("participants")]
        public string Participants { get; set; }                                        // 
        /// <summary>
        /// Предмет позову
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }                                         // 
        /// <summary>
        /// Id судового документа (за ким можна отримати контент)
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }                                                  // 
    }
    /// <summary>
    /// Початкові параметри запиту
    /// </summary>
    public partial class Zapyt                                                          // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        [JsonProperty("edrpou")] 
        public string Edrpou { get; set; }                                              // 
    }
    /// <summary>
    /// Операції ЗЕД по країнам з офшорних зон
    /// </summary>
    public partial class ZedKrayiny                                                     // 
    {/// <summary>
     /// Рік
     /// </summary>
        [JsonProperty("rik")] 
        public long? Rik { get; set; }                                                  // 
        /// <summary>
        /// Це імпорт? true - так(імпорт) / false - ні(єкспорт)
        /// </summary>
        [JsonProperty("tseImport")] 
        public bool? TseImport { get; set; }                                            // 
        /// <summary>
        /// Код країни
        /// </summary>
        [JsonProperty("cod")] 
        public long? Cod { get; set; }                                                  // 
        /// <summary>
        /// Назва країни
        /// </summary>
        [JsonProperty("krayina")]
        public string Krayina { get; set; }                                             // 
        /// <summary>
        /// Загальна сума імпорту/єкспорту
        /// </summary>
        [JsonProperty("zahalnaSuma")] 
        public double? ZahalnaSuma { get; set; }                                        // 
        /// <summary>
        /// Позиція в рейтингу імпортерів/єкспортерів в дану країну в данному році
        /// </summary>
        [JsonProperty("pozytsiya")] 
        public long? Pozytsiya { get; set; }                                            // 
        /// <summary>
        /// Відсоток імпорту/єкспорту в дану країну в данному році від всіх імпортерів/єкспортерів
        /// </summary>
        [JsonProperty("vidsotok")] 
        public double? Vidsotok { get; set; }                                           // 
        /// <summary>
        /// Назва країни офшорної зони
        /// </summary>
        [JsonProperty("ofshornaZona")] 
        public string OfshornaZona { get; set; }                                        // 
        /// <summary>
        /// Перелік оффшорних списків до яких входить дана країна через ";" Приклад: FATF;APG;EAG
        /// </summary>
        [JsonProperty("ofshornaZonaTyp")] 
        public string OfshornaZonaTyp { get; set; }                                     // 
    }
}
