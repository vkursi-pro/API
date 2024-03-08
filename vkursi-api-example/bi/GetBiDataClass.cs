using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;
using System.ComponentModel;

namespace vkursi_api_example.bi
{
    class GetBiDataClass
    {
        /*
         
        17. Отримати перелік компаний які відібрані в модулі BI
        [POST] /api/1.0/bi/getbidata

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/getbidata' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Label":null,"Size":10}'

        */

        public static GetBiDataResponseModel GetBiData(string label, int size, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetBiDataRequestBodyModel GBDRequestBody = new GetBiDataRequestBodyModel
                {
                    Label = label,                                              // Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
                    Size = size,                                                // К-ть записів в відповіді. При кожному отриманні відповіді всі записи зберігаються з певним Pack id по значенню якого записи можуть бути отримані повторно.
                    Pack = null                                                 // Отримати записи повторно по Pack id
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);      // Example Body: {"Label":null,"Size":10}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/getbidata");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetBiDataResponseModel GetBiDataList = new GetBiDataResponseModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetBiDataResponseModel>(responseString);

            return GetBiDataList;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Label\":null,\"Size\":10}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/bi/getbidata", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Label\":null,\"Size\":10}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/bi/getbidata")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

     */

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    class GetBiDataRequestBodyModel                                             // 
    {
        /// <summary>
        /// Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
        /// </summary>
        public string Label { get; set; }                                       // 
        /// <summary>
        /// Розмір списку (від 1 до 10000)
        /// </summary>
        public int Size { get; set; }                                           // 
        /// <summary>
        /// Для повторного отримання записів по Pack id
        /// </summary>
        public int? Pack { get; set; }                                          // 
    }
    /// <summary>
    /// Модель відповіді GetBiData
    /// </summary>
    class GetBiDataResponseModel                                                // 
    {
        /// <summary>
        /// Успішно виконано?
        /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// success, error, (Дані успішно знайдено. Pack: " + part)
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// 404, 200, ...
        /// </summary>
        public int code { get; set; }                                           // 
        /// <summary>
        /// Перелік компаній
        /// </summary>
        public List<ResponseModel> data { get; set; }                           // 
    }

    /// <summary>
    /// Перелік компаній
    /// </summary>
    public class ResponseModel                                                  // 
    {
        /// <summary>
        /// Унікальний ідентифікатор
        /// </summary>
        [DisplayName("Унікальний ідентифікатор")]
        public string Id { get; set; }
        /// <summary>
        /// Дата початку
        /// </summary>
        [DisplayName("Дата початку")]
        public string DateStart { get; set; }
        /// <summary>
        /// Назва мітки
        /// </summary>
        [DisplayName("Назва мітки")]
        public string Label { get; set; }

        /// <summary>
        /// Державний класифікатор об'єктів адміністративно-територіального устрою України
        /// </summary>
        [DisplayName("КОАТУУ")]
        public string Koatuu { get; set; }

        /// <summary>
        /// Повне найменування підприємства
        /// </summary>
        [DisplayName("Повне найменування підприємства")]
        public string NazvaPidpriyemstva { get; set; }

        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        [DisplayName("Код ЄДРПОУ")]
        public string KodYedrpou { get; set; }

        /// <summary>
        /// Дата реєстрації
        /// </summary>
        [DisplayName("Дата реєстрації")]
        public string DataReyestratsiyi { get; set; }
        /// <summary>
        /// Статус юридичної особи
        /// </summary>
        [DisplayName("Статус юридичної особи")]
        public string StatusYuridichnoyiOsobi { get; set; }

        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        [DisplayName("Відомості про банкрутство")]
        public string VidomostiProBankrutstvo { get; set; }

        /// <summary>
        /// Відомості про припинення або реорганізацію юридичної особи
        /// </summary>
        [DisplayName("Відомості про припинення або реорганізацію юридичної особи")]
        public string VidomostiProPripinennya { get; set; }

        /// <summary>
        /// Адреса реєстрації підприємства
        /// </summary>
        [DisplayName("Адреса реєстрації підприємства")]
        public string AdresaReyestratsiyi { get; set; }

        /// <summary>
        /// КВЕД (основний)
        /// </summary>
        [DisplayName("КВЕД (основний)")]
        public string KvedOsnovniy { get; set; }
        /// <summary>
        /// Статутний капітал
        /// </summary>
        [DisplayName("Статутний капітал")]
        public string StatutniyKapital { get; set; }
        /// <summary>
        /// Уповноважені особи
        /// </summary>
        [DisplayName("Уповноважені особи")]
        public string UpovnovazheniOsobi { get; set; }
        /// <summary>
        /// Кількість засновників
        /// </summary>
        [DisplayName("Кількість засновників")]
        public string KilkistZasnovnikiv { get; set; }
        /// <summary>
        /// Іноземні бенефіціари
        /// </summary>
        [DisplayName("Іноземні бенефіціари")]
        public string InozomniBenefitsiari { get; set; }
        /// <summary>
        /// Кількість власників пакетів акцій
        /// </summary>
        [DisplayName("Кількість власників пакетів акцій")]
        public string KilkistVlasnikivAktsiy { get; set; }
        /// <summary>
        /// Відокремлені підрозділи
        /// </summary>
        [DisplayName("Відокремлені підрозділи")]
        public string VidkremleniPidrozdili { get; set; }
        /// <summary>
        /// Реєстр платників ПДВ
        /// </summary>
        [DisplayName("Реєстр платників ПДВ")]
        public string ReyestrPlatnikivPdv { get; set; }

        /// <summary>
        /// Анульована реєстрація платників ПДВ
        /// </summary>
        [DisplayName("Анульовано ПДВ")]
        public string AnulovanoPdv { get; set; }
        /// <summary>
        /// Реєстр платників єдиного податку
        /// </summary>
        [DisplayName("Реєстр платників єдиного податку")]
        public string ReyestrPlatnikivYep { get; set; }
        /// <summary>
        /// Санкційні списки
        /// </summary>
        [DisplayName("Санкційні списки")]
        public string SanktsiyniSpiski { get; set; }
        /// <summary>
        /// Виконавчі впровадження
        /// </summary>
        [DisplayName("Виконавчі впровадження")]
        public string VikonavchiVprovadzhennya { get; set; }

        /// <summary>
        /// Судові рішення
        /// </summary>
        [DisplayName("Судові рішення")]
        public string SudoviRishennya { get; set; }

        /// <summary>
        /// Судові засідання (Справи призначені до розгляду)
        /// </summary>
        [DisplayName("Судові засідання")]
        public string SudoviZasidannya { get; set; }

        ///// <summary>
        ///// Включено в план-графік перевірок 2020
        ///// </summary>
        //[DisplayName("План-графік перевірок 2020")]
        //public string PlangrafikPerevirok2020 { get; set; }
        /// <summary>
        /// Перевіряючий орган
        /// </summary>
        [DisplayName("Перевіряючий орган")]
        public string PereviryayuchiyOrgan { get; set; }
        /// <summary>
        /// Податковий борг
        /// </summary>
        [DisplayName("Податковий борг")]
        public string PodatkoviyBorg { get; set; }
        /// <summary>
        /// Перед місцевим бюджетом
        /// </summary>
        [DisplayName("Перед місцевим бюджетом")]
        public string PeredMistsevimByudzhetom { get; set; }
        /// <summary>
        /// Перед державним бюджетом
        /// </summary>
        [DisplayName("Перед державним бюджетом")]
        public string PeredDerzhavnimByudzhetom { get; set; }
        /// <summary>
        /// За юридичним місцезнаходженням зареєстровано більше 1 суб'єкта господарювання
        /// </summary>
        [DisplayName("Компанії за адресою")]
        public string KompaniyZaAdresoyu { get; set; }

        /// <summary>
        /// Зв'язки (керівник) Аналіз компаній, які можливо пов'язані з керівником
        /// </summary>
        [DisplayName("Зв'язки керівника")]
        public string ZvyazkyKerivnyka { get; set; }

        /// <summary>
        /// Зв'язки (засновники) Наявність можливих афілійованих зв'язків по засновникам та бенефіціарам
        /// </summary>
        [DisplayName("Зв'язки бенефіціарів")]
        public string ZvyazkyBenefitsiariv { get; set; }

        /// <summary>
        /// Зв'язки з ЄДРД (РЕР Декларанти та члени сім'ї)
        /// </summary>
        [DisplayName("Зв'язки з ЄДРД")]
        public string ZvyazkiZYedrd { get; set; }

        /// <summary>
        /// Тип/назва ліцензій/дозволів
        /// </summary>
        [DisplayName("Тип/назва ліцензій/дозволів")]
        public string TipnazvaLitsenziydozvoliv { get; set; }

        /// <summary>
        /// Кількість ліцензій/Дозволів
        /// </summary>
        [DisplayName("Кількість ліцензій/дозволів")]
        public string KilkistLitsenziydozvoliv { get; set; }

        /// <summary>
        /// Заборгованість по ЗП
        /// </summary>
        [DisplayName("Заборгованість по ЗП")]
        public string ZaborgovanistPoZp { get; set; }

        /// <summary>
        /// Кількість об'єктів нерухомості
        /// </summary>
        [DisplayName("Кількість об'єктів нерухомості")]
        public string KilkistObyektivNerukhomosti { get; set; }

        /// <summary>
        /// Кількість земельних ділянок
        /// </summary>
        [DisplayName("Кількість земельних ділянок")]
        public string KilkistZemelnikhDilyanok { get; set; }

        /// <summary>
        /// Площа землі
        /// </summary>
        [DisplayName("Площа землі")]
        public string PloshchaZemli { get; set; }

        /// <summary>
        /// Кількість транспортних засобів
        /// </summary>
        [DisplayName("Кількість транспортних засобів")]
        public string KilkistTransportnikhZasobiv { get; set; }

        // <summary>
        /// Кількість тендерів в яких приймав участь
        /// </summary>
        [DisplayName("Кількість тендерів в яких приймав участь")]
        public string? KilkistTenderivPriymavUchast { get; set; }

        /// <summary>
        /// Кількість виграних тендерів
        /// </summary>
        [DisplayName("Кількість виграних тендерів")]
        public string KilkistVigranikhTenderiv { get; set; }

        /// <summary>
        /// Сума виграних тендерів
        /// </summary>
        [DisplayName("Сума виграних тендерів")]
        public string? SumaVigranikhTenderiv { get; set; }

        /// <summary>
        /// Фінансовий стан
        /// </summary>
        [DisplayName("Фінансовий стан")]
        public string? FinansoviyStan { get; set; }

        /// <summary>
        /// Забов'язання
        /// </summary>
        [DisplayName("Забов'язання")]
        public string? Zabovyazannya { get; set; }

        /// <summary>
        /// Міжнародна рейтингова оцінка
        /// </summary>
        [DisplayName("Міжнародна рейтингова оцінка")]
        public string MizhnarodnaReytingovaOtsinka { get; set; }

        /// <summary>
        /// Основні показники в розрізі років
        /// </summary>
        [DisplayName("Основні показники в розрізі років")]
        public List<BiNewExcelMainPerYearModel>? BiNewExcelMainPerYear { get; set; }

        /// <summary>
        /// Контактна інформація
        /// </summary>
        [DisplayName("Контактна інформація")]
        public string KontaktnaInformatsiya { get; set; }

        /// <summary>
        /// Реєстраційний номер
        /// </summary>
        [DisplayName("Реєстраційний номер")]
        public string RegNumb { get; set; }
    }


    /// <summary>
    /// Основні показники в розрізі років
    /// </summary>
    public class BiNewExcelMainPerYearModel : ImportMainBalanceIndicatorsModel
    {
        /// <summary>
        /// Основні засоби
        /// </summary>
        [DisplayName("Основні засоби")]
        public double? OsnovniZasobi { get; set; }
        /// <summary>
        /// Поточні забов'язання
        /// </summary>
        [DisplayName("Поточні забов'язання")]
        public double? PotochniZabovyazannya { get; set; }
        /// <summary>
        /// Чистий дохід (виручка)
        /// </summary>
        [DisplayName("Чистий дохід (виручка)")]
        public double? ChistiyDokhidViruchka { get; set; }
        /// <summary>
        /// Чистий прибуток/збиток
        /// </summary>
        [DisplayName("Чистий прибуток/збиток")]
        public double? ChistiyPributokzbitok { get; set; }
        /// <summary>
        /// Сума експортних операцій
        /// </summary>
        [DisplayName("Сума експортних операцій")]
        public double? SumaEksportny { get; set; }
        /// <summary>
        /// Сума імпортних операцій
        /// </summary>
        [DisplayName("Сума імпортних операцій")]
        public double? SumaImporty { get; set; }
        /// <summary>
        /// Штатна чисельність працівників
        /// </summary>
        [DisplayName("Штатна чисельність працівників")]
        public int? Pratsivnikiv { get; set; }
        /// <summary>
        /// Включено в план-графік перевірок
        /// </summary>
        [DisplayName("Включено в план-графік перевірок")]
        public int? PlangrafikPerevirok { get; set; }
        /// <summary>
        /// Перевіряючий орган
        /// </summary>
        [DisplayName("Перевіряючий орган")]
        public string? PereviryayuchiyOrgan { get; set; }
    }

    /// <summary>
    /// Основні фінансові показники по роках
    /// </summary>
    public class ImportMainBalanceIndicatorsModel
    {
        /// <summary>
        /// Рік
        /// </summary>
        [DisplayName("Рік")]
        public int period { get; set; }

        /// <summary>
        /// Необоротні активи (Усього)
        /// </summary>
        [DisplayName("Необоротні активи (Усього)")]
        public double? d108002UsohozarozdilomID1109502 { get; set; }

        /// <summary>
        /// Оборотні активи (Усього)
        /// </summary>
        [DisplayName("Оборотні активи (Усього)")]
        public double? d126002UsohozarozdilomIID1119502 { get; set; }

        /// <summary>
        /// Необоротні активи, утримувані для продажу, та групи вибуття
        /// </summary>
        [DisplayName("Необоротні активи, утримувані для продажу, та групи вибуття")]
        public double? d124002IIINeoborotniaktyvy { get; set; }

        /// <summary>
        /// Баланс активу
        /// </summary>
        [DisplayName("Баланс активу")]
        public double? d128002BalansaktivuD1130002 { get; set; }

        /// <summary>
        /// Власний капітал (Усього)
        /// </summary>
        [DisplayName("Власний капітал (Усього)")]
        public double? d138002UsohozarozdilomID1149502 { get; set; }

        /// <summary>
        /// Довгострокові зобов'язання і забезпечення (Усього)
        /// </summary>
        [DisplayName("Довгострокові зобов'язання і забезпечення (Усього)")]
        public double? d148002UsohozarozdilomIID1159502 { get; set; }

        /// <summary>
        /// Поточні зобов'язання і забезпечення (Усього)
        /// </summary>
        [DisplayName("Поточні зобов'язання і забезпечення (Усього)")]
        public double? d162002UsohozarozdilomIIID1169502 { get; set; }

        /// <summary>
        /// Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття
        /// </summary>
        [DisplayName("Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття")]
        public double? d1170002D141802IVZobovyazanny { get; set; }

        /// <summary>
        /// Чистий дохід від реалізації продукції
        /// </summary>
        [DisplayName("Чистий дохід від реалізації продукції")]
        public double? d203501Chystyydokhid { get; set; }

        /// <summary>
        /// Фінансовий результат від операційної діяльності (Прибуток)
        /// </summary>
        [DisplayName("Фінансовий результат від операційної діяльності (Прибуток)")]
        public double? d210001Finansovyyrezul { get; set; }

        /// <summary>
        /// Фінансовий результат від операційної діяльності (Збиток)
        /// </summary>
        [DisplayName("Фінансовий результат від операційної діяльності (Збиток)")]
        public double? d210501FinRez { get; set; }

        /// <summary>
        /// Фінансовий результат до оподаткування (Прибуток)
        /// </summary>
        [DisplayName("Фінансовий результат до оподаткування (Прибуток)")]
        public double? d217001FinRezdoopodatkuvannyaprybutok { get; set; }

        /// <summary>
        /// Фінансовий результат до оподаткування (Збиток)
        /// </summary>
        [DisplayName("Фінансовий результат до оподаткування (Збиток)")]
        public double? d217501FinRezdoopodatkuvannyazbytok { get; set; }

        /// <summary>
        /// Прибуток
        /// </summary>
        [DisplayName("Прибуток")]
        public double? d222001ChystyyFinRezprybutokD2235001 { get; set; }

        /// <summary>
        /// Збиток
        /// </summary>
        [DisplayName("Збиток")]
        public double? d222501ChystyyFinRezzbytokD2235501 { get; set; }

        /// <summary>
        /// Разом доходи (малі підприємства)
        /// </summary>
        [DisplayName("Разом доходи (малі підприємства)")]
        public double? d207001RazomdokhodyD2228001 { get; set; }

        /// <summary>
        /// Разом витрати (малі підприємства)
        /// </summary>
        [DisplayName("Разом витрати (малі підприємства)")]
        public double? d212001RazomvytratyD2228501 { get; set; }
        /// <summary>
        /// Форма
        /// </summary>
        [DisplayName("Форма")]
        public int? forma { get; set; }
    }
}
