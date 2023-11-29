using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

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
        /// Повне найменування підприємства
        /// </summary>
        public string NazvaPidpriyemstva { get; set; }                          // 
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string KodYedrpou { get; set; }                                  // 
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public string DataReyestratsiyi { get; set; }                           // 
        /// <summary>
        /// Статус юридичної особи
        /// </summary>
        public string StatusYuridichnoyiOsobi { get; set; }                     // 
        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        public string VidomostiProBankrutstvo { get; set; }                     // 
        /// <summary>
        /// Відомості про припинення або реорганізацію юридичної особи
        /// </summary>
        public string VidomostiProPripinennya { get; set; }                     // 
        /// <summary>
        /// Адреса реєстрації підприємства
        /// </summary>
        public string AdresaReyestratsiyi { get; set; }                         // 
        /// <summary>
        /// КВЕД (основний)
        /// </summary>
        public string KvedOsnovniy { get; set; }                                // 
        /// <summary>
        /// Статутний капітал
        /// </summary>
        public string StatutniyKapital { get; set; }                            // 
        /// <summary>
        /// Уповноважені особи
        /// </summary>
        public string UpovnovazheniOsobi { get; set; }                          // 
        /// <summary>
        /// Кількість засновників 
        /// </summary>
        public string KilkistZasnovnikiv { get; set; }                          // 
        /// <summary>
        /// Іноземні бенефіціари
        /// </summary>
        public string InozomniBenefitsiari { get; set; }                        // 
        /// <summary>
        /// Кількість Власників пакетів акцій 
        /// </summary>
        public string KilkistVlasnikivAktsiy { get; set; }                      // 
        /// <summary>
        /// Відкремлені підрозділи
        /// </summary>
        public string VidkremleniPidrozdili { get; set; }                       // 
        /// <summary>
        /// Реєстр платників ПДВ
        /// </summary>
        public string ReyestrPlatnikivPdv { get; set; }                         // 
        /// <summary>
        /// Анульована реєстрація платників ПДВ
        /// </summary>
        public string AnulovanoPdv { get; set; }                                // 
        /// <summary>
        /// Реєстр платників єдинго податку(дописать в еластик)
        /// </summary>
        public string ReyestrPlatnikivYep { get; set; }                         // 
        /// <summary>
        /// Санкційні списки
        /// </summary>
        public string SanktsiyniSpiski { get; set; }                            // 
        /// <summary>
        /// Виконавчі впровадження
        /// </summary>
        public string VikonavchiVprovadzhennya { get; set; }                    // 
        /// <summary>
        /// Судові рішення 
        /// </summary>
        public string SudoviRishennya { get; set; }                             // 
        /// <summary>
        /// Судові засідання (Спарави призначені до розгляду)
        /// </summary>
        public string SudoviZasidannya { get; set; }                            // 
        /// <summary>
        /// Включено в план-графік перевірок 2020
        /// </summary>
        public string PlangrafikPerevirok2020 { get; set; }                     // 
        /// <summary>
        /// Перевіряючий орган
        /// </summary>
        public string PereviryayuchiyOrgan { get; set; }                        // 
        /// <summary>
        /// Податковий борг
        /// </summary>
        public string PodatkoviyBorg { get; set; }                              // 
        /// <summary>
        /// Перед місцевим бюджетом
        /// </summary>
        public string PeredMistsevimByudzhetom { get; set; }                    // 
        /// <summary>
        /// Перед державним бюджетом
        /// </summary>
        public string PeredDerzhavnimByudzhetom { get; set; }                   // 
        /// <summary>
        /// За юридичним місцезнаходженням зареєстровано більше 1 суб’єкта господарювання
        /// </summary>
        public string KompaniyZaAdresoyu { get; set; }                          // 
        /// <summary>
        /// Зв'язки (керівник) Аналіз компаній, які можливо пов’язані з керівником (ПІБ керівника співпадає з ПІБ керівника в інших компаніях)
        /// </summary>
        public string ZvyazkyKerivnyka { get; set; }                            // 
        /// <summary>
        /// Зв'язки (засновники) Наявність можливих афілійованих зв’язків по засновникам та бенефіціарам
        /// </summary>
        public string ZvyazkyBenefitsiariv { get; set; }                        // 
        /// <summary>
        /// Зв'язки з ЄДРД (РЕР Декларанти та члени сім'ї)
        /// </summary>
        public string ZvyazkiZYedrd { get; set; }                               // 
        /// <summary>
        /// Тип/назва ліцензій/дозволів 
        /// </summary>
        public string TipnazvaLitsenziydozvoliv { get; set; }                   // 
        /// <summary>
        /// Кількість ліцензій/Дозволів
        /// </summary>
        public string KilkistLitsenziydozvoliv { get; set; }                    // 
        /// <summary>
        /// Заборгованість по ЗП
        /// </summary>
        public string ZaborgovanistPoZp { get; set; }                           // 
        /// <summary>
        /// Кількість об'єктів нерухомості 
        /// </summary>
        public string KilkistObyektivNerukhomosti { get; set; }                 // 
        /// <summary>
        /// Кількість земельних ділянок 
        /// </summary>
        public string KilkistZemelnikhDilyanok { get; set; }                    // 
        /// <summary>
        /// Площа землі
        /// </summary>
        public string PloshchaZemli { get; set; }                               // 
        /// <summary>
        /// Кількість транспортних засобів
        /// </summary>
        public string KilkistTransportnikhZasobiv { get; set; }                 // 
        /// <summary>
        /// Штатна чисельність працівників - 2018
        /// </summary>
        public string Pratsivnikiv2018 { get; set; }                            // 
        /// <summary>
        /// Сума експортних операцій - 2018
        /// </summary>
        public string SumaEksportny2018 { get; set; }                           // 
        /// <summary>
        /// Сума імпортних операцій - 2018
        /// </summary>
        public string SumaImporty2018 { get; set; }                             // 
        /// <summary>
        /// Кіькість тендерів в яких приймав участь
        /// </summary>
        public string KilkistTenderivPriymavUchast { get; set; }                // 
        /// <summary>
        /// Кількість виграних тендерів
        /// </summary>
        public string KilkistVigranikhTenderiv { get; set; }                    // 
        /// <summary>
        /// Сума виграних тендерів 
        /// </summary>
        public string SumaVigranikhTenderiv { get; set; }                       // 
        /// <summary>
        /// Клас боржника НБУ - 2018
        /// </summary>
        public string KlasBorzhnikaNbu2018 { get; set; }                        // 
        /// <summary>
        /// Фінансовий стан 
        /// </summary>
        public string FinansoviyStan { get; set; }                              // 
        /// <summary>
        /// Забов'язання 
        /// </summary>
        public string Zabovyazannya { get; set; }                               // 
        /// <summary>
        /// Міжнародна рейтингова оцінка 
        /// </summary>
        public string MizhnarodnaReytingovaOtsinka { get; set; }                // 
        /// <summary>
        /// Основні засоби 2018
        /// </summary>
        public string OsnovniZasobi2018 { get; set; }                           // 
        /// <summary>
        /// Поточні забов'язання 2018
        /// </summary>
        public string PotochniZabovyazannya2018 { get; set; }                   // 
        /// <summary>
        /// Чистий дохід (виручка) 2018
        /// </summary>
        public string ChistiyDokhidViruchka2018 { get; set; }                   // 
        /// <summary>
        /// Чистий прибуток/збиток 2018
        /// </summary>
        public string ChistiyPributokzbitok2018 { get; set; }                   // 
        /// <summary>
        /// Контактна інформація
        /// </summary>
        public string KontaktnaInformatsiya { get; set; }                       // 
        /// <summary>
        /// Назва збереженного списку
        /// </summary>
        public string Label { get; set; }                                       // 
    }
}
