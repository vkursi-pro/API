using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetOrgFinanceClass
    {
        /*

        57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
        [POST] api/1.0/organizations/GetOrgFinance

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":["00131305"]}'

        */

        public static GetOrgFinanceResponseModel GetOrgFinance(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance");
                RestRequest request = new RestRequest(Method.POST);

                GetOrgFinanceRequestBodyModel GOFRequesRow = new GetOrgFinanceRequestBodyModel
                {
                    Code = new List<string> {
                        edrpou
                    }
                };

                string body = JsonConvert.SerializeObject(GOFRequesRow); // Example: {"Code":["00131305"]}

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

            GetOrgFinanceResponseModel GOFResponseRow = new GetOrgFinanceResponseModel();

            GOFResponseRow = JsonConvert.DeserializeObject<GetOrgFinanceResponseModel>(responseString);

            return GOFResponseRow;
        }
    }
    /*

        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Code\":[\"41462280\"]}"
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ik...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetOrgFinance", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

        
        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":[\"41462280\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinance")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVC...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class GetOrgFinanceRequestBodyModel      // 
    {/// <summary>
     /// Перелік кодів ЄДРПОУ (обмеження 1)
     /// </summary>
        public List<string> Code { get; set; }      // 
    }
    /// <summary>
    /// Відповідь на запит
    /// </summary>
    public class GetOrgFinanceResponseModel         // 
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }


    /// <summary>
    /// Модель фінансового балансу організації з API (основні активи, пасиви, тендери, імпорт/експорт)
    /// </summary>
    public class FinancialBalanceApiModel
    {
        /// <summary>Дата та час формування запиту фінансових даних</summary>
        [JsonProperty("date_of_request")]
        [JsonPropertyName("date_of_request")]
        public DateTimeOffset DateOfRequest { get; set; }

        /// <summary>Код ЄДРПОУ організації</summary>
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public long? Name { get; set; }

        /// <summary>Вік організації з моменту державної реєстрації (масив може містити кілька записів)</summary>
        [JsonProperty("age")]
        [JsonPropertyName("age")]
        public Age[] Age { get; set; }

        /// <summary>Повна назва організації</summary>
        [JsonProperty("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>Показники ліквідності організації (динамічна структура залежно від типу)</summary>
        [JsonProperty("likvid")]
        [JsonPropertyName("likvid")]
        public object Likvid { get; set; }

        /// <summary>Динаміка необоротних активів по роках (рядок 1090 форми 1)</summary>
        [JsonProperty("main_active")]
        [JsonPropertyName("main_active")]
        public Active[] MainActive { get; set; }

        /// <summary>Динаміка загальних активів (валюта балансу) по роках (рядок 1300 форми 1)</summary>
        [JsonProperty("actives")]
        [JsonPropertyName("actives")]
        public Active[] Actives { get; set; }

        /// <summary>Динаміка поточних зобов'язань по роках (рядок 1690 форми 1)</summary>
        [JsonProperty("current_liabilities")]
        [JsonPropertyName("current_liabilities")]
        public Active[] CurrentLiabilities { get; set; }

        /// <summary>Динаміка чистого доходу від реалізації по роках (рядок 2000 форми 2)</summary>
        [JsonProperty("net_income")]
        [JsonPropertyName("net_income")]
        public Active[] NetIncome { get; set; }

        /// <summary>Динаміка чистого фінансового результату (прибуток/збиток) по роках (рядок 2350 форми 2)</summary>
        [JsonProperty("net_profit")]
        [JsonPropertyName("net_profit")]
        public Active[] NetProfit { get; set; }

        /// <summary>Баланс за спрощеною формою звітності (для мікро- та малих підприємств)</summary>
        [JsonProperty("balance_small")]
        [JsonPropertyName("balance_small")]
        public Balance[] BalanceSmall { get; set; }

        /// <summary>Баланс за стандартною (повною) формою звітності</summary>
        [JsonProperty("balance_normal")]
        [JsonPropertyName("balance_normal")]
        public Balance[] BalanceNormal { get; set; }

        /// <summary>Кількість участей у тендерах (організація як постачальник) по роках</summary>
        [JsonProperty("tend_num")]
        [JsonPropertyName("tend_num")]
        public ExpGroupNum[] TendNum { get; set; }

        /// <summary>Загальна сума виграних тендерів (організація як постачальник) по роках</summary>
        [JsonProperty("tend_sum")]
        [JsonPropertyName("tend_sum")]
        public Active[] TendSum { get; set; }

        /// <summary>Кількість тендерів, де організація була єдиним учасником, по роках</summary>
        [JsonProperty("tend_num_solo")]
        [JsonPropertyName("tend_num_solo")]
        public ExpGroupNum[] TendNumSolo { get; set; }

        /// <summary>Відсоток тендерів із єдиним учасником від загальної кількості по роках</summary>
        [JsonProperty("tend_percent_solo")]
        [JsonPropertyName("tend_percent_solo")]
        public ExpBiggestGroupPercent[] TendPercentSolo { get; set; }

        /// <summary>Кількість проведених закупівель (організація як замовник) по роках</summary>
        [JsonProperty("tend_zamov_num")]
        [JsonPropertyName("tend_zamov_num")]
        public ExpGroupNum[] TendZamovNum { get; set; }

        /// <summary>Загальна сума проведених закупівель (організація як замовник) по роках</summary>
        [JsonProperty("tend_zamov_sum")]
        [JsonPropertyName("tend_zamov_sum")]
        public ExpGroupNum[] TendZamovSum { get; set; }

        /// <summary>Кількість закупівель із єдиним учасником (організація як замовник) по роках</summary>
        [JsonProperty("tend_zamov_num_solo")]
        [JsonPropertyName("tend_zamov_num_solo")]
        public ExpGroupNum[] TendZamovNumSolo { get; set; }

        /// <summary>Відсоток закупівель з єдиним учасником від загальної кількості (організація як замовник) по роках</summary>
        [JsonProperty("tend_zamov_percent_solo")]
        [JsonPropertyName("tend_zamov_percent_solo")]
        public ExpBiggestGroupPercent[] TendZamovPercentSolo { get; set; }

        /// <summary>Кількість експортних операцій по роках</summary>
        [JsonProperty("exp_num")]
        [JsonPropertyName("exp_num")]
        public ExpGroupNum[] ExpNum { get; set; }

        /// <summary>Загальна сума експорту по роках (USD)</summary>
        [JsonProperty("exp_sum")]
        [JsonPropertyName("exp_sum")]
        public Active[] ExpSum { get; set; }

        /// <summary>Кількість товарних груп УКТ ЗЕД в експорті по роках</summary>
        [JsonProperty("exp_group_num")]
        [JsonPropertyName("exp_group_num")]
        public ExpGroupNum[] ExpGroupNum { get; set; }

        /// <summary>Частка найбільшої товарної групи в загальному обсязі експорту по роках (%)</summary>
        [JsonProperty("exp_biggest_group_percent")]
        [JsonPropertyName("exp_biggest_group_percent")]
        public ExpBiggestGroupPercent[] ExpBiggestGroupPercent { get; set; }

        /// <summary>Кількість імпортних операцій по роках</summary>
        [JsonProperty("imp_num")]
        [JsonPropertyName("imp_num")]
        public ExpGroupNum[] ImpNum { get; set; }

        /// <summary>Загальна сума імпорту по роках (USD)</summary>
        [JsonProperty("imp_sum")]
        [JsonPropertyName("imp_sum")]
        public Active[] ImpSum { get; set; }

        /// <summary>Кількість товарних груп УКТ ЗЕД в імпорті по роках</summary>
        [JsonProperty("imp_group_num")]
        [JsonPropertyName("imp_group_num")]
        public ExpGroupNum[] ImpGroupNum { get; set; }

        /// <summary>Частка найбільшої товарної групи в загальному обсязі імпорту по роках (%)</summary>
        [JsonProperty("imp_biggest_group_percent")]
        [JsonPropertyName("imp_biggest_group_percent")]
        public ExpBiggestGroupPercent[] ImpBiggestGroupPercent { get; set; }
    }

    /// <summary>
    /// Фінансовий показник за рік (сума)
    /// </summary>
    public class Active
    {
        /// <summary>Звітний рік</summary>
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public long? Year { get; set; }

        /// <summary>Значення показника за рік (тис. грн або одиниці залежно від поля)</summary>
        [JsonProperty("sum")]
        [JsonPropertyName("sum")]
        public double? Sum { get; set; } = 0;
    }

    /// <summary>
    /// Вік організації (текстове представлення, роки, місяці)
    /// </summary>
    public partial class Age
    {
        /// <summary>Вік організації у текстовому форматі, напр. «5 років 3 місяці»</summary>
        [JsonProperty("age_text")]
        [JsonPropertyName("age_text")]
        public string AgeText { get; set; }

        /// <summary>Кількість повних років з моменту державної реєстрації</summary>
        [JsonProperty("age_year")]
        [JsonPropertyName("age_year")]
        public long? AgeYear { get; set; }

        /// <summary>Кількість місяців понад повні роки з моменту реєстрації</summary>
        [JsonProperty("age_month")]
        [JsonPropertyName("age_month")]
        public long? AgeMonth { get; set; }
    }

    /// <summary>
    /// Відсоток найбільшої групи експорту/імпорту або тендерів за рік
    /// </summary>
    public class ExpBiggestGroupPercent
    {
        /// <summary>Звітний рік</summary>
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public long? Year { get; set; }

        /// <summary>Відсоток найбільшої групи від загального обсягу</summary>
        [JsonProperty("perc")]
        [JsonPropertyName("perc")]
        public double? Perc { get; set; }
    }

    /// <summary>
    /// Кількість груп експорту/імпорту або тендерів за рік
    /// </summary>
    public class ExpGroupNum
    {
        /// <summary>Звітний рік</summary>
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public long? Year { get; set; }

        /// <summary>Кількість (груп, операцій або тендерів) за рік</summary>
        [JsonProperty("num")]
        [JsonPropertyName("num")]
        public long? Num { get; set; }
    }

    /// <summary>
    /// Рядки фінансової звітності організації (форма 1 — Баланс, форма 2 — Звіт про фінансові результати).
    /// D1_XXXX — форма 1 (Баланс): актив (1000–1300) та пасив (1400–1900).
    /// D2_XXXX — форма 2 (Звіт про фінансові результати).
    /// Суфікс _01 — на кінець звітного року; _02 — на початок звітного року (порівняльний попередній рік).
    /// Усі суми у тисячах гривень, якщо не зазначено інше.
    /// </summary>
    public class Balance
    {
        /// <summary>Звітний рік</summary>
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public long? Year { get; set; }

        /// <summary>Код ЄДРПОУ організації</summary>
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public long? Code { get; set; }

        // =====================================================================
        // ФОРМА 1 — БАЛАНС (D1)
        // =====================================================================
        // ==================== АКТИВ ====================
        // ========== Розділ I. Необоротні активи ==========

        /// <summary>Нематеріальні активи — залишкова (балансова) вартість, на кінець звітного року [рядок 1000]</summary>
        [JsonProperty("D1_1000_01")]
        [JsonPropertyName("D1_1000_01")]
        public double? D11000_01 { get; set; }

        /// <summary>Нематеріальні активи — залишкова вартість, на початок звітного року [рядок 1000]</summary>
        [JsonProperty("D1_1000_02")]
        [JsonPropertyName("D1_1000_02")]
        public double? D11000_02 { get; set; }

        /// <summary>Капіталізовані витрати на розробки — залишкова вартість, на кінець звітного року [рядок 1001]</summary>
        [JsonProperty("D1_1001_01")]
        [JsonPropertyName("D1_1001_01")]
        public double? D11001_01 { get; set; }

        /// <summary>Капіталізовані витрати на розробки — залишкова вартість, на початок звітного року [рядок 1001]</summary>
        [JsonProperty("D1_1001_02")]
        [JsonPropertyName("D1_1001_02")]
        public double? D11001_02 { get; set; }

        /// <summary>Концесійні нематеріальні активи — залишкова вартість, на кінець звітного року [рядок 1002]</summary>
        [JsonProperty("D1_1002_01")]
        [JsonPropertyName("D1_1002_01")]
        public double? D11002_01 { get; set; }

        /// <summary>Концесійні нематеріальні активи — залишкова вартість, на початок звітного року [рядок 1002]</summary>
        [JsonProperty("D1_1002_02")]
        [JsonPropertyName("D1_1002_02")]
        public double? D11002_02 { get; set; }

        /// <summary>Незавершені капітальні інвестиції (незавершене будівництво та придбання активів), на кінець звітного року [рядок 1005]</summary>
        [JsonProperty("D1_1005_01")]
        [JsonPropertyName("D1_1005_01")]
        public double? D11005_01 { get; set; }

        /// <summary>Незавершені капітальні інвестиції, на початок звітного року [рядок 1005]</summary>
        [JsonProperty("D1_1005_02")]
        [JsonPropertyName("D1_1005_02")]
        public double? D11005_02 { get; set; }

        /// <summary>Основні засоби — залишкова вартість, на кінець звітного року [рядок 1010]</summary>
        [JsonProperty("D1_1010_01")]
        [JsonPropertyName("D1_1010_01")]
        public double? D11010_01 { get; set; }

        /// <summary>Основні засоби — залишкова вартість, на початок звітного року [рядок 1010]</summary>
        [JsonProperty("D1_1010_02")]
        [JsonPropertyName("D1_1010_02")]
        public double? D11010_02 { get; set; }

        /// <summary>Основні засоби — первісна вартість, на кінець звітного року [рядок 1011]</summary>
        [JsonProperty("D1_1011_01")]
        [JsonPropertyName("D1_1011_01")]
        public double? D11011_01 { get; set; }

        /// <summary>Основні засоби — первісна вартість, на початок звітного року [рядок 1011]</summary>
        [JsonProperty("D1_1011_02")]
        [JsonPropertyName("D1_1011_02")]
        public double? D11011_02 { get; set; }

        /// <summary>Основні засоби — накопичений знос, на кінець звітного року [рядок 1012]</summary>
        [JsonProperty("D1_1012_01")]
        [JsonPropertyName("D1_1012_01")]
        public double? D11012_01 { get; set; }

        /// <summary>Основні засоби — накопичений знос, на початок звітного року [рядок 1012]</summary>
        [JsonProperty("D1_1012_02")]
        [JsonPropertyName("D1_1012_02")]
        public double? D11012_02 { get; set; }

        /// <summary>Інвестиційна нерухомість — залишкова вартість, на кінець звітного року [рядок 1015]</summary>
        [JsonProperty("D1_1015_01")]
        [JsonPropertyName("D1_1015_01")]
        public double? D11015_01 { get; set; }

        /// <summary>Інвестиційна нерухомість — залишкова вартість, на початок звітного року [рядок 1015]</summary>
        [JsonProperty("D1_1015_02")]
        [JsonPropertyName("D1_1015_02")]
        public double? D11015_02 { get; set; }

        /// <summary>Інвестиційна нерухомість — первісна або справедлива вартість, на кінець звітного року [рядок 1016]</summary>
        [JsonProperty("D1_1016_01")]
        [JsonPropertyName("D1_1016_01")]
        public double? D11016_01 { get; set; }

        /// <summary>Інвестиційна нерухомість — первісна або справедлива вартість, на початок звітного року [рядок 1016]</summary>
        [JsonProperty("D1_1016_02")]
        [JsonPropertyName("D1_1016_02")]
        public double? D11016_02 { get; set; }

        /// <summary>Інвестиційна нерухомість — накопичений знос, на кінець звітного року [рядок 1017]</summary>
        [JsonProperty("D1_1017_01")]
        [JsonPropertyName("D1_1017_01")]
        public double? D11017_01 { get; set; }

        /// <summary>Інвестиційна нерухомість — накопичений знос, на початок звітного року [рядок 1017]</summary>
        [JsonProperty("D1_1017_02")]
        [JsonPropertyName("D1_1017_02")]
        public double? D11017_02 { get; set; }

        /// <summary>Довгострокові біологічні активи — залишкова вартість, на кінець звітного року [рядок 1020]</summary>
        [JsonProperty("D1_1020_01")]
        [JsonPropertyName("D1_1020_01")]
        public double? D11020_01 { get; set; }

        /// <summary>Довгострокові біологічні активи — залишкова вартість, на початок звітного року [рядок 1020]</summary>
        [JsonProperty("D1_1020_02")]
        [JsonPropertyName("D1_1020_02")]
        public double? D11020_02 { get; set; }

        /// <summary>Довгострокові біологічні активи — первісна або справедлива вартість, на кінець звітного року [рядок 1021]</summary>
        [JsonProperty("D1_1021_01")]
        [JsonPropertyName("D1_1021_01")]
        public double? D11021_01 { get; set; }

        /// <summary>Довгострокові біологічні активи — первісна або справедлива вартість, на початок звітного року [рядок 1021]</summary>
        [JsonProperty("D1_1021_02")]
        [JsonPropertyName("D1_1021_02")]
        public double? D11021_02 { get; set; }

        /// <summary>Довгострокові біологічні активи — накопичена амортизація, на кінець звітного року [рядок 1022]</summary>
        [JsonProperty("D1_1022_01")]
        [JsonPropertyName("D1_1022_01")]
        public double? D11022_01 { get; set; }

        /// <summary>Довгострокові біологічні активи — накопичена амортизація, на початок звітного року [рядок 1022]</summary>
        [JsonProperty("D1_1022_02")]
        [JsonPropertyName("D1_1022_02")]
        public double? D11022_02 { get; set; }

        /// <summary>Довгострокові фінансові інвестиції, що обліковуються за методом участі в капіталі, на кінець звітного року [рядок 1030]</summary>
        [JsonProperty("D1_1030_01")]
        [JsonPropertyName("D1_1030_01")]
        public double? D11030_01 { get; set; }

        /// <summary>Довгострокові фінансові інвестиції за методом участі в капіталі, на початок звітного року [рядок 1030]</summary>
        [JsonProperty("D1_1030_02")]
        [JsonPropertyName("D1_1030_02")]
        public double? D11030_02 { get; set; }

        /// <summary>Інші довгострокові фінансові інвестиції, на кінець звітного року [рядок 1035]</summary>
        [JsonProperty("D1_1035_01")]
        [JsonPropertyName("D1_1035_01")]
        public double? D11035_01 { get; set; }

        /// <summary>Інші довгострокові фінансові інвестиції, на початок звітного року [рядок 1035]</summary>
        [JsonProperty("D1_1035_02")]
        [JsonPropertyName("D1_1035_02")]
        public double? D11035_02 { get; set; }

        /// <summary>Довгострокова дебіторська заборгованість, на кінець звітного року [рядок 1040]</summary>
        [JsonProperty("D1_1040_01")]
        [JsonPropertyName("D1_1040_01")]
        public double? D11040_01 { get; set; }

        /// <summary>Довгострокова дебіторська заборгованість, на початок звітного року [рядок 1040]</summary>
        [JsonProperty("D1_1040_02")]
        [JsonPropertyName("D1_1040_02")]
        public double? D11040_02 { get; set; }

        /// <summary>Відстрочені податкові активи, на кінець звітного року [рядок 1045]</summary>
        [JsonProperty("D1_1045_01")]
        [JsonPropertyName("D1_1045_01")]
        public double? D11045_01 { get; set; }

        /// <summary>Відстрочені податкові активи, на початок звітного року [рядок 1045]</summary>
        [JsonProperty("D1_1045_02")]
        [JsonPropertyName("D1_1045_02")]
        public double? D11045_02 { get; set; }

        /// <summary>Гудвіл, на кінець звітного року [рядок 1050]</summary>
        [JsonProperty("D1_1050_01")]
        [JsonPropertyName("D1_1050_01")]
        public double? D11050_01 { get; set; }

        /// <summary>Гудвіл, на початок звітного року [рядок 1050]</summary>
        [JsonProperty("D1_1050_02")]
        [JsonPropertyName("D1_1050_02")]
        public double? D11050_02 { get; set; }

        /// <summary>Права користування природними ресурсами — залишкова вартість, на кінець звітного року [рядок 1060]</summary>
        [JsonProperty("D1_1060_01")]
        [JsonPropertyName("D1_1060_01")]
        public double? D11060_01 { get; set; }

        /// <summary>Права користування природними ресурсами — залишкова вартість, на початок звітного року [рядок 1060]</summary>
        [JsonProperty("D1_1060_02")]
        [JsonPropertyName("D1_1060_02")]
        public double? D11060_02 { get; set; }

        /// <summary>Права користування майном (право користування за договорами оренди/лізингу) — залишкова вартість, на кінець звітного року [рядок 1065]</summary>
        [JsonProperty("D1_1065_01")]
        [JsonPropertyName("D1_1065_01")]
        public double? D11065_01 { get; set; }

        /// <summary>Права користування майном — залишкова вартість, на початок звітного року [рядок 1065]</summary>
        [JsonProperty("D1_1065_02")]
        [JsonPropertyName("D1_1065_02")]
        public double? D11065_02 { get; set; }

        /// <summary>Усього за розділом I «Необоротні активи», на кінець звітного року [рядок 1090]</summary>
        [JsonProperty("D1_1090_01")]
        [JsonPropertyName("D1_1090_01")]
        public double? D11090_01 { get; set; }

        /// <summary>Усього за розділом I «Необоротні активи», на початок звітного року [рядок 1090]</summary>
        [JsonProperty("D1_1090_02")]
        [JsonPropertyName("D1_1090_02")]
        public double? D11090_02 { get; set; }

        /// <summary>Необоротні активи, утримувані для продажу, та групи вибуття, на кінець звітного року [рядок 1095]</summary>
        [JsonProperty("D1_1095_01")]
        [JsonPropertyName("D1_1095_01")]
        public double? D11095_01 { get; set; }

        /// <summary>Необоротні активи, утримувані для продажу, та групи вибуття, на початок звітного року [рядок 1095]</summary>
        [JsonProperty("D1_1095_02")]
        [JsonPropertyName("D1_1095_02")]
        public double? D11095_02 { get; set; }

        // ========== Розділ II. Оборотні активи ==========

        /// <summary>Запаси (загальна сума: виробничі запаси + НЗВ + готова продукція + товари), на кінець звітного року [рядок 1100]</summary>
        [JsonProperty("D1_1100_01")]
        [JsonPropertyName("D1_1100_01")]
        public double? D11100_01 { get; set; }

        /// <summary>Запаси — загальна сума, на початок звітного року [рядок 1100]</summary>
        [JsonProperty("D1_1100_02")]
        [JsonPropertyName("D1_1100_02")]
        public double? D11100_02 { get; set; }

        /// <summary>Виробничі запаси (сировина, матеріали, паливо тощо), на кінець звітного року [рядок 1101]</summary>
        [JsonProperty("D1_1101_01")]
        [JsonPropertyName("D1_1101_01")]
        public double? D11101_01 { get; set; }

        /// <summary>Виробничі запаси, на початок звітного року [рядок 1101]</summary>
        [JsonProperty("D1_1101_02")]
        [JsonPropertyName("D1_1101_02")]
        public double? D11101_02 { get; set; }

        /// <summary>Незавершене виробництво, на кінець звітного року [рядок 1102]</summary>
        [JsonProperty("D1_1102_01")]
        [JsonPropertyName("D1_1102_01")]
        public double? D11102_01 { get; set; }

        /// <summary>Незавершене виробництво, на початок звітного року [рядок 1102]</summary>
        [JsonProperty("D1_1102_02")]
        [JsonPropertyName("D1_1102_02")]
        public double? D11102_02 { get; set; }

        /// <summary>Готова продукція, на кінець звітного року [рядок 1103]</summary>
        [JsonProperty("D1_1103_01")]
        [JsonPropertyName("D1_1103_01")]
        public double? D11103_01 { get; set; }

        /// <summary>Готова продукція, на початок звітного року [рядок 1103]</summary>
        [JsonProperty("D1_1103_02")]
        [JsonPropertyName("D1_1103_02")]
        public double? D11103_02 { get; set; }

        /// <summary>Товари (придбані для перепродажу), на кінець звітного року [рядок 1104]</summary>
        [JsonProperty("D1_1104_01")]
        [JsonPropertyName("D1_1104_01")]
        public double? D11104_01 { get; set; }

        /// <summary>Товари, на початок звітного року [рядок 1104]</summary>
        [JsonProperty("D1_1104_02")]
        [JsonPropertyName("D1_1104_02")]
        public double? D11104_02 { get; set; }

        /// <summary>Поточні біологічні активи, на кінець звітного року [рядок 1110]</summary>
        [JsonProperty("D1_1110_01")]
        [JsonPropertyName("D1_1110_01")]
        public double? D11110_01 { get; set; }

        /// <summary>Поточні біологічні активи, на початок звітного року [рядок 1110]</summary>
        [JsonProperty("D1_1110_02")]
        [JsonPropertyName("D1_1110_02")]
        public double? D11110_02 { get; set; }

        /// <summary>Депозити перестрахування (для страхових компаній), на кінець звітного року [рядок 1115]</summary>
        [JsonProperty("D1_1115_01")]
        [JsonPropertyName("D1_1115_01")]
        public double? D11115_01 { get; set; }

        /// <summary>Депозити перестрахування, на початок звітного року [рядок 1115]</summary>
        [JsonProperty("D1_1115_02")]
        [JsonPropertyName("D1_1115_02")]
        public double? D11115_02 { get; set; }

        /// <summary>Векселі одержані (поточна дебіторська заборгованість, забезпечена векселями), на кінець звітного року [рядок 1120]</summary>
        [JsonProperty("D1_1120_01")]
        [JsonPropertyName("D1_1120_01")]
        public double? D11120_01 { get; set; }

        /// <summary>Векселі одержані, на початок звітного року [рядок 1120]</summary>
        [JsonProperty("D1_1120_02")]
        [JsonPropertyName("D1_1120_02")]
        public double? D11120_02 { get; set; }

        /// <summary>Дебіторська заборгованість за продукцію, товари, роботи, послуги (чиста реалізаційна вартість), на кінець звітного року [рядок 1125]</summary>
        [JsonProperty("D1_1125_01")]
        [JsonPropertyName("D1_1125_01")]
        public double? D11125_01 { get; set; }

        /// <summary>Дебіторська заборгованість за продукцію, товари, роботи, послуги, на початок звітного року [рядок 1125]</summary>
        [JsonProperty("D1_1125_02")]
        [JsonPropertyName("D1_1125_02")]
        public double? D11125_02 { get; set; }

        /// <summary>Дебіторська заборгованість за розрахунками з бюджетом (ПДВ до відшкодування, акцизи тощо), на кінець звітного року [рядок 1130]</summary>
        [JsonProperty("D1_1130_01")]
        [JsonPropertyName("D1_1130_01")]
        public double? D11130_01 { get; set; }

        /// <summary>Дебіторська заборгованість за розрахунками з бюджетом, на початок звітного року [рядок 1130]</summary>
        [JsonProperty("D1_1130_02")]
        [JsonPropertyName("D1_1130_02")]
        public double? D11130_02 { get; set; }

        /// <summary>Дебіторська заборгованість з податку на прибуток (переплата ПНП), на кінець звітного року [рядок 1135]</summary>
        [JsonProperty("D1_1135_01")]
        [JsonPropertyName("D1_1135_01")]
        public double? D11135_01 { get; set; }

        /// <summary>Дебіторська заборгованість з податку на прибуток, на початок звітного року [рядок 1135]</summary>
        [JsonProperty("D1_1135_02")]
        [JsonPropertyName("D1_1135_02")]
        public double? D11135_02 { get; set; }

        /// <summary>Дебіторська заборгованість з нарахованих доходів (проценти, дивіденди до отримання), на кінець звітного року [рядок 1136]</summary>
        [JsonProperty("D1_1136_01")]
        [JsonPropertyName("D1_1136_01")]
        public double? D11136_01 { get; set; }

        /// <summary>Дебіторська заборгованість з нарахованих доходів, на початок звітного року [рядок 1136]</summary>
        [JsonProperty("D1_1136_02")]
        [JsonPropertyName("D1_1136_02")]
        public double? D11136_02 { get; set; }

        /// <summary>Дебіторська заборгованість за виданими авансами (передоплати постачальникам), на кінець звітного року [рядок 1140]</summary>
        [JsonProperty("D1_1140_01")]
        [JsonPropertyName("D1_1140_01")]
        public double? D11140_01 { get; set; }

        /// <summary>Дебіторська заборгованість за виданими авансами, на початок звітного року [рядок 1140]</summary>
        [JsonProperty("D1_1140_02")]
        [JsonPropertyName("D1_1140_02")]
        public double? D11140_02 { get; set; }

        /// <summary>Дебіторська заборгованість із внутрішніх розрахунків (заборгованість пов'язаних сторін/філій), на кінець звітного року [рядок 1145]</summary>
        [JsonProperty("D1_1145_01")]
        [JsonPropertyName("D1_1145_01")]
        public double? D11145_01 { get; set; }

        /// <summary>Дебіторська заборгованість із внутрішніх розрахунків, на початок звітного року [рядок 1145]</summary>
        [JsonProperty("D1_1145_02")]
        [JsonPropertyName("D1_1145_02")]
        public double? D11145_02 { get; set; }

        /// <summary>Інша поточна дебіторська заборгованість, на кінець звітного року [рядок 1155]</summary>
        [JsonProperty("D1_1155_01")]
        [JsonPropertyName("D1_1155_01")]
        public double? D11155_01 { get; set; }

        /// <summary>Інша поточна дебіторська заборгованість, на початок звітного року [рядок 1155]</summary>
        [JsonProperty("D1_1155_02")]
        [JsonPropertyName("D1_1155_02")]
        public double? D11155_02 { get; set; }

        /// <summary>Поточні фінансові інвестиції (короткострокові цінні папери, депозити до 1 року), на кінець звітного року [рядок 1160]</summary>
        [JsonProperty("D1_1160_01")]
        [JsonPropertyName("D1_1160_01")]
        public double? D11160_01 { get; set; }

        /// <summary>Поточні фінансові інвестиції, на початок звітного року [рядок 1160]</summary>
        [JsonProperty("D1_1160_02")]
        [JsonPropertyName("D1_1160_02")]
        public double? D11160_02 { get; set; }

        /// <summary>Гроші та їх еквіваленти — загальна сума (каса + рахунки в банках), на кінець звітного року [рядок 1165]</summary>
        [JsonProperty("D1_1165_01")]
        [JsonPropertyName("D1_1165_01")]
        public double? D11165_01 { get; set; }

        /// <summary>Гроші та їх еквіваленти — загальна сума, на початок звітного року [рядок 1165]</summary>
        [JsonProperty("D1_1165_02")]
        [JsonPropertyName("D1_1165_02")]
        public double? D11165_02 { get; set; }

        /// <summary>Готівка в касі, на кінець звітного року [рядок 1166]</summary>
        [JsonProperty("D1_1166_01")]
        [JsonPropertyName("D1_1166_01")]
        public double? D11166_01 { get; set; }

        /// <summary>Готівка в касі, на початок звітного року [рядок 1166]</summary>
        [JsonProperty("D1_1166_02")]
        [JsonPropertyName("D1_1166_02")]
        public double? D11166_02 { get; set; }

        /// <summary>Кошти на рахунках у банках (поточні, депозитні до запитання), на кінець звітного року [рядок 1167]</summary>
        [JsonProperty("D1_1167_01")]
        [JsonPropertyName("D1_1167_01")]
        public double? D11167_01 { get; set; }

        /// <summary>Кошти на рахунках у банках, на початок звітного року [рядок 1167]</summary>
        [JsonProperty("D1_1167_02")]
        [JsonPropertyName("D1_1167_02")]
        public double? D11167_02 { get; set; }

        /// <summary>Витрати майбутніх періодів (передплати, що будуть визнані витратами у наступних звітних періодах), на кінець звітного року [рядок 1170]</summary>
        [JsonProperty("D1_1170_01")]
        [JsonPropertyName("D1_1170_01")]
        public double? D11170_01 { get; set; }

        /// <summary>Витрати майбутніх періодів, на початок звітного року [рядок 1170]</summary>
        [JsonProperty("D1_1170_02")]
        [JsonPropertyName("D1_1170_02")]
        public double? D11170_02 { get; set; }

        /// <summary>Частка перестраховика у страхових резервах — загальна сума (для страхових компаній), на кінець звітного року [рядок 1180]</summary>
        [JsonProperty("D1_1180_01")]
        [JsonPropertyName("D1_1180_01")]
        public double? D11180_01 { get; set; }

        /// <summary>Частка перестраховика у страхових резервах, на початок звітного року [рядок 1180]</summary>
        [JsonProperty("D1_1180_02")]
        [JsonPropertyName("D1_1180_02")]
        public double? D11180_02 { get; set; }

        /// <summary>Частка перестраховика у резервах зі страхування життя, на кінець звітного року [рядок 1181]</summary>
        [JsonProperty("D1_1181_01")]
        [JsonPropertyName("D1_1181_01")]
        public double? D11181_01 { get; set; }

        /// <summary>Частка перестраховика у резервах зі страхування життя, на початок звітного року [рядок 1181]</summary>
        [JsonProperty("D1_1181_02")]
        [JsonPropertyName("D1_1181_02")]
        public double? D11181_02 { get; set; }

        /// <summary>Частка перестраховика у технічних резервах, на кінець звітного року [рядок 1182]</summary>
        [JsonProperty("D1_1182_01")]
        [JsonPropertyName("D1_1182_01")]
        public double? D11182_01 { get; set; }

        /// <summary>Частка перестраховика у технічних резервах, на початок звітного року [рядок 1182]</summary>
        [JsonProperty("D1_1182_02")]
        [JsonPropertyName("D1_1182_02")]
        public double? D11182_02 { get; set; }

        /// <summary>Частка перестраховика у резервах збитків, на кінець звітного року [рядок 1183]</summary>
        [JsonProperty("D1_1183_01")]
        [JsonPropertyName("D1_1183_01")]
        public double? D11183_01 { get; set; }

        /// <summary>Частка перестраховика у резервах збитків, на початок звітного року [рядок 1183]</summary>
        [JsonProperty("D1_1183_02")]
        [JsonPropertyName("D1_1183_02")]
        public double? D11183_02 { get; set; }

        /// <summary>Частка перестраховика в інших страхових резервах, на кінець звітного року [рядок 1184]</summary>
        [JsonProperty("D1_1184_01")]
        [JsonPropertyName("D1_1184_01")]
        public double? D11184_01 { get; set; }

        /// <summary>Частка перестраховика в інших страхових резервах, на початок звітного року [рядок 1184]</summary>
        [JsonProperty("D1_1184_02")]
        [JsonPropertyName("D1_1184_02")]
        public double? D11184_02 { get; set; }

        /// <summary>Інші оборотні активи, на кінець звітного року [рядок 1190]</summary>
        [JsonProperty("D1_1190_01")]
        [JsonPropertyName("D1_1190_01")]
        public double? D11190_01 { get; set; }

        /// <summary>Інші оборотні активи, на початок звітного року [рядок 1190]</summary>
        [JsonProperty("D1_1190_02")]
        [JsonPropertyName("D1_1190_02")]
        public double? D11190_02 { get; set; }

        /// <summary>Усього за розділом II «Оборотні активи», на кінець звітного року [рядок 1195]</summary>
        [JsonProperty("D1_1195_01")]
        [JsonPropertyName("D1_1195_01")]
        public double? D11195_01 { get; set; }

        /// <summary>Усього за розділом II «Оборотні активи», на початок звітного року [рядок 1195]</summary>
        [JsonProperty("D1_1195_02")]
        [JsonPropertyName("D1_1195_02")]
        public double? D11195_02 { get; set; }

        // ========== Розділ III. Необоротні активи для продажу ==========

        /// <summary>Необоротні активи, утримувані для продажу, та активи груп вибуття, на кінець звітного року [рядок 1200]</summary>
        [JsonProperty("D1_1200_01")]
        [JsonPropertyName("D1_1200_01")]
        public double? D11200_01 { get; set; }

        /// <summary>Необоротні активи, утримувані для продажу, та активи груп вибуття, на початок звітного року [рядок 1200]</summary>
        [JsonProperty("D1_1200_02")]
        [JsonPropertyName("D1_1200_02")]
        public double? D11200_02 { get; set; }

        /// <summary>БАЛАНС — підсумок активу (усього активів), на кінець звітного року [рядок 1300]</summary>
        [JsonProperty("D1_1300_01")]
        [JsonPropertyName("D1_1300_01")]
        public double? D11300_01 { get; set; }

        /// <summary>БАЛАНС — підсумок активу, на початок звітного року [рядок 1300]</summary>
        [JsonProperty("D1_1300_02")]
        [JsonPropertyName("D1_1300_02")]
        public double? D11300_02 { get; set; }

        // =====================================================================
        // ==================== ПАСИВ ====================
        // ========== Розділ I. Власний капітал ==========

        /// <summary>Зареєстрований (пайовий) капітал (статутний/складений капітал), на кінець звітного року [рядок 1400]</summary>
        [JsonProperty("D1_1400_01")]
        [JsonPropertyName("D1_1400_01")]
        public double? D11400_01 { get; set; }

        /// <summary>Зареєстрований (пайовий) капітал, на початок звітного року [рядок 1400]</summary>
        [JsonProperty("D1_1400_02")]
        [JsonPropertyName("D1_1400_02")]
        public double? D11400_02 { get; set; }

        /// <summary>Внески до незареєстрованого статутного капіталу, на кінець звітного року [рядок 1401]</summary>
        [JsonProperty("D1_1401_01")]
        [JsonPropertyName("D1_1401_01")]
        public double? D11401_01 { get; set; }

        /// <summary>Внески до незареєстрованого статутного капіталу, на початок звітного року [рядок 1401]</summary>
        [JsonProperty("D1_1401_02")]
        [JsonPropertyName("D1_1401_02")]
        public double? D11401_02 { get; set; }

        /// <summary>Капітал у дооцінках (накопичені дооцінки/уцінки необоротних активів і фінансових інструментів), на кінець звітного року [рядок 1405]</summary>
        [JsonProperty("D1_1405_01")]
        [JsonPropertyName("D1_1405_01")]
        public double? D11405_01 { get; set; }

        /// <summary>Капітал у дооцінках, на початок звітного року [рядок 1405]</summary>
        [JsonProperty("D1_1405_02")]
        [JsonPropertyName("D1_1405_02")]
        public double? D11405_02 { get; set; }

        /// <summary>Додатковий капітал (інший, ніж емісійний дохід), на кінець звітного року [рядок 1410]</summary>
        [JsonProperty("D1_1410_01")]
        [JsonPropertyName("D1_1410_01")]
        public double? D11410_01 { get; set; }

        /// <summary>Додатковий капітал, на початок звітного року [рядок 1410]</summary>
        [JsonProperty("D1_1410_02")]
        [JsonPropertyName("D1_1410_02")]
        public double? D11410_02 { get; set; }

        /// <summary>Емісійний дохід (перевищення ціни розміщення акцій над їх номіналом), на кінець звітного року [рядок 1411]</summary>
        [JsonProperty("D1_1411_01")]
        [JsonPropertyName("D1_1411_01")]
        public double? D11411_01 { get; set; }

        /// <summary>Емісійний дохід, на початок звітного року [рядок 1411]</summary>
        [JsonProperty("D1_1411_02")]
        [JsonPropertyName("D1_1411_02")]
        public double? D11411_02 { get; set; }

        /// <summary>Накопичені курсові різниці від перерахунку фінансової звітності в іноземній валюті, на кінець звітного року [рядок 1412]</summary>
        [JsonProperty("D1_1412_01")]
        [JsonPropertyName("D1_1412_01")]
        public double? D11412_01 { get; set; }

        /// <summary>Накопичені курсові різниці, на початок звітного року [рядок 1412]</summary>
        [JsonProperty("D1_1412_02")]
        [JsonPropertyName("D1_1412_02")]
        public double? D11412_02 { get; set; }

        /// <summary>Резервний капітал (законодавчо обов'язковий або добровільно сформований резерв), на кінець звітного року [рядок 1415]</summary>
        [JsonProperty("D1_1415_01")]
        [JsonPropertyName("D1_1415_01")]
        public double? D11415_01 { get; set; }

        /// <summary>Резервний капітал, на початок звітного року [рядок 1415]</summary>
        [JsonProperty("D1_1415_02")]
        [JsonPropertyName("D1_1415_02")]
        public double? D11415_02 { get; set; }

        /// <summary>Нерозподілений прибуток (непокритий збиток), на кінець звітного року [рядок 1420]</summary>
        [JsonProperty("D1_1420_01")]
        [JsonPropertyName("D1_1420_01")]
        public double? D11420_01 { get; set; }

        /// <summary>Нерозподілений прибуток (непокритий збиток), на початок звітного року [рядок 1420]</summary>
        [JsonProperty("D1_1420_02")]
        [JsonPropertyName("D1_1420_02")]
        public double? D11420_02 { get; set; }

        /// <summary>Неоплачений капітал (заборгованість засновників за внесками до статутного капіталу, від'ємна величина), на кінець звітного року [рядок 1425]</summary>
        [JsonProperty("D1_1425_01")]
        [JsonPropertyName("D1_1425_01")]
        public double? D11425_01 { get; set; }

        /// <summary>Неоплачений капітал, на початок звітного року [рядок 1425]</summary>
        [JsonProperty("D1_1425_02")]
        [JsonPropertyName("D1_1425_02")]
        public double? D11425_02 { get; set; }

        /// <summary>Вилучений капітал (викуплені власні акції/частки, від'ємна величина), на кінець звітного року [рядок 1430]</summary>
        [JsonProperty("D1_1430_01")]
        [JsonPropertyName("D1_1430_01")]
        public double? D11430_01 { get; set; }

        /// <summary>Вилучений капітал, на початок звітного року [рядок 1430]</summary>
        [JsonProperty("D1_1430_02")]
        [JsonPropertyName("D1_1430_02")]
        public double? D11430_02 { get; set; }

        /// <summary>Інші резерви власного капіталу, на кінець звітного року [рядок 1435]</summary>
        [JsonProperty("D1_1435_01")]
        [JsonPropertyName("D1_1435_01")]
        public double? D11435_01 { get; set; }

        /// <summary>Інші резерви власного капіталу, на початок звітного року [рядок 1435]</summary>
        [JsonProperty("D1_1435_02")]
        [JsonPropertyName("D1_1435_02")]
        public double? D11435_02 { get; set; }

        /// <summary>Усього за розділом I «Власний капітал», на кінець звітного року [рядок 1495]</summary>
        [JsonProperty("D1_1495_01")]
        [JsonPropertyName("D1_1495_01")]
        public double? D11495_01 { get; set; }

        /// <summary>Усього за розділом I «Власний капітал», на початок звітного року [рядок 1495]</summary>
        [JsonProperty("D1_1495_02")]
        [JsonPropertyName("D1_1495_02")]
        public double? D11495_02 { get; set; }

        // ========== Розділ II. Довгострокові зобов'язання і забезпечення ==========

        /// <summary>Відстрочені податкові зобов'язання, на кінець звітного року [рядок 1500]</summary>
        [JsonProperty("D1_1500_01")]
        [JsonPropertyName("D1_1500_01")]
        public double? D11500_01 { get; set; }

        /// <summary>Відстрочені податкові зобов'язання, на початок звітного року [рядок 1500]</summary>
        [JsonProperty("D1_1500_02")]
        [JsonPropertyName("D1_1500_02")]
        public double? D11500_02 { get; set; }

        /// <summary>Пенсійні зобов'язання (довгострокові зобов'язання з виплат персоналу після виходу на пенсію), на кінець звітного року [рядок 1505]</summary>
        [JsonProperty("D1_1505_01")]
        [JsonPropertyName("D1_1505_01")]
        public double? D11505_01 { get; set; }

        /// <summary>Пенсійні зобов'язання, на початок звітного року [рядок 1505]</summary>
        [JsonProperty("D1_1505_02")]
        [JsonPropertyName("D1_1505_02")]
        public double? D11505_02 { get; set; }

        /// <summary>Довгострокові кредити банків (кредити та позики зі строком погашення понад 1 рік), на кінець звітного року [рядок 1510]</summary>
        [JsonProperty("D1_1510_01")]
        [JsonPropertyName("D1_1510_01")]
        public double? D11510_01 { get; set; }

        /// <summary>Довгострокові кредити банків, на початок звітного року [рядок 1510]</summary>
        [JsonProperty("D1_1510_02")]
        [JsonPropertyName("D1_1510_02")]
        public double? D11510_02 { get; set; }

        /// <summary>Інші довгострокові зобов'язання (облігації, фінансова оренда тощо), на кінець звітного року [рядок 1515]</summary>
        [JsonProperty("D1_1515_01")]
        [JsonPropertyName("D1_1515_01")]
        public double? D11515_01 { get; set; }

        /// <summary>Інші довгострокові зобов'язання, на початок звітного року [рядок 1515]</summary>
        [JsonProperty("D1_1515_02")]
        [JsonPropertyName("D1_1515_02")]
        public double? D11515_02 { get; set; }

        /// <summary>Довгострокові забезпечення — загальна сума (резерви на виплати, гарантії тощо), на кінець звітного року [рядок 1520]</summary>
        [JsonProperty("D1_1520_01")]
        [JsonPropertyName("D1_1520_01")]
        public double? D11520_01 { get; set; }

        /// <summary>Довгострокові забезпечення — загальна сума, на початок звітного року [рядок 1520]</summary>
        [JsonProperty("D1_1520_02")]
        [JsonPropertyName("D1_1520_02")]
        public double? D11520_02 { get; set; }

        /// <summary>Забезпечення виплат персоналу (довгострокові забезпечення відпусток, бонусів тощо), на кінець звітного року [рядок 1521]</summary>
        [JsonProperty("D1_1525_01")]
        [JsonPropertyName("D1_1525_01")]
        public double? D11525_01 { get; set; }

        /// <summary>Забезпечення виплат персоналу, на початок звітного року [рядок 1521]</summary>
        [JsonProperty("D1_1525_02")]
        [JsonPropertyName("D1_1525_02")]
        public double? D11525_02 { get; set; }

        /// <summary>Цільове фінансування (отримані довгострокові гранти та субсидії від держави чи донорів), на кінець звітного року [рядок 1526]</summary>
        [JsonProperty("D1_1526_01")]
        [JsonPropertyName("D1_1526_01")]
        public double? D11526_01 { get; set; }

        /// <summary>Цільове фінансування, на початок звітного року [рядок 1526]</summary>
        [JsonProperty("D1_1526_02")]
        [JsonPropertyName("D1_1526_02")]
        public double? D11526_02 { get; set; }

        /// <summary>Страхові резерви — загальна сума (для страхових компаній), на кінець звітного року [рядок 1530]</summary>
        [JsonProperty("D1_1530_01")]
        [JsonPropertyName("D1_1530_01")]
        public double? D11530_01 { get; set; }

        /// <summary>Страхові резерви — загальна сума, на початок звітного року [рядок 1530]</summary>
        [JsonProperty("D1_1530_02")]
        [JsonPropertyName("D1_1530_02")]
        public double? D11530_02 { get; set; }

        /// <summary>Технічні страхові резерви (резерви незароблених премій, резерви збитків тощо), на кінець звітного року [рядок 1531]</summary>
        [JsonProperty("D1_1531_01")]
        [JsonPropertyName("D1_1531_01")]
        public double? D11531_01 { get; set; }

        /// <summary>Технічні страхові резерви, на початок звітного року [рядок 1531]</summary>
        [JsonProperty("D1_1531_02")]
        [JsonPropertyName("D1_1531_02")]
        public double? D11531_02 { get; set; }

        /// <summary>Резерви зі страхування життя, на кінець звітного року [рядок 1532]</summary>
        [JsonProperty("D1_1532_01")]
        [JsonPropertyName("D1_1532_01")]
        public double? D11532_01 { get; set; }

        /// <summary>Резерви зі страхування життя, на початок звітного року [рядок 1532]</summary>
        [JsonProperty("D1_1532_02")]
        [JsonPropertyName("D1_1532_02")]
        public double? D11532_02 { get; set; }

        /// <summary>Частки перестраховиків у страхових резервах (від'ємна позиція в пасиві), на кінець звітного року [рядок 1533]</summary>
        [JsonProperty("D1_1533_01")]
        [JsonPropertyName("D1_1533_01")]
        public double? D11533_01 { get; set; }

        /// <summary>Частки перестраховиків у страхових резервах, на початок звітного року [рядок 1533]</summary>
        [JsonProperty("D1_1533_02")]
        [JsonPropertyName("D1_1533_02")]
        public double? D11533_02 { get; set; }

        /// <summary>Інші страхові резерви, на кінець звітного року [рядок 1534]</summary>
        [JsonProperty("D1_1534_01")]
        [JsonPropertyName("D1_1534_01")]
        public double? D11534_01 { get; set; }

        /// <summary>Інші страхові резерви, на початок звітного року [рядок 1534]</summary>
        [JsonProperty("D1_1534_02")]
        [JsonPropertyName("D1_1534_02")]
        public double? D11534_02 { get; set; }

        /// <summary>Додаткові страхові резерви або інші довгострокові забезпечення, на кінець звітного року [рядок 1535]</summary>
        [JsonProperty("D1_1535_01")]
        [JsonPropertyName("D1_1535_01")]
        public double? D11535_01 { get; set; }

        /// <summary>Додаткові страхові резерви або інші довгострокові забезпечення, на початок звітного року [рядок 1535]</summary>
        [JsonProperty("D1_1535_02")]
        [JsonPropertyName("D1_1535_02")]
        public double? D11535_02 { get; set; }

        /// <summary>Інвестиційні контракти (зобов'язання за інвестиційними договорами страхових компаній), на кінець звітного року [рядок 1540]</summary>
        [JsonProperty("D1_1540_01")]
        [JsonPropertyName("D1_1540_01")]
        public double? D11540_01 { get; set; }

        /// <summary>Інвестиційні контракти, на початок звітного року [рядок 1540]</summary>
        [JsonProperty("D1_1540_02")]
        [JsonPropertyName("D1_1540_02")]
        public double? D11540_02 { get; set; }

        /// <summary>Інші довгострокові зобов'язання та забезпечення, на кінець звітного року [рядок 1545]</summary>
        [JsonProperty("D1_1545_01")]
        [JsonPropertyName("D1_1545_01")]
        public double? D11545_01 { get; set; }

        /// <summary>Інші довгострокові зобов'язання та забезпечення, на початок звітного року [рядок 1545]</summary>
        [JsonProperty("D1_1545_02")]
        [JsonPropertyName("D1_1545_02")]
        public double? D11545_02 { get; set; }

        /// <summary>Усього за розділом II «Довгострокові зобов'язання і забезпечення», на кінець звітного року [рядок 1595]</summary>
        [JsonProperty("D1_1595_01")]
        [JsonPropertyName("D1_1595_01")]
        public double? D11595_01 { get; set; }

        /// <summary>Усього за розділом II «Довгострокові зобов'язання і забезпечення», на початок звітного року [рядок 1595]</summary>
        [JsonProperty("D1_1595_02")]
        [JsonPropertyName("D1_1595_02")]
        public double? D11595_02 { get; set; }

        // ========== Розділ III. Поточні зобов'язання і забезпечення ==========

        /// <summary>Короткострокові кредити банків (кредити, овердрафт зі строком до 1 року), на кінець звітного року [рядок 1600]</summary>
        [JsonProperty("D1_1600_01")]
        [JsonPropertyName("D1_1600_01")]
        public double? D11600_01 { get; set; }

        /// <summary>Короткострокові кредити банків, на початок звітного року [рядок 1600]</summary>
        [JsonProperty("D1_1600_02")]
        [JsonPropertyName("D1_1600_02")]
        public double? D11600_02 { get; set; }

        /// <summary>Векселі видані (поточна кредиторська заборгованість, забезпечена виданими векселями), на кінець звітного року [рядок 1605]</summary>
        [JsonProperty("D1_1605_01")]
        [JsonPropertyName("D1_1605_01")]
        public double? D11605_01 { get; set; }

        /// <summary>Векселі видані, на початок звітного року [рядок 1605]</summary>
        [JsonProperty("D1_1605_02")]
        [JsonPropertyName("D1_1605_02")]
        public double? D11605_02 { get; set; }

        /// <summary>Поточна частина довгострокових зобов'язань (кредити та позики зі строком погашення до 1 року), на кінець звітного року [рядок 1610]</summary>
        [JsonProperty("D1_1610_01")]
        [JsonPropertyName("D1_1610_01")]
        public double? D11610_01 { get; set; }

        /// <summary>Поточна частина довгострокових зобов'язань, на початок звітного року [рядок 1610]</summary>
        [JsonProperty("D1_1610_02")]
        [JsonPropertyName("D1_1610_02")]
        public double? D11610_02 { get; set; }

        /// <summary>Кредиторська заборгованість за товари, роботи, послуги (торгова кредиторська заборгованість), на кінець звітного року [рядок 1615]</summary>
        [JsonProperty("D1_1615_01")]
        [JsonPropertyName("D1_1615_01")]
        public double? D11615_01 { get; set; }

        /// <summary>Кредиторська заборгованість за товари, роботи, послуги, на початок звітного року [рядок 1615]</summary>
        [JsonProperty("D1_1615_02")]
        [JsonPropertyName("D1_1615_02")]
        public double? D11615_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками з бюджетом (податки та збори до сплати), на кінець звітного року [рядок 1620]</summary>
        [JsonProperty("D1_1620_01")]
        [JsonPropertyName("D1_1620_01")]
        public double? D11620_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками з бюджетом, на початок звітного року [рядок 1620]</summary>
        [JsonProperty("D1_1620_02")]
        [JsonPropertyName("D1_1620_02")]
        public double? D11620_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість з податку на прибуток, на кінець звітного року [рядок 1621]</summary>
        [JsonProperty("D1_1621_01")]
        [JsonPropertyName("D1_1621_01")]
        public double? D11621_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість з податку на прибуток, на початок звітного року [рядок 1621]</summary>
        [JsonProperty("D1_1621_02")]
        [JsonPropertyName("D1_1621_02")]
        public double? D11621_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками зі страхування (ЄСВ та інші збори), на кінець звітного року [рядок 1625]</summary>
        [JsonProperty("D1_1625_01")]
        [JsonPropertyName("D1_1625_01")]
        public double? D11625_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками зі страхування, на початок звітного року [рядок 1625]</summary>
        [JsonProperty("D1_1625_02")]
        [JsonPropertyName("D1_1625_02")]
        public double? D11625_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість з оплати праці (нарахована, але не виплачена зарплата), на кінець звітного року [рядок 1630]</summary>
        [JsonProperty("D1_1630_01")]
        [JsonPropertyName("D1_1630_01")]
        public double? D11630_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість з оплати праці, на початок звітного року [рядок 1630]</summary>
        [JsonProperty("D1_1630_02")]
        [JsonPropertyName("D1_1630_02")]
        public double? D11630_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за одержаними авансами (передплати від покупців), на кінець звітного року [рядок 1635]</summary>
        [JsonProperty("D1_1635_01")]
        [JsonPropertyName("D1_1635_01")]
        public double? D11635_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за одержаними авансами, на початок звітного року [рядок 1635]</summary>
        [JsonProperty("D1_1635_02")]
        [JsonPropertyName("D1_1635_02")]
        public double? D11635_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками з учасниками (нараховані, але не виплачені дивіденди), на кінець звітного року [рядок 1640]</summary>
        [JsonProperty("D1_1640_01")]
        [JsonPropertyName("D1_1640_01")]
        public double? D11640_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість за розрахунками з учасниками, на початок звітного року [рядок 1640]</summary>
        [JsonProperty("D1_1640_02")]
        [JsonPropertyName("D1_1640_02")]
        public double? D11640_02 { get; set; }

        /// <summary>Поточна кредиторська заборгованість із внутрішніх розрахунків (заборгованість перед філіями/пов'язаними сторонами), на кінець звітного року [рядок 1645]</summary>
        [JsonProperty("D1_1645_01")]
        [JsonPropertyName("D1_1645_01")]
        public double? D11645_01 { get; set; }

        /// <summary>Поточна кредиторська заборгованість із внутрішніх розрахунків, на початок звітного року [рядок 1645]</summary>
        [JsonProperty("D1_1645_02")]
        [JsonPropertyName("D1_1645_02")]
        public double? D11645_02 { get; set; }

        /// <summary>Поточні забезпечення (резерви на відпустки, гарантійні зобов'язання тощо), на кінець звітного року [рядок 1650]</summary>
        [JsonProperty("D1_1650_01")]
        [JsonPropertyName("D1_1650_01")]
        public double? D11650_01 { get; set; }

        /// <summary>Поточні забезпечення, на початок звітного року [рядок 1650]</summary>
        [JsonProperty("D1_1650_02")]
        [JsonPropertyName("D1_1650_02")]
        public double? D11650_02 { get; set; }

        /// <summary>Доходи майбутніх періодів (відстрочені доходи, субсидії тощо), на кінець звітного року [рядок 1660]</summary>
        [JsonProperty("D1_1660_01")]
        [JsonPropertyName("D1_1660_01")]
        public double? D11660_01 { get; set; }

        /// <summary>Доходи майбутніх періодів, на початок звітного року [рядок 1660]</summary>
        [JsonProperty("D1_1660_02")]
        [JsonPropertyName("D1_1660_02")]
        public double? D11660_02 { get; set; }

        /// <summary>Інші поточні зобов'язання (не включені до попередніх рядків), на кінець звітного року [рядок 1665]</summary>
        [JsonProperty("D1_1665_01")]
        [JsonPropertyName("D1_1665_01")]
        public double? D11665_01 { get; set; }

        /// <summary>Інші поточні зобов'язання, на початок звітного року [рядок 1665]</summary>
        [JsonProperty("D1_1665_02")]
        [JsonPropertyName("D1_1665_02")]
        public double? D11665_02 { get; set; }

        /// <summary>Інші поточні зобов'язання — додаткова стаття, на кінець звітного року [рядок 1670]</summary>
        [JsonProperty("D1_1670_01")]
        [JsonPropertyName("D1_1670_01")]
        public double? D11670_01 { get; set; }

        /// <summary>Інші поточні зобов'язання — додаткова стаття, на початок звітного року [рядок 1670]</summary>
        [JsonProperty("D1_1670_02")]
        [JsonPropertyName("D1_1670_02")]
        public double? D11670_02 { get; set; }

        /// <summary>Усього за розділом III «Поточні зобов'язання і забезпечення», на кінець звітного року [рядок 1690]</summary>
        [JsonProperty("D1_1690_01")]
        [JsonPropertyName("D1_1690_01")]
        public double? D11690_01 { get; set; }

        /// <summary>Усього за розділом III «Поточні зобов'язання і забезпечення», на початок звітного року [рядок 1690]</summary>
        [JsonProperty("D1_1690_02")]
        [JsonPropertyName("D1_1690_02")]
        public double? D11690_02 { get; set; }

        // ========== Розділ IV. Зобов'язання, пов'язані з необоротними активами для продажу ==========

        /// <summary>Зобов'язання, пов'язані з необоротними активами, утримуваними для продажу, та групами вибуття, на кінець звітного року [рядок 1695]</summary>
        [JsonProperty("D1_1695_01")]
        [JsonPropertyName("D1_1695_01")]
        public double? D11695_01 { get; set; }

        /// <summary>Зобов'язання, пов'язані з необоротними активами, утримуваними для продажу, та групами вибуття, на початок звітного року [рядок 1695]</summary>
        [JsonProperty("D1_1695_02")]
        [JsonPropertyName("D1_1695_02")]
        public double? D11695_02 { get; set; }

        /// <summary>БАЛАНС — підсумок пасиву (власний капітал + усі зобов'язання), на кінець звітного року [рядок 1700/1800]</summary>
        [JsonProperty("D1_1700_01")]
        [JsonPropertyName("D1_1700_01")]
        public double? D11700_01 { get; set; }

        /// <summary>БАЛАНС — підсумок пасиву, на початок звітного року [рядок 1700/1800]</summary>
        [JsonProperty("D1_1700_02")]
        [JsonPropertyName("D1_1700_02")]
        public double? D11700_02 { get; set; }

        /// <summary>БАЛАНС — альтернативний підсумок пасиву (деякі форми звітності), на кінець звітного року [рядок 1800]</summary>
        [JsonProperty("D1_1800_01")]
        [JsonPropertyName("D1_1800_01")]
        public double? D11800_01 { get; set; }

        /// <summary>БАЛАНС — альтернативний підсумок пасиву, на початок звітного року [рядок 1800]</summary>
        [JsonProperty("D1_1800_02")]
        [JsonPropertyName("D1_1800_02")]
        public double? D11800_02 { get; set; }

        /// <summary>Додатковий підсумок пасиву балансу (специфічні форми або консолідована звітність), на кінець звітного року [рядок 1900]</summary>
        [JsonProperty("D1_1900_01")]
        [JsonPropertyName("D1_1900_01")]
        public double? D11900_01 { get; set; }

        /// <summary>Додатковий підсумок пасиву балансу, на початок звітного року [рядок 1900]</summary>
        [JsonProperty("D1_1900_02")]
        [JsonPropertyName("D1_1900_02")]
        public double? D11900_02 { get; set; }

        // =====================================================================
        // ФОРМА 2 — ЗВІТ ПРО ФІНАНСОВІ РЕЗУЛЬТАТИ (D2)
        // Суфікс _01 — за звітний рік; _02 — за попередній рік (або альтернативний рядок)
        // =====================================================================
        // ========== Розділ I. Фінансові результати ==========

        /// <summary>Чистий дохід від реалізації продукції (товарів, робіт, послуг) — за звітний рік [рядок 2000]</summary>
        [JsonProperty("D2_2000_01")]
        [JsonPropertyName("D2_2000_01")]
        public double? D22000_01 { get; set; }

        /// <summary>Собівартість реалізованої продукції (товарів, робіт, послуг) — за звітний рік [рядок 2010]</summary>
        [JsonProperty("D2_2010_01")]
        [JsonPropertyName("D2_2010_01")]
        public double? D22010_01 { get; set; }

        /// <summary>Виробнича собівартість реалізованої продукції власного виробництва — за звітний рік [рядок 2011]</summary>
        [JsonProperty("D2_2011_01")]
        [JsonPropertyName("D2_2011_01")]
        public double? D22011_01 { get; set; }

        /// <summary>Собівартість реалізованих товарів (закупівельна вартість) — за звітний рік [рядок 2012]</summary>
        [JsonProperty("D2_2012_01")]
        [JsonPropertyName("D2_2012_01")]
        public double? D22012_01 { get; set; }

        /// <summary>Собівартість реалізованих робіт і послуг — за звітний рік [рядок 2013]</summary>
        [JsonProperty("D2_2013_01")]
        [JsonPropertyName("D2_2013_01")]
        public double? D22013_01 { get; set; }

        /// <summary>Інші складові собівартості реалізації — за звітний рік [рядок 2014]</summary>
        [JsonProperty("D2_2014_01")]
        [JsonPropertyName("D2_2014_01")]
        public double? D22014_01 { get; set; }

        /// <summary>Валовий прибуток (збиток) = Чистий дохід − Собівартість — за звітний рік [рядок 2050]</summary>
        [JsonProperty("D2_2050_01")]
        [JsonPropertyName("D2_2050_01")]
        public double? D22050_01 { get; set; }

        /// <summary>Інші операційні доходи (дохід від оренди, курсові різниці, штрафи отримані тощо) — за звітний рік [рядок 2070]</summary>
        [JsonProperty("D2_2070_01")]
        [JsonPropertyName("D2_2070_01")]
        public double? D22070_01 { get; set; }

        /// <summary>Адміністративні витрати (утримання управлінського персоналу, офісу тощо) — за звітний рік [рядок 2090]</summary>
        [JsonProperty("D2_2090_01")]
        [JsonPropertyName("D2_2090_01")]
        public double? D22090_01 { get; set; }

        /// <summary>Витрати на збут (реклама, доставка покупцям, витрати відділу продажів) — за звітний рік [рядок 2095]</summary>
        [JsonProperty("D2_2095_01")]
        [JsonPropertyName("D2_2095_01")]
        public double? D22095_01 { get; set; }

        /// <summary>Витрати на дослідження та розробки (R&amp;D) — за звітний рік [рядок 2111]</summary>
        [JsonProperty("D2_2111_01")]
        [JsonPropertyName("D2_2111_01")]
        public double? D22111_01 { get; set; }

        /// <summary>Витрати від знецінення активів (збитки від зменшення корисності) — за звітний рік [рядок 2112]</summary>
        [JsonProperty("D2_2112_01")]
        [JsonPropertyName("D2_2112_01")]
        public double? D22112_01 { get; set; }

        /// <summary>Інші операційні витрати (курсові різниці, штрафи сплачені, псування запасів тощо) — за звітний рік [рядок 2120]</summary>
        [JsonProperty("D2_2120_01")]
        [JsonPropertyName("D2_2120_01")]
        public double? D22120_01 { get; set; }

        /// <summary>Резерв на покриття очікуваних кредитних збитків (ECL) за фінансовими інструментами — за звітний рік [рядок 2121]</summary>
        [JsonProperty("D2_2121_01")]
        [JsonPropertyName("D2_2121_01")]
        public double? D22121_01 { get; set; }

        /// <summary>Інші операційні витрати — додаткова стаття — за звітний рік [рядок 2122]</summary>
        [JsonProperty("D2_2122_01")]
        [JsonPropertyName("D2_2122_01")]
        public double? D22122_01 { get; set; }

        /// <summary>Інші операційні витрати — розширена стаття (для окремих видів діяльності) — за звітний рік [рядок 2130]</summary>
        [JsonProperty("D2_2130_01")]
        [JsonPropertyName("D2_2130_01")]
        public double? D22130_01 { get; set; }

        /// <summary>Фінансовий результат від операційної діяльності: прибуток (+) або збиток (−) — за звітний рік [рядок 2150]</summary>
        [JsonProperty("D2_2150_01")]
        [JsonPropertyName("D2_2150_01")]
        public double? D22150_01 { get; set; }

        /// <summary>Дохід від участі в капіталі (частка прибутку асоційованих та спільно контрольованих підприємств) — за звітний рік [рядок 2160]</summary>
        [JsonProperty("D2_2160_01")]
        [JsonPropertyName("D2_2160_01")]
        public double? D22160_01 { get; set; }

        /// <summary>Інші фінансові доходи (проценти до отримання, дивіденди від фінансових інвестицій тощо) — за звітний рік [рядок 2165]</summary>
        [JsonProperty("D2_2165_01")]
        [JsonPropertyName("D2_2165_01")]
        public double? D22165_01 { get; set; }

        /// <summary>Фінансові витрати — загальна сума (проценти за кредитами, витрати з фінансової оренди тощо) — за звітний рік [рядок 2180]</summary>
        [JsonProperty("D2_2180_01")]
        [JsonPropertyName("D2_2180_01")]
        public double? D22180_01 { get; set; }

        /// <summary>Витрати на сплату відсотків за кредитами та позиками — за звітний рік [рядок 2181]</summary>
        [JsonProperty("D2_2181_01")]
        [JsonPropertyName("D2_2181_01")]
        public double? D22181_01 { get; set; }

        /// <summary>Інші фінансові витрати (комісії банків, витрати за облігаціями тощо) — за звітний рік [рядок 2182]</summary>
        [JsonProperty("D2_2182_01")]
        [JsonPropertyName("D2_2182_01")]
        public double? D22182_01 { get; set; }

        /// <summary>Втрати від участі в капіталі (частка збитків асоційованих та спільно контрольованих підприємств) — за звітний рік [рядок 2190]</summary>
        [JsonProperty("D2_2190_01")]
        [JsonPropertyName("D2_2190_01")]
        public double? D22190_01 { get; set; }

        /// <summary>Інші витрати (витрати від вибуття необоротних активів, уцінка фінансових інвестицій тощо) — за звітний рік [рядок 2195]</summary>
        [JsonProperty("D2_2195_01")]
        [JsonPropertyName("D2_2195_01")]
        public double? D22195_01 { get; set; }

        /// <summary>Фінансовий результат до оподаткування: прибуток (+) або збиток (−) — за звітний рік [рядок 2200]</summary>
        [JsonProperty("D2_2200_01")]
        [JsonPropertyName("D2_2200_01")]
        public double? D22200_01 { get; set; }

        /// <summary>Витрати (дохід) з податку на прибуток (поточний ПНП ± відстрочені зміни) — за звітний рік [рядок 2220]</summary>
        [JsonProperty("D2_2220_01")]
        [JsonPropertyName("D2_2220_01")]
        public double? D22220_01 { get; set; }

        /// <summary>Чистий фінансовий результат: прибуток — за звітний рік [рядок 2350, колонка «прибуток»]</summary>
        [JsonProperty("D2_2240_01")]
        [JsonPropertyName("D2_2240_01")]
        public double? D22240_01 { get; set; }

        /// <summary>Чистий фінансовий результат: збиток — за звітний рік [рядок 2350, колонка «збиток»]</summary>
        [JsonProperty("D2_2240_02")]
        [JsonPropertyName("D2_2240_02")]
        public double? D22240_02 { get; set; }

        /// <summary>Скоригований чистий фінансовий результат (після коригувань) — за звітний рік [рядок 2241]</summary>
        [JsonProperty("D2_2241_01")]
        [JsonPropertyName("D2_2241_01")]
        public double? D22241_01 { get; set; }

        // ========== Розділ II. Інший сукупний дохід ==========

        /// <summary>Дооцінка (уцінка) необоротних активів, що відображається в іншому сукупному доході — за звітний рік [рядок 2250]</summary>
        [JsonProperty("D2_2250_01")]
        [JsonPropertyName("D2_2250_01")]
        public double? D22250_01 { get; set; }

        /// <summary>Дооцінка (уцінка) фінансових інструментів, що оцінюються через інший сукупний дохід (FVOCI) — за звітний рік [рядок 2255]</summary>
        [JsonProperty("D2_2255_01")]
        [JsonPropertyName("D2_2255_01")]
        public double? D22255_01 { get; set; }

        /// <summary>Накопичені курсові різниці від перерахунку закордонних господарських одиниць — за звітний рік [рядок 2270]</summary>
        [JsonProperty("D2_2270_01")]
        [JsonPropertyName("D2_2270_01")]
        public double? D22270_01 { get; set; }

        /// <summary>Зміна вартості фінансових інструментів хеджування (хеджування грошових потоків) — за звітний рік [рядок 2275]</summary>
        [JsonProperty("D2_2275_01")]
        [JsonPropertyName("D2_2275_01")]
        public double? D22275_01 { get; set; }

        /// <summary>Частка іншого сукупного доходу асоційованих та спільних підприємств — прибуток — за звітний рік [рядок 2280]</summary>
        [JsonProperty("D2_2280_01")]
        [JsonPropertyName("D2_2280_01")]
        public double? D22280_01 { get; set; }

        /// <summary>Частка іншого сукупного доходу асоційованих та спільних підприємств — збиток — за звітний рік [рядок 2280]</summary>
        [JsonProperty("D2_2280_02")]
        [JsonPropertyName("D2_2280_02")]
        public double? D22280_02 { get; set; }

        /// <summary>Інший сукупний дохід до оподаткування — загальна сума складових OCI — за звітний рік [рядок 2285]</summary>
        [JsonProperty("D2_2285_01")]
        [JsonPropertyName("D2_2285_01")]
        public double? D22285_01 { get; set; }

        /// <summary>Інший сукупний дохід після оподаткування — підсумок розділу II OCI — за звітний рік [рядок 2290]</summary>
        [JsonProperty("D2_2290_01")]
        [JsonPropertyName("D2_2290_01")]
        public double? D22290_01 { get; set; }

        /// <summary>Податок на прибуток, пов'язаний з іншим сукупним доходом — за звітний рік [рядок 2295]</summary>
        [JsonProperty("D2_2295_01")]
        [JsonPropertyName("D2_2295_01")]
        public double? D22295_01 { get; set; }

        /// <summary>Інший сукупний дохід після оподаткування — за звітний рік [рядок 2300]</summary>
        [JsonProperty("D2_2300_01")]
        [JsonPropertyName("D2_2300_01")]
        public double? D22300_01 { get; set; }

        /// <summary>Додаткова стаття іншого сукупного доходу або проміжний підсумок — за звітний рік [рядок 2305]</summary>
        [JsonProperty("D2_2305_01")]
        [JsonPropertyName("D2_2305_01")]
        public double? D22305_01 { get; set; }

        /// <summary>Додаткова стаття іншого сукупного доходу (специфічні галузеві форми) — за звітний рік [рядок 2310]</summary>
        [JsonProperty("D2_2310_01")]
        [JsonPropertyName("D2_2310_01")]
        public double? D22310_01 { get; set; }

        /// <summary>Сукупний дохід звітного року (чистий прибуток + інший сукупний дохід) — за звітний рік [рядок 2350]</summary>
        [JsonProperty("D2_2350_01")]
        [JsonPropertyName("D2_2350_01")]
        public double? D22350_01 { get; set; }

        /// <summary>Сукупний дохід, що відноситься до власників материнської компанії або некontrolюючих часток — за звітний рік [рядок 2355]</summary>
        [JsonProperty("D2_2355_01")]
        [JsonPropertyName("D2_2355_01")]
        public double? D22355_01 { get; set; }

        // ========== Розділ III. Елементи операційних витрат ==========

        /// <summary>Матеріальні витрати (сировина, матеріали, паливо, енергія) — за звітний рік [рядок 2400]</summary>
        [JsonProperty("D2_2400_01")]
        [JsonPropertyName("D2_2400_01")]
        public double? D22400_01 { get; set; }

        /// <summary>Витрати на оплату праці (нарахована зарплата всього персоналу) — за звітний рік [рядок 2405]</summary>
        [JsonProperty("D2_2405_01")]
        [JsonPropertyName("D2_2405_01")]
        public double? D22405_01 { get; set; }

        /// <summary>Відрахування на соціальні заходи (ЄСВ та інші обов'язкові нарахування на зарплату) — за звітний рік [рядок 2410]</summary>
        [JsonProperty("D2_2410_01")]
        [JsonPropertyName("D2_2410_01")]
        public double? D22410_01 { get; set; }

        /// <summary>Амортизація необоротних активів — за звітний рік [рядок 2415]</summary>
        [JsonProperty("D2_2415_01")]
        [JsonPropertyName("D2_2415_01")]
        public double? D22415_01 { get; set; }

        /// <summary>Інші операційні витрати (за економічним елементом) — за звітний рік [рядок 2445]</summary>
        [JsonProperty("D2_2445_01")]
        [JsonPropertyName("D2_2445_01")]
        public double? D22445_01 { get; set; }

        /// <summary>Разом операційні витрати за економічними елементами — за звітний рік [рядок 2450]</summary>
        [JsonProperty("D2_2450_01")]
        [JsonPropertyName("D2_2450_01")]
        public double? D22450_01 { get; set; }

        /// <summary>Витрати на оренду активів (операційна оренда та право користування) — за звітний рік [рядок 2455]</summary>
        [JsonProperty("D2_2455_01")]
        [JsonPropertyName("D2_2455_01")]
        public double? D22455_01 { get; set; }

        /// <summary>Витрати на інформаційні, консультаційні та аудиторські послуги — за звітний рік [рядок 2460]</summary>
        [JsonProperty("D2_2460_01")]
        [JsonPropertyName("D2_2460_01")]
        public double? D22460_01 { get; set; }

        /// <summary>Інші елементи операційних витрат (витрати на відрядження, охорону, зв'язок тощо) — за звітний рік [рядок 2465]</summary>
        [JsonProperty("D2_2465_01")]
        [JsonPropertyName("D2_2465_01")]
        public double? D22465_01 { get; set; }

        // ========== Розділ IV. Показники прибутковості акцій ==========

        /// <summary>Середньорічна кількість простих акцій в обігу (зважена середня) — за звітний рік [рядок 2500]</summary>
        [JsonProperty("D2_2500_01")]
        [JsonPropertyName("D2_2500_01")]
        public double? D22500_01 { get; set; }

        /// <summary>Скоригована середньорічна кількість простих акцій (з урахуванням потенційно розводнюючих інструментів) — за звітний рік [рядок 2505]</summary>
        [JsonProperty("D2_2505_01")]
        [JsonPropertyName("D2_2505_01")]
        public double? D22505_01 { get; set; }

        /// <summary>Чистий прибуток (збиток) на одну просту акцію — базовий EPS, грн — за звітний рік [рядок 2510]</summary>
        [JsonProperty("D2_2510_01")]
        [JsonPropertyName("D2_2510_01")]
        public double? D22510_01 { get; set; }

        /// <summary>Скоригований чистий прибуток (збиток) на одну просту акцію — розбавлений EPS, грн — за звітний рік [рядок 2515]</summary>
        [JsonProperty("D2_2515_01")]
        [JsonPropertyName("D2_2515_01")]
        public double? D22515_01 { get; set; }

        /// <summary>Дивіденди на одну просту акцію, грн — за звітний рік [рядок 2520]</summary>
        [JsonProperty("D2_2520_01")]
        [JsonPropertyName("D2_2520_01")]
        public double? D22520_01 { get; set; }

        /// <summary>Додатковий показник прибутковості акцій або підсумок розділу IV — за звітний рік [рядок 2550]</summary>
        [JsonProperty("D2_2550_01")]
        [JsonPropertyName("D2_2550_01")]
        public double? D22550_01 { get; set; }

        // =====================================================================
        // Службові поля
        // =====================================================================

        /// <summary>Схема (код форми) фінансової звітності: 1 — повна форма, 2 — спрощена (малі підприємства), 3 — мікропідприємства</summary>
        [JsonProperty("sCh")]
        [JsonPropertyName("sCh")]
        public int? SCh { get; set; }

        /// <summary>Звітний період у місяцях: 3 — квартал, 6 — півріччя, 9 — 9 місяців, 12 — рік</summary>
        [JsonProperty("periodMonth")]
        [JsonPropertyName("periodMonth")]
        public int? PeriodMonth { get; set; }
    }
}
