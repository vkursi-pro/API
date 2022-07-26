using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.bi
{
    class GetDataBiInfoClass
    {
        /*
         
        Отримати перелік компаний які відібрані в модулі BI
        [POST] /api/1.0/bi/GetDataBiInfo

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":100,"DateStart":"2019-11-28T19:00:52.059"}'
         
        */

        public static GetDataBiInfoRequestModel GetDataBiInfo(string labelId, int size, DateTime dateStart, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDataBiInfoRequestBodyModel GBDRequestBody = new GetDataBiInfoRequestBodyModel
                {
                    LabelId = labelId,                                              // Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
                    Size = size,                                                    // К-ть записів в відповіді. При кожному отриманні відповіді всі записи зберігаються з певним Pack id по значенню якого записи можуть бути отримані повторно.
                    DateStart = dateStart                                           // Дата з якої почнеться відбір (відсортованій в порядку зменшення) від якої буде братись Size
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);          // Example Body: {"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateStart":"2019-11-28T19:00:52.059"}

                // 

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo");
                // https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&DateStart=2019-11-28T19:00:52.059
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

            GetDataBiInfoRequestModel GetBiDataList = new GetDataBiInfoRequestModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetDataBiInfoRequestModel>(responseString);

            return GetBiDataList;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetDataBiInfoRequestBodyModel                                  // 
    {/// <summary>
     /// Назва збереженного списку
     /// </summary>
        public string LabelId { get; set; }                                     // 
        /// <summary>
        /// Розмір списку (від 1 до 10000)
        /// </summary>
        public int Size { get; set; }                                           // 
        /// <summary>
        /// Дата з якої почнеться відбір (відсортованій в порядку зменшення) від якої буде братись Size
        /// </summary>
        public DateTime? DateStart { get; set; }                                // 
    }

    /// <summary>
    /// Модель відповіді GetBiData
    /// </summary>
    class GetDataBiInfoRequestModel                                             // 
    {/// <summary>
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
        ///Перелік компаній 
        /// </summary>
        public List<GetDataCompanyModel> data { get; set; }                     // 
    }
    /// <summary>
    /// Перелік компаній
    /// </summary>
    public class GetDataCompanyModel                                            // 
    {/// <summary>
     /// Id компанії
     /// </summary>
        public string Id { get; set; }                                          // 
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
        /// Інозомні бенефіціари
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
        /// Кількість тендерів в яких приймав участь
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
        ///Забов'язання  
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
        public string Label { get; set; }                                     
    }
}
