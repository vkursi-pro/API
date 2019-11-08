using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.organizations
{
    public class GetAnalyticClass
    {
        // 9.	Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
        // [POST] /api/1.0/organizations/getanalytic

        public static GetAnalyticResponseModel GetAnalytic(string code, string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = AuthorizeClass.Authorize();
            }
        
            link1:
            
            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/organizations/getanalytic", Method.POST);

            // Example1: {"code":"00131305"};
            // Example2: "00131305";

            string body = "\"" + code + "\"";
            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "Not found")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            GetAnalyticResponseModel organizationAnalytic = JsonConvert.DeserializeObject<GetAnalyticResponseModel>(responseString);

            return organizationAnalytic;
        }
    }

    public class GetAnalyticResponseModel
    {
        public string orgId { get; set; }
        public string name { get; set; }
        public string clearName { get; set; }
        public List<string> namePrevious { get; set; }
        public bool legalEntity { get; set; }
        public string edrpou { get; set; }
        public int? stateInt { get; set; }
        public State state { get; set; }
        public double totalAmount { get; set; }
        public string chiefCurrent { get; set; }
        public VatPayers vatPayers { get; set; }
        public List<Tenders> tenders { get; set; }
        public List<Patents> patents { get; set; }
        public List<Declarations> declarations { get; set; }
        public Date date { get; set; }
        public TotalCourts totalCourts { get; set; }

        public List<OrganizationAnalytiCourtsAnalytics> courtsAnalytics { get; set; }
        public List<OrganizationAnalytiCourtsAnalytics> courtsAssignedAnalytics { get; set; }
        public List<OrganizationAnalyticEnforcements> enforcements { get; set; }
        public OrganizationAnalyticEnforcementsStatistic enforcementsStatistic { get; set; }
        public List<OrganizationAnalyticBankruptcy> bankruptcy { get; set; }
        public List<OrganizationAnalyticEdata> edata { get; set; }
        public List<OrgChecks> orgChecks { get; set; }

        public List<DebtorsBorg> debtorsBorg { get; set; }
        public List<string> chiefPrevious { get; set; }
        public ChangeHistory changeHistory { get; set; }
        public List<OrganizationLicensesElastic> organizationLicenses { get; set; }
        public List<OrganizationAnalyticRealEstateRights> realEstateRights { get; set; }
        public OrganizationAnalyticExpressScore expressScore { get; set; }
        public List<OrganizationAnalyticSanctions> sanctions { get; set; }
        public List<string> kveds { get; set; }
        public List<int> kvedsInt { get; set; }
        public List<Regions> regions { get; set; }
        public List<Ownership> ownership { get; set; }
        public List<Founders> founders { get; set; }
        public TenderStatistic tenderStatistic { get; set; }
        public List<OrganizationAnalyticFinancialBKI> financialBKI { get; set; }

        public OrganizationAnalyticTenderBidStatistics tenderBidStatistics { get; set; }
        public OrganizationAnalyticTenderOrganizerStatistics tenderOrganizerStatistics { get; set; }

        public List<OrganizationAnalyticTenderBidStatisticsTenderCpvStats> tenderCpvStats { get; set; }
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand realEstateRightsLand { get; set; }
        public OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj realEstateRightsObj { get; set; }
        public List<OrganizationAnalyticOrganizationFEAModel> financialFEA { get; set; }
        public List<OrganizationAnalyticFinancialRisks> financialRisks { get; set; }

        public List<OrganizationAnalyticEmployeesModel> employees { get; set; }

    }


    public class OrganizationAnalyticEmployeesModel
    {
        public int? year { get; set; }
        public int? count { get; set; }
        public int? differentPrevCount { get; set; }
    }


    public class OrganizationAnalyticFinancialRisks
    {
        public int? year { get; set; }
        public int? kvedGroupNumb { get; set; }
        public int? debtClass { get; set; }
        public int? metricsCount { get; set; }
        public int? risksFirstInt { get; set; }
        public int? risksSecondInt { get; set; }
        public int? risksThirdInt { get; set; }
        public int? risksCategoryInt { get; set; }
    }

    public class OrganizationAnalyticOrganizationFEAModel
    {
        public int? year { get; set; }
        public bool? isImport { get; set; }
        public int? operationsCount { get; set; }
        public double? operationsSum { get; set; }
        public double? groupPersent { get; set; }
        public int? groupCount { get; set; }
        public int? largestGroupCode { get; set; }
        public int? largestCountryCode { get; set; }
        public int? countryCount { get; set; }
        public decimal? countryLargestPersent { get; set; }
    }


    public class OrganizationAnalyticEnforcementsStatistic
    {
        public long? govermentCountDebtor { get; set; }
        public long? personCountDebtor { get; set; }
        public long? orgCountDebtor { get; set; }
        public long? anotherCountDebtor { get; set; }
        public long? govermentCountCreditor { get; set; }
        public long? personCountCreditor { get; set; }
        public long? orgCountCreditor { get; set; }
        public long? anotherCountCreditor { get; set; }
    }


    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsObj
    {
        public int? full { get; set; }
        public int? vlasnyk { get; set; }
        public int? pravonabuvach { get; set; }
        public int? pravokorystuvach { get; set; }
        public int? zemlevlasnyk { get; set; }
        public int? zemlevolodilets { get; set; }
        public int? inshyy { get; set; }
        public int? naymach { get; set; }
        public int? orendar { get; set; }
        public int? naymodavets { get; set; }
        public int? orendodavets { get; set; }
        public int? upravytel { get; set; }
        public int? vyhodonabuvach { get; set; }
        public int? ustanovnyk { get; set; }
        public int? ipotekoderzhatel { get; set; }
        public int? maynovyyPoruchytel { get; set; }
        public int? ipotekodavets { get; set; }
        public int? borzhnyk { get; set; }
        public int? obtyazhuvach { get; set; }
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; }
        public int? osobaVinteresakhYakoyi { get; set; }
        public bool? registerError { get; set; }
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; }
    }

    public class OrganizationAnalyticTenderBidStatisticsRealEstateRightsLand
    {
        public int? full { get; set; }
        public int? vlasnyk { get; set; }
        public int? pravonabuvach { get; set; }
        public int? pravokorystuvach { get; set; }
        public int? zemlevlasnyk { get; set; }
        public int? zemlevolodilets { get; set; }
        public int? inshyy { get; set; }
        public int? naymach { get; set; }
        public int? orendar { get; set; }
        public int? naymodavets { get; set; }
        public int? orendodavets { get; set; }
        public int? upravytel { get; set; }
        public int? vyhodonabuvach { get; set; }
        public int? ustanovnyk { get; set; }
        public int? ipotekoderzhatel { get; set; }
        public int? maynovyyPoruchytel { get; set; }
        public int? ipotekodavets { get; set; }
        public int? borzhnyk { get; set; }
        public int? obtyazhuvach { get; set; }
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; }
        public int? osobaVinteresakhYakoyi { get; set; }
        public double? ploshcha { get; set; }
        public double? ploshchaObroblena { get; set; }
        public List<OrganizationAnalyticRegionsEstate> regions { get; set; }
        public List<OrganizationAnalyticPurposes> purposes { get; set; }
        public bool? registerError { get; set; }
    }

    public class OrganizationAnalyticPurposes
    {
        public int? purposeCode { get; set; }
        public int? objectCount { get; set; }
        public double? area { get; set; }
        public int? courtCount { get; set; }
    }

    public class OrganizationAnalyticRegionsEstate
    {
        public int? regionId { get; set; }
        public int? objectCount { get; set; }
        public double? area { get; set; }
        public int? courtCount { get; set; }
    }

    public class OrganizationAnalyticTenderBidStatisticsTenderCpvStats
    {
        public int? year { get; set; }
        public long? cpv { get; set; }
        public int? bidCount { get; set; }
        public double? bidSum { get; set; }
        public int? awardCount { get; set; }
        public double? awardSum { get; set; }
        public int? organizerCount { get; set; }
        public double? organizerSum { get; set; }
    }

    public class OrganizationAnalyticTenderOrganizerStatistics
    {
        public int? announcedTenders { get; set; }
        public double? sum { get; set; }
        public double? averageLag { get; set; }
        public int? participants { get; set; }
        public double? concurrency { get; set; }
        public double? avgContracts { get; set; }
        public int? currentTendersCount { get; set; }
        public double? currentTendersSum { get; set; }
        public int? nonConcurrentTendersCount { get; set; }
        public double? nonConcurrentTendersSum { get; set; }
        public int? concurrentTendersCount { get; set; }
        public double? concurrentTendersSum { get; set; }
        public int? changedBidsTendersCount { get; set; }
        public double? changedBidsTendersSum { get; set; }
        public int? notChangedBidsTendersCount { get; set; }
        public double? notChangedBidsTendersSum { get; set; }
        public int? curConNonConCount { get; set; }
        public double? curConNonConSum { get; set; }
        public double? priceReduction { get; set; }
        public double? priceIncrease { get; set; }
        public double? shippingReduction { get; set; }
        public double? other { get; set; }
        public int? sumTenders { get; set; }
    }

    public class OrganizationAnalyticTenderBidStatistics
    {
        public int? disqualified { get; set; }
        public double? avgConcurrency { get; set; }
        public double? sum { get; set; }
        public int? wins { get; set; }
        public int? organizators { get; set; }
        public int? requests { get; set; }
        public int? currentTendersCount { get; set; }
        public double? currentTendersSum { get; set; }
        public int? nonConcurrentTendersCount { get; set; }
        public double? nonConcurrentTendersSum { get; set; }
        public int? concurrentTendersCount { get; set; }
        public double? concurrentTendersSum { get; set; }
        public int? changedBidsTendersCount { get; set; }
        public double? changedBidsTendersSum { get; set; }
        public int? notChangedBidsTendersCount { get; set; }
        public double? notChangedBidsTendersSum { get; set; }
        public int? curConNonConCount { get; set; }
        public double? curConNonConSum { get; set; }
        public int? winsNonConcurrentTendersCount { get; set; }
        public double? winsNonConcurrentTendersSum { get; set; }
        public int? winsConcurrentTendersCount { get; set; }
        public double? winsConcurrentTendersSum { get; set; }
        public int? winsChangedBidsTendersCount { get; set; }
        public double? winsChangedBidsTendersSum { get; set; }
        public int? winsNotChangedBidsTendersCount { get; set; }
        public double? winsNotChangedBidsTendersSum { get; set; }
        public int? winsCurConNonConCount { get; set; }
        public double? winsCurConNonConSum { get; set; }
        public int? firstMinimalPriceWin { get; set; }
        public int? firstMinimalPriceLose { get; set; }
        public int? firstMinimalPriceWinPercent { get; set; }
        public int? firstMinimalPriceLosePercent { get; set; }
        public int? notChangedBidWin { get; set; }
        public int? notChangedBidLose { get; set; }
        public int? notChangedBidWinPercent { get; set; }
        public int? notChangedBidLosePercent { get; set; }
        public int? onLastBidWin { get; set; }
        public int? onLastBidLose { get; set; }
        public int? onLastBidWinPercent { get; set; }
        public int? onLastBidLosePercent { get; set; }
        public int? sumTenders { get; set; }
    }

    public class OrganizationAnalyticFinancialBKI
    {
        public int year { get; set; }
        public double main_active { get; set; }
        public double main_active_percent { get; set; }
        public double current_liabilities { get; set; }
        public double current_liabilities_percent { get; set; }
        public double net_income { get; set; }
        public double net_income_percent { get; set; }
        public double net_profit { get; set; }
        public double net_profit_percent { get; set; }
    }

    public class OrganizationAnalyticSanctions
    {
        public int? sanctionTypeId { get; set; }
        public bool isActive { get; set; }
        public DateTime? sanctionStart { get; set; }
        public DateTime? sanctionEnd { get; set; }
    }

    public class OrganizationAnalyticExpressScore
    {
        public bool liquidation { get; set; }
        public bool bankruptcy { get; set; }
        public int? badRelations { get; set; }
        public int? sffiliatedMore { get; set; }
        public bool sanctions { get; set; }
        public long? introduction { get; set; }
        public int? criminal { get; set; }
        public bool audits { get; set; }
        public bool canceledVat { get; set; }
        public bool taxDebt { get; set; }
        public decimal? wageArrears { get; set; }
        public bool kved { get; set; }
        public bool newDirector { get; set; }
        public int? massRegistration { get; set; }
        public bool youngCompany { get; set; }
        public bool atoRegistered { get; set; }
        public bool fictitiousBusiness { get; set; }
        public int? headOtherCompanies { get; set; }
        public int? notLicense { get; set; }
        public bool vatLessThree { get; set; }
        public int? sanctionsRelationships { get; set; }
        public int? territoriesRelationships { get; set; }
        public int? criminalRelationships { get; set; }
    }

    public class OrganizationAnalyticRealEstateRights
    {
        public int? dcGroupType { get; set; }
        public int? full { get; set; }
        public int? vlasnyk { get; set; }
        public int? pravonabuvach { get; set; }
        public int? pravokorystuvach { get; set; }
        public int? zemlevlasnyk { get; set; }
        public int? zemlevolodilets { get; set; }
        public int? inshyy { get; set; }
        public int? naymach { get; set; }
        public int? orendar { get; set; }
        public int? naymodavets { get; set; }
        public int? orendodavets { get; set; }
        public int? upravytel { get; set; }
        public int? vyhodonabuvach { get; set; }
        public int? ustanovnyk { get; set; }
        public int? ipotekoderzhatel { get; set; }
        public int? maynovyyPoruchytel { get; set; }
        public int? ipotekodavets { get; set; }
        public int? borzhnyk { get; set; }
        public int? obtyazhuvach { get; set; }
        public int? osobaMaynoYakoyiObtyazhuyutsya { get; set; }
        public int? osobaVinteresakhYakoyi { get; set; }
    }

    public class OrganizationAnalyticEdata
    {
        public DateTime? period { get; set; }
        public int? count { get; set; }
        public double? paymentSumIn { get; set; }
        public double? paymentSumOnt { get; set; }
        public long? paymentCountIn { get; set; }
        public long? paymentCountOnt { get; set; }
    }

    public class OrganizationAnalyticBankruptcy
    {
        public int? publicationType { get; set; }
        public DateTime? dateProclamation { get; set; }
        public double? totalSumPossessions { get; set; }
    }

    public class OrganizationAnalyticEnforcements
    {
        public DateTime? period { get; set; }
        public int? vidkrytoCreditor { get; set; }
        public int? zavershenoCreditor { get; set; }
        public int? inshyyStanCreditor { get; set; }
        public int? vidkrytoDebitor { get; set; }
        public int? zavershenoDebitor { get; set; }
        public int? inshyyStanDebitor { get; set; }
    }

    public class OrganizationAnalytiCourtsAnalytics
    {
        public DateTime? period { get; set; }
        public int? documentsCount { get; set; }
        public int? tsyvilne { get; set; }
        public int? kryminalne { get; set; }
        public int? hospodarske { get; set; }
        public int? administratyvne { get; set; }
        public int? admіnpravoporushennya { get; set; }
        public int? inshe { get; set; }
        public int? vidpovidachi { get; set; }
        public int? pozyvachi { get; set; }
        public int? inshaStorona { get; set; }
        public int? vyhrano { get; set; }
        public int? prohrano { get; set; }
    }

    public class Founders
    {
        public string name { get; set; }
        public double amount { get; set; }
        public bool person { get; set; }
        public string nationality { get; set; }

        //ADD 
        public int? countryId { get; set; }
        public int? personCount { get; set; }
        public int? companyCount { get; set; }
        public decimal? personCapitalSum { get; set; }
        public decimal? companyCapitalSum { get; set; }
        public decimal? personCapitalPercent { get; set; }
        public decimal? companyCapitalPercent { get; set; }

    }

    public class TotalCourts
    {
        public long? civil { get; set; }
        public long? criminal { get; set; }
        public long? household { get; set; }
        public long? administrative { get; set; }
        public long? adminoffense { get; set; }
        public long? plaintiff { get; set; }
        public long? defendant { get; set; }
        public long? otherSide { get; set; }
        public long? lost { get; set; }
        public long? win { get; set; }
        public long? appointedConsideration { get; set; }
        public long? totalDecision { get; set; }
        public long? casecount { get; set; }
        public long? inprocess { get; set; }

    }

    public class TenderStatistic
    {
        public long? applicationsForParticipation { get; set; }
        public long? avarage { get; set; }
        public long? declaration { get; set; }
        public long? inProcess { get; set; }
        public long? lost { get; set; }
        public long? summ { get; set; }
        public long? win { get; set; }
    }

    public class Declarations
    {
        public string declarationId { get; set; }
        public string declarationYear { get; set; }
        public string personType { get; set; }
        public string relationshipType { get; set; }
        public string objectType { get; set; }
        public string workPost { get; set; }
        public string cost { get; set; }
        public string assetsCurrency { get; set; }

        // ADD 
        public int? declarationYearInt { get; set; }
        public int? corporateRightsCount { get; set; }
        public int? beneficiaryOwnersCount { get; set; }
        public int? intangibleAssetsCount { get; set; }
        public int? incomefinanceCount { get; set; }
        public int? moneyAssetsCount { get; set; }
        public int? financialLiabilitiesCount { get; set; }
        public int? membershipOrgCount { get; set; }
        public decimal? corporateRightsSum { get; set; }
        public decimal? beneficiaryOwnersSum { get; set; }
        public decimal? intangibleAssetsSum { get; set; }
        public decimal? incomefinanceSum { get; set; }
        public decimal? moneyAssetsSum { get; set; }
        public decimal? financialLiabilitiesSum { get; set; }
        public decimal? membershipOrgSum { get; set; }

    }

    public class Patents
    {
        public string name { get; set; }
        public string type { get; set; }
        public DateTime? dateReg { get; set; }
        public DateTime? dateAnul { get; set; }
        public List<string> mktp { get; set; }


        // ADD
        public DateTime? period { get; set; }
        public int? patentsDesignsCount { get; set; }
        public int? patentsCount { get; set; }
        public int? tradeMarkCount { get; set; }
        public int? usefulModelsCount { get; set; }
        public int? integratedСircuitsCount { get; set; }

    }
    public class Tenders
    {
        public string period { get; set; }
        public double summ { get; set; }

        // ADD 
        public DateTime? periodDete { get; set; }
        public int? uchastCount { get; set; }
        public double? uchastSum { get; set; }
        public int? peremohyCount { get; set; }
        public double? peremohySum { get; set; }
        public int? peremohyConcurCount { get; set; }
        public double? peremohyConcurSum { get; set; }
    }

    public class VatPayers
    {
        public bool vatPayer { get; set; }
        public DateTime? vatPayerDate { get; set; }
        public bool vatPayerCancel { get; set; }
        public DateTime? vatPayerCancelDate { get; set; }
    }

    public class Date
    {
        public DateTime? dateOpened { get; set; }
        public DateTime? dateCanceled { get; set; }
    }

    public class State
    {
        public string orgState { get; set; }
        public string bankruptcyState { get; set; }
        public string canceledState { get; set; }
    }

    public class Regions
    {
        public string code { get; set; }
        public string country { get; set; }
        public string countryId { get; set; }
        public string region { get; set; }
        public string regionId { get; set; }
        public string district { get; set; }
        public string districtId { get; set; }
        public string city { get; set; }
        public string cityId { get; set; }
        public string street { get; set; }
        public string house { get; set; }
        public string room { get; set; }
    }

    public class Ownership
    {
        public long Id { get; set; }
        public string form { get; set; }
        public int? olf_code { get; set; }
    }

    public class OrganizationLicensesElastic
    {
        public bool actual { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public Info info { get; set; }
        public string statusOfLicense { get; set; }
        public string typeOfLicense { get; set; }

        public int? organInt { get; set; }
        public int? typeOfLicenseInt { get; set; }


        public long? actualCount { get; set; }
        public long? nonActualCount { get; set; }


    }

    public class Info
    {
        public string part1 { get; set; }
        public string part2 { get; set; }
        public string part3 { get; set; }
        public string part4 { get; set; }
        public string name_part1 { get; set; }
        public string name_part2 { get; set; }
        public string name_part3 { get; set; }
        public string name_part4 { get; set; }
    }

    public class ChangeHistory
    {
        public List<DateTime> changeAdress { get; set; }
        public List<DateTime> changeChief { get; set; }
        public List<DateTime> changeKved { get; set; }
        public List<DateTime> changeName { get; set; }
        public List<DateTime> changeState { get; set; }
        // ADD
        public List<DateTime> changeFounder { get; set; }
    }

    public class DebtorsBorg
    {
        public DateTime? period { get; set; }
        public double db { get; set; }
        public double mb { get; set; }

        // ADD
        public double? zp { get; set; }
    }

    public class OrgChecks
    {
        public string organ { get; set; }
        public string activityType { get; set; }
        public DateTime? checkDate { get; set; }
        public DateTime? update { get; set; }

        // ADD 
        public bool? isPlanned { get; set; }
        public int? regulatorId { get; set; }
        public int? statusInt { get; set; }
        public string sanctionType { get; set; }
        public double? sanctionAmount { get; set; }

    }
}
