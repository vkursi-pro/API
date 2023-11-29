using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetAnalyticClass
    {

        /*

        9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
        [POST] /api/1.0/organizations/getanalytic

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getanalytic' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"code":"00131305"}'

        */

        public static GetAnalyticResponseModel GetAnalytic(string code, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetAnalyticRequestBodyModel GARequestBody = new GetAnalyticRequestBodyModel
                {
                    code = code                                             // Код ЄДРПОУ / ІПН
                };

                string body = JsonConvert.SerializeObject(GARequestBody);   // Example body: {"code":"00131305"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getanalytic");
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetAnalyticResponseModel organizationAnalytic = new GetAnalyticResponseModel();

            organizationAnalytic = JsonConvert.DeserializeObject<GetAnalyticResponseModel>(responseString);

            //string jsonStr = JsonConvert.SerializeObject(organizationAnalytic, Formatting.Indented);

            //var vidkrytoCreditor = organizationAnalytic.enforcements.Where(w => w.vidkrytoCreditor != null).Sum(w => w.vidkrytoCreditor);
            //var zavershenoCreditor = organizationAnalytic.enforcements.Where(w => w.zavershenoCreditor != null).Sum(w => w.zavershenoCreditor);
            //var inshyyStanCreditor = organizationAnalytic.enforcements.Where(w => w.inshyyStanCreditor != null).Sum(w => w.inshyyStanCreditor);
            //var vidkrytoDebitor = organizationAnalytic.enforcements.Where(w => w.vidkrytoDebitor != null).Sum(w => w.vidkrytoDebitor);
            //var zavershenoDebitor = organizationAnalytic.enforcements.Where(w => w.zavershenoDebitor != null).Sum(w => w.zavershenoDebitor);
            //var inshyyStanDebitor = organizationAnalytic.enforcements.Where(w => w.inshyyStanDebitor != null).Sum(w => w.inshyyStanDebitor);

            return organizationAnalytic;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"code\":\"00131305\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getanalytic", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"code\":\"00131305\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getanalytic")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetAnalyticRequestBodyModel                                    // 
    {/// <summary>
     /// Код ЄДРПОУ / ІПН (maxLength:10)
     /// </summary>
        public string code { get; set; }                                        // 
    }
    /// <summary>
    /// Модель відповіді GetAnalytic
    /// </summary>
    public class GetAnalyticResponseModel                                       // 
    {/// <summary>
     /// Id організації (maxLength:64)
     /// </summary>
        public string orgId { get; set; }                                       // 
        /// <summary>
        /// Назва ЮО / ФОП (maxLength:512)
        /// </summary>
        public string name { get; set; }                                        // 
        /// <summary>
        /// Тип ЮО - true/ ФОП - false
        /// </summary>
        public bool legalEntity { get; set; }                                   // 
        /// <summary>
        /// код ЄДРПОУ | ІПН (maxLength:10)
        /// </summary>
        public string edrpou { get; set; }                                      // 
        /// <summary>
        /// Статус код ( 1 - зареєстровано, 2 - припинено,... відп.до довідника № 1. Стан суб’єкта)
        /// </summary>
        public int? stateInt { get; set; }                                      //  
        /// <summary>
        /// Статуc назва
        /// </summary>
        public State state { get; set; }                                        // 
        /// <summary>
        /// Сума статутного капіталу
        /// </summary>
        public double totalAmount { get; set; }                                 // 
        /// <summary>
        /// ПДВ 
        /// </summary>
        public VatPayers vatPayers { get; set; }                                // 
        /// <summary>
        /// Аналіз тендерів в розрізі періоду (місяць)
        /// </summary>
        public List<Tenders> tenders { get; set; }                              // 
        /// <summary>
        /// Статистика по об`єктів інтелектуальної власності в розрізі місяця (патентам / ТМ)
        /// </summary>
        public List<Patents> patents { get; set; }                              // // 
        /// <summary>
        /// Аналітика по деклараціям  (період рік)
        /// </summary>
        public List<Declarations> declarations { get; set; }                    // 
        /// <summary>
        /// Дата створення / закрыття компанії / ФОП
        /// </summary>
        public Date date { get; set; }                                          // 
        /// <summary>
        /// Загальна аналітика по судовим рішенням
        /// </summary>
        public TotalCourts totalCourts { get; set; }                            // 
        /// <summary>
        /// Аналітика по судовим в розрізі місяця
        /// </summary>
        public List<OrganizationAnalytiCourtsAnalytics> courtsAnalytics { get; set; }           // 
        /// <summary>
        /// Аналітика по справам призначенним до розгляду в розрізі місяця
        /// </summary>
        public List<OrganizationAnalytiCourtsAnalytics> courtsAssignedAnalytics { get; set; }   // 
        /// <summary>
        /// Виконавчі провадження
        /// </summary>
        public List<OrganizationAnalyticEnforcements> enforcements { get; set; }                // 
        /// <summary>
        /// Виконавчі провадження по категоріям сторін
        /// </summary>
        public OrganizationAnalyticEnforcementsStatistic enforcementsStatistic { get; set; }    // 
        /// <summary>
        /// Публікації ВГСУ про банкрутство
        /// </summary>
        public List<OrganizationAnalyticBankruptcy> bankruptcy { get; set; }    // 
        /// <summary>
        /// Аналітика по Edata період (місяць)
        /// </summary>
        public List<OrganizationAnalyticEdata> edata { get; set; }              // 
        /// <summary>
        /// Аналітика по перевіркам
        /// </summary>
        public List<OrgChecks> orgChecks { get; set; }                          // 
        /// <summary>
        /// Динаміка податкового боргу (період місяць)
        /// </summary>
        public List<DebtorsBorg> debtorsBorg { get; set; }                      // 
        /// <summary>
        /// Історія реєстрацийних змін
        /// </summary>
        public ChangeHistory changeHistory { get; set; }                        // 
        /// <summary>
        /// Аналіз ліцензій (в розрізі органу ліцензування)
        /// </summary>
        public List<OrganizationLicensesElastic> organizationLicenses { get; set; }             // 
        /// <summary>
        /// Дані експрес перевірки
        /// </summary>
        public OrganizationAnalyticExpressScore expressScore { get; set; }      //   
        /// <summary>
        /// Відомості про наявні санкції
        /// </summary>
        public List<OrganizationAnalyticSanctions> sanctions { get; set; }      // 
        /// <summary>
        /// Основний квед Id
        /// </summary>
        public List<int> kvedsInt { get; set; }                                 // 
        /// <summary>
        /// Форма власності
        /// </summary>
        public List<Ownership> ownership { get; set; }                          // 
        /// <summary>
        /// Аналітика по засновникам в розрізі країни
        /// </summary>
        public List<Founders> founders { get; set; }                            // 
        /// <summary>
        /// Фінансова аналітика (період рік)
        /// </summary>
        public List<OrganizationAnalyticFinancialBKI> financial { get; set; }   // 
        /// <summary>
        /// Аналіз участі в торгах 
        /// </summary>
        public OrganizationAnalyticTenderBidStatistics tenderBidStatistics { get; set; }                        // 
        /// <summary>
        /// Аналіз організованніх тендерів
        /// </summary>
        public OrganizationAnalyticTenderOrganizerStatistics tenderOrganizerStatistics { get; set; }            // 
        /// <summary>
        /// Аналіз участі в тендерах в розрізі CPV (період рік)
        /// </summary>
        public List<OrganizationAnalyticTenderBidStatisticsTenderCpvStats> tenderCpvStats { get; set; }         // 
        /// <summary>
        /// Аналіз земельних ділянок
        /// </summary>
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand realEstateRightsLand { get; set; }   // 
        /// <summary>
        /// Аналіз об'єктів нерухомого майна (крім земельних ділянок)
        /// </summary>
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj realEstateRightsObj { get; set; }     // 
        /// <summary>
        /// Аналіз ЗЕД (період рік)
        /// </summary>
        public List<OrganizationAnalyticOrganizationFEAModel> financialFEA { get; set; }                        // 
        /// <summary>
        /// Аналіз фінансових ризиків (період рік) (відп.до довідника № 12) 
        /// </summary>
        public List<OrganizationAnalyticFinancialRisks> financialRisks { get; set; }                            // 
        /// <summary>
        /// Дані про кількість співробітників (період рік)
        /// </summary>
        public List<OrganizationAnalyticEmployeesModel> employees { get; set; }                                 // 
        /// <summary>
        /// Системна інформація (не використовується)
        /// </summary>
        public List<string> vehiclesInfo { get; set; }                                                          // 
        /// <summary>
        /// Дані про код КОАТУУ
        /// </summary>
        public OrganizationAnalyticKoatuInfo koatuInfo { get; set; }                                            // 
        /// <summary>
        /// Системна інформація (не використовується)
        /// </summary>
        public List<LtStAnalyticsModel> equityLtStAnalytics { get; set; }                                       // 
        /// <summary>
        /// Відомості про єдиний податок
        /// </summary>
        public SinglePaxPayer singlePayers { get; set; }                                                        // 
        /// <summary>
        /// Системне поле
        /// </summary>
        public int? shareholdersCount { get; set; }                                                             // 
        /// <summary>
        /// Системне поле
        /// </summary>
        public int? branchCount { get; set; }                                                                   // 
        /// <summary>
        /// Дані по ЗЄД в розрізі країни та року
        /// </summary>
        public List<FeaCountryGroup> feaCountry { get; set; }                                                   // 
        /// <summary>
        /// Дані по ЗЄД в розрізі групи товарів та року
        /// </summary>
        public List<FeaCountryGroup> feaGroup { get; set; }                                                     // 
        /// <summary>
        /// Основні показники балансу
        /// </summary>
        public List<ImportMainBalanceIndicators> mainBalanceIndicators { get; set; }                            // 
    }

    /// <summary>
    /// Дані про кількість співробітників (період рік)
    /// </summary>
    public class OrganizationAnalyticEmployeesModel                             // 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int? year { get; set; }                                          // 
        /// <summary>
        /// К-ть співробітників
        /// </summary>
        public int? count { get; set; }                                         // 
        /// <summary>
        /// Різниця в к-ті співробітників з попереднім періодом
        /// </summary>
        public int? differentPrevCount { get; set; }                            // 
    }

    /// <summary>
    /// Аналіз фінансових ризиків (період рік) (відп.до довідника № 12) 
    /// </summary>
    public class OrganizationAnalyticFinancialRisks                             // 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int? year { get; set; }                                          // 
        /// <summary>
        /// Група за квед
        /// </summary>
        public int? kvedGroupNumb { get; set; }                                 // 
        /// <summary>
        /// Класс боржника
        /// </summary>
        public int? debtClass { get; set; }                                     // 
        /// <summary>
        /// Кількість показників
        /// </summary>
        public int? metricsCount { get; set; }                                  // 
        /// <summary>
        /// Категорія
        /// </summary>
        public int? risksCategoryInt { get; set; }                              // 
    }
    /// <summary>
    /// Аналіз ЗЕД (період рік)
    /// </summary>
    public class OrganizationAnalyticOrganizationFEAModel 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int? year { get; set; }
        /// <summary>
        /// Імпорт (true) / Експорт (false)
        /// </summary>
        public bool? isImport { get; set; }
        /// <summary>
        /// Кількість операцій
        /// </summary>
        public int? operationsCount { get; set; }
        /// <summary>
        /// Сума операцій
        /// </summary>
        public double? operationsSum { get; set; }
        /// <summary>
        /// Відсоток основної групи
        /// </summary>
        public double? groupPersent { get; set; }
        /// <summary>
        /// Загальна кількість груп імпорту / експорту
        /// </summary>
        public int? groupCount { get; set; }
        /// <summary>
        /// Код найбільшої групи імпорту / експорту
        /// </summary>
        public int? largestGroupCode { get; set; }
        /// <summary>
        /// Код найбільшої країни імпорту / експорту
        /// </summary>
        public int? largestCountryCode { get; set; }
        /// <summary>
        /// Загальна кількість країн імпорту / експорту
        /// </summary>
        public int? countryCount { get; set; }
        /// <summary>
        /// Відсоток найбільної країни
        /// </summary>
        public decimal? countryLargestPersent { get; set; }
    }

    /// <summary>
    /// Виконавчі провадження по категоріям сторін
    /// </summary>
    public class OrganizationAnalyticEnforcementsStatistic 
    {/// <summary>
     /// Держава стягувач
     /// </summary>
        public long? govermentCountDebtor { get; set; } 
        /// <summary>
        /// ФО стягувач
        /// </summary>
        public long? personCountDebtor { get; set; }
        /// <summary>
        /// Організація стягувач
        /// </summary>
        public long? orgCountDebtor { get; set; }
        /// <summary>
        /// Інша категорія стягувачів
        /// </summary>
        public long? anotherCountDebtor { get; set; } 
        /// <summary>
        /// Держава боржник
        /// </summary>
        public long? govermentCountCreditor { get; set; } 
        /// <summary>
        /// ФО боржник
        /// </summary>
        public long? personCountCreditor { get; set; }
        /// <summary>
        /// Організація боржник
        /// </summary>
        public long? orgCountCreditor { get; set; }
        /// <summary>
        /// Інша категорія боржник
        /// </summary>
        public long? anotherCountCreditor { get; set; } 
    }

    /// <summary>
    /// ???
    /// </summary>
    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj
    {/// <summary>
     /// Загальна кількість
     /// </summary>
        public int? full { get; set; }              
        /// <summary>
        /// Власник 
        /// </summary>
        public int? vlasnyk { get; set; }           
        /// <summary>
        /// Правонабувач
        /// </summary>
        public int? pravonabuvach { get; set; }     
        /// <summary>
        /// Правокористувач
        /// </summary>
        public int? pravokorystuvach { get; set; }  
        /// <summary>
        /// Землевласник
        /// </summary>
        public int? zemlevlasnyk { get; set; }      
        /// <summary>
        /// Землеволоділець
        /// </summary>
        public int? zemlevolodilets { get; set; }   
        /// <summary>
        /// Інший
        /// </summary>
        public int? inshyy { get; set; }            
        /// <summary>
        /// Наймач
        /// </summary>
        public int? naymach { get; set; }           
        /// <summary>
        /// Орендар
        /// </summary>
        public int? orendar { get; set; }           
        /// <summary>
        /// Наймодавець
        /// </summary>
        public int? naymodavets { get; set; }       
        /// <summary>
        /// Орендодавець
        /// </summary>
        public int? orendodavets { get; set; }      
        /// <summary>
        /// Управитель
        /// </summary>
        public int? upravytel { get; set; }         
        /// <summary>
        /// Вигодонабувач
        /// </summary>
        public int? vyhodonabuvach { get; set; }    
        /// <summary>
        /// Установник
        /// </summary>
        public int? ustanovnyk { get; set; }        
        /// <summary>
        /// Іпотекодержатель
        /// </summary>
        public int? ipotekoderzhatel { get; set; }  
        /// <summary>
        /// Майновий поручитель
        /// </summary>
        public int? maynovyyPoruchytel { get; set; }
        /// <summary>
        /// Іпотекодавець
        /// </summary>
        public int? ipotekodavets { get; set; }     
        /// <summary>
        /// Боржник
        /// </summary>
        public int? borzhnyk { get; set; }          
        /// <summary>
        /// Обтяжувач
        /// </summary>
        public int? obtyazhuvach { get; set; }      
        /// <summary>
        /// Особа, майно/права якої обтяжуються
        /// </summary>
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; } 
        /// <summary>
        /// Особа, в інтересах якої встановлено обтяження
        /// </summary>
        public int? osobaVinteresakhYakoyi { get; set; } 
        /// <summary>
        /// Помилка при оновленні
        /// </summary>
        public bool? registerError { get; set; }    
        /// <summary>
        /// Об'єкти нерухомого майна в розрізі регіону
        /// </summary>
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; }  
    }
    /// <summary>
    /// Аналіз земельних ділянок
    /// </summary>
    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand  
    {/// <summary>
     /// Загальна кількість
     /// </summary>
        public int? full { get; set; }              
        /// <summary>
        /// Власник 
        /// </summary>
        public int? vlasnyk { get; set; }            
        /// <summary>
        /// Правонабувач
        /// </summary>
        public int? pravonabuvach { get; set; }     
        /// <summary>
        /// Правокористувач
        /// </summary>
        public int? pravokorystuvach { get; set; }   
        /// <summary>
        /// Землевласник
        /// </summary>
        public int? zemlevlasnyk { get; set; }      
        /// <summary>
        /// Землеволоділець
        /// </summary>
        public int? zemlevolodilets { get; set; }    
        /// <summary>
        /// Інший
        /// </summary>
        public int? inshyy { get; set; }            
        /// <summary>
        /// Наймач
        /// </summary>
        public int? naymach { get; set; }           
        /// <summary>
        /// Орендар
        /// </summary>
        public int? orendar { get; set; }           
        /// <summary>
        /// Наймодавець
        /// </summary>
        public int? naymodavets { get; set; }       
        /// <summary>
        /// Орендодавець
        /// </summary>
        public int? orendodavets { get; set; }      
        /// <summary>
        /// Управитель
        /// </summary>
        public int? upravytel { get; set; }         
        /// <summary>
        /// Вигодонабувач
        /// </summary>
        public int? vyhodonabuvach { get; set; }    
        /// <summary>
        /// Установник
        /// </summary>
        public int? ustanovnyk { get; set; }        
        /// <summary>
        /// Іпотекодержатель
        /// </summary>
        public int? ipotekoderzhatel { get; set; }  
        /// <summary>
        /// Майновий поручитель
        /// </summary>
        public int? maynovyyPoruchytel { get; set; }
        /// <summary>
        /// Іпотекодавець
        /// </summary>
        public int? ipotekodavets { get; set; }     
        /// <summary>
        /// Боржник
        /// </summary>
        public int? borzhnyk { get; set; }          
        /// <summary>
        /// Обтяжувач
        /// </summary>
        public int? obtyazhuvach { get; set; }      
        /// <summary>
        /// Особа, майно/права якої обтяжуються
        /// </summary>
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; } 
        /// <summary>
        /// Особа, в інтересах якої встановлено обтяження
        /// </summary>
        public int? osobaVinteresakhYakoyi { get; set; } 
        /// <summary>
        /// Загальна площа
        /// </summary>
        public double? ploshcha { get; set; } 
        /// <summary>
        /// Площа в обробітку (офіційно)
        /// </summary>
        public double? ploshchaObroblena { get; set; } 
        /// <summary>
        /// Земельні ділянки в розрізі регіону
        /// </summary>
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; } 
        /// <summary>
        /// Земельні ділянки в розрізі призначення
        /// </summary>
        public List<OrganizationAnalyticPurposes> purposes { get; set; } 
        /// <summary>
        /// ???
        /// </summary>
        public bool? registerError { get; set; }
    }
    /// <summary>
    /// Земельні ділянки в розрізі призначення
    /// </summary>
    public class OrganizationAnalyticPurposes 
    {/// <summary>
     /// Призначення код (відп.до довідника № 11)
     /// </summary>
        public int? purposeCode { get; set; }  
        /// <summary>
        /// К-ть земельних ділянок
        /// </summary>
        public int? objectCount { get; set; }
        /// <summary>
        /// Площа земельних ділянок, га
        /// </summary>
        public double? area { get; set; }
        /// <summary>
        /// К-ть судових рішень
        /// </summary>
        public int? courtCount { get; set; } 
    }
    /// <summary>
    /// Земельні ділянки в розрізі регіону
    /// </summary>
    public class OrganizationAnalyticRegionsEstate 
    {/// <summary>
     /// Регіон код (відп.до довідника № 9)
     /// </summary>
        public int? regionId { get; set; }  
        /// <summary>
        /// К-ть земельних ділянок
        /// </summary>
        public int? objectCount { get; set; }
        /// <summary>
        /// Площа земельних ділянок, га
        /// </summary>
        public double? area { get; set; }
        /// <summary>
        /// К-ть судових рішень
        /// </summary>
        public int? courtCount { get; set; } 
    }
    /// <summary>
    /// Аналіз участі в тендерах в розрізі CPV (період рік)
    /// </summary>
    public class OrganizationAnalyticTenderBidStatisticsTenderCpvStats 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int? year { get; set; }
        /// <summary>
        /// CPV код
        /// </summary>
        public long? cpv { get; set; }
        /// <summary>
        /// Кількість участей
        /// </summary>
        public int? bidCount { get; set; }
        /// <summary>
        /// Сума участей
        /// </summary>
        public double? bidSum { get; set; } 
        /// <summary>
        /// Кількість перемог
        /// </summary>
        public int? awardCount { get; set; }
        /// <summary>
        /// Сума перемог
        /// </summary>
        public double? awardSum { get; set; }
        /// <summary>
        /// К-ть організованних
        /// </summary>
        public int? organizerCount { get; set; } 
        /// <summary>
        /// Сума організованних
        /// </summary>
        public double? organizerSum { get; set; } 
    }
    /// <summary>
    /// Аналіз організованніх тендерів
    /// </summary>
    public class OrganizationAnalyticTenderOrganizerStatistics  
    {/// <summary>
     /// К-ть організованніх тендерів
     /// </summary>
        public int? announcedTenders { get; set; } 
        /// <summary>
        /// Сума організованих тендерів
        /// </summary>
        public double? sum { get; set; }
        /// <summary>
        /// Середній період отримання оплати
        /// </summary>
        public double? averageLag { get; set; } 
        /// <summary>
        /// Унікальних учасників
        /// </summary>
        public int? participants { get; set; }
        /// <summary>
        /// Середня к-ть унакальних учасників
        /// </summary>
        public double? concurrency { get; set; }
        /// <summary>
        /// Середня к-ть контрактів на одного учасника
        /// </summary>
        public double? avgContracts { get; set; } 
        /// <summary>
        /// К-ть тендерів з торгами
        /// </summary>
        public int? currentTendersCount { get; set; }
        /// <summary>
        /// Сума тендерів з торгами
        /// </summary>
        public double? currentTendersSum { get; set; } 
        /// <summary>
        /// К-ть неконкурентних тендерів
        /// </summary>
        public int? nonConcurrentTendersCount { get; set; }
        /// <summary>
        /// Сума неконкурентних тендерів
        /// </summary>
        public double? nonConcurrentTendersSum { get; set; } 
        /// <summary>
        ///К-ть конкурентних тендерів 
        /// </summary>
        public int? concurrentTendersCount { get; set; } 
        /// <summary>
        /// Сума конкурентних тендерів
        /// </summary>
        public double? concurrentTendersSum { get; set; }
        /// <summary>
        /// К-ть тендерів зі зміною ставки
        /// </summary>
        public int? changedBidsTendersCount { get; set; } 
        /// <summary>
        /// Сума тендерів зі зміною ставки
        /// </summary>
        public double? changedBidsTendersSum { get; set; }
        /// <summary>
        /// К-ть тендерів без зміни ставки
        /// </summary>
        public int? notChangedBidsTendersCount { get; set; }
        /// <summary>
        /// Сума тендерів без зміни ставки
        /// </summary>
        public double? notChangedBidsTendersSum { get; set; } 
        /// <summary>
        /// К-ть контрактів у конкурентних
        /// </summary>
        public int? curConNonConCount { get; set; }
        /// <summary>
        /// Сума контрактів у конкурентних
        /// </summary>
        public double? curConNonConSum { get; set; }
        /// <summary>
        /// Дод.угода зменшення ціни
        /// </summary>
        public double? priceReduction { get; set; }
        /// <summary>
        /// Дод.угода збільшення ціни
        /// </summary>
        public double? priceIncrease { get; set; }
        /// <summary>
        /// Дод.угода зміна об'єму
        /// </summary>
        public double? shippingReduction { get; set; }
        /// <summary>
        /// Дод.угода інше
        /// </summary>
        public double? other { get; set; }
        /// <summary>
        /// Загальна сума тендерів
        /// </summary>
        public int? sumTenders { get; set; } 
    }
    /// <summary>
    /// Аналіз участі в торгах 
    /// </summary>
    public class OrganizationAnalyticTenderBidStatistics  
    {/// <summary>
     /// Кількість дискваліцікацій
     /// </summary>
        public int? disqualified { get; set; }
        /// <summary>
        /// Середня конкуренція
        /// </summary>
        public double? avgConcurrency { get; set; }
        /// <summary>
        /// Загальна сума участей
        /// </summary>
        public double? sum { get; set; }
        /// <summary>
        /// Кількість перемог (лоти)
        /// </summary>
        public int? wins { get; set; }
        /// <summary>
        /// Кількість унакальних організаторів
        /// </summary>
        public int? organizators { get; set; }
        /// <summary>
        /// Загальна к-ть участей (тендери)
        /// </summary>
        public int? requests { get; set; }
        /// <summary>
        /// Поточна к-ть тендерів 
        /// </summary>
        public int? currentTendersCount { get; set; }
        /// <summary>
        /// Поточна сума тендерів 
        /// </summary>
        public double? currentTendersSum { get; set; }
        /// <summary>
        /// К-ть неконкурентних тендерів
        /// </summary>
        public int? nonConcurrentTendersCount { get; set; }
        /// <summary>
        /// Сума неконкурентних тендерів
        /// </summary>
        public double? nonConcurrentTendersSum { get; set; }
        /// <summary>
        /// К-ть конкурентних тендерів
        /// </summary>
        public int? concurrentTendersCount { get; set; }
        /// <summary>
        /// Сума конкурентних тендерів
        /// </summary>
        public double? concurrentTendersSum { get; set; }
        /// <summary>
        /// К-ть тендерів зі зміною ставки
        /// </summary>
        public int? changedBidsTendersCount { get; set; }
        /// <summary>
        /// Сума тендерів зі зміною ставки
        /// </summary>
        public double? changedBidsTendersSum { get; set; }
        /// <summary>
        /// К-ть тендерів без зміни ставки
        /// </summary>
        public int? notChangedBidsTendersCount { get; set; } 
        /// <summary>
        /// Сума тендерів без зміни ставки
        /// </summary>
        public double? notChangedBidsTendersSum { get; set; }
        /// <summary>
        /// К-ть неконкурентних тендерів без зміни ставки
        /// </summary>
        public int? curConNonConCount { get; set; } 
        /// <summary>
        /// Сума неконкурентних тендерів без зміни ставки
        /// </summary>
        public double? curConNonConSum { get; set; } 
        /// <summary>
        /// К-ть перемог в неконкурентних тендерах
        /// </summary>
        public int? winsNonConcurrentTendersCount { get; set; }
        /// <summary>
        /// Сума перемог в неконкурентних тендерах
        /// </summary>
        public double? winsNonConcurrentTendersSum { get; set; }
        /// <summary>
        /// К-ть перемог в конкурентних тендерах
        /// </summary>
        public int? winsConcurrentTendersCount { get; set; }
        /// <summary>
        /// Сума перемог в конкурентних тендерах
        /// </summary>
        public double? winsConcurrentTendersSum { get; set; }
        /// <summary>
        /// К-ть перемог в тендерах зі зміною ставки
        /// </summary>
        public int? winsChangedBidsTendersCount { get; set; }
        /// <summary>
        /// Сума перемог в тендерах зі зміною ставки
        /// </summary>
        public double? winsChangedBidsTendersSum { get; set; }
        /// <summary>
        /// К-ть перемог в тендерах без зміни ставки
        /// </summary>
        public int? winsNotChangedBidsTendersCount { get; set; }
        /// <summary>
        /// Сума перемог в тендерах без зміни ставки
        /// </summary>
        public double? winsNotChangedBidsTendersSum { get; set; }
        /// <summary>
        /// Сума перемог в неконкурентних тендерах без зміни ставки
        /// </summary>
        public int? winsCurConNonConCount { get; set; } 
        /// <summary>
        /// К-ть перемог в неконкурентних тендерах без зміни ставки
        /// </summary>
        public double? winsCurConNonConSum { get; set; } 
        /// <summary>
        /// К-ть перемог на першій мінімальній ціні
        /// </summary>
        public int? firstMinimalPriceWin { get; set; }
        /// <summary>
        /// К-ть поразок на першій мінімальній ціні
        /// </summary>
        public int? firstMinimalPriceLose { get; set; }
        /// <summary>
        /// Відсоток перемог на першій мінімальній ціні
        /// </summary>
        public int? firstMinimalPriceWinPercent { get; set; }
        /// <summary>
        /// Відсоток поразок на першій мінімальній ціні
        /// </summary>
        public int? firstMinimalPriceLosePercent { get; set; }
        /// <summary>
        /// К-ть перемог в тендерах без зміни ставок
        /// </summary>
        public int? notChangedBidWin { get; set; }
        /// <summary>
        /// К-ть поразок в тендерах без зміни ставок
        /// </summary>
        public int? notChangedBidLose { get; set; } 
        /// <summary>
        /// Відсоток перемог в тендерах без зміни ставок
        /// </summary>
        public int? notChangedBidWinPercent { get; set; } 
        /// <summary>
        /// Відсоток поразок в тендерах без зміни ставок
        /// </summary>
        public int? notChangedBidLosePercent { get; set; }
        /// <summary>
        /// К-ть перемог в тендерах на останній ставці
        /// </summary>
        public int? onLastBidWin { get; set; }
        /// <summary>
        /// К-ть поразок в тендерах на останній ставці
        /// </summary>
        public int? onLastBidLose { get; set; }
        /// <summary>
        ///  Відсоток перемог в тендерах на останній ставці
        /// </summary>
        public int? onLastBidWinPercent { get; set; }
        /// <summary>
        /// Відсоток поразок в тендерах на останній ставці
        /// </summary>
        public int? onLastBidLosePercent { get; set; }
        /// <summary>
        /// Загальна сумма тендерів
        /// </summary>
        public int? sumTenders { get; set; } 
    }
    /// <summary>
    /// Фінансова аналітика (період рік)
    /// </summary>
    public class OrganizationAnalyticFinancialBKI 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int year { get; set; }
        /// <summary>
        /// Основні засоби
        /// </summary>
        public double main_active { get; set; }
        /// <summary>
        /// Основні засоби в порівняні з попереднім періодом
        /// </summary>
        public double main_active_percent { get; set; }
        /// <summary>
        /// Поточні зобов'язання
        /// </summary>
        public double current_liabilities { get; set; }
        /// <summary>
        /// Поточні зобов'язання в порівняні з попереднім періодом
        /// </summary>
        public double current_liabilities_percent { get; set; }
        /// <summary>
        /// Виручка
        /// </summary>
        public double net_income { get; set; }
        /// <summary>
        /// Виручка в порівняні з попереднім періодом
        /// </summary>
        public double net_income_percent { get; set; }
        /// <summary>
        /// Дохід
        /// </summary>
        public double net_profit { get; set; }
        /// <summary>
        /// Дохід в порівняні з попереднім періодом
        /// </summary>
        public double net_profit_percent { get; set; }
    }
    /// <summary>
    /// Відомості про наявні санкції
    /// </summary>
    public class OrganizationAnalyticSanctions
    {/// <summary>
     /// Тип санкцій код (відп.до довідника № 11) 
     /// </summary>
        public int? sanctionTypeId { get; set; }
        /// <summary>
        /// Активні на даний момент (так - true / ні - false)
        /// </summary>
        public bool isActive { get; set; }
        /// <summary>
        /// Дата початку
        /// </summary>
        public DateTime? sanctionStart { get; set; }
        /// <summary>
        /// Дата закінчення
        /// </summary>
        public DateTime? sanctionEnd { get; set; } 
    }
    /// <summary>
    /// Дані експрес перевірки 
    /// </summary>
    public class OrganizationAnalyticExpressScore 
    {/// <summary>
     /// В процесі ліквідації
     /// </summary>
        public bool liquidation { get; set; } 
        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        public bool bankruptcy { get; set; }  
        /// <summary>
        /// Зв’язки з компаніями банкрутами
        /// </summary>
        public int? badRelations { get; set; }  
        /// <summary>
        /// Афілійовані зв’язки
        /// </summary>
        public int? sffiliatedMore { get; set; } 
        /// <summary>
        /// Санкційні списки
        /// </summary>
        public bool sanctions { get; set; }
        /// <summary>
        /// Виконавчі впровадження
        /// </summary>
        public long? introduction { get; set; } 
        /// <summary>
        /// Кримінальні справи
        /// </summary>
        public int? criminal { get; set; } 
        /// <summary>
        /// Включено в план-графік перевірок
        /// </summary>
        public bool audits { get; set; }
        /// <summary>
        /// Анульована реєстрація платника ПДВ
        /// </summary>
        public bool canceledVat { get; set; }
        /// <summary>
        /// Податковий борг
        /// </summary>
        public bool taxDebt { get; set; }
        /// <summary>
        /// Заборгованість по ЗП
        /// </summary>
        public decimal? wageArrears { get; set; } 
        /// <summary>
        /// Зміна основного КВЕД
        /// </summary>
        public bool kved { get; set; }
        /// <summary>
        /// Новий директор
        /// </summary>
        public bool newDirector { get; set; }
        /// <summary>
        /// Адреса масової реєстрації
        /// </summary>
        public int? massRegistration { get; set; }
        /// <summary>
        /// Нова компанія
        /// </summary>
        public bool youngCompany { get; set; }
        /// <summary>
        /// Зареєстровано на окупованій території
        /// </summary>
        public bool atoRegistered { get; set; } 
        /// <summary>
        /// Стаття 205 ККУ. Фіктивне підприємництво
        /// </summary>
        public bool fictitiousBusiness { get; set; }
        /// <summary>
        /// Керівник цієї компанії є керівником в інших компаніях
        /// </summary>
        public int? headOtherCompanies { get; set; }
        /// <summary>
        /// Не діюча ліцензія
        /// </summary>
        public int? notLicense { get; set; }
        /// <summary>
        /// Статус платника ПДВ не перевищує 3 місяців
        /// </summary>
        public bool vatLessThree { get; set; }
        /// <summary>
        /// Зв’язки з компаніями під санкціями
        /// </summary>
        public int? sanctionsRelationships { get; set; }
        /// <summary>
        /// Зв’язки з компаніями з окупованих територій
        /// </summary>
        public int? territoriesRelationships { get; set; }
        /// <summary>
        /// Зв’язки з компаніями, які мають кримінальні справи
        /// </summary>
        public int? criminalRelationships { get; set; } 
    }
    /// <summary>
    /// Аналітика по Edata період (місяць)
    /// </summary>
    public class OrganizationAnalyticEdata 
    {/// <summary>
     /// Період (місяць)
     /// </summary>
        public DateTime? period { get; set; }
        /// <summary>
        /// Загальна кількість транзакцій 
        /// </summary>
        public int? count { get; set; }
        /// <summary>
        /// Сума вхідних транзакцій 
        /// </summary>
        public double? paymentSumIn { get; set; }
        /// <summary>
        /// К-ть вхідних транзакцій 
        /// </summary>
        public double? paymentSumOnt { get; set; }
        /// <summary>
        /// Сума вихідних транзакцій 
        /// </summary>
        public long? paymentCountIn { get; set; }
        /// <summary>
        /// К-ть вихідних транзакцій 
        /// </summary>
        public long? paymentCountOnt { get; set; }
    }
    /// <summary>
    /// Публікації ВГСУ про банкрутство
    /// </summary>
    public class OrganizationAnalyticBankruptcy 
    {/// <summary>
     /// Тип публікації (відповідно таблиці)
     /// </summary>
        public int? publicationType { get; set; } // 
        /// <summary>
        /// Дата публікації повідомлення
        /// </summary>
        public DateTime? dateProclamation { get; set; } // 
        /// <summary>
        /// Загальна сума виставленрого майна
        /// </summary>
        public double? totalSumPossessions { get; set; } // 
    }
    /// <summary>
    /// Виконавчі проваждення
    /// </summary>
    public class OrganizationAnalyticEnforcements // 
    {/// <summary>
     /// Період (місяць)
     /// </summary>
        public DateTime? period { get; set; } // 
        /// <summary>
        /// Відкрито (компанія стягувач)
        /// </summary>
        public int? vidkrytoCreditor { get; set; } // 
        /// <summary>
        /// Завершено (компанія стягувач)
        /// </summary>
        public int? zavershenoCreditor { get; set; }//
        /// <summary>
        /// Інший статус (компанія стягувач)
        /// </summary>
        public int? inshyyStanCreditor { get; set; }// 
        /// <summary>
        /// Відкрито (компанія боржник)
        /// </summary>
        public int? vidkrytoDebitor { get; set; }// 
        /// <summary>
        /// Завершено (компанія боржник)
        /// </summary>
        public int? zavershenoDebitor { get; set; }// 
        /// <summary>
        /// Інший статус (компанія боржник)
        /// </summary>
        public int? inshyyStanDebitor { get; set; }// 
    }
    /// <summary>
    /// Аналітика по судовим в розрізі місяця
    /// </summary>
    public class OrganizationAnalytiCourtsAnalytics // 
    {/// <summary>
     /// Період (місяць)
     /// </summary>
        public DateTime? period { get; set; }// 
        /// <summary>
        /// Загальна кількість судових документів
        /// </summary>
        public int? documentsCount { get; set; }// 
        /// <summary>
        /// К-ть цивільних справ
        /// </summary>
        public int? tsyvilne { get; set; }// 
        /// <summary>
        ///К-ть кримінальних справ 
        /// </summary>
        public int? kryminalne { get; set; }// 
        /// <summary>
        /// К-ть господарських справ
        /// </summary>
        public int? hospodarske { get; set; }// 
        /// <summary>
        /// К-ть адміністративних справ
        /// </summary>
        public int? administratyvne { get; set; }// 
        /// <summary>
        /// К-ть справ про адмін правопорушення
        /// </summary>
        public int? admіnpravoporushennya { get; set; }// 
        /// <summary>
        /// К-ть справ з невизначеним статусом
        /// </summary>
        public int? inshe { get; set; } // 
        /// <summary>
        /// К-ть справ де сторона відповідач
        /// </summary>
        public int? vidpovidachi { get; set; }// 
        /// <summary>
        /// К-ть справ де сторона позивач
        /// </summary>
        public int? pozyvachi { get; set; }// 
        /// <summary>
        /// -ть справ де інша сторона
        /// </summary>
        public int? inshaStorona { get; set; }// К
        /// <summary>
        /// К-ть вигранних справ
        /// </summary>
        public int? vyhrano { get; set; }// 
        /// <summary>
        /// К-ть програнних справ
        /// </summary>
        public int? prohrano { get; set; }// 
    }
    /// <summary>
    /// Аналітика по засновникам в розрізі країни
    /// </summary>
    public class Founders // 
    {/// <summary>
     /// Країна код (відп.до довідника № 8)
     /// </summary>
        public int? countryId { get; set; } // 
        /// <summary>
        /// Кількість персон (з вказаної країни)
        /// </summary>
        public int? personCount { get; set; }// 
        /// <summary>
        /// Кількість компаній (з вказаної країни)
        /// </summary>
        public int? companyCount { get; set; }// 
        /// <summary>
        /// Сума статутного капіталу персон (з вказаної країни)
        /// </summary>
        public decimal? personCapitalSum { get; set; }// 	
        /// <summary>
        /// Сума статутного капіталу компаній (з вказаної країни)	
        /// </summary>
        public decimal? companyCapitalSum { get; set; }// 
        /// <summary>
        /// %, статутного капіталу персон (з вказаної країни)	
        /// </summary>
        public decimal? personCapitalPercent { get; set; }// 
        /// <summary>
        /// %, статутного капіталу компаній (з вказаної країни)
        /// </summary>
        public decimal? companyCapitalPercent { get; set; }// 

    }
    /// <summary>
    /// Загальна аналітика по судовим рішенням
    /// </summary>
    public class TotalCourts // 
    {/// <summary>
     /// К-ть цивільних справ
     /// </summary>
        public long? civil { get; set; }// 
        /// <summary>
        /// К-ть кримінальних справ
        /// </summary>
        public long? criminal { get; set; }// 
        /// <summary>
        /// К-ть господарських справ
        /// </summary>
        public long? household { get; set; }// 
        /// <summary>
        /// К-ть адміністративних справ
        /// </summary>
        public long? administrative { get; set; }// 
        /// <summary>
        /// К-ть справ про адмін.правопорушення
        /// </summary>
        public long? adminoffense { get; set; }// 
        /// <summary>
        /// К-ть справ в яких позивач
        /// </summary>
        public long? plaintiff { get; set; }// 
        /// <summary>
        /// К-ть справ в яких відповідач
        /// </summary>
        public long? defendant { get; set; }// 
        /// <summary>
        /// К-ть справ в яких інша сторона
        /// </summary>
        public long? otherSide { get; set; }// 
        /// <summary>
        /// К-ть справ в яких програв
        /// </summary>
        public long? lost { get; set; }// 
        /// <summary>
        /// К-ть справ в яких переміг
        /// </summary>
        public long? win { get; set; }// 
        /// <summary>
        /// К-ть цивільних справ
        /// </summary>
        public long? appointedConsideration { get; set; }// 
        /// <summary>
        /// К-ть документів
        /// </summary>
        public long? totalDecision { get; set; }// 
        /// <summary>
        /// К-ть справ
        /// </summary>
        public long? casecount { get; set; }// 
        /// <summary>
        /// К-ть справ в процесі
        /// </summary>
        public long? inprocess { get; set; }// 

    }/// <summary>
     /// Аналітика по деклараціям  (період рік)
     /// </summary>
    public class Declarations // 
    {/// <summary>
     /// Період (рік)
     /// </summary>
        public int? declarationYearInt { get; set; }// 
        /// <summary>
        /// К-ть осіб які мають корпоративні права
        /// </summary>
        public int? corporateRightsCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які є бенефіціарами
        /// </summary>
        public int? beneficiaryOwnersCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які мають нематериальні активи
        /// </summary>
        public int? intangibleAssetsCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які мають доходи, у тому числі подарунки
        /// </summary>
        public int? incomefinanceCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які мають грошові активи
        /// </summary>
        public int? moneyAssetsCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які мають фінансові зобов'язання
        /// </summary>
        public int? financialLiabilitiesCount { get; set; }// 
        /// <summary>
        /// К-ть осіб які є членами в організаці
        /// </summary>
        public int? membershipOrgCount { get; set; }// 
        /// <summary>
        /// Сума часток осіб які мають корпоративні права
        /// </summary>
        public decimal? corporateRightsSum { get; set; }// 
        /// <summary>
        /// Сума часток осіб які є бенефіціарами
        /// </summary>
        public decimal? beneficiaryOwnersSum { get; set; }// 
        /// <summary>
        /// Сума нематериальних активів
        /// </summary>
        public decimal? intangibleAssetsSum { get; set; }// 
        /// <summary>
        /// Сума доходів, у тому числі подарунків
        /// </summary>
        public decimal? incomefinanceSum { get; set; }// 
        /// <summary>
        /// Сума грошових активів
        /// </summary>
        public decimal? moneyAssetsSum { get; set; }// 
        /// <summary>
        /// // Сума фінансових зобов'язань
        /// </summary>
        public decimal? financialLiabilitiesSum { get; set; }
        /// <summary>
        /// Членство в організаціях
        /// </summary>
        public decimal? membershipOrgSum { get; set; } // 

    }
    /// <summary>
    /// Статистика по об`єктів інтелектуальної власності в розрізі місяця (патентам / ТМ)
    /// </summary>
    public class Patents                                                                // 
    {
        /// <summary>
        /// Період (помісячно)
        /// </summary>
        public DateTime? period { get; set; }                                           // 
        /// <summary>
        /// Промислові зразки
        /// </summary>
        public int? patentsDesignsCount { get; set; }                                   // 
        /// <summary>
        /// Патенти
        /// </summary>
        public int? patentsCount { get; set; }                                          // 
        /// <summary>
        /// ТМ
        /// </summary>
        public int? tradeMarkCount { get; set; }                                        // 
        /// <summary>
        /// Корисні моделі
        /// </summary>
        public int? usefulModelsCount { get; set; }                                     // 
        /// <summary>
        /// Топографії інтегральних мікросхем
        /// </summary>
        public int? integratedСircuitsCount { get; set; }                               // 

    }/// <summary>
     /// Аналіз тендерів в розрізі періоду (місяць)
     /// </summary>
    public class Tenders // 
    {/// <summary>
     /// Період (місяць)
     /// </summary>
        public DateTime? periodDete { get; set; } // 
        /// <summary>
        /// Кількість тендерів в яких брав участь
        /// </summary>
        public int? uchastCount { get; set; }// 
        /// <summary>
        /// Сума тендерів в яких брав участь, грн.
        /// </summary>
        public double? uchastSum { get; set; }// 
        /// <summary>
        /// Кількість тендерів в яких переміг
        /// </summary>
        public int? peremohyCount { get; set; }// 
        /// <summary>
        /// Сума тендерів в яких переміг, грн.
        /// </summary>
        public double? peremohySum { get; set; }// 
        /// <summary>
        /// Кількість перемог в конкурентних тендерах
        /// </summary>
        public int? peremohyConcurCount { get; set; }// 
        /// <summary>
        /// Сума перемог в конкурентних тендерах, грн.
        /// </summary>
        public double? peremohyConcurSum { get; set; }// 
    }
    /// <summary>
    /// ПДВ 
    /// </summary>
    public class VatPayers // 
    {/// <summary>
     /// Платник | Не платник
     /// </summary>
        public bool vatPayer { get; set; }// 
        /// <summary>
        /// Дата отримання
        /// </summary>
        public DateTime? vatPayerDate { get; set; } // 
        /// <summary>
        /// ПДВ анулюванно
        /// </summary>
        public bool vatPayerCancel { get; set; } // 
        /// <summary>
        /// Дата анулювання
        /// </summary>
        public DateTime? vatPayerCancelDate { get; set; }// 
    }
    /// <summary>
    /// Дата створення / закрыття компанії / ФОП
    /// </summary>
    public class Date // 
    {/// <summary>
     /// Дата створення компанії / ФОП
     /// </summary>
        public DateTime? dateOpened { get; set; }// 
        /// <summary>
        /// Дата закрыття компанії / ФОП
        /// </summary>
        public DateTime? dateCanceled { get; set; } // 
    }
    /// <summary>
    /// Стани організації
    /// </summary>
    public class State                                                                          // 
    {/// <summary>
     /// Стан організації (зареєстровано, припинено)(maxLength:64)
     /// </summary>
        public string orgState { get; set; }                                                    // 
        /// <summary>
        /// Чи перебуває в процесі банкрутства(maxLength:128)
        /// </summary>
        public string bankruptcyState { get; set; }                                             // 
        /// <summary>
        /// Чи перебуває в процесі припиненні(maxLength:128)
        /// </summary>
        public string canceledState { get; set; }                                               // 
    }
    /// <summary>
    /// Організаційно-правова форма(ОПФ) та форма власності
    /// </summary>
    public class Ownership                                                                      // 
    {/// <summary>
     /// Форма власності Id { 1, "Державна власність"}, { 2, "Комунальна власність"},  { 3, "Змішана власність (Державна частка перевищує 50 відсотків)"}, // Змішана власність (Державна частка перевищує 50 відсотків)  { 4, "Недержавна власність"},
     /// </summary>
        public long? Id { get; set; }                                                           //      
        /// <summary>
        /// Форма власності назва { 1, "Державна власність"}, { 2, "Комунальна власність"}, { 3, "Змішана власність (Державна частка перевищує 50 відсотків)"}, // Змішана власність (Державна частка перевищує 50 відсотків) { 4, "Недержавна власність"},
        /// </summary>
        public string form { get; set; }                                                        //        
        /// <summary>
        /// ОПФ код
        /// </summary>
        public int? olf_code { get; set; }                                                      // 
    }
    /// <summary>
    /// Аналіз ліцензій (в розрізі органу ліцензування)
    /// </summary>
    public class OrganizationLicensesElastic // 
    {/// <summary>
     /// Орган ліцензування код (відп.до довідника № 6)
     /// </summary>
        public int? organInt { get; set; }// 
        /// <summary>
        /// Тип ліцензії (відп.до довідника № 7)
        /// </summary>
        public int? typeOfLicenseInt { get; set; }// 
        /// <summary>
        /// К-ть не актуальних ліцензій
        /// </summary>
        public long? actualCount { get; set; }// 
        /// <summary>
        /// К-ть актуальних ліцензій
        /// </summary>
        public long? nonActualCount { get; set; }// 
    }
    /// <summary>
    /// Історія реєстрацийних змін
    /// </summary>
    public class ChangeHistory// 
    {/// <summary>
     /// Зміни адреси
     /// </summary>
        public List<DateTime> changeAdress { get; set; } // 
        /// <summary>
        /// Зміни керівників
        /// </summary>
        public List<DateTime> changeChief { get; set; }// 
        /// <summary>
        /// Зміни кведів
        /// </summary>
        public List<DateTime> changeKved { get; set; }// 
        /// <summary>
        /// Зміни назв
        /// </summary>
        public List<DateTime> changeName { get; set; }// 
        /// <summary>
        /// Зміни статуса
        /// </summary>
        public List<DateTime> changeState { get; set; }// 
        /// <summary>
        /// Зміни бенефіціарів
        /// </summary>
        public List<DateTime> changeFounder { get; set; }// 
    }
    /// <summary>
    /// Динаміка податкового боргу (період місяць)
    /// </summary>
    public class DebtorsBorg // 
    {/// <summary>
     /// Період (місяць)
     /// </summary>
        public DateTime? period { get; set; }// 
        /// <summary>
        /// Сума заборгованості перед державним бюджетом
        /// </summary>
        public double db { get; set; }// 
        /// <summary>
        /// Сума заборгованості перед місцевим бюджетом
        /// </summary>
        public double mb { get; set; }// 

        /// <summary>
        /// Сума заборгованості по заробітній платі
        /// </summary>
        public double? zp { get; set; }
    }
    /// <summary>
    /// Аналітика по перевіркам
    /// </summary>
    public class OrgChecks// 
    {/// <summary>
     /// Дата події
     /// </summary>
        public DateTime? checkDate { get; set; }// 
        /// <summary>
        /// Запранована перевірка (так - true / ні - false)
        /// </summary>
        public bool? isPlanned { get; set; }// 
        /// <summary>
        /// Перевіряючий орган код (відп.до довідника № 4))
        /// </summary>
        public int? regulatorId { get; set; }
        /// <summary>
        /// Статус перевірки (відп.до довідника № 5)
        /// </summary>
        public int? statusInt { get; set; }
        /// <summary>
        /// Тип штрафних санкцій (maxLength:256)
        /// </summary>
        public string sanctionType { get; set; }                                                
        /// <summary>
        /// Сума штрафу
        /// </summary>
        public double? sanctionAmount { get; set; }

    }

    /// <summary>
    /// Дані по ЗЄД в розрізі групи товарів (або країни) та року
    /// </summary>
    public class FeaCountryGroup                                                                // 
    {/// <summary>
     /// Рік
     /// </summary>
        public int? year { get; set; }                                                          // 
        /// <summary>
        /// Це імпорт? (true - так / false - ні)
        /// </summary>
        public bool? isImport { get; set; }                                                     // 
        /// <summary>
        /// Код країни / ВЕД
        /// </summary>
        public int? code { get; set; }                                                          // 
        /// <summary>
        /// Сумма
        /// </summary>
        public double? totalSum { get; set; }                                                   // 
        /// <summary>
        /// Позиція на ринку (за даний рік по даному коду)
        /// </summary>
        public int? position { get; set; }                                                      // 
        /// <summary>
        /// Відсоток ринку (за даний рік по даному коду)
        /// </summary>
        public double? persent { get; set; }                                                    // 
    }
    /// <summary>
    /// Відомості про єдиний податок
    /// </summary>
    public class SinglePaxPayer                                                                 // 
    {/// <summary>
     /// Група ЄП
     /// </summary>
        public int? group { get; set; }                                                         // 
        /// <summary>
        /// Чи є платников єдиного податку (true - так / false - ні)
        /// </summary>
        public bool? singlePayer { get; set; }                                                  // 
        /// <summary>
        /// Дата анулювання
        /// </summary>
        public DateTime? singlePayerCancelDate { get; set; }                                    // 
        /// <summary>
        /// Дата отримання свідоцтва ЄП
        /// </summary>
        public DateTime? singlePayerDate { get; set; }                                          // 
        /// <summary>
        /// Ставка ЄП
        /// </summary>
        public int? stavka { get; set; }                                                        // 
    }

    /// <summary>
    /// Системна інформація (не використовується)
    /// </summary>
    public class LtStAnalyticsModel                             
    {/// <summary>
    /// ???
    /// </summary>
        public int? year { get; set; }                              
        /// <summary>
        /// ???
        /// </summary>
        public bool? isBigForm { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public float? equity { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public float? ltStNetDebt { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public float? ltStDebtSales { get; set; }
    }
    /// <summary>
    /// Відомості про КОАТУУ 
    /// </summary>
    public class OrganizationAnalyticKoatuInfo                  // 
    {/// <summary>
     /// Розряди 1-2 – перший рівень класифікації.
     /// </summary>
        public int? firstLevel { get; set; }                    //  
        /// <summary>
        /// Включає в себе Автономну Республіку Крим , області України , міста зі спеціальним статусом ( Київ , Севастополь ).
        /// </summary>
        public int? secondLevel { get; set; }                   // Розряди 3-5 - другий рівень класифікації. Включає міста обласного значення, райони Автономної Республіки Крим та областей, райони в містах зі спеціальним статусом.
        /// <summary>
        /// Розряди 6-8 - третій рівень класифікації. Включає міста районного значення, райони в містах обласного значення, селища міського типу ,сільські та селищні ради.
        /// </summary>
        public int? thirdLevel { get; set; }                    //  
        /// <summary>
        /// Розряди 9-10 - четвертий рівень класифікації. Включає в себе села та селища.
        /// </summary>
        public int? fourthLevel { get; set; }                   // 
        /// <summary>
        /// Повний КОАТУУ (приведений до формату цілого числа)
        /// </summary>
        public long? fullKoatu { get; set; }                    // 
    }
    /// <summary>
    /// Основні показники балансу
    /// </summary>
    public class ImportMainBalanceIndicators                                // 
    {/// <summary>
     /// Рік
     /// </summary>
        public int period { get; set; }                                     // 
        /// <summary>
        /// I. Необоротні активи // Усього за розділом I // 1095
        /// </summary>
        public double? d108002UsohozarozdilomID1109502 { get; set; }        // 
        /// <summary>
        /// II. Оборотні активи // Усього за розділом II // 1195
        /// </summary>
        public double? d126002UsohozarozdilomIID1119502 { get; set; }       // 
        /// <summary>
        /// III. Необоротні активи, утримувані для продажу, та групи вибуття // 1200
        /// </summary>
        public double? d124002IIINeoborotniaktyvy { get; set; }             // 
        /// <summary>
        /// Баланс // 1300
        /// </summary>
        public double? d128002BalansaktivuD1130002 { get; set; }            // 
        /// <summary>
        /// I. Власний капітал // Усього за розділом I // 1495
        /// </summary>
        public double? d138002UsohozarozdilomID1149502 { get; set; }        // 
        /// <summary>
        /// II. Довгострокові зобов'язання і забезпечення // Усього за розділом II // 1595
        /// </summary>
        public double? d148002UsohozarozdilomIID1159502 { get; set; }       // 
        /// <summary>
        /// IІІ. Поточні зобов'язання і забезпечення // Усього за розділом IІІ // 1695
        /// </summary>
        public double? d162002UsohozarozdilomIIID1169502 { get; set; }      // 
        /// <summary>
        /// ІV. Зобов'язання, пов'язані з необоротними активами, утримуваними для продажу, та групами вибуття // 1700
        /// </summary>
        public double? d1170002D141802IVZobovyazanny { get; set; }          // 
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) // 2000
        /// </summary>
        public double? d203501Chystyydokhid { get; set; }                   // 
        /// <summary>
        /// Фінансовий результат від операційної діяльності: прибуток // 2190
        /// </summary>
        public double? d210001Finansovyyrezul { get; set; }                 // 
        /// <summary>
        /// Фінансовий результат від операційної діяльності: збиток // 2195
        /// </summary>
        public double? d210501FinRez { get; set; }                          // 
        /// <summary>
        /// Фінансовий результат до оподаткування: // прибуток // 2290 
        /// </summary>
        public double? d217001FinRezdoopodatkuvannyaprybutok { get; set; }  // 
        /// <summary>
        /// Фінансовий результат до оподаткування: // збиток // 2295
        /// </summary>
        public double? d217501FinRezdoopodatkuvannyazbytok { get; set; }    // 
        /// <summary>
        /// Чистий фінансовий результат: прибуток // 2350
        /// </summary>
        public double? d222001ChystyyFinRezprybutokD2235001 { get; set; }   // 
        /// <summary>
        /// Чистий фінансовий результат: збиток // 2355
        /// </summary>
        public double? d222501ChystyyFinRezzbytokD2235501 { get; set; }     // 

        /// <summary>
        /// Разом доходи (малі підприємства)
        /// </summary>
        public double? d207001RazomdokhodyD2228001 { get; set; }            // 
        /// <summary>
        /// Разом витрати (малі підприємства)
        /// </summary>
        public double? d212001RazomvytratyD2228501 { get; set; }            // 
        /// <summary>
        /// Форма звітності (1 - велика / 2- мала)
        /// </summary>
        public int forma { get; set; }                                      // 
    }
}
