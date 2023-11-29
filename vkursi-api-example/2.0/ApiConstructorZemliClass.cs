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
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class ApiConstructorZemliRequestBodyModel                             // 
    {/// <summary>
     /// Список кодастрових
     /// </summary>
        public List<string> Cadastrs { get; set; }                              // 
        /// <summary>
        /// Id звіту з якого будуть отримані дані (*обовязково, з часом буде прибрано)
        /// </summary>
        public Guid TaskId { get; set; }                                        // 
        /// <summary>
        /// Перелік розділів з яких будуть отримані дані:
        ///  0   Всі
        ///  1	Загальна кадастрова інформація
        ///  2	Загальні відомості
        ///  3	Нормативна оцінка
        ///  4	Обмеження
        ///  5	Геометрія ділянки
        ///  6	Сусідні ділянки
        ///  7	Накладання ділянок
        ///  8	Природні характеристики ділянки
        ///  9	Загальна інформація про речові права
        ///  10	Право власності
        ///  11	Інше речове право
        ///  12	Обтяження та іпотека
        ///  13	Аграрні розписки
        ///  14	Нерухомість на ділянці
        ///  15	Судові справи
        ///  16	Намір про продаж
        /// </summary>
        public HashSet<int> MethodsList { get; set; }                           // 

        /*
        
        MethodsList:

       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    
         

        */
    }
    /// <summary>
    /// Відповідь на запит
    /// </summary>
    public class ApiConstructorZemliResponseModel                                                       // 
    {/// <summary>
     /// Кадастровий номер
     /// </summary>
        public string Cadastr { get; set; }                                                             // 
        /// <summary>
        /// Загальна інформація
        /// </summary>
        public List<ConstructorZemliPlotGeneralInfo> PlotGeneralInfo { get; set; }			            // 
        /// <summary>
        /// Право власності
        /// </summary>
        public List<ConstructorZemliPlotOwnershipInfo> PlotOwnershipInfo { get; set; }	            	// 
        /// <summary>
        /// Інше речове право
        /// </summary>
        public List<ConstructorZemliPlotUseRightInfo> PlotUseRightInfo { get; set; }                    // 
        /// <summary>
        /// Обтяження
        /// </summary>
        public List<ConstructorZemliPlotEncumbranceInfo> PlotEncumbranceInfo { get; set; }	          	// 
        /// <summary>
        /// Намір про продаж
        /// </summary>
        public List<ConstructorZemliPlotSaleIntention> PlotSaleIntention { get; set; }                  // 
        /// <summary>
        /// Іпотека
        /// </summary>
        public List<ConstructorZemliPlotMortgageInfo> PlotMortgageInfo { get; set; }                    // 
        /// <summary>
        /// Обмеження
        /// </summary>
        public List<ConstructorZemliPlotLimitationInfo> PlotLimitationInfo { get; set; }                // 
        /// <summary>
        /// Нормативна грошова оцінка
        /// </summary>
        public List<ConstructorZemliPlotNGOInfo> PlotNgoInfo { get; set; }                              // 
        /// <summary>
        /// Нерухомість
        /// </summary>
        public List<ConstructorZemliPlotRealtyInfo> PlotRealtyInfo { get; set; }                        // 
        /// <summary>
        /// Геометрія ділянки
        /// </summary>
        public List<ConstructorZemliPlotGeometry> PlotGeometry { get; set; }                            // 
        /// <summary>
        /// Судові справи
        /// </summary>
        public List<ConstructorZemliPlotCaseInfo> PlotCaseInfo { get; set; }                            // 
        /// <summary>
        /// Природні характеристики
        /// </summary>
        public List<ConstructorZemliPlotNaturalAnalytics> PlotNaturalAnalytics { get; set; }            // 
        /// <summary>
        /// Сусідні ділянки
        /// </summary>
        public List<ConstructorZemliPlotAdjacentAreas> PlotAdjacentAreas { get; set; }                  // 
        /// <summary>
        /// Накладання ділянок
        /// </summary>
        public List<ConstructorZemliPlotOverlapAnalytics> PlotOverlapAnalytics { get; set; }            // 
        /// <summary>
        /// Аграрні розписки
        /// </summary>
        public List<ConstructorZemliPlotAgriculturalReceipts> PlotAgriculturalReceipts { get; set; }    // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotGeneralInfo
    {/// <summary>
     /// Площа ділянки
     /// </summary>
        public double? Area { get; set; }                                                               // 
        /// <summary>
        /// Цільове призначення
        /// </summary>
        public string LandZone { get; set; }                                                            // 
        /// <summary>
        /// Категорія
        /// </summary>
        public string Category { get; set; }                                                            // 
        /// <summary>
        /// Вид угіддя
        /// </summary>
        public string LandType { get; set; }                                                            // 
        /// <summary>
        /// Форма власності
        /// </summary>
        public string OwnershipForm { get; set; }                                                       // 
        /// <summary>
        /// Область
        /// </summary>
        public string Region { get; set; }                                                              // 
        /// <summary>
        /// Район
        /// </summary>
        public string District { get; set; }                                                            // 
        /// <summary>
        /// Громада
        /// </summary>
        public string Hromada { get; set; }                                                             // 
        /// <summary>
        /// Сільрада
        /// </summary>
        public string VillageCouncil { get; set; }                                                      // 
        /// <summary>
        /// КОАТУУ
        /// </summary>
        public string Koatuu { get; set; }                                                              // 
        /// <summary>
        /// Реєстраційний номер об'єкта нерухомості ОНМ
        /// </summary>
        public string ONM { get; set; }                                                                 // 
        /// <summary>
        /// Наявність зареєстрованої нерухомості в ДРРП
        /// </summary>
        public bool? Realty { get; set; }                                                               // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotOwnershipInfo
    {/// <summary>
     /// Форма власності
     /// </summary>
        public string OwnershipForm { get; set; }                                                       // 
        /// <summary>
        /// Тип власності
        /// </summary>
        public string OwnershipType { get; set; }                                                       // 
        /// <summary>
        /// Частка власності
        /// </summary>
        public double? Part { get; set; }                                                               // 
        /// <summary>
        /// Назва власника
        /// </summary>
        public string OwnerName { get; set; }                                                           // 
        /// <summary>
        /// Код власника
        /// </summary>
        public string OwnerCode { get; set; }                                                           // 
        /// <summary>
        /// Тип суб'єкта власника
        /// </summary>
        public string OwnerSubjectType { get; set; }                                                    // 
        /// <summary>
        /// Реєстраційний номер власності
        /// </summary>
        public int? RegNum { get; set; }                                                                // 
        /// <summary>
        /// Дата реєстрації права власності
        /// </summary>
        public DateTime? RegDate { get; set; }                                                          // 
        /// <summary>
        /// Реєстратор права власності
        /// </summary>
        public string Register { get; set; }                                                            // 
        /// <summary>
        /// Підстава реєстрації права власності
        /// </summary>
        public string OperationReason { get; set; }                                                     // 
        /// <summary>
        /// Документ підстави реєстрації права власності
        /// </summary>
        public string OwnershipDocName { get; set; }                                                    // 
        /// <summary>
        /// Номер документу підстави реєстрації права власності
        /// </summary>
        public string OwnershipDocNum { get; set; }                                                     // 
        /// <summary>
        /// Дата видачі документу підстави реєстрації права власності
        /// </summary>
        public DateTime? OwnershipDocDate { get; set; }                                                 // 
        /// <summary>
        /// Видавник документу підстави реєстрації права власності
        /// </summary>
        public string OwnershipDocPublisher { get; set; }                                               // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotUseRightInfo
    {/// <summary>
     /// Вид іншого речового права
     /// </summary>
        public string RightType { get; set; }                                                           // 
        /// <summary>
        /// Назва користувача
        /// </summary>
        public string UserName { get; set; }                                                            // 
        /// <summary>
        /// Код користувача
        /// </summary>
        public string UserCode { get; set; }                                                            // 
        /// <summary>
        /// Перелік власників ділянки
        /// </summary>
        public List<ConstructorZemliSubjectInfo> Landlords { get; set; }                                 // 
        /// <summary>
        /// Реєстраційний номер іншого речового права
        /// </summary>
        public int? RightRegNum { get; set; }                                                           // 
        /// <summary>
        /// Дата реєстрації іншого речового права
        /// </summary>
        public DateTime? RightRegDate { get; set; }                                                     // 
        /// <summary>
        /// Реєстратор іншого речового права
        /// </summary>
        public string RightRegister { get; set; }                                                       // 
        /// <summary>
        ///Підстава реєстрації іншого речового права 
        /// </summary>
        public string OperationReason { get; set; }                                                     // 
        /// <summary>
        /// Перелік документів іншого речового права
        /// </summary>
        public List<ConstructorZemliCauseDocument> CauseDocuments { get; set; }                          // 
        /// <summary>
        /// Автоматична пролонгація
        /// </summary>
        public bool? IsAutomaticProlongation { get; set; }                                              // 
        /// <summary>
        /// ???
        /// </summary>
        public bool? IsRightToSublease { get; set; }                                // 
        /// <summary>
        /// Дата початку дії іншого речового права
        /// </summary>
        public DateTime? StartDate { get; set; }                                    // 
        /// <summary>
        /// Термін дії іншого речового права
        /// </summary>
        public string ContractPeriod { get; set; }                                  // 
        /// <summary>
        /// Дата завершення дії іншого речового права
        /// </summary>
        public DateTime? EndDate { get; set; }                                                          // 
        /// <summary>
        /// Нормалізована дата початку дії іншого речового права
        /// </summary>
        public DateTime? RightStartDate { get; set; }                                                   // 
        /// <summary>
        /// Нормалізована дата завершення дії іншого речового права
        /// </summary>
        public DateTime? RightEndDate { get; set; }                                                     // 
        /// <summary>
        /// Вид визначення дати завершення іншого речового права
        /// </summary>
        public string RightEndDateStatus { get; set; }                                                  // 
        /// <summary>
        /// Відсоток орендної плати
        /// </summary>
        public double? PaymentPercent { get; set; }                                                     // 
        /// <summary>
        /// Сума річного внеску орендної плати за ділянку
        /// </summary>
        public double? AnnualPaymentAmount { get; set; }                                                // 
        /// <summary>
        /// ???
        /// </summary>
        public double? OneTimePaymentAmount { get; set; }
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotEncumbranceInfo
    {/// <summary>
     /// Тип обтяження
     /// </summary>
        public string EncumbranceType { get; set; }                                                     // 
        /// <summary>
        /// Реєстраційний номер обтяження
        /// </summary>
        public int? EncumbranceRegNum { get; set; }                                                     // 
        /// <summary>
        /// Дата реєстрації обтяження 
        /// </summary>
        public DateTime? EncumbranceRegDate { get; set; }                                               // 
        /// <summary>
        /// Реєстратор обтяження
        /// </summary>
        public string EncumbranceRegister { get; set; }                                                 // 
        /// <summary>
        /// Підстава реєстрації обтяження
        /// </summary>
        public string EncumbranceOperationReason { get; set; }                                          // 
        /// <summary>
        /// Документ обтяження
        /// </summary>
        public string EncumbranceDocName { get; set; }                                                  // 
        /// <summary>
        /// Номер документу обтяження
        /// </summary>
        public string EncumbranceDocNum { get; set; }                                                   // 
        /// <summary>
        /// Дата видачі документу обтяження
        /// </summary>
        public DateTime? EncumbranceDocDate { get; set; }                                               // 
        /// <summary>
        /// Видавник документу підстави реєстрації обтяження
        /// </summary>
        public string EncumbranceDocPublisher { get; set; }                                             // 
        /// <summary>
        /// Назва обтяжувача
        /// </summary>
        public string EncumbrancerName { get; set; }                                                    // 
        /// <summary>
        /// Код обтяжувача
        /// </summary>
        public string EncumbrancerCode { get; set; }                                                    // 
        /// <summary>
        /// Перелік власників ділянки
        /// </summary>
        public List<ConstructorZemliSubjectInfo> Owners { get; set; }                                    // 
        /// <summary>
        /// Перелік осіб права яких обтяжуються
        /// </summary>
        public List<ConstructorZemliSubjectInfo> EncumberedPerson { get; set; }                          // 
        /// <summary>
        /// Назва особи, на користь якої встановлено обтяження
        /// </summary>
        public string BenefitedPersonName { get; set; }                                                 // 
        /// <summary>
        /// Код особи, на користь якої встановлено обтяження
        /// </summary>
        public string BenefitedPersonCode { get; set; }                                                 // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotSaleIntention
    {/// <summary>
     /// Тип наміру
     /// </summary>
        public string SaleIntentionType { get; set; }                                                   // 
        /// <summary>
        /// Реєстраційний номер наміру про продаж
        /// </summary>
        public int? SaleIntentionRegNum { get; set; }                                                   // 
        /// <summary>
        /// Дата реєстрації наміру про продаж
        /// </summary>
        public DateTime? SaleIntentionRegDate { get; set; }                                             // 
        /// <summary>
        /// Реєстратор наміру про продаж
        /// </summary>
        public string SaleIntentionRegister { get; set; }                                               // 
        /// <summary>
        /// Підстава реєстрації наміру про продаж
        /// </summary>
        public string SaleIntentionOperationReason { get; set; }                                        // 
        /// <summary>
        /// Документ підстави реєстрації наміру про продаж
        /// </summary>
        public string SaleIntentionDocName { get; set; }                                                // 
        /// <summary>
        /// Номер документу підстави реєстрації наміру
        /// </summary>
        public string SaleIntentionDocNum { get; set; }                                                 // 
        /// <summary>
        /// Дата видачі документу підстави реєстрації наміру
        /// </summary>
        public DateTime? SaleIntentionDocDate { get; set; }                                             // 
        /// <summary>
        /// Видавник документу підстави реєстрації наміру
        /// </summary>
        public string SaleIntentionDocPublisher { get; set; }                                           // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotMortgageInfo
    {/// <summary>
     /// Дата реєстрації іпотеки
     /// </summary>
        public DateTime MortgageRegDate { get; set; }                                                   // 
        /// <summary>
        /// Реєстратор іпотеки
        /// </summary>
        public string MortgageRegister { get; set; }                                                    // 
        /// <summary>
        /// Реєстраційний номер іпотеки
        /// </summary>
        public int? MortgageRegNum { get; set; }                                                        // 
        /// <summary>
        /// Підстава реєстрації іпотеки
        /// </summary>
        public string MortgageOperationReason { get; set; }                                             // 
        /// <summary>
        /// Сума іпотечного зобов'язання
        /// </summary>
        public double? MortgageAmount { get; set; }                                                     // 
        /// <summary>
        /// Валюта іпотечного зобов'язання
        /// </summary>
        public string CurrencyType { get; set; }                                                        // 
        /// <summary>
        /// Дата погашення іпотеки
        /// </summary>
        public DateTime? MortgageExecTerm { get; set; }                                                 // 
        /// <summary>
        /// Перелік іпотекодавців
        /// </summary>
        public List<ConstructorZemliSubjectInfo> Mortgagors { get; set; }                                // 
        /// <summary>
        /// Назва іпотекодержателя
        /// </summary>
        public string MortgageeName { get; set; }                                                       // 
        /// <summary>
        /// Код іпотекодержателя
        /// </summary>
        public string MortgageeCode { get; set; }                                                       // 
        /// <summary>
        /// Назва боржника
        /// </summary>
        public string DebtorName { get; set; }                                                          // 
        /// <summary>
        /// Код боржника
        /// </summary>
        public string DebtorCode { get; set; }                                                          // 
        /// <summary>
        /// Назва майнового поручителя
        /// </summary>
        public string GuarantorName { get; set; }                                                       // 
        /// <summary>
        /// Код майнового поручителя
        /// </summary>
        public string GuarantorCode { get; set; }                                                       // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotLimitationInfo
    {/// <summary>
     /// Тип обмеження
     /// </summary>
        public string LimitationType { get; set; }                                                      // 
        /// <summary>
        /// Дата реєстрації обмеження
        /// </summary>
        public DateTime? LimitationRegDate { get; set; }                                                // 
        /// <summary>
        /// Назва особи, на користь якої встановленно обмеження
        /// </summary>
        public string LimitationRightHolderName { get; set; }                                           // 
        /// <summary>
        /// Код особи, на користь якої встановленно обмеження 
        /// </summary>
        public string LimitationRightHolderCode { get; set; }                                           // 
        /// <summary>
        /// Реєстраційний номер права обмеження
        /// </summary>
        public int? LimitationRegNum { get; set; }                                                      // 
        /// <summary>
        /// Реєстратор права обмеження
        /// </summary>
        public string LimitationRegister { get; set; }                                                  // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotNGOInfo
    {/// <summary>
     /// НГО за ділянку
     /// </summary>
        public double? NGOPerPlot { get; set; }                                                         // 
        /// <summary>
        /// НГО за гектар
        /// </summary>
        public double? NGOPerHa { get; set; }                                                           // 
        /// <summary>
        /// Дата проведення НГО
        /// </summary>
        public DateTime? PlotEvaluationDate { get; set; }                                               // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotRealtyInfo
    {/// <summary>
     /// Тип об'єкту нерухомості
     /// </summary>
        public string RealtyType { get; set; }                                                          // 
        /// <summary>
        /// Доповнення до типу об'єкту нерухомості
        /// </summary>
        public string RealtyTypeDetail { get; set; }                                                    // 
        /// <summary>
        /// Площа об'єкту нерухомості
        /// </summary>
        public double? Area { get; set; }                                                               // 
        /// <summary>
        /// Опис об'єкту нерухомості
        /// </summary>
        public string TechDescription { get; set; }                                                     // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotGeometry
    {/// <summary>
     /// Координати ділянки
     /// </summary>
        public string PlotCoordinates { get; set; }                                                     // 
        /// <summary>
        /// Координати центру ділянки
        /// </summary>
        public List<double> PlotCenterCoordinates { get; set; }                                         // 
        /// <summary>
        /// Площа по карті
        /// </summary>
        public double? AreaMap { get; set; }                                                            // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotCaseInfo
    {/// <summary>
     /// Кількість судових справ, виявлених по ділянці
     /// </summary>
        public int? AllCasesQuantity { get; set; }                                                       // 
        /// <summary>
        /// Кількість відкритих судових справ, виявлених по ділянці
        /// </summary>
        public int? ActiveCasesQuantity { get; set; }                                                    // 
        /// <summary>
        /// Перелік Id судових справ
        /// </summary>
        public List<string> CaseId { get; set; }                                                         // 
        /// <summary>
        /// Перелік активних судових справ
        /// </summary>
        public List<ConstructorZemliActiveCase> ActiveCases { get; set; }                                 // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliActiveCase
    {/// <summary>
     /// Id документа
     /// </summary>
        public string ActiveCaseId { get; set; }                                                         // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Url { get; set; }                                                                  // 
        /// <summary>
        /// Форма судочинства
        /// </summary>
        public string JusticeKind { get; set; }                                                          // 
        /// <summary>
        /// Інстанція
        /// </summary>
        public string Instance { get; set; }                                                             // 
        /// <summary>
        /// Сума позову
        /// </summary>
        public double? ClaimAmount { get; set; }                                                         // 
        /// <summary>
        /// Тип валюти
        /// </summary>
        public string CurrencyType { get; set; }                                                         // 
        /// <summary>
        /// Сторони справи
        /// </summary>
        public List<ConstructorZemliCaseParty> CaseParties { get; set; }                                  // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotNaturalAnalytics
    {/// <summary>
     /// Тип грунту
     /// </summary>
        public string SoilType { get; set; }                                                             // 
        /// <summary>
        /// Експозиція схилу
        /// </summary>
        public string SlopeExposition { get; set; }                                                      // 
        /// <summary>
        /// Кут ухилу поверхні
        /// </summary>
        public double? SurfaceAngle { get; set; }                                                        // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotAdjacentAreas
    {/// <summary>
     /// Перелік кадстрових номерів сусідніх ділянок
     /// </summary>
        public List<string> AdjacentPlotCadastrNumber { get; set; }                                      // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotOverlapAnalytics
    {/// <summary>
     /// Кадастровий номер ділянки накладання
     /// </summary>
        public string OverlapedPlotCadastrNumber { get; set; }                                           // 
        /// <summary>
        /// Площа накладання
        /// </summary>
        public double? OverlapArea { get; set; }                                                         // 
        /// <summary>
        /// Координати зони накладання
        /// </summary>
        public List<List<Coordinate>> Geometry { get; set; }                                             // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliPlotAgriculturalReceipts
    {/// <summary>
     /// Аграрні розписки
     /// </summary>
        public List<AgroRegisters> AgroRegistersList { get; set; }                                       // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliCaseParty
    {/// <summary>
     /// Назва сторони
     /// </summary>
        public string PartyName { get; set; }                                                            // 
        /// <summary>
        /// Код сторони
        /// </summary>
        public string PartyCode { get; set; }                                                            // 
        /// <summary>
        /// Тип сторони
        /// </summary>
        public string PartType { get; set; }                                                             // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliSubjectInfo
    {/// <summary>
     /// Назва власника
     /// </summary>
        public string SubjectName { get; set; }                                                          // 
        /// <summary>
        /// Код власника
        /// </summary>
        public string SubjectCode { get; set; }                                                          // 
    }/// <summary>
    /// ???
    /// </summary>
    public class ConstructorZemliCauseDocument
    {/// <summary>
     /// Документ підстави реєстрації іншого речового прав
     /// </summary>
        public string UseRightDocName { get; set; }                                                      // 
        /// <summary>
        /// Номер документу підстави іншого речового права
        /// </summary>
        public string UseRightDocNum { get; set; }                                                       // 
        /// <summary>
        /// Дата документу підстави іншого речового права
        /// </summary>
        public DateTime? UseRightDocDate { get; set; }                                                   // 
        /// <summary>
        /// Видавник документу підстави іншого речового права
        /// </summary>
        public string UseRightDocPublisher { get; set; }                                                 // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class AgroRegisters
    {/// <summary>
     /// Id розписки
     /// </summary>
        public int Id { get; set; }                                 // 
        /// <summary>
        /// Тип розписки
        /// </summary>
        public int TypeReceipt { get; set; }                        // 
        /// <summary>
        /// Номери нотаріальних бланків
        /// </summary>
        public string NotarizedForms { get; set; }                  // 
        /// <summary>
        /// ???
        /// </summary>
        public string DeliveryTermCalcDesc { get; set; }            // 
        /// <summary>
        /// Строк сплати коштів
        /// </summary>
        public DateTime DeliveryTermDeadline { get; set; }          // 
        /// <summary>
        /// Сума (оціночна вартість)
        /// </summary>
        public string DeliveryTermAmountCurrency { get; set; }      // 
        /// <summary>
        /// Опис предмету застави за аграрною розпискою
        /// </summary>
        public string Collateral { get; set; }                      // 
        /// <summary>
        /// Умови страхування
        /// </summary>
        public string CollateralInsurance { get; set; }             // 
        /// <summary>
        /// Оцінка предмету застави
        /// </summary>
        public double CollateralEstimation { get; set; }            // 
        /// <summary>
        /// Кінцева дата зобов'язання
        /// </summary>
        public DateTime? EndDate { get; set; }                      // 
        /// <summary>
        /// Дата реєстрації розписки
        /// </summary>
        public DateTime CrDate { get; set; }                        // 
        /// <summary>
        /// Боржники
        /// </summary>
        public string Debtors { get; set; }                         // 
    }
}