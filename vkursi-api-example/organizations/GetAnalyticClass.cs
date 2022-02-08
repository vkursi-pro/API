﻿using Newtonsoft.Json;
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

    public class GetAnalyticRequestBodyModel                                    // Модель Body запиту
    {
        public string code { get; set; }                                        // Код ЄДРПОУ / ІПН (maxLength:10)
    }

    public class GetAnalyticResponseModel                                       // Модель відповіді GetAnalytic
    {
        public string orgId { get; set; }                                       // Id організації (maxLength:64)
        public string name { get; set; }                                        // Назва ЮО / ФОП (maxLength:512)
        public bool legalEntity { get; set; }                                   // Тип ЮО - true/ ФОП - false
        public string edrpou { get; set; }                                      // код ЄДРПОУ | ІПН (maxLength:10)
        public int? stateInt { get; set; }                                      // Статус код ( 1 - зареєстровано, 2 - припинено, ... відп.до довідника № 1. Стан суб’єкта)
        public State state { get; set; }                                        // Статуc назва
        public double totalAmount { get; set; }                                 // Сума статутного капіталу
        public VatPayers vatPayers { get; set; }                                // ПДВ 
        public List<Tenders> tenders { get; set; }                              // Аналіз тендерів в розрізі періоду (місяць)
        public List<Patents> patents { get; set; }                              // // Статистика по об`єктів інтелектуальної власності в розрізі місяця (патентам / ТМ)
        public List<Declarations> declarations { get; set; }                    // Аналітика по деклараціям  (період рік)
        public Date date { get; set; }                                          // Дата створення / закрыття компанії / ФОП
        public TotalCourts totalCourts { get; set; }                            // Загальна аналітика по судовим рішенням

        public List<OrganizationAnalytiCourtsAnalytics> courtsAnalytics { get; set; }           // Аналітика по судовим в розрізі місяця
        public List<OrganizationAnalytiCourtsAnalytics> courtsAssignedAnalytics { get; set; }   // Аналітика по справам призначенним до розгляду в розрізі місяця
        public List<OrganizationAnalyticEnforcements> enforcements { get; set; }                // Виконавчі провадження
        public OrganizationAnalyticEnforcementsStatistic enforcementsStatistic { get; set; }    // Виконавчі провадження по категоріям сторін
        public List<OrganizationAnalyticBankruptcy> bankruptcy { get; set; }    // Публікації ВГСУ про банкрутство
        public List<OrganizationAnalyticEdata> edata { get; set; }              // Аналітика по Edata період (місяць)
        public List<OrgChecks> orgChecks { get; set; }                          // Аналітика по перевіркам
        public List<DebtorsBorg> debtorsBorg { get; set; }                      // Динаміка податкового боргу (період місяць)
        public ChangeHistory changeHistory { get; set; }                        // Історія реєстрацийних змін
        public List<OrganizationLicensesElastic> organizationLicenses { get; set; }             // Аналіз ліцензій (в розрізі органу ліцензування)
        public OrganizationAnalyticExpressScore expressScore { get; set; }      // Дані експрес перевірки  
        public List<OrganizationAnalyticSanctions> sanctions { get; set; }      // Відомості про наявні санкції
        public List<int> kvedsInt { get; set; }                                 // Основний квед Id
        public List<Ownership> ownership { get; set; }                          // Форма власності
        public List<Founders> founders { get; set; }                            // Аналітика по засновникам в розрізі країни
        public List<OrganizationAnalyticFinancialBKI> financial { get; set; }   // Фінансова аналітика (період рік)
        public OrganizationAnalyticTenderBidStatistics tenderBidStatistics { get; set; }                        // Аналіз участі в торгах 
        public OrganizationAnalyticTenderOrganizerStatistics tenderOrganizerStatistics { get; set; }            // Аналіз організованніх тендерів

        public List<OrganizationAnalyticTenderBidStatisticsTenderCpvStats> tenderCpvStats { get; set; }         // Аналіз участі в тендерах в розрізі CPV (період рік)
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand realEstateRightsLand { get; set; }   // Аналіз земельних ділянок
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj realEstateRightsObj { get; set; }     // Аналіз об'єктів нерухомого майна (крім земельних ділянок)
        public List<OrganizationAnalyticOrganizationFEAModel> financialFEA { get; set; }                        // Аналіз ЗЕД (період рік)
        public List<OrganizationAnalyticFinancialRisks> financialRisks { get; set; }                            // Аналіз фінансових ризиків (період рік) (відп.до довідника № 12) 
        public List<OrganizationAnalyticEmployeesModel> employees { get; set; }                                 // Дані про кількість співробітників (період рік)
        public List<string> vehiclesInfo { get; set; }                                                          // Системна інформація (не використовується)
        public OrganizationAnalyticKoatuInfo koatuInfo { get; set; }                                            // Дані про код КОАТУУ
        public List<LtStAnalyticsModel> equityLtStAnalytics { get; set; }                                       // Системна інформація (не використовується)
        public SinglePaxPayer singlePayers { get; set; }                                                        // Відомості про єдиний податок
        public int? shareholdersCount { get; set; }                                                             // Системне поле
        public int? branchCount { get; set; }                                                                   // Системне поле
        public List<FeaCountryGroup> feaCountry { get; set; }                                                   // Дані по ЗЄД в розрізі країни та року
        public List<FeaCountryGroup> feaGroup { get; set; }                                                     // Дані по ЗЄД в розрізі групи товарів та року
        public List<ImportMainBalanceIndicators> mainBalanceIndicators { get; set; }                            // Основні показники балансу
    }


    public class OrganizationAnalyticEmployeesModel                             // Дані про кількість співробітників (період рік)
    {
        public int? year { get; set; }                                          // Період (рік)
        public int? count { get; set; }                                         // К-ть співробітників
        public int? differentPrevCount { get; set; }                            // Різниця в к-ті співробітників з попереднім періодом
    }


    public class OrganizationAnalyticFinancialRisks                             // Аналіз фінансових ризиків (період рік) (відп.до довідника № 12) 
    {
        public int? year { get; set; }                                          // Період (рік)
        public int? kvedGroupNumb { get; set; }                                 // Група за квед
        public int? debtClass { get; set; }                                     // Класс боржника
        public int? metricsCount { get; set; }                                  // Кількість показників
        public int? risksCategoryInt { get; set; }                              // Категорія
    }

    public class OrganizationAnalyticOrganizationFEAModel // Аналіз ЗЕД (період рік)
    {
        public int? year { get; set; }// Період (рік)
        public bool? isImport { get; set; }// Імпорт (true) / Експорт (false)
        public int? operationsCount { get; set; }// Кількість операцій
        public double? operationsSum { get; set; }// Сума операцій
        public double? groupPersent { get; set; }// Відсоток основної групи
        public int? groupCount { get; set; }// Загальна кількість груп імпорту / експорту
        public int? largestGroupCode { get; set; }// Код найбільшої групи імпорту / експорту
        public int? largestCountryCode { get; set; }// Код найбільшої країни імпорту / експорту
        public int? countryCount { get; set; }// Загальна кількість країн імпорту / експорту
        public decimal? countryLargestPersent { get; set; }// Відсоток найбільної країни
    }


    public class OrganizationAnalyticEnforcementsStatistic // Виконавчі провадження по категоріям сторін
    {
        public long? govermentCountDebtor { get; set; } // Держава стягувач
        public long? personCountDebtor { get; set; } // ФО стягувач
        public long? orgCountDebtor { get; set; } // Організація стягувач
        public long? anotherCountDebtor { get; set; } // Інша категорія стягувачів
        public long? govermentCountCreditor { get; set; } // Держава боржник
        public long? personCountCreditor { get; set; } // ФО боржник
        public long? orgCountCreditor { get; set; } // Організація боржник
        public long? anotherCountCreditor { get; set; } // Інша категорія боржник
    }


    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj
    {
        public int? full { get; set; }              // Загальна кількість
        public int? vlasnyk { get; set; }           // Власник 
        public int? pravonabuvach { get; set; }     // Правонабувач
        public int? pravokorystuvach { get; set; }  // Правокористувач
        public int? zemlevlasnyk { get; set; }      // Землевласник
        public int? zemlevolodilets { get; set; }   // Землеволоділець
        public int? inshyy { get; set; }            // Інший
        public int? naymach { get; set; }           // Наймач
        public int? orendar { get; set; }           // Орендар
        public int? naymodavets { get; set; }       // Наймодавець
        public int? orendodavets { get; set; }      // Орендодавець
        public int? upravytel { get; set; }         // Управитель
        public int? vyhodonabuvach { get; set; }    // Вигодонабувач
        public int? ustanovnyk { get; set; }        // Установник
        public int? ipotekoderzhatel { get; set; }  // Іпотекодержатель
        public int? maynovyyPoruchytel { get; set; }// Майновий поручитель
        public int? ipotekodavets { get; set; }     // Іпотекодавець
        public int? borzhnyk { get; set; }          // Боржник
        public int? obtyazhuvach { get; set; }      // Обтяжувач
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; } // Особа, майно/права якої обтяжуються
        public int? osobaVinteresakhYakoyi { get; set; } // Особа, в інтересах якої встановлено обтяження
        public bool? registerError { get; set; }    // Помилка при оновленні
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; } // Об'єкти нерухомого майна в розрізі регіону
    }

    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand // Аналіз земельних ділянок
    {
        public int? full { get; set; }              // Загальна кількість
        public int? vlasnyk { get; set; }           // Власник 
        public int? pravonabuvach { get; set; }     // Правонабувач
        public int? pravokorystuvach { get; set; }  // Правокористувач
        public int? zemlevlasnyk { get; set; }      // Землевласник
        public int? zemlevolodilets { get; set; }   // Землеволоділець
        public int? inshyy { get; set; }            // Інший
        public int? naymach { get; set; }           // Наймач
        public int? orendar { get; set; }           // Орендар
        public int? naymodavets { get; set; }       // Наймодавець
        public int? orendodavets { get; set; }      // Орендодавець
        public int? upravytel { get; set; }         // Управитель
        public int? vyhodonabuvach { get; set; }    // Вигодонабувач
        public int? ustanovnyk { get; set; }        // Установник
        public int? ipotekoderzhatel { get; set; }  // Іпотекодержатель
        public int? maynovyyPoruchytel { get; set; }// Майновий поручитель
        public int? ipotekodavets { get; set; }     // Іпотекодавець
        public int? borzhnyk { get; set; }          // Боржник
        public int? obtyazhuvach { get; set; }      // Обтяжувач
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; } // Особа, майно/права якої обтяжуються
        public int? osobaVinteresakhYakoyi { get; set; } // Особа, в інтересах якої встановлено обтяження
        public double? ploshcha { get; set; } // Загальна площа
        public double? ploshchaObroblena { get; set; } // Площа в обробітку (офіційно)
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; }// Земельні ділянки в розрізі регіону
        public List<OrganizationAnalyticPurposes> purposes { get; set; }// Земельні ділянки в розрізі призначення
        public bool? registerError { get; set; }
    }

    public class OrganizationAnalyticPurposes // Земельні ділянки в розрізі призначення
    {
        public int? purposeCode { get; set; } // Призначення код (відп.до довідника № 11)
        public int? objectCount { get; set; }// К-ть земельних ділянок
        public double? area { get; set; }// Площа земельних ділянок, га
        public int? courtCount { get; set; }// К-ть судових рішень
    }

    public class OrganizationAnalyticRegionsEstate // Земельні ділянки в розрізі регіону
    {
        public int? regionId { get; set; } // Регіон код (відп.до довідника № 9)
        public int? objectCount { get; set; }// К-ть земельних ділянок
        public double? area { get; set; }// Площа земельних ділянок, га
        public int? courtCount { get; set; } // К-ть судових рішень
    }

    public class OrganizationAnalyticTenderBidStatisticsTenderCpvStats // Аналіз участі в тендерах в розрізі CPV (період рік)
    {
        public int? year { get; set; }// Період (рік)
        public long? cpv { get; set; }// CPV код
        public int? bidCount { get; set; }// Кількість участей
        public double? bidSum { get; set; }// Сума участей
        public int? awardCount { get; set; }// Кількість перемог
        public double? awardSum { get; set; }// Сума перемог
        public int? organizerCount { get; set; } // К-ть організованних
        public double? organizerSum { get; set; }// Сума організованних
    }

    public class OrganizationAnalyticTenderOrganizerStatistics // Аналіз організованніх тендерів
    {
        public int? announcedTenders { get; set; } // К-ть організованніх тендерів
        public double? sum { get; set; }// Сума організованих тендерів
        public double? averageLag { get; set; } // Середній період отримання оплати
        public int? participants { get; set; }// Унікальних учасників
        public double? concurrency { get; set; }// Середня к-ть унакальних учасників
        public double? avgContracts { get; set; }// Середня к-ть контрактів на одного учасника
        public int? currentTendersCount { get; set; }// К-ть тендерів з торгами
        public double? currentTendersSum { get; set; } // Сума тендерів з торгами
        public int? nonConcurrentTendersCount { get; set; }// К-ть неконкурентних тендерів
        public double? nonConcurrentTendersSum { get; set; } // Сума неконкурентних тендерів
        public int? concurrentTendersCount { get; set; }// К-ть конкурентних тендерів
        public double? concurrentTendersSum { get; set; }// Сума конкурентних тендерів
        public int? changedBidsTendersCount { get; set; }// К-ть тендерів зі зміною ставки
        public double? changedBidsTendersSum { get; set; }// Сума тендерів зі зміною ставки
        public int? notChangedBidsTendersCount { get; set; }// К-ть тендерів без зміни ставки
        public double? notChangedBidsTendersSum { get; set; }// Сума тендерів без зміни ставки
        public int? curConNonConCount { get; set; }// К-ть контрактів у конкурентних
        public double? curConNonConSum { get; set; }// Сума контрактів у конкурентних
        public double? priceReduction { get; set; }// Дод.угода зменшення ціни
        public double? priceIncrease { get; set; }// Дод.угода збільшення ціни
        public double? shippingReduction { get; set; }// Дод.угода зміна об'єму
        public double? other { get; set; }// Дод.угода інше
        public int? sumTenders { get; set; } // Загальна сума тендерів
    }

    public class OrganizationAnalyticTenderBidStatistics // Аналіз участі в торгах 
    {
        public int? disqualified { get; set; }// Кількість дискваліцікацій
        public double? avgConcurrency { get; set; }// Середня конкуренція
        public double? sum { get; set; }// Загальна сума участей
        public int? wins { get; set; }// Кількість перемог (лоти)
        public int? organizators { get; set; }// Кількість унакальних організаторів
        public int? requests { get; set; }// Загальна к-ть участей (тендери)
        public int? currentTendersCount { get; set; }// Поточна к-ть тендерів 
        public double? currentTendersSum { get; set; }// Поточна сума тендерів 
        public int? nonConcurrentTendersCount { get; set; }// К-ть неконкурентних тендерів
        public double? nonConcurrentTendersSum { get; set; }// Сума неконкурентних тендерів
        public int? concurrentTendersCount { get; set; }// К-ть конкурентних тендерів
        public double? concurrentTendersSum { get; set; }// Сума конкурентних тендерів
        public int? changedBidsTendersCount { get; set; }// К-ть тендерів зі зміною ставки
        public double? changedBidsTendersSum { get; set; }// Сума тендерів зі зміною ставки
        public int? notChangedBidsTendersCount { get; set; }// К-ть тендерів без зміни ставки
        public double? notChangedBidsTendersSum { get; set; }// Сума тендерів без зміни ставки
        public int? curConNonConCount { get; set; } // К-ть неконкурентних тендерів без зміни ставки
        public double? curConNonConSum { get; set; } // Сума неконкурентних тендерів без зміни ставки
        public int? winsNonConcurrentTendersCount { get; set; }// К-ть перемог в неконкурентних тендерах
        public double? winsNonConcurrentTendersSum { get; set; }// Сума перемог в неконкурентних тендерах
        public int? winsConcurrentTendersCount { get; set; }// К-ть перемог в конкурентних тендерах
        public double? winsConcurrentTendersSum { get; set; }// Сума перемог в конкурентних тендерах
        public int? winsChangedBidsTendersCount { get; set; }// К-ть перемог в тендерах зі зміною ставки
        public double? winsChangedBidsTendersSum { get; set; }// Сума перемог в тендерах зі зміною ставки
        public int? winsNotChangedBidsTendersCount { get; set; }// К-ть перемог в тендерах без зміни ставки
        public double? winsNotChangedBidsTendersSum { get; set; }// Сума перемог в тендерах без зміни ставки
        public int? winsCurConNonConCount { get; set; } // Сума перемог в неконкурентних тендерах без зміни ставки
        public double? winsCurConNonConSum { get; set; } // К-ть перемог в неконкурентних тендерах без зміни ставки
        public int? firstMinimalPriceWin { get; set; }// К-ть перемог на першій мінімальній ціні
        public int? firstMinimalPriceLose { get; set; }// К-ть поразок на першій мінімальній ціні
        public int? firstMinimalPriceWinPercent { get; set; }// Відсоток перемог на першій мінімальній ціні
        public int? firstMinimalPriceLosePercent { get; set; }// Відсоток поразок на першій мінімальній ціні
        public int? notChangedBidWin { get; set; }// К-ть перемог в тендерах без зміни ставок
        public int? notChangedBidLose { get; set; }// К-ть поразок в тендерах без зміни ставок
        public int? notChangedBidWinPercent { get; set; }// Відсоток перемог в тендерах без зміни ставок
        public int? notChangedBidLosePercent { get; set; }// Відсоток поразок в тендерах без зміни ставок
        public int? onLastBidWin { get; set; }// К-ть перемог в тендерах на останній ставці
        public int? onLastBidLose { get; set; }// К-ть поразок в тендерах на останній ставці
        public int? onLastBidWinPercent { get; set; }// Відсоток перемог в тендерах на останній ставці
        public int? onLastBidLosePercent { get; set; }// Відсоток поразок в тендерах на останній ставці
        public int? sumTenders { get; set; } // Загальна сумма тендерів
    }

    public class OrganizationAnalyticFinancialBKI // Фінансова аналітика (період рік)
    {
        public int year { get; set; }// Період (рік)
        public double main_active { get; set; }// Основні засоби
        public double main_active_percent { get; set; }// Основні засоби в порівняні з попереднім періодом
        public double current_liabilities { get; set; }// Поточні зобов'язання
        public double current_liabilities_percent { get; set; }// Поточні зобов'язання в порівняні з попереднім періодом
        public double net_income { get; set; }// Виручка
        public double net_income_percent { get; set; }// Виручка в порівняні з попереднім періодом
        public double net_profit { get; set; }// Дохід
        public double net_profit_percent { get; set; }// Дохід в порівняні з попереднім періодом
    }

    public class OrganizationAnalyticSanctions// Відомості про наявні санкції
    {
        public int? sanctionTypeId { get; set; }// Тип санкцій код (відп.до довідника № 11) 
        public bool isActive { get; set; }// Активні на даний момент (так - true / ні - false)
        public DateTime? sanctionStart { get; set; }// Дата початку
        public DateTime? sanctionEnd { get; set; }// Дата закінчення
    }

    public class OrganizationAnalyticExpressScore // Дані експрес перевірки 
    {
        public bool liquidation { get; set; } // В процесі ліквідації
        public bool bankruptcy { get; set; } // Відомості про банкрутство
        public int? badRelations { get; set; } // Зв’язки з компаніями банкрутами
        public int? sffiliatedMore { get; set; }// Афілійовані зв’язки
        public bool sanctions { get; set; }// Санкційні списки
        public long? introduction { get; set; } //Виконавчі впровадження
        public int? criminal { get; set; }// Кримінальні справи
        public bool audits { get; set; }// Включено в план-графік перевірок
        public bool canceledVat { get; set; }// Анульована реєстрація платника ПДВ
        public bool taxDebt { get; set; }// Податковий борг
        public decimal? wageArrears { get; set; }// Заборгованість по ЗП
        public bool kved { get; set; }// Зміна основного КВЕД
        public bool newDirector { get; set; }// Новий директор
        public int? massRegistration { get; set; }// Адреса масової реєстрації
        public bool youngCompany { get; set; }// Нова компанія
        public bool atoRegistered { get; set; }// Зареєстровано на окупованій території
        public bool fictitiousBusiness { get; set; }// Стаття 205 ККУ. Фіктивне підприємництво
        public int? headOtherCompanies { get; set; }// Керівник цієї компанії є керівником в інших компаніях
        public int? notLicense { get; set; }// Не діюча ліцензія
        public bool vatLessThree { get; set; }// Статус платника ПДВ не перевищує 3 місяців
        public int? sanctionsRelationships { get; set; }// Зв’язки з компаніями під санкціями
        public int? territoriesRelationships { get; set; }// Зв’язки з компаніями з окупованих територій
        public int? criminalRelationships { get; set; } // Зв’язки з компаніями, які мають кримінальні справи
    }

    public class OrganizationAnalyticEdata // Аналітика по Edata період (місяць)
    {
        public DateTime? period { get; set; }// Період (місяць)
        public int? count { get; set; }// Загальна кількість транзакцій 
        public double? paymentSumIn { get; set; }// Сума вхідних транзакцій 
        public double? paymentSumOnt { get; set; }// К-ть вхідних транзакцій 
        public long? paymentCountIn { get; set; }// Сума вихідних транзакцій 
        public long? paymentCountOnt { get; set; }// К-ть вихідних транзакцій 
    }

    public class OrganizationAnalyticBankruptcy // Публікації ВГСУ про банкрутство
    {
        public int? publicationType { get; set; } // Тип публікації (відповідно таблиці)
        public DateTime? dateProclamation { get; set; } // Дата публікації повідомлення
        public double? totalSumPossessions { get; set; } // Загальна сума виставленрого майна
    }

    public class OrganizationAnalyticEnforcements // Виконавчі проваждення
    {
        public DateTime? period { get; set; } // Період (місяць)
        public int? vidkrytoCreditor { get; set; } // Відкрито (компанія стягувач)
        public int? zavershenoCreditor { get; set; }//Завершено (компанія стягувач)
        public int? inshyyStanCreditor { get; set; }// Інший статус (компанія стягувач)
        public int? vidkrytoDebitor { get; set; }// Відкрито (компанія боржник)
        public int? zavershenoDebitor { get; set; }// Завершено (компанія боржник)
        public int? inshyyStanDebitor { get; set; }// Інший статус (компанія боржник)
    }

    public class OrganizationAnalytiCourtsAnalytics // Аналітика по судовим в розрізі місяця
    {
        public DateTime? period { get; set; }// Період (місяць)
        public int? documentsCount { get; set; }// Загальна кількість судових документів
        public int? tsyvilne { get; set; }// К-ть цивільних справ
        public int? kryminalne { get; set; }// К-ть кримінальних справ
        public int? hospodarske { get; set; }// К-ть господарських справ
        public int? administratyvne { get; set; }// К-ть адміністративних справ
        public int? admіnpravoporushennya { get; set; }// К-ть справ про адмін правопорушення
        public int? inshe { get; set; } // К-ть справ з невизначеним статусом
        public int? vidpovidachi { get; set; }// К-ть справ де сторона відповідач
        public int? pozyvachi { get; set; }// К-ть справ де сторона позивач
        public int? inshaStorona { get; set; }// К-ть справ де інша сторона
        public int? vyhrano { get; set; }// К-ть вигранних справ
        public int? prohrano { get; set; }// К-ть програнних справ
    }

    public class Founders // Аналітика по засновникам в розрізі країни
    {
        public int? countryId { get; set; } // Країна код (відп.до довідника № 8)
        public int? personCount { get; set; }// Кількість персон (з вказаної країни)
        public int? companyCount { get; set; }// Кількість компаній (з вказаної країни)
        public decimal? personCapitalSum { get; set; }// Сума статутного капіталу персон (з вказаної країни)	
        public decimal? companyCapitalSum { get; set; }// Сума статутного капіталу компаній (з вказаної країни)	
        public decimal? personCapitalPercent { get; set; }// %, статутного капіталу персон (з вказаної країни)	
        public decimal? companyCapitalPercent { get; set; }// %, статутного капіталу компаній (з вказаної країни)

    }

    public class TotalCourts // Загальна аналітика по судовим рішенням
    {
        public long? civil { get; set; }// К-ть цивільних справ
        public long? criminal { get; set; }// К-ть кримінальних справ
        public long? household { get; set; }// К-ть господарських справ
        public long? administrative { get; set; }// К-ть адміністративних справ
        public long? adminoffense { get; set; }// К-ть справ про адмін.правопорушення
        public long? plaintiff { get; set; }// К-ть справ в яких позивач
        public long? defendant { get; set; }// К-ть справ в яких відповідач
        public long? otherSide { get; set; }// К-ть справ в яких інша сторона
        public long? lost { get; set; }// К-ть справ в яких програв
        public long? win { get; set; }// К-ть справ в яких переміг
        public long? appointedConsideration { get; set; }// К-ть цивільних справ
        public long? totalDecision { get; set; }// К-ть документів
        public long? casecount { get; set; }// К-ть справ
        public long? inprocess { get; set; }// К-ть справ в процесі

    }
    public class Declarations // Аналітика по деклараціям  (період рік)
    {
        public int? declarationYearInt { get; set; }// Період (рік)
        public int? corporateRightsCount { get; set; }// К-ть осіб які мають корпоративні права
        public int? beneficiaryOwnersCount { get; set; }// К-ть осіб які є бенефіціарами
        public int? intangibleAssetsCount { get; set; }// К-ть осіб які мають нематериальні активи
        public int? incomefinanceCount { get; set; }// К-ть осіб які мають доходи, у тому числі подарунки
        public int? moneyAssetsCount { get; set; }// К-ть осіб які мають грошові активи
        public int? financialLiabilitiesCount { get; set; }// К-ть осіб які мають фінансові зобов'язання
        public int? membershipOrgCount { get; set; }// К-ть осіб які є членами в організаці
        public decimal? corporateRightsSum { get; set; }// Сума часток осіб які мають корпоративні права
        public decimal? beneficiaryOwnersSum { get; set; }// Сума часток осіб які є бенефіціарами
        public decimal? intangibleAssetsSum { get; set; }// Сума нематериальних активів
        public decimal? incomefinanceSum { get; set; }// Сума доходів, у тому числі подарунків
        public decimal? moneyAssetsSum { get; set; }// Сума грошових активів
        public decimal? financialLiabilitiesSum { get; set; } // Сума фінансових зобов'язань
        public decimal? membershipOrgSum { get; set; } // Членство в організаціях

    }

    public class Patents                                                                // Статистика по об`єктів інтелектуальної власності в розрізі місяця (патентам / ТМ)
    {
        // ADD
        public DateTime? period { get; set; }                                           // Період (помісячно)
        public int? patentsDesignsCount { get; set; }                                   // Промислові зразки
        public int? patentsCount { get; set; }                                          // Патенти
        public int? tradeMarkCount { get; set; }                                        // ТМ
        public int? usefulModelsCount { get; set; }                                     // Корисні моделі
        public int? integratedСircuitsCount { get; set; }                               // Топографії інтегральних мікросхем

    }
    public class Tenders // Аналіз тендерів в розрізі періоду (місяць)
    {
        public DateTime? periodDete { get; set; } // Період (місяць)
        public int? uchastCount { get; set; }// Кількість тендерів в яких брав участь
        public double? uchastSum { get; set; }// Сума тендерів в яких брав участь, грн.
        public int? peremohyCount { get; set; }// Кількість тендерів в яких переміг
        public double? peremohySum { get; set; }// Сума тендерів в яких переміг, грн.
        public int? peremohyConcurCount { get; set; }// Кількість перемог в конкурентних тендерах
        public double? peremohyConcurSum { get; set; }// Сума перемог в конкурентних тендерах, грн.
    }

    public class VatPayers // ПДВ 
    {
        public bool vatPayer { get; set; }// Платник | Не платник
        public DateTime? vatPayerDate { get; set; } // Дата отримання
        public bool vatPayerCancel { get; set; } // ПДВ анулюванно
        public DateTime? vatPayerCancelDate { get; set; }// Дата анулювання
    }

    public class Date // Дата створення / закрыття компанії / ФОП
    {
        public DateTime? dateOpened { get; set; }// Дата створення компанії / ФОП
        public DateTime? dateCanceled { get; set; } // Дата закрыття компанії / ФОП
    }

    public class State                                                                          // Стани організації
    {
        public string orgState { get; set; }                                                    // Стан організації (зареєстровано, припинено)(maxLength:64)
        public string bankruptcyState { get; set; }                                             // Чи перебуває в процесі банкрутства(maxLength:128)
        public string canceledState { get; set; }                                               // Чи перебуває в процесі припиненні(maxLength:128)
    }

    public class Ownership                                                                      // Організаційно-правова форма(ОПФ) та форма власності
    {
        public long? Id { get; set; }                                                           // Форма власності Id { 1, "Державна власність"},       { 2, "Комунальна власність"},  { 3, "Змішана власність (Державна частка перевищує 50 відсотків)"}, // Змішана власність (Державна частка перевищує 50 відсотків) { 4, "Недержавна власність"},
        public string form { get; set; }                                                        // Форма власності назва { 1, "Державна власність"},       { 2, "Комунальна власність"},  { 3, "Змішана власність (Державна частка перевищує 50 відсотків)"}, // Змішана власність (Державна частка перевищує 50 відсотків) { 4, "Недержавна власність"},
        public int? olf_code { get; set; }                                                      // ОПФ код
    }

    public class OrganizationLicensesElastic // Аналіз ліцензій (в розрізі органу ліцензування)
    {
        public int? organInt { get; set; }// Орган ліцензування код (відп.до довідника № 6)
        public int? typeOfLicenseInt { get; set; }// Тип ліцензії (відп.до довідника № 7)
        public long? actualCount { get; set; }// К-ть не актуальних ліцензій
        public long? nonActualCount { get; set; }// К-ть актуальних ліцензій
    }

    public class ChangeHistory// Історія реєстрацийних змін
    {
        public List<DateTime> changeAdress { get; set; } // Зміни адреси
        public List<DateTime> changeChief { get; set; }// Зміни керівників
        public List<DateTime> changeKved { get; set; }// Зміни кведів
        public List<DateTime> changeName { get; set; }// Зміни назв
        public List<DateTime> changeState { get; set; }// Зміни статуса
        // ADD
        public List<DateTime> changeFounder { get; set; }// Зміни бенефіціарів
    }

    public class DebtorsBorg // Динаміка податкового боргу (період місяць)
    {
        public DateTime? period { get; set; }// Період (місяць)
        public double db { get; set; }// Сума заборгованості перед державним бюджетом
        public double mb { get; set; }// Сума заборгованості перед місцевим бюджетом

        // ADD
        public double? zp { get; set; }// Сума заборгованості по заробітній платі
    }

    public class OrgChecks// Аналітика по перевіркам
    {
        public DateTime? checkDate { get; set; }// Дата події
        public bool? isPlanned { get; set; }// Запранована перевірка (так - true / ні - false)
        public int? regulatorId { get; set; }// Перевіряючий орган код (відп.до довідника № 4))
        public int? statusInt { get; set; }// Статус перевірки (відп.до довідника № 5)
        public string sanctionType { get; set; }                                                // Тип штрафних санкцій (maxLength:256)
        public double? sanctionAmount { get; set; }// Сума штрафу

    }


    public class FeaCountryGroup                                                                // Дані по ЗЄД в розрізі групи товарів (або країни) та року
    {
        public int? year { get; set; }                                                          // Рік

        public bool? isImport { get; set; }                                                     // Це імпорт? (true - так / false - ні)

        public int? code { get; set; }                                                          // Код країни / ВЕД

        public double? totalSum { get; set; }                                                   // Сумма

        public int? position { get; set; }                                                      // Позиція на ринку (за даний рік по даному коду)

        public double? persent { get; set; }                                                    // Відсоток ринку (за даний рік по даному коду)
    }

    public class SinglePaxPayer                                                                 // Відомості про єдиний податок
    {
        public int? group { get; set; }                                                         // Група ЄП
        public bool? singlePayer { get; set; }                                                  // Чи є платников єдиного податку (true - так / false - ні)
        public DateTime? singlePayerCancelDate { get; set; }                                    // Дата анулювання
        public DateTime? singlePayerDate { get; set; }                                          // Дата отримання свідоцтва ЄП
        public int? stavka { get; set; }                                                        // Ставка ЄП
    }


    public class LtStAnalyticsModel                             // Системна інформація (не використовується)
    {
        public int? year { get; set; }                              
        public bool? isBigForm { get; set; }
        public float? equity { get; set; }
        public float? ltStNetDebt { get; set; }
        public float? ltStDebtSales { get; set; }
    }

    public class OrganizationAnalyticKoatuInfo                  // Відомості про КОАТУУ 
    {
        public int? firstLevel { get; set; }                    // Розряди 1-2 – перший рівень класифікації. Включає в себе Автономну Республіку Крим , області України , міста зі спеціальним статусом ( Київ , Севастополь ).
        public int? secondLevel { get; set; }                   // Розряди 3-5 - другий рівень класифікації. Включає міста обласного значення, райони Автономної Республіки Крим та областей, райони в містах зі спеціальним статусом.
        public int? thirdLevel { get; set; }                    // Розряди 6-8 - третій рівень класифікації. Включає міста районного значення, райони в містах обласного значення, селища міського типу , сільські та селищні ради.
        public int? fourthLevel { get; set; }                   // Розряди 9-10 - четвертий рівень класифікації. Включає в себе села та селища.
        public long? fullKoatu { get; set; }                    // Повний КОАТУУ (приведений до формату цілого числа)
    }

    public class ImportMainBalanceIndicators                                // Основні показники балансу
    {
        public int period { get; set; }                                     // Рік
        public double? d108002UsohozarozdilomID1109502 { get; set; }        // I. Необоротні активи // Усього за розділом I // 1095
        public double? d126002UsohozarozdilomIID1119502 { get; set; }       // II. Оборотні активи // Усього за розділом II // 1195
        public double? d124002IIINeoborotniaktyvy { get; set; }             // III. Необоротні активи, утримувані для продажу, та групи вибуття // 1200
        public double? d128002BalansaktivuD1130002 { get; set; }            // Баланс // 1300
        public double? d138002UsohozarozdilomID1149502 { get; set; }        // I. Власний капітал // Усього за розділом I // 1495
        public double? d148002UsohozarozdilomIID1159502 { get; set; }       // II. Довгострокові зобов'язання і забезпечення // Усього за розділом II // 1595
        public double? d162002UsohozarozdilomIIID1169502 { get; set; }      // IІІ. Поточні зобов'язання і забезпечення // Усього за розділом IІІ // 1695
        public double? d1170002D141802IVZobovyazanny { get; set; }          // ІV. Зобов'язання, пов'язані з необоротними активами, утримуваними для продажу, та групами вибуття // 1700
        public double? d203501Chystyydokhid { get; set; }                   // Чистий дохід від реалізації продукції (товарів, робіт, послуг) // 2000
        public double? d210001Finansovyyrezul { get; set; }                 // Фінансовий результат від операційної діяльності: прибуток // 2190
        public double? d210501FinRez { get; set; }                          // Фінансовий результат від операційної діяльності: збиток // 2195
        public double? d217001FinRezdoopodatkuvannyaprybutok { get; set; }  // Фінансовий результат до оподаткування: // прибуток // 2290 
        public double? d217501FinRezdoopodatkuvannyazbytok { get; set; }    // Фінансовий результат до оподаткування: // збиток // 2295
        public double? d222001ChystyyFinRezprybutokD2235001 { get; set; }   // Чистий фінансовий результат: прибуток // 2350
        public double? d222501ChystyyFinRezzbytokD2235501 { get; set; }     // Чистий фінансовий результат: збиток // 2355


        public double? d207001RazomdokhodyD2228001 { get; set; }            // Разом доходи (малі підприємства)
        public double? d212001RazomvytratyD2228501 { get; set; }            // Разом витрати (малі підприємства)

        public int forma { get; set; }                                      // Форма звітності (1 - велика / 2- мала)
    }
}
