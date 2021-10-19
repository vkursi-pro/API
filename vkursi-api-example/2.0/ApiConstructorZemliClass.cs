using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.estate;
using vkursi_api_example.token;

namespace vkursi_api_example._2._0
{
    public class ApiConstructorZemliClass
    {
        /*        
        
        Метод:
            75. API 2.0 Конструктор API Zemli
            [POST] /api/2.0/ApiConstructorZemli

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/ApiConstructorZemliResponse.json
            
        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/2.0/ApiConstructorZemli' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJWa3Vyc2kiLCJlbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYTk4ZDUwMWYtOThmMi00NDk3LWIyNjMtOTY0YmY1ZTA0Y2RhIiwianRpIjoiMmU1YjNiN2QtMWY4Mi00ZmY0LTliYmEtOWU3NzBhYmZiMmFkIiwiZXhwIjoxNjM0NjUxODk5LCJpc3MiOiJodHRwczovL3ZrdXJzaS5wcm8vIiwiYXVkIjoiaHR0cHM6Ly92a3Vyc2kucHJvLyJ9.e8OGv-quDD9phFf3Kr7Ok0zktcoy29zM63IHfO5WdkM' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Cadastrs":["8000000000:63:294:0006"],"TaskId":"b19152cf-302c-4c90-b893-ad8fa129e032","MethodsList":[0]}'
        
        */

        public static ApiConstructorZemliResponseModel ApiConstructorZemli(ref string token, List<string> cadastrs, Guid taskId, HashSet<int> methodsList)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/2.0/ApiConstructorZemli");
                RestRequest request = new RestRequest(Method.POST);

                ApiConstructorZemliRequestBodyModel ACZRequestBody = new ApiConstructorZemliRequestBodyModel
                {
                    Cadastrs = cadastrs,                                    // Список кодастрових
                    TaskId = taskId,                                        // Id звіту з якого будуть отримані дані (*обовязково, з часом буде прибрано)
                    MethodsList = methodsList                               // Перелік розділів з яких будуть отримані дані
                };

                string body = JsonConvert.SerializeObject(ACZRequestBody);  // Example: {"Cadastrs":["8000000000:63:294:0006"],"TaskId":"b19152cf-302c-4c90-b893-ad8fa129e032","MethodsList":[0]}

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

            ApiConstructorZemliResponseModel ACZResponseRow = new ApiConstructorZemliResponseModel();

            ACZResponseRow = JsonConvert.DeserializeObject<ApiConstructorZemliResponseModel>(responseString);

            return ACZResponseRow;
        }
    }

    public class ApiConstructorZemliRequestBodyModel                             // Модель запиту 
    {
        public List<string> Cadastrs { get; set; }                              // Список кодастрових
        public Guid TaskId { get; set; }                                        // Id звіту з якого будуть отримані дані (*обовязково, з часом буде прибрано)
        public HashSet<int> MethodsList { get; set; }                           // Перелік розділів з яких будуть отримані дані:

        /*
        
        MethodsList:

        0   Всі
        1	Загальна кадастрова інформація
        2	Загальні відомості
        3	Нормативна оцінка
        4	Обмеження
        5	Геометрія ділянки
        6	Сусідні ділянки
        7	Накладання ділянок
        8	Природні характеристики ділянки
        9	Загальна інформація про речові права
        10	Право власності
        11	Інше речове право
        12	Обтяження та іпотека
        13	Аграрні розписки
        14	Нерухомість на ділянці
        15	Судові справи
        16	Намір про продаж 

        */
    }

    public class ApiConstructorZemliResponseModel                                                       // Відповідь на запит
    {
        public string Cadastr { get; set; }                                                             // Кадастровий номер
        public List<ConstructorZemliPlotGeneralInfo> PlotGeneralInfo { get; set; }			            // Загальна інформація
        public List<ConstructorZemliPlotOwnershipInfo> PlotOwnershipInfo { get; set; }	            	// Право власності
        public List<ConstructorZemliPlotUseRightInfo> PlotUseRightInfo { get; set; }                    // Інше речове право
        public List<ConstructorZemliPlotEncumbranceInfo> PlotEncumbranceInfo { get; set; }	          	// Обтяження
        public List<ConstructorZemliPlotSaleIntention> PlotSaleIntention { get; set; }                  // Намір про продаж
        public List<ConstructorZemliPlotMortgageInfo> PlotMortgageInfo { get; set; }                    // Іпотека
        public List<ConstructorZemliPlotLimitationInfo> PlotLimitationInfo { get; set; }                // Обмеження
        public List<ConstructorZemliPlotNGOInfo> PlotNgoInfo { get; set; }                              // Нормативна грошова оцінка
        public List<ConstructorZemliPlotRealtyInfo> PlotRealtyInfo { get; set; }                        // Нерухомість
        public List<ConstructorZemliPlotGeometry> PlotGeometry { get; set; }                            // Геометрія ділянки
        public List<ConstructorZemliPlotCaseInfo> PlotCaseInfo { get; set; }                            // Судові справи
        public List<ConstructorZemliPlotNaturalAnalytics> PlotNaturalAnalytics { get; set; }            // Природні характеристики
        public List<ConstructorZemliPlotAdjacentAreas> PlotAdjacentAreas { get; set; }                  // Сусідні ділянки
        public List<ConstructorZemliPlotOverlapAnalytics> PlotOverlapAnalytics { get; set; }            // Накладання ділянок
        public List<ConstructorZemliPlotAgriculturalReceipts> PlotAgriculturalReceipts { get; set; }    // Аграрні розписки
    }

    public class ConstructorZemliPlotGeneralInfo
    {
        public double? Area { get; set; }                                                               // Площа ділянки
        public string LandZone { get; set; }                                                            // Цільове призначення
        public string Category { get; set; }                                                            // Категорія
        public string LandType { get; set; }                                                            // Вид угіддя
        public string OwnershipForm { get; set; }                                                       // Форма власності
        public string Region { get; set; }                                                              // Область
        public string District { get; set; }                                                            // Район
        public string Hromada { get; set; }                                                             // Громада
        public string VillageCouncil { get; set; }                                                      // Сільрада
        public string Koatuu { get; set; }                                                              // КОАТУУ
        public string ONM { get; set; }                                                                 // Реєстраційний номер об'єкта нерухомості ОНМ
        public bool? Realty { get; set; }                                                               // Наявність зареєстрованої нерухомості в ДРРП
    }

    public class ConstructorZemliPlotOwnershipInfo
    {
        public string OwnershipForm { get; set; }                                                       // Форма власності
        public string OwnershipType { get; set; }                                                       // Тип власності
        public double? Part { get; set; }                                                               // Частка власності
        public string OwnerName { get; set; }                                                           // Назва власника
        public string OwnerCode { get; set; }                                                           // Код власника
        public string OwnerSubjectType { get; set; }                                                    // Тип суб'єкта власника
        public int? RegNum { get; set; }                                                                // Реєстраційний номер власності
        public DateTime? RegDate { get; set; }                                                          // Дата реєстрації права власності
        public string Register { get; set; }                                                            // Реєстратор права власності
        public string OperationReason { get; set; }                                                     // Підстава реєстрації права власності
        public string OwnershipDocName { get; set; }                                                    // Документ підстави реєстрації права власності
        public string OwnershipDocNum { get; set; }                                                     // Номер документу підстави реєстрації права власності
        public DateTime? OwnershipDocDate { get; set; }                                                 // Дата видачі документу підстави реєстрації права власності
        public string OwnershipDocPublisher { get; set; }                                               // Видавник документу підстави реєстрації права власності
    }

    public class ConstructorZemliPlotUseRightInfo
    {
        public string RightType { get; set; }                                                           // Вид іншого речового права
        public string UserName { get; set; }                                                            // Назва користувача
        public string UserCode { get; set; }                                                            // Код користувача
        public List<ConstructorZemliSubjectInfo> Landlords { get; set; }                                 // Перелік власників ділянки
        public int? RightRegNum { get; set; }                                                           // Реєстраційний номер іншого речового права
        public DateTime? RightRegDate { get; set; }                                                     // Дата реєстрації іншого речового права
        public string RightRegister { get; set; }                                                       // Реєстратор іншого речового права
        public string OperationReason { get; set; }                                                     // Підстава реєстрації іншого речового права
        public List<ConstructorZemliCauseDocument> CauseDocuments { get; set; }                          // Перелік документів іншого речового права
        public bool? IsAutomaticProlongation { get; set; }                                              // Автоматична пролонгація
        public bool? IsRightToSublease { get; set; }                                // 
        public DateTime? StartDate { get; set; }                                    // Дата початку дії іншого речового права
        public string ContractPeriod { get; set; }                                  // Термін дії іншого речового права
        public DateTime? EndDate { get; set; }                                                          // Дата завершення дії іншого речового права
        public DateTime? RightStartDate { get; set; }                                                   // Нормалізована дата початку дії іншого речового права
        public DateTime? RightEndDate { get; set; }                                                     // Нормалізована дата завершення дії іншого речового права
        public string RightEndDateStatus { get; set; }                                                  // Вид визначення дати завершення іншого речового права
        public double? PaymentPercent { get; set; }                                                     // Відсоток орендної плати
        public double? AnnualPaymentAmount { get; set; }                                                // Сума річного внеску орендної плати за ділянку
        public double? OneTimePaymentAmount { get; set; }
    }

    public class ConstructorZemliPlotEncumbranceInfo
    {
        public string EncumbranceType { get; set; }                                                     // Тип обтяження
        public int? EncumbranceRegNum { get; set; }                                                     // Реєстраційний номер обтяження
        public DateTime? EncumbranceRegDate { get; set; }                                               // Дата реєстрації обтяження 
        public string EncumbranceRegister { get; set; }                                                 // Реєстратор обтяження
        public string EncumbranceOperationReason { get; set; }                                          // Підстава реєстрації обтяження
        public string EncumbranceDocName { get; set; }                                                  // Документ обтяження
        public string EncumbranceDocNum { get; set; }                                                   // Номер документу обтяження
        public DateTime? EncumbranceDocDate { get; set; }                                               // Дата видачі документу обтяження
        public string EncumbranceDocPublisher { get; set; }                                             // Видавник документу підстави реєстрації обтяження
        public string EncumbrancerName { get; set; }                                                    // Назва обтяжувача
        public string EncumbrancerCode { get; set; }                                                    // Код обтяжувача
        public List<ConstructorZemliSubjectInfo> Owners { get; set; }                                    // Перелік власників ділянки
        public List<ConstructorZemliSubjectInfo> EncumberedPerson { get; set; }                          // Перелік осіб права яких обтяжуються
        public string BenefitedPersonName { get; set; }                                                 // Назва особи, на користь якої встановлено обтяження
        public string BenefitedPersonCode { get; set; }                                                 // Код особи, на користь якої встановлено обтяження
    }

    public class ConstructorZemliPlotSaleIntention
    {
        public string SaleIntentionType { get; set; }                                                   // Тип наміру
        public int? SaleIntentionRegNum { get; set; }                                                   // Реєстраційний номер наміру про продаж
        public DateTime? SaleIntentionRegDate { get; set; }                                             // Дата реєстрації наміру про продаж
        public string SaleIntentionRegister { get; set; }                                               // Реєстратор наміру про продаж
        public string SaleIntentionOperationReason { get; set; }                                        // Підстава реєстрації наміру про продаж
        public string SaleIntentionDocName { get; set; }                                                // Документ підстави реєстрації наміру про продаж
        public string SaleIntentionDocNum { get; set; }                                                 // Номер документу підстави реєстрації наміру
        public DateTime? SaleIntentionDocDate { get; set; }                                             // Дата видачі документу підстави реєстрації наміру
        public string SaleIntentionDocPublisher { get; set; }                                           // Видавник документу підстави реєстрації наміру
    }

    public class ConstructorZemliPlotMortgageInfo
    {
        public DateTime MortgageRegDate { get; set; }                                                   // Дата реєстрації іпотеки
        public string MortgageRegister { get; set; }                                                    // Реєстратор іпотеки
        public int? MortgageRegNum { get; set; }                                                        // Реєстраційний номер іпотеки
        public string MortgageOperationReason { get; set; }                                             // Підстава реєстрації іпотеки
        public double? MortgageAmount { get; set; }                                                     // Сума іпотечного зобов'язання
        public string CurrencyType { get; set; }                                                        // Валюта іпотечного зобов'язання
        public DateTime? MortgageExecTerm { get; set; }                                                 // Дата погашення іпотеки
        public List<ConstructorZemliSubjectInfo> Mortgagors { get; set; }                                // Перелік іпотекодавців
        public string MortgageeName { get; set; }                                                       // Назва іпотекодержателя
        public string MortgageeCode { get; set; }                                                       // Код іпотекодержателя
        public string DebtorName { get; set; }                                                          // Назва боржника
        public string DebtorCode { get; set; }                                                          // Код боржника
        public string GuarantorName { get; set; }                                                       // Назва майнового поручителя
        public string GuarantorCode { get; set; }                                                       // Код майнового поручителя
    }

    public class ConstructorZemliPlotLimitationInfo
    {
        public string LimitationType { get; set; }                                                      // Тип обмеження
        public DateTime? LimitationRegDate { get; set; }                                                // Дата реєстрації обмеження
        public string LimitationRightHolderName { get; set; }                                           // Назва особи, на користь якої встановленно обмеження
        public string LimitationRightHolderCode { get; set; }                                           // Код особи, на користь якої встановленно обмеження 
        public int? LimitationRegNum { get; set; }                                                      // Реєстраційний номер права обмеження
        public string LimitationRegister { get; set; }                                                  // Реєстратор права обмеження
    }

    public class ConstructorZemliPlotNGOInfo
    {
        public double? NGOPerPlot { get; set; }                                                         // НГО за ділянку
        public double? NGOPerHa { get; set; }                                                           // НГО за гектар
        public DateTime? PlotEvaluationDate { get; set; }                                               // Дата проведення НГО
    }

    public class ConstructorZemliPlotRealtyInfo
    {
        public string RealtyType { get; set; }                                                          // Тип об'єкту нерухомості
        public string RealtyTypeDetail { get; set; }                                                    // Доповнення до типу об'єкту нерухомості
        public double? Area { get; set; }                                                               // Площа об'єкту нерухомості
        public string TechDescription { get; set; }                                                     // Опис об'єкту нерухомості
    }

    public class ConstructorZemliPlotGeometry
    {
        public string PlotCoordinates { get; set; }                                                     // Координати ділянки
        public List<double> PlotCenterCoordinates { get; set; }                                         // Координати центру ділянки
        public double? AreaMap { get; set; }                                                            // Площа по карті
    }

    public class ConstructorZemliPlotCaseInfo
    {
        public int? AllCasesQuantity { get; set; }                                                       // Кількість судових справ, виявлених по ділянці
        public int? ActiveCasesQuantity { get; set; }                                                    // Кількість відкритих судових справ, виявлених по ділянці
        public List<string> CaseId { get; set; }                                                         // Перелік Id судових справ
        public List<ConstructorZemliActiveCase> ActiveCases { get; set; }                                 // Перелік активних судових справ
    }

    public class ConstructorZemliActiveCase
    {
        public string ActiveCaseId { get; set; }                                                         // Id документа
        public string Url { get; set; }                                                                  // Посилання
        public string JusticeKind { get; set; }                                                          // Форма судочинства
        public string Instance { get; set; }                                                             // Інстанція
        public double? ClaimAmount { get; set; }                                                         // Сума позову
        public string CurrencyType { get; set; }                                                         // Тип валюти
        public List<ConstructorZemliCaseParty> CaseParties { get; set; }                                  // Сторони справи
    }

    public class ConstructorZemliPlotNaturalAnalytics
    {
        public string SoilType { get; set; }                                                             // Тип грунту
        public string SlopeExposition { get; set; }                                                      // Експозиція схилу
        public double? SurfaceAngle { get; set; }                                                        // Кут ухилу поверхні
    }
    public class ConstructorZemliPlotAdjacentAreas
    {
        public List<string> AdjacentPlotCadastrNumber { get; set; }                                      // Перелік кадстрових номерів сусідніх ділянок
    }
    public class ConstructorZemliPlotOverlapAnalytics
    {
        public string OverlapedPlotCadastrNumber { get; set; }                                           // Кадастровий номер ділянки накладання
        public double? OverlapArea { get; set; }                                                         // Площа накладання
        public List<List<Coordinate>> Geometry { get; set; }                                             // Координати зони накладання
    }
    public class ConstructorZemliPlotAgriculturalReceipts
    {
        public List<AgroRegisters> AgroRegistersList { get; set; }                                       // Аграрні розписки
    }
    public class ConstructorZemliCaseParty
    {
        public string PartyName { get; set; }                                                            // Назва сторони
        public string PartyCode { get; set; }                                                            // Код сторони
        public string PartType { get; set; }                                                             // Тип сторони
    }
    public class ConstructorZemliSubjectInfo
    {
        public string SubjectName { get; set; }                                                          // Назва власника
        public string SubjectCode { get; set; }                                                          // Код власника
    }
    public class ConstructorZemliCauseDocument
    {
        public string UseRightDocName { get; set; }                                                      // Документ підстави реєстрації іншого речового прав
        public string UseRightDocNum { get; set; }                                                       // Номер документу підстави іншого речового права
        public DateTime? UseRightDocDate { get; set; }                                                   // Дата документу підстави іншого речового права
        public string UseRightDocPublisher { get; set; }                                                 // Видавник документу підстави іншого речового права
    }

    public class AgroRegisters
    {
        public int Id { get; set; }                                 // Id розписки
        public int TypeReceipt { get; set; }                        // Тип розписки
        public string NotarizedForms { get; set; }                  // Номери нотаріальних бланків
        public string DeliveryTermCalcDesc { get; set; }            // 
        public DateTime DeliveryTermDeadline { get; set; }          // Строк сплати коштів
        public string DeliveryTermAmountCurrency { get; set; }      // Сума (оціночна вартість)
        public string Collateral { get; set; }                      // Опис предмету застави за аграрною розпискою
        public string CollateralInsurance { get; set; }             // Умови страхування
        public double CollateralEstimation { get; set; }            // Оцінка предмету застави
        public DateTime? EndDate { get; set; }                      // Кінцева дата зобов'язання
        public DateTime CrDate { get; set; }                        // Дата реєстрації розписки
        public string Debtors { get; set; }                         // Боржники
    }
}