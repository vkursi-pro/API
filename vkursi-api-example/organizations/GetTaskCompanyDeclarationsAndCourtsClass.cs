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
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

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
                    token = AuthorizeClass.Authorize();
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

    public class GetTaskCompanyDeclarationsAndCourtsBodyModel                           // Модель запиту 
    {
        public Guid TaskId { get; set; }                                                // Перелік кодів ЄДРПОУ (обмеження 1)
    }


    public class GetTaskCompanyDeclarationsAndCourtsResponseModel                       // Модель на відповідь
    {
        [JsonProperty("isSuccess")] 
        public bool IsSuccess { get; set; }                                             // Чи успішний запит

        [JsonProperty("status")] 
        public string Status { get; set; }                                              // Статус відповіді по API

        [JsonProperty("data")] 
        public List<Datum> Data { get; set; }                                           // Дані відповіді
    }

    public partial class Datum                                                          // Дані відповіді
    {
        [JsonProperty("zapyt")] 
        public Zapyt Zapyt { get; set; }                                                // Початкові параметри запиту

        [JsonProperty("companyRelations")] 
        public List<CompanyRelation> CompanyRelations { get; set; }                     // Зв'язки компанії

        [JsonProperty("zedKrayiny")] 
        public List<ZedKrayiny> ZedKrayiny { get; set; }                                // Операції ЗЕД по країнам з офшорних зон

        [JsonProperty("deklaratsiyi")] 
        public List<Deklaratsiyi> Deklaratsiyi { get; set; }                            // Зв'язки з деклараціями

        [JsonProperty("sudoviRishennya")] 
        public List<SudoviRishennya> SudoviRishennya { get; set; }                      // Судові рішення

        [JsonProperty("sudoviPryznacheni")] 
        public List<SudoviPryznacheni> SudoviPryznacheni { get; set; }                  // Судові справи призначені до розгляду

    }

    public partial class CompanyRelation                                                // Зв'язки компанії
    {
        [JsonProperty("nazva")] 
        public string Nazva { get; set; }                                               // Назва ЮО / ФО

        [JsonProperty("typZvyazku")] 
        public string TypZvyazku { get; set; }                                          // Тип звязку:        
        
                                                                                        // FounderType Бенефіціар
                                                                                        // LocationType Адреса
                                                                                        // ChiefType Керівник
                                                                                        // OldNodeAdressType Попередня адреса
                                                                                        // OldNodeFounder Попередній бенефіціар
                                                                                        // OldNodeChiefType Попередній керівник
                                                                                        // OldNodeNameType Попередня назва
                                                                                        // Branch Філія
                                                                                        // Shareholder Власники пакетів акцій
                                                                                        // Assignee Правонаступник

        [JsonProperty("tseKompaniya")] 
        public bool TseKompaniya { get; set; }                                          // Це компанія? true - так / false - ні

        [JsonProperty("adresa")] 
        public string Adresa { get; set; }                                              // Адреса

        [JsonProperty("chastka")] 
        public double? Chastka { get; set; }                                            // Частка в статутному капіталі, грн

        [JsonProperty("chastkaVidsotok")] 
        public double? ChastkaVidsotok { get; set; }                                    // Частка в статутному капіталі, %

        [JsonProperty("krayina")] 
        public string Krayina { get; set; }                                             // Країна

        [JsonProperty("publichnaOsoba")] 
        public long? PublichnaOsoba { get; set; }                                       // Публічна особа true - так / false - ні

        [JsonProperty("ofshornaZona")] 
        public string OfshornaZona { get; set; }                                        // Назва країни офшорної зони

        [JsonProperty("ofshornaZonaTyp")] 
        public string OfshornaZonaTyp { get; set; }                                     // Перелік оффшорних списків до яких входить дана країна через ";" Приклад: FATF;APG;EAG
        public List<int> SanctionsList { get; set; }                                    // Перелік санкційних реестрів

        [JsonProperty("cod")]
        public string Cod { get; set; }                                                 // Код ЄДРПОУ / ІПН
    }

    public partial class Deklaratsiyi                                                   // Зв'язки з деклараціями
    {
        [JsonProperty("pib")] 
        public string Pib { get; set; }                                                 // ПІБ

        [JsonProperty("pibDeklaranta")] 
        public string PibDeklaranta { get; set; }                                       // Піб особи яка подавала декларацію

        [JsonProperty("rikDeklaratsiyi")] 
        public string RikDeklaratsiyi { get; set; }                                     // Період подання

        [JsonProperty("typDklaratsiyi")] 
        public string TypDklaratsiyi { get; set; }                                      // Тип декларації (Щорічна / Кандидата на посаду / Перед звільненням / Після звільнення )

        [JsonProperty("typOsoby")] 
        public string TypOsoby { get; set; }                                            // Тип особи

        [JsonProperty("publichnaOsoba")] 
        public int? PublichnaOsoba { get; set; }                                        // Публічна особа 1 - особа яка вказана в декларації публічної особи / 2-  публічна особи / null - не публічна/не визначено

        [JsonProperty("posada")] 
        public string Posada { get; set; }                                              // Посада

        [JsonProperty("vkazanyyRozdil")] 
        public string VkazanyyRozdil { get; set; }                                      // Розділ декларації в якому знайдено інформацію 

        [JsonProperty("typRozdily")] 
        public string TypRozdily { get; set; }                                          // Тип операцій

        [JsonProperty("zahalnaSuma")] 
        public double? ZahalnaSuma { get; set; }                                        // Зазальна сума

        [JsonProperty("valyuta")] 
        public string Valyuta { get; set; }                                             // Валюта

        [JsonProperty("idDeklaratsiyi")] 
        public Guid? IdDeklaratsiyi { get; set; }                                       // Id декларації (можна сформувати посилання: https://public.nazk.gov.ua/declaration/8bfb39b3-7ebf-43e3-91db-c6c27836b443)
    }

    public partial class SudoviRishennya                                                // Судові рішення
    {
        [JsonProperty("pib")] 
        public string Pib { get; set; }                                                 // ПІБ особи по якій знайдені судові рішення

        [JsonProperty("courtName")] 
        public string CourtName { get; set; }                                           // Назва суду

        [JsonProperty("courtType")] 
        public string CourtType { get; set; }                                           // Форма судочинства (Кримінальне / Цівільне / ...)

        [JsonProperty("caseNumber")] 
        public string CaseNumber { get; set; }                                          // Номер документа

        [JsonProperty("caseProc")] 
        public string CaseProc { get; set; }                                            // № провадження

        [JsonProperty("registrationDate")] 
        public DateTime? RegistrationDate { get; set; }                                 // Дата рішення

        [JsonProperty("judge")] 
        public string Judge { get; set; }                                               // Головуючий суддя

        [JsonProperty("judges")] 
        public string Judges { get; set; }                                              // Суддя-доповідач

        [JsonProperty("participants")] 
        public string Participants { get; set; }                                        // Учасники

        [JsonProperty("stageDate")] 
        public DateTime? StageDate { get; set; }                                        // Дата наступної події

        [JsonProperty("stageName")] 
        public string StageName { get; set; }                                           // Стадія розгляду

        [JsonProperty("causeResult")] 
        public string CauseResult { get; set; }                                         // Результат

        [JsonProperty("type")] 
        public int? Type { get; set; }                                                  // Тип заяви

        [JsonProperty("description")] 
        public string Description { get; set; }                                         // Предмет позову

        [JsonProperty("id")] 
        public string Id { get; set; }                                                  // Id судового документа (за ким можна отримати контент)
    }

    public partial class SudoviPryznacheni                                              // Судові справи призначені до розгляду
    {
        [JsonProperty("pib")]
        public string Pib { get; set; }                                                 // ПІБ особи по якій знайдені судові рішення

        [JsonProperty("courtName")]
        public string CourtName { get; set; }                                           // Назва суду

        [JsonProperty("courtType")]
        public string CourtType { get; set; }                                           // Форма судочинства (Кримінальне / Цівільне / ...)

        [JsonProperty("caseNumber")]
        public string CaseNumber { get; set; }                                          // Номер документа

        [JsonProperty("caseProc")]
        public string CaseProc { get; set; }                                            // № провадження

        [JsonProperty("registrationDate")]
        public DateTime? RegistrationDate { get; set; }                                 // Дата рішення

        [JsonProperty("judge")]
        public string Judge { get; set; }                                               // Головуючий суддя

        [JsonProperty("judges")]
        public string Judges { get; set; }                                              // Суддя-доповідач

        [JsonProperty("participants")]
        public string Participants { get; set; }                                        // Учасники

        [JsonProperty("description")]
        public string Description { get; set; }                                         // Предмет позову

        [JsonProperty("id")]
        public string Id { get; set; }                                                  // Id судового документа (за ким можна отримати контент)
    }

    public partial class Zapyt                                                          // Початкові параметри запиту
    {
        [JsonProperty("edrpou")] 
        public string Edrpou { get; set; }                                              // Код ЄДРПОУ
    }

    public partial class ZedKrayiny                                                     // Операції ЗЕД по країнам з офшорних зон
    {
        [JsonProperty("rik")] 
        public long? Rik { get; set; }                                                  // Рік

        [JsonProperty("tseImport")] 
        public bool? TseImport { get; set; }                                            // Це імпорт? true - так(імпорт) / false - ні(єкспорт)

        [JsonProperty("cod")] 
        public long? Cod { get; set; }                                                  // Код країни

        [JsonProperty("krayina")]
        public string Krayina { get; set; }                                             // Назва країни

        [JsonProperty("zahalnaSuma")] 
        public double? ZahalnaSuma { get; set; }                                        // Загальна сума імпорту/єкспорту

        [JsonProperty("pozytsiya")] 
        public long? Pozytsiya { get; set; }                                            // Позиція в рейтингу імпортерів/єкспортерів в дану країну в данному році

        [JsonProperty("vidsotok")] 
        public double? Vidsotok { get; set; }                                           // Відсоток імпорту/єкспорту в дану країну в данному році від всіх імпортерів/єкспортерів

        [JsonProperty("ofshornaZona")] 
        public string OfshornaZona { get; set; }                                        // Назва країни офшорної зони

        [JsonProperty("ofshornaZonaTyp")] 
        public string OfshornaZonaTyp { get; set; }                                     // Перелік оффшорних списків до яких входить дана країна через ";" Приклад: FATF;APG;EAG
    }
}
